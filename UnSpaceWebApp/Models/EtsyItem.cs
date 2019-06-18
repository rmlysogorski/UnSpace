using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnSpaceWebApp.Models
{
    public class EtsyItem
    {
        public string Listing_Id{ set; get;}
        public string Title { set; get; }
        public string Price { set; get; }
        public string Currency_Code { set; get; }
        public string Item_Length { set; get; }
        public string Item_Width { set; get; }
        public string Item_Height { set; get; }
        public string Item_Dimensions_unit { set; get; }
        public string Url { set; get; }
        public string ImageThumbUrl { set; get; }
        public string ImageFullUrl { set; get; }

    }
}