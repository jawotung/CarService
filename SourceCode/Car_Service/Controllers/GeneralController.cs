using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarService.Controllers
{
    [AllowAnonymous]
    public class GeneralController : Controller
    {
        public ActionResult GetSelect2Data()
        {
            ArrayList results = new ArrayList();
            string val = Request.QueryString["q"];
            string id = Request.QueryString["id"];
            string text = Request.QueryString["text"];
            string table = Request.QueryString["table"];
            string condition = String.IsNullOrEmpty(Request.QueryString["condition"]) ? "" : Request.QueryString["condition"];
            string isDistict = String.IsNullOrEmpty(Request.QueryString["isDistict"]) ? "" : "DISTINCT";
            string addOptionVal = Request.QueryString["addOptionVal"];
            string addOptionText = Request.QueryString["addOptionText"];
            string query = Request.QueryString["query"];
            string ColData = String.IsNullOrEmpty(Request.QueryString["ColData"])  ? "" : Request.QueryString["ColData"];
            string db = String.IsNullOrEmpty(Request.QueryString["db"]) ? "CarService" : Request.QueryString["db"];
            condition = condition.Replace("null", "");
            if (addOptionVal != null)
                results.Add(new { id = addOptionVal, text = addOptionText });

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[db].ConnectionString.ToString()))

                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        try
                        {
                            if (query == null || query == "")
                            {

                                #region
                                cmdSql.CommandType = CommandType.Text;
                                cmdSql.CommandText = " SELECT" + isDistict + "  TOP 1000  " + id + " AS id," + text + " AS text " + ColData +
                                                      " FROM " + table + 
                                                      " WHERE (" + id + " like '%" + val + "%' OR " + text + " like '%" + val + "%')  " + 
                                                      condition +
                                                     " ORDER BY text";
                               using (SqlDataReader sdr = cmdSql.ExecuteReader())
                                {
                                    while (sdr.Read())
                                    {
                                        if(ColData != "") 
                                            results.Add(new { id = sdr["id"].ToString().Trim(), text = sdr["text"].ToString().Trim(), ColData = sdr["ColData"].ToString().Trim() });
                                        else
                                            results.Add(new { id = sdr["id"].ToString().Trim(), text = sdr["text"].ToString().Trim() });

                                    }

                                }
                                #endregion
                            }
                            else
                            {
                                #region
                                cmdSql.CommandType = CommandType.Text;
                                cmdSql.CommandText = query;
                               using (SqlDataReader sdr = cmdSql.ExecuteReader())
                                {
                                    while (sdr.Read())
                                    {
                                        results.Add(new { id = sdr["id"].ToString(), text = sdr["text"].ToString() });
                                    }

                                }
                                #endregion
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
                string errmsg;
                if (err.InnerException != null)
                    errmsg = "An error occured: " + err.InnerException.ToString();
                else
                    errmsg = "An error occured: " + err.Message.ToString();

                return Json(new { success = false, msg = errmsg }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { results }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSelect2SP()
        {
            List<Select2Data> results = new List<Select2Data>();
            string val = Request.QueryString["q"];
            string sp = Request.QueryString["sp"];
            string db = Request.QueryString["db"];

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[db].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        try
                        {
                            cmdSql.CommandType = CommandType.Text;
                            cmdSql.CommandText = sp;
                            cmdSql.Parameters.Clear();
                            cmdSql.ExecuteNonQuery();
                           using (SqlDataReader sdr = cmdSql.ExecuteReader())
                            {
                                while (sdr.Read())
                                {
                                    results.Add(new Select2Data
                                    {
                                        ID = sdr["ID"].ToString(),
                                        Text = sdr["text"].ToString(),
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
            }
            if (!string.IsNullOrEmpty(val))//filter
                results = results.Where(x =>x.Text.ToLower().Contains(val.ToLower())).ToList<Select2Data>();

            return Json(new { results }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetNotifications()
        {
            ArrayList data = new ArrayList();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ConnectionString.ToString()))

                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        try
                        {
                            #region
                            cmdSql.CommandType = CommandType.Text;
                            cmdSql.CommandText =  " SELECT * FROM aNotification " +
                                                  " WHERE Receiver = '" + Session["ID"] + "'" +
                                                  " ORDER BY CreateDate DESC";
                           using (SqlDataReader sdr = cmdSql.ExecuteReader())
                            {
                                while (sdr.Read())
                                {
                                    data.Add(new {
                                        Header = sdr["Header"].ToString(),
                                        Details = sdr["Details"].ToString(),
                                        Receiver = sdr["Receiver"].ToString(),
                                        CreateDate = Convert.ToDateTime(sdr["CreateDate"]).ToString("MM/dd/yyyy HH:mm:ss"),
                                    });
                                }

                            }
                            #endregion
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
                string errmsg;
                if (err.InnerException != null)
                    errmsg = "An error occured: " + err.InnerException.ToString();
                else
                    errmsg = "An error occured: " + err.Message.ToString();

                return Json(new { success = false, msg = errmsg }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data,success = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SessionLanguage(string Languages)
        {
            try
            {
                Session["SystemLanguage"] = Languages;
            }
            catch
            {
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }

    public class Select2Data
    {
        public string ID { get; set; }
        public string Text { get; set; }
    }

}
