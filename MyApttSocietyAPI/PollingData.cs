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
    
    public partial class PollingData
    {
        public int PollID { get; set; }
        public string Question { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Answer1Count { get; set; }
        public Nullable<int> Answer2Count { get; set; }
        public Nullable<int> Answer3Count { get; set; }
        public Nullable<int> Answer4Count { get; set; }
        public string Answer1String { get; set; }
        public string Answer2String { get; set; }
        public string Answer3String { get; set; }
        public string Answer4String { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public int SocietyID { get; set; }
    }
}