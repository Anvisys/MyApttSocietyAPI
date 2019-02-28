using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public class VisitorEntry
    {
        public int RequestId { get; set; }
        public int SocietyId { get; set; }
        public int VisitorId { get; set; }
        public string VisitorMobile { get; set; }
        public string VisitorName { get; set; }
        public byte[] VisitorImage { get; set; }
        public string VisitorAddress { get; set; }
        public string VisitPurpose { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }

        public int ResID { get; set; }
        public String FlatNumber { get; set; }
        public String HostMobile { get; set; }

    }
}