using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace v.Portal
{
    public partial class Portal : System.Web.UI.Page
    {
        protected string menus = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            var userinfo = Formula.FormulaHelper.GetUserInfo();
            if (userinfo.UserSystemName == "admin")
            {
                menus = @"[
	        { id: '1', text: '业务单元管理', iconCls: 'icon-edit', url: '../Manage/BusinessUnitList.aspx' }
        ]";
            }
            else
            {
                menus = @"[
	        { id: '1', text: '业务单元管理', iconCls: 'icon-edit', url: '../Manage/BusinessUnitList.aspx' }
        ]";
            }
        }
    }
}
