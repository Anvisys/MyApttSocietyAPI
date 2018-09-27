using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public class Shop
    {


        public int ID { get; set; }
        public String VendorName { get; set; }

        public String ContactNum { get; set; }
        public String Address { get; set; }

        public String ShopCategory { get; set; }
    }
}