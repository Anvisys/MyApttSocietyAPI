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
    public class ForumDiffController : ApiController
    {
        // GET: api/ForumDiff
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ForumDiff/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ForumDiff
        public IEnumerable<ViewThreadSummaryNoImageCount> Post([FromBody]Batch value)
        {

          /*  Log.log("Input Value is  : " + value.value);

            DateTime lastDateTime;
            try
            {
                lastDateTime = DateTime.ParseExact(dateTime.value, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentUICulture);

                Log.log("Date found is  : " + lastDateTime.ToString());
            }
            catch (Exception ex)
            {
                Log.log("Could not parse String to Date time found is 1 : ");
                lastDateTime = DateTime.Now;
            }*/

            try
            {
                var context = new SocietyDBEntities();
                if (value.LastRefreshTime == "")
                {
                
                var count = value.EndIndex - value.StartIndex;
                var Forum = (from thread in context.ViewThreadSummaryNoImageCounts
                             orderby thread.UpdatedAt descending
                             select thread);
                return Forum.Skip(value.StartIndex).Take(count);

                }
                else
                {
                    DateTime updatedDateTime = DateTime.ParseExact(value.LastRefreshTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentUICulture);
                    return (from thread in context.ViewThreadSummaryNoImageCounts
                            where thread.UpdatedAt > updatedDateTime
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
