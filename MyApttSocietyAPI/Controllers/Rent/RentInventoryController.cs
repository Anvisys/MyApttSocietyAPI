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


namespace MyApttSocietyAPI.Controllers.Rent
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


        [Route("Find/{FlatId}/{HouseId}")]
        [HttpGet]
        public IHttpActionResult GetMyInventory(int FlatId, int HouseId)
        {
            try
            {

                var context = new SocietyDBEntities();

                var inventory = context.ViewRentInventories.Where(X => (X.HouseID == HouseId && X.FlatID == FlatId)
                                                                  && X.Available == true).ToList();
                return Ok(inventory);


            }
            catch (Exception ex)
            {
                return InternalServerError();

            }
        }


        [Route("{SocietyId}")]
        [HttpGet]
        public IHttpActionResult GetSocietyInventory(int SocietyId)
        {
            try
            {
                var context = new SocietyDBEntities();
                var inventory = context.ViewRentInventories.Where(X => (X.SocietyID == SocietyId )
                                                                  && X.Available == true).ToList();
                return Ok(inventory);

            }
            catch (Exception ex)
            {
                return InternalServerError();

            }
        }



        [Route("Find")]
        [HttpPost]
        public IHttpActionResult GetInventory([FromBody]ViewRentInventory value)
        {
            try {

                var context = new SocietyDBEntities();

                var inventory = context.ViewRentInventories.Where(X => X.InventoryTypeID == value.InventoryTypeID && X.AccomodationTypeID == value.AccomodationTypeID
                                                                && X.FlatCity == value.FlatCity && X.Available == true
                                                               && X.RentValue > 0.8 * value.RentValue && X.RentValue < 1.2 * value.RentValue).ToList();
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

                var inv = context.ViewRentInventories.Where(X => (X.HouseID == value.HouseID && X.FlatID == value.FlatID)
                                                                  && X.Available == true).ToList();

                if (inv.Count > 0)
                {
                    resp = "{\"Response\":\"Duplicate\"}";
                    var response = Request.CreateResponse(HttpStatusCode.Conflict);
                    response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                    return response;
                }
                else {
                    context.RentInventories.Add(value);
                    context.SaveChanges();
                    resp = "{\"Response\":\"OK\"}";
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                    return response;
                }



            }
            catch (Exception ex)
            {
                resp = "{\"Response\":\"Fail\"}";
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }
        }

        [Route("Close")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody]InventoryUpdate value)
        {
            String resp;

            try
            {

                var context = new SocietyDBEntities();

                var inv = context.RentInventories.Where(X => X.RentInventoryID == value.InventoryId).First();

                if (inv != null)
                {
                    inv.Available = false;
                    context.SaveChanges();
                    resp = "{\"Response\":\"Ok\"}";
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                    return response;
                }
                else
                {
                    resp = "{\"Response\":\"NotFound\"}";
                    var response = Request.CreateResponse(HttpStatusCode.NotFound);
                    response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                    return response;
                }

            }
            catch (Exception ex)
            {
                resp = "{\"Response\":\"Fail\"}";
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }
        }


        [Route("Add/Interest")]
        [HttpPost]
        public HttpResponseMessage AddRentInterest([FromBody]RentEngagemment interest)
        {
            String resp;
            try
            {
                var context = new SocietyDBEntities();

                var exist = context.RentEngagemments.Where(x => x.InventoryID == interest.InventoryID && x.InterestedUserId == interest.InterestedUserId).ToList();
                if(exist.Count>0)
                {
                    resp = "{\"Response\":\"Fail\"}";
                    var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                    response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                    return response;
                }
                else
                {
                    context.RentEngagemments.Add(interest);
                    context.SaveChanges();

                    resp = "{\"Response\":\"Ok\"}";
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                    return response;
                }

                
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

        public class InventoryUpdate
            {
            public int InventoryId;
            public bool Status;
            }
    }
}
