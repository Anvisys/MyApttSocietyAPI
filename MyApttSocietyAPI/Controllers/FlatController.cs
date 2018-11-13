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
    public class FlatController : ApiController
    {
        // GET: api/Flat
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Flat/5
        public ViewFlat Get(String id)
        {
            try
            {
                using (var context = new SocietyDBEntities())
                {
                    var L2EQuery = context.ViewFlats.Where(f => f.FlatNumber == id);
                    var flat = L2EQuery.FirstOrDefault();
                    return flat;
                }
            }
            catch (Exception ex)
            {
                Log.log(" ViewFlat Error at: " + DateTime.Now.ToString());
                return null;
            }
        }

        // POST: api/Flat
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Flat/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Flat/5
        public void Delete(int id)
        {
        }
    }
}
