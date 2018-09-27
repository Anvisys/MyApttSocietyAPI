using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public class Complaint
    {
        public int CompID;
        public int UserID;
        public int CompType;
        public int CompSeverity;
        public int CompStatusID;
        public int AssignedTo;
        public String CompDescription;
        public String FlatNumber;

    }
}