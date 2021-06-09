using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MyApttSocietyAPI.Models;


namespace MyApttSocietyAPI.Controllers
{
    public class DomainController : ApiController
    {

        NestinDBEntities context;
        public DomainController()
        {
            context = new NestinDBEntities();
        }


        // GET: api/Domain
        public DomainInfo Get()
        {
            DomainInfo di = new DomainInfo();

            try
            {
                using (context)
                {
                    //get comp types
                    var ctype = from c in context.lukComplaintTypes
                                select new Domain() { ID = c.CompTypeID, Value = c.CompType };
                    if (ctype != null && ctype.Any())
                        di.ComplaintType = ctype.ToList();

                    //get comp status
                    var cStatus = from c in context.lukComplaintStatus
                                  select new Domain() { ID = c.StatusID, Value = c.CompStatus };
                    if (cStatus != null && cStatus.Any())
                        di.ComplaintStatus = cStatus.ToList();

                    //get comp status
                    var cSev = from c in context.lukComplaintSeverities
                               select new Domain() { ID = c.SeverityID, Value = c.Severity };
                    if (cSev != null && cSev.Any())
                        di.Severity = cSev.ToList();


                    //get Vendor Category
                    var cVendorCategory = from v in context.lukVendorCategories
                               select new Domain() { ID = v.ID, Value = v.ShopCategory };
                    if (cVendorCategory != null && cVendorCategory.Any())
                        di.VendorCategory = cVendorCategory.ToList();

                }

                Log.log(" Domain Info Returned : Type -" + di.ComplaintType.Count.ToString() + "-- at" + DateTime.Now.ToString());
                return di;
            }
            catch (Exception ex)
            {
                Log.log(" Domain Info returned Exception -" + ex.Message + "-- at" + DateTime.Now.ToString());
                return null;
            }
        }

        // GET: api/Domain/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Domain
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Domain/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Domain/5
        public void Delete(int id)
        {
        }
    }
}
