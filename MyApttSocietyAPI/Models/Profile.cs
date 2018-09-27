using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public class Profile
    {
        public int UserID { get; set; }
        public String UserName { get; set; }

        public int ResID { get; set; }
        public String MobileNumber { get; set; }

        public String Email { get; set; }

        public String GCMCode { get; set; }

        public String ImageString { get; set; }

        public String Location { get; set; }

      
    }
}