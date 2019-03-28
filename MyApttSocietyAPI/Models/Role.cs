using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public class Role
    {

        public String FlatNumber { get; set; }
        public String Floor { get; set; }
      public String Block { get; set; }
      public String IntercomNumber { get; set; }
      public String BHK { get; set; }
      public String UserID { get; set; }
      public int FlatArea { get; set; }
      public int SocietyID { get; set; }
    }
}