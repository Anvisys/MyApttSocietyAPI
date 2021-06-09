using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyApttSocietyAPI.Models;

namespace MyApttSocietyAPI.Controllers
{
    public class ComplaintDiffController : ApiController
    {
        // GET: api/ComplaintDiff
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ComplaintDiff/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ComplaintDiff
        public IEnumerable<ViewComplaintSummary> Post([FromBody]Batch value)
        {
             try
            {
                String[] ints1 = new String[0];

                if (value.CompStatus == "Open" )
                {
                    ints1 = new String[5];
                    ints1[0] = "New";
                    ints1[1] = "Assigned";
                    ints1[2] = "InProgress";
                    ints1[3] = "Complete";
                    ints1[4] = "Re-open";

                }
                else if (value.CompStatus == "Closed")
                {

                    ints1 = new String[1];
                    ints1[0] = "Closed";
                }
                else if (value.CompStatus == "All")
                {
                    ints1 = new String[6];
                    ints1[0] = "New";
                    ints1[1] = "Assigned";
                    ints1[2] = "InProgress";
                    ints1[3] = "Complete";
                    ints1[4] = "Closed";
                    ints1[5] = "Re-open";
                }
               

                var context = new NestinDBEntities();
               
                
                    var count = value.EndIndex - value.StartIndex;
                    var Complaints = (from comp in context.ViewComplaintSummaries
                                      where comp.FlatNumber == value.FlatNumber && comp.SocietyID== value.SocietyID && ints1.Contains(comp.LastStatus)
                                      orderby comp.LastAt descending
                                      select comp);


                    return Complaints.Skip(value.StartIndex).Take(count);
              
              /*  else
                {
                    DateTime ComplaintDateTime = DateTime.ParseExact(value.LastRefreshTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentUICulture);
                    return (from comp in context.ViewComplaintSummaries
                            where comp.ResidentID == value.ResId  && comp.LastAt > ComplaintDateTime
                                             select comp).Take(10);
                }*/

              
            }
            catch (Exception ex)
            {
                Log.log(" Get Complaint has error at: " + DateTime.Now.ToString() + " " + ex.Message);
                return null;
            }      

        }

        // PUT: api/ComplaintDiff/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ComplaintDiff/5
        public void Delete(int id)
        {
        }
    }
}
