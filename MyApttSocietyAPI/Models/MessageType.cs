﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace MyApttSocietyAPI.Models
{
    public abstract class NotificationType
    {
        protected NestinDBEntities DbContext { get; private set; }

        public NotificationType(NestinDBEntities dbContext, string userIdentification)
        {
         DbContext = dbContext;
         UserIdentification = userIdentification;
         //InitParams();
        }

        public string UserIdentification { get; protected set; }
        public string UserEmail { get; protected set; }
        public string UserPhone { get; protected set; }
        public bool IsSms { get; protected set; }
        public bool IsEmail { get; protected set; }
        public bool IsGcm { get; protected set; }

        public bool NotifyResidents(Message message, String Heading)
        {
            String textMessage = JsonConvert.SerializeObject(message);
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
            var userSetting = DbContext.ViewNewUserSettings.Where(us => us.MobileNo == UserIdentification).First();
           
            
        }
    }

    public class BillingNotification : NotificationType
    {
       
        public BillingNotification(NestinDBEntities dbContext, string userIdentification) :base(dbContext,userIdentification)
        {
            InitParams();
        }
        private void InitParams()
        {
            var userSetting = DbContext.ViewNewUserSettings.Where(us => us.MobileNo == UserIdentification).First();
            //IsSms = userSetting.BillingSMS;
            //IsEmail = userSetting.BillingMail;
            //IsGcm = userSetting.BillingNotification;
            UserEmail = userSetting.EmailId;
            UserPhone = userSetting.MobileNo;
        }       
    }

    public class FormNotification : NotificationType
    {
        
        public FormNotification(NestinDBEntities dbContext, string userIdentification) : base(dbContext, userIdentification)
        {
            InitParams();
        }
        private void InitParams()
        {
            var userSetting = DbContext.ViewNewUserSettings.Where(us => us.MobileNo == UserIdentification).First();
            //IsSms = userSetting.forumSMS;
            //IsEmail = userSetting.forumMail;
            //IsGcm = userSetting.forumNotification;
        }
    }

    public class ComplaintNotification : NotificationType
    {
        
        public ComplaintNotification(NestinDBEntities dbContext, string userIdentification) : base(dbContext,userIdentification)
        {
            InitParams();
        }
        private void InitParams()
        {
            var userSetting = DbContext.ViewNewUserSettings.Where(us => us.MobileNo == UserIdentification).First();
            //IsSms = userSetting.ComplaintSMS;
            //IsEmail = userSetting.ComplaintMail;
            //IsGcm = userSetting.ComplaintNotification;
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
       
        public NoticeNotification(NestinDBEntities dbContext, string userIdentification) : base(dbContext,userIdentification)
        {
            InitParams();
        }
        private void InitParams()
        {
            var userSetting = DbContext.ViewNewUserSettings.Where(us => us.MobileNo == UserIdentification).First();
            //IsSms = userSetting.NoticeSMS;
            //IsEmail = userSetting.NoticeMail;
            //IsGcm = userSetting.NoticeNotification;
        }
    }

    public class VisitorNotification : NotificationType
    {
        public VisitorNotification(NestinDBEntities dbContext, string userIdentification) : base(dbContext, userIdentification)
        {
            IsGcm = true;
        }

        public String NotifyVisitor(String Message, String MobileNUmber)
        {
          return  Utility.sendSMS(Message, MobileNUmber);
        }

    }
}