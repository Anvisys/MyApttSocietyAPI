using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public abstract class NotificationType
    {
        protected SocietyDBEntities DbContext { get; private set; }

        public NotificationType(SocietyDBEntities dbContext, string userIdentification)
        {
         DbContext = dbContext;
         UserIdentification = UserIdentification;
         //InitParams();
        }
        public string UserIdentification { get; protected set; }
        public string UserEmail { get; protected set; }
        public string UserPhone { get; protected set; }
        public bool IsSms { get; protected set; }
        public bool IsEmail { get; protected set; }
        public bool IsGcm { get; protected set; }

        public bool NotifyResidents(String textMessage, String Heading)
        {
           
            if (IsEmail)
            {
                Utility.SendMail(UserEmail, Heading, textMessage);
            }
            if (IsGcm)
            {
                Utility.SendGCMNotification(UserPhone, textMessage);
            }
            if (IsSms)
            {
                Utility.sendSMS(textMessage, UserPhone);
            }

            return true;
        }

        private void InitParams()
        {
            var userSetting = DbContext.ViewUserSettings.Where(us => us.MobileNo == UserIdentification).First();
           
            
        }
    }

    public class BillingNotification : NotificationType
    {
       
        public BillingNotification(SocietyDBEntities dbContext, string userIdentification) :base(dbContext,userIdentification)
        {
            InitParams();
        }
        private void InitParams()
        {
            var userSetting = DbContext.ViewUserSettings.Where(us => us.MobileNo == UserIdentification).First();
            IsSms = userSetting.BillingSMS;
            IsEmail = userSetting.BillingMail;
            IsGcm = userSetting.BillingNotification;
            UserEmail = userSetting.EmailId;
            UserPhone = userSetting.MobileNo;
        }       
    }

    public class FormNotification : NotificationType
    {
        
        public FormNotification(SocietyDBEntities dbContext, string userIdentification) : base(dbContext, userIdentification)
        {
            InitParams();
        }
        private void InitParams()
        {
            var userSetting = DbContext.ViewUserSettings.Where(us => us.MobileNo == UserIdentification).First();
            IsSms = userSetting.forumSMS;
            IsEmail = userSetting.forumMail;
            IsGcm = userSetting.forumNotification;
        }
    }

    public class ComplaintNotification : NotificationType
    {
        
        public ComplaintNotification(SocietyDBEntities dbContext, string userIdentification) : base(dbContext,userIdentification)
        {
            InitParams();
        }
        private void InitParams()
        {
            var userSetting = DbContext.ViewUserSettings.Where(us => us.MobileNo == UserIdentification).First();
            IsSms = userSetting.ComplaintSMS;
            IsEmail = userSetting.ComplaintMail;
            IsGcm = userSetting.ComplaintNotification;
        }

        public bool NotifyEmployee(String Message, String MobileNumber)
        {
            try
            {
              var result =  Utility.sendSMS(Message, MobileNumber);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }


    public class NoticeNotification : NotificationType
    {
       
        public NoticeNotification(SocietyDBEntities dbContext, string userIdentification) : base(dbContext,userIdentification)
        {
            InitParams();
        }
        private void InitParams()
        {
            var userSetting = DbContext.ViewUserSettings.Where(us => us.MobileNo == UserIdentification).First();
            IsSms = userSetting.NoticeSMS;
            IsEmail = userSetting.NoticeMail;
            IsGcm = userSetting.NoticeNotification;
        }
    }

    public class VisitorNotification : NotificationType
    {
        public VisitorNotification(SocietyDBEntities dbContext, string userIdentification) : base(dbContext, userIdentification)
        {
            IsGcm = true;
        }

        public void NotifyVisitor(String Message, String MobileNUmber)
        {
            Utility.sendSMS(Message, MobileNUmber);
        }

    }
}