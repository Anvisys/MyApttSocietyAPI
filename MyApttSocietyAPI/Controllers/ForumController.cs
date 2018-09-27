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
    public class ForumController : ApiController
    {
        // GET: api/Forum
        public IEnumerable<ViewThreadSummaryNoImageCount> Get()
        {
            try
            {
                var context = new SocietyDBEntities();
                var Forum = (from thread in context.ViewThreadSummaryNoImageCounts
                             orderby thread.UpdatedAt descending
                             select thread);

                Log.log(" Get Forum Results found are:" + DateTime.Now.ToString());
                return Forum;
            }
            catch (Exception ex)
            {
                Log.log(" Get Forum has error at: " + DateTime.Now.ToString() + " " + ex.Message);
                return null;
            }
        }

        // GET: api/Forum/5
        public IEnumerable<ViewForumNoImage> Get(int id)
        {
            try
            {
                var context = new SocietyDBEntities();

                var Thread = (from th in context.ViewForumNoImages
                              where th.ThreadID == id
                              orderby th.UpdatedOn ascending
                              select th);

                Log.log(" Get thread Results found at:" + DateTime.Now.ToString());
                return Thread;
            }
            catch (Exception ex)
            {
                Log.log(" Get thread by ID has error at: " + DateTime.Now.ToString() + " " + ex.Message);
                return null;
            }
        }

        // POST: api/Forum
        public HttpResponseMessage Post([FromBody]MyApttSocietyAPI.Models.Forum frm)
        {

            Log.log(" new post on thread is received : " + DateTime.Now.ToString());

            try
            {
                using (var context = new SocietyDBEntities())
                {
                    var c = context.Fora;
                    Log.log(" log counts are  " + c.Count());

                    if (frm.ThreadID < 1)
                    {
                        if (c.Count() > 0)
                        {
                            var maxID = c.Max(maxT => maxT.ThreadID);
                            frm.ThreadID = maxID + 1;
                            Log.log(" new post ThreadID is not null  at  " + DateTime.Now.ToString());
                        }
                        else
                        {
                            Log.log(" new post ThreadID is : null  at  " + DateTime.Now.ToString());
                            frm.ThreadID = 1; }
                   }
                   
                   c.Add(new Forum()
                    {

                        ThreadID = frm.ThreadID,
                        ResID = frm.resID,
                        Topic = frm.Topic,
                        Thread = frm.CurrentThread,
                        UpdatedOn = DateTime.Now.ToUniversalTime(),
                        
                    });
                    context.SaveChanges();


                    String resp = "{\"Response\":\"OK\", \"ThreadID\":"+ frm.ThreadID +"}";
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                    return response;

                }
            }
            catch (Exception ex)
            {
                String resp = "{\"Response\":\"FAIL\",\"Error\":\"" + ex.Message + "\"}";
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }


        }

        // PUT: api/Forum/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Forum/5
        public void Delete(int id)
        {
        }
    }
}
