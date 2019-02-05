using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MyApttSocietyAPI.Models;
using System.Net.Http.Formatting;
using System.Web.Http.Cors;

namespace MyApttSocietyAPI.Controllers
{


    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ComplaintController : ApiController
    {
        // GET: api/Complaint
        public IEnumerable<ViewComplaintHistory> Get()
        {
            var context = new SocietyDBEntities();
            var Complaints = (from comp in context.ViewComplaintHistories
                              orderby comp.ModifiedAt descending
                              select comp);
            Log.log(" Get Complaint Results found are: Dhanajay" + DateTime.Now.ToString());
            return Complaints;
        }

        // GET: api/Complaint/5
        public IEnumerable<ViewComplaintHistory> Get(int id)
        {
            var context = new SocietyDBEntities();

            var Complaints = (from comp in context.ViewComplaintHistories
                              where comp.CompID == id
                              select comp);
            Log.log(" Get Complaint History by ID Results found are:  " + Complaints.Count() +"  at " + DateTime.Now.ToString() + id.ToString() + "---" + Complaints.Count().ToString());
            return Complaints;
        }

        // POST: api/Complaint
         [HttpPost]
        public HttpResponseMessage Post([FromBody]MyApttSocietyAPI.Models.Complaint comp)
        {

            Log.log(" Complaint Post request received : " + DateTime.Now.ToString());

            try
            {
                using (var context = new SocietyDBEntities())
                {
                    var c = context.Complaints;

                    if (comp.CompID < 1)
                    {
                        var maxID = c.Max(maxcomp => maxcomp.CompID);
                        comp.CompID = maxID + 1;
                    }

                    var employee = (from emp in context.ViewSocietyUsers
                                    orderby emp.ServiceType == comp.CompType && emp.SocietyID == comp.SocietyID descending
                                    select emp);

                    if (employee.Count() == 0)
                    {
                      employee = (from emp in context.ViewSocietyUsers
                                  orderby emp.Type == "Admin" && emp.SocietyID == comp.SocietyID descending
                                        select emp);
                    }

                    c.Add(new Complaint()
                    {
                        CompID = comp.CompID,
                        ResidentID = comp.UserID,
                        SocietyID = comp.SocietyID,
                        FlatNumber = comp.FlatNumber,
                        CompTypeID = comp.CompType,
                        SeverityID = comp.CompSeverity,
                        Descrption = comp.CompDescription,
                        Assignedto = employee.First().ResID,
                        ModifiedAt = DateTime.Now.ToUniversalTime(),
                        CurrentStatus = comp.CompStatusID,

                    });
                    context.SaveChanges();
                    String resp = "{\"Response\":\"OK\"}";
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                    return response;

                }
            }
            catch (Exception ex)
            {
                String resp = "{\"Response\":\"FAIL\",\"Error\":\"" + ex.Message + "\"}";
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }

        }

        // PUT: api/Complaint/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Complaint/5
        public void Delete(int id)
        {
        }
    }
}
