using System;
using System.Collections.Generic;
//using CarService.Areas.PersonalInfo.Models;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace CarService.Models
{

    public class ByteFiles
    {
        //byte ByteFile = ;
        public string FileName = "";
        public byte[] ByteFile = { };
        public string PDFPath;
    }
    public class Common
    {
        /// <summary>
        /// Convert null to empty string
        /// </summary>
        /// <param name="objString"></param>
        /// <param name="TrimOption"></param>
        /// <returns></returns>
        public string FgNullToString(object objString, int TrimOption = 0)
        {
            try
            {
                //Check object
                if (objString == DBNull.Value)
                {
                    //If NULL, returns an empty string
                    return string.Empty;
                }
                else if (objString == null)
                {
                    //Nothing returns an empty string
                    return string.Empty;
                }
                else
                {
                    //Type-convert to String and return
                    switch (TrimOption)
                    {
                        case 0: //Trim
                            return Convert.ToString(objString).Trim();
                        case 1: //TrimEnd
                            return Convert.ToString(objString).TrimEnd();
                        case 2: //Without Trim
                            return Convert.ToString(objString);
                        default:
                            return Convert.ToString(objString).Trim();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Convert null to empty double
        /// </summary>
        /// <param name="objString"></param>
        /// <param name="TrimOption"></param>
        /// <returns></returns>
        public double FgNullToDouble(object objString)
        {
            try
            {
                //Check object
                if (objString == DBNull.Value)
                {
                    //If NULL, returns an empty string
                    return 0.00;
                }
                else if (objString == null)
                {
                    //If NULL, returns an empty string
                    return 0.00;
                }
                else if (String.IsNullOrEmpty(objString.ToString()))
                {
                    //Nothing returns an empty string
                    return 0.00;
                }
                else
                {
                    return Convert.ToDouble(objString);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Convert null to empty string
        /// </summary>
        /// <param name="objString"></param>
        /// <param name="TrimOption"></param>
        /// <returns></returns>
        public int FgNullToInt(object objString)
        {
            try
            {
                //Check object
                if (objString == DBNull.Value)
                {
                    //If NULL, returns an empty string
                    return 0;
                }
                else if (objString == null)
                {
                    //If NULL, returns an empty string
                    return 0;
                }
                else if (String.IsNullOrEmpty(objString.ToString()))
                {
                    //Nothing returns an empty string
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(objString);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Convert null to empty string
        /// </summary>
        /// <param name="objString"></param>
        /// <param name="TrimOption"></param>
        /// <returns></returns>
        public int? FgInt(object objString)
        {
            try
            {
                //Check object
                if (objString == DBNull.Value)
                {
                    //If NULL, returns an empty string
                    return null;
                }
                else if (objString == null)
                {
                    //If NULL, returns an empty string
                    return null;
                }
                else if (String.IsNullOrEmpty(objString.ToString()))
                {
                    //Nothing returns an empty string
                    return null;
                }
                else
                {
                    return Convert.ToInt32(objString);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Convert object to zero
        /// </summary>
        /// <param name="objNumber"></param>
        /// <returns></returns>
        public object FgNullToZero(object objNumber)
        {
            try
            {
                //Check object
                if (objNumber == DBNull.Value)
                {
                    //If NULL, return 0
                    return 0;
                }
                else if (objNumber == null)
                {
                    //If Nothing, return 0
                    return 0;
                }
                else if (String.IsNullOrEmpty(objNumber.ToString()))
                {
                    //If Nothing, return 0
                    return 0;
                }
                else
                {
                    //Type converted to Decimal type and returned
                    return Convert.ToDecimal(objNumber);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Convert to DBNUll value
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        public object FgToDBNull(object objData)
        {
            try
            {
                //Check object
                if (objData == DBNull.Value)
                {
                    //If NULL, return DBNull.Value
                    return DBNull.Value;
                }
                else if (objData == null)
                {
                    //If Nothing, return DBNull.Value
                    return DBNull.Value;
                }
                else
                {
                    return objData;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Convert to null value
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        public object FgToNull(object objData)
        {
            try
            {
                //Check object
                if (objData == DBNull.Value)
                {
                    //If NULL, return null
                    return null;
                }
                else if (objData == null)
                {
                    //If Nothing, return null
                    return null;
                }
                else
                {
                    return objData;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Check if object is null
        /// </summary>
        /// <param name="objData"></param>
        /// <param name="bNumberFlg"></param>
        /// <returns></returns>
        public bool FgNullCheck(object objData)
        {
            bool retValue = false;

            try
            {
                //DBNull Case
                if (objData == DBNull.Value)
                {
                    retValue = true;
                }
                //Nothing Case
                if (objData == null)
                {
                    retValue = true;
                }
                return retValue;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Check if object is on [checkbox]
        /// </summary>
        /// <param name="objData"></param>
        /// <param name="bNumberFlg"></param>
        /// <returns></returns>
        public int FgStringCheckboxToInt(object objData)
        {
            try
            {
                //DBNull Case
                if (objData == DBNull.Value)
                {
                    return 0;
                }
                //Nothing Case
                if (objData == null)
                {
                    return 0;
                }
                //On Case
                if (objData.ToString() == "on")
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Convert String to Integer
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        public int FgStringToInt(object objData)
        {
            try
            {
                //DBNull Case
                if (objData == DBNull.Value)
                {
                    return 0;
                }
                //Nothing Case
                if (objData == null)
                {
                    return 0;
                }
                //Convert to Integer
                return Convert.ToInt32(objData);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Check if passed value is numeric
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        public bool FgCheckIfNumeric(object objData)
        {
            try
            {
                //DBNull Case
                if (objData == DBNull.Value)
                {
                    return false;
                }
                //Nothing Case
                if (objData == null)
                {
                    return false;
                }

                int iValue = Convert.ToInt32(objData);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Check if object is null
        /// </summary>
        /// <param name="objData"></param>
        /// <param name="bNumberFlg"></param>
        /// <returns></returns>
        public string FgNullToDateTime(object objDate, int x = 1)
        {
            string dString;
            try
            {
                //Check object
                if(objDate == DBNull.Value)
                {
                    //If NULL, return 0
                    return "";
                }
                else if (objDate == null)
                {
                    //If Nothing, return 0
                    return "";
                }
                else if (objDate.ToString() == "")
                {
                    //If Nothing, return 0
                    return "";
                }
                else if (Convert.ToDateTime(objDate).ToString("MM/dd/yyyy") == "01/01/1900")
                {
                    //If Nothing, return 0
                    return "";
                }
                else if(objDate.GetType() == typeof(System.DateTime) && x == 1)
                {
                    return Convert.ToDateTime(objDate).ToString("MM/dd/yyyy");
                }
                else
                {
                    //Type converted to Decimal type and returned
                    dString = Convert.ToDateTime(objDate).ToString("MM/dd/yyyy");
                }
            }
            catch
            {
                try
                {
                    if (!DateTime.TryParseExact(objDate.ToString(), "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime bdayDate))
                        return "";
                    dString = Convert.ToDateTime(bdayDate).ToString("MM/dd/yyyy");
                }
                catch
                {
                    return "";
                }
            }
            return dString != "" && x != 1 ? dString.Substring(0, 2) + "/" + dString.Substring(6, 4) : dString;
        }
        public DateTime FgDateTime(object objDate)
        {
            try
            {
                //Check object
                if (objDate == DBNull.Value)
                {
                    //If NULL, return 0
                    return Convert.ToDateTime("01/01/1900");
                }
                else if (objDate == null)
                {
                    //If Nothing, return 0
                    return Convert.ToDateTime("01/01/1900");
                }
                else if (objDate.ToString() == "")
                {
                    //If Nothing, return 0
                    return Convert.ToDateTime("01/01/1900");
                }
                else if (Convert.ToDateTime(objDate).ToString("MM/dd/yyyy") == "01/01/1900")
                {
                    //If Nothing, return 0
                    return Convert.ToDateTime("01/01/1900");
                }
                else if (objDate.GetType() == typeof(System.DateTime))
                {
                    return Convert.ToDateTime(objDate);
                }
                else
                {
                    try
                    {
                        if (!DateTime.TryParseExact(objDate.ToString(), "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime rDate))
                            return Convert.ToDateTime("01/01/1900");
                        return Convert.ToDateTime(rDate);
                    }
                    catch
                    {
                        return Convert.ToDateTime("01/01/1900");
                    }
                }
            }
            catch
            {
                return Convert.ToDateTime("01/01/1900");
            }
        }

        /// <summary>
        /// Check if object is null
        /// </summary>
        /// <param name="objData"></param>
        /// <param name="bNumberFlg"></param>
        /// <returns></returns>
        public object FgDBDateTime(object objDate)
        {
            try
            {
                if (objDate == DBNull.Value)
                {
                    //If NULL, return 0
                    return DBNull.Value;
                }
                else if (objDate == null)
                {
                    return DBNull.Value;
                }
                else if (objDate.ToString() == "")
                {
                    return DBNull.Value;
                }
                else if (Convert.ToDateTime(objDate).ToString("MM/dd/yyyy") == "01/01/1900" || Convert.ToDateTime(objDate).ToString("MM/dd/yyyy") == "01/01/0001")
                {
                    return DBNull.Value;
                }
                else if(objDate.GetType() == typeof(System.DateTime))
                {
                    return Convert.ToDateTime(objDate);
                }
                else
                {
                    if(objDate.GetType().ToString() == "System.DateTime")
                        return objDate;
                    else
                    {
                        if (DateTime.TryParseExact(objDate.ToString(), "MM/dd/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime cDateTime))
                            return cDateTime;
                        else if (DateTime.TryParseExact(objDate.ToString(), "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime cDate))
                            return cDate;
                    }

                }
            }
            catch
            {
                try
                {
                    if (DateTime.TryParseExact(objDate.ToString(), "MM/dd/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime cDateTime))
                        return cDateTime;
                    else if (DateTime.TryParseExact(objDate.ToString(), "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime cDate))
                        return cDate;
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            return DBNull.Value;
        }
        public object FgDBDouble(object objDate)
        {
            try
            {
                //Check object
                if (objDate == DBNull.Value)
                {
                    //If NULL, return 0
                    return DBNull.Value;
                }
                else if (objDate == null)
                {
                    return DBNull.Value;
                }
                else if (objDate.ToString() == "")
                {
                    return DBNull.Value;
                }
                else
                {
                    return Convert.ToDecimal(objDate.ToString().Replace(",", ""));
                }
            }
            catch
            {
                try
                {
                    if (DateTime.TryParseExact(objDate.ToString(), "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime cDateTime))
                        return cDateTime;
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            return DBNull.Value;
        }
        public object FgDBString(object objString)
        {
            try
            {
                //Check object
                if (objString == DBNull.Value)
                {
                    //If NULL, returns an empty string
                    return "";
                }
                else if (objString == null)
                {
                    //Nothing returns an empty string
                    return "";
                }
                else
                {
                    return Convert.ToString(objString);
                }
            }
            catch
            {
                return "";
            }
        }
        public object FgDBInt(object objInt)
        {
            try
            {
                //Check object
                if (objInt == DBNull.Value)
                {
                    //If NULL, returns an empty Int
                    return DBNull.Value;
                }
                else if (objInt == null)
                {
                    //Nothing returns an empty Int
                    return DBNull.Value;
                }
                else
                {
                    return Convert.ToInt32(objInt.ToString().Replace(",", ""));
                }
            }
            catch
            {
                return DBNull.Value;
            }
        }
        public string FgDateYYMM(object objString)
        {
            try
            {
                //Check object
                if (objString == DBNull.Value)
                {
                    //If NULL, returns an empty string
                    return "";
                }
                else if (objString == null)
                {
                    //If NULL, returns an empty string
                    return "";
                }
                else if (String.IsNullOrEmpty(objString.ToString()))
                {
                    //Nothing returns an empty string
                    return "";
                }
                else
                {
                    return objString.ToString().Length == 10 ? objString.ToString() : objString.ToString().Substring(0, 2) + "/01/" + objString.ToString().Substring(3, 4);
                }
            }
            catch
            {
                return "";
            }
        }
        public int FgNullToNegativeOne(object objString)
        {
            try
            {
                //Check object
                if (objString == DBNull.Value)
                {
                    //If NULL, returns an empty string
                    return -1;
                }
                else if (objString == null)
                {
                    //If NULL, returns an empty string
                    return -1;
                }
                else if (String.IsNullOrEmpty(objString.ToString()))
                {
                    //Nothing returns an empty string
                    return -1;
                }
                else
                {
                    return Convert.ToInt32(objString);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string FgIntToYesNo(object objString)
        {
            try
            {
                //Check object
                if (objString == DBNull.Value)
                {
                    //If NULL, returns an empty string
                    return "No";
                }
                else if (objString == null)
                {
                    //If NULL, returns an empty string
                    return "No";
                }
                else if (String.IsNullOrEmpty(objString.ToString()))
                {
                    //Nothing returns an empty string
                    return "No";
                }
                else
                {
                    if (this.FgStringToInt(objString) == 0)
                    {
                        return "No";
                    }
                    else
                    {
                        return "Yes";
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int FgEmptyStringToNegativeOne(object objString)
        {
            try
            {
                //Check object
                if (objString == DBNull.Value)
                {
                    //If NULL, returns an empty string
                    return -1;
                }
                else if (objString == null)
                {
                    //If NULL, returns an empty string
                    return -1;
                }
                else if (String.IsNullOrEmpty(objString.ToString()))
                {
                    //Nothing returns an empty string
                    return -1;
                }
                else
                {
                    return Convert.ToInt32(objString);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Return error msg
        /// </summary>
        /// <param name="Err"></param>
        /// <returns></returns>
        public string MyErrorMsg(Exception Err)
        {
            if (Err.InnerException != null)
                return "An error occured: " + Err.InnerException.ToString();
            else
                return "An error occured: " + Err.Message.ToString();

        }
        public int ComputeBirthday(string bday)
        {
            if (bday == "")
                return 0;
            else
            {
                DateTime today = DateTime.Today;
                if (!DateTime.TryParseExact(bday, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime bdayDate))
                {
                    return 0;
                }
                return (Convert.ToInt32(today.ToString("MMdd")) < Convert.ToInt32(bdayDate.ToString("MMdd")) ? (today.Year - bdayDate.Year) - 1 : (today.Year - bdayDate.Year));
            }

        }
        public string FindWord(List<MWordTransalate> wt, string DefaultValue, string CallCode)
        {
            return FgNullToString(wt.Where(x => x.CallCode == CallCode && x.DefaultValue == DefaultValue).Select(x => x.Messege).FirstOrDefault());
        }
        public List<MWordTransalate> GetWordTransalate()
        {
            List<MWordTransalate> data = new List<MWordTransalate>();
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
                            cmdSql.CommandText = " SELECT * FROM cWordTransalate";
                            using (SqlDataReader sdr = cmdSql.ExecuteReader())
                            {
                                while (sdr.Read())
                                {
                                    data.Add(new MWordTransalate
                                    {
                                        DefaultValue = sdr["DefaultValue"].ToString(),
                                        CallCode = sdr["CallCode"].ToString(),
                                        Messege = sdr["Messege"].ToString(),
                                        PageLabel = sdr["PageLabel"].ToString(),
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
            catch
            {
            }
            return data;
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
        public int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return (lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year);
        }
        public string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }
            return columnName;
        }
    }
}