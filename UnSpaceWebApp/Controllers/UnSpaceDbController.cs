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
            UserSpace result = ORM.UserSpaces.SingleOrDefault(u => u.UserId == userSpace.UserId && u.Name == userSpace.Name);
            if (result != null)
            {
                result.Listing = userSpace.Listing;
                result.QRCode = userSpace.QRCode;
                result.Name = userSpace.Name;
                result.SpaceDimensions = userSpace.SpaceDimensions;
                ORM.SaveChanges();
            }
            else
            {
                ORM.UserSpaces.Add(userSpace);
                try
                {
                    ORM.SaveChanges();
                }
                catch(DbEntityValidationException e)
                {
                    string error = e.Message;
                }
            }
            return RedirectToAction("Index", "Space");
        }

        public static List<UserSpace> GetUserSpaces(string Name)
        {
            List<UserSpace> userSpaces = new List<UserSpace>();
            userSpaces = ORM.UserSpaces.Where(s => s.UserId == Name).ToList();
            return userSpaces;
        }

        public static UserSpace GetThisUserSpace(string Id)
        {
            UserSpace thisSpace = new UserSpace();            
            thisSpace = ORM.UserSpaces.Find(int.Parse(Id));
            return thisSpace;
        }
    }
}