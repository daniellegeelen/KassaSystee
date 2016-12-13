//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Net.Http;
//using Newtonsoft.Json.Serialization;
//using System.Windows;

//namespace KassaSystee
//{
//    public sealed class Oauth
//    {
//        private static readonly Oauth instance = new Oauth();
//        private Oauth() { }
//        public static Oauth Instance
//        {
//            get
//            {
//                return instance;
//            }
//        }
//        const string CLIENT_ID = "929f51e9-879c-4a84-b21f-773fa74a6531";//const may not be modifyed
//        const string CLIENT_SECRET = "ykYJ2PYnrjE2";
//        const string CLIENT_STATE = "1wzxvfh2naf3vvkj2lprxyda";
//        const string CALLBACK_URL = "http://youtube.nl";
//        const string BASE_URI = "https://start.exactonline.nl";

//        private String Code = null, State = null, AccessToken = null, RefreshToken = null, CurrentDivision = null, FullName = null;
//        private Double ExpiredAt = DateTime.UtcNow.Ticks / 10000000;

//        public List<GLAccount> Result = new List<GLAccount>();
//        public async Task errorAlert(string s)
//        {

//            MessageBox.Show("" + s);

//            (s).ShowAsync();

//            Application.Current.Shutdown();
//        }

//        public async Task getAuthorization()
//        {
//            string request = BASE_URI + "/api/oauth2/auth" +
//                            "?client_id={" + CLIENT_ID + "}" +
//                            "&redirect_uri=" + CALLBACK_URL +
//                            "&state=" + CLIENT_STATE +
//                            "&response_type=code";

//            System.Uri requestUri = new Uri(request);
//            System.Uri callbackUri = new Uri(CALLBACK_URL);

//            try
//            {


//                WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, requestUri, callbackUri);

//                switch (result.ResponseStatus)
//                {
//                    case WebAuthenticationStatus.Success:                               // Successful authentication. 
//                        String response = result.ResponseData.ToString();
//                        int c = response.IndexOf("?code=");
//                        int s = response.IndexOf("&state=");
//                        Code = response.Substring(c + 6, s - c - 6);
//                        State = response.Substring(s + 7);                              //notify("Gelukt\n\nRespons: " + response + "\n\nCode: " + Code + "\n\nState: " + State);
//                        break;
//                    case WebAuthenticationStatus.ErrorHttp:                             // HTTP error. 
//                        await errorAlert("getAuthorization Mislukt " + result.ResponseErrorDetail.ToString());
//                        break;
//                    default:                                                            // Other error.
//                        await errorAlert("getAuthorization Mislukt " + result.ResponseData.ToString());
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Error launching WebAuth " + ex.Message + "\r\n");
//            }
//        }

//    }
//}
