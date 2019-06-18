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

        public ActionResult ProductList(string searchQ)
        {
            JObject data = EtsyDAL.GetEtsyAPI("&category=furniture&keywords=" + searchQ);
            List<EtsyItem> items = new List<EtsyItem>();
            for (int i = 0; i < 25; i++)
            {
                EtsyItem newItem = new EtsyItem();
                newItem.Listing_Id = data["results"][i]["listing_id"].ToString();
                items.Add(newItem);
            }
            return View("Index",items);
        }


    }
}