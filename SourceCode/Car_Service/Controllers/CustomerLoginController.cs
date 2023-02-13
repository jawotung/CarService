using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarService.Controllers
{
    public class CustomerLoginController : Controller
    {
        // GET: CustomerLogin
        public ActionResult Index()
        {
            return View("CustomerLogin");
        }
    }
}