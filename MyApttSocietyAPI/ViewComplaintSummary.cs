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
    
    public partial class ViewComplaintSummary
    {
        public int FirstID { get; set; }
        public int SocietyID { get; set; }
        public int CompID { get; set; }
        public int ResidentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FlatNumber { get; set; }
        public Nullable<int> Age { get; set; }
        public string CompType { get; set; }
        public string InitialComment { get; set; }
        public int FirstStatusID { get; set; }
        public string FirstStatus { get; set; }
        public Nullable<System.DateTime> InitiatedAt { get; set; }
        public Nullable<int> StatusCount { get; set; }
        public string LastComment { get; set; }
        public int Assignedto { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeContact { get; set; }
        public string Type { get; set; }
        public int LastStatusID { get; set; }
        public string LastStatus { get; set; }
        public Nullable<System.DateTime> LastAt { get; set; }
    }
}
