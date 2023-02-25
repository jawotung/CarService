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

namespace CarService.Areas.Transaction.Controllers
{
    public class AssignWorkerController : Controller
    {
        // GET: Transaction/AssignWorker
        readonly List<string> ModelErrors = new List<string>();
        readonly Common common = new Common();
        public ActionResult Index()
        {
            return View("AssignWorker");
        }
        [AllowAnonymous]
        public ActionResult GetAssignWorkerList()
        {
            List<MWalkIn> data = new List<MWalkIn>();
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
                            cmdSql.CommandText = "tAssignWorker_GetList";
                            cmdSql.Parameters.Clear();
                            using (SqlDataReader sdr = cmdSql.ExecuteReader())
                            {
                                while (sdr.Read())
                                {
                                    data.Add(new MWalkIn
                                    {
                                        ID = common.FgNullToInt(sdr["ID"]),
                                        JONo = sdr["JONo"].ToString(),
                                        FullName = sdr["LastName"].ToString() + ", " + sdr["FirstName"].ToString() + " " + sdr["MiddleName"].ToString(),
                                        UserID = sdr["UserID"].ToString(),
                                        FirstName = sdr["FirstName"].ToString(),
                                        MiddleName = sdr["MiddleName"].ToString(),
                                        LastName = sdr["LastName"].ToString(),
                                        EmailAddress = sdr["EmailAddress"].ToString(),
                                        ContactNo = sdr["ContactNo"].ToString(),
                                        Password = sdr["Password"].ToString(),
                                        ServiceName = sdr["ServiceName"].ToString(),
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
        public ActionResult GetServiceList()
        {
            List<MWalkIn> data = new List<MWalkIn>();
            int ID = Request["ID"] == "" ? 0 : Convert.ToInt32(Request["ID"]);
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
                            cmdSql.CommandText = "tAssignWorker_Service";
                            cmdSql.Parameters.Clear();
                            cmdSql.Parameters.AddWithValue("@ID", ID);
                            using (SqlDataReader sdr = cmdSql.ExecuteReader())
                            {
                                while (sdr.Read())
                                {
                                    data.Add(new MWalkIn
                                    {
                                        ID = common.FgNullToInt(sdr["ID"]),
                                        JODetailID = common.FgNullToInt(sdr["JODetailID"]),
                                        ServiceName = sdr["ServiceName"].ToString(),
                                        Startdate = common.FgNullToDateTime(sdr["Startdate"])
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
        public ActionResult SaveWalkIn(List<MAssignWorker> data)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ToString()))
                {
                    conn.Open();
                    SqlTransaction transaction;
                    transaction = conn.BeginTransaction();
                    foreach (MAssignWorker x in data)
                    {
                        using (SqlCommand cmdSql = conn.CreateCommand())
                        {
                            try
                            {
                                cmdSql.Transaction = transaction;
                                cmdSql.CommandType = CommandType.StoredProcedure;
                                cmdSql.CommandText = "tAssignWorker_InsertUpdate";
                                cmdSql.Parameters.Clear();
                                cmdSql.Parameters.AddWithValue("@JODetailID", x.JODetailID);
                                cmdSql.Parameters.AddWithValue("@WorkerID", x.WorkerID);
                                cmdSql.Parameters.AddWithValue("@CreateID", Session["WorkerID"]);
                                SqlParameter ErrorMessage = cmdSql.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000);
                                SqlParameter Error = cmdSql.Parameters.Add("@Error", SqlDbType.Bit);

                                Error.Direction = ParameterDirection.Output;
                                ErrorMessage.Direction = ParameterDirection.Output;

                                cmdSql.ExecuteNonQuery();

                                if (Convert.ToBoolean(Error.Value))
                                {
                                    throw new InvalidOperationException(ErrorMessage.Value.ToString());
                                }
                            }
                            catch (Exception err)
                            {
                                transaction.Rollback();
                                cmdSql.Dispose();
                                conn.Close();
                                throw new InvalidOperationException(err.Message);
                            }
                        }
                    }
                    transaction.Commit();
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                string errmsg;
                if (err.InnerException != null)
                    errmsg = "Error: " + err.InnerException.ToString();
                else
                    errmsg = "Error: " + err.Message.ToString();

                return Json(new { success = false, msg = errmsg }, JsonRequestBehavior.AllowGet);
            }
            if(ModelErrors.Count != 0)
                return Json(new { success = false, msg = ModelErrors });
            else
            {
                return Json(new { success = true, msg = "WalkIn was successfully saved" });
            }
        }
    }
}