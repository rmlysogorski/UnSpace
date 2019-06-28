using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnSpaceWebApp.Models;

namespace UnSpaceWebApp.Controllers
{
    public class UnSpaceDbController : Controller
    {
        private static UnSpaceDbEntities ORM = new UnSpaceDbEntities();
        // GET: UnSpaceDb
        public ActionResult SaveUserSpace(UserSpace userSpace)
        {
            List<EtsyItem> furnList = new List<EtsyItem>();
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
            if (TempData["furnList"] != null)
            {
                furnList = (List<EtsyItem>)TempData["furnList"];
            }
            UserSpace result = ORM.UserSpaces.SingleOrDefault(u => u.UserId == userSpace.UserId && u.Name == userSpace.Name);
            if (result != null)
            {
                result.Listing = userSpace.Listing;
                result.QRCode = userSpace.QRCode;
                result.Name = userSpace.Name;
                result.SpaceDimensions = userSpace.SpaceDimensions;
                result.Positions = userSpace.Positions;
                try
                {
                    ORM.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    string error = e.Message;
                }
            }
            else
            {
                ORM.UserSpaces.Add(userSpace);
                try
                {
                    ORM.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    string error = e.Message;
                }
            }
            foreach (EtsyItem e in furnList)
            {
                List<EtsyItemDb> res = ORM.EtsyItemDbs.Where(r=>r.Listing_Id == e.Listing_Id).ToList();
                if (res.Count == 0)
                {
                    EtsyItemDb dbItem = new EtsyItemDb();
                    dbItem.Listing_Id = e.Listing_Id;
                    dbItem.Title = e.Title;
                    dbItem.Price = e.Price;
                    dbItem.Currency_Code = e.Currency_Code;
                    dbItem.Item_Length = e.Item_Length;
                    dbItem.Item_Width = e.Item_Width;
                    dbItem.Item_Height = e.Item_Height;
                    dbItem.Item_Dimensions_Unit = e.Item_Dimensions_unit;
                    dbItem.Url = e.Url;
                    dbItem.ImageThumbUrl = e.ImageThumbUrl;
                    dbItem.ImageFullUrl = e.ImageFullUrl;
                    dbItem.Description = e.Description;
                    ORM.EtsyItemDbs.Add(dbItem);
                    try
                    {
                        ORM.SaveChanges();
                    }
                    catch (DbEntityValidationException err)
                    {
                        string error = err.Message;
                    }
                }          
                
            }
           
            return RedirectToAction("Index", "Space");
        }

        public ActionResult RemoveUserSpace(int Id)
        {
            UserSpace userSpace = ORM.UserSpaces.Find(Id);
            ORM.UserSpaces.Remove(userSpace);
            ORM.SaveChanges();
            return RedirectToAction("Favorites", "Home");
        }
        public static List<UserSpace> GetUserSpaces(string Name)
        {
            List<UserSpace> userSpaces = new List<UserSpace>();
            userSpaces = ORM.UserSpaces.Where(s => s.UserId == Name).ToList();
            return userSpaces;
        }

        public static UserSpace GetThisUserSpace(string Id)
        {            
            return ORM.UserSpaces.Find(int.Parse(Id));
        }

        public static EtsyItem ConvertEtsyItemDb(string listing)
        {
            List<EtsyItemDb> res = ORM.EtsyItemDbs.Where(r => r.Listing_Id == listing).ToList();
            if(res.Count > 0)
            {
                EtsyItemDb e = res[0];
                EtsyItem etsyItem = new EtsyItem();
                etsyItem.Listing_Id = e.Listing_Id;
                etsyItem.Title = e.Title;
                etsyItem.Price = e.Price;
                etsyItem.Currency_Code = e.Currency_Code;
                etsyItem.Item_Length = e.Item_Length;
                etsyItem.Item_Width = e.Item_Width;
                etsyItem.Item_Height = e.Item_Height;
                etsyItem.Item_Dimensions_unit = e.Item_Dimensions_Unit;
                etsyItem.Url = e.Url;
                etsyItem.ImageThumbUrl = e.ImageThumbUrl;
                etsyItem.ImageFullUrl = e.ImageFullUrl;
                etsyItem.Description = e.Description;
                return etsyItem;
            }
            else
            {
                return new EtsyItem();
            }
            
        }
    }
}