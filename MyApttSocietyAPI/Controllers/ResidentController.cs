using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.Entity;
using System.Data.Objects;

using MyApttSocietyAPI.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace MyApttSocietyAPI.Controllers
{
     [EnableCors(origins: "*", headers: "*", methods: "*")]
     [RoutePrefix("api/Resident")]
    public class ResidentController : ApiController
    {
        // GET: api/Resident
         [Route("All")]
         [HttpGet]
        public IEnumerable<ViewSocietyUser> Get()
        {
            var context = new SocietyDBEntities();
            var Residents = (from res in context.ViewSocietyUsers
                             where (res.DeActiveDate == null) || (DbFunctions.TruncateTime(res.DeActiveDate) > DbFunctions.TruncateTime(DateTime.UtcNow))
                             select res);
            return Residents;
        }

        // GET: api/Resident/5
         [Route("User/{id}")]
         [HttpGet]
         public IEnumerable<ViewSocietyUser> GetByUserID(int id)
        {
            try
            {
                    var context = new SocietyDBEntities();
                    var Residents = (from res in context.ViewSocietyUsers
                                     where ((res.DeActiveDate == null) || (DbFunctions.TruncateTime(res.DeActiveDate) > DbFunctions.TruncateTime(DateTime.UtcNow))) && res.UserID ==id
                                     select res);
                    return Residents;
                
            }
            catch (Exception ex)
            {
                Log.log(" ViewResident by ID Results found are: " + DateTime.Now.ToString());
                return null;
            }
        }


        // GET: api/Resident/5
         [Route("Mobile/{mobile}")]
         [HttpGet]
         public ViewSocietyUser GetByMobile(String mobile)
        {
            try
            {
                using (var context = new SocietyDBEntities())
                {
                    var L2EQuery = context.ViewSocietyUsers.Where(u => u.MobileNo == mobile);
                    var user = L2EQuery.FirstOrDefault<ViewSocietyUser>();
                     return user;
                }
            }
            catch (Exception ex)
            {
                Log.log(" ViewResident by ID Results found are: " + DateTime.Now.ToString());
                return null;
            }
        }

        // GET: api/Resident/5
         [Route("Email/{email}")]
         [HttpGet]
         public ViewResident GetByEmail(String email)
        {
            try
            {
                using (var context = new SocietyDBEntities())
                {
                    var L2EQuery = context.ViewResidents.Where(u => u.EmailId.ToLower() == email.ToLower());
                    var user = L2EQuery.FirstOrDefault<ViewResident>();
                    return user;
                }
            }
            catch (Exception ex)
            {
                Log.log(" ViewResident by ID Results found are: " + DateTime.Now.ToString());
                return null;
            }
        }

       
        // POST: api/Resident
         [Route("Update")]
         [HttpPost]
        public HttpResponseMessage Post([FromBody]Profile value)
        {
            String resp;
            try
            {
                using (var context = new SocietyDBEntities())
                {
                    var usr = context.Residents;
                    List<Resident> users = (from u in context.Residents
                                            where u.UserID == value.UserID
                                            select u).ToList();

                    foreach (Resident user in users)
                    {
                        user.FirstName = value.UserName;
                        user.EmailId = value.Email;
                        user.Addres = value.Location;
                        user.Function = "EDIT";
                    }

                    context.SaveChanges();
                    resp = "{\"Response\":\"OK\"}";
                }

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }
            
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {

                        Log.log("api/Profile Failed to Edit : Property-" + validationError.PropertyName + "  Error- " + validationError.ErrorMessage + "  At " + DateTime.Now.ToString());


                    }
                }
                resp = "{\"Response\":\"Fail\"}";
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }
        }

        [Route("AddResident")]
        [HttpPost]
        // POST: api/Resident/AddResident
        public HttpResponseMessage AddResident([FromBody]SocietyUser Res)
        {
            String resp;
            try
            {
                using (var context = new SocietyDBEntities())
                {
                    var usr = context.SocietyUsers;
                    //usr.Add(new SocietyUser()
                    //{
                    //    UserID = Res.UserID,
                    //    FlatID = Res.FlatID,
                    //    Type = Res.Type,
                    //    CompanyName = Res.CompanyName,
                    //    ServiceType = Res.ServiceType,
                    //    ActiveDate = Res.ActiveDate,
                    //    SocietyID = Res.SocietyID,
         
                    //    ModifiedDate = DateTime.Now.ToUniversalTime()          
                    //});

                    usr.Add(Res);
                    context.SaveChanges();

                    context.SaveChanges();
                    resp = "{\"Response\":\"OK\"}";
                }

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }

            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {

                        Log.log("api/Profile Failed to Add User : Property-" + validationError.PropertyName + "  Error- " + validationError.ErrorMessage + "  At " + DateTime.Now.ToString());


                    }
                }
                resp = "{\"Response\":\"Fail\"}";
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }
        }

        // PUT: api/Resident/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Resident/5
        public void Delete(int id)
        {
        }
    }
}
