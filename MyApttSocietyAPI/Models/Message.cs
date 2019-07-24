using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
   

    public class Message
    {

        public string Topic { get; set; }

        public int SocietyID { get; set; }

        public string TextMessage { get; set; }
    }
}