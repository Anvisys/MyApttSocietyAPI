using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace MyApttSocietyAPI.Models
{
    public class Notifications
    {
        protected NestinDBEntities DbContext { get; private set; }
        //protected String Topic { get; private set; }
        // Message message;
        //private int UserID;

        public Notifications(NestinDBEntities _dbContext)
        {
            DbContext = _dbContext;
        }
      

        public enum TO { All, Society, User, Flat }




        private List<ViewNewUserSetting> GetReceipents(TO sendTo, int ID, String Topic)
        {
            List<ViewNewUserSetting> list;
            if (sendTo == TO.All)
            {
                list = DbContext.ViewNewUserSettings.Where(us => us.Topic == Topic).Distinct().ToList();
            }
            else if (sendTo == TO.Society)
            {
                list = DbContext.ViewNewUserSettings.Where(us => us.SocietyID == ID && us.Topic == Topic).Distinct().ToList();
            }
            else if (sendTo == TO.User)
            {
                list = DbContext.ViewNewUserSettings.Where(us => us.ResID == ID && us.Topic == Topic).ToList().ToList();
            }
            else if (sendTo == TO.Flat)
            {
                list = DbContext.ViewNewUserSettings.Where(us => us.FlatID == ID && us.Topic == Topic).ToList().ToList();
            }
            else
            {
                list = DbContext.ViewNewUserSettings.Where(us => us.ResID == ID && us.Topic == Topic).ToList().ToList();
            }
            return list;
        }

        public void Notify(TO sendTo, int ID, Message message)
        {
            try
            {
                List<ViewNewUserSetting> list = GetReceipents(sendTo, ID, message.Topic);

                String textMessage = message.Topic + "&" + message.TextMessage;

                var GCMList = (from g in list
                               where g.GCM == true
                               select g.RegID).ToArray();
                if (GCMList.Count() > 0)
                {
                    string gcmArray = String.Concat(GCMList);

                    Utility.SendGCMNotification(gcmArray, textMessage);
                }

                var mailList = (from g in list
                                where g.Mail == true
                                select g.EmailId).ToArray();

                foreach (String mail in mailList)
                {

                    Utility.SendMail(mail, message.Topic, message.TextMessage);

                }
            }
            catch (Exception ex)
            {
            }


        }

    }


  
}