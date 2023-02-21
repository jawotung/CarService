using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarService.Areas.Transaction.Models;
using CarService.Areas.MasterMaintenance.Models;
using CarService.Models;

namespace CarServiceSystem.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        readonly Security ph = new Security();
        readonly List<string> ModelErrors = new List<string>();
        readonly Common common = new Common();
        bool error = false;

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

        public ActionResult GetServices()
        {
            List<MService> objServiceList = new List<MService>();

            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ConnectionString.ToString()))

                {
                    myConnection.Open();
                    using (SqlCommand myCommand = myConnection.CreateCommand())
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.CommandText = "mService_GetList";

                        using (SqlDataReader sdr = myCommand.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                objServiceList.Add(new MService
                                {
                                    ID = common.FgNullToInt(sdr["ID"]),
                                    ServiceName = sdr["ServiceName"].ToString(),
                                    DurationTo = sdr["DurationTo"].ToString(),
                                    DurationFrom = sdr["DurationFrom"].ToString(),
                                    Duration = sdr["DurationFrom"].ToString() + " - " + sdr["DurationTo"].ToString(),
                                    Amount = sdr["Amount"].ToString(),
                                    Position = common.FgNullToInt(sdr["Position"]),
                                    PositionName = sdr["PositionName"].ToString(),
                                });
                            }
                        }
                    }
                    myConnection.Close();
                }

                return Json(new { success = true, data = objServiceList }, JsonRequestBehavior.AllowGet);

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
        }

        public ActionResult SaveOnlineJobOrder(OnlineJobOrder HeaderData, List<MJO_Detail> ServiceDetail)
        {
            int iType = 1; // Default as Online Job Order Type
            int iIsNewCustomer = 1; // Default as New Customer

            try
            {
                #region CreateDataTable
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("ServiceID", typeof(int)));
                dt.Columns.Add(new DataColumn("Price", typeof(string)));

                foreach (MJO_Detail x in ServiceDetail)
                {
                    DataRow dr = dt.NewRow();
                    dr["ServiceID"] = x.ServiceID;
                    dr["Price"] = common.FgNullToString(x.Price);
                    dt.Rows.Add(dr);
                }
                #endregion

                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ToString()))
                {
                    myConnection.Open();
                    using (SqlCommand myCommand = myConnection.CreateCommand())
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.CommandText = "tCustomerJobOrder_InsertUpdate";

                        myCommand.Parameters.Clear();
                        myCommand.Parameters.AddWithValue("@IsNewCustomer", iIsNewCustomer);
                        myCommand.Parameters.AddWithValue("@UserID", common.FgNullToString(HeaderData.UserID));
                        myCommand.Parameters.AddWithValue("@Password", common.FgNullToString(HeaderData.Password));
                        myCommand.Parameters.AddWithValue("@FirstName", common.FgNullToString(HeaderData.FirstName));
                        myCommand.Parameters.AddWithValue("@MiddleName", common.FgNullToString(HeaderData.MiddleName));
                        myCommand.Parameters.AddWithValue("@LastName", common.FgNullToString(HeaderData.LastName));
                        myCommand.Parameters.AddWithValue("@ContactNo", common.FgNullToString(HeaderData.ContactNo));
                        myCommand.Parameters.AddWithValue("@EmailAddress", common.FgNullToString(HeaderData.EmailAddress));
                        myCommand.Parameters.AddWithValue("@Type", iType);
                        myCommand.Parameters.AddWithValue("@Startdate", common.FgNullToString(HeaderData.Startdate));
                        myCommand.Parameters.AddWithValue("@Remarks", common.FgNullToString(HeaderData.Remarks));

                        SqlParameter tvpParam = myCommand.Parameters.AddWithValue("@dt_tCustomerJobOrder_Detail", dt);
                        tvpParam.SqlDbType = SqlDbType.Structured;
                        tvpParam.TypeName = "dt_tCustomerJobOrder_Detail";

                        SqlParameter ErrorMessage = myCommand.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000);
                        SqlParameter Error = myCommand.Parameters.Add("@Error", SqlDbType.Bit);

                        Error.Direction = ParameterDirection.Output;
                        ErrorMessage.Direction = ParameterDirection.Output;

                        myCommand.ExecuteNonQuery();

                        error = Convert.ToBoolean(Error.Value);
                        if (error)
                            ModelErrors.Add(ErrorMessage.Value.ToString());
                    }
                    myConnection.Close();
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
    }
}