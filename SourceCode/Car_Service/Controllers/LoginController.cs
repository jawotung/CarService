using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using CarService.Models;
using System.Collections;
using System.Linq;
using System.IO;
using System.Net.Http;
using CarService.Areas.MasterMaintenance.Models;

namespace CarServiceSystem.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        // GET: Login
        readonly Common common = new Common();
        public ActionResult Index()
        {
            if (Session["WorkerID"] != null)
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Dashboard" });
            }
            else
            {
                return View("Login");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult LoginEntry(Login data)
        {
            string errmsg = "";
            try
            {
                Security ph = new Security();
                DataHelper dataHelper = new DataHelper();

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        try
                        {
                            cmdSql.CommandType = CommandType.StoredProcedure;
                            cmdSql.CommandText = "cWorker_Login";
                            cmdSql.Parameters.Clear();
                            cmdSql.Parameters.AddWithValue("@WorkerID", data.WorkerID);
                            //cmdSql.Parameters.AddWithValue("@Password", ph.Base64Encode(data.Password.ToString()));
                            cmdSql.Parameters.AddWithValue("@Password", data.Password);
                            using (SqlDataReader sdr = cmdSql.ExecuteReader())
                            {
                                if (sdr.Read())
                                {
                                    if (common.FgNullToInt(sdr["isDeleted"]) == 1)
                                    {
                                        errmsg = "Deleted user. Please contact your admin or IT support";
                                    }
                                    else
                                    {
                                        Session["ID"] = Convert.ToInt32(sdr["ID"]);
                                        Session["WorkerID"] = sdr["WorkerID"].ToString();
                                        Session["FirstName"] = sdr["FirstName"].ToString();
                                        Session["LastName"] = sdr["LastName"].ToString();
                                        Session["FullName"] = sdr["FirstName"].ToString() + " " + sdr["LastName"].ToString();
                                        Session["LastName"] = sdr["LastName"].ToString();
                                        Session["Position"] = common.FgNullToInt(sdr["Position"]);
                                    }
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            throw new InvalidOperationException(err.Message);
                        }
                        finally
                        {
                            cmdSql.Dispose();
                            conn.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                if (err.InnerException != null)
                    errmsg = "Error: " + err.InnerException.ToString();
                else
                    errmsg = "Error: " + err.Message.ToString();
            }
            if (!String.IsNullOrEmpty(errmsg))
                return Json(new { success = true, data = new { error = true, errmsg } });
            else
            {
                return Json(new { success = true, data = new { error = false } });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login", new { area = "" });
        }

        public ActionResult SessionError()
        {
            return Json(new { success = false, type = "Login", errors = "Session has expired. Please login again. Thank you." }, JsonRequestBehavior.AllowGet);
        }

    }
}
