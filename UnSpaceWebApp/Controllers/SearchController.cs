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
        private static int countPage;
        private List<EtsyItem> items = new List<EtsyItem>();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewProductList(string searchQ)
        {
            if(Session["data"] != null)
            {
                Session.Remove("data");
            }
            countPage = 0;
            return RedirectToAction("ProductList", "Search", new { searchQ });
        }

        public ActionResult ProductList(string searchQ, string forOrBack = "")
        {
            ViewBag.SearchQ = searchQ;
            JObject data = new JObject();
            if(Session["data"] == null)
            {
                data = EtsyDAL.GetEtsyAPI("&category=furniture&keywords=" + searchQ, "active");
                Session["data"] = data;
            }
            else
            {
                data = (JObject)Session["data"];
            }                        

            int min = 0;
            int max = 5;
            if(forOrBack == "for")
            {
                countPage++;
            }
            else if (forOrBack == "back")
            {
                countPage--;
            }

            int pagePrev;
            int pageNext;

            if (countPage == 0)
            {
                pagePrev = (int)data["pagination"]["effective_page"] - 1;
                if (pagePrev > 0)
                {
                    ViewBag.Prev = pagePrev.ToString();
                }
            }

            if (countPage == 4)
            {
                pageNext = (int)data["pagination"]["next_page"];
                ViewBag.Next = pageNext.ToString();
            }

            switch (countPage)
            {
                case 0:
                    min = 0;
                    max = 5;
                    break;
                case 1:
                    min = 5;
                    max = 10;
                    break;
                case 2:
                    min = 10;
                    max = 15;
                    break;
                case 3:
                    min = 15;
                    max = 20;
                    break;
                case 4:
                    min = 20;
                    max = 25;
                    break;
            }

            ViewBag.CountPage = countPage;
            for (int i = min; i < max; i++)
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
                JObject imageData = EtsyDAL.GetEtsyAPI(newItem.Listing_Id,"image");
                newItem.ImageThumbUrl = imageData["results"][0]["url_75x75"].ToString();
                newItem.ImageFullUrl = imageData["results"][0]["url_fullxfull"].ToString();
                items.Add(newItem);
            }
            return View("Index",items);
        }

        public ActionResult SearchPage(string pageNo, string searchQ, string forOrBack)
        {
            string newSearchQ = "&category=furniture&keywords=" + searchQ + "&page=" + pageNo;
            Session["data"] = EtsyDAL.GetEtsyAPI(newSearchQ, "active"); ;

            if (forOrBack == "for")
            {
                countPage = 0;
            }
            
            if(forOrBack == "back")
            {
                countPage = 4;
            }
            return RedirectToAction("ProductList", "Search", new { searchQ = newSearchQ });
        }
    }
}