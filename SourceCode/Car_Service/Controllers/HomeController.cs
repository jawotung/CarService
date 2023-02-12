using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarServiceSystem.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["WorkerID"] != null)
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Dashboard" });
            }
            else
            {
                return View();
            }
        }

        public ActionResult GetCustomerPageInfo()
        {
            List<string> objServicesPhoto = new List<string>();

            try
            {

                string strPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\assets\img\", "carservices");
                foreach (string fileName in System.IO.Directory.GetFiles(strPath, "*.jpg"))
                {
                    string strFileName = fileName.Split('\\').Last();
                    objServicesPhoto.Add(strFileName);
                }

                return Json(new { success = true, data = objServicesPhoto }, JsonRequestBehavior.AllowGet);

            } catch (Exception err)
            {
                string errmsg;
                if (err.InnerException != null)
                    errmsg = "Error: " + err.InnerException.ToString();
                else
                    errmsg = "Error: " + err.Message.ToString();

                return Json(new { success = false, errors = errmsg }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}