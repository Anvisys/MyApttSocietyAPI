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
    [RoutePrefix("api/RentInventory")]
    public class RentInventoryController : ApiController
    {
        // GET: api/RentInventory
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Find")]
        [HttpPost]
        public IHttpActionResult GetInventory([FromBody]RentInventory value)
        {
            try {
                var context = new SocietyDBEntities();

                var inventory = context.RentInventories.Where(X => X.InventoryID == value.InventoryID && X.RentTypeID == value.RentTypeID 
                                                           && X.RentValue == value.RentValue ).ToList();

                return Ok(inventory);


            }
            catch (Exception ex)
            {
                return InternalServerError();

            }
        }

        [Route("New")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]RentInventory value)
        {
            try {
                var context = new SocietyDBEntities();
                    context.RentInventories.Add(new RentInventory {
                        Available = value.Available,
                        ContactName = value.ContactName,
                        ContactNumber = value.ContactNumber,
                        Description = value.Description,
                        FlatID = value.FlatID,
                        InventoryID = value.InventoryID,
                        RentTypeID = value.RentTypeID,
                        RentValue = value.RentValue,
                        UserID = value.UserID
                    } );
                    context.SaveChanges();
                    return Ok();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex.InnerException);
            }
        }

        // PUT: api/RentInventory/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/RentInventory/5
        public void Delete(int id)
        {
        }
    }
}
