using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Formula.Helper
{
    public static class EntityHelper
    {
        public static IDictionary<string, string> EntityToDictionary<T>(T Entity)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
                dic.Add(property.Name, CommonTool.ToString(property.GetValue(Entity, null)));
            return dic;
        }

        public static IDictionary<string, string> EntityDescriptionToDictionary(Type Entity)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in Entity.GetProperties())
            {
                object[] arr = item.GetCustomAttributes(typeof(DescriptionAttribute), true);
                dic.Add(item.Name, arr.Length > 0 ? ((DescriptionAttribute)arr[0]).Description : item.Name);
            }
            return dic;
        }

        public static T UpdateEntity<T>(T entity, IDictionary<string, object> data) where T : new()
        {
            var destinationType = typeof(T);
            var properties = destinationType.GetProperties();
            foreach (var property in properties)
            {
                if (data.ContainsKey(property.Name))
                    property.SetValue(entity, data[property.Name], null);
            }

            return entity;
        }
    }
}
