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

        public int ResId { get; set; }

        public String LastRefreshTime { get; set; }

        public int SocietyID { get; set; }
    }
}