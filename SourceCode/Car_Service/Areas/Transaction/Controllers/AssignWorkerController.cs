using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarService.Areas.Transaction.Controllers
{
    public class AssignWorkerController : Controller
    {
        // GET: Transaction/AssignWorker
        public ActionResult Index()
        {
            return View("AssignWorker");
        }
    }
}