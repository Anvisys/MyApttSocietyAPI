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
        [Route("All")]
        [HttpGet]
        public IEnumerable<ViewSociety> AllSocieties()
        {
            var context = new NestinDBEntities();
            var socList = (from s in context.ViewSocieties
                           select s).ToList();
            return socList;
        }


        [Route("{UserId}")]
        [HttpGet]
        public IEnumerable<ViewSociety> MySocietyRequests(int UserId)
        {
            var context = new NestinDBEntities();
            var socReq = context.ViewSocieties.Where(x=> x.UserID == UserId).ToList();
            return socReq;
        }

        [Route("All/{UserId}")]
        [HttpGet]
        public IEnumerable<ViewSocietyUser> MyAllRequests(int UserId)
        {
            var context = new NestinDBEntities();
            var socReq = context.ViewSocietyUsers.Where(x => x.UserID == UserId).ToList();
            return socReq;
        }

        [Route("House/{UserId}")]
        [HttpGet]
        public IEnumerable<ViewSocietyUser> MyHouses(int UserId)
        {
            var context = new NestinDBEntities();
            var socReq = context.ViewSocietyUsers.Where(x => x.UserID == UserId && x.Type == "Individual").ToList();
            return socReq;
        }

        [Route("Flats/{UserId}")]
        [HttpGet]
        public IEnumerable<ViewSocietyUser> MyFlats(int UserId)
        {
            var context = new NestinDBEntities();
            var socReq = context.ViewSocietyUsers.Where(x => x.UserID == UserId && (x.Type == "Owner" || x.Type == "Tenant")).ToList();
            return socReq;
        }


        [Route("Add")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]Society society)
        {
            var context = new NestinDBEntities();

            context.Societies.Add(society);
            context.SaveChanges();

            return Ok();
        }

       
        // DELETE: api/Society/5
        public void Delete(int id)
        {
        }
    }
}
