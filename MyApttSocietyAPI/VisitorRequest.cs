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
    
    public partial class VisitorRequest
    {
        public int id { get; set; }
        public int VisitorId { get; set; }
        public string VisitPurpose { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public string SecurityCode { get; set; }
        public Nullable<System.DateTime> ActualInTime { get; set; }
        public Nullable<System.DateTime> ActualOutTime { get; set; }
        public string Flat { get; set; }
        public int ResId { get; set; }
        public Nullable<int> SocietyId { get; set; }
    }
}
