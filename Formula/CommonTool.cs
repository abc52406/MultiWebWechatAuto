using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formula
{
    public static class CommonTool
    {
        public static bool ToBool(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Trim() == "")
                throw new Exception(string.Format("字符串{0}不能转换为布尔值", value));
            if (value.ToLower().StartsWith("t"))
                return true;
            return false;
        }

        public static string ToString(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return string.Empty;
            return obj.ToString();
        }

        public static int ToInt(object obj, int defaultvalue = 0)
        {
            if (obj == null || obj == DBNull.Value)
                return defaultvalue;
            int.TryParse(obj.ToString(), out defaultvalue);
            return defaultvalue;
        }

        public static decimal ToDecimal(object obj, decimal defaultvalue = 0)
        {
            if (obj == null || obj == DBNull.Value)
                return defaultvalue;
            decimal.TryParse(obj.ToString(), out defaultvalue);
            return defaultvalue;
        }

        /// <summary>
        ///DataTable2ArrayList
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ArrayList DataTable2ArrayList(DataTable data)
        {
            ArrayList array = new ArrayList();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow row = data.Rows[i];

                Hashtable record = new Hashtable();
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    object cellValue = row[j];
                    if (cellValue.GetType() == typeof(DBNull))
                    {
                        cellValue = null;
                    }
                    record[data.Columns[j].ColumnName] = cellValue;
                }
                array.Add(record);
            }
            return array;
        }
    }
}
