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
    
    public partial class ViewNotification
    {
        public int ID { get; set; }
        public string Notification { get; set; }
        public System.DateTime Date { get; set; }
        public string AttachName { get; set; }
        public int send_by { get; set; }
        public int SocietyID { get; set; }
        public System.DateTime EndDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> FlatID { get; set; }
        public Nullable<int> UserID { get; set; }
    }
}
