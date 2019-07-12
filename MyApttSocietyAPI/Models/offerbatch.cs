using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public class offerbatch
    {
        int VendorID { get; set; }
        string Offers { get; set; }
        DateTime StartDate{ get; set; }
        DateTime EndDate { get; set; }
       int SocietyID { get; set; }
    }
}