//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyApttSocietyAPI
{
    using System;
    using System.Collections.Generic;
    
    public partial class ViewLatestBillCycle
    {
        public int SocietyID { get; set; }
        public string FlatID { get; set; }
        public string FlatArea { get; set; }
        public int BillID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BillType { get; set; }
        public string Rate { get; set; }
        public string ChargeType { get; set; }
        public string CycleType { get; set; }
        public Nullable<System.DateTime> CycleStart { get; set; }
        public Nullable<System.DateTime> CycleEnd { get; set; }
        public System.DateTime created_date { get; set; }
        public string comments { get; set; }
    }
}