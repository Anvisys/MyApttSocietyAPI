using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public class InventoryAccomodation
    {
        public int InventoryId { get; set; }
        public string Inventory { get; set; }
        public int AccomodationId { get; set; }
        public string Accomodation { get; set; }

    }
}