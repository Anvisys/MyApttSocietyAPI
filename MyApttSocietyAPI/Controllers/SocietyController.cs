using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyApttSocietyAPI.Models;
using System.Web.Http.Cors;
using System.Data.Entity.Validation;
using System.Data.Entity;


namespace MyApttSocietyAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Society")]
    public class SocietyController : ApiController
    {
        // GET: api/Society
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Society/5
        public string Get(int id)
        {
            return "value";
        }

        [Route("Add")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]Society society)
        {
            var context = new SocietyDBEntities();

            context.Societies.Add(society);
            context.SaveChanges();

            return Ok();
        }

        // PUT: api/Society/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Society/5
        public void Delete(int id)
        {
        }
    }
}
