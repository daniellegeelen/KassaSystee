using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace KassaSystee
{
    /// <summary>
    /// Interaction logic for loadScreen.xaml
    /// </summary>
    public partial class loadScreen : Window
    {
        const string CLIENT_ID = "929f51e9-879c-4a84-b21f-773fa74a6531";//const may not be modifyed
        const string CLIENT_SECRET = "ykYJ2PYnrjE2";
        const string CLIENT_STATE = "1wzxvfh2naf3vvkj2lprxyda";
        const string CALLBACK_URL = "http://localhost/exact%20GLAccount%20Sample%20php/callback.php";
        const string BASE_URI = "https://start.exactonline.nl";

        
        public static MainWindow mainwindow;
        public static loadScreen Loadscreen;
        public static bool goToLogin = true;
        int countForProgressbar = 0;
       // private retourneren rt;
        private MainWindow mw;

        // public static loadScreen ls;
        public loadScreen(MainWindow mw)
        {
            this.mw = mw;
            goToLogin = false;
            Loadscreen = this;
          
            load(mw);
            InitializeComponent();
        }
        public async void load(MainWindow mw)
        {
            this.mw = mw;
            //mw.Show();
            await getSalesItemPrices();
            
            mw.Show();
            this.Close();

        }
        public async Task ensureAccess()
        {
            if (Inlog.Code == null) await getAuthorization();
            if (Inlog.AccessToken == null) await getToken();
            if (Inlog.CurrentDivision == null) await getMe();
            if ((DateTime.UtcNow.Ticks / 10000000) > Inlog.ExpiredAt) await refreshToken();
        }
        public async Task getAuthorization()
        {
            string request = BASE_URI + "/api/oauth2/auth" +
                            "?client_id={" + CLIENT_ID + "}" +
                            "&redirect_uri=" + CALLBACK_URL +
                            "&state=" + CLIENT_STATE +
                            "&response_type=code&force_login=0";
            //&force_login=0"

            // System.Uri requestUri = new Uri(request);
            //System.Uri callbackUri = new Uri(CALLBACK_URL);
            //this.Show();

            Inlog.webbrowser.Navigate(new Uri(request));

        }
       
        

        public async Task getToken()
        {
            Uri request = new Uri(BASE_URI + "/api/oauth2/token");

            HttpClient client = new HttpClient();

            String poststring = "grant_type=authorization_code" +
                                 "&code=" + Inlog.Code +
                                 "&redirect_uri=" + CALLBACK_URL +
                                 "&client_id={" + CLIENT_ID + "}" +
                                 "&client_secret=" + CLIENT_SECRET +
                                 "&force_login=0";

            StringContent content = new StringContent(poststring, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage respons = await client.PostAsync(request, content);

            //  String poststring = "grant_type=authorization_code" +
            //"&code=" + Code +
            //"&redirect_uri=" + CALLBACK_URL +
            //"&client_id={" + CLIENT_ID + "}" +
            //"&client_secret=" + CLIENT_SECRET +
            //"&force_login=0";

            //StringContent content = new StringContent(poststring, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            //HttpResponseMessage respons = await client.PostAsync(request, content);

            if (respons.IsSuccessStatusCode == false)
            {
                await errorAlert("getToken Mislukt:  status = " + respons.StatusCode.ToString());
                return;
            }
            respons.EnsureSuccessStatusCode();
            string responsecontent = await respons.Content.ReadAsStringAsync();


            //dynamic jo = JsonConvert.DeserializeObject(responsecontent);


            JObject jo = JObject.Parse(responsecontent);

            Inlog.AccessToken = jo.GetValue("access_token").ToObject<string>();
            Inlog.RefreshToken = jo.GetValue("refresh_token").ToObject<string>();
            Inlog.ExpiredAt = System.Convert.ToDouble(jo.GetValue("expires_in")) + DateTime.UtcNow.Ticks / 10000000;

            //notify("AccessToken: " + AccessToken + "\n\nRefreshToken: " + RefreshToken + "\n\nExpiredAt: " + ExpiredAt.ToString());
        }
        public async Task errorAlert(string s)
        {

            MessageBox.Show("" + s);

            //(s).ShowAsync();

            Application.Current.Shutdown();
        }
        public async Task getMe()
        {
            if ((DateTime.UtcNow.Ticks / 10000000) > Inlog.ExpiredAt) await refreshToken();

            Uri request = new Uri(BASE_URI + "/api/v1/current/Me?access_token=" +Inlog.AccessToken);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            // client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            HttpResponseMessage respons = await client.GetAsync(request);
            if (respons.IsSuccessStatusCode == false)
            {
                await errorAlert("getMe Mislukt:  status = " + respons.StatusCode.ToString());
                return;
            }
            respons.EnsureSuccessStatusCode();
            string responsecontent = await respons.Content.ReadAsStringAsync();

            //	dataarray['d']['results'][0]['CurrentDivision'];

            JObject jo = JObject.Parse(responsecontent).GetValue("d")["results"].First.ToObject<JObject>();

            //JObject jo = JsonConvert.DeserializeObject(responsecontent);
            //  job jo = jo1.GetNamedObject("d").GetNamedArray("results").GetObjectAt(0);

            Inlog.CurrentDivision = jo.GetValue("CurrentDivision").ToString();
            Inlog.FullName = jo.GetValue("FullName").ToString();

            //notify("CurrentDivision: " + CurrentDivision + "\n\nFullName: " + FullName + "\n\nExpiredAt: " + ExpiredAt.ToString());
        }
        public async Task refreshToken()
        {
            Uri request = new Uri(BASE_URI + "/api/oauth2/token");

            HttpClient client = new HttpClient();

            String poststring = "grant_type=refresh_token" +
                                "&client_id={" + CLIENT_ID + "}" +
                                "&client_secret=" + CLIENT_SECRET;

            StringContent content = new StringContent(poststring, System.Text.Encoding.UTF8, "Content-Type: application/x-www-form-urlencoded");
            HttpResponseMessage respons = await client.PostAsync(request, content);

            if (respons.IsSuccessStatusCode == false)
            {
                await errorAlert("refreshToken Mislukt:  status = " + respons.StatusCode.ToString());
                return;
            }
            respons.EnsureSuccessStatusCode();
            string responsecontent = await respons.Content.ReadAsStringAsync();

            JObject jo = JObject.Parse(responsecontent);
            //  dynamic jo = JsonConvert.DeserializeObject(responsecontent);

            Inlog.AccessToken = jo.GetValue("access_token").ToObject<string>();
            Inlog.RefreshToken = jo.GetValue("refresh_token").ToObject<string>();
            Inlog.ExpiredAt = System.Convert.ToDouble(jo.GetValue("expires_in")) + DateTime.UtcNow.Ticks / 10000000;

            //notify("AccessToken: " + AccessToken + "\n\nRefreshToken: " + RefreshToken + "\n\nExpiredAt: " + ExpiredAt.ToString());
        }

        public async Task getCount()
        {
            await ensureAccess();
            Uri count = new Uri(BASE_URI + "/api/v1/" + Inlog.CurrentDivision + "/logistics/Items?access_token=" + Inlog.AccessToken + "&$inlinecount = allpages");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            HttpResponseMessage respons = await client.GetAsync(count);
            if (respons.IsSuccessStatusCode == false)
            {
                await errorAlert("getCount Mislukt:  status = " + respons.StatusCode.ToString());
            }
            respons.EnsureSuccessStatusCode();
            string responsecontent1 = await respons.Content.ReadAsStringAsync();
            countForProgressbar = int.Parse(JObject.Parse(responsecontent1).GetValue("d")["__count"].ToString());
            //countForProgressbar = 1;
        }
        public async Task getSalesItemPrices()//ophalen gegevens en in list stoppen
        {
            await getCount();
            int aantalItemPerProcent = countForProgressbar/100;
            int i = 0;
            int y = 0;
            tbPercentage.Text = "0 %";
            // await getCount();
            Inlog.Result.Clear();
            Inlog.opId.Clear();
            // string filter = "&$filter=trim(Code) eq '10001'&$limit=10";
            string offset = "0";
            string orderby = "&$orderby=Code+asc";
            while (offset != "")
            {
                await ensureAccess();
                // string filter = "&$filter=substringof('" + f + "',Description) eq true";
                string skiptoken = "&$skiptoken = '" + offset + "' ,guid'dd5f25dd-9572-489f-9b1c-2cb8ef382851'";
                Uri request = new Uri(BASE_URI + "/api/v1/" + Inlog.CurrentDivision + "/logistics/Items?access_token=" + Inlog.AccessToken + orderby + skiptoken + "&$inlinecount = allpages");

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                HttpResponseMessage respons = await client.GetAsync(request);
                if (respons.IsSuccessStatusCode == false)
                {
                    await errorAlert("getSalesItemPrices Mislukt:  status = " + respons.StatusCode.ToString());
                    return;
                }
                respons.EnsureSuccessStatusCode();
                string responsecontent = await respons.Content.ReadAsStringAsync();
                JArray results = JObject.Parse(responsecontent).GetValue("d")["results"].ToObject<JArray>();

               // countForProgressbar = int.Parse(JObject.Parse(responsecontent).GetValue("d")["__count"].ToString());



                if (JObject.Parse(responsecontent).GetValue("d")["__next"] != null)
                {
                    offset = (Convert.ToInt32(JObject.Parse(responsecontent).GetValue("d")["__next"].ToString().Substring(117, 5).ToString()) + 1).ToString();
                }
                else
                {
                    offset = "";
                }
                foreach (JObject item in results)
                {
                    i++;
                    if(i % 3 == 0)
                    {
                        tbloading.Text = "Gegevens worden opgehaald...";
                    }
                    else if (i % 2 == 0)
                    {
                        tbloading.Text = "Gegevens worden opgehaald..";
                    }
                    else if( i % 3 == 0)
                    {
                        tbloading.Text = "Gegevens worden opgehaald.";
                    }
                    if(i % (aantalItemPerProcent) == 0)
                    {
                        y++;
                        pbLoading.Value = y;
                        tbPercentage.Text = y.ToString() + " %"; 
                    }
                    string sCategorie = (string)item["ItemGroupDescription"];
                    double dCost = ((double)item["CostPriceStandard"]) * 2.05;
                    string sCode = (string)item["Code"];
                    string sDescription = (string)item["Description"];
                    Inlog.Result.Add(new Items(sCode, sDescription, dCost, sCategorie));
                    Inlog.opId.Add(new Items(sCode, sDescription, dCost, sCategorie));
                    if (sCategorie == "MG")
                    {
                        Inlog.Mestgrond.Add(new Items(sCode, sDescription, dCost, sCategorie));
                    }
                    if (sCategorie == "HW")
                    {
                        Inlog.Gereedschap.Add(new Items(sCode, sDescription, dCost, sCategorie));
                    }
                    if (sCategorie == "KB" || sCategorie == "Planten en Bloemen")
                    {
                        Inlog.Planten.Add(new Items(sCode, sDescription, dCost, sCategorie));
                    }
                }                
            }
        }
    }
}
