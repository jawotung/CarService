using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using CarService.Areas.MasterMaintenance.Models;
using CarService.Models;
using System.IO;

namespace CarService.Areas.MasterMaintenance.Controllers
{
    public class WorkerMasterController : Controller
    {
        readonly Security ph = new Security();
        readonly List<string> ModelErrors = new List<string>();
        readonly Common common = new Common();
        bool error = false;

        public ActionResult Index()
        {
            return View("WorkerMaster");
        }
        [AllowAnonymous]
        public ActionResult GetWorkerList()
        {
            List<MWorker> data = new List<MWorker>();
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
                            cmdSql.CommandText = "mWorker_GetList";
                            cmdSql.Parameters.Clear();
                           using (SqlDataReader sdr = cmdSql.ExecuteReader())
                            {
                                while (sdr.Read())
                                {
                                    data.Add(new MWorker
                                    {
                                        ID = common.FgNullToInt(sdr["ID"]),
                                        FullName = sdr["LastName"].ToString() + ", " + sdr["FirstName"].ToString() + " " + sdr["MiddleName"].ToString(),
                                        WorkerID = sdr["WorkerID"].ToString(),
                                        FirstName = sdr["FirstName"].ToString(),
                                        MiddleName = sdr["MiddleName"].ToString(),
                                        LastName = sdr["LastName"].ToString(),
                                        Email = sdr["Email"].ToString(),
                                        ContactNo = sdr["ContactNo"].ToString(),
                                        Password = ph.Base64Decode(sdr["Password"].ToString()),
                                        Gender = sdr["Gender"].ToString(),
                                        GenderName = sdr["Gender"].ToString() == "0" ? "Male" : "Female",
                                        Position = common.FgNullToInt(sdr["Position"]),
                                        PositionName = sdr["PositionName"].ToString(),
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
        public ActionResult SaveWorker(MWorker data)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(data.Password))
                                data.Password = ph.Base64Encode(data.Password).ToString();
                            cmdSql.CommandType = CommandType.StoredProcedure;
                            cmdSql.CommandText = "mWorker_InsertUpdate";
                            cmdSql.Parameters.Clear();
                            cmdSql.Parameters.AddWithValue("@ID", data.ID);
                            cmdSql.Parameters.AddWithValue("@WorkerID", common.FgDBString(data.WorkerID));
                            cmdSql.Parameters.AddWithValue("@FirstName", common.FgDBString(data.FirstName));
                            cmdSql.Parameters.AddWithValue("@MiddleName", common.FgDBString(data.MiddleName));
                            cmdSql.Parameters.AddWithValue("@LastName", common.FgDBString(data.LastName));
                            cmdSql.Parameters.AddWithValue("@Password", common.FgDBString(data.Password));
                            cmdSql.Parameters.AddWithValue("@Position", common.FgDBString(data.Position));
                            cmdSql.Parameters.AddWithValue("@Gender", common.FgDBString(data.Gender));
                            cmdSql.Parameters.AddWithValue("@Email", common.FgDBString(data.Email));
                            cmdSql.Parameters.AddWithValue("@ContactNo", common.FgDBString(data.ContactNo));
                            cmdSql.Parameters.AddWithValue("@CreateID", Session["ID"]);
                            SqlParameter ErrorMessage = cmdSql.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000);
                            SqlParameter Error = cmdSql.Parameters.Add("@Error", SqlDbType.Bit);

                            Error.Direction = ParameterDirection.Output;
                            ErrorMessage.Direction = ParameterDirection.Output;

                            cmdSql.ExecuteNonQuery();

                            error = Convert.ToBoolean(Error.Value);
                            if (error)
                                ModelErrors.Add(ErrorMessage.Value.ToString());
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
                    errmsg = "Error: " + err.InnerException.ToString();
                else
                    errmsg = "Error: " + err.Message.ToString();

                return Json(new { success = false, errors = errmsg }, JsonRequestBehavior.AllowGet);
            }
            if (ModelErrors.Count != 0 || error)
                return Json(new { success = false, errors = ModelErrors });
            else
            {
                return Json(new { success = true, msg = "Worker was successfully saved" });
            }
        }
        public ActionResult DeleteWorker(string ID)
        {
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
                            cmdSql.CommandText = "mWorker_Delete";
                            cmdSql.Parameters.Clear();
                            cmdSql.Parameters.AddWithValue("@ID", ID);
                            cmdSql.Parameters.AddWithValue("@UpdateID", Session["ID"]);

                            SqlParameter Error = cmdSql.Parameters.Add("@Error", SqlDbType.Bit);
                            SqlParameter ErrorMessage = cmdSql.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000);

                            Error.Direction = ParameterDirection.Output;
                            ErrorMessage.Direction = ParameterDirection.Output;

                            cmdSql.ExecuteNonQuery();
                            error = Convert.ToBoolean(Error.Value);
                            if (error)
                                ModelErrors.Add(ErrorMessage.Value.ToString());
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
                    errmsg = "Error: " + err.InnerException.ToString();
                else
                    errmsg = "Error: " + err.Message.ToString();

                return Json(new { success = false, errors = errmsg }, JsonRequestBehavior.AllowGet);
            }
            if (ModelErrors.Count != 0 || error)
                return Json(new { success = false, errors = ModelErrors });
            else
                return Json(new { success = true, msg = "Workers was successfully deleted." });

        }

    }
}
