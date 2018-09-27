using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

using MyApttSocietyAPI.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;


namespace MyApttSocietyAPI.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TenantController : ApiController
    {
        // GET: api/Tenant
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Tenant/5
        public string Get(int id)
        {
            return "value";
        }

        
        // POST: api/Tenant
        public HttpResponseMessage Post([FromBody]Tenant Res)
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
                        ActiveDate = DateTime.ParseExact(Res.ActiveDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        DeActiveDate = DateTime.ParseExact(Res.DeActiveDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Function = "ADD",
                        ModifiedDate = DateTime.Now.ToUniversalTime()
                    });
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

        [Route("UpdateTenant")]
        [HttpPost]
        // POST: api/Tenant/UpdateTenant
        public HttpResponseMessage UpdateTenant(int id,[FromBody]UpdateDate updatedDate)
        {
            String resp;
            try
            {
                using (var context = new SocietyDBEntities())
                {
                    List<Resident> users = (from u in context.Residents
                                            where u.UserID == updatedDate.id
                                            select u).ToList();

                    foreach (Resident user in users)
                    {

                        user.DeActiveDate = DateTime.ParseExact(updatedDate.date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
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

                        Log.log("api/Profile Failed to Add User : Property-" + validationError.PropertyName + "  Error- " + validationError.ErrorMessage + "  At " + DateTime.Now.ToString());


                    }
                }
                resp = "{\"Response\":\"Fail\"}";
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }
        }

        // PUT: api/Tenant/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Tenant/5
        public void Delete(int id)
        {
        }
    }
}
