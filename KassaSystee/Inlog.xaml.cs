using CefSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
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

namespace KassaSystee
{
    /// <summary>
    /// Interaction logic for Inlog.xaml
    /// </summary>
    public sealed partial class Inlog : Window
    {


        loadScreen ls;
        public static int countForProgressbarr = 0;

        public static WebBrowser webbrowser;
        public static Double ExpiredAt = DateTime.UtcNow.Ticks / 10000000;
         const string CLIENT_ID = "929f51e9-879c-4a84-b21f-773fa74a6531";//const may not be modifyed
        const string CLIENT_SECRET = "ykYJ2PYnrjE2";
        const string CLIENT_STATE = "1wzxvfh2naf3vvkj2lprxyda";
        const string CALLBACK_URL = "http://localhost/exact%20GLAccount%20Sample%20php/callback.php";
        const string BASE_URI = "https://start.exactonline.nl";

        public static String Code = null, State = null, AccessToken = null, RefreshToken = null, CurrentDivision = null, FullName = null;
       

        public static List<Items> Result = new List<Items>();//haal alles op
        public static List<Items> Planten = new List<Items>();
        public static List<Items> Mestgrond = new List<Items>();
        public static List<Items> Gereedschap = new List<Items>();
        public static List<Items> opId = new List<Items>();

        private MainWindow mw;
        
        public Inlog(MainWindow mw)
        {
            this.mw = mw;
            
            webbrowser = browser;
            this.InitializeComponent();
           // window = pagina;
            // getAuthorization();
            
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            getinlog();
        }

        public async void getinlog()
        {
            if (Inlog.CheckForInternetConnection() == true)
            {
                string request = BASE_URI + "/api/oauth2/auth" +
                            "?client_id={" + CLIENT_ID + "}" +
                            "&redirect_uri=" + CALLBACK_URL +
                            "&state=" + CLIENT_STATE +
                            "&response_type=code&force_login=0";
                browser.Navigate(new Uri(request));//inlogscherm komt op
            }
            else
            {
                await errorAlert("Geen internetverbinding gedetecteerd, probeer het opnieuw");
            }

        }
        private void browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            HideScriptErrors();
            string url = e.Uri.ToString();
            if (url.IndexOf(BASE_URI) < 0)
            {
                int c = url.IndexOf("?code=");
                int s = url.IndexOf("&state=");
                Inlog.Code = url.Substring(c + 6, s - c - 6);
                Inlog.State = url.Substring(s + 7);
                //  ensureAccess();//gives error
                
                ls = new loadScreen(mw);
                ls.Show();
               // MainWindow.Show();
                this.Close();
            }
        }

