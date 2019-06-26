using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MyApttSocietyAPI.Models;
using System.Net.Http.Formatting;
using System.Web.Http.Cors;
using MyApttSocietyAPI;

namespace MyApttSocietyAPI.Controllers
{


    [EnableCors(origins: "*", headers: "*", methods: "*")]
   [RoutePrefix("api/Complaint")]
    public class ComplaintController : ApiController
    {
        // GET: api/Complaint
       [Route("All")]
        [HttpGet]
        public IEnumerable<ViewComplaintHistory> Get()
        {
          
            try
            {
                var context = new SocietyDBEntities();
                var Complaints = (from comp in context.ViewComplaintHistories
                                  orderby comp.ModifiedAt descending
                                  select comp).ToList();
                Log.log(" Get Complaint Results found are: Dhanajay" + DateTime.Now.ToString());
                return Complaints;
            }
            catch (Exception e)
            {

                throw;
            }
          
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

        [Route("Get/{status}/{societyid}/{flatnumber}/{pagenumber}/{count}")]
        [HttpGet]
        public IEnumerable<ViewComplaintSummary> GetFindData(string status, int societyid ,string flatnumber ,int pagenumber ,int count)
        {
            //var count = 10;
            try
            {
                String[] ints1 = new String[0];

                if (status == "Open")
                {
                    ints1 = new String[5];
                    ints1[0] = "New";
                    ints1[1] = "Assigned";
                    ints1[2] = "InProgress";
                    ints1[3] = "Complete";
                    ints1[4] = "Re-open";

                }
                else if (status == "Closed")
                {

                    ints1 = new String[1];
                    ints1[0] = "Closed";
                }
                else if (status == "All")
                {
                    ints1 = new String[6];
                    ints1[0] = "New";
                    ints1[1] = "Assigned";
                    ints1[2] = "InProgress";
                    ints1[3] = "Complete";
                    ints1[4] = "Closed";
                    ints1[5] = "Re-open";
                }


                var context = new SocietyDBEntities();


               
                var Complaints = (from comp in context.ViewComplaintSummaries
                                  where comp.FlatNumber == flatnumber && comp.SocietyID == societyid && ints1.Contains(comp.LastStatus)
                                  orderby comp.LastAt descending
                                  select comp).ToList();


                return Complaints.Skip((pagenumber-1)*count).Take(count);



            }
            catch (Exception ex)
            {
                Log.log(" Get Complaint has error at: " + DateTime.Now.ToString() + " " + ex.Message);
                return null;
            }

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
                    bool isNewComplaint = false;

                    String smsMessage = "";
                    var c = context.Complaints;

                    if (comp.CompID < 1)
                    {
                        isNewComplaint = true;
                        var maxID = c.Max(maxcomp => maxcomp.CompID);
                        comp.CompID = maxID + 1;
                        smsMessage = "Ticket No " + comp.CompID + " Updated to " + comp.CompStatusID;
                    }
                    else {
                        isNewComplaint = false;
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

                    var em = employee.First();

                    c.Add(new Complaint()
                    {
                        CompID = comp.CompID,
                        ResidentID = comp.UserID,
                        SocietyID = comp.SocietyID,
                        FlatNumber = comp.FlatNumber,
                        CompTypeID = comp.CompType,
                        SeverityID = comp.CompSeverity,
                        Descrption = comp.CompDescription,
                        Assignedto = em.ResID,
                        ModifiedAt = DateTime.Now.ToUniversalTime(),
                        CurrentStatus = comp.CompStatusID,

                    });
                    context.SaveChanges();

                    if (isNewComplaint)
                    {
                        smsMessage = "A New Complaint is assigned to you. Flat Number: " + comp.FlatNumber +
                            ", Ticket No: " + comp.CompID + ", Description: " + comp.CompDescription;

                        bool result =  Utility.NotifyEmployee(smsMessage, em.MobileNo);
                       
                    }
                    else
                    {
                        smsMessage = "Ticket No: " + comp.CompID + " is assigned to you, Flat : " + comp.FlatNumber + ", Description" + comp.CompDescription;
                       // bool result = Utility.NotifyEmployee(smsMessage, em.MobileNo);

                        string strSubject = "Ticket no: " + comp.CompID;
                        string residentMessage = "Your ticket no: " + comp.CompID + " is assigned to " + em.MobileNo;
                        ComplaintNotification notification = new ComplaintNotification(context, em.MobileNo);
                        notification.NotifyResidents(smsMessage, strSubject);
                    }

                    String resp = "{\"Response\":\"Ok\"}";
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                    return response;

                }
            }
            catch (Exception ex)
            {
                String resp = "{\"Response\":\"Fail\",\"Error\":\"" + ex.Message + "\"}";
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

        private void SendNotification()
        {

        }
    }
}
