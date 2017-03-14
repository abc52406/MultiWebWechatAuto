using System;
using System.Data.Entity;

namespace Formula.DbPool
{
    public class DbContextStruct<T> : IDisposable where T : DbContext, new()
    {
        T entities;

        public DbContextStruct(T e)
        {
            entities = e;
        }

        /// <summary>
        /// 是否正在使用
        /// </summary>
        public bool IsUse
        {
            get;
            set;
        }

        /// <summary>
        /// 标识符
        /// </summary>
        public object Key
        {
            get;
            set;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get;
            set;
        }

        /// <summary>
        /// 实体
        /// </summary>
        public T Entities
        {
            get
            {
                return entities;
            }
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
}
