using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formula.Helper
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 根据类型获取枚举
        /// </summary>
        /// <param name="emType"></param>
        /// <returns></returns>
        public static Dictionary<string,string> GetEnum(Type emType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in emType.GetFields())
            {
                if (item.FieldType.IsEnum)
                {
                    object[] arr = item.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    dic.Add(item.Name,arr.Length > 0 ? ((DescriptionAttribute)arr[0]).Description : item.Name);
                }
            }

            return dic;
        }

        /// <summary>
        /// 获取枚举的文本值
        /// </summary>
        /// <param name="dic">枚举</param>
        /// <param name="value">枚举键</param>
        /// <returns></returns>
        public static string GetEnumText(Dictionary<string,string> dic,string value)
        {
            if (dic.ContainsKey(value))
                return dic[value];
            else
                return null;
        }

        /// <summary>
        /// 获取枚举的文本值
        /// </summary>
        /// <param name="dic">枚举</param>
        /// <param name="value">枚举键</param>
        /// <returns></returns>
        public static string GetEnumText(Type emType, string value)
        {
            var dic = GetEnum(emType);
            if (dic.ContainsKey(value))
                return dic[value];
            else
                return null;
        }

        /// <summary>
        /// 获取枚举的键值
        /// </summary>
        /// <param name="dic">枚举</param>
        /// <param name="text">枚举文本</param>
        /// <returns></returns>
        public static string GetEnumValue(Type emType, string text)
        {
            var dic = GetEnum(emType);
            if (dic.ContainsValue(text))
                return dic.Where(c=>c.Value==text).First().Key;
            else
                return null;
        }

        //static SQLHelper adminHelper = SQLHelper.CreateSqlHelper(ConnEnum.Admin.ToString());
        //public static Dictionary<string, string> GetEnum(string enumKey)
        //{
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    var table = adminHelper.ExecuteDataTable(string.Format("select SysEnumID,SysEnumName from SysEnums where SysEnumType='{0}' order by OrderIndex", enumKey)).AsEnumerable();
        //    foreach (var row in table)
        //        dic.Add(row["SysEnumID"].ToString(), row["SysEnumName"].ToString());
        //    return dic;
        //}
    }
}
