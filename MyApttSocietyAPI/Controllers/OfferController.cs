using MyApttSocietyAPI.Models;
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
    [RoutePrefix("api/Offers")]
    public class OfferController : ApiController
    {
        [HttpGet]
        [Route("All")]
        public IEnumerable<Offer> GetAllOffers()
        {
            var context = new SocietyDBEntities();
            var offers = (from s in context.Offers
                          select s).ToList();
            return offers;
        }

        // GET: api/Offer/5
        [HttpGet]
        [Route("Society/{SocietyID}/Vendor/{VendorID}")]
        public IEnumerable<ViewOffer> Get(int societyid ,int vendorid)
        {

            var context = new SocietyDBEntities();
            var offers = (from s in context.ViewOffers
                          where s.SocietyID == societyid && s.VendorID == vendorid
                          select s).ToList();
            return offers;
        }

        // POST: api/Offer
        [HttpPost]
        [Route("New")]
        public IHttpActionResult AddNewOffer([FromBody]Offer value)
        {
            try
            {
                var context = new SocietyDBEntities();
                context.Offers.Add(value);
                context.SaveChanges();
                return Ok("ok");
            }
            catch (Exception e)
            {
                return Ok("fail ");
            }

        }

        // PUT: api/Offer/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Offer/5
        public void Delete(int id)
        {
        }
    }
}
