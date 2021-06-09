using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MyApttSocietyAPI.Models;
using System.Web.Http.Cors;
using System.Data.Entity;

namespace MyApttSocietyAPI.Controllers
{
      [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SummaryController : ApiController
    {
        // GET: api/Summary
        public Summary Get()
        {
            try
            {
                DateTime today = DateTime.Today;
                // diff Compliant count
                DateTime lastMonth = DateTime.Today.AddMonths(-1);
                Summary summary = new Summary();
                var context = new NestinDBEntities();
                Log.log(" Get Summary found are: 1   " + DateTime.Now.ToString());
                var Complaints = (from comp in context.ViewComplaintHistories
                                  where comp.CompStatus == "Initiated" & DbFunctions.TruncateTime(comp.ModifiedAt) > DbFunctions.TruncateTime(lastMonth)
                                  select comp);
                summary.New_Comp = Complaints.Count();

                var TotalComplaints = (from comp in context.ViewComplaintHistories
                                       where DbFunctions.TruncateTime(comp.ModifiedAt) > DbFunctions.TruncateTime(lastMonth)
                                       select comp);
                summary.Total_Comp = TotalComplaints.Count();

                var resolvedComplaints = (from comp in context.ViewComplaintHistories
                                          where comp.CompStatus == "Resolved" & DbFunctions.TruncateTime(comp.ModifiedAt) > DbFunctions.TruncateTime(lastMonth)
                                          select comp);
                summary.Resolved_Comp = resolvedComplaints.Count();

                summary.InProg_Comp = summary.Total_Comp - summary.Resolved_Comp - summary.New_Comp;
                Log.log(" Get Summary found are: 2" + DateTime.Now.ToString());
                // Top 3 forum / discussion

                var forum = (from thread in context.ViewThreadSummaryNoImageCounts
                             orderby thread.UpdatedAt descending
                             select thread).Take(3);
             
                String[] threads = new String[3];
                int i = 0;
                foreach (var thread in forum)
                {
                    if (thread.latestThread == null)
                    {
                        threads[i] = "";
                    }
                    else
                    {
                        threads[i] = thread.latestThread;
                    }
                   
                    i++;
                    Log.log(" Get Summary found are: loop" + DateTime.Now.ToString());
                }
                summary.forum_1 = threads[0];
                summary.forum_2 = threads[1];
                summary.forum_3 = threads[2];


                // Top 3 notifications
                var notice = (from note in context.Notifications
                              orderby note.Date descending
                              select note).Take(3);
                Log.log(" Get Summary found are: 5   " + DateTime.Now.ToString());
                String[] notes = new String[3];
                int j = 0;
                foreach (var n in notice)
                {

                    if (n.Notification1 == null)
                    {
                        notes[j] = "";
                    }
                    else
                    {
                        notes[j] = n.Notification1;
                    }

                   
                    j++;
                }
                summary.notice_1 = notes[0];
                summary.notice_2 = notes[1];
                summary.notice_3 = notes[2];

                Log.log(" Get Summary found are: 6  " + DateTime.Now.ToString());
                // Total number of discussions updated today
                var todayforum = (from thread in context.ViewThreadSummaryNoImageCounts
                                  where DbFunctions.TruncateTime(thread.UpdatedAt) == DbFunctions.TruncateTime(today)
                                  select thread);

                summary.today_discussion = todayforum.Count();

                // Total number of discussions updated today
                var todayNotice = (from note in context.Notifications
                                   where DbFunctions.TruncateTime(note.Date) == DbFunctions.TruncateTime(today)
                                   select note);

                summary.today_Notice = todayNotice.Count();

                summary.vendor_1st = "10% discount of all vegetables";
                summary.vendor_2nd = "clearance Sale at Aditya Super Mall";

                Log.log(" Get Summary found are: " + DateTime.Now.ToString());
                return summary;
            }
            catch (Exception ex)
            {
                Log.log(" Exception occured at Summary return: " + ex.Message + "   " + DateTime.Now.ToString());
                return null;

            }
        }

        // GET: api/Summary/5
        public Summary Get(int id)
        {
            try
            {
                DateTime today = DateTime.Today;
                // diff Compliant count
                DateTime lastMonth = DateTime.Today.AddMonths(-1);
                Summary summary = new Summary();
                var context = new NestinDBEntities();
                var Complaints = (from comp in context.ViewComplaintHistories
                                  where comp.ResidentID == id & comp.CompStatus == "Initiated" & DbFunctions.TruncateTime(comp.ModifiedAt) > DbFunctions.TruncateTime(lastMonth)
                                  select comp);
                summary.New_Comp = Complaints.Count();

                var TotalComplaints = (from comp in context.ViewComplaintHistories
                                       where comp.ResidentID == id & DbFunctions.TruncateTime(comp.ModifiedAt) > DbFunctions.TruncateTime(lastMonth)
                                       select comp);
                summary.Total_Comp = TotalComplaints.Count();

                var resolvedComplaints = (from comp in context.ViewComplaintHistories
                                          where comp.ResidentID == id & comp.CompStatus == "Resolved" & DbFunctions.TruncateTime(comp.ModifiedAt) > DbFunctions.TruncateTime(lastMonth)
                                          select comp);
                summary.Resolved_Comp = resolvedComplaints.Count();

                summary.InProg_Comp = summary.Total_Comp - summary.Resolved_Comp - summary.New_Comp;

                // Top 3 forum / discussion

                var forum = (from thread in context.ViewThreadSummaryNoImageCounts
                             orderby thread.UpdatedAt descending
                             select thread).Take(3);

                String[] threads = new String[3];
                int i = 0;
                foreach (var thread in forum)
                {
                    if (thread.latestThread == null)
                    {
                        threads[i] = "";
                    }
                    else
                    {
                        threads[i] = thread.latestThread;
                    }

                    i++;
                }
                summary.forum_1 = threads[0];
                summary.forum_2 = threads[1];
                summary.forum_3 = threads[2];


                // Top 3 notifications
                var notice = (from note in context.Notifications
                              orderby note.Date descending
                              select note).Take(3);

                String[] notes = new String[3];
                int j = 0;
                foreach (var n in notice)
                {
                    if (n.Notification1 == null)
                    {
                        notes[j] = "";
                    }
                    else
                    {
                        notes[j] = n.Notification1;
                    }


                    j++;
                }
                summary.notice_1 = notes[0];
                summary.notice_2 = notes[1];
                summary.notice_3 = notes[2];


                // Total number of discussions updated today
                var todayforum = (from thread in context.ViewThreadSummaryNoImageCounts
                                  where DbFunctions.TruncateTime(thread.UpdatedAt) == DbFunctions.TruncateTime(today)
                                  select thread);
                summary.today_discussion = todayforum.Count();

                // Total number of discussions updated today
                var todayNotice = (from note in context.Notifications
                                   where DbFunctions.TruncateTime(note.Date) == DbFunctions.TruncateTime(today)
                                   select note);

                summary.today_Notice = todayNotice.Count();

                summary.vendor_1st = "10% discount of all vegetables";
                summary.vendor_2nd = "clearance Sale at Aditya Super Mall";

                Log.log(" Get Summary found are: " + DateTime.Now.ToString());
                return summary;
            }
            catch (Exception ex)
            {
                Log.log(" Exception occured at Summary return: " + ex.Message + "   " + DateTime.Now.ToString());
                return null;

            }
        }

        // POST: api/Summary
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Summary/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Summary/5
        public void Delete(int id)
        {
        }
    }
}
