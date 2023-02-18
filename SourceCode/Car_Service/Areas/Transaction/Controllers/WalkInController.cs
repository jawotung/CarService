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
    public class WalkInController : Controller
    {
        readonly List<string> ModelErrors = new List<string>();
        readonly Common common = new Common();
        // GET: Transaction/WalkIn
        public ActionResult Index()
        {
            return View("WalkIn");
        }
        public ActionResult SaveWalkIn(MWalkIn data, MJO_Detail Detail)
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
                            cmdSql.CommandType = CommandType.StoredProcedure;
                            cmdSql.CommandText = "mWalkIn_InsertUpdate";
                            cmdSql.Parameters.Clear(); 
                            cmdSql.Parameters.AddWithValue("@UserID", common.FgNullToString(data.UserID));
                            cmdSql.Parameters.AddWithValue("@Password", common.FgNullToString(data.Password));
                            cmdSql.Parameters.AddWithValue("@FirstName", common.FgNullToString(data.FirstName));
                            cmdSql.Parameters.AddWithValue("@MiddleName", common.FgNullToString(data.MiddleName));
                            cmdSql.Parameters.AddWithValue("@LastName", common.FgNullToString(data.LastName));
                            cmdSql.Parameters.AddWithValue("@ContactNo", common.FgNullToString(data.ContactNo));
                            cmdSql.Parameters.AddWithValue("@EmailAddress", common.FgNullToString(data.EmailAddress));
                            cmdSql.Parameters.AddWithValue("@Type", common.FgNullToString(data.Type));
                            cmdSql.Parameters.AddWithValue("@ServiceID", common.FgNullToString(data.ServiceID));
                            cmdSql.Parameters.AddWithValue("@Startdate", common.FgNullToString(data.Startdate));
                            cmdSql.Parameters.AddWithValue("@Enddate", common.FgNullToString(data.Enddate));
                            cmdSql.Parameters.AddWithValue("@Remarks", common.FgNullToString(data.Remarks));
                            cmdSql.Parameters.AddWithValue("@CreateID", Session["ID"]);
                            SqlParameter ErrorMessage = cmdSql.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000);
                            SqlParameter Error = cmdSql.Parameters.Add("@Error", SqlDbType.Bit);

                            Error.Direction = ParameterDirection.Output;
                            ErrorMessage.Direction = ParameterDirection.Output;

                            cmdSql.ExecuteNonQuery();

                            if (Convert.ToBoolean(Error.Value))
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
            if (ModelErrors.Count != 0)
                return Json(new { success = false, errors = ModelErrors });
            else
            {
                return Json(new { success = true, msg = "WalkIn was successfully saved" });
            }
        }
        public ActionResult DeleteWalkIn(string ID)
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
                            cmdSql.CommandText = "mWalkIn_Delete";
                            cmdSql.Parameters.Clear();
                            cmdSql.Parameters.AddWithValue("@ID", ID);
                            cmdSql.Parameters.AddWithValue("@UpdateID", Session["ID"]);

                            SqlParameter Error = cmdSql.Parameters.Add("@Error", SqlDbType.Bit);
                            SqlParameter ErrorMessage = cmdSql.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000);

                            Error.Direction = ParameterDirection.Output;
                            ErrorMessage.Direction = ParameterDirection.Output;

                            cmdSql.ExecuteNonQuery();
                            if (Convert.ToBoolean(Error.Value))
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
            if (ModelErrors.Count != 0)
                return Json(new { success = false, errors = ModelErrors });
            else
                return Json(new { success = true, msg = "WalkIns was successfully deleted." });

        }
    }
}