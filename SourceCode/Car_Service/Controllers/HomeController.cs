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

        public ActionResult SaveCustomer(MWalkIn data)
        {
            try
            {
                using (SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ToString()))
                {
                    myConnection.Open();
                    using (SqlCommand myCommand = myConnection.CreateCommand())
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.CommandText = "mCustomer_InsertUpdate";
                        myCommand.Parameters.Clear();
                        myCommand.Parameters.AddWithValue("@UserID", data.UserID);
                        myCommand.Parameters.AddWithValue("@Password", common.FgDBString(data.Password));
                        myCommand.Parameters.AddWithValue("@FirstName", common.FgDBString(data.FirstName));
                        myCommand.Parameters.AddWithValue("@MiddleName", common.FgDBString(data.MiddleName));
                        myCommand.Parameters.AddWithValue("@LastName", common.FgDBString(data.LastName));
                        myCommand.Parameters.AddWithValue("@Password", common.FgDBString(data.Password));
                        myCommand.Parameters.AddWithValue("@ContactNo", common.FgDBString(data.ContactNo));
                        myCommand.Parameters.AddWithValue("@EmailAddress", common.FgDBString(data.EmailAddress));
                        myCommand.Parameters.AddWithValue("@CreateID", Session["ID"]);
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