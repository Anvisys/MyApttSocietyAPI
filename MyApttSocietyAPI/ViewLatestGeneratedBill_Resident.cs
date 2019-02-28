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
    
    public partial class ViewLatestGeneratedBill_Resident
    {
        public int PayID { get; set; }
        public string FlatNumber { get; set; }
        public int SocietyID { get; set; }
        public Nullable<System.DateTime> BillStartDate { get; set; }
        public Nullable<System.DateTime> BillEndDate { get; set; }
        public int BillID { get; set; }
        public string BillType { get; set; }
        public Nullable<System.DateTime> BillDate { get; set; }
        public string Rate { get; set; }
        public string BillDescription { get; set; }
        public string ChargeType { get; set; }
        public Nullable<int> CurrentBillAmount { get; set; }
        public string CycleType { get; set; }
        public Nullable<System.DateTime> PaymentDueDate { get; set; }
        public System.DateTime BillMonth { get; set; }
        public Nullable<int> AmountTobePaid { get; set; }
        public Nullable<int> PreviousMonthBalance { get; set; }
        public Nullable<System.DateTime> AmountPaidDate { get; set; }
        public Nullable<int> AmountPaid { get; set; }
        public Nullable<int> CurrentMonthBalance { get; set; }
        public string PaymentMode { get; set; }
        public string TransactionID { get; set; }
        public string InvoiceID { get; set; }
        public Nullable<System.DateTime> ModifiedAt { get; set; }
        public string FlatArea { get; set; }
        public int FlatID { get; set; }
        public Nullable<int> OwnerUserID { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerMobile { get; set; }
        public Nullable<int> TenantUserID { get; set; }
        public string TenantFirstName { get; set; }
        public string TenantLastName { get; set; }
        public string TenantEmail { get; set; }
        public string TenantMobile { get; set; }
    }
}