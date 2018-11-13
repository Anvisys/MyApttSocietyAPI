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
    public class AdsController : ApiController
    {
        // GET: api/Ads
        public IEnumerable<Advertisement> Get()
        {
            try
            {
                var context = new SocietyDBEntities();

                var Ad = (from th in context.Advertisements
                             
                              select th);

              return Ad;
            }
            catch (Exception ex)
            {
                Log.log(" Get Ads has error at: " + DateTime.Now.ToString() + " " + ex.Message);
                return null;
            }
        }

        // GET: api/Ads/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Ads
        public HttpResponseMessage Post([FromBody]Advertisement value)
        {

             String resp = "{\"Response\":\"Undefine\"}";
            var   ctx = new SocietyDBEntities();
           
                try
                {

                    ctx.Advertisements.Add(value);
                    ctx.SaveChanges();

                    resp = "{\"Response\":\"Ok\"}";
                }
                catch (Exception ex)
                {
                    resp = "{\"Response\":\"Fail\"}";
                }
           
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        // PUT: api/Ads/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Ads/5
        public void Delete(int id)
        {
        }
    }
}
