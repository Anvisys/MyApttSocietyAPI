using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public class Forum
    {
        public int ID;
        public int ThreadID;
        public int resID;
        public int Initiater;
        public int CurrentPostBy;
        public String Topic;
        public String FirstThread;
        public String CurrentThread;
        public DateTime InitiatedAt;
        public DateTime ModifiedAt;
        public int SocietyId;
      
    }
}