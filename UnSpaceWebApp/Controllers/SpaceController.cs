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
                TempData["prevPage"] = TempData["prevPage"];
            }
            if(TempData["nextPage"] != null)
            {
                ViewBag.NextPage = TempData["nextPage"];
                TempData["nextPage"] = TempData["nextPage"];
            }
            if(TempData["SearchQ"] != null)
            {
                ViewBag.SearchQ = TempData["SearchQ"];
                TempData["SearchQ"] = TempData["SearchQ"];
            }
            return View(thisSpace);
        }  

        public static string GenerateListingString(List<string> Listing)
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
            Random randomPage = new Random();

            JObject data = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0,2005)}&category=furniture&keywords=" + "table", "active");
            Random randomResult = new Random();
            int randomo = randomResult.Next(0, 25);

            EtsyItem newItem = new EtsyItem();
            newItem.Listing_Id = data["results"][randomo]["listing_id"].ToString();

            JObject data2 = EtsyDAL.GetEtsyAPI($"{newItem.Listing_Id}", "listing");

            newItem.Title = data["results"][randomo]["title"].ToString();
            newItem.Url = data["results"][randomo]["url"].ToString();
            newItem.Price = data["results"][randomo]["price"].ToString();
            newItem.Item_Width = data2["results"][0]["item_width"].ToString();
            newItem.Item_Length = data2["results"][0]["item_length"].ToString();
            newItem.Item_Height = data2["results"][0]["item_height"].ToString();
            newItem.Item_Dimensions_unit = data["results"][randomo]["item_dimensions_unit"].ToString();
            newItem.Currency_Code = data["results"][randomo]["currency_code"].ToString();
            JObject imageData = EtsyDAL.GetEtsyAPI(newItem.Listing_Id, "image");
            newItem.ImageThumbUrl = imageData["results"][0]["url_75x75"].ToString();
            newItem.ImageFullUrl = imageData["results"][0]["url_fullxfull"].ToString();
            items.Add(newItem);            
            
            return View(items);
        }


    

        public ActionResult GenerateSpace(string Width, string Length, string Measurement)
        {
            thisSpace.SpaceDimensions.Width = Width;
            thisSpace.SpaceDimensions.Length = Length;
            thisSpace.SpaceDimensions.Measurement = Measurement;
            return View("Index", thisSpace);
        }

        public ActionResult AddFurn(string Listing_Id, string Title, string Url, string Price, string Currency_Code, string ImageThumbUrl, string ImageFullUrl)
        {
            if (TempData["prevPage"] != null)
            {
                TempData["prevPage"] = TempData["prevPage"];
            }
            if (TempData["nextPage"] != null)
            {
                TempData["nextPage"] = TempData["nextPage"];
            }
            if (TempData["SearchQ"] != null)
            {
                TempData["SearchQ"] = TempData["SearchQ"];
            }
            EtsyItem etsyItem = new EtsyItem();
            etsyItem.Listing_Id = Listing_Id;
            if (Title.Contains('&'))
            {
                Title.Replace("&", "&amp;");
            }
            etsyItem.Title = Title;
            etsyItem.Url = Url;
            etsyItem.Price = Price;
            etsyItem.Currency_Code = Currency_Code;
            etsyItem.ImageThumbUrl = ImageThumbUrl;
            etsyItem.ImageFullUrl = ImageFullUrl;
            thisSpace.furnList.Add(etsyItem);
            return RedirectToAction("Index");
        }

        public ActionResult SaveFurn(List<string> Listings, List<string> Left, List<string> Top)
        {
            if (TempData["prevPage"] != null)
            {
                TempData["prevPage"] = TempData["prevPage"];
            }
            if (TempData["nextPage"] != null)
            {
                TempData["nextPage"] = TempData["nextPage"];
            }
            if (TempData["SearchQ"] != null)
            {
                TempData["SearchQ"] = TempData["SearchQ"];
            }
            string left = Left[0];
            string top = Top[0];
            UserSpace userSpace = new UserSpace();
            userSpace.UserId = User.Identity.Name;
            userSpace.Listing = GenerateListingString(Listings);
            return RedirectToAction("SaveUserSpace", "UnSpaceDb", userSpace);
        }

        public List<EtsyItem> GetSavedSpace(int id)
        {
            UnSpaceDbController uc = new UnSpaceDbController();
            List<UserSpace> userSpaces = uc.GetUserSpaces();
            UserSpace thisUserSpace = userSpaces.Find(u => u.Id == id);
            List<EtsyItem> userItems = new List<EtsyItem>();
            int count = 0;
            foreach(string listing in thisUserSpace.Listing.Split(','))
            {
                if(listing != string.Empty)
                {
                    userItems[count].Listing_Id = listing;
                }
            }
            foreach(EtsyItem e in userItems)
            {
                EtsyItem newItem = new EtsyItem();
                JObject data = EtsyDAL.GetEtsyAPI(e.Listing_Id, "listing");
                newItem.Title = data["results"][0]["title"].ToString();
                newItem.Url = data["results"][0]["url"].ToString();
                newItem.Price = data["results"][0]["price"].ToString();
                newItem.Item_Width = data["results"][0]["item_width"].ToString();
                newItem.Item_Length = data["results"][0]["item_length"].ToString();
                newItem.Item_Height = data["results"][0]["item_height"].ToString();
                newItem.Item_Dimensions_unit = data["results"][0]["item_dimensions_unit"].ToString();
                newItem.Currency_Code = data["results"][0]["currency_code"].ToString();
                JObject imageData = EtsyDAL.GetEtsyAPI(newItem.Listing_Id, "image");
                newItem.ImageThumbUrl = imageData["results"][0]["url_75x75"].ToString();
                newItem.ImageFullUrl = imageData["results"][0]["url_fullxfull"].ToString();
                System.Threading.Thread.Sleep(500);
            }
            return userItems;
        }

        public ActionResult RemoveFurn(string Index)
        {
            if (TempData["prevPage"] != null)
            {
                TempData["prevPage"] = TempData["prevPage"];
            }
            if (TempData["nextPage"] != null)
            {
                TempData["nextPage"] = TempData["nextPage"];
            }
            if (TempData["SearchQ"] != null)
            {
                TempData["SearchQ"] = TempData["SearchQ"];
            }
            thisSpace.furnList.RemoveAt(int.Parse(Index));
            return RedirectToAction("Index");
        }
    }
}