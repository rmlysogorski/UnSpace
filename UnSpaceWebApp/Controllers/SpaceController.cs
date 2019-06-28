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
    [Authorize]
    public class SpaceController : Controller
    {
        public static MySpace thisSpace = new MySpace();
        // GET: Space
        public ActionResult Index(string Id)
        {
            if(Id != null)
            {
                UserSpace userSpace = UnSpaceDbController.GetThisUserSpace(Id);
                thisSpace.furnList = GetSavedSpaceFurn(userSpace);
                thisSpace.Id = Id;
                thisSpace.Name = userSpace.Name;
                UnravelDimensionsString(userSpace.SpaceDimensions);
                UnravelPositionsString(userSpace.Positions);
            }
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
            if(TempData["MaxP"] != null)
            {
                ViewBag.MaxP = TempData["MaxP"];
                TempData["MaxP"] = TempData["MaxP"];
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

        public static string GeneratePositionString(List<string> Left, List<string> Top)
        {
            string positions = string.Empty;
            for(int i=0; i < Left.Count; i++)
            {
                positions += Left[i] + "|" + Top[i] + ",";
            }
            return positions;
        }
        
        public static string GenerateSpaceDimensions()
        {
            string spaceDimensions = $"{thisSpace.SpaceDimensions.Width}|{thisSpace.SpaceDimensions.Length}|{thisSpace.SpaceDimensions.Measurement}";
            return spaceDimensions;
        }

        public static void UnravelDimensionsString(string Listing)
        {
            thisSpace.SpaceDimensions.Width = Listing.Split('|')[0];
            thisSpace.SpaceDimensions.Length = Listing.Split('|')[1];
            thisSpace.SpaceDimensions.Measurement = Listing.Split('|')[2];
        }

        public static void UnravelPositionsString(string Positions)
        {
            int count = 0;
            foreach (string pos in Positions.Split(','))
            {
                if(pos != string.Empty)
                {
                    thisSpace.furnList[count].Positions.Left = pos.Split('|')[0];
                    thisSpace.furnList[count].Positions.Top = pos.Split('|')[1];
                    count++;
                }
            }
        }

        public ActionResult AutofillForm()
        {
            return View();
        }

        public static EtsyItem MakeRandomEtsyItem(string Listing_Id)
        {
            EtsyItem newItem = new EtsyItem();
            newItem.Listing_Id = Listing_Id;
            newItem = EtsyDAL.MakeEtsyItem(newItem.Listing_Id);
            if (newItem.Item_Width is null || newItem.Item_Width == "0")
            {
                newItem.Item_Width = "150";
                newItem.Item_Length = "150";
            }
            return newItem;
        }
        public ActionResult AutoFill(string benjamin)
        {
            List<EtsyItem> items = new List<EtsyItem>();
            Random randomPage = new Random();

            switch (benjamin)
            {
                case "Living Room":

                    Random randomResult = new Random();
                    int randomo = randomResult.Next(0, 25);
                    JObject data = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 2005)}&category=furniture&keywords=" + "table", "active");
                    items.Add(MakeRandomEtsyItem(data["results"][randomo]["listing_id"].ToString()));
                    JObject data2 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 123)}&category=furniture&keywords=" + "couch", "active");
                    items.Add(MakeRandomEtsyItem(data2["results"][randomo]["listing_id"].ToString()));
                    JObject data3 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 123)}&category=furniture&keywords=" + "chair", "active");
                    items.Add(MakeRandomEtsyItem(data3["results"][randomo]["listing_id"].ToString()));
                    break;

                case "Kitchen":
                    Random randomResultK = new Random();
                    int randomoK = randomResultK.Next(0, 25);
                    JObject dataK = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 2005)}&keywords=" + "plant", "active");
                    items.Add(MakeRandomEtsyItem(dataK["results"][randomoK]["listing_id"].ToString()));
                    JObject dataK2 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 20)}&category=furniture&keywords=" + "clock", "active");
                    items.Add(MakeRandomEtsyItem(dataK2["results"][randomoK]["listing_id"].ToString()));
                    JObject dataK3 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 851)}&category=furniture&keywords=" + "stool", "active");
                    items.Add(MakeRandomEtsyItem(dataK3["results"][randomoK]["listing_id"].ToString()));
                    break;
                case "Bed Room":
                    Random randomResultB = new Random();
                    int randomoB = randomResultB.Next(0, 25);
                    JObject dataB = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 557)}&category=furniture&keywords=" + "bed", "active");
                    items.Add(MakeRandomEtsyItem(dataB["results"][randomoB]["listing_id"].ToString()));
                    JObject dataB2 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 135)}&category=furniture&keywords=" + "night%20stand", "active");
                    items.Add(MakeRandomEtsyItem(dataB2["results"][randomoB]["listing_id"].ToString()));
                    JObject dataB3 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 2005)}&keywords=" + "lamp", "active");
                    items.Add(MakeRandomEtsyItem(dataB3["results"][randomoB]["listing_id"].ToString()));
                    break;
                case "Dinning Room":
                    Random randomResultD = new Random();
                    int randomoD = randomResultD.Next(0, 25);
                    JObject dataD = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 52)}&category=furniture&keywords=" + "dinning%20table", "active");
                    items.Add(MakeRandomEtsyItem(dataD["results"][randomoD]["listing_id"].ToString()));

                    JObject dataD2 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 123)}&category=furniture&keywords=" + "chair", "active");
                    items.Add(MakeRandomEtsyItem(dataD2["results"][randomoD]["listing_id"].ToString()));
                    JObject dataD3 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 24)}&keywords=" + "clock", "active");
                    items.Add(MakeRandomEtsyItem(dataD3["results"][randomoD]["listing_id"].ToString()));
                    break;
                case "Finished Basement":
                    Random randomResultF = new Random();
                    int randomoF = randomResultF.Next(0, 25);
                    JObject dataF = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 2005)}&keywords=" + "painting", "active");
                    items.Add(MakeRandomEtsyItem(dataF["results"][randomoF]["listing_id"].ToString()));
                    JObject dataF2 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 123)}&category=furniture&keywords=" + "chair", "active");
                    items.Add(MakeRandomEtsyItem(dataF2["results"][randomoF]["listing_id"].ToString()));
                    JObject dataF3 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 123)}&keywords=" + "lamp", "active");
                    items.Add(MakeRandomEtsyItem(dataF3["results"][randomoF]["listing_id"].ToString()));
                    break;
                default:

                    break;
            }
            thisSpace.furnList = items;
            return RedirectToAction("Index");
        }

        public ActionResult GenerateSpace(string Width, string Length, string Measurement)
        {
            double.TryParse(Width, out double width);
            double.TryParse(Length, out double length);
            thisSpace.SpaceDimensions.Measurement = Measurement;
            switch (thisSpace.SpaceDimensions.Measurement)
            {
                case "cm":
                    break;
                case "ft":
                    width = width / 0.032808;
                    length = length / 0.032808;
                    break;
                case "in":
                    width = width / 12 / 0.032808;
                    length = length / 12 / 0.032808;
                    break;
            }
            width *= 1.5;
            length *= 1.5;
            thisSpace.SpaceDimensions.Width = width.ToString();
            thisSpace.SpaceDimensions.Length = length.ToString();
            return View("Index", thisSpace);
        }

        public ActionResult AddFurn(string Listing_Id, List<string> Left, List<string> Top, string Measurement, string Width = "", string Length = "")
        {
            SavePositions(Left, Top);
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
            if (TempData["MaxP"] != null)
            {
                TempData["MaxP"] = TempData["MaxP"];
            }
            EtsyItem etsyItem = new EtsyItem();
            etsyItem.Listing_Id = Listing_Id;
            etsyItem = EtsyDAL.MakeEtsyItem(etsyItem.Listing_Id);
            if (Width != string.Empty)
            {
                etsyItem.Item_Width = EtsyDAL.CalculatePixels(Width, Measurement);
            }
            if (Length != string.Empty)
            {
                etsyItem.Item_Length = EtsyDAL.CalculatePixels(Length, Measurement);
            }
            thisSpace.furnList.Add(etsyItem);
            return RedirectToAction("Index");
        }

        public ActionResult SaveFurn(List<string> Listings, List<string> Left, List<string> Top, string Name = "")
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
            if (TempData["MaxP"] != null)
            {
                TempData["MaxP"] = TempData["MaxP"];
            }
            SavePositions(Left, Top);
            UserSpace userSpace = new UserSpace();
            userSpace.UserId = User.Identity.Name;
            userSpace.Name = Name;
            if (Name == string.Empty || Name == null)
            {
                return RedirectToAction("Index");
            }
            if(Listings == null)
            {
                return RedirectToAction("Index");
            }
            userSpace.Listing = GenerateListingString(Listings);
            userSpace.SpaceDimensions = GenerateSpaceDimensions();
            userSpace.Positions = GeneratePositionString(Left, Top);
            TempData["furnList"] = thisSpace.furnList;
            
            return RedirectToAction("SaveUserSpace", "UnSpaceDb",  userSpace );
        }

        public List<EtsyItem> GetSavedSpaceFurn(UserSpace thisUserSpace)
        {   
            List<EtsyItem> userItems = new List<EtsyItem>();
            //int count = 0;
            foreach (string listing in thisUserSpace.Listing.Split(','))
            {
                if (listing != string.Empty)
                {
                    EtsyItem newItem = new EtsyItem();
                    newItem = UnSpaceDbController.ConvertEtsyItemDb(listing);
                    userItems.Add(newItem);
                }
            }

            //List<EtsyItem> returnList = new List<EtsyItem>();
            //foreach (EtsyItem e in userItems)
            //{
            //    EtsyItem newItem = new EtsyItem();
            //    newItem.Listing_Id = e.Listing_Id;
            //    newItem.Positions.Left = thisUserSpace.Positions.Split(',')[count].Split('|')[0];
            //    newItem.Positions.Top = thisUserSpace.Positions.Split(',')[count].Split('|')[1];
            //    count++;
            //    JObject data = EtsyDAL.GetEtsyAPI(e.Listing_Id, "listing");
            //    if(data["results"][0]["state"].ToString() != "sold_out")
            //    {
            //        newItem.Title = data["results"][0]["title"].ToString();
            //        newItem.Url = data["results"][0]["url"].ToString();
            //        newItem.Price = data["results"][0]["price"].ToString();
            //        newItem.Item_Width = data["results"][0]["item_width"].ToString();
            //        newItem.Item_Length = data["results"][0]["item_length"].ToString();
            //        newItem.Item_Height = data["results"][0]["item_height"].ToString();
            //        newItem.Item_Dimensions_unit = data["results"][0]["item_dimensions_unit"].ToString();
            //        newItem.Currency_Code = data["results"][0]["currency_code"].ToString();
            //        JObject imageData = EtsyDAL.GetEtsyAPI(newItem.Listing_Id, "image");
            //        newItem.ImageThumbUrl = imageData["results"][0]["url_75x75"].ToString();
            //        newItem.ImageFullUrl = imageData["results"][0]["url_fullxfull"].ToString();
            //    }                
            //    returnList.Add(newItem);
            //    System.Threading.Thread.Sleep(500);
            //}
            return userItems;
        }

        public ActionResult RemoveFurn(string Index, List<string> Left, List<string> Top)
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
            if (TempData["MaxP"] != null)
            {
                TempData["MaxP"] = TempData["MaxP"];
            }
            SavePositions(Left, Top);
            thisSpace.furnList.RemoveAt(int.Parse(Index));
            return RedirectToAction("Index");
        }

        public ActionResult SlideShowTest()
        {            
            return View(thisSpace.furnList);
        }

        public static void SavePositions(List<string> Left, List<string> Top)
        {
            if (Left != null)
            {
                for (int i = 0; i < Left.Count; i++)
                {
                    thisSpace.furnList[i].Positions.Left = Left[i];
                    thisSpace.furnList[i].Positions.Top = Top[i];
                }
            }
        }

        public ActionResult MakeNewSpace()
        {
            thisSpace = new MySpace();
            return RedirectToAction("Index");
        }

        

    }
}