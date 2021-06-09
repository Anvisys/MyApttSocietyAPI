using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MyApttSocietyAPI.Models;
using System.Web.Http.Cors;
using System.Data.Entity.Validation;

namespace MyApttSocietyAPI.Controllers
{

     [EnableCors(origins: "*", headers: "*", methods: "*")]
     [RoutePrefix("api/Visitor")]
    public class VisitorController : ApiController
    {
        // GET: api/Guest
         [Route("Soc/{SocId}/{Page}/{Size}")]
         [HttpGet]
         public IEnumerable<viewVisitorData> Get(int SocId, int Page, int Size)
        {
            try
            {
                int skip = (Page - 1) * Size;
                var context = new NestinDBEntities();
                var Visitor = (from g in context.viewVisitorDatas
                               where g.SocietyId == SocId orderby g.RequestId descending
                             select g).Skip(skip).Take(Size);

                return Visitor;
            }
            catch (Exception ex)
            {
                Log.log(" Get Guest has error at: " + DateTime.Now.ToString() + " " + ex.Message);
                return null;
            }
        }

         // GET: api/Guest/5
         [Route("{SocId}/Mob/{Mobile}")]
         [HttpGet]
         public VisitorDetail Get(string Mobile, int SocId)
         {
             try
             {
                 var context = new NestinDBEntities();
                 var guest = (from g in context.VisitorDetails
                              where g.VisitorMobileNo == Mobile && g.SocietyId == SocId
                              select g).FirstOrDefault();

                 if (guest == null)
                 {

                     guest = new VisitorDetail
                     {
                         VisitorName = "NoUser",
                         VisitorAddress = "NoUser",
                         VisitorMobileNo = "NoUser",
                         id = 0
                     };
                 }
                 return guest;
             }
             catch (Exception ex)
             {
                 Log.log(" Get Guest has error at: " + DateTime.Now.ToString() + " " + ex.Message);
                 return null;
             }
         }

         // GET: api/Guest/5
         [Route("{SocId}/Res/{ResID}/{Page}/{Size}")]
         [HttpGet]
         public IEnumerable<viewVisitorData> GetByResID(int ResID, int SocId, int Page, int Size)
         {
             try
             {
                int skip = (Page - 1) * Size;
                var context = new NestinDBEntities();
                 var guest = (from g in context.viewVisitorDatas
                              where g.ResId == ResID && g.SocietyId == SocId
                              orderby g.RequestId descending
                              select g).Skip(skip).Take(Size);

                 return guest.ToList();
             }
             catch (Exception ex)
             {
                 Log.log(" Get Guest has error at: " + DateTime.Now.ToString() + " " + ex.Message);
                 return null;
             }
         }
          
        // GET: api/Guest/5
         [Route("Code")]
         [HttpPost]
        public viewVisitorData VerifyByCode([FromBody]Visitor value)
        {
            try
            {
                var context = new NestinDBEntities();
                var guest = (from g in context.viewVisitorDatas
                             where g.SecurityCode == value.VisitorCode && g.SocietyId == value.SocietyID
                             select g).FirstOrDefault();

                if (guest == null)
                {

                    guest = new viewVisitorData
                    {
                    VisitorName= "NoUser",
                    VisitorAddress="NoUser",
                    VisitorMobile="NoUser",
                    VisitorId=0
                    };
                }
                return guest;
            }
            catch (Exception ex)
            {
                Log.log(" Get Guest has error at: " + DateTime.Now.ToString() + " " + ex.Message);
                return null;
            }
        }

