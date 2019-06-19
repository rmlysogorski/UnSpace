using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnSpaceWebApp.Models
{
    public class MySpace
    {
        public spaceDimensions SpaceDimensions = new spaceDimensions();

        public List<EtsyItem> items = new List<EtsyItem>(); //Grabs the current page's items from the API

        public List<EtsyItem> furnList = new List<EtsyItem>();

        public class spaceDimensions
        {
            public string Width { get; set; }
            public string Length { get; set; }
            public string Measurement { get; set; }

        }

        
    }
}