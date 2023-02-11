using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarService.Models
{
    public class MAPIReturn
    {
        public bool Status { get; set; }
        public string Msg { get; set; }
        public List<Dictionary<string, object>> ReturnData { get; set; }
    }
}