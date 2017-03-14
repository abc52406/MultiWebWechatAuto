using Common.Config;
using Formula;
using Formula.Helper;
using Formula.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Manage
{
    public partial class BusinessUnitList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string method = Request["Method"];

            switch (method)
            {
                case "GetData":
                    string key = Request.Form["key"];
                    string pageIndex = Request.Form["pageIndex"];
                    string pageSize = Request.Form["pageSize"];
                    string sortField = Request.Form["sortField"];
                    string sortOrder = Request.Form["sortOrder"];
                    GetInfo(key, pageIndex, pageSize, sortField, sortOrder);
                    break;
                default:
                    break;
            }
        }

        private void GetInfo(string key, string pageIndex, string pageSize, string sortField, string sortOrder)
        {
            #region 初始化分页排序信息
            SearchCondition cnd = new SearchCondition();
            cnd.AllowPaging = true;
            int index = 1, size = 10;
            int.TryParse(pageIndex, out index);
            int.TryParse(pageSize, out size);
            cnd.CurrentPage = index;
            cnd.PageSize = size;
            if (!string.IsNullOrEmpty(sortField))
            {
                cnd.AddSortSql(string.Format("{0} {1}", sortField, sortOrder));
            }
            else
            {
                cnd.AddSort("ID");
            }
            #endregion

            #region 初始化查询条件
            if (!string.IsNullOrEmpty(key))
            {
                cnd.AddSearchSql(string.Format("and (Title like '%{0}%' or Description like '%{0}%')", key.Replace("'", "''")));
            }
            #endregion

            #region 查询数据

            SQLHelper sh = SQLHelper.CreateSqlHelper(ConnEnum.Common.ToString());
            string sql = string.Format("select * from BusinessUnit where IsDelete=0");
            var dt = sh.ExecuteDataTable(SearchCondition.CreatePagerSql(sql, cnd));
            var count = sh.ExecuteScalar(SearchCondition.CreateCountSql(sql, cnd));
            ArrayList data = CommonTool.DataTable2ArrayList(dt);
            Hashtable result = new Hashtable();
            result["data"] = data;
            result["total"] = Convert.ToInt32(count);
            #endregion

            Response.ContentType = "text/plain";
            Response.Write(PluSoft.Utils.JSON.Encode(result));
            Response.End();
        }

        public void ExportShakeData(string key)
        {
            SQLHelper sh = SQLHelper.CreateSqlHelper(ConnEnum.Common.ToString());
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select {0} from QRUser where 1=1", string.Join(",", columnNames.Keys));
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendFormat("and (OpenID like '%{0}%' or NickName like '%{0}%' or HeadPicUrl like '%{0}%' or QRIndex like '%{0}%')", key.Replace("'", "''"));
            }
            DataTable dt = sh.ExecuteDataTable(sb.ToString());
            foreach (var columnkey in columnNames.Keys)
            {
                dt.Columns[columnkey].Caption = columnNames[columnkey];
            }
            new ExcelHelper().ExportToExcel(dt);
        }

        Dictionary<string, string> columnNames = new Dictionary<string, string>()
        {
            {"OpenID","openid"},
            {"NickName","昵称"},
            {"HeadPicUrl","头像地址"},
            {"CreateTime","扫码时间"},
            {"QRIndex","二维码编号"},
        };
    }
}