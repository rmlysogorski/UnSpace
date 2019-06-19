using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnSpaceWebApp.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;


namespace UnSpaceWebApp.Models
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }     
        
        public ActionResult FurnitureList()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult FurnitureList(string SearchQ, string pageNo = "")
        {
            List<EtsyItem> items = new List<EtsyItem>();
            if (SearchQ != null)
            {   
                if(pageNo != string.Empty)
                {
                    if (int.Parse(pageNo) > 1)
                    {
                        SearchQ += "&page=" + pageNo;
                    }
                }
                JObject data = EtsyDAL.GetEtsyAPI("&limit=5&category=furniture&keywords=" + SearchQ, "active");
                if (SearchQ.Contains('&'))
                {
                    TempData["SearchQ"] = SearchQ.Split('&')[0];
                }
                else
                {
                    TempData["SearchQ"] = SearchQ;
                }
                int prevPage = (int)data["pagination"]["effective_page"];
                if (prevPage > 1)
                {
                    TempData["prevPage"] = prevPage - 1;
                }
                else
                {
                    TempData["prevPage"] = null;
                }
                TempData["nextPage"] = (int)data["pagination"]["next_page"];
                for (int i = 0; i < data["results"].Count(); i++)
                {
                    EtsyItem newItem = new EtsyItem();
                    newItem.Listing_Id = data["results"][i]["listing_id"].ToString();
                    newItem.Title = data["results"][i]["title"].ToString();
                    newItem.Url = data["results"][i]["url"].ToString();
                    newItem.Price = data["results"][i]["price"].ToString();
                    newItem.Item_Width = data["results"][i]["item_width"].ToString();
                    newItem.Item_Length = data["results"][i]["item_length"].ToString();
                    newItem.Item_Height = data["results"][i]["item_height"].ToString();
                    newItem.Item_Dimensions_unit = data["results"][i]["item_dimensions_unit"].ToString();
                    newItem.Currency_Code = data["results"][i]["currency_code"].ToString();
                    JObject imageData = EtsyDAL.GetEtsyAPI(newItem.Listing_Id, "image");
                    newItem.ImageThumbUrl = imageData["results"][0]["url_75x75"].ToString();
                    newItem.ImageFullUrl = imageData["results"][0]["url_fullxfull"].ToString();
                    items.Add(newItem);
                    System.Threading.Thread.Sleep(500);
                }
            }
            TempData["items"] = items;
            return RedirectToAction("Index", "Space");
        }

        public ActionResult SearchPage(int pageNo, string searchQ)
        {
            string newSearchQ = searchQ;
            if (pageNo > 1)
            {
                newSearchQ += "&page=" + pageNo.ToString();
            }
            return RedirectToAction("FurnitureList", "Search", new { SearchQ = newSearchQ });
        }
    }
}