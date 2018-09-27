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
     [Route("api/[controller]/[action]")]
    public class ResidentController : ApiController
    {
        // GET: api/Resident
        public IEnumerable<ViewResident> Get()
        {
            var context = new SocietyDBEntities();
            var Residents = (from res in context.ViewResidents
                             where (res.DeActiveDate == null) || (DbFunctions.TruncateTime(res.DeActiveDate) > DbFunctions.TruncateTime(DateTime.UtcNow))
                             select res);
            return Residents;
        }

        // GET: api/Resident/5
        public ViewResident GetByUserID(int id)
        {
            try
            {
                using (var context = new SocietyDBEntities())
                {
                    var L2EQuery = context.ViewResidents.Where(u => u.UserID == id);
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


        // GET: api/Resident/5
        public ViewResident GetByMobile(String id)
        {
            try
            {
                using (var context = new SocietyDBEntities())
                {
                    var L2EQuery = context.ViewResidents.Where(u => u.MobileNo == id);
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

        // GET: api/Resident/5
        public ViewResident GetByEmail(String id)
        {
            try
            {
                using (var context = new SocietyDBEntities())
                {
                    var L2EQuery = context.ViewResidents.Where(u => u.EmailId.ToLower() == id.ToLower());
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
        public HttpResponseMessage AddResident([FromBody]Resident Res)
        {
            String resp;
            try
            {
                using (var context = new SocietyDBEntities())
                {
                    var usr = context.Residents;
                    usr.Add(new Resident()
                    {
                        UserID = Res.UserID,
                        FlatID = Res.FlatID,
                        Type = Res.Type,
                        FirstName = Res.FirstName,
                        LastName = Res.LastName,
                        MobileNo = Res.MobileNo,
                        EmailId = Res.EmailId,
                        Addres = Res.Addres,
                        ActiveDate = Res.ActiveDate,
                        DeActiveDate = Res.DeActiveDate,
                        Function="ADD",
                        ModifiedDate = DateTime.Now.ToUniversalTime()          
                    });
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
