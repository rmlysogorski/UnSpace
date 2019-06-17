using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace UnSpaceWebApp.Models
{
    public class EtsyDAL
    {
        private static string APIKey = ConfigurationManager.AppSettings["EtsyAPIKey"];
        public static JObject GetEtsyAPI(string options)
        {
            HttpWebRequest request = WebRequest.CreateHttp($"https://openapi.etsy.com/v2/listings/active?api_key={APIKey}{options}");


            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader data = new StreamReader(response.GetResponseStream());
                JObject furnitureData = JObject.Parse(data.ReadToEnd());
                return furnitureData;

            }
            else
            {
                return null;
            }
        }
    }
}