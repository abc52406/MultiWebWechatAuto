using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Formula.Helper
{
    public class ExcelHelper
    {
        /// <summary>
        /// ExportToExcel
        /// </summary>
        /// <param name="dt"></param>
        public void ExportToExcel(DataTable dt)
        {
            var Response = HttpContext.Current.Response;
            Response.ContentType = "text/csv;Charset=UTF-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.CacheControl = "public";
            string realFileName = string.Format("{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + HttpUtility.UrlEncode(realFileName, System.Text.Encoding.GetEncoding("UTF-8")) + "\"");

            Response.Clear();


            StringBuilder sb = new StringBuilder();
            sb.Append("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\"><tr style=\"font-weight: bold; white-space: nowrap;\">");

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sb.AppendFormat("<td>{0}</td>", dt.Columns[i].Caption);
            }
            sb.Append("</tr>");

            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr>");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string value = row[i].ToString();
                    if (dt.Columns[i].DataType == typeof(DateTime) && value != "")
                        value = DateTime.Parse(value).ToString("yyyy-MM-dd HH:mm:ss");

                    if (dt.Columns[i].DataType == typeof(int) || dt.Columns[i].DataType == typeof(decimal) || dt.Columns[i].DataType == typeof(float) || dt.Columns[i].DataType == typeof(double))
                        sb.AppendFormat("<td>{0}</td>", value);
                    else
                        sb.AppendFormat("<td style=\"vnd.ms-excel.numberformat:@\">{0}</td>", value);

                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");

            Response.Write(sb.ToString());

            Response.End();

            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}
