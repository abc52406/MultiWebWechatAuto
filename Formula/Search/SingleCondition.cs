namespace Formula.Search
{
    using System;

    [Serializable]
    public class SingleCondition
    {
        #region 构造方法

        /// <summary>
        /// 构造查询条件，默认为等于查询
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="value">查询值</param>
        public SingleCondition(string fieldName, string value)
            : this(fieldName, value, Mode.Equal)
        {
        }



        /// <summary>
        /// 构造查询条件
        /// </summary>
        /// <param name="conditionName">字段名</param>
        /// <param name="conditionValue">查询值</param>
        /// <param name="mode">查询方式</param>
        public SingleCondition(string fieldName, string value, Mode mode)
        {
            this.FieldName = fieldName;
            this.FieldValue = value;
            this.SearchMode = mode;

        }

        #endregion

        #region 属性

        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段值
        /// </summary>
        public string FieldValue { get; set; }

        /// <summary>
        /// 查询方式
        /// </summary>
        public Mode SearchMode { get; set; }


        #endregion

        #region ToSqlString

        /// <summary>
        /// 转化为 " and field = 'value'" 形式的字符串
        /// </summary>
        /// <returns></returns>
        public string ToSqlString()
        {
            string strSql = "";

            if (FieldValue == null)
                FieldValue = "";

            string fieldValue = FieldValue.Replace("\'", "\\'").Replace("\"", "\\\"").Trim();


            switch (SearchMode)
            {
                case Mode.Equal:
                    strSql = string.Format(" and {0} = '{1}'", FieldName, fieldValue);
                    break;
                case Mode.NotEqual:
                    strSql = string.Format(" and {0} <> '{1}'", FieldName, fieldValue);
                    break;
                case Mode.In:
                    if (fieldValue.Trim() != "")
                        strSql = string.Format(" and {0} in ('{1}')", FieldName, fieldValue.Trim(',').Replace(",", "','"));
                    break;
                case Mode.NotIn:
                    strSql = string.Format(" and {0} not in ('{1}')", FieldName, fieldValue.Replace(",", "','"));
                    break;
                case Mode.Like:
                    strSql = string.Format(" and {0} like '%{1}%'", FieldName, fieldValue);
                    break;
                case Mode.NotLike:
                    strSql = string.Format(" and {0} not like '%{1}%'", FieldName, fieldValue);
                    break;
                case Mode.StartWith:
                    strSql = string.Format(" and {0} like '{1}%'", FieldName, fieldValue);
                    break;
                case Mode.EndWith:
                    strSql = string.Format(" and {0} like '%{1}'", FieldName, fieldValue);
                    break;
                case Mode.NotStartWith:
                    strSql = string.Format(" and {0} not like '{1}%'", FieldName, fieldValue);
                    break;
                case Mode.NotEndWith:
                    strSql = string.Format(" and {0} not like '%{1}'", FieldName, fieldValue);
                    break;
                case Mode.MoreThan:
                    strSql = string.Format(" and {0} > '{1}'", FieldName, fieldValue);
                    break;
                case Mode.MoreThanEqual:
                    strSql = string.Format(" and {0} >= '{1}'", FieldName, fieldValue);
                    break;
                case Mode.LessThan:
                    strSql = string.Format(" and {0} < '{1}'", FieldName, fieldValue);
                    break;
                case Mode.LessThanEqual:
                    strSql = string.Format(" and {0} <= '{1}'", FieldName, fieldValue);
                    break;
            }

            switch (fieldValue)
            {
                case "IsNull":
                    strSql = string.Format(" and {0} is null", FieldName);
                    break;
                case "IsNotNull":
                    strSql = string.Format(" and {0} is not null", FieldName);
                    break;
                case "IsNullOrEmpty":
                    strSql = string.Format(" and ({0} is null or {0} = '')", FieldName);
                    break;
                case "IsNotNullorEmpty":
                    strSql = string.Format(" and ({0} is not null and {0} <> '')", FieldName);
                    break;
            }


            return strSql;
        }

        #endregion
    }
}

