using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MyApttSocietyAPI.Models;
using System.Web.Http.Cors;

namespace MyApttSocietyAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/GCM")]
    public class GCMRegisterController : ApiController
    {
        // GET: api/GCMRegister
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("User/{UserId}")]
        [HttpGet]
        public ViewGCMList GetByUserId(int UserId)
        {
            try
            {
                Log.log("Register User called At :" + DateTime.Now.ToString());
                using (var context = new NestinDBEntities())
                {
                    var GCM = context.ViewGCMLists;
                    var reg = GCM.Where(g => g.UserID == UserId).First();
                    if (reg != null)
                    {
                        return reg;
                    }
                    else
                    {
                        return new ViewGCMList { ResID = -99, UserID = UserId, RegID = "", FirstName="", LastName="",MobileNo=""};
                    }
                }
                                
            }
            catch (Exception ex)
            {
                return new ViewGCMList { ResID = -99, UserID = UserId, RegID = "", FirstName = "", LastName = "", MobileNo = "" };
            }
           
        }



        [Route("Res/{ResId}")]
        [HttpGet]
        public ViewGCMList GetByResId(int ResId)
        {
            try
            {
                Log.log("Register User called At :" + DateTime.Now.ToString());
                using (var context = new NestinDBEntities())
                {
                    var GCM = context.ViewGCMLists;
                    var reg = GCM.Where(g => g.ResID == ResId).First();
                    if (reg != null)
                    {
                        return reg;
                    }
                    else
                    {
                        return new ViewGCMList { ResID = -99, UserID = -99, RegID = "", FirstName = "", LastName = "", MobileNo = "" };
                    }
                }

            }
            catch (Exception ex)
            {
                return new ViewGCMList { ResID = -99, UserID = -99, RegID = "", FirstName = "", LastName = "", MobileNo = "" };
            }

        }


        [Route("Add")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody]GCMList value)
        {
            try
            {
                Log.log("Register User called At :" + DateTime.Now.ToString());
                using (var context = new NestinDBEntities())
                {
                    var GCM = context.GCMLists;
                    var reg = GCM.Where(g => g.UserId == value.UserId);
                    if (reg.Count() == 0)
                    {
                       
                        GCM.Add(new GCMList
                        {
                            UserId = value.UserId,
                            RegID = value.RegID,
                            Topic = value.Topic,
                        });
                        
                    }
                    else {

                        reg.First().RegID = value.RegID;
                    }
                    context.SaveChanges();
                }
                String resp = "{\"Response\":\"OK\"}";
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                Log.log("Failed to save :" + ex.Message + " At " + DateTime.Now.ToString());
                String resp = "{\"Response\":\"Fail\"}";
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }
        }

        [Route("Delete")]
        [HttpPost]
        public HttpResponseMessage Remove([FromBody]GCMList value)
        {
            try
            {
                Log.log("Register User called At :" + DateTime.Now.ToString());
                using (var context = new NestinDBEntities())
                {
                    var GCM = context.GCMLists;
                    var reg = GCM.Where(g => g.UserId == value.UserId);
                    if (reg.Count() == 0)
                    {

                        GCM.RemoveRange(reg);

                    }
                   
                    context.SaveChanges();
                }
                String resp = "{\"Response\":\"OK\"}";
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                Log.log("Failed to delete :" + ex.Message + " At " + DateTime.Now.ToString());
                String resp = "{\"Response\":\"Fail\"}";
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }
        }

        [Route("SendMessage")]
        [HttpPost]
        public HttpResponseMessage SendMessage([FromBody]GCMRegister value)
        {
            try
            {
                Log.log("Register User called At :" + DateTime.Now.ToString());
                Utility.SendGCMNotification(value.RegID, value.Message);
                String resp = "{\"Response\":\"OK\"}";
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                Log.log("Failed to delete :" + ex.Message + " At " + DateTime.Now.ToString());
                String resp = "{\"Response\":\"Fail\"}";
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }
        }
    }
}
