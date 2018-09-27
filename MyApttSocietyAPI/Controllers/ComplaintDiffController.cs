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
                
                var context = new SocietyDBEntities();
               
                if (value.LastRefreshTime == "")
                {
                    var count = value.EndIndex - value.StartIndex;
                    var Complaints = (from comp in context.ViewComplaintSummaries
                                      where comp.ResidentID == value.ResId
                                      orderby comp.LastAt descending
                                      select comp);


                    return Complaints.Skip(value.StartIndex).Take(count);
                }
                else
                {
                    DateTime ComplaintDateTime = DateTime.ParseExact(value.LastRefreshTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentUICulture);
                    return (from comp in context.ViewComplaintSummaries
                                             where comp.ResidentID == value.ResId && comp.LastAt > ComplaintDateTime
                                             select comp).Take(10);
                }

              
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
