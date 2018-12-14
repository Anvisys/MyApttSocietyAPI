using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public class Poll
    {
        public int PollID { get; set; }
        public int SocietyId { get; set; }

        public int ResID { get; set; }

        public int selectedAnswer { get; set; }

        public int previousSelected { get; set; }

        public String Question { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public String Answer1 { get; set; }
        public String Answer2 { get; set; }
        public String Answer3 { get; set; }
        public String Answer4 { get; set; }

        public int Answer1Count { get; set; }
        public int Answer2Count { get; set; }
        public int Answer3Count { get; set; }
        public int Answer4Count { get; set; }

    }
}