using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;
using System.Drawing.Imaging;

namespace CarService.Models
{
    public class DataHelper
    {
        public static string GetData(string field, string table, string condition, string asVar = "",string query ="")
        {
            string retVal = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        try
                        {
                            cmdSql.CommandTimeout = 0;
                            cmdSql.CommandType = CommandType.Text;
                            if (query == "")
                                cmdSql.CommandText = "SELECT " + field + " AS " + (asVar == "" ? "text" : asVar) + " FROM " + table + " WHERE " + condition;
                            else
                                cmdSql.CommandText = query;
                            using (SqlDataReader sdr = cmdSql.ExecuteReader())
                            {
                                if (sdr.Read())
                                {
                                    retVal = sdr["text"].ToString();
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
                    errmsg = "An error occured: " + err.ToString();
                throw new InvalidOperationException(errmsg);
            }
            return retVal;
        }
        public static object GetDataObject(string field, string table, string condition, string asVar = "", string query = "")
        {
            string retVal = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        try
                        {
                            cmdSql.CommandTimeout = 0;
                            cmdSql.CommandType = CommandType.Text;
                            if (query == "")
                                cmdSql.CommandText = "SELECT " + field + " AS " + (asVar == "" ? "text" : asVar) + " FROM " + table + " WHERE " + condition;
                            else
                                cmdSql.CommandText = query;
                            using (SqlDataReader sdr = cmdSql.ExecuteReader())
                            {
                                if (sdr.Read())
                                {
                                    return sdr["text"];
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
                    errmsg = "An error occured: " + err.ToString();
                throw new InvalidOperationException(errmsg);
            }
            return retVal;
        }
        public List<Dictionary<string, object>> DataTableToRow(DataTable dt)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return rows;
        }
        public static string ByteArrayToImage(byte[] bytesArr,string filetype)
        {
            using (MemoryStream memstr = new MemoryStream(bytesArr))
            {
                ImageFormat format;
                Image img = Image.FromStream(memstr);
                switch (filetype)
                {
                    case "png":
                        format = ImageFormat.Png;
                        break;
                    case "gif":
                        format = ImageFormat.Gif;
                        break;
                    default:
                        format = ImageFormat.Jpeg;
                        break;
                }
                string FileName = DateTime.Now.ToString("yyyyMMddHHss") + "." + filetype;
                img.Save(HttpContext.Current.Server.MapPath("~/Areas/PersonalInfo/Files/Img/" + FileName), format);
                return FileName;
            }
        }
        public object GetPropertyValue(object obj, string name)
        {
            return obj ?? obj.GetType().GetProperty(name).GetValue(obj, null);
        }
    }

}
