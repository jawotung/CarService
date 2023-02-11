using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.IO;
using CarService.Controllers;
using CarService.Models;

namespace CarService.App_Start
{
    public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpContext context = HttpContext.Current;
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                return;
            if (context.Session["ID"] == null)
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "SessionError" }, { "controller", "Login" }, { "area", "" } });
                else
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Index" }, { "controller", "Login" }, { "area", "" } });
            else
            {
                ArrayList userMenuList = new ArrayList();
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarService"].ConnectionString.ToString()))
                    {
                        conn.Open();
                        using (SqlCommand cmdSql = conn.CreateCommand())
                        {
                            cmdSql.CommandType = CommandType.StoredProcedure;
                            cmdSql.CommandText = "cUserPageAccess_Validate";
                            cmdSql.ExecuteNonQuery();
                            using (SqlDataReader sdr = cmdSql.ExecuteReader())
                            {
                                while (sdr.Read())
                                {
                                    userMenuList.Add(new
                                    {
                                        ID = Convert.ToInt32(sdr["ID"]),
                                        GroupLabel = sdr["GroupLabel"].ToString(),
                                        PageName = sdr["PageName"].ToString(),
                                        PageLabel = sdr["PageLabel"].ToString(),
                                        URL = sdr["URL"].ToString(),
                                        HasSub = Convert.ToInt32(sdr["HasSub"]),
                                        ParentMenu = sdr["ParentMenu"].ToString(),
                                        ParentOrder = Convert.ToInt32(sdr["ParentOrder"]),
                                        Order = Convert.ToInt32(sdr["Order"]),
                                        Icon = sdr["Icon"].ToString(),
                                        ReadAndWrite = sdr["ReadAndWrite"].ToString(),
                                        DeleteEnabled = sdr["DeleteEnabled"].ToString(),
                                    });
                                }
                            }
                            var jsonSerialiser = new JavaScriptSerializer();
                            var json = jsonSerialiser.Serialize(userMenuList);
                            context.Session["Menu"] = json;
                        }
                        conn.Close();
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}