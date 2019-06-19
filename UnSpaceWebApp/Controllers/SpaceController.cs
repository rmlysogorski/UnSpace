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

        public ActionResult GenerateSpace(string Width, string Length, string Measurement)
        {
            thisSpace.SpaceDimensions.Width = Width;
            thisSpace.SpaceDimensions.Length = Length;
            thisSpace.SpaceDimensions.Measurement = Measurement;
            return View("Index", thisSpace);
        }
    }
}