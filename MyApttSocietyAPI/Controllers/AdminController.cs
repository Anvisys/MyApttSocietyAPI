using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MyApttSocietyAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    [RoutePrefix("api/Admin")]
    public class AdminController : ApiController
    {
        [HttpGet]
        [Route("society/{societyid}")]
        // GET: api/Admin
        public ViewSocietyUser GetAdminDetails(int societyid)
        {
            var context = new NestinDBEntities();
            var admindetails = (from s in context.ViewSocietyUsers
                                where s.SocietyID==societyid && s.Type=="Admin"
                                select s).First();
            return admindetails;
        }

        // GET: api/Admin/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Admin
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Admin/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Admin/5
        public void Delete(int id)
        {
        }
    }
}
