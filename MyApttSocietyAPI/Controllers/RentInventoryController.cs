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
                                                               && X.RentValue > 0.8*value.RentValue && X.RentValue<1.2*value.RentValue).ToList();
                return Ok(inventory);


            }
            catch (Exception ex)
            {
                return InternalServerError();

            }
        }

        [Route("New")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody]RentInventory value)
        {
            String resp;
            try {
                var context = new SocietyDBEntities();
                    context.RentInventories.Add(value);
                    context.SaveChanges();
                resp = "{\"Response\":\"OK\"}";
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;

            }
            catch (Exception ex)
            {
                resp = "{\"Response\":\"Fail\"}";
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
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
