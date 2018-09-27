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
    public class GCMRegisterController : ApiController
    {
        // GET: api/GCMRegister
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/GCMRegister/5
        public GCMList Get(String id)
        {
            try
            {
                Log.log("Register User called At :" + DateTime.Now.ToString());
                using (var context = new SocietyDBEntities())
                {
                    var GCM = context.GCMLists;
                    var reg = GCM.Where(g => g.MobileNo.Contains(id.ToString()) || id.ToString().Contains(g.MobileNo)).First();
                    if (reg != null)
                    {
                        return reg;
                    }
                    else
                    {
                        return new GCMList { ID = -99, MobileNo = "", RegID = "" };
                    }
                }
                                
            }
            catch (Exception ex)
            {
                return new GCMList { ID = -99, MobileNo = "", RegID = "" };
            }
           
        }

        // POST: api/GCMRegister
        public HttpResponseMessage Post([FromBody]GCMRegister value)
        {
            try
            {
                Log.log("Register User called At :" + DateTime.Now.ToString());
                using (var context = new SocietyDBEntities())
                {
                    var GCM = context.GCMLists;
                    var reg = GCM.Where(g => g.MobileNo == value.MobileNo);
                    if (reg.Count() == 0)
                    {
                       
                        GCM.Add(new GCMList
                        {
                            MobileNo = value.MobileNo,
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

        // PUT: api/GCMRegister/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/GCMRegister/5
        public void Delete(int id)
        {
        }
    }
}
