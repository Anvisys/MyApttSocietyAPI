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
    
    public partial class Resident
    {
        public int ResID { get; set; }
        public int UserID { get; set; }
        public string FlatID { get; set; }
        public string Type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public Nullable<System.DateTime> ActiveDate { get; set; }
        public Nullable<System.DateTime> DeActiveDate { get; set; }
        public string Addres { get; set; }
        public string Function { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int SocietyID { get; set; }
    }
}
