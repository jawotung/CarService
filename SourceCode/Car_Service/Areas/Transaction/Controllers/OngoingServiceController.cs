using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarService.Areas.Transaction.Controllers
{
    public class OngoingServiceController : Controller
    {
        // GET: Transaction/OngoingService
        public ActionResult Index()
        {
            return View("OngoingService");
        }
    }
}