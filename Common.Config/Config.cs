using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    public enum ConnEnum
    {
        Common
    }

    public static class SysConfig
    {
        /// <summary>
        /// 站点域名
        /// </summary>
        public static string LocalDomain
        {
            get
            {
                return ConfigurationManager.AppSettings["LocalDomain"];
            }
        }

        /// <summary>
        /// 附件虚拟路径
        /// </summary>
        public static string FileVisualPath = ConfigurationManager.AppSettings["FileVisualPath"];

        /// <summary>
        /// 缩略图宽度
        /// </summary>
        public static int ThumbWidth
        {
            get
            {
                int w = 100;
                if (int.TryParse(ConfigurationManager.AppSettings["ThumbWidth"], out w))
                    return w;
                else return 100;
            }
        }

        /// <summary>
        /// 缩略图高度
        /// </summary>
        public static int ThumbHeight
        {
            get
            {
                int w = 70;
                if (int.TryParse(ConfigurationManager.AppSettings["ThumbHeight"], out w))
                    return w;
                else return 70;
            }
        }

        public static UserInfo GetUserInfoBySysName(string sysname)
        {
            UserInfo ui = new UserInfo();
            SQLHelper sh = SQLHelper.CreateSqlHelper(ConnEnum.Common.ToString());
            var dt = sh.ExecuteDataTable(string.Format("select * from UserInfo where SystemName='{0}'", sysname));
            if (dt.Rows.Count > 0)
            {
                ui.UserID = dt.Rows[0]["ID"].ToString();
                ui.UserName = dt.Rows[0]["UserName"].ToString();
                ui.UserSystemName = dt.Rows[0]["SystemName"].ToString();
            }
            return ui;
        }

        public static UserInfo GetUserInfoByID(string userid)
        {
            UserInfo ui = new UserInfo();
            SQLHelper sh = SQLHelper.CreateSqlHelper(ConnEnum.Common.ToString());
            var dt = sh.ExecuteDataTable(string.Format("select * from UserInfo where ID='{0}'", userid));
            if (dt.Rows.Count > 0)
            {
                ui.UserID = dt.Rows[0]["ID"].ToString();
                ui.UserName = dt.Rows[0]["UserName"].ToString();
                ui.UserSystemName = dt.Rows[0]["SystemName"].ToString();
            }
            return ui;
        }
    }
}
