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
    
    public partial class ViewVehiclePool
    {
        public int VehiclePoolID { get; set; }
        public bool OneWay { get; set; }
        public int PoolTypeID { get; set; }
        public string Destination { get; set; }
        public System.DateTime InitiatedDateTime { get; set; }
        public System.DateTime JourneyDateTime { get; set; }
        public System.DateTime ReturnDateTime { get; set; }
        public string VehicleType { get; set; }
        public int AvailableSeats { get; set; }
        public int SharedCost { get; set; }
        public string Description { get; set; }
        public int ResID { get; set; }
        public int SocietyID { get; set; }
        public bool Active { get; set; }
        public int DealStatus { get; set; }
        public int InterestedCount { get; set; }
        public int InterestedSeatsCount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FlatNumber { get; set; }
        public string MobileNo { get; set; }
    }
}