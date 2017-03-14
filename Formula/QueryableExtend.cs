using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Formula
{
    public static class BaseQueryableExtend
    {
        public static void Delete<TEntity>(this IDbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            foreach (var item in dbSet.Where(predicate).ToArray())
            {
                dbSet.Remove(item);
            }
        }

        public static void RemoveWhere<TEntity>(this ICollection<TEntity> collection, Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var item in collection.AsQueryable().Where(predicate).ToArray())
            {
                collection.Remove(item);
            }
        }

        public static void Update<TEntity>(this IQueryable<TEntity> query, Action<TEntity> updateAction)
        {
            foreach (var item in query.ToArray())
            {
                updateAction(item);
            }
        }

        public static void Update<TEntity>(this ICollection<TEntity> collection, Action<TEntity> updateAction)
        {
            collection.AsQueryable().Update(updateAction);
        }


        #region 动态OrderBy

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> query, string property, bool isAscending, bool isThenBy = false)
        {
            ParameterExpression param = System.Linq.Expressions.Expression.Parameter(typeof(TEntity), "it");
            System.Linq.Expressions.Expression body = param;
            if (Nullable.GetUnderlyingType(body.Type) != null)
                body = System.Linq.Expressions.Expression.Property(body, "Value");

            PropertyInfo sortProperty = typeof(TEntity).GetProperty(property, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (sortProperty == null)
                throw new Exception("对象上不存在" + property + "的字段");
            body = System.Linq.Expressions.Expression.MakeMemberAccess(body, sortProperty);
            LambdaExpression keySelectorLambda = System.Linq.Expressions.Expression.Lambda(body, param);
            string queryMethod = isAscending ? "OrderBy" : "OrderByDescending";
            if (isThenBy == true)
                queryMethod = isAscending ? "ThenBy" : "ThenByDescending";
            query = query.Provider.CreateQuery<TEntity>(System.Linq.Expressions.Expression.Call(typeof(Queryable), queryMethod,
                                                      new Type[] { typeof(TEntity), body.Type },
                                                      query.Expression,
                                                       System.Linq.Expressions.Expression.Quote(keySelectorLambda)));
            return query;
        }

        #endregion
    }
}
