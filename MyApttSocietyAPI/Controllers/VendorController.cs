﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

using MyApttSocietyAPI.Models;

namespace MyApttSocietyAPI.Controllers
{
      [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VendorController : ApiController
    {
        // GET: api/Vendor
        public IEnumerable<Vendor> Get()
        {
            try
            {
                var context = new SocietyDBEntities();
                var vendor = (from vend in context.Vendors
                              select vend);

                if (vendor.Count() < 1)
                {
                    Vendor tempVendor = new Vendor();
                    tempVendor.ID = 9999;
                    tempVendor.Address = "Nothing";
                    tempVendor.ContactNum = "11111111";
                    tempVendor.ShopCategory = "Water";
                    tempVendor.VendorName = "NA";
                    List<Vendor> temp = new List<Vendor> { tempVendor };
                    return temp.AsQueryable();
                }
                else {

                    return vendor;
                }

                
            }
            catch (Exception ex)
            {
                Log.log("Exception Occured at : " + DateTime.Now.ToString() + "----" + ex.Message);
                return null;
            }
        }

        // GET: api/Vendor/5
        public IEnumerable<Vendor> Get(String id)
        {
            DateTime lastDateTime = DateTime.ParseExact(id,"dd/mm/yyyy HH:MM:SS",null);
            Log.log("Date found is  : " + lastDateTime.ToLongDateString());

            String sdate = "23/10/2016 4:05:00";

            DateTime newDateTime = Convert.ToDateTime(sdate);
            Log.log("new Date found is  : " + newDateTime.ToLongDateString());

            try
            {
                var context = new SocietyDBEntities();
                var vendor = (from vend in context.Vendors
                              where vend.Date >lastDateTime
                              select vend
                             );

                if (vendor.Count() < 1)
                {
                    Vendor tempVendor = new Vendor();
                    tempVendor.ID = 9999;
                    tempVendor.Address = "No Data";
                    tempVendor.ContactNum = "11111111";
                    tempVendor.ShopCategory = "Water";
                    tempVendor.VendorName = "NA";
                    List<Vendor> temp = new List<Vendor> { tempVendor };
                    return temp.AsQueryable();
                }
                else
                {
                    return vendor;
                }


            }
            catch (Exception ex)
            {
                Log.log("Exception Occured at : " + DateTime.Now.ToString() + "----" + ex.Message);
                return null;
            }
        }

        // POST: api/Vendor
        public IEnumerable<Shop> Post([FromBody]DateTimeInput dateTime)
        {

            Log.log("Input Value is  : " + dateTime.value);

            DateTime lastDateTime;
            try
            {
                lastDateTime = DateTime.ParseExact(dateTime.value, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentUICulture);
                
                Log.log("Date found is  : " + lastDateTime.ToString());
            }
            catch (Exception ex)
            {
                Log.log("Could not parse String to Date time found is 1 : " );
                lastDateTime = DateTime.Now; }


            try
            {
                var context = new SocietyDBEntities();
                var vendor = (from vend in context.Vendors
                              where vend.Date > lastDateTime && vend.SocietyID == dateTime.SocietyID
                              select new Shop() { ID = vend.ID, VendorName = vend.VendorName, ContactNum = vend.ContactNum, Address = vend.Address, ShopCategory = vend.ShopCategory }
                             );
                               
                    return vendor;
  
            }
            catch (Exception ex)
            {
                Log.log("Exception Occured at : " + DateTime.Now.ToString() + "----" + ex.Message);
                return null;
            }
        }

        // PUT: api/Vendor/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Vendor/5
        public void Delete(int id)
        {
        }

       
    }
}
