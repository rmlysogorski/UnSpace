using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using UnSpaceWebApp.Models;
using System.Web.Mvc;
using System.Web.UI;
using Newtonsoft.Json.Linq;

namespace UnSpaceWebApp.Controllers
{
    public class SpaceController : Controller
    {

        UserSpacesEntities ORM = new UserSpacesEntities();
        public static MySpace thisSpace = new MySpace();
        // GET: Space
        public ActionResult Index()
        {
            if(TempData["items"] != null)
            {
                thisSpace.items = (List<EtsyItem>)TempData["items"];
            }
            if(TempData["prevPage"] != null)
            {
                ViewBag.PrevPage = TempData["prevPage"];
            }
            if(TempData["nextPage"] != null)
            {
                ViewBag.NextPage = TempData["nextPage"];
            }
            if(TempData["SearchQ"] != null)
            {
                ViewBag.SearchQ = TempData["SearchQ"];
            }
            return View(thisSpace);
        }  

        public static string AddToList(List<string> Listing)
        {

            string listing = string.Empty;
            for(int i=0;i<Listing.Count; i++)
            {
                listing += Listing[i] + ",";

            }
            return listing;
        }

        public ActionResult AutoFill()
        {
           
            List<EtsyItem> items = new List<EtsyItem>();
            
            JObject data = EtsyDAL.GetEtsyAPI("&category=furniture&keywords=" + "table", "active");

            Random table = new Random();

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
            }

            
            return View();

        }


    

        public ActionResult GenerateSpace(string Width, string Length, string Measurement)
        {
            thisSpace.SpaceDimensions.Width = Width;
            thisSpace.SpaceDimensions.Length = Length;
            thisSpace.SpaceDimensions.Measurement = Measurement;
            return View("Index", thisSpace);
        }
    }
}