        public void HideScriptErrors()  // zucht, zie  https://social.msdn.microsoft.com/Forums/vstudio/en-US/4f686de1-8884-4a8d-8ec5-ae4eff8ce6db/new-wpf-webbrowser-how-do-i-suppress-script-errors?forum=wpf
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            object objComWebBrowser = fiComWebBrowser.GetValue(browser);
            if (objComWebBrowser == null) return;
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { true });
        }
        // private Inlog()
        // {

        //}





        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        
        public async Task errorAlert(string s)
        {
            
            MessageBox.Show("" + s);

            //(s).ShowAsync();

            Application.Current.Shutdown();
        }

       

        

        //try
        //{


        //WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, requestUri, callbackUri);

        //switch (result.ResponseStatus)
        //{
        //    case WebAuthenticationStatus.Success:                               // Successful authentication. 
        //        String response = result.ResponseData.ToString();
        //        int c = response.IndexOf("?code=");
        //        int s = response.IndexOf("&state=");
        //        Code = response.Substring(c + 6, s - c - 6);
        //        State = response.Substring(s + 7);                              //notify("Gelukt\n\nRespons: " + response + "\n\nCode: " + Code + "\n\nState: " + State);
        //        break;
        //    case WebAuthenticationStatus.ErrorHttp:                             // HTTP error. 
        //        await errorAlert("getAuthorization Mislukt " + result.ResponseErrorDetail.ToString());
        //        break;
        //    default:                                                            // Other error.
        //        await errorAlert("getAuthorization Mislukt " + result.ResponseData.ToString());
        //        break;
        //    }
        //}
        //catch (Exception ex)
        //{
        //    MessageBox.Show("Error launching WebAuth " + ex.Message + "\r\n");
        //}
        
    

        //============ refreshToken ===============================

       

        //============ getMe ===============================

       
        

       

        //============ ensureAccess ===============================

       


        //============ getLogisticks/salesitemprices================

       
        //public async Task getSalesItemPrices()//ophalen gegevens en in list stoppen
        //{
        //   // await getCount();
        //    Result.Clear();
        //    opId.Clear();
        //    // string filter = "&$filter=trim(Code) eq '10001'&$limit=10";
        //    string offset = "0";
        //    string orderby = "&$orderby=Code+asc";
        //    while (offset != "")
        //    {
        //        await ensureAccess();
        //       // string filter = "&$filter=substringof('" + f + "',Description) eq true";
        //        string skiptoken = "&$skiptoken = '" + offset + "' ,guid'dd5f25dd-9572-489f-9b1c-2cb8ef382851'";
        //        Uri request = new Uri(BASE_URI + "/api/v1/" + CurrentDivision + "/logistics/Items?access_token=" + AccessToken + orderby + skiptoken + "&$inlinecount = allpages");
               
        //        HttpClient client = new HttpClient();
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        //        HttpResponseMessage respons = await client.GetAsync(request);
        //        if (respons.IsSuccessStatusCode == false)
        //        {
        //            await errorAlert("getSalesItemPrices Mislukt:  status = " + respons.StatusCode.ToString());
        //            return;
        //        }
        //        respons.EnsureSuccessStatusCode();
        //        string responsecontent = await respons.Content.ReadAsStringAsync();
        //        JArray results = JObject.Parse(responsecontent).GetValue("d")["results"].ToObject<JArray>();

        //        countForProgressbarr = int.Parse(JObject.Parse(responsecontent).GetValue("d")["__count"].ToString());
                
               

        //        if (JObject.Parse(responsecontent).GetValue("d")["__next"] != null)
        //        {
        //            offset =  (Convert.ToInt32(JObject.Parse(responsecontent).GetValue("d")["__next"].ToString().Substring(117, 5).ToString())+1).ToString();
        //        }
        //        else
        //        {
        //            offset = "";
        //        }
        //        foreach (JObject item in results)
        //        {
        //            string sCategorie = (string)item["ItemGroupDescription"];
        //            double dCost = ((double)item["CostPriceStandard"]) * 2.05;
        //            string sCode = (string)item["Code"];
        //            string sDescription = (string)item["Description"];
        //            Result.Add(new Items(sCode, sDescription, dCost, sCategorie));
        //            opId.Add(new Items(sCode, sDescription, dCost, sCategorie));
        //            if (sCategorie == "MG")
        //            {
        //                Mestgrond.Add(new Items(sCode, sDescription, dCost, sCategorie));
        //            }
        //            if(sCategorie == "HW")
        //            {
        //                Gereedschap.Add(new Items(sCode, sDescription, dCost, sCategorie));
        //            }
        //            if (sCategorie == "KB" || sCategorie == "Planten en Bloemen")
        //            {
        //                Planten.Add(new Items(sCode, sDescription, dCost, sCategorie));
        //            }
        //        }
        //    }
          
           
        // //  MessageBox.Show("Done!");
        // }

        //============ updateGLAccount ===============================

        public async Task updateItems(Items it)
        {
            //await ensureAccess();
            //Uri request = new Uri(BASE_URI + "/api/v1/" + CurrentDivision + "/financial/GLAccounts(guid\'" + it.Code + "\')?access_token=" + AccessToken);
            //HttpClient client = new HttpClient();

            //JsonObject jsonObject = new JsonObject();
            //jsonObject["Description"] = JsonValue.CreateStringValue(it.Description);
            //string bodystring = jsonObject.Stringify();
            //StringContent content = new StringContent(bodystring, System.Text.Encoding.UTF8, "application/json");

            //HttpResponseMessage respons = await client.PutAsync(request, content);

            //if (respons.IsSuccessStatusCode == false)                               //            notify(respons.StatusCode.ToString());
            //{
            //    await errorAlert("updateGLAccount Mislukt:  status = " + respons.StatusCode.ToString());
            //    return;
            //}
            //respons.EnsureSuccessStatusCode();
        }

        //============ insertGLAccount ===============================

        public async Task insertItems(Items it)
        {
            //await ensureAccess();
            //Uri request = new Uri(BASE_URI + "/api/v1/" + CurrentDivision + "/financial/GLAccounts?access_token=" + AccessToken);
            //HttpClient client = new HttpClient();

            //JsonObject jsonObject = new JsonObject();
            //jsonObject["Code"] = JsonValue.CreateStringValue(it.Code);
            //jsonObject["Description"] = JsonValue.CreateStringValue(it.Description);
            //string bodystring = jsonObject.Stringify();
            //StringContent content = new StringContent(bodystring, System.Text.Encoding.UTF8, "application/json");

            //HttpResponseMessage respons = await client.PostAsync(request, content);

            //if (respons.IsSuccessStatusCode == false)
            //{
            //    await errorAlert("insertGLAccount Mislukt:  status = " + respons.StatusCode.ToString());
            //    return;
            //}
            //respons.EnsureSuccessStatusCode();
        }

        //============ updateGLAccount ===============================

        //public async Task deleteItems(Items it)
        //{
        //    await ensureAccess();
        //    Uri request = new Uri(BASE_URI + "/api/v1/" + CurrentDivision + "/financial/GLAccounts(guid\'" + it.Code + "\')?access_token=" + AccessToken);
        //    HttpClient client = new HttpClient();

        //    HttpResponseMessage respons = await client.DeleteAsync(request);

        //    if (respons.IsSuccessStatusCode == false)
        //    {
        //        await errorAlert("updateGLAccount Mislukt:  status = " + respons.StatusCode.ToString());
        //        return;
        //    }
        //    respons.EnsureSuccessStatusCode();
        //}

    }

}


