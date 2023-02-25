using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using CarService.Areas.Transaction.Models;
using CarService.Models;
using System.IO;

namespace CarService.Areas.Report.Controllers
{
    public class TransactionHistoryController : Controller
    {
        readonly List<string> ModelErrors = new List<string>();
        readonly Common common = new Common();
        // GET: Report/TransactionHistory
        public ActionResult Index()
        {
            return View("TransactionHistory");
        }
        [AllowAnonymous]
        public ActionResult GetTransactionHistoryList()
        {
            List<MWalkIn> data = new List<MWalkIn>();
            string StartDate = Request["StartDate"];
            string EndDate = Request["EndDate"];
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ConnectionString.ToString()))

                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        try
                        {
                            cmdSql.CommandType = CommandType.StoredProcedure;
                            cmdSql.CommandText = "rTransactionHistory_GetList";
                            cmdSql.Parameters.Clear();
                            cmdSql.Parameters.AddWithValue("@StartDate", StartDate);
                            cmdSql.Parameters.AddWithValue("@EndDate", EndDate);
                            using (SqlDataReader sdr = cmdSql.ExecuteReader())
                            {
                                while (sdr.Read())
                                {
                                    data.Add(new MWalkIn
                                    {
                                        JODetailID = common.FgNullToInt(sdr["JODetailID"]),
                                        JONo = sdr["JONo"].ToString(),
                                        FullName = sdr["LastName"].ToString() + ", " + sdr["FirstName"].ToString() + " " + sdr["MiddleName"].ToString(),
                                        Worker = sdr["WLastName"].ToString() + ", " + sdr["WFirstName"].ToString() + " " + sdr["WMiddleName"].ToString(),
                                        UserID = sdr["UserID"].ToString(),
                                        ServiceName = sdr["ServiceName"].ToString(),
                                        Startdate = sdr["Startdate"].ToString(),
                                        Enddate = sdr["Enddate"].ToString(),
                                        Price = sdr["Price"].ToString(),
                                    });
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
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                string errmsg;
                if (err.InnerException != null)
                    errmsg = "An error occured: " + err.InnerException.ToString();
                else
                    errmsg = "An error occured: " + err.Message.ToString();

                return Json(new { success = false, msg = errmsg }, JsonRequestBehavior.AllowGet);
            };

            return Json(new { data, draw = Request["draw"], recordsTotal = data.Count, recordsFiltered = data.Count }, JsonRequestBehavior.AllowGet);

        }
    }
}