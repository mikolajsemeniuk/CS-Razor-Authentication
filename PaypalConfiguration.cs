using System.Linq;
using System.Web;
using PayPal.Api;
using System.Collections.Generic;

namespace application;
  
    public class PaypalConfiguration
    {
        public readonly static string clientId;
        public readonly static string clientSecret;
        static PaypalConfiguration()
        {
            var config = getconfig();
            clientId = "AQisFmSHllJGx8UhcAYS-IP4Eg8Subi1yoWx3JTbFzgxBstymCFOb61vGJSt48-lm0Wox2k-aHN__Daa";
            clientSecret = "EE9TdNiAxvu4_WjjFuva0lV1MX2Gvs9280nXkAyNi7dZsGSLv9KGkAE20iylAwZ1lPOHqwqZsRJ2E7zB";
        }

        private static Dictionary<string, string> getconfig()
        {
            return ConfigManager.Instance.GetProperties();
        }

        private static string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(clientId, clientSecret, getconfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            APIContext apicontext = new APIContext(GetAccessToken());
            apicontext.Config = getconfig();
            return apicontext;
        }
    }