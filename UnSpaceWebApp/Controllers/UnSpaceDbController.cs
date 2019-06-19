using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnSpaceWebApp.Models;

namespace UnSpaceWebApp.Controllers
{
    public class UnSpaceDbController : Controller
    {
        private static UnSpaceDb ORM = new UnSpaceDb();
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
            UserSpace result = ORM.UserSpaces.SingleOrDefault(u => u.UserId == userSpace.UserId && u.Listing == userSpace.Listing);
            if (result != null)
            {
                result.Listing = userSpace.Listing;
                result.QRCode = userSpace.QRCode;
                ORM.SaveChanges();
            }
            else
            {
                ORM.UserSpaces.Add(userSpace);
                ORM.SaveChanges();
            }
            return RedirectToAction("Index", "Space");
        }

        public List<UserSpace> GetUserSpaces()
        {
            List<UserSpace> userSpaces = new List<UserSpace>();
            userSpaces = ORM.UserSpaces.Where(s => s.UserId == User.Identity.Name).ToList();            
            return userSpaces;
        }
    }
}