﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Common.Config
{
    /// <summary>
    /// 数据库操作工具类
    /// </summary>
    public class SQLHelper
    {

        public static DataTable ExecuteTable(string connName, string sql)
        {
            return SQLHelper.CreateSqlHelper(connName).ExecuteDataTable(sql);
        }

        public static string ExecuteNonQuery(string connName, string sql)
        {
            return SQLHelper.CreateSqlHelper(connName).ExecuteNonQuery(sql);
        }

        public static object ExecuteScalar(string connName, string sql)
        {
            return SQLHelper.ExecuteScalar(connName, sql);
        }




        #region 静态构造方法
        private static Dictionary<string, object> _dic = new Dictionary<string, object>();
        public static SQLHelper CreateSqlHelper(string connName)
        {
            string key = string.Format("{0}Entities", connName);

            SQLHelper sqlHelper = null;
            if (_dic.Keys.Contains(key))
                return _dic[key] as SQLHelper;

            sqlHelper = new SQLHelper(ConfigurationManager.ConnectionStrings[connName].ConnectionString);
            _dic[key] = sqlHelper;

            return sqlHelper;
        }

        #endregion

        #region 常量
        // 数据库连接字符串
        private string connStr = "";
        private string _dbName = "";

        public string ConnStr
        {
            get
            {
                return connStr;
            }
        }

        public string Name
        {
            get;
            set;
        }

        public string ShortName
        {
            get
            {
                return Name.Remove(0, Name.LastIndexOf('_') + 1);
            }
        }

        public string DbName
        {
            get
            {
                if (_dbName == "")
                {
                    if (connStr == "")
                        _dbName = ""; ;
                    SqlConnection conn = new SqlConnection(connStr);
                    _dbName = conn.Database;
                }
                return _dbName;
            }
        }
        #endregion

        #region 构造函数
        internal SQLHelper(string connStr)
        {
            this.connStr = connStr;
        }
        #endregion

        #region SQL无返回值可操作的方法

        //带事务，带参，带SQL执行类型，无返回值的SQL执行
        public string ExecuteNonQuery(SqlTransaction tran, string cmdText, SqlParameter[] parms, CommandType cmdtype)
        {
            string retVal = "0";

            SqlCommand cmd = new SqlCommand(cmdText);
            cmd.Connection = tran.Connection;
            cmd.Transaction = tran;
            cmd.CommandType = cmdtype;
            cmd.CommandTimeout = 600;

            if (parms != null)
            {
                //添加参数
                foreach (SqlParameter parm in parms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
            retVal = cmd.ExecuteNonQuery().ToString();

            return retVal;
        }


        //带连接字符串，带参，带SQL执行类型，无返回值的SQL执行
        public string ExecuteNonQuery(string constr, string cmdText, SqlParameter[] parms, CommandType cmdtype)
        {
            string retVal = "0";
            using (SqlConnection conn = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.CommandType = cmdtype;

                if (parms != null)
                {
                    //添加参数
                    foreach (SqlParameter parm in parms)
                    {
                        cmd.Parameters.Add(parm);
                    }
                }
                try
                {
                    conn.Open();
                    retVal = cmd.ExecuteNonQuery().ToString();
                }
                catch (Exception exp)
                {
                    retVal = exp.Message;
                }
                finally
                {
                    conn.Close();
                }
                return retVal;
            }
        }


        //带连接，带参，带SQL执行类型，无返回值的SQL执行
        public string ExecuteNonQuery(SqlConnection conn, string cmdText, SqlParameter[] parms, CommandType cmdtype)
        {
            string retVal = "0";
            SqlCommand cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = cmdtype;

            if (parms != null)
            {
                //添加参数
                foreach (SqlParameter parm in parms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                retVal = cmd.ExecuteNonQuery().ToString();
            }
            catch (Exception exp)
            {
                retVal = exp.Message;
            }
            finally
            {
                conn.Close();
            }
            return retVal;
        }


        //带参，带SQL执行类型，无返回值的SQL执行
        public string ExecuteNonQuery(string cmdText, SqlParameter[] parms, CommandType cmdtype)
        {
            string retVal = "0";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.CommandType = cmdtype;

                if (parms != null)
                {
                    //添加参数
                    foreach (SqlParameter parm in parms)
                    {
                        cmd.Parameters.Add(parm);
                    }
                }
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    retVal = cmd.ExecuteNonQuery().ToString();
                }
                catch (Exception exp)
                {
                    //retVal = exp.Message;
                    throw exp;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
            return retVal;
        }


        //带SQL执行类型，无返回值的SQL执行
        public string ExecuteNonQuery(string cmdText, CommandType cmdtype)
        {
            string retVal = "0";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.CommandType = cmdtype;

                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    retVal = cmd.ExecuteNonQuery().ToString();
                }
                catch (Exception exp)
                {
                    retVal = exp.Message;
                }
                finally
                {
                    conn.Close();
                }
            }
            return retVal;
        }

        //带错误输出，无返回值的SQL执行（默认是Text）
        public string ExecuteNonQuery(string cmdText)
        {
            string retVal = "0";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.CommandType = CommandType.Text;

                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    retVal = cmd.ExecuteNonQuery().ToString();
                }
                catch (Exception exp)
                {
                    //retVal = exp.Message;
                    throw exp;
                }
                finally
                {
                    conn.Close();
                }
            }
            return retVal;
        }


        #endregion

        #region SQL有返回值可操作的方法

        //带连接，带参，带SQL执行类型，有返回值的SQL执行
        public object ExecuteScalar(SqlConnection conn, string cmdText, SqlParameter[] parms, CommandType cmdtype)
        {
            object retVal = null;

            SqlCommand cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = cmdtype;

            if (parms != null)
            {
                //添加参数
                foreach (SqlParameter parm in parms)
                {
                    cmd.Parameters.Add(parm);
                }
            }

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                retVal = cmd.ExecuteScalar();
            }
            catch (Exception exp)
            {
                retVal = (object)exp.Message;
            }
            finally
            {
                conn.Close();
            }
            return retVal;
        }

        //带连接字符串，带参，带SQL执行类型，有返回值的SQL执行
        public object ExecuteScalar(string constr, string cmdText, SqlParameter[] parms, CommandType cmdtype)
        {
            object retVal = null;

            using (SqlConnection conn = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.CommandType = cmdtype;

                if (parms != null)
                {
                    //添加参数
                    foreach (SqlParameter parm in parms)
                    {
                        cmd.Parameters.Add(parm);
                    }
                }

                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    retVal = cmd.ExecuteScalar();
                }
                catch (Exception exp)
                {
                    retVal = (object)exp.Message;
                }
                finally
                {
                    conn.Close();
                }
            }
            return retVal;
        }

        //带参，带SQL执行类型，有返回值的SQL执行
        public object ExecuteScalar(string cmdText, SqlParameter[] parms, CommandType cmdtype)
        {
            object retVal = null;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.CommandType = cmdtype;

                if (parms != null)
                {
                    //添加参数
                    foreach (SqlParameter parm in parms)
                    {
                        cmd.Parameters.Add(parm);
                    }
                }

                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    retVal = cmd.ExecuteScalar();
                }
                catch (Exception exp)
                {
                    retVal = (object)exp.Message;
                }
                finally
                {
                    conn.Close();
                }
            }
            return retVal;
        }

        //带参，有返回值的SQL执行
        public object ExecuteScalar(string cmdText, CommandType cmdtype)
        {
            object retVal = null;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.CommandType = cmdtype;

                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    retVal = cmd.ExecuteScalar();
                }
                catch (Exception exp)
                {
                    retVal = (object)exp.Message;
                }
                finally
                {
                    conn.Close();
                }
            }

            return retVal;
        }

        //有返回值的SQL执行
        public object ExecuteScalar(string cmdText)
        {
            object retVal = null;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.CommandType = CommandType.Text;

                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    retVal = cmd.ExecuteScalar();
                }
                catch (Exception exp)
                {
                    retVal = (object)exp.Message;
                }
                finally
                {
                    conn.Close();
                }
            }

            return retVal;
        }

        #endregion

        #region 返回数据读取器(DataReader)可操作的方法

        //带连接字符串，带参，带SQL执行类型，有返回值的SQL执行
        public SqlDataReader ExecuteReader(string constr, string cmdText, SqlParameter[] parms, CommandType cmdtype)
        {
            SqlDataReader reader;
            SqlConnection conn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = cmdtype;

            if (parms != null)
            {
                //添加参数
                foreach (SqlParameter parm in parms)
                    cmd.Parameters.Add(parm);
            }
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                //将关闭数据库的操作交给Read操作，即关闭了read就关闭了数据库
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        //带连接，带参，带SQL执行类型，有返回值的SQL执行
        public SqlDataReader ExecuteReader(SqlConnection conn, string cmdText, SqlParameter[] parms, CommandType cmdtype)
        {
            SqlDataReader reader;
            SqlCommand cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = cmdtype;

            if (parms != null)
            {
                //添加参数
                foreach (SqlParameter parm in parms)
                    cmd.Parameters.Add(parm);
            }
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                //将关闭数据库的操作交给Read操作，即关闭了read就关闭了数据库
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        //带参，带SQL执行类型，有返回值的SQL执行
        public SqlDataReader ExecuteReader(string cmdText, SqlParameter[] parms, CommandType cmdtype)
        {
            SqlDataReader reader;
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = cmdtype;

            if (parms != null)
            {
                //添加参数
                foreach (SqlParameter parm in parms)
                    cmd.Parameters.Add(parm);
            }
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                //将关闭数据库的操作交给Read操作，即关闭了read就关闭了数据库
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        //带SQL执行类型，有返回值的SQL执行
        public SqlDataReader ExecuteReader(string cmdText, CommandType cmdtype)
        {
            SqlDataReader reader;

            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = cmdtype;
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        //有返回值的SQL执行
        public SqlDataReader ExecuteReader(string cmdText)
        {
            SqlDataReader reader;

            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        #endregion

        #region 返回DataTable可操作的方法

        //带参，带SQL执行类型，返回DataTable的SQL执行
        public DataTable ExecuteDataTable(string cmdText, int statRowNum, int maxRowNum, SqlParameter[] parms, CommandType cmdtype)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            int dtflag = 0;

            SqlConnection conn = new SqlConnection(connStr);
            SqlDataAdapter apt = new SqlDataAdapter(cmdText, conn);
            apt.SelectCommand.CommandType = cmdtype;

            if (parms != null)
            {
                //添加参数
                foreach (SqlParameter parm in parms)
                {
                    apt.SelectCommand.Parameters.Add(parm);
                }
            }

            try
            {
                if (statRowNum == 0 && maxRowNum == 0)
                {
                    apt.Fill(dt);
                }
                else
                {
                    apt.Fill(ds, statRowNum, maxRowNum, "ThisTable");
                    dtflag = 1;
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

            if (dtflag == 1)
            {
                dt = ds.Tables["ThisTable"];
            }
            return dt;
        }

        //带参，带SQL执行类型，返回DataTable的SQL执行
        public DataTable ExecuteDataTable(string cmdText, SqlParameter[] parms, CommandType cmdtype)
        {
            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(connStr);
            SqlDataAdapter apt = new SqlDataAdapter(cmdText, conn);
            apt.SelectCommand.CommandType = cmdtype;

            if (parms != null)
            {
                //添加参数
                foreach (SqlParameter parm in parms)
                {
                    apt.SelectCommand.Parameters.Add(parm);
                }
            }

            try
            {
                apt.Fill(dt);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt;
        }


        //带连接字符串，带参，带SQL执行类型，返回DataTable的SQL执行
        public DataTable ExecuteDataTable(string constr, string cmdText, SqlParameter[] parms, CommandType cmdtype)
        {
            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(constr);
            SqlDataAdapter apt = new SqlDataAdapter(cmdText, conn);
            apt.SelectCommand.CommandType = cmdtype;

            if (parms != null)
            {
                //添加参数
                foreach (SqlParameter parm in parms)
                {
                    apt.SelectCommand.Parameters.Add(parm);
                }
            }

            try
            {
                apt.Fill(dt);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt;
        }


        //带连接，带参，带SQL执行类型，返回DataTable的SQL执行
        public DataTable ExecuteDataTable(SqlConnection conn, string cmdText, SqlParameter[] parms, CommandType cmdtype)
        {
            DataTable dt = new DataTable();

            SqlDataAdapter apt = new SqlDataAdapter(cmdText, conn);
            apt.SelectCommand.CommandType = cmdtype;

            if (parms != null)
            {
                //添加参数
                foreach (SqlParameter parm in parms)
                {
                    apt.SelectCommand.Parameters.Add(parm);
                }
            }

            try
            {
                apt.Fill(dt);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt;
        }


        //带SQL执行类型，返回DataTable的SQL执行
        public DataTable ExecuteDataTable(string cmdText, CommandType cmdtype)
        {
            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(connStr);
            SqlDataAdapter apt = new SqlDataAdapter(cmdText, conn);
            apt.SelectCommand.CommandType = cmdtype;
            try
            {
                apt.Fill(dt);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt;
        }


        //返回DataTable的SQL执行
        public DataTable ExecuteDataTable(string cmdText)
        {
            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(connStr);
            SqlDataAdapter apt = new SqlDataAdapter(cmdText, conn);
            apt.SelectCommand.CommandType = CommandType.Text;
            try
            {
                apt.Fill(dt);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt;
        }


        #endregion

    }
}
