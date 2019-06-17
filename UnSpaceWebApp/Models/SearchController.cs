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

        //public ActionResult ProductList(string furiture)
        //{
        //    JObject chair = EtsyDAL.GetEtsyAPI("&category=furniture");
        //    ViewBag.chair = chair;
        //    List<EtsyItem> items = new List<EtsyItem>();
        //    for (int i = 0; i < chair.Count; i++)
        //    {
        //        for (int j = 0; j =< chair[i]["tags"].Contains("chair"); j++)
        //        {
        //            if (chair[i]["tags"][j].Contains("chair"))
        //            {

        //            }
        //        }
        //    }
        //    return View();
        

    }
}