using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotSistemi.Models
{
    public class User
    {
        public int USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string USER_PASSWORD { get; set; }
        public string USER_FIRSTNAME { get; set; }
        public string USER_LASTNAME { get; set; }
        public string USER_ROLE { get; set; }
        public bool REMEMBERME { get; set; }
    }
}