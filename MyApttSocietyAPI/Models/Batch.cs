using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public class Batch
    {

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public String CompStatus { get; set; }

        public int ResId { get; set; }

        public String LastRefreshTime { get; set; }

        public int SocietyID { get; set; }

        public String FlatNumber { get; set; }

        public int PageNumber { get; set; }

        
    }
}