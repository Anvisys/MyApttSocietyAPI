using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public class ValidUser
    {
        public String result { get; set; }
        public String message { get; set; }
        public TotalUser UserData { get; set; }
        public List<ViewSocietyUser> SocietyUser = new List<ViewSocietyUser>();
    }

    
}