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
    public class PollDiffController : ApiController
    {
        // GET: api/PollDiff
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/PollDiff/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PollDiff
        public IEnumerable<Poll> Post([FromBody]Batch value)
        {

            try
            {
                var context = new SocietyDBEntities();
                IQueryable<ViewPollDataWithCount> polldata;

                if (value.LastRefreshTime != "")
                {
                    DateTime updatedDateTime = DateTime.ParseExact(value.LastRefreshTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentUICulture);

                    polldata = (from poll in context.ViewPollDataWithCounts
                                where poll.StartDate > updatedDateTime && poll.SocietyID==value.SocietyID
                                orderby poll.EndDate descending
                                select poll).Take(10);
   
                }
                else
                {
                    var count = value.EndIndex - value.StartIndex;

                    polldata = (from poll in context.ViewPollDataWithCounts
                                orderby poll.EndDate descending
                                select poll).Skip(value.StartIndex).Take(count);
                   // var y = Poll.Skip(value.StartIndex).Take(count);

                }
                List<Poll> pollList = new List<Poll>();

                foreach (ViewPollDataWithCount p in polldata)
                {

                    var selectedAnswer = (from ans in context.PollingAnswers
                                where ans.PollID == p.PollID && ans.ResID == value.ResId
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

        // PUT: api/PollDiff/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PollDiff/5
        public void Delete(int id)
        {
        }

        private int GetSelectedAnswer(PollingData p, String resID)
        {
            int selectedAnswer = 0;

            if (selectedAnswer == 0)
            {
                String[] arr1 = p.Answer1String.Split(',');
                foreach (String str in arr1)
                {
                    if (str == resID)
                    {
                        selectedAnswer = 1;
                    }
                }
            }

            if (selectedAnswer == 0)
            {
                String[] arr2 = p.Answer2String.Split(',');
                foreach (String str in arr2)
                {
                    if (str == resID)
                    {
                        selectedAnswer = 2;
                    }
                }
            }

            if (selectedAnswer == 0)
            {
                String[] arr3 = p.Answer3String.Split(',');
                foreach (String str in arr3)
                {
                    if (str == resID)
                    {
                        selectedAnswer = 3;
                    }
                }
            }


            if (selectedAnswer == 0)
            {
                String[] arr4 = p.Answer4String.Split(',');
                foreach (String str in arr4)
                {
                    if (str == resID)
                    {
                        selectedAnswer = 4;
                    }
                }
            }

            return selectedAnswer;

        }
    }
}
