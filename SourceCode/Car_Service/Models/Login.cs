using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarService.Models
{
    public class Login
    {
        public string WorkerID { get; set; }
        public string Password { get; set; }
    }

    public class CustomerLogin
    {
        public string LoginUserID { get; set; }
        public string LoginPassword { get; set; }
    }
}