using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public class DifferentialInput
    {
        public String ComplaintRefreshTime { get; set; }
     
        public String ForumRefreshTime { get; set; }
     
        public String VendorRefreshTime { get; set; }
     
        public String NoticeRefreshTime { get; set; }
     
        public String PollRefreshTime { get; set; }
     
        public String BillRefreshTime { get; set; }
     
        public int resID { get; set; }

        public String flatNo { get; set; }
    }
}