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
        // GET: Space
        public ActionResult Index()
        {
            return View();
        }  
    }
}