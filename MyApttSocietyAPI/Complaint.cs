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
    
    public partial class Complaint
    {
        public int ID { get; set; }
        public int CompID { get; set; }
        public string Descrption { get; set; }
        public int Assignedto { get; set; }
        public int CurrentStatus { get; set; }
        public int CompTypeID { get; set; }
        public int SeverityID { get; set; }
        public int ResidentID { get; set; }
        public System.DateTime ModifiedAt { get; set; }
        public string FlatNumber { get; set; }
        public int SocietyID { get; set; }
    }
}
