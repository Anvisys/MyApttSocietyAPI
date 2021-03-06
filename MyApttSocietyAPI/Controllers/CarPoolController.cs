﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using MyApttSocietyAPI.Models;

namespace MyApttSocietyAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/CarPool")]
    public class CarPoolController : ApiController
    {
        // GET: api/CarPool
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("All/{SocietyId}/{ResID}/{PageNumber}/{count}")]
        [HttpGet]
        public IEnumerable<ViewVehiclePool> GetPoolOffer(int SocietyId, int ResID, int PageNumber ,int count)
        {
           // int Count = 10;
            try
            {
                var context = new NestinDBEntities();
                var pools = context.ViewVehiclePools.Where(x => x.SocietyID == SocietyId && x.ResID != ResID
                            && x.JourneyDateTime > DateTime.Now && x.Active == true)
                            .OrderByDescending(p=>p.VehiclePoolID).ToList()
                            .Skip((PageNumber-1)* count).Take(count); 
                return pools;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        [Route("self/{SocietyId}/{ResID}/{PageNumber}/{count}")]
        [HttpGet]
        public IEnumerable<ViewVehiclePool> GetMyPoolOffer(int SocietyId, int ResID, int PageNumber ,int count)
        {
           // int Count = 10;
            try
            {
                var context = new NestinDBEntities();
                var pools = context.ViewVehiclePools.Where(x => x.SocietyID == SocietyId 
                           && x.JourneyDateTime > DateTime.Now && x.ResID == ResID && x.Active == true)
                            .OrderByDescending(p => p.VehiclePoolID).ToList()
                            .Skip((PageNumber-1)* count).Take(count);

                return pools;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        [Route("self/{VehiclePollID}")]
        [HttpGet]
        public IEnumerable<ViewVehiclePoolEngagement> GetMyPoolEngagement(int VehiclePollID)
        {
            try
            {
                var context = new NestinDBEntities();
                var pools = context.ViewVehiclePoolEngagements.Where(x => x.VehiclePoolId == VehiclePollID)
                    .OrderBy(p => p.FirstName).ToList();
                           
                return pools;
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        

        [Route("Add")]
        [HttpPost]
        public HttpResponseMessage AddPoolOffer([FromBody]VehiclePool value)
        {
            String resp;
            try
            {
                var context = new NestinDBEntities();
                
                var existing = context.VehiclePools.Where(x => x.Active == true && x.ResID == value.ResID
                                && (x.ReturnDateTime > value.JourneyDateTime && x.ReturnDateTime < value.ReturnDateTime) 
                                || (x.JourneyDateTime > value.JourneyDateTime && x.ReturnDateTime < value.ReturnDateTime)).ToList();

                if (existing.Count > 0)
                {
                    //context.VehiclePools.Add(value);
                    //context.SaveChanges();
                    resp = "{\"Response\":\"Duplicate\"}";
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                    return response;
                }
                else
                {
                    context.VehiclePools.Add(value);
                    context.SaveChanges();

                    Message message = new Message();
                    message.Topic = "CarPool";
                    message.SocietyID = value.SocietyID;
                    message.TextMessage = "A new Car Pool for " + value.Destination.ToString() + " is available is your Society;";

                    Notifications msg = new Notifications(context);
                    msg.Notify(Notifications.TO.Society, value.SocietyID, message);


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

        [Route("Add/Interest")]
        [HttpPost]
        public HttpResponseMessage AddPoolEngagement([FromBody]VehiclePoolEngagemment value)
        {
            String resp;
            try
            {
                var context = new NestinDBEntities();

                var exist = context.VehiclePoolEngagemments.Where(x => x.PoolID == value.PoolID && x.InterestedResId == value.InterestedResId).ToList();

                var user = context.VehiclePools.Where(v => v.VehiclePoolID == value.PoolID).First();

                if (exist.Count > 0)
                {
                    resp = "{\"Response\":\"Duplicate\"}";
                    var response = Request.CreateResponse(HttpStatusCode.Conflict);
                    response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                    return response;
                }
                else
                {
                    context.VehiclePoolEngagemments.Add(value);
                    context.SaveChanges();

                    Message message = new Message();
                    message.Topic = "CarPool";
                    message.SocietyID = user.SocietyID;
                    message.TextMessage = "Someone is interested in Your Ride Share offer to " + user.Destination;

                    Notifications msg = new Notifications(context);
                    msg.Notify(Notifications.TO.User, user.ResID, message);

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

        [Route("Status")]
        [HttpPost]
        public HttpResponseMessage UpdatePoolStatus([FromBody]VehiclePool value)
        {
            String resp;
            try
            {
                var context = new NestinDBEntities();
                var p = context.VehiclePools.Where(x => x.VehiclePoolID == value.VehiclePoolID && x.ResID == value.ResID).First();

                if (p == null)
                {
                    resp = "{\"Response\":\"NotExist\"}";
                    var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                    response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                    return response;
                }
                else
                {
                    p.Active = value.Active;
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

        // PUT: api/CarPool/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CarPool/5
        public void Delete(int id)
        {
        }
    }
}
