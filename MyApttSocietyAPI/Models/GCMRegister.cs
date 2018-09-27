using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public class GCMRegister
    {
        public String MobileNo { get; set; }

        public String DeviceID { get; set; }

        public String RegID { get; set; }

        public String Topic { get; set; }
    }
}