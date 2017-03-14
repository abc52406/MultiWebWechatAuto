using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Formula.DbPool;
using System.Web;
using Formula.Helper;
using Common.Config;

namespace Formula
{
    public class FormulaHelper
    {
        #region 生成ID

        /// <summary>
        /// 创建一个按时间排序的Guid
        /// </summary>
        /// <returns></returns>
        public static string CreateGuid()
        {
            //CAST(CAST(NEWID() AS BINARY(10)) + CAST(GETDATE() AS BINARY(6)) AS UNIQUEIDENTIFIER)
            byte[] guidArray = Guid.NewGuid().ToByteArray();
            DateTime now = DateTime.Now;

            DateTime baseDate = new DateTime(1900, 1, 1);

            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);

            TimeSpan msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            Array.Copy(daysArray, 0, guidArray, 2, 2);
            //毫秒高位
            Array.Copy(msecsArray, 2, guidArray, 0, 2);
            //毫秒低位
            Array.Copy(msecsArray, 0, guidArray, 4, 2);
            return new System.Guid(guidArray).ToString();
        }

        public static DateTime GetDateTimeFromGuid(string strGuid)
        {
            Guid guid = Guid.Parse(strGuid);

            DateTime baseDate = new DateTime(1900, 1, 1);
            byte[] daysArray = new byte[4];
            byte[] msecsArray = new byte[4];
            byte[] guidArray = guid.ToByteArray();

            // Copy the date parts of the guid to the respective byte arrays. 
            Array.Copy(guidArray, guidArray.Length - 6, daysArray, 2, 2);
            Array.Copy(guidArray, guidArray.Length - 4, msecsArray, 0, 4);

            // Reverse the arrays to put them into the appropriate order 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Convert the bytes to ints 
            int days = BitConverter.ToInt32(daysArray, 0);
            int msecs = BitConverter.ToInt32(msecsArray, 0);

            DateTime date = baseDate.AddDays(days);
            date = date.AddMilliseconds(msecs * 3.333333);

            return date;
        }

        #endregion

        #region 实体库
        private static Dictionary<string, object> _entitiesContext = new Dictionary<string, object>();
        public static void DisposeEntities()
        {
            foreach (var item in _entitiesContext)
            {
                if (item.Value.GetType().BaseType == typeof(DbContext))
                    ((DbContext)item.Value).Dispose();
            }
        }

        private static Dictionary<string, string> _entitiesDic = new Dictionary<string, string>();
        public static void RegistEntities<T>(ConnEnum conn)
        {
            _entitiesDic[typeof(T).FullName] = conn.ToString();
        }

        public static void RegistEntities<T>(string conn)
        {
            _entitiesDic[typeof(T).FullName] = conn;
        }


