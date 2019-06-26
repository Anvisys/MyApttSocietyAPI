using System;
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
    [RoutePrefix("api/Vendor")]
    public class VendorController : ApiController
    {
        [Route("All")]
        [HttpGet]
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
                    tempVendor.ContactNumber = "11111111";
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

        [Route("Get/{SocietyID}/{Category}/{PageNumber}/{Count}")]
        [HttpGet]
        public IEnumerable<ViewVendor> Get(int SocietyID,String Category, int PageNumber, int Count)
        {   
            try
            {
                var context = new SocietyDBEntities();
                
                if (Category == "All")
                {
                  var   vendor = (from vend in context.ViewVendors
                                  where vend.SocietyID == SocietyID
                                  orderby vend.VendorName
                                  select vend);

                    return vendor.Skip((PageNumber - 1) * Count).Take(Count);
                }
                else {
                    var vendor = (from vend in context.ViewVendors
                                  where vend.SocietyID == SocietyID && vend.ShopCategory == Category
                                  orderby vend.VendorName
                                  select vend);

                    return vendor.Skip((PageNumber - 1) * Count).Take(Count);
                }
              


            }
            catch (Exception ex)
            {
                Log.log("Exception Occured at : " + DateTime.Now.ToString() + "----" + ex.Message);
                return null;
            }
        }



        [Route("Society/{SocID}/Date/{date}")]
        [HttpGet]
        public IEnumerable<Vendor> Get(String date, int SocID)
        {
            DateTime lastDateTime = DateTime.ParseExact(date, "dd/mm/yyyy HH:MM:SS", null);
            Log.log("Date found is  : " + lastDateTime.ToLongDateString());

            String sdate = "23/10/2016 4:05:00";

            DateTime newDateTime = Convert.ToDateTime(sdate);
            Log.log("new Date found is  : " + newDateTime.ToLongDateString());

            try
            {
                var context = new SocietyDBEntities();
                var vendor = (from vend in context.Vendors
                              where vend.Date >lastDateTime && vend.SocietyID == SocID
                              select vend
                             );

                if (vendor.Count() < 1)
                {
                    Vendor tempVendor = new Vendor();
                    tempVendor.ID = 9999;
                    tempVendor.Address = "No Data";
                    tempVendor.ContactNumber = "11111111";
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

        [Route("ByDate")]
        [HttpPost]
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
                              select new Shop() { ID = vend.ID, VendorName = vend.VendorName, ContactNum = vend.ContactNumber, Address = vend.Address, ShopCategory = vend.ShopCategory }
                             );
                               
                    return vendor;
  
            }
            catch (Exception ex)
            {
                Log.log("Exception Occured at : " + DateTime.Now.ToString() + "----" + ex.Message);
                return null;
            }
        }

        [Route("Images")]
        [HttpPost]
        public IEnumerable<ShopImage> Post([FromBody]Batch value)
        {
            var context = new SocietyDBEntities();

            var count = value.EndIndex - value.StartIndex;

            var vendor = (from vend in context.Vendors
                          where vend.SocietyID == value.SocietyID
                          orderby vend.Date descending
                          select new ShopImage() { ID = vend.ID, ImageString = vend.VendorIcon }
                             );

            var y = vendor.Skip(value.StartIndex).Take(count);

            return y;
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
