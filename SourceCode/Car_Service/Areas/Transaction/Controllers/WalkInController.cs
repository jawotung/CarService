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
        [AllowAnonymous]
        public ActionResult GetWalkInList()
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
                            cmdSql.CommandText = "tWalkIn_GetList";
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
        public ActionResult SaveWalkIn(MWalkIn data,List<MJO_Detail> Detail,Boolean IsNewCustomer)
        {
            try
            {
                #region CreateDataTable
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("ServiceID", typeof(int)));
                dt.Columns.Add(new DataColumn("Price", typeof(string)));
                foreach (MJO_Detail x in Detail)
                {
                    DataRow dr = dt.NewRow();
                    dr["ServiceID"] = x.ServiceID;
                    dr["Price"] =common.FgNullToString(x.Price);
                    dt.Rows.Add(dr);
                }
                #endregion
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        try
                        {
                            cmdSql.CommandType = CommandType.StoredProcedure;
                            cmdSql.CommandText = "tCustomerJobOrder_InsertUpdate";
                            cmdSql.Parameters.Clear();
                            cmdSql.Parameters.AddWithValue("@IsNewCustomer",Convert.ToInt32(IsNewCustomer));
                            cmdSql.Parameters.AddWithValue("@UserID", common.FgNullToString(data.UserID));
                            cmdSql.Parameters.AddWithValue("@Password", common.FgNullToString(data.Password));
                            cmdSql.Parameters.AddWithValue("@FirstName", common.FgNullToString(data.FirstName));
                            cmdSql.Parameters.AddWithValue("@MiddleName", common.FgNullToString(data.MiddleName));
                            cmdSql.Parameters.AddWithValue("@LastName", common.FgNullToString(data.LastName));
                            cmdSql.Parameters.AddWithValue("@ContactNo", common.FgNullToString(data.ContactNo));
                            cmdSql.Parameters.AddWithValue("@EmailAddress", common.FgNullToString(data.EmailAddress));
                            cmdSql.Parameters.AddWithValue("@Type", 2);
                            cmdSql.Parameters.AddWithValue("@Startdate", common.FgNullToString(data.Startdate));
                            cmdSql.Parameters.AddWithValue("@Remarks", common.FgNullToString(data.Remarks));
                            SqlParameter tvpParam = cmdSql.Parameters.AddWithValue("@dt_tCustomerJobOrder_Detail", dt);
                            tvpParam.SqlDbType = SqlDbType.Structured;
                            tvpParam.TypeName = "dt_tCustomerJobOrder_Detail";
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

                return Json(new { success = false, msg = errmsg }, JsonRequestBehavior.AllowGet);
            }
            if (ModelErrors.Count != 0)
                return Json(new { success = false, msg = ModelErrors });
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