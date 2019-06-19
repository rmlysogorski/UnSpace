using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UnSpaceWebApp.Models;

namespace UnSpaceWebApp.Controllers
{
    
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QRCode(string incomingUrl)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(incomingUrl, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            qrCodeImage.Save(Response.OutputStream, ImageFormat.Gif);
            ViewBag.Image = qrCodeImage;
            return View();
        }
        public ActionResult ListSpaces()
        {
            MySpace fakedata1 = new MySpace();
            EtsyItem fakefurn1 = new EtsyItem();
            fakefurn1.Listing_Id = "621428887";
            fakefurn1.Title = "Heritage Edition Antique 1930&#39";
            fakefurn1.Price = "$550.00";
            fakefurn1.Item_Width = "12";
            fakefurn1.Item_Length = "40";
            fakefurn1.Item_Height = "15";
            fakefurn1.Item_Dimensions_unit = "";
            fakefurn1.ImageThumbUrl = "";
            fakefurn1.ImageFullUrl = $"https:www.etsy.com/listing/621428887/heritage-edition-antique-1930s-theater?";
            fakedata1.furnList.Add(fakefurn1);
            List<MySpace> newDummy = new List<MySpace>();
            newDummy.Add(fakedata1);
            return View(newDummy);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}