using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public class Update
    {
      
        public int ComplaintCount { get; set; }

        public int TotalComplaintCount { get; set; }

        public int ForumCount { get; set; }

        public int TotalForumCount { get; set; }

        public int VendorCount { get; set; }

        public int TotalVendorCount { get; set; }

        public int NoticeCount { get; set; }

        public int TotalNoticeCount { get; set; }

        public int PollCount { get; set; }

        public int TotalPollCount { get; set; }

        public int BillCount { get; set; }

        public int TotalBillCount { get; set; }

    }
}