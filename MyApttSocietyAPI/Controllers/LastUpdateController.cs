using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MyApttSocietyAPI.Models;

namespace MyApttSocietyAPI.Controllers
{
    public class LastUpdateController : ApiController
    {
        // GET: api/LastUpdate
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/LastUpdate/5
        public string Get(int id)
        {
            return "value";


        }

        // POST: api/LastUpdate
        public Update Post([FromBody]DifferentialInput value)
        {
            try
            {

            Update update = new Update();
                           
            DateTime ForumDateTime = DateTime.ParseExact(value.ForumRefreshTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentUICulture);

            DateTime ComplaintDateTime = DateTime.ParseExact(value.ComplaintRefreshTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentUICulture);

            DateTime BillingDateTime = DateTime.ParseExact(value.BillRefreshTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentUICulture);

            DateTime NotificationDateTime = DateTime.ParseExact(value.NoticeRefreshTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentUICulture);

            DateTime VendorDateTime = DateTime.ParseExact(value.VendorRefreshTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentUICulture);

            DateTime PollDateTime = DateTime.ParseExact(value.PollRefreshTime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentUICulture);

          
                var context = new SocietyDBEntities();
                update.ComplaintCount = (from comp in context.ViewComplaintHistories
                                         where comp.ResidentID == value.resID && comp.ModifiedAt > ComplaintDateTime
                                         select comp).Count();

                update.TotalComplaintCount = (from comp in context.ViewComplaintHistories
                                         where comp.ResidentID == value.resID
                                         select comp).Count();

                update.ForumCount = (from thread in context.ViewThreadSummaryNoImageCounts
                                     where thread.UpdatedAt > ForumDateTime || thread.InitiatedAt > ForumDateTime
                                     select thread).Count();

                update.TotalForumCount = (from thread in context.ViewThreadSummaryNoImageCounts
                                           select thread).Count();


                update.PollCount = (from poll in context.PollingDatas
                                    where poll.LastUpdated > PollDateTime
                                    select poll).Count();

                update.TotalPollCount = (from poll in context.PollingDatas
                                     select poll).Count();

                update.VendorCount = (from vend in context.Vendors
                                      where vend.Date > VendorDateTime
                                      select vend).Count();

                update.TotalVendorCount = (from vend in context.Vendors
                                          select vend).Count();


                update.NoticeCount = (from notice in context.Notifications
                                      where notice.Date > NotificationDateTime
                                      select notice).Count();

                update.TotalNoticeCount = (from notice in context.Notifications
                                      select notice).Count();

                update.BillCount = context.GeneratedBills.Where(gb => gb.FlatID == value.FlatId && (gb.AmountPaidDate > BillingDateTime || gb.ModifiedAt > BillingDateTime)).Count();

                return update;
            }
            catch (Exception ex)
            {
                Log.log(" Get Forum has error at: " + DateTime.Now.ToString() + " " + ex.Message);
                return null;
            }
        }

        // PUT: api/LastUpdate/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LastUpdate/5
        public void Delete(int id)
        {
        }
    }
}
