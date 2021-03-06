﻿using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Formula.Helper
{
    /// <summary>
    /// Summary description for JsonHelper
    /// </summary>
    public static class JsonHelper
    {
        public static string ToJson<T>(T obj)
        {

            if (obj == null || obj.ToString() == "null") return null;

            if (obj != null && (obj.GetType() == typeof(String) || obj.GetType() == typeof(string)))
            {
                return obj.ToString();
            }

            IsoDateTimeConverter dt = new IsoDateTimeConverter();
            dt.DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
            return JsonConvert.SerializeObject(obj, dt);

        }

        /// <summary>    
        ///  从一个Json串生成对象信息   
        ///  </summary>        
        ///  <param name="jsonString">JSON字符串</param>    
        /// <typeparam name="T">对象类型</typeparam>         
        /// <returns></returns>        
        public static T ToObject<T>(string json) where T : class
        {
            if (String.IsNullOrEmpty(json)) return null;
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        /// <summary>
        /// 组装对象
        /// </summary>
        /// <param name="json"></param>
        /// <param name="obj"></param>
        public static void PopulateObject(string json, object obj)
        {
            if (String.IsNullOrEmpty(json)) return;
            JsonConvert.PopulateObject(json, obj);
        }
    }
}
