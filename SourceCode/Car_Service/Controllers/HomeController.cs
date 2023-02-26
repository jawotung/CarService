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
            return View();
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
        }
        public ActionResult GetCustomerDetails(int ID)
        {
            OnlineJobOrder objGetCustomerDetails = new OnlineJobOrder();

            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ConnectionString.ToString()))

                {
                    myConnection.Open();
                    using (SqlCommand myCommand = myConnection.CreateCommand())
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.CommandText = "cCustomer_Login";

                        myCommand.Parameters.AddWithValue("@ID", ID);

                        using (SqlDataReader myReader = myCommand.ExecuteReader())
                        {
                            while (myReader.Read())
                            {
                                objGetCustomerDetails.ID = common.FgNullToInt(myReader["ID"]);
                                objGetCustomerDetails.UserID = common.FgNullToString(myReader["UserID"]);
                                objGetCustomerDetails.Password = common.FgNullToString(myReader["Password"]);
                                objGetCustomerDetails.FirstName = common.FgNullToString(myReader["FirstName"]);
                                objGetCustomerDetails.MiddleName = common.FgNullToString(myReader["MiddleName"]);
                                objGetCustomerDetails.LastName = common.FgNullToString(myReader["LastName"]);
                                objGetCustomerDetails.ContactNo = common.FgNullToString(myReader["ContactNo"]);
                                objGetCustomerDetails.EmailAddress = common.FgNullToString(myReader["EmailAddress"]);
                            }
                        }
                    }
                    myConnection.Close();
                }

                return Json(new { success = true, data = objGetCustomerDetails }, JsonRequestBehavior.AllowGet);

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
        public ActionResult GetJobOrderListByUserID()
        {
            int ID = Convert.ToInt16(Request["ID"]);
            List<MWalkIn> objJOList = new List<MWalkIn>();

            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ConnectionString.ToString()))

                {
                    myConnection.Open();
                    using (SqlCommand myCommand = myConnection.CreateCommand())
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.CommandText = "tCustomerJobOrder_GetList";

                        myCommand.Parameters.AddWithValue("@UserID", ID);

                        using (SqlDataReader myReader = myCommand.ExecuteReader())
                        {
                            while (myReader.Read())
                            {
                                objJOList.Add(new MWalkIn
                                {
                                    Row_Num = Convert.ToInt32(myReader["Row_Num"]),
                                    JONo = myReader["JONo"].ToString(),
                                    Startdate = Convert.ToDateTime(myReader["Startdate"]).ToString("yyyy/MM/dd"),
                                    ServiceName = myReader["ServiceName"].ToString(),
                                    Remarks = myReader["Remarks"].ToString(),
                                    CreateDate = Convert.ToDateTime(myReader["CreateDate"]).ToString("yyyy/MM/dd"),
                                });
                            }
                        }
                    }
                    myConnection.Close();
                }

                return Json(new { success = true, data = objJOList }, JsonRequestBehavior.AllowGet);

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
        public ActionResult LoginMeIn(CustomerLogin data)
        {
            string errmsg = "Invalid UserID or Password. Please try again.";

            try
            {
                Security ph = new Security();
                DataHelper dataHelper = new DataHelper();

                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ConnectionString.ToString()))
                {
                    myConnection.Open();
                    using (SqlCommand myCommand = myConnection.CreateCommand())
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.CommandText = "cCustomer_Login";

                        myCommand.Parameters.Clear();
                        myCommand.Parameters.AddWithValue("@LoginUserID", data.LoginUserID);
                        myCommand.Parameters.AddWithValue("@LoginPassword", data.LoginPassword.ToString());

                        using (SqlDataReader sdr = myCommand.ExecuteReader())
                        {
                            if (sdr.Read())
                            {
                                if (common.FgNullToInt(sdr["isDeleted"]) == 1)
                                {
                                    errmsg = "Deleted user. Please contact your admin or IT support";
                                }
                                else
                                {
                                    Session["CustomerID"] = Convert.ToInt32(sdr["ID"]);
                                    Session["CustomerUserID"] = sdr["UserID"].ToString();
                                    Session["FirstName"] = sdr["FirstName"].ToString();
                                    Session["LastName"] = sdr["LastName"].ToString();
                                    Session["FullName"] = sdr["FirstName"].ToString() + " " + sdr["LastName"].ToString();
                                    Session["LastName"] = sdr["LastName"].ToString();

                                    errmsg = "";
                                }
                            }
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
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        public ActionResult SaveOnlineJobOrder(OnlineJobOrder HeaderData, List<MJO_Detail> ServiceDetail, int IsNewCustomer)
        {
            string strCheckOtherSchedule = "";
            string strServiceID = "";
            int iDtCount = 0;

            int iType = 1; // Default as Online Job Order Type

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

                    if (iDtCount == 0)
                        strServiceID += x.ServiceID.ToString();
                    else
                        strServiceID += "," + x.ServiceID.ToString();
                    iDtCount = iDtCount + 1;
                }
                #endregion

                // Check other schedule
                strCheckOtherSchedule = CheckOtherSchedule(HeaderData.Startdate, strServiceID);
                if (strCheckOtherSchedule != "")
                {
                    string[] strScheduledServices = strCheckOtherSchedule.Split(',');
                    foreach(string x in strScheduledServices)
                    {
                        ModelErrors.Add("Cannot schedule " + x + " due to conflict schedule.");
                    }
                    return Json(new { success = false, errors = ModelErrors });
                }

                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ToString()))
                {
                    myConnection.Open();
                    using (SqlCommand myCommand = myConnection.CreateCommand())
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.CommandText = "tCustomerJobOrder_InsertUpdate";

                        myCommand.Parameters.Clear();
                        myCommand.Parameters.AddWithValue("@IsNewCustomer", IsNewCustomer);
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
                        myCommand.Parameters.AddWithValue("@CreateID", common.FgNullToZero(Session["CustomerID"]));

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

        private string CheckOtherSchedule(string Startdate, string ServiceID)
        {
            bool bError = false;
            string strErrorMessage = "";

            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ConnectionString.ToString()))

                {
                    myConnection.Open();
                    using (SqlCommand myCommand = myConnection.CreateCommand())
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.CommandText = "tCustomerJobOrder_CheckOtherSchedule";

                        myCommand.Parameters.Clear();
                        myCommand.Parameters.AddWithValue("@Startdate", Startdate);
                        myCommand.Parameters.AddWithValue("@ServiceID", ServiceID);

                        using (SqlDataReader myReader = myCommand.ExecuteReader())
                        {
                            while (myReader.Read())
                            {
                                bError = Convert.ToBoolean(myReader["Error"]);
                                strErrorMessage = myReader["ErrorMessage"].ToString();
                            }
                        }
                    }
                    myConnection.Close();
                }

                return strErrorMessage;

            }
            catch (Exception err)
            {
                throw err;
            };
        }
    }
}