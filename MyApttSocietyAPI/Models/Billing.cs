using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public class Billing
    {

        public String ResID { get; set; }

        public String BillType { get; set; }

        public int PaidAmount { get; set; }

        public int PayID { get; set; }

        public String TransactionID { get; set; }

        public String InvoiceID { get; set; }

        public String PaymentMode { get; set; }
    }
}