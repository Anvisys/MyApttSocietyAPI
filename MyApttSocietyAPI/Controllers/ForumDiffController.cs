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

     [RoutePrefix("api/ForumDiff")]
    public class ForumDiffController : ApiController
    {
        // GET: api/ForumDiff
     
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("{SocietyID}")]
        [HttpGet]
         public string Get(int SocietyID)
        {
            return "value";
        }

        // POST: api/ForumDiff

         [Route("{NewForumDiff}")]
         [HttpPost]
        public IEnumerable<ViewThreadSummaryNoImageCount> Post([FromBody]Batch value)
        {

         
            try
            {
                var context = new NestinDBEntities();
                if (value.LastRefreshTime == "")
                {
                
                var count = value.EndIndex - value.StartIndex;
                var Forum = (from thread in context.ViewThreadSummaryNoImageCounts
                             where thread.SocietyID == value.SocietyID
                             orderby thread.UpdatedAt descending
                             select thread);
                return Forum.Skip(value.StartIndex).Take(count);

                }
                else
                {
                    DateTime updatedDateTime = DateTime.ParseExact(value.LastRefreshTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentUICulture);
                    return (from thread in context.ViewThreadSummaryNoImageCounts
                            where thread.SocietyID == value.SocietyID && thread.UpdatedAt > updatedDateTime
                            orderby thread.UpdatedAt descending
                            select thread).Take(10);
                }
               
            }
            catch (Exception ex)
            {
                Log.log(" Get Forum has error at: " + DateTime.Now.ToString() + " " + ex.Message);
                return null;
            }
        }

        // PUT: api/ForumDiff/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ForumDiff/5
        public void Delete(int id)
        {
        }
    }
}
