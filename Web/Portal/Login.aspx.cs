using Common.Logic.Domain;
using Formula;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Shake.Portal
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string method = Request.Params["Method"];
            if (method == "Login")
            {

                string LoginName = Request.Params["SysLoginName"];
                string Password = Request.Params["SysPassword"];
                string password = string.Empty;
                //如果不是空密码，则进行密码加密
                if (!String.IsNullOrEmpty(Password))
                    password = FormsAuthentication.HashPasswordForStoringInConfigFile(Password.Trim(), "SHA1");
                var key = new object();
                var entities = FormulaHelper.GetDbContext<CommonEntities>(key);
                try
                {
                    var userinfo = entities.UserInfo.Where(c => (c.IsDelete == null || c.IsDelete != true) && c.SystemName == LoginName).FirstOrDefault();
                    if (userinfo == null)
                    {
                        Response.Write("账号或密码错误");
                    }
                    else
                    {
                        var maxerrorcount = ConfigurationManager.AppSettings["MaxPasswordError"];
                        int maxcount = -1;
                        int.TryParse(maxerrorcount, out maxcount);
                        if (maxcount != -1 && (userinfo.ErrorCount ?? 0) >= maxcount)
                        {
                            Response.Write("账号连续多次输入了错误密码，请联系管理员");
                        }
                        else if (userinfo.Password != password)
                        {
                            userinfo.ErrorCount = (userinfo.ErrorCount ?? 0 + 1);
                            entities.SaveChanges();
                            Response.Write("账号或密码错误");
                        }
                        else
                        {
                            userinfo.ErrorCount = 0;
                            userinfo.LastLoginTime = DateTime.Now;
                            entities.SaveChanges();
                            FormsAuthentication.SetAuthCookie(LoginName, true);
                            Response.Write("Success");
                        }
                    }
                }
                catch (Exception ex)
                {
                    FormulaHelper.RemoveDbContext<CommonEntities>(key);
                    Response.Write(ex.Message);
                }
                finally
                {
                    Response.End();
                }
            }
        }
    }
}