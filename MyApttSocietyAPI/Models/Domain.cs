using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApttSocietyAPI.Models
{
    public class Domain
    {
        public int ID { get; set; }

        public String Value { get; set; }
    }

    public class DomainInfo
    {


        public List<Domain> ComplaintType { get; set; }


        public List<Domain> Severity { get; set; }


        public List<Domain> ComplaintStatus { get; set; }

    }
}