﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SocietyDBEntities : DbContext
    {
        public SocietyDBEntities()
            : base("name=SocietyDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Complaint> Complaints { get; set; }
        public virtual DbSet<Forum> Fora { get; set; }
        public virtual DbSet<GCMList> GCMLists { get; set; }
        public virtual DbSet<GeneratedBill> GeneratedBills { get; set; }
        public virtual DbSet<PollingAnswer> PollingAnswers { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<BillCycle> BillCycles { get; set; }
        public virtual DbSet<ImageCheck> ImageChecks { get; set; }
        public virtual DbSet<Societybillplan> Societybillplans { get; set; }
        public virtual DbSet<ViewComplaintAge> ViewComplaintAges { get; set; }
        public virtual DbSet<ViewFirstThreadwithCount> ViewFirstThreadwithCounts { get; set; }
        public virtual DbSet<ViewLatestGeneratedBill> ViewLatestGeneratedBills { get; set; }
        public virtual DbSet<ViewLatestThread> ViewLatestThreads { get; set; }
        public virtual DbSet<ViewPollCount> ViewPollCounts { get; set; }
        public virtual DbSet<WatchViewFirstThreadNoImage> WatchViewFirstThreadNoImages { get; set; }
        public virtual DbSet<PollingData> PollingDatas { get; set; }
        public virtual DbSet<ViewPollDataWithCount> ViewPollDataWithCounts { get; set; }
        public virtual DbSet<lukBillType> lukBillTypes { get; set; }
        public virtual DbSet<lukComplaintSeverity> lukComplaintSeverities { get; set; }
        public virtual DbSet<lukComplaintStatu> lukComplaintStatus { get; set; }
        public virtual DbSet<lukComplaintType> lukComplaintTypes { get; set; }
        public virtual DbSet<lukVendorCategory> lukVendorCategories { get; set; }
        public virtual DbSet<ViewComplaintHistory> ViewComplaintHistories { get; set; }
        public virtual DbSet<ViewComplaintInitiated> ViewComplaintInitiateds { get; set; }
        public virtual DbSet<ViewComplaintLatest> ViewComplaintLatests { get; set; }
        public virtual DbSet<Advertisement> Advertisements { get; set; }
        public virtual DbSet<VisitorDetail> VisitorDetails { get; set; }
        public virtual DbSet<VisitorRequest> VisitorRequests { get; set; }
        public virtual DbSet<ViewGeneratedBill> ViewGeneratedBills { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<ViewComplaintSummary> ViewComplaintSummaries { get; set; }
        public virtual DbSet<ViewLatestBillCycle> ViewLatestBillCycles { get; set; }
        public virtual DbSet<viewVisitorData> viewVisitorDatas { get; set; }
        public virtual DbSet<ViewLatestGeneratedBill_Resident> ViewLatestGeneratedBill_Resident { get; set; }
        public virtual DbSet<ViewFlat> ViewFlats { get; set; }
        public virtual DbSet<ViewUser> ViewUsers { get; set; }
        public virtual DbSet<ViewForumNoImage> ViewForumNoImages { get; set; }
        public virtual DbSet<ViewNotification> ViewNotifications { get; set; }
        public virtual DbSet<ViewThreadSummaryNoImageCount> ViewThreadSummaryNoImageCounts { get; set; }
        public virtual DbSet<Flat> Flats { get; set; }
        public virtual DbSet<lukInventory> lukInventories { get; set; }
        public virtual DbSet<lukRentType> lukRentTypes { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<RentInventory> RentInventories { get; set; }
        public virtual DbSet<Society> Societies { get; set; }
        public virtual DbSet<SocietyUser> SocietyUsers { get; set; }
        public virtual DbSet<TotalUser> TotalUsers { get; set; }
        public virtual DbSet<UserImage> UserImages { get; set; }
        public virtual DbSet<UserSetting> UserSettings { get; set; }
        public virtual DbSet<ViewOffer> ViewOffers { get; set; }
        public virtual DbSet<ViewRentInventory> ViewRentInventories { get; set; }
        public virtual DbSet<ViewSocietyUser> ViewSocietyUsers { get; set; }
        public virtual DbSet<ViewUserImage> ViewUserImages { get; set; }
        public virtual DbSet<ViewUserSetting> ViewUserSettings { get; set; }
        public virtual DbSet<House> Houses { get; set; }
    }
}
