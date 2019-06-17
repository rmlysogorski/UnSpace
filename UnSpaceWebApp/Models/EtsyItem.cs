using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnSpaceWebApp.Models
{
    public class EtsyItem
    {
        public string listing_id;
        public string title;
        public string price;
        public string currency_code;
        public string item_length;
        public string item_width;
        public string item_height;
        public string item_dimensions_unit;
        public string url;

        public string Listing_Id{ set; get;}
        public string Title { set; get; }
        public string Price { set; get; }
        public string Currency_Code { set; get; }
        public string Item_Length { set; get; }
        public string Item_Width { set; get; }
        public string Item_Height { set; get; }
        public string Item_Dimensions_unit { set; get; }
        public string Url { set; get; }

    }
}