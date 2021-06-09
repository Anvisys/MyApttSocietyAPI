using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using MyApttSocietyAPI.Models;

namespace MyApttSocietyAPI.Controllers.Rent
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api")]
    public class RentLookupController : ApiController
    {
        [Route("InventoryType")]
        public IHttpActionResult GetInventoryType()
        {
            try
            {
                using(var context = new NestinDBEntities())
                {
                    var rt = context.lukInventoryTypes.ToList();
                    return Ok(rt);
                }           

            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(ex.Message));

            }
        }


        [Route("Accomodation/{InvTypeID}")]
        public IHttpActionResult GetAccomodationType(int InvTypeID)
        {
            try
            {
                using (var context = new NestinDBEntities())
                {
                    var at = context.ViewInventoryAccomodations.Where(x=> x.InventoryTypeID == InvTypeID).ToList();
                    return Ok(at);
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(ex.Message));

            }
        }




        [Route("AccomodationType")]
        public IHttpActionResult GetAccomodationType()
        {
            try
            {
                using (var context = new NestinDBEntities())
                {
                    var rt = context.lukAccomodationTypes.ToList();
                    return Ok(rt);
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(ex.Message));

            }
        }

        [Route("InventoryAccomodationType")]
        public IHttpActionResult GetInventoryAccomodationType()
        {
            try
            {
                using (var context = new NestinDBEntities())
                {
                    var rt = from ia in context.lukInventoryAccomodations
                             join at in context.lukAccomodationTypes
                             on ia.AccomodationTypeId equals at.AccomodationTypeID
                             select new InventoryAccomodation() { InventoryId = ia.InventoryTypeId, AccomodationId = ia.AccomodationTypeId, Accomodation = at.AccomodationType };

                    return Ok(rt.ToList());
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception(ex.Message));

            }
        }

        // POST: api/RentLookup
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/RentLookup/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/RentLookup/5
        public void Delete(int id)
        {
        }
    }
}
