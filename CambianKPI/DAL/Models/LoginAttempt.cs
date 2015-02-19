using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CambianKPI.DAL.Models
{
    public class LoginAttempt
    {
        public int LoginAttemptID { get; set; }
        public string AccountName { get; set; }
        public string AuthType { get; set; }
        public DateTime AttemptTime { get; set; }
        public string UserAgent { get; set; }
        public string BrowserVersion { get; set; }
        public string AppVersion { get; set; }
        public string HostName { get; set; }
        public bool Success { get; set; }
        public string RequestURL { get; set; }
    }
}