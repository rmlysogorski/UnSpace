﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnSpaceWebApp.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using UnSpaceWebApp.Controllers;

namespace UnSpaceWebApp.Models
{
    [Authorize]
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
        public ActionResult FurnitureList( List<string> Left, List<string> Top, string SearchQ, string MaxP, string pageNo = "")
        {
            SpaceController.SavePositions(Left, Top);
            List<EtsyItem> items = new List<EtsyItem>();
            if (SearchQ != null && SearchQ != string.Empty)
            {   
                if(pageNo != string.Empty)
                {
                    if (int.Parse(pageNo) > 1)
                    {
                        SearchQ += "&page=" + pageNo;
                    }
                }
                if (MaxP != string.Empty)
                {
                    
                    SearchQ += "&min_price= 0";
                    SearchQ += "&max_price= " + MaxP;
                    TempData["MaxP"] = MaxP;

                }
                JObject data = EtsyDAL.GetEtsyAPI(" &limit=5&category=furniture&keywords=" + SearchQ, "active");
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
                string nextPage = data["pagination"]["next_page"].ToString();
                if (nextPage != string.Empty)
                {
                    TempData["nextPage"] = (int)data["pagination"]["next_page"];
                }
                for (int i = 0; i < data["results"].Count(); i++)
                {
                    EtsyItem newItem = new EtsyItem();
                    newItem.Listing_Id = data["results"][i]["listing_id"].ToString();
                    newItem = EtsyDAL.MakeEtsyItem(newItem.Listing_Id);
                    items.Add(newItem);
                    System.Threading.Thread.Sleep(500);
                }
            }
            TempData["items"] = items;
            return RedirectToAction("Index", "Space");
        }

        public ActionResult SearchPage(int pageNo, string searchQ, List<string> Left, List<string> Top)
        {
            SpaceController.SavePositions(Left, Top);
            string newSearchQ = searchQ;
            if (pageNo > 1)
            {
                newSearchQ += "&page=" + pageNo.ToString();
            }
            return RedirectToAction("FurnitureList", "Search", new { SearchQ = newSearchQ });
        }
    }
}