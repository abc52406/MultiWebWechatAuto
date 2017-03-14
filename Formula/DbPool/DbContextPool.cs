using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Formula.DbPool
{
    /// <summary>
    /// 实体池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbContextPool<T> : IDisposable where T : DbContext, new()
    {
        List<DbContextStruct<T>> list = new List<DbContextStruct<T>>();
        string _ConnName = string.Empty;
        int _CreateContextCount = 1;

        /// <summary>
        /// 连接池空了之后，重新创建连接池的数量
        /// </summary>
        /// <param name="ConnName"></param>
        /// <param name="CreateContextCount"></param>
        public DbContextPool(string ConnName,int CreateContextCount = 500)
        {
            string fullname = typeof(T).FullName;
            string lastName = fullname.Split('.').Last();
            _CreateContextCount = CreateContextCount;
            _ConnName = ConnName;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="Key">标识符</param>
        /// <returns></returns>
        public T Get(object Key)
        {
            lock (this)
            {
                var free = list.Where(c => c.IsUse == false);
                if (free.Count() == 0)
                {
                    for (int i = 0; i < _CreateContextCount; i++)
                    {
                        ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] { typeof(string) });
                        var dbcontext = (T)constructor.Invoke(new object[] { _ConnName });
                        list.Add(new DbContextStruct<T>(dbcontext)
                        {
                            IsUse = false,
                            Key = Key,
                            StartTime = DateTime.Now,
                        });
                    }
                    free = list.Where(c => c.IsUse == false);
                }
                var s = free.First();
                s.IsUse = true;
                s.Key = Key;
                s.StartTime = DateTime.Now;
                return s.Entities;
            }
        }

        /// <summary>
        /// 释放实体
        /// </summary>
        /// <param name="Key"></param>
        public void Remove(object Key)
        {
            lock (this)
            {
                var s = list.Where(c => c.Key == Key).FirstOrDefault();
                if (s != null)
                {
                    list.Remove(s);
                    s.IsUse = false;
                    s.Key = null;
                    s.Entities.Dispose();
                }
            }
        }

        /// <summary>
        /// 正在使用的实体数量
        /// </summary>
        public int UsedCount
        {
            get
            {
                lock (this)
                {
                    return list.Where(c => c.IsUse = true).Count();
                }
            }
        }

        /// <summary>
        /// 空闲的实体数量
        /// </summary>
        public int NotUseCount
        {
            get
            {
                lock (this)
                {
                    return list.Where(c => c.IsUse = false).Count();
                }
            }
        }

        /// <summary>
        /// 实体数量
        /// </summary>
        public int Count
        {
            get
            {
                lock (this)
                {
                    return list.Count();
                }
            }
        }

        public void Dispose()
        {
            lock (this)
            {
                var count = list.Count;
                for (int i = count - 1; i > -1; i--)
                {
                    var obj = list[i];
                    obj.Dispose();
                    list.Remove(obj);
                }
            }
        }
    }
}