        // POST: api/Guest
         [Route("New")]
         [HttpPost]
        public HttpResponseMessage Post([FromBody]VisitorEntry value)
        {
            String resp = "{\"Response\":\"Undefine\"}";
            
            var   ctx = new NestinDBEntities();
            using (var dbContextTransaction = ctx.Database.BeginTransaction())
            {
                try
                {

                    Random r = new Random();
                    var code = 0;

                    do
                    {
                        code = r.Next(1000, 9999);
                    } while (IsCodeInUse(code.ToString()));

                    String mobile = value.VisitorMobile;
                    if (mobile.Length > 10)
                    {
                        mobile = mobile.Substring(mobile.Length - 10, mobile.Length);
                    }

                    if (value.VisitorId == 0)
                    {
                        VisitorDetail guest = new VisitorDetail();
                        guest.VisitorMobileNo = mobile;
                        guest.VisitorName = value.VisitorName;
                        guest.VisitorAddress = value.VisitorAddress;
                        guest.SocietyId = value.SocietyId;
                        guest.VisitorImage = value.VisitorImage;
                        var c = ctx.VisitorDetails;
                        c.Add(guest);
                        ctx.SaveChanges();
                        value.VisitorId = guest.id;

                    }

                    if (value.VisitorId > 0)
                    {
                        ctx.VisitorRequests.Add(new VisitorRequest
                        {
                            VisitorId = value.VisitorId,
                            VisitPurpose = value.VisitPurpose,
                            StartTime = DateTime.ParseExact(value.StartTime, "yyyy-MM-ddTHH:mm:ss.SSSZ", System.Globalization.CultureInfo.InvariantCulture),
                            EndTime = DateTime.ParseExact(value.EndTime, "yyyy-MM-ddTHH:mm:ss.SSSZ", System.Globalization.CultureInfo.InvariantCulture),

                            SecurityCode = code.ToString(),
                            SocietyId = value.SocietyId,
                            ResId = value.ResID,
                            Flat = value.FlatNumber
                        });

                    }

                    ctx.SaveChanges();

                    dbContextTransaction.Commit();
                    var strMessage = "Code for Entry in Flat : " + value.FlatNumber + " is " + code.ToString();
                    VisitorNotification visitorNotification = new VisitorNotification(ctx, value.HostMobile);
                    var result = visitorNotification.NotifyVisitor(strMessage, value.VisitorMobile);


                    resp = "{\"Response\":\"Ok\"}";
                }
                //Exception ex
                catch (DbEntityValidationException dbEx)
                {

                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {

                            Log.log("api/Profile Failed to Add User : Property-" + validationError.PropertyName + "  Error- " + validationError.ErrorMessage + "  At " + DateTime.Now.ToString());


                        }
                    }
                    dbContextTransaction.Rollback();
                    resp = "{\"Response\":\"Fail\"}";
                }
                catch (Exception ex)
                {
                   

                            Log.log("api/Profile Failed to Add User Error- " + ex.Message + "  At " + DateTime.Now.ToString());

                    dbContextTransaction.Rollback();
                    resp = "{\"Response\":\"Fail\"}";
                }
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
            return response;


        }

        // POST: api/Guest

        [Route("CheckIn")]
        [HttpPost]
        public HttpResponseMessage PostCheckIn([FromBody]VisitorEntry value)
        {
            String resp = "{\"Response\":\"Undefine\"}";
            var ctx = new NestinDBEntities();
         
                try
                {
                    var context = new NestinDBEntities();
                    VisitorRequest guest = (from g in context.VisitorRequests
                                 where g.id == value.RequestId
                                 select g).FirstOrDefault();

                    guest.ActualInTime = DateTime.Now.ToUniversalTime();
                    context.SaveChanges();
                    Message message = new Message();
                    message.Topic = "Visitor";
                    message.SocietyID = value.SocietyId;
                    message.TextMessage = "Your guest " + value.VisitorName + " has arrived.";
                    //VisitorNotification visitorNotification = new VisitorNotification(context, value.HostMobile);
                     Notifications msg = new Notifications(context);
                     msg.Notify(Notifications.TO.User, value.ResID, message);
                        
                    resp = "{\"Response\":\"OK\"}";
                }
                catch (Exception ex)
                {
                    resp = "{\"Response\":\"Fail\"}";
                }
            
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
            return response;


        }

        private bool IsCodeInUse(String code)
        {
            bool result = true;
            try
            { 
            var context = new NestinDBEntities();
            var guest = (from g in context.viewVisitorDatas
                         where g.SecurityCode == code && g.ActualInTime < g.StartTime
                         select g);

            if (guest.Count() == 0)
            {

                result = false;
            }


            }
            catch (Exception ex)
            {
                result = true;
            }

            return result;
        }

        // PUT: api/Guest/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Guest/5
        public void Delete(int id)
        {
        }
    }
}
