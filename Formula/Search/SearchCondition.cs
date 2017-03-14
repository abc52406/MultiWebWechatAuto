namespace Formula.Search
{
    using System.Collections.Generic;
    using System;
    using System.Text;

    /// <summary>
    /// 包含查询、分页和排序
    /// </summary>
    [Serializable]
    public class SearchCondition
    {

        #region AddSearch

        /// <summary>
        /// 增加查询条件，查询方式默认为等于查询
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">字段值</param>
        /// <returns></returns>
        public SearchCondition AddSearch(string fieldName, string fieldValue)
        {
            return AddSearch(fieldName, fieldValue, Mode.Equal);
        }

        /// <summary>
        /// 增加查询条件，查询方式默认为等于查询
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">字段值</param>
        /// <returns></returns>
        public SearchCondition AddSearch(string fieldName, SpecialValue fieldValue)
        {
            return AddSearch(fieldName, fieldValue.ToString());
        }

        /// <summary>
        /// 增加查询条件
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="searchMode">查询方式</param>
        /// <returns></returns>
        public SearchCondition AddSearch(string fieldName, string fieldValue, Mode searchMode)
        {
            SingleCondition cnd = new SingleCondition(fieldName, fieldValue, searchMode);

            _searchSql += cnd.ToSqlString();

            return this;
        }

        /// <summary>
        /// 增加基本查询条件，查询方式默认为等于查询
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">字段值</param>
        /// <returns></returns>
        public SearchCondition AddBaseSearch(string fieldName, string fieldValue)
        {
            return AddBaseSearch(fieldName, fieldValue, Mode.Equal);
        }

        /// <summary>
        /// 增加基本查询条件
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="searchMode">查询方式</param>
        /// <returns></returns>
        public SearchCondition AddBaseSearch(string fieldName, string fieldValue, Mode searchMode)
        {
            SingleCondition cnd = new SingleCondition(fieldName, fieldValue, searchMode);

            _baseSearchSql += cnd.ToSqlString();

            return this;
        }

        #endregion

        #region AddSort

        /// <summary>
        /// 增加排序条件，如果已经有本字段的排序，则改变排序方向，默认降序
        /// </summary>
        /// <param name="sortField">排序字段</param>
        public SearchCondition AddSort(string sortField)
        {
            sortField = sortField.ToLower().Trim();

            if (_sortSql.Trim() == "" || _sortSql.IndexOf(sortField) < 0)
                return AddSort(sortField, SortDir.desc);

            string[] strAry = _sortSql.Split(new char[] { ',' }, StringSplitOptions.None);
            for (int i = 0; i < strAry.Length; i++)
            {
                if (strAry[i].IndexOf(sortField) >= 0 && strAry[i].IndexOf(SortDir.desc.ToString()) >= 0)
                {
                    strAry[i] = string.Format("{0} {1}", sortField, SortDir.asc.ToString());
                    break;
                }

            }
            _sortSql = string.Join(",", strAry);

            return this;

        }
        /// <summary>
        /// 增加排序条件
        /// </summary>
        /// <param name="sortField">排序字段</param>
        public SearchCondition AddSort(string sortField, SortDir dir)
        {
            sortField = sortField.ToLower().Trim();

            if (_sortSql.Trim() == "")
                _sortSql = string.Format("{0} {1}", sortField, dir);
            else
            {
                if (_sortSql.IndexOf(sortField) >= 0)
                {
                    string[] strAry = _sortSql.Split(new char[] { ',' }, StringSplitOptions.None);
                    for (int i = 0; i < strAry.Length; i++)
                    {
                        if (strAry[i].IndexOf(sortField) >= 0)
                        {
                            strAry[i] = string.Format("{0} {1}", sortField, dir.ToString());
                            break;
                        }

                    }
                    _sortSql = string.Join(",", strAry);
                }
                else
                {
                    _sortSql = string.Format("{0},{1} {2}", _sortSql, sortField, dir.ToString());
                }
            }
            return this;
        }

        #endregion

        #region Sql

        #region SearchSql

        /// <summary>
        /// 获取查询条件，包含基本查询条件，包含前置空格,以and开头，不包含where
        /// </summary>
        /// <returns></returns>
        public string GetSearchSql()
        {
            string sql = string.Format(" {0} {1}", _baseSearchSql, _searchSql).Trim();
            if (sql != "")
                sql = " " + sql;
            return sql;
        }

        /// <summary>
        /// 不以and开头，不包含where
        /// </summary>
        /// <returns></returns>
        public string GetSearchSqlForLinq()
        {
            string sql = GetSearchSql();
            sql = sql.Replace('(', ' ').Replace(')', ' ').Trim();
            if (sql == "")
                return "";
            sql = sql.Substring(3);
            return sql;

        }

        /// <summary>
        /// 设置查询条件，不涉及基本查询条件
        /// </summary>
        /// <param name="sql">以and开头的查询条件，不需要前置空格</param>
        public SearchCondition SetSearchSql(string sql)
        {
            if (!sql.Trim().StartsWith("and", StringComparison.CurrentCultureIgnoreCase))
                sql = "and " + sql.Trim();
            _searchSql = sql.ToLower().Trim();
            return this;
        }

        /// <summary>
        /// 增加查询条件
        /// </summary>
        /// <param name="sql">以and开头的查询条件，不需要前置空格</param>
        public SearchCondition AddSearchSql(string sql)
        {
            _searchSql = string.Format("{0} {1}", _searchSql, sql.ToLower().Trim());
            return this;
        }

        /// <summary>
        /// 设置基本查询条件
        /// </summary>
        /// <param name="sql">以and开头的查询条件，不需要前置空格</param>
        public SearchCondition SetBaseSearchSql(string sql)
        {
            _baseSearchSql = sql.ToLower().Trim();
            return this;
        }

        /// <summary>
        /// 增加基本查询条件
        /// </summary>
        /// <param name="sql">以and开头的查询条件，不需要前置空格</param>
        public SearchCondition AddBaseSearchSql(string sql)
        {
            _baseSearchSql = string.Format("{0} {1}", _baseSearchSql, sql.ToLower().Trim());
            return this;
        }

        #endregion

        #region SortSql

        /// <summary>
        /// 获取排序条件Sql字符串，包含前置空格，不包含Order by
        /// </summary>
        /// <returns></returns>
        public string GetSortSql()
        {
            if (!string.IsNullOrEmpty(_defaultSortSql.Trim()) && !string.IsNullOrEmpty(_sortSql.Trim()))
                return string.Format(" {0},{1}", _sortSql.Trim(), _defaultSortSql);
            if (!string.IsNullOrEmpty(_defaultSortSql.Trim()))
                return string.Format(" {0}", _defaultSortSql.Trim());
            if (!string.IsNullOrEmpty(_sortSql.Trim()))
                return string.Format(" {0}", _sortSql.Trim());

            return "";
        }

        /// <summary>
        /// 设置默认查询条件，格式为例如“UserCode desc”，可以设置为空字符串
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SearchCondition SetDefaultSortSql(string sql)
        {
            _defaultSortSql = sql.Trim();
            return this;
        }

        /// <summary>
        /// 设置排序条件，不需要前置空格，不要加Order by
        /// </summary>
        /// <param name="sql"></param>
        public SearchCondition SetSortSql(string sql)
        {
            _sortSql = sql.Trim();
            return this;
        }

        /// <summary>
        /// 增加排序条件，不需要前置空格，不要加Order by，注意与原有排序条件不要冲突
        /// </summary>
        /// <param name="sql"></param>
        public SearchCondition AddSortSql(string sql)
        {
            _sortSql = string.Format("{0} {1}", _sortSql, sql);
            return this;
        }

        #endregion

        #region Fields

        /// <summary>
        /// 获取要查询的字段，默认为*
        /// </summary>
        /// <returns></returns>
        public string GetFields()
        {
            string str = _fieldsSql.Trim(new char[] { ' ', ',' });
            return str;
        }

        /// <summary>
        /// 设置要查询的字段，多个字段用逗号隔开
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public SearchCondition SetFields(string fields)
        {
            _fieldsSql = fields.Trim(new char[] { ' ', ',' });
            return this;
        }

        /// <summary>
        /// 增加要查询的字段，多个字段用逗号隔开
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public SearchCondition AddFields(string fields)
        {
            _fieldsSql = string.Format("{0},{1}", _fieldsSql, fields.Trim(new char[] { ' ', ',' }));
            _fieldsSql = _fieldsSql.Trim(new char[] { ' ', ',', '*' });
            return this;
        }


        #endregion

        #region Distinct

        /// <summary>
        /// 设置是否消除重复行，默认为false
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public SearchCondition SetDistinct(bool flag)
        {
            _distinct = flag;
            return this;
        }

        /// <summary>
        /// 获取是否消除重复行
        /// </summary>
        /// <returns></returns>
        public string GetDistinct()
        {
            if (_distinct)
                return "distinct";
            else
                return "";
        }


        #endregion

        #endregion

        #region Clear

        /// <summary>
        /// 清空查询条件和排序条件，但不能清空基本查询条件和查询的字段
        /// </summary>
        public void Clear()
        {
            _searchSql = "";
            _sortSql = "";
        }

        #endregion

        #region 私有字段

        //默认排序条件
        private string _defaultSortSql = "";

        //排序条件
        private string _sortSql = "";
        //基础查询条件
        private string _baseSearchSql = "";
        //查询条件
        protected string _searchSql = "";
        //查询的字段
        protected string _fieldsSql = "*";
        //是否消除重复行
        public bool _distinct = false;


        //是否允许分页，默认false
        private bool _allowPaging = false;
        //当前页
        private int _currentPage = 1;
        //每页记录条数
        private int _pageSize = _defaultPageSize;
        //总记录数量
        private int _recordCount = -1;
        //总页数
        private int _pageCount = -1;
        //是否获取总记录数量
        private bool _getRecordCount = false;

        #endregion

        #region 属性


        /// <summary>
        /// 设置每页默认的记录数，默认值为10条,
        /// </summary>
        /// <param name="pageSize"></param>
        public static int DefaultPageSize
        {
            get { return _defaultPageSize; }
            set { _defaultPageSize = value; }
        }
        private static int _defaultPageSize = 10;
        /// <summary>
        /// 是否允许分页,默认false
        /// </summary>
        public bool AllowPaging
        {
            get { return _allowPaging; }
            set { _allowPaging = value; }
        }

        /// <summary>
        /// 当前第几页，默认为1
        /// </summary>
        public int CurrentPage
        {
            get
            {
                return this._currentPage;
            }
            set
            {
                this._currentPage = value;
            }
        }

        /// <summary>
        /// 是否获取总记录数，默认false
        /// </summary>
        public bool GetRecordCount
        {
            get
            {
                return this._getRecordCount;
            }
            set
            {
                this._getRecordCount = value;
            }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (_pageCount != -1)
                    return _pageCount;

                if (this.PageSize >= 0)
                {
                    _pageCount = (((this.RecordCount - 1) / this.PageSize) + 1);
                    return _pageCount;
                }
                return 1;
            }
        }

        /// <summary>
        /// 每页记录条数，默认为10
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                this._pageSize = value;
                this._pageCount = -1;
            }
        }

        /// <summary>
        /// 记录数量
        /// </summary>
        public int RecordCount
        {
            get
            {
                return this._recordCount;
            }
            set
            {
                this._recordCount = value;
                this._pageCount = -1;
            }
        }


        #endregion

        #region 静态方法
        #region 创建分页查询sql
        /// <summary>
        /// 创建分页查询sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cnd"></param>
        /// <returns></returns>
        public static string CreatePagerSql(string sql, SearchCondition cnd)
        {
            string commandText = "";

            if (sql.Trim().Substring(0, 4).ToLower() != "from")
                sql = string.Format(" from ({0}) as tableSource", sql);

            if (!cnd.AllowPaging)
            {
                commandText = string.Format("select {0} {1} {2} where 1=1 {3}", cnd.GetDistinct(), cnd.GetFields(), sql, cnd.GetSearchSql());
                if (!string.IsNullOrEmpty(cnd.GetSortSql().Trim()))
                    commandText += " order by" + cnd.GetSortSql();

                return commandText;
            }

            if (cnd.GetSortSql().Trim() == "")
                throw new ApplicationException("需要指明排序字段");



            commandText = @"select * from (select Row_number() over(order by {2}) as RowNumber,* from (select {0} {1} {3} where 1=1 {4}) as tmpTable1) as tempTable2 where RowNumber between {5} and {6} ";

            commandText = string.Format(commandText, cnd.GetDistinct(), cnd.GetFields(), cnd.GetSortSql(), sql, cnd.GetSearchSql(), cnd.CurrentPage * cnd.PageSize + 1, (cnd.CurrentPage+1) * cnd.PageSize);

            return commandText;
        }
        #endregion

        #region 创建查询数量sql
        /// <summary>
        /// 创建查询数量sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cnd"></param>
        /// <param name="sumField">汇总列</param>
        /// <returns></returns>
        public static string CreateCountSql(string sql, SearchCondition cnd, string sumField = "")
        {
            string commandText = "";

            if (sql.Trim().Substring(0, 4).ToLower() != "from")
                sql = string.Format(" from ({0}) as tableSource", sql);

            commandText = @"select RecordCount=count(*){4} from (select {0} {1} {2} where 1=1 {3}) a";

            commandText = string.Format(commandText, cnd.GetDistinct(), cnd.GetFields(), sql, cnd.GetSearchSql(), sumField == "" ? "" : ("," + sumField.TrimStart(new char[] { ',' ,' '})));

            return commandText;
        }
        #endregion
        #endregion
    }
}
