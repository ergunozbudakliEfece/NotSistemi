using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NotSistemi.Models
{
    public class Ticket
    {
        public int TICKET_ID { get; set; }

        public string USER_NAME { get; set; }

       
        public string DATE { get; set; }
        public string ISSUE { get; set; }
        public bool STATUS { get; set; }
        public string SOLUTION { get; set; }
    }
}