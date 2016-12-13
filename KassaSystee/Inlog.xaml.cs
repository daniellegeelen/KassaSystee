using CefSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
        MainWindow window;
        public Inlog(MainWindow pagina)
        {

            this.InitializeComponent();
            window = pagina;
           // getAuthorization();
            getinlog();
        }
       // private Inlog()
       // {
          
        //}

            
        
        const string CLIENT_ID = "929f51e9-879c-4a84-b21f-773fa74a6531";//const may not be modifyed
        const string CLIENT_SECRET = "ykYJ2PYnrjE2";
        const string CLIENT_STATE = "1wzxvfh2naf3vvkj2lprxyda";
        const string CALLBACK_URL = "http://localhost/exact%20GLAccount%20Sample%20php/callback.php";
        const string BASE_URI = "https://start.exactonline.nl";

        private String Code = null, State = null, AccessToken = null, RefreshToken = null, CurrentDivision = null, FullName = null;
        private Double ExpiredAt = DateTime.UtcNow.Ticks / 10000000;

        public List<Items> Result = new List<Items>();
        public void getinlog()
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

            browser.Navigate(new Uri(request));
        }
        public async Task errorAlert(string s)
        {
            
            MessageBox.Show("" + s);

            //(s).ShowAsync();

            Application.Current.Shutdown();
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
            
            browser.Navigate(new Uri(request));

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
        public async Task getToken()
        {
            Uri request = new Uri(BASE_URI + "/api/oauth2/token");

            HttpClient client = new HttpClient();

            String poststring = "grant_type=authorization_code" +
                                 "&code=" + Code +
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

            AccessToken = jo.GetValue("access_token").ToObject<string>();
            RefreshToken = jo.GetValue("refresh_token").ToObject<string>();
            ExpiredAt = System.Convert.ToDouble(jo.GetValue("expires_in")) + DateTime.UtcNow.Ticks / 10000000;

            //notify("AccessToken: " + AccessToken + "\n\nRefreshToken: " + RefreshToken + "\n\nExpiredAt: " + ExpiredAt.ToString());
        }
    

        //============ refreshToken ===============================

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

            AccessToken = jo.GetValue("access_token").ToObject<string>();
            RefreshToken = jo.GetValue("refresh_token").ToObject<string>();
            ExpiredAt = System.Convert.ToDouble(jo.GetValue("expires_in")) + DateTime.UtcNow.Ticks / 10000000;

            //notify("AccessToken: " + AccessToken + "\n\nRefreshToken: " + RefreshToken + "\n\nExpiredAt: " + ExpiredAt.ToString());
        }

        //============ getMe ===============================

        public async Task getMe()
        {
            if ((DateTime.UtcNow.Ticks / 10000000) > ExpiredAt) await refreshToken();

            Uri request = new Uri(BASE_URI + "/api/v1/current/Me?access_token=" + AccessToken);

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

            CurrentDivision = jo.GetValue("CurrentDivision").ToString();
            FullName = jo.GetValue("FullName").ToString();

            //notify("CurrentDivision: " + CurrentDivision + "\n\nFullName: " + FullName + "\n\nExpiredAt: " + ExpiredAt.ToString());
        }
        public void HideScriptErrors()  // zucht, zie  https://social.msdn.microsoft.com/Forums/vstudio/en-US/4f686de1-8884-4a8d-8ec5-ae4eff8ce6db/new-wpf-webbrowser-how-do-i-suppress-script-errors?forum=wpf
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            object objComWebBrowser = fiComWebBrowser.GetValue(browser);
            if (objComWebBrowser == null) return;
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { true });
        }

        private void browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            HideScriptErrors();
           // webBrowser.RenderSize = new Size(600, 500);
            string url = e.Uri.ToString();
            if (url.IndexOf(BASE_URI) < 0)
            {
                int c = url.IndexOf("?code=");
                int s = url.IndexOf("&state=");
                this.Code = url.Substring(c + 6, s - c - 6);
                this.State = url.Substring(s + 7);
                //  ensureAccess();//gives error
                window.Show();
                this.Close();
            }
        }

        //============ ensureAccess ===============================

        public async Task ensureAccess()
        {
            if (Code == null) await getAuthorization();
            if (AccessToken == null) await getToken();
            if (CurrentDivision == null) await getMe();
            if ((DateTime.UtcNow.Ticks / 10000000) > ExpiredAt) await refreshToken();
        }


        //============ getLogisticks/salesitemprices================
        public async Task getSalesItemPrices()//ophalen gegevens en in list stoppen (dit nog om de zoveel tijd doen)
        {
           Result.Clear();
            // string filter = "&$filter=trim(Code) eq '10001'&$limit=10";
            string offset = "0";
            string orderby = "&$orderby=Code+asc";
            while (offset != "")
            {
                await ensureAccess();
               // string filter = "&$filter=substringof('" + f + "',Description) eq true";
                string skiptoken = "&$skiptoken = '" + offset + "' ,guid'dd5f25dd-9572-489f-9b1c-2cb8ef382851'";
                 Uri request = new Uri(BASE_URI + "/api/v1/" + CurrentDivision + "/logistics/Items?access_token=" + AccessToken + orderby + skiptoken);

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
              //  JArray next = JObject.Parse(responsecontent).GetValue("d")["__next"].ToObject<JArray>();

                if(JObject.Parse(responsecontent).GetValue("d")["__next"] != null)
                {
                    
                    
                    offset =  (Convert.ToInt32(JObject.Parse(responsecontent).GetValue("d")["__next"].ToString().Substring(92, 5).ToString())+1).ToString();
                }
                else
                {
                    offset = "";
                }
                foreach (JObject item in results)
                {
                    double dCost = ((double)item["CostPriceStandard"]) * 2.05;
                    string sCode = (string)item["Code"];
                    string sDescription = (string)item["Description"];
                    Result.Add(new Items(sCode, sDescription, dCost));
                }
            }
        }

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

        public async Task deleteItems(Items it)
        {
            await ensureAccess();
            Uri request = new Uri(BASE_URI + "/api/v1/" + CurrentDivision + "/financial/GLAccounts(guid\'" + it.Code + "\')?access_token=" + AccessToken);
            HttpClient client = new HttpClient();

            HttpResponseMessage respons = await client.DeleteAsync(request);

            if (respons.IsSuccessStatusCode == false)
            {
                await errorAlert("updateGLAccount Mislukt:  status = " + respons.StatusCode.ToString());
                return;
            }
            respons.EnsureSuccessStatusCode();
        }

    }

}


