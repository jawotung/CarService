using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarService.Areas.Report.Controllers
{
    public class TransactionHistoryController : Controller
    {
        // GET: Report/TransactionHistory
        public ActionResult Index()
        {
            return View("TransactionHistory");
        }
    }
}