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
    
    public partial class Vendor
    {
        public int ID { get; set; }
        public string ShopCategory { get; set; }
        public string VendorName { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public byte[] VendorIcon { get; set; }
        public string VendorIconFormat { get; set; }
        public string CmdType { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public int SocietyID { get; set; }
        public string ContactNumber2 { get; set; }
        public string Address2 { get; set; }
    }
}