        public static T GetEntities<T>() where T : DbContext, new()
        {
            string key = typeof(T).FullName;
            string connName = "";
            string entitiesName = key.Split('.').Last();

            foreach (ConnectionStringSettings item in System.Configuration.ConfigurationManager.ConnectionStrings)
            {
                if (entitiesName.StartsWith(item.Name))
                {
                    connName = item.Name;
                    break;
                }
            }
            if (string.IsNullOrEmpty(connName))
                throw new Exception(string.Format("配置文件中不包含{0}的链接字符串", key));


            ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] { typeof(string) });
            return (T)constructor.Invoke(new object[] { connName });
        }

        private static Dictionary<string, object> _DbContextPool = new Dictionary<string, object>();
        static object l = new object();
        /// <summary>
        /// 从实体池中获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="key">标识符</param>
        /// <returns></returns>
        public static T GetDbContext<T>(object key) where T : DbContext, new()
        {
            string fullname = typeof(T).FullName;
            string connName = "";
            string lastName = fullname.Split('.').Last();

            foreach (ConnectionStringSettings item in System.Configuration.ConfigurationManager.ConnectionStrings)
            {
                if (lastName.StartsWith(item.Name))
                {
                    connName = item.Name;
                    break;
                }
            }
            if (string.IsNullOrEmpty(connName))
                throw new Exception(string.Format("配置文件中不包含{0}的链接字符串", fullname));

            lock (l)
            {
                if (_DbContextPool.ContainsKey(connName))
                {
                    var pool = (DbContextPool<T>)_DbContextPool[connName];
                    return pool.Get(key);
                }
                else
                {
                    var pool = new DbContextPool<T>(connName);
                    _DbContextPool.Add(connName, pool);
                    return pool.Get(key);
                }
            }
        }

        /// <summary>
        /// 释放实体池中的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="key">标识符</param>
        public static void RemoveDbContext<T>(object key) where T : DbContext, new()
        {
            string fullname = typeof(T).FullName;
            string connName = "";
            string lastName = fullname.Split('.').Last();

            foreach (ConnectionStringSettings item in System.Configuration.ConfigurationManager.ConnectionStrings)
            {
                if (lastName.StartsWith(item.Name))
                {
                    connName = item.Name;
                    break;
                }
            }
            if (string.IsNullOrEmpty(connName))
                throw new Exception(string.Format("配置文件中不包含{0}的链接字符串", fullname));


            lock (l)
            {
                if (_DbContextPool.ContainsKey(connName))
                {
                    var pool = (DbContextPool<T>)_DbContextPool[connName];
                    pool.Remove(key);
                }
            }
        }

        /// <summary>
        /// 获取实体池
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        public static DbContextPool<T> GetDbContextPool<T>() where T : DbContext, new()
        {
            string fullname = typeof(T).FullName;
            string connName = "";
            string lastName = fullname.Split('.').Last();
            foreach (ConnectionStringSettings item in System.Configuration.ConfigurationManager.ConnectionStrings)
            {
                if (lastName.StartsWith(item.Name))
                {
                    connName = item.Name;
                    break;
                }
            }
            if (string.IsNullOrEmpty(connName))
                throw new Exception(string.Format("配置文件中不包含{0}的链接字符串", fullname));


            lock (l)
            {
                if (_DbContextPool.ContainsKey(connName))
                    return (DbContextPool<T>)_DbContextPool[connName];
                else
                {
                    var pool = new DbContextPool<T>(connName);
                    _DbContextPool.Add(connName, pool);
                    return pool;
                }
            }
        }
        #endregion

        #region 实体转化

        /// <summary>
        /// 对象列表转化为字典列表
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> CollectionToListDic<T>(ICollection<T> list)
        {
            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

            foreach (var item in list)
            {
                resultList.Add(ModelToDic<T>(item));
            }

            return resultList;
        }

        /// <summary>
        /// 对象转换为字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ModelToDic<T>(T obj)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            PropertyInfo[] arrPtys = typeof(T).GetProperties();
            foreach (PropertyInfo destPty in arrPtys)
            {
                if (destPty.CanRead == false)
                    continue;
                if (destPty.PropertyType.Name == "ICollection`1")
                    continue;
                if ((destPty.PropertyType.IsClass && destPty.PropertyType.Name != "String") || destPty.PropertyType.IsArray || destPty.PropertyType.IsInterface)
                    continue;
                object value = destPty.GetValue(obj, null);
                dic.Add(destPty.Name, value);
            }
            return dic;
        }

        public static void UpdateModel(object dest, object src)
        {
            PropertyInfo[] destPtys = dest.GetType().GetProperties();
            PropertyInfo[] srcPtys = src.GetType().GetProperties();

            foreach (PropertyInfo destPty in destPtys)
            {
                if (destPty.CanRead == false)
                    continue;
                if (destPty.PropertyType.Name == "ICollection`1")
                    continue;
                if ((destPty.PropertyType.IsClass && destPty.PropertyType.Name != "String") || destPty.PropertyType.IsArray || destPty.PropertyType.IsInterface)
                    continue;

                PropertyInfo srcPty = srcPtys.Where(c => c.Name == destPty.Name).SingleOrDefault();
                if (srcPty == null)
                    continue;
                if (srcPty.CanWrite == false)
                    continue;

                object value = srcPty.GetValue(src, null);

                destPty.SetValue(dest, value, null);
            }
        }


        public static List<T> DataTableToList<T>(DataTable dt) where T : new()
        {
            PropertyInfo[] pis = typeof(T).GetProperties();
            //dt.Columns.IndexOf(
            List<T> list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T t = new T();
                foreach (PropertyInfo pi in pis)
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        if (row[pi.Name] != DBNull.Value)
                            pi.SetValue(t, row[pi.Name], null);
                    }
                }
                list.Add(t);
            }
            return list;

        }

        #endregion

        #region 获取单例化服务
        /// <summary>
        /// 获取服务（服务是单例的）
        /// </summary>
        /// <typeparam name="T">服务接口</typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {
            if (_DicSingletonSerivces.ContainsKey(typeof(T)))
            {
                return (T)Activator.CreateInstance(_DicSingletonSerivces[typeof(T)]);
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <typeparam name="T1">接口</typeparam>
        /// <typeparam name="T2">实现</typeparam>
        public static void RegisterService<T1, T2>()
            where T2 : T1
        //where T1 : ISingleton
        {
            _DicSingletonSerivces[typeof(T1)] = typeof(T2);
        }
        private static Dictionary<Type, Type> _DicSingletonSerivces = new Dictionary<Type, Type>();
        #endregion


        #region 当前用户

        public static string UserID
        {
            get
            {
                return GetUserInfo().UserID;
            }
        }

        public static UserInfo GetUserInfo()
        {
            string systemName = "";
            if (HttpContext.Current != null && HttpContext.Current.User != null)
                systemName = HttpContext.Current.User.Identity.Name;
            if (string.IsNullOrEmpty(systemName))
                return new UserInfo();

            string key = "UserInfo_" + systemName;

            UserInfo user = (UserInfo)CacheHelper.Get(key);
            if (user == null)
            {
                user = SysConfig.GetUserInfoBySysName(systemName);
                if (user != null)
                {
                    CacheHelper.Set(key, user);
                }

            }
            return user;
        }

        public static UserInfo GetUserInfoByID(string userID)
        {
            return SysConfig.GetUserInfoByID(userID);
        }

        #endregion
    }
}
