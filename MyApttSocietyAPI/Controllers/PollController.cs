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
    [RoutePrefix("api/Poll")]
    public class PollController : ApiController
    {

        private Object thisLock = new Object();
        // GET: api/Poll
        public IEnumerable<PollingData> Get()
        {
            try
            {
                var context = new NestinDBEntities();
                var op = (from poll in context.PollingDatas
                             orderby poll.EndDate descending
                             select poll);
               
                Log.log(" Get Forum Results found are:" + DateTime.Now.ToString());
              

                return op;
            }
            catch (Exception ex)
            {
                Log.log(" Get Forum has error at: " + DateTime.Now.ToString() + " " + ex.Message);
                return null;
            }
        }

        // GET: api/Poll/5
        public IEnumerable<Poll> Get(int id)
        {
            try
            {
                var context = new NestinDBEntities();
                var Poll = (from poll in context.ViewPollCounts
                            orderby poll.PollID == id
                            select poll);
                Log.log(" Get Forum Results found are:" + DateTime.Now.ToString());

                List<Poll> pollList = new List<Poll>();

                foreach (ViewPollCount p in Poll)
                {
                    Poll newPoll = new Poll();
                    newPoll.PollID = p.PollID;
                    newPoll.Answer1Count = (Int32)p.Ans1;
                    newPoll.Answer2Count = (Int32)p.Ans2;
                    newPoll.Answer3Count = (Int32)p.Ans3;
                    newPoll.Answer4Count = (Int32)p.Ans4;
                    pollList.Add(newPoll);
                }
                return pollList;
            }
            catch (Exception ex)
            {
                Log.log(" Get Forum has error at: " + DateTime.Now.ToString() + " " + ex.Message);
                return null;
            }
        }



        [Route("Get/{SocietyID}/{ResID}/{PageNumber}/{count}")]
        [HttpGet]
        // POST: api/PollDiff
        public IEnumerable<Poll> GetForum(int SocietyID, int ResID, int PageNumber ,int count)
        {

            try
            {
                var context = new NestinDBEntities();
               
                  //  var count = 10;

                   var polldata = (from poll in context.ViewPollDataWithCounts
                                where poll.SocietyID == SocietyID && poll.EndDate >= DateTime.UtcNow
                                orderby poll.EndDate descending
                                select poll).ToList().Skip((PageNumber-1)*count).Take(count);
                   

                List<Poll> pollList = new List<Poll>();

                foreach (ViewPollDataWithCount p in polldata)
                {

                    var selectedAnswer = (from ans in context.PollingAnswers
                                          where ans.PollID == p.PollID && ans.ResID == ResID
                                          select ans.SelectedAnswer);

                    Poll newPoll = new Poll();
                    Log.log(" Get answer for : " + p.PollID.ToString());

                    if (selectedAnswer.Count() >= 1)
                    {
                        newPoll.previousSelected = selectedAnswer.First();
                    }
                    else
                    {
                        newPoll.previousSelected = 0;
                    }
                    newPoll.PollID = p.PollID;
                    newPoll.Question = p.Question;
                    newPoll.StartDate = (DateTime)p.StartDate;
                    newPoll.EndDate = (DateTime)p.EndDate;
                    newPoll.Answer1 = p.Answer1;
                    newPoll.Answer1Count = (Int32)p.Answer1Count;
                    newPoll.Answer2 = p.Answer2;
                    newPoll.Answer2Count = (Int32)p.Answer2Count;
                    newPoll.Answer3 = p.Answer3;
                    newPoll.Answer3Count = (Int32)p.Answer3Count;
                    newPoll.Answer4 = p.Answer4;
                    newPoll.Answer4Count = (Int32)p.Answer4Count;
                    newPoll.SocietyId = p.SocietyID;
                    pollList.Add(newPoll);

                }
                return pollList;

            }
            catch (Exception ex)
            {
                Log.log(" Get Forum has error at: " + DateTime.Now.ToString() + " " + ex.Message);
                return null;
            }

        }


        // POST: api/Poll
        public HttpResponseMessage Post([FromBody]Poll poll)
        {
            try
            {
                   using (var context = new NestinDBEntities())
                    {
                        var c = context.PollingDatas;

                        var polls = (from p in context.PollingAnswers
                                                   where (p.PollID == poll.PollID && p.ResID == poll.ResID)
                                                   select p);

                        if (polls.Count() == 0)
                        {

                            context.PollingAnswers.Add(new PollingAnswer
                            {
                                PollID = poll.PollID,
                                ResID = poll.ResID,
                                SocietyID = poll.SocietyId,
                                SelectedAnswer = poll.selectedAnswer,
                                LastUpdated = DateTime.UtcNow,
                            
                            });
                         
                        }
                        else
                        {
                            polls.First().SelectedAnswer = poll.selectedAnswer;
                        
                        }
                     

                        context.SaveChanges();
                        String resp = "{\"Response\":\"OK\"}";
                        var response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                        return response;

                    }
             
            }
            catch (Exception ex)
            {
                Log.log(" Poll Data Updated error : " + ex.Message);
                String resp = "{\"Response\":\"FAIL\",\"Error\":\"" + ex.Message + "\"}";
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.Content = new StringContent(resp, System.Text.Encoding.UTF8, "application/json");
                return response;
            }

        }

        [Route("GetAnswer")]
        [HttpPost]
        // POST: api/Poll
        public Poll GetAnswer(int id, [FromBody]PollCount pValue)
        {
            try
            {
                var context = new NestinDBEntities();
                var Poll = (from poll in context.ViewPollCounts
                            where poll.PollID == pValue.PollID
                            select poll).First();
                var answer = (from ans in context.PollingAnswers
                              where ans.PollID == pValue.PollID && ans.ResID == pValue.ResID
                              select ans.SelectedAnswer);
               
                    Poll newPoll = new Poll();
                    newPoll.PollID = Poll.PollID;
                    newPoll.Answer1Count = (Int32)Poll.Ans1;
                    newPoll.Answer2Count = (Int32)Poll.Ans2;
                    newPoll.Answer3Count = (Int32)Poll.Ans3;
                    newPoll.Answer4Count = (Int32)Poll.Ans4;
                    if (answer.Count() == 0)
                    {
                        newPoll.previousSelected = 0;
                    }
                    else
                    {
                        newPoll.previousSelected = answer.First();
                    }
              
                return newPoll;
            }
            catch (Exception ex)
            {
                Log.log(" Get Forum has error at: " + DateTime.Now.ToString() + " " + ex.Message);
                return null;
            }

        }

        // PUT: api/Poll/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Poll/5
        public void Delete(int id)
        {
        }


    }
}
