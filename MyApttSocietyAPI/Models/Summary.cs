using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public class Summary
    {
        public int Total_Comp { get; set; }
        public int InProg_Comp { get; set; }
        public int Resolved_Comp { get; set; }

        public int New_Comp { get; set; }

        public String vendor_1st { get; set; }
        public String vendor_2nd { get; set; }

        public String forum_1 { get; set; }
        public String forum_2 { get; set; }
        public String forum_3 { get; set; }

        public int today_discussion { get; set; }

        public String notice_1 { get; set; }
        public String notice_2 { get; set; }
        public String notice_3 { get; set; }

        public int today_Notice { get; set; }
    }
}