using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MyApttSocietyAPI.Models;

namespace MyApttSocietyAPI.Controllers
{
    public class BatchVendorController : ApiController
    {
        // GET: api/BatchVendor
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/BatchVendor/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/BatchVendor
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

        // PUT: api/BatchVendor/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/BatchVendor/5
        public void Delete(int id)
        {
        }
    }
}
