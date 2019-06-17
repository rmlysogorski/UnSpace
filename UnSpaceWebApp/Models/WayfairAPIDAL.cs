using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace UnSpaceWebApp.Models
{
    public class WayfairAPIDAL
    {       
        private static string APIKey = ConfigurationManager.AppSettings["WayfairAPIKey"];
        public static JArray GetWayfairAPI(string options)
        {
            HttpWebRequest request = WebRequest.CreateHttp("https://www.wayfair.com/3dapi/models" + options); 
            request.Headers["Allow-Control-Allow-Origin"] = "*";
            request.Headers["Username"] = "rmlysogorski@gmail.com";
            request.Headers["Password"] = APIKey;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader data = new StreamReader(response.GetResponseStream());
                JArray furnitureData = JArray.Parse(data.ReadToEnd());
                return furnitureData;

            }
            else
            {
                return null;
            }
        }
    }
}