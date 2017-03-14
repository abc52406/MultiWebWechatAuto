using Common.Config;
using Common.Logic.Domain;
using Formula;
using Formula.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Formula.Helper;

namespace Web.Port
{
    /// <summary>
    /// wechat 的摘要说明
    /// </summary>
    public class wechat : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string method = context.Request["Method"];
            switch (method)
            {
                case "WebWeChatLogin":
                    string openid = context.Request["openid"];
                    WebWeChatLogin(context, openid);
                    break;
                default:
                    break;
            }
        }

        private void WebWeChatLogin(HttpContext context, string openid)
        {
            #region 查询数据
            var entities = FormulaHelper.GetEntities<CommonEntities>();
            try
            {
                if (!entities.Set<FansLogin>().Any(c => c.OpenID == openid && c.IsFinish != true))
                {
                    var ety = new FansLogin();
                    ety.ID = FormulaHelper.CreateGuid();
                    ety.OpenID = openid;
                    ety.ApplyDate = DateTime.Now;
                    ety.IsReply = false;
                    ety.IsFinish = false;
                    entities.Set<FansLogin>().Add(ety);
                    entities.SaveChanges();
                }
                context.Response.Write("");
            }
            catch (Exception ex)
            {
                LogWriter.Error(ex);
                context.Response.Write("");
            }
            finally
            {
                context.Response.End();
            }
            #endregion
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}