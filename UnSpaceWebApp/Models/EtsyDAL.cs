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
        public static JObject GetEtsyAPI(string options, string code)
        {
            string url = "https://openapi.etsy.com/v2/listings/";
            if (code == "active")
            {
                url += $"active?api_key={APIKey}{options}";
            }
            if(code == "image")
            {
                url += $"{options}/images?api_key={APIKey}";
            }
            if(code == "listing")
            {
                url += $"{options}?api_key={APIKey}";
            }

            HttpWebRequest request = WebRequest.CreateHttp(url);
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

        public static EtsyItem MakeEtsyItem(string Listing_Id)
        {
            EtsyItem newItem = new EtsyItem();
            newItem.Listing_Id = Listing_Id;
            JObject data = GetEtsyAPI(Listing_Id, "listing");
            if (data["results"][0]["state"].ToString() != "sold_out")
            {
                newItem.Title = data["results"][0]["title"].ToString();
                newItem.Title = HttpUtility.HtmlDecode(newItem.Title);
                newItem.Description = data["results"][0]["description"].ToString();
                newItem.Description = HttpUtility.HtmlDecode(newItem.Description);
                newItem.Url = data["results"][0]["url"].ToString();
                newItem.Price = data["results"][0]["price"].ToString();
                newItem.Item_Width = data["results"][0]["item_width"].ToString();               
                newItem.Item_Length = data["results"][0]["item_length"].ToString();
                newItem.Item_Height = data["results"][0]["item_height"].ToString();
                newItem.Item_Dimensions_unit = data["results"][0]["item_dimensions_unit"].ToString();
                if(newItem.Item_Width != null && newItem.Item_Dimensions_unit != null)
                {
                    newItem.Item_Width = CalculatePixels(newItem.Item_Width, newItem.Item_Dimensions_unit);
                    newItem.Item_Length = CalculatePixels(newItem.Item_Length, newItem.Item_Dimensions_unit);
                }
                newItem.Currency_Code = data["results"][0]["currency_code"].ToString();
                JObject imageData = GetEtsyAPI(newItem.Listing_Id, "image");
                newItem.ImageThumbUrl = imageData["results"][0]["url_170x135"].ToString();
                newItem.ImageFullUrl = imageData["results"][0]["url_fullxfull"].ToString();
            }
            return newItem;
        }

        public static string CalculatePixels(string widthOrLength, string Measurement)
        {
            double.TryParse(widthOrLength, out double WOL);
            switch (Measurement.ToLower())
            {
                case "cm":
                    break;
                case "ft":
                    WOL = WOL / 0.032808;
                    break;
                case "in":
                    WOL = WOL / 12 / 0.032808;
                    break;
            }
            WOL *= 1.5;
            return WOL.ToString();
        }
    }
}