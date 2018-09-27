using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

using System.Data.Entity;
using MyApttSocietyAPI.Models;

namespace MyApttSocietyAPI.Controllers
{

     [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BillingController : ApiController
    {

         private Object thisLock = new Object();

        // GET: api/Billing
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Billing/5
        public IEnumerable<ViewLatestGeneratedBill> Get(String id)
        {
            try
            {
                DateTime date = System.DateTime.Now;
                String dueFor = String.Format("{0:MMMM, yyyy}", date);
                   Log.log("Calculated for: " + dueFor + " at " + DateTime.Now.ToString());

                   var context = new SocietyDBEntities();
                    var L2EQuery = context.ViewLatestGeneratedBills.Where(gb => gb.FlatNumber == id);

                    Log.log(" ViewBilling by Flat Number Results found are: " + DateTime.Now.ToString());
                    return L2EQuery;
              
            }
            catch (Exception ex)
            {
                Log.log(" Get thread by ID has error at: " + DateTime.Now.ToString() + " " + ex.Message);
                return null;
            }
        }

        // POST: api/Billing
        public HttpResponseMessage Post([FromBody]Billing value)
        {
            try
            {
                lock (thisLock)
                {
                    using (var context = new SocietyDBEntities())
                    {
                        var c = context.PollingDatas;

                        List<GeneratedBill> Bills = (from bill in context.GeneratedBills
                                                   where bill.PayID == value.PayID
                                                   select bill).ToList();

                        Log.log(" Poll for given ID : " + Bills.Count);
                        foreach (GeneratedBill b in Bills)
                        {
                            if ((b.AmountPaid == 0) && b.AmountPaidDate == null)
                            {
                                b.AmountPaid = value.PaidAmount;
                                b.AmountPaidDate = DateTime.Now;
                                b.TransactionID = value.TransactionID;
                                b.InvoiceID = value.PayID.ToString();
                                b.PaymentMode = value.PaymentMode;
                            }

                        }

                        context.SaveChanges();

                        Log.log(" Payment Data Updated : " + DateTime.Now.ToString());

                        String resp = "{\"Response\":\"OK\"}";
                        var response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                        return response;

                    }
                }
            }
            catch (Exception ex)
            {
                Log.log(" Payment Data Updated error : " + ex.Message);
                String resp = "{\"Response\":\"FAIL\",\"Error\":\"" + ex.Message + "\"}";
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }


        }

        // PUT: api/Billing/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Billing/5
        public void Delete(int id)
        {
        }
    }
}
