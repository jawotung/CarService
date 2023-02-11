using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using CarService.Areas.MasterMaintenance.Models;
using CarService.Models;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;

namespace CarService.Areas.MasterMaintenance.Controllers
{
    public class GeneralMasterController : Controller
    {
        DataHelper dataHelper = new DataHelper();
        List<string> modelErrors = new List<string>();
        bool error = false;
        string errmsg = "";
        public ActionResult Index()
        {
            return View("GeneralMaster");
        }
        public ActionResult GetGeneralList()
        {
            List<MGeneral> data = new List<MGeneral>();
            DataTableHelper TypeHelper = new DataTableHelper();

            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][data]"];
            string sortDirection = Request["order[0][dir]"];
            int TypeID = Request["TypeID"] == "" ? 0 : Convert.ToInt32(Request["TypeID"]);

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        try
                        {
                            cmdSql.CommandType = CommandType.Text;
                            if (TypeID > 0)
                                cmdSql.CommandText = "SELECT g.ID, g.TypeID, t.[Type], g.Value,g.UpdateDate FROM MTypes AS t JOIN MGeneral as g ON t.ID = g.TypeID WHERE g.IsDeleted = 0 AND t.IsDeleted = 0 AND TypeID LIKE '" + TypeID + "'";
                            else
                                cmdSql.CommandText = "SELECT g.ID, g.TypeID, t.[Type], g.Value,g.UpdateDate FROM MTypes AS t JOIN MGeneral as g ON t.ID = g.TypeID WHERE g.IsDeleted = 0 AND t.IsDeleted = 0; ";
                            using (SqlDataReader sdr = cmdSql.ExecuteReader())
                            {
                                while (sdr.Read())
                                {
                                    data.Add(new MGeneral
                                    {
                                        ID = Convert.ToInt32(sdr["ID"]),
                                        TypeID = Convert.ToInt32(sdr["TypeID"]),
                                        TypeDesc = sdr["Type"].ToString(),
                                        Value = sdr["Value"].ToString(),
                                        UpdateDate = sdr["UpdateDate"].ToString()
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
                    errmsg = "An error occured: " + err.ToString();

                return Json(new { success = false, msg = errmsg }, JsonRequestBehavior.AllowGet);
            }
            int totalrows = data.Count;
            if (!string.IsNullOrEmpty(searchValue))//filter
                data = data.Where(x =>
                    x.TypeID.ToString().ToLower().Contains(searchValue.ToLower()) ||
                    x.Value.ToString().ToLower().Contains(searchValue.ToLower())
                ).ToList<MGeneral>();

            int totalrowsafterfiltering = data.Count;
            if (sortDirection == "asc")
                data = data.OrderBy(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();

            if (sortDirection == "desc")
                data = data.OrderByDescending(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();

            data = data.Skip(start).Take(length).ToList<MGeneral>();


            return Json(new { data = data, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ValidateValue(string TypeID, string Value)
        {
            bool isValid = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        try
                        {
                            cmdSql.CommandType = CommandType.Text;
                            cmdSql.CommandText = "SELECT Value FROM MGeneral WHERE IsDeleted=0 AND TypeID='" + TypeID + "' AND Value='" + Value + "'";
                            using (SqlDataReader sdr = cmdSql.ExecuteReader())
                            {
                                if (sdr.HasRows)
                                    isValid = false;
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
                if (err.InnerException != null)
                    errmsg = "An error occured: " + err.InnerException.ToString();
                else
                    errmsg = "An error occured: " + err.Message.ToString(); ;
                error = true;
            }
            if (error)
                return Json(new { success = false, errors = errmsg }, JsonRequestBehavior.AllowGet);
            else
            {
                return Json(new { success = true, data = new { isValid = isValid } });
            }
        }
        public ActionResult SaveGeneral(MGeneral General)
        {
            string endMsg = "";
            ModelState.Remove("ID");
            if (ModelState.IsValid)
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
                                cmdSql.CommandText = "mGeneral_InsertUpdate";
                                cmdSql.Parameters.Clear();
                                cmdSql.Parameters.AddWithValue("@ID", General.ID);
                                cmdSql.Parameters.AddWithValue("@TypeID", General.TypeID);
                                cmdSql.Parameters.AddWithValue("@Value", General.Value);
                                cmdSql.Parameters.AddWithValue("@CreateID", Session["WorkerID"]);
                                SqlParameter EndMsg = cmdSql.Parameters.Add("@EndMsg", SqlDbType.VarChar, 1000);
                                SqlParameter ErrorMessage = cmdSql.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000);
                                SqlParameter Error = cmdSql.Parameters.Add("@Error", SqlDbType.Bit);

                                EndMsg.Direction = ParameterDirection.Output;
                                Error.Direction = ParameterDirection.Output;
                                ErrorMessage.Direction = ParameterDirection.Output;

                                cmdSql.ExecuteNonQuery();

                                error = Convert.ToBoolean(Error.Value);
                                if (error)
                                    modelErrors.Add(ErrorMessage.Value.ToString());
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
            }
            else
            {
                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var modelStateVal = ViewData.ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        var key = modelStateKey;
                        var errMessage = error.ErrorMessage;
                        var exception = error.Exception;
                        modelErrors.Add(errMessage);
                    }
                }
            }
            if (modelErrors.Count != 0 || error)
                return Json(new { success = false, errors = modelErrors });
            else
            {
                return Json(new { success = true, msg = "Data was successfully " + endMsg });
            }
        }
        public ActionResult GetGeneralDetails(int ID)
        {
            MGeneral Details = new MGeneral();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ConnectionString.ToString()))
                {
                    conn.Open();
                    string getGeneralSql = "SELECT g.ID, g.TypeID, t.[Type], g.Value,g.UpdateDate FROM MTypes AS t JOIN MGeneral as g ON t.ID = g.TypeID WHERE g.IsDeleted = 0 AND t.IsDeleted = 0 AND g.ID='" + ID + "'";
                    using (SqlCommand comm = new SqlCommand(getGeneralSql, conn))
                    {
                        SqlDataReader reader = comm.ExecuteReader();
                        try
                        {
                            if (!reader.Read())
                                throw new InvalidOperationException("No records found.");

                            Details.ID = Convert.ToInt32(reader["ID"]);
                            Details.TypeID = Convert.ToInt32(reader["TypeID"]);
                            Details.TypeDesc = reader["Type"].ToString();
                            Details.Value = reader["Value"].ToString();
                            Details.UpdateDate = reader["UpdateDate"].ToString();
                        }
                        catch (Exception err)
                        {
                            throw new InvalidOperationException(err.Message);
                        }
                        finally
                        {
                            reader.Close();
                            comm.Dispose();
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
            return Json(new { success = true, data = new { generalData = Details } }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteGeneral(int ID, DateTime UpdateDate)
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
                            cmdSql.CommandText = "mGeneral_Delete";
                            cmdSql.Parameters.Clear();
                            cmdSql.Parameters.AddWithValue("@ID", ID);
                            cmdSql.Parameters.AddWithValue("@UpdateID", Session["WorkerID"]);

                            SqlParameter Error = cmdSql.Parameters.Add("@Error", SqlDbType.Bit);
                            SqlParameter ErrorMessage = cmdSql.Parameters.Add("@ErrorMessage", SqlDbType.NVarChar, 1000);

                            Error.Direction = ParameterDirection.Output;
                            ErrorMessage.Direction = ParameterDirection.Output;

                            cmdSql.ExecuteNonQuery();

                            error = Convert.ToBoolean(Error.Value);
                            if (error)
                            {
                                cmdSql.Dispose();
                                conn.Close();
                                return Json(new { success = false, errors = ErrorMessage.Value.ToString() }, JsonRequestBehavior.AllowGet);
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
                    errmsg = "Error: " + err.InnerException.ToString();
                else
                    errmsg = "Error: " + err.Message.ToString();

                return Json(new { success = false, errors = errmsg }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, msg = "Field was successfully deleted." });

        }

        //Type Data Functions Here
        public ActionResult GetTypeList()
        {
            List<MTypes> data = new List<MTypes>();
            DataTableHelper TypeHelper = new DataTableHelper();

            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][data]"];
            string sortDirection = Request["order[0][dir]"];

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        try
                        {
                            cmdSql.CommandType = CommandType.Text;
                            cmdSql.CommandText = "SELECT * FROM MTypes WHERE IsDeleted = 0 ";
                            using (SqlDataReader sdr = cmdSql.ExecuteReader())
                            {
                                while (sdr.Read())
                                {
                                    data.Add(new MTypes
                                    {
                                        ID = Convert.ToInt32(sdr["ID"]),
                                        Type = sdr["Type"].ToString(),
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
                    errmsg = "An error occured: " + err.ToString();

                return Json(new { success = false, msg = errmsg }, JsonRequestBehavior.AllowGet);
            }
            int totalrows = data.Count;
            if (!string.IsNullOrEmpty(searchValue))//filter
                data = data.Where(x =>
                    x.Type.ToLower().Contains(searchValue.ToLower())
                ).ToList<MTypes>();

            int totalrowsafterfiltering = data.Count;
            if (sortDirection == "asc")
                data = data.OrderBy(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();

            if (sortDirection == "desc")
                data = data.OrderByDescending(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();

            data = data.Skip(start).Take(length).ToList<MTypes>();


            return Json(new { data = data, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTypeDetails(int ID)
        {
            MTypes Details = new MTypes();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ConnectionString.ToString()))
                {
                    conn.Open();
                    string getTypeSql = "SELECT * FROM MTypes WHERE IsDeleted = '0' AND ID='" + ID + "'";
                    using (SqlCommand comm = new SqlCommand(getTypeSql, conn))
                    {
                        SqlDataReader reader = comm.ExecuteReader();
                        try
                        {
                            if (!reader.Read())
                                throw new InvalidOperationException("No records found.");

                            Details.ID = Convert.ToInt32(reader["ID"]);
                            Details.Type = reader["Type"].ToString();
                            Details.UpdateDate = reader["UpdateDate"].ToString();
                        }
                        catch (Exception err)
                        {
                            throw new InvalidOperationException(err.Message);
                        }
                        finally
                        {
                            reader.Close();
                            comm.Dispose();
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
            return Json(new { success = true, data = new { generalData = Details } }, JsonRequestBehavior.AllowGet);
        }
        public List<MGeneral> GetGeneral(string TypeID)
        {
            List<MGeneral> data = new List<MGeneral>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        try
                        {
                            cmdSql.CommandType = CommandType.Text;
                            if (TypeID != "")
                                cmdSql.CommandText = "SELECT g.ID, g.TypeID, t.[Type], g.Value,g.UpdateDate FROM MTypes AS t JOIN MGeneral as g ON t.ID = g.TypeID WHERE g.IsDeleted = 0 AND t.IsDeleted = 0 AND TypeID LIKE '" + TypeID + "'";
                            else
                                cmdSql.CommandText = "SELECT g.ID, g.TypeID, t.[Type], g.Value,g.UpdateDate FROM MTypes AS t JOIN MGeneral as g ON t.ID = g.TypeID WHERE g.IsDeleted = 0 AND t.IsDeleted = 0; ";
                            using (SqlDataReader sdr = cmdSql.ExecuteReader())
                            {
                                while (sdr.Read())
                                {
                                    data.Add(new MGeneral
                                    {
                                        ID = Convert.ToInt32(sdr["ID"]),
                                        TypeID = Convert.ToInt32(sdr["TypeID"]),
                                        TypeDesc = sdr["Type"].ToString(),
                                        Value = sdr["Value"].ToString(),
                                        UpdateDate = sdr["UpdateDate"].ToString()
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
                    errmsg = "An error occured: " + err.ToString();

                return null;
            }
            return data;
        }
    }
}
