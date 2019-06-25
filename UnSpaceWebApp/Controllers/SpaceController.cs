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
                        EtsyItem newItem = new EtsyItem();
                        newItem.Listing_Id = data["results"][randomo]["listing_id"].ToString();
                        items.Add(EtsyDAL.MakeEtsyItem(newItem.Listing_Id));

                        JObject data2 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 123)}&category=furniture&keywords=" + "couch", "active");
                        EtsyItem newItem2 = new EtsyItem();
                        newItem2.Listing_Id = data2["results"][randomo]["listing_id"].ToString();
                        items.Add(EtsyDAL.MakeEtsyItem(newItem2.Listing_Id));

                        JObject data3 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 123)}&category=furniture&keywords=" + "chair", "active");                                                                  
                        EtsyItem newItem3 = new EtsyItem();               
                        newItem3.Listing_Id = data3["results"][randomo]["listing_id"].ToString();
                        items.Add(EtsyDAL.MakeEtsyItem(newItem3.Listing_Id));


                        break;

                    
                case "Kitchen":
                    Random randomResultK = new Random();
                    int randomoK = randomResultK.Next(0, 25);

                    JObject dataK = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 2005)}&keywords=" + "plant", "active");
                    EtsyItem newItemK = new EtsyItem();
                    newItemK.Listing_Id = dataK["results"][randomoK]["listing_id"].ToString();
                    items.Add(EtsyDAL.MakeEtsyItem(newItemK.Listing_Id));

                    JObject dataK2 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 20)}&category=furniture&keywords=" + "clock", "active");
                    EtsyItem newItemK2 = new EtsyItem();
                    newItemK2.Listing_Id = dataK2["results"][randomoK]["listing_id"].ToString();
                    items.Add(EtsyDAL.MakeEtsyItem(newItemK2.Listing_Id));

                    JObject dataK3 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 851)}&category=furniture&keywords=" + "stool", "active");
                    EtsyItem newItemK3 = new EtsyItem();
                    newItemK3.Listing_Id = dataK3["results"][randomoK]["listing_id"].ToString();
                    items.Add(EtsyDAL.MakeEtsyItem(newItemK3.Listing_Id));


                    break;

                case "Bed Room":
                    Random randomResultB = new Random();
                    int randomoB = randomResultB.Next(0, 25);

                    JObject dataB = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 557)}&category=furniture&keywords=" + "bed", "active");
                    EtsyItem newItemB = new EtsyItem();
                    newItemB.Listing_Id = dataB["results"][randomoB]["listing_id"].ToString();
                    items.Add(EtsyDAL.MakeEtsyItem(newItemB.Listing_Id));

                    JObject dataB2 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 135)}&category=furniture&keywords=" + "night%20stand", "active");
                    EtsyItem newItemB2 = new EtsyItem();
                    newItemB2.Listing_Id = dataB2["results"][randomoB]["listing_id"].ToString();
                    items.Add(EtsyDAL.MakeEtsyItem(newItemB2.Listing_Id));

                    JObject dataB3 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 2005)}&keywords=" + "lamp", "active");
                    EtsyItem newItemB3 = new EtsyItem();
                    newItemB3.Listing_Id = dataB3["results"][randomoB]["listing_id"].ToString();
                    items.Add(EtsyDAL.MakeEtsyItem(newItemB3.Listing_Id));

                    break;
                case "Dinning Room":
                    Random randomResultD = new Random();
                    int randomoD = randomResultD.Next(0, 25);

                    JObject dataD = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 52)}&category=furniture&keywords=" + "dinning%20table", "active");
                    EtsyItem newItemD = new EtsyItem();
                    newItemD.Listing_Id = dataD["results"][randomoD]["listing_id"].ToString();
                    items.Add(EtsyDAL.MakeEtsyItem(newItemD.Listing_Id));

                    JObject dataD2 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 123)}&category=furniture&keywords=" + "chair", "active");
                    EtsyItem newItemD2 = new EtsyItem();
                    newItemD2.Listing_Id = dataD2["results"][randomoD]["listing_id"].ToString();
                    items.Add(EtsyDAL.MakeEtsyItem(newItemD2.Listing_Id));

                    JObject dataD3 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 24)}&keywords=" + "clock", "active");
                    EtsyItem newItemD3 = new EtsyItem();
                    newItemD3.Listing_Id = dataD3["results"][randomoD]["listing_id"].ToString();
                    items.Add(EtsyDAL.MakeEtsyItem(newItemD3.Listing_Id));

                    break;
                case "Finished Basement":
                    Random randomResultF = new Random();
                    int randomoF = randomResultF.Next(0, 25);

                    JObject dataF = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 2005)}&keywords=" + "painting", "active");
                    EtsyItem newItemF = new EtsyItem();
                    newItemF.Listing_Id = dataF["results"][randomoF]["listing_id"].ToString();
                    items.Add(EtsyDAL.MakeEtsyItem(newItemF.Listing_Id));

                    JObject dataF2 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 123)}&category=furniture&keywords=" + "chair", "active");
                    EtsyItem newItemF2 = new EtsyItem();
                    newItemF2.Listing_Id = dataF2["results"][randomoF]["listing_id"].ToString();
                    items.Add(EtsyDAL.MakeEtsyItem(newItemF2.Listing_Id));

                    JObject dataF3 = EtsyDAL.GetEtsyAPI($"&page={randomPage.Next(0, 123)}&keywords=" + "lamp", "active");
                    EtsyItem newItemF3 = new EtsyItem();
                    newItemF3.Listing_Id = dataF3["results"][randomoF]["listing_id"].ToString();
                    items.Add(EtsyDAL.MakeEtsyItem(newItemF3.Listing_Id));

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

        public ActionResult AddFurn(string Listing_Id, string Title, string Url, string Price, string Currency_Code, string ImageThumbUrl, string ImageFullUrl, List<string> Left, List<string> Top)
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
            EtsyItem etsyItem = new EtsyItem();
            etsyItem.Listing_Id = Listing_Id;
            etsyItem = EtsyDAL.MakeEtsyItem(etsyItem.Listing_Id);
            thisSpace.furnList.Add(etsyItem);
            return RedirectToAction("Index");
        }

        public ActionResult SaveFurn(List<string> Listings, List<string> Left, List<string> Top, string Name)
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
            SavePositions(Left, Top);
            UserSpace userSpace = new UserSpace();
            userSpace.UserId = User.Identity.Name;
            userSpace.Name = Name;
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
            SavePositions(Left, Top);
            thisSpace.furnList.RemoveAt(int.Parse(Index));
            return RedirectToAction("Index");
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
    }
}