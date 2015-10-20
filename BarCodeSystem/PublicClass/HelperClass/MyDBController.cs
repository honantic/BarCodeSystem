using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Windows.Media;
using System.Reflection;
using Xceed.Wpf.AvalonDock;
using System.Threading.Tasks;
/// <summary>
///MyDBController 的摘要说明
/// </summary>
public class MyDBController
{
    public MyDBController()
    { }


    #region 模块级变量

    private static string server = ".";         //服务器
    public static string Server
    {
        get { return MyDBController.server; }
        set { MyDBController.server = value; }
    }

    private static string database = "master";  //数据库名称，如果不赋值，则默认为master
    public static string Database
    {
        get { return MyDBController.database; }
        set { MyDBController.database = value; }
    }

    private static string uid = "sa";           //登录名,如果不赋值，则默认为sa
    public static string Uid
    {
        get { return MyDBController.uid; }
        set { MyDBController.uid = value; }
    }

    private static string pwd = "";             //登录密码
    public static string Pwd
    {
        get { return MyDBController.pwd; }
        set { MyDBController.pwd = value; }
    }

    public static SqlConnection M_scn_myConn;   //数据库连接对象
    public static DataSet M_ds = new DataSet();                     //数据集对象

    #endregion

    /// <summary>
    /// 连接数据库
    /// </summary>
    public static void GetConnection()          //连接数据库
    {
        try
        {
            string M_str_connstr = "server=" + Server + ";database="
                + Database + ";uid="
                + Uid + ";pwd=" + Pwd;          //数据库连接字符串
            M_scn_myConn = new SqlConnection(M_str_connstr);
            if (M_scn_myConn.State == ConnectionState.Closed)
            {
                M_scn_myConn.Open();
            }
            while (M_scn_myConn.State == ConnectionState.Connecting)
            {

            }
        }
        catch (Exception ee)                                  //异常处理
        {
            Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message + "\r\n服务器" + Server + "\r\n数据库" + Database + "\r\n连接失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// 关闭数据库连接
    /// </summary>
    public static void CloseConnection()        //关闭数据库连接
    {
        if (M_scn_myConn.State == ConnectionState.Open)
        {
            M_scn_myConn.Close();
            M_scn_myConn.Dispose();
        }
    }


    /// <summary>
    /// 根据命令字符串生成SQL 命令
    /// </summary>
    /// <param name="commandstring">SQL命令字符串</param>
    /// <returns>SQL命令</returns>
    public static SqlCommand CreateCommand(string commandstring)
    {
        SqlCommand M_scm = new SqlCommand(commandstring, M_scn_myConn) { CommandTimeout = 180 };
        return M_scm;
    }

    /// <summary>
    /// 根据指定的sql连接生成sqlcommand
    /// </summary>
    /// <param name="_sqlcon"></param>
    /// <param name="commandstring"></param>
    /// <returns></returns>
    public static SqlCommand CreateCommand(SqlConnection _sqlcon, string commandstring)
    {
        if (_sqlcon.State == ConnectionState.Closed)
        {
            _sqlcon.Open();
        }
        SqlCommand M_scm = new SqlCommand(commandstring, _sqlcon) { CommandTimeout = 180 };
        return M_scm;
    }

    /// <summary>
    /// 执行命令，返回受影响的行数
    /// </summary>
    /// <param name="commandstring">SQL命令字符串</param>
    /// <returns>受影响行数</returns>
    public static int ExecuteNonQuery(string commandstring)
    {
        return CreateCommand(commandstring).ExecuteNonQuery();
    }

    /// <summary>
    /// 执行命令，返回手影响行数，自动打开连接
    /// </summary>
    /// <param name="commandstring"></param>
    /// <param name="_auto"></param>
    /// <returns></returns>
    public static int ExecuteNonQuery(string commandstring, bool _auto = true)
    {
        GetConnection();
        int count = CreateCommand(commandstring).ExecuteNonQuery();
        CloseConnection();
        return count;
    }


    /// <summary>
    /// 用指定的sql连接执行指定的命令，返回受影响的行数
    /// </summary>
    /// <param name="_sqlcon"></param>
    /// <param name="commandstring"></param>
    /// <returns></returns>
    public static int ExecuteNonQuery(SqlConnection _sqlcon, string commandstring)
    {
        using (_sqlcon)
        {
            return CreateCommand(_sqlcon, commandstring).ExecuteNonQuery();
        }
    }

    /// <summary>
    ///  执行命令，返回结果集的第一行第一列
    /// </summary>
    /// <param name="commandstring">SQL命令字符串</param>
    /// <returns>结果集的第一行第一列</returns>
    public static object ExecuteScalar(string commandstring)
    {
        return CreateCommand(commandstring).ExecuteScalar();
    }

    /// <summary>
    /// 使用指定的sql连接执行命令，返回结果集的第一行第一列
    /// </summary>
    /// <param name="_sqlcon"></param>
    /// <param name="commandstring"></param>
    /// <returns></returns>
    public static object ExecuteScalar(SqlConnection _sqlcon, string commandstring)
    {
        using (_sqlcon)
        {
            return CreateCommand(_sqlcon, commandstring).ExecuteScalar();
        }
    }

    /// <summary>
    /// 返回datareader
    /// </summary>
    /// <param name="commandstring">SQL命令字符串</param>
    /// <returns>datareader</returns>
    public static SqlDataReader GetDataReader(string commandstring)
    {
        return CreateCommand(commandstring).ExecuteReader();
    }

    /// <summary>
    /// 使用指定的sql连接，返回datareader
    /// </summary>
    /// <param name="_sqlcon"></param>
    /// <param name="commandstring"></param>
    /// <returns></returns>
    public static SqlDataReader GetDataReader(SqlConnection _sqlcon, string commandstring)
    {
        using (_sqlcon)
        {
            return CreateCommand(_sqlcon, commandstring).ExecuteReader();
        }
    }

    /// <summary>
    /// 根据SQL命令commandstring返回默认结果集DataSet,内部DataTable名称未指定
    /// </summary>
    /// <param name="commandstring">SQL命令字符串</param>
    /// <returns>默认结果集DataSet</returns>
    public static DataSet GetDataSet(string commandstring)
    {
        SqlDataAdapter M_sda = new SqlDataAdapter(commandstring, M_scn_myConn);
        try
        {
            M_sda.Fill(M_ds);
        }
        catch (Exception ee)
        {

            MessageBox.Show(ee.Message, "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        return M_ds;
    }


    /// <summary>
    /// 根据SQL命令commandstring返回默认结果集DataSet,内部DataTable名称为tablename
    /// </summary>
    /// <param name="commandstring">SQL命令字符串</param>
    /// <param name="tablename">数据表名称</param>
    /// <returns>默认结果集DataSet</returns>
    public static DataSet GetDataSet(string commandstring, string tablename)
    {
        SqlDataAdapter M_sda = new SqlDataAdapter(commandstring, M_scn_myConn);
        M_sda.Fill(M_ds, tablename);
        return M_ds;
    }

    /// <summary>
    /// 根据SQL命令commandstring返回自定义结果集DataSet,内部DataTable名称未指定
    /// </summary>
    /// <param name="commandstring">SQL命令字符串</param>
    /// <param name="data_set">自定义结果集名称</param>
    /// <returns>自己定义结果集DataSet</returns>
    public static DataSet GetDataSet(string commandstring, DataSet data_set)
    {
        SqlDataAdapter M_sda = new SqlDataAdapter(commandstring, M_scn_myConn);
        M_sda.Fill(data_set);
        return data_set;
    }


    /// <summary>
    /// 根据SQL命令commandstring返回自定义结果集DataSet,内部DataTable名称为tablename
    /// </summary>
    /// <param name="commandstring">SQL命令字符串</param>
    /// <param name="data_set">自定义结果集名称</param>
    /// <param name="tablename">数据表名称</param>
    /// <returns>默认结果集DataSet</returns>
    public static DataSet GetDataSet(string commandstring, DataSet data_set, string tablename)
    {
        SqlDataAdapter M_sda = new SqlDataAdapter(commandstring, M_scn_myConn);
        M_sda.Fill(data_set, tablename);
        return data_set;
    }

    /// <summary>
    /// 用指定的sql连接执行指定的sql命令
    /// </summary>
    /// <param name="_sqlcon"></param>
    /// <param name="commandstring"></param>
    /// <param name="data_set"></param>
    /// <param name="tablename"></param>
    /// <returns></returns>
    public static DataSet GetDataSet(SqlConnection _sqlcon, string commandstring, DataSet data_set, string tablename)
    {
        using (_sqlcon)
        {
            if (_sqlcon.State == ConnectionState.Closed)
            {
                _sqlcon.Open();
            }
            SqlDataAdapter M_sda = new SqlDataAdapter() { };
            M_sda.SelectCommand = CreateCommand(_sqlcon, commandstring);
            M_sda.Fill(data_set, tablename);
            if (_sqlcon.State == ConnectionState.Open)
            {
                _sqlcon.Close();
            }
        }
        return data_set;
    }

    /// <summary>
    /// 执行带图片插入操作的SQL语句
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="bytes">图片转后的数组</param>
    /// <returns>受影响的行数</returns>
    /// 存图像。参数1：SQL语句；参数2：图像转换的数组。
    public static int SaveImage(string sql, object bytes)
    {
        SqlCommand scm = new SqlCommand(sql, M_scn_myConn);
        SqlParameter imgsp = new SqlParameter("@imgBytes", SqlDbType.Image);//设置参数的值
        imgsp.Value = (byte[])bytes;
        scm.Parameters.Add(imgsp);
        return scm.ExecuteNonQuery();
    }



    /// <summary>
    /// 将更新的数据存入目标数据表。参数DataTable为数据表，其表名、列顺序等等一定要和数据库中要更新的目标表一致
    /// 如果目标表有主键，则在dt原有的列后面为每一列主键添加一个数据类型、数据一样的影射列，影射列名命名规则为
    /// "主键名"+"New"。参数List string为数据库目标表中要变动的列，包括insert 和update所涉及到的所有列
    /// </summary>
    /// <param name="dt">源数据表</param>
    /// <param name="colList">数据库目标表中要变动的列，包括insert 和update所涉及到的所有列</param>
    /// <returns></returns>
    public static bool InsertSqlBulk(DataTable dt, List<string> colList, out int updateNum, out int insertNum)
    {
        bool flag = true;
        updateNum = insertNum = 0;
        try
        {
            SqlConnection conn = MyDBController.M_scn_myConn;

            #region
            SqlCommand cmd = conn.CreateCommand();
            StringBuilder sb = new StringBuilder();
            DataTable pk = new DataTable();
            string tempTableName = "#bulktable";//#表名 为当前连接有效的临时表 ##表名 为全局有效的临时表
            string filedstr = "";//临时表获取表结构命令字符串
            string filedsetstr = "";//update set 命令字符串
            string pkfiledwherestr = "";//insert 命令用来排除已经存在记录的 not exist 命令中where条件字符串
            string colmunstr = "";//列名字符串
            string colmunstrs = "";//t.+列名 字符串

            sb = new StringBuilder();
            sb.AppendFormat(@"select a.name as Name,b.name as Type from syscolumns a
                                  left join systypes b 
                                  on a.xtype = b.xtype 
                                    where colid in 
                                        (select colid from sysindexkeys 
                                            where id = object_id('{0}') 
                                            and indid = 
                                                (select indid from sysindexes 
                                                    where name = (select name from sysobjects 
                                                        where xtype='pk' 
                                                        and parent_obj = object_id('{0}')
                                                                  )
                                                 )
                                         ) and a.id = object_id('{0}');", dt.TableName);
            cmd.CommandText = sb.ToString();
            pk.Load(cmd.ExecuteReader());//查询主键列表
            #endregion

            #region
            /* 利用传递进来的DataTable列名列表，从数据库的源表获取
                 * 临时表的表结构*/
            for (int i = 0; i < colList.Count; i++)
            {
                if (filedsetstr.Length > 0)
                {
                    filedsetstr += ",";
                    colmunstr += ",";
                    colmunstrs += ",";
                }
                for (int j = 0; j < pk.Rows.Count; j++)
                {
                    /* 如果当前列是主键,set命令字符串跳过不作处理,
                     * 临时表获取表结构命令字符串不论何种情况都不跳过 */
                    if (colList[i] != pk.Rows[j]["Name"].ToString())
                    {
                        filedsetstr += colList[i] + "= t." + colList[i];
                        colmunstr += colList[i];
                        colmunstrs += colList[i];
                    }
                }

                if (i > 0)
                {
                    filedstr += ",";
                }
                filedstr += "s." + colList[i];
            }
            #endregion

            #region
            sb = new StringBuilder();
            sb.AppendFormat("select top 0 {0} into {1} from {2} s;", filedstr, tempTableName, dt.TableName);
            cmd.CommandText = sb.ToString();
            cmd.ExecuteNonQuery();//创建临时表

            /* 根据获得的目标表主键，来为SQL Server 系统中的临时表增加相应的非主键但是数据相等
             * 的 影射列，因为有些系统的主键为自增类型，在调用bulk.WriteToServer方法的时候，自增主键会
             * 在临时表中从0开始计算，没办法用临时表的主键和目标表的主键做 where 条件，故用影射列代替*/
            for (int i = 0; i < pk.Rows.Count; i++)
            {
                if (i > 0)
                {
                    pkfiledwherestr += " and ";
                }
                string newColName = pk.Rows[i]["name"].ToString() + "New";
                sb = new StringBuilder();
                sb.AppendFormat("alter table {0} add {1} {2}",
                                tempTableName, newColName, pk.Rows[i]["Type"].ToString());
                cmd.CommandText = sb.ToString();
                cmd.ExecuteNonQuery();
                pkfiledwherestr += "t." + newColName + "=s." + pk.Rows[i]["name"];
            }

            using (System.Data.SqlClient.SqlBulkCopy bulk = new System.Data.SqlClient.SqlBulkCopy(conn))
            {

                bulk.DestinationTableName = tempTableName;
                bulk.BulkCopyTimeout = 36000;
                try
                {
                    bulk.WriteToServer(dt);//将数据写入临时表
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            #endregion

            #region
            if (pkfiledwherestr.Equals(""))//如果不存在主键
            {
                sb = new StringBuilder();
                sb.AppendFormat("insert into {0} select {1} from {2} s;", dt.TableName, filedstr, tempTableName);
                cmd.CommandText = sb.ToString();
                insertNum = cmd.ExecuteNonQuery();//插入临时表数据到目的表
            }
            else
            {
                sb = new StringBuilder();
                sb.AppendFormat("update {0} set {1} from {0} s INNER JOIN {2} t on {3}",
                                dt.TableName, filedsetstr, tempTableName, pkfiledwherestr);
                cmd.CommandText = sb.ToString();
                Stopwatch sw = new Stopwatch();
                sw.Start();
                updateNum = cmd.ExecuteNonQuery();//更新已存在主键数据
                sw.Stop();
                sb = new StringBuilder();
                sb.AppendFormat("insert into {0}({4}) select {1} from {2} t where not EXISTS(select 1 from {0} s where {3});",
                                 dt.TableName, colmunstrs, tempTableName, pkfiledwherestr, colmunstr);
                cmd.CommandText = sb.ToString();
                insertNum = cmd.ExecuteNonQuery();//插入新数据
                //MessageBox.Show("共用时"+sw.Elapsed+"\n 共新增:"+insertNum+"条记录,更新:"+updateNum+"条记录！");
            }
            #endregion

            return flag;
        }
        catch (Exception e)
        {
            flag = false;
            MessageBox.Show(e.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            return flag;
        }
    }

    /// <summary>
    /// 用指定的sql连接来批量更新/插入数据
    /// </summary>
    /// <param name="_sqlcon"></param>
    /// <param name="dt">源数据表</param>
    /// <param name="colList">数据库目标表中要变动的列，包括insert 和update所涉及到的所有列</param>
    /// <param name="updateNum"></param>
    /// <param name="insertNum"></param>
    public static bool InsertSqlBulk(SqlConnection _sqlcon, DataTable dt, List<string> colList, out int updateNum, out int insertNum)
    {
        bool flag = true;
        updateNum = insertNum = 0;
        try
        {
            SqlConnection conn = _sqlcon;
            #region
            if (_sqlcon.State == ConnectionState.Closed)
            {
                _sqlcon.Open();
            }
            SqlCommand cmd = conn.CreateCommand();
            StringBuilder sb = new StringBuilder();
            DataTable pk = new DataTable();
            string tempTableName = "#bulktable";//#表名 为当前连接有效的临时表 ##表名 为全局有效的临时表
            string filedstr = "";//临时表获取表结构命令字符串
            string filedsetstr = "";//update set 命令字符串
            string pkfiledwherestr = "";//insert 命令用来排除已经存在记录的 not exist 命令中where条件字符串
            string colmunstr = "";//列名字符串
            string colmunstrs = "";//t.+列名 字符串

            sb = new StringBuilder();
            sb.AppendFormat(@"select a.name as Name,b.name as Type from syscolumns a
                                  left join systypes b 
                                  on a.xtype = b.xtype 
                                    where colid in 
                                        (select colid from sysindexkeys 
                                            where id = object_id('{0}') 
                                            and indid = 
                                                (select indid from sysindexes 
                                                    where name = (select name from sysobjects 
                                                        where xtype='pk' 
                                                        and parent_obj = object_id('{0}')
                                                                  )
                                                 )
                                         ) and a.id = object_id('{0}');", dt.TableName);
            cmd.CommandText = sb.ToString();
            pk.Load(cmd.ExecuteReader());//查询主键列表
            #endregion

            #region
            /* 利用传递进来的DataTable列名列表，从数据库的源表获取
                 * 临时表的表结构*/
            for (int i = 0; i < colList.Count; i++)
            {
                if (filedsetstr.Length > 0)
                {
                    filedsetstr += ",";
                    colmunstr += ",";
                    colmunstrs += ",";
                }
                for (int j = 0; j < pk.Rows.Count; j++)
                {
                    /* 如果当前列是主键,set命令字符串跳过不作处理,
                     * 临时表获取表结构命令字符串不论何种情况都不跳过 */
                    if (colList[i] != pk.Rows[j]["Name"].ToString())
                    {
                        filedsetstr += colList[i] + "= t." + colList[i];
                        colmunstr += colList[i];
                        colmunstrs += colList[i];
                    }
                }

                if (i > 0)
                {
                    filedstr += ",";
                }
                filedstr += "s." + colList[i];
            }
            #endregion

            #region
            sb = new StringBuilder();
            sb.AppendFormat("select top 0 {0} into {1} from {2} s;", filedstr, tempTableName, dt.TableName);
            cmd.CommandText = sb.ToString();
            cmd.ExecuteNonQuery();//创建临时表

            /* 根据获得的目标表主键，来为SQL Server 系统中的临时表增加相应的非主键但是数据相等
             * 的 影射列，因为有些系统的主键为自增类型，在调用bulk.WriteToServer方法的时候，自增主键会
             * 在临时表中从0开始计算，没办法用临时表的主键和目标表的主键做 where 条件，故用影射列代替*/
            for (int i = 0; i < pk.Rows.Count; i++)
            {
                if (i > 0)
                {
                    pkfiledwherestr += " and ";
                }
                string newColName = pk.Rows[i]["name"].ToString() + "New";
                sb = new StringBuilder();
                sb.AppendFormat("alter table {0} add {1} {2}",
                                tempTableName, newColName, pk.Rows[i]["Type"].ToString());
                cmd.CommandText = sb.ToString();
                cmd.ExecuteNonQuery();
                pkfiledwherestr += "t." + newColName + "=s." + pk.Rows[i]["name"];
            }

            using (System.Data.SqlClient.SqlBulkCopy bulk = new System.Data.SqlClient.SqlBulkCopy(conn))
            {

                bulk.DestinationTableName = tempTableName;
                bulk.BulkCopyTimeout = 36000;
                try
                {
                    bulk.WriteToServer(dt);//将数据写入临时表
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            #endregion

            #region
            if (pkfiledwherestr.Equals(""))//如果不存在主键
            {
                sb = new StringBuilder();
                sb.AppendFormat("insert into {0} select {1} from {2} s;", dt.TableName, filedstr, tempTableName);
                cmd.CommandText = sb.ToString();
                insertNum = cmd.ExecuteNonQuery();//插入临时表数据到目的表
            }
            else
            {
                sb = new StringBuilder();
                sb.AppendFormat("update {0} set {1} from {0} s INNER JOIN {2} t on {3}",
                                dt.TableName, filedsetstr, tempTableName, pkfiledwherestr);
                cmd.CommandText = sb.ToString();
                Stopwatch sw = new Stopwatch();
                sw.Start();
                updateNum = cmd.ExecuteNonQuery();//更新已存在主键数据
                sw.Stop();
                sb = new StringBuilder();
                sb.AppendFormat("insert into {0}({4}) select {1} from {2} t where not EXISTS(select 1 from {0} s where {3});",
                                 dt.TableName, colmunstrs, tempTableName, pkfiledwherestr, colmunstr);
                cmd.CommandText = sb.ToString();
                insertNum = cmd.ExecuteNonQuery();//插入新数据
                //MessageBox.Show("共用时"+sw.Elapsed+"\n 共新增:"+insertNum+"条记录,更新:"+updateNum+"条记录！");
            }
            #endregion
        }
        catch (Exception e)
        {
            flag = false;
            MessageBox.Show(e.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            if (_sqlcon.State == ConnectionState.Open)
            {
                _sqlcon.Close();
            }
        }
        return flag;
    }


    /// <summary>
    /// 快捷更新数据库，自动根据ID区分update和insert。海量数据秒级更新
    /// </summary>
    /// <typeparam name="T">数据源类型</typeparam>
    /// <param name="dt">有指定表结构的空表</param>
    /// <param name="_tList">数据源list</param>
    /// <param name="updateNum">更新的行数</param>
    /// <param name="insertNum">插入的行数</param>
    public static bool AutoUpdateInsert<T>(DataTable dt, List<T> _tList, out int updateNum, out int insertNum)
    {
        updateNum = insertNum = 0;

        PropertyInfo[] props = null;
        List<string> colList = new List<string>();
        if (_tList.Count > 0)
        {
            props = _tList[0].GetType().GetProperties();
        }


        foreach (DataColumn col in dt.Columns)
        {
            colList.Add(col.ColumnName);
        }
        dt.Columns.Add(new DataColumn("IDNew", typeof(Int64)));

        foreach (T item in _tList)
        {
            DataRow row = dt.NewRow();
            foreach (PropertyInfo prop in props)
            {
                if (colList.Contains(prop.Name))
                {
                    string name = prop.Name;
                    row[name] = prop.GetValue(item, null);
                }
            }
            dt.Rows.Add(row);
        }

        foreach (DataRow row in dt.Rows)
        {
            row["IDNew"] = row["ID"];
        }

        return InsertSqlBulk(dt, colList, out updateNum, out insertNum);
    }

    /// <summary>
    /// 使用默认数据库连接更新数据库，参数dt的tablename属性必须与数据库中目标表名一致
    /// </summary>
    /// <param name="_dt">数据表</param>
    public static void InsertSqlBulk(DataTable _dt)
    {
        List<string> colList = new List<string>();
        foreach (DataColumn col in _dt.Columns)
        {
            colList.Add(col.ColumnName);
        }
        _dt.Columns.Add(new DataColumn("IDNew", typeof(Int64)));
        foreach (DataRow row in _dt.Rows)
        {
            row["IDNew"] = row["ID"];
        }
        int updateNum, insertNum;
        InsertSqlBulk(_dt, colList, out updateNum, out insertNum);
    }

    /// <summary>
    /// 使用指定的数据库连接更新数据库，参数dt的tablename属性必须与数据库中目标表名一致
    /// </summary>
    /// <param name="_sqlcon"></param>
    /// <param name="_dt"></param>
    public static void InsertSqlBulk(SqlConnection _sqlcon, DataTable _dt)
    {
        List<string> colList = new List<string>();
        foreach (DataColumn col in _dt.Columns)
        {
            colList.Add(col.ColumnName);
        }
        _dt.Columns.Add(new DataColumn("IDNew", typeof(Int64)));
        foreach (DataRow row in _dt.Rows)
        {
            row["IDNew"] = row["ID"];
        }
        int updateNum, insertNum;
        InsertSqlBulk(_sqlcon, _dt, colList, out updateNum, out insertNum);
    }

    /// <summary>
    /// 用list和带指定结构的dt来更新数据库
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    /// <param name="_list">数据列表</param>
    /// <param name="_dt">跟数据库结构吻合的dt</param>
    public static void InsertSqlBulk<T>(List<T> _list, DataTable _dt)
    {
        _dt = ListToDataTable(_list, _dt);
        InsertSqlBulk(_dt);
    }

    /// <summary>
    /// 将list转换成指定结构的dt，用来进行数据库更新的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_list"></param>
    /// <param name="_dt"></param>
    /// <returns></returns>
    public static DataTable ListToDataTable<T>(List<T> _list, DataTable _dt)
    {
        PropertyInfo[] props = null;
        List<string> colList = new List<string>();
        if (_list.Count > 0)
        {
            props = _list[0].GetType().GetProperties();
        }


        foreach (DataColumn col in _dt.Columns)
        {
            colList.Add(col.ColumnName);
        }

        foreach (T item in _list)
        {
            DataRow row = _dt.NewRow();
            foreach (PropertyInfo prop in props)
            {
                if (colList.Contains(prop.Name))
                {
                    string name = prop.Name;
                    row[name] = prop.GetValue(item, null);
                }
            }
            _dt.Rows.Add(row);
        }
        return _dt;
    }

    /// <summary>
    /// 将list转换成指定结构的dt,用来excel导出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_list"></param>
    /// <returns></returns>
    public static DataTable ListToDataTable<T>(List<T> _list)
    {
        DataTable _dt = new DataTable();
        PropertyInfo[] props = null;
        List<string> colList = new List<string>();
        if (_list.Count > 0)
        {
            props = _list[0].GetType().GetProperties();
        }

        foreach (PropertyInfo prop in props)
        {
            _dt.Columns.Add(prop.Name, prop.PropertyType);
        }

        foreach (DataColumn col in _dt.Columns)
        {
            colList.Add(col.ColumnName);
        }

        foreach (T item in _list)
        {
            DataRow row = _dt.NewRow();
            foreach (PropertyInfo prop in props)
            {
                if (colList.Contains(prop.Name))
                {
                    string name = prop.Name;
                    row[name] = prop.GetValue(item, null);
                }
            }
            _dt.Rows.Add(row);
        }
        return _dt;
    }

    /// <summary>
    /// list的复制工作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_list"></param>
    /// <param name="_destList"></param>
    /// <returns></returns>
    public static List<T> ListCopy<T>(List<T> _list) where T : new()
    {
        List<T> _destList = new List<T>();
        PropertyInfo[] props = null;
        if (_list.Count > 0)
        {
            props = _list[0].GetType().GetProperties();
        }

        _list.ForEach(
            p =>
            {
                T item = new T();
                props.ToList().ForEach(
                    pi =>
                    {
                        if (!pi.PropertyType.IsGenericType)
                        {
                            if (pi.SetMethod != null && pi.GetValue(p, null) != null)
                            {
                                pi.SetValue(item, Convert.ChangeType(pi.GetValue(p, null), pi.PropertyType), null);
                            }
                        }
                        else
                        {
                            Type genericTypeDefinition = pi.PropertyType.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Nullable<>))
                            {
                                if (pi.GetValue(p, null) != null)
                                {
                                    pi.SetValue(item, Convert.ChangeType(pi.GetValue(p, null), Nullable.GetUnderlyingType(pi.PropertyType)), null);
                                }
                            }
                        }
                    });
                _destList.Add(item);
            });
        return _destList;
    }

    /// <summary>
    /// 引用类型的值赋值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_t"></param>
    /// <returns></returns>
    public static T ValueCopy<T>(T _t) where T : new()
    {
        T item = new T();
        PropertyInfo[] props = _t.GetType().GetProperties();

        props.ToList().ForEach(
                    pi =>
                    {
                        if (!pi.PropertyType.IsGenericType)
                        {
                            if (pi.GetValue(_t, null) != null)
                            {
                                pi.SetValue(item, Convert.ChangeType(pi.GetValue(_t, null), pi.PropertyType), null);
                            }
                        }
                        else
                        {
                            Type genericTypeDefinition = pi.PropertyType.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Nullable<>))
                            {
                                if (pi.GetValue(_t, null) != null)
                                {
                                    pi.SetValue(item, Convert.ChangeType(pi.GetValue(_t, null), Nullable.GetUnderlyingType(pi.PropertyType)), null);
                                }
                            }
                        }
                    });
        return item;
    }

    /// <summary>
    /// 利用visualtreehelper寻找窗体中的子控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static List<T> FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
    {
        try
        {
            List<T> TList = new List<T> { };
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                {
                    TList.Add((T)child);
                    List<T> childOfChildren = FindVisualChild<T>(child);
                    if (childOfChildren != null)
                    {
                        TList.AddRange(childOfChildren);
                    }
                }
                else
                {
                    List<T> childOfChildren = FindVisualChild<T>(child);
                    if (childOfChildren != null)
                    {
                        TList.AddRange(childOfChildren);
                    }
                }
            }
            return TList;
        }
        catch (Exception ee)
        {
            MessageBox.Show(ee.Message);
            return null;
        }
    }


    /// <summary>
    /// 利用VisualTreeHelper寻找指定依赖对象的父级对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static List<T> FindVisualParent<T>(DependencyObject obj) where T : DependencyObject
    {
        try
        {
            List<T> TList = new List<T> { };
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            if (parent != null && parent is T)
            {
                TList.Add((T)parent);
                List<T> parentOfParent = FindVisualParent<T>(parent);
                if (parentOfParent != null)
                {
                    TList.AddRange(parentOfParent);
                }
            }
            else if (parent != null)
            {
                List<T> parentOfParent = FindVisualParent<T>(parent);
                if (parentOfParent != null)
                {
                    TList.AddRange(parentOfParent);
                }
            }
            return TList;
        }
        catch (Exception ee)
        {
            MessageBox.Show(ee.Message);
            return null;
        }
    }

    /// <summary>
    /// 利用logictreehelper寻找子对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static List<T> FindLogicChild<T>(DependencyObject obj) where T : DependencyObject
    {
        try
        {
            List<T> childList = new List<T>();
            foreach (DependencyObject item in LogicalTreeHelper.GetChildren(obj))
            {
                if (item is T)
                {
                    childList.Add((T)item);
                    List<T> childOfChild = FindLogicChild<T>(item);
                    if (childOfChild != null)
                    {
                        childList.AddRange(childOfChild);
                    }
                }
                else
                {
                    List<T> childOfChild = FindLogicChild<T>(item);
                    if (childOfChild != null)
                    {
                        childList.AddRange(childOfChild);
                    }
                }
            }
            return childList;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// 利用VisualTreeHelper寻找指定依赖对象的父级对象，返回名称
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    static List<string> FindVisualParentName(DependencyObject obj)
    {
        try
        {
            List<string> strList = new List<string> { };
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            if (parent != null)
            {
                strList.Add(parent.GetType().ToString());
                if (parent is DockingManager)
                {
                    strList.Add(((DockingManager)parent).Name);
                }
                List<string> parentOfParent = FindVisualParentName(parent);
                if (parentOfParent != null)
                {
                    strList.AddRange(parentOfParent);
                }
            }

            return strList;
        }
        catch (Exception ee)
        {
            MessageBox.Show(ee.Message);
            return null;
        }
    }


    /// <summary>
    /// 处理时间信息，保证系统的时间字符串能够成为被sql server 数据库识别的格式，目前只识别中文系统语言 不需要用到了
    /// </summary>
    /// <param name="timestring"></param>
    /// <returns></returns>
    public static string HandleTimeInfo(string timestring)
    {
        int formatType = -1;//0代表只包含星期信息，1代表包含只包含上下午信息，2代表两个信息都包含
        int index = -1;
        if (timestring.IndexOf("星期") != -1 || timestring.IndexOf("午") != -1)
        {
            if (timestring.IndexOf("星期") != -1 && timestring.IndexOf("午") == -1)
            {
                formatType = 0;
            }
            else if (timestring.IndexOf("星期") == -1 && timestring.IndexOf("午") != -1)
            {
                formatType = 1;
            }
            else if (timestring.IndexOf("星期") != -1 && timestring.IndexOf("午") != -1)
            {
                formatType = 2;
            }
        }
        switch (formatType)
        {
            case -1:
                return timestring;
            case 0:
                index = timestring.IndexOf("星");
                timestring = timestring.Remove(index, "星".Length * 3);
                return timestring;
            case 1:
                index = timestring.IndexOf("午");
                if (timestring.Contains("下"))
                {
                    string time = timestring.Substring(index + "午".Length);
                    string hour = time.Split(':')[0].ToString().Trim();
                    if (Convert.ToInt32(hour) == 12)
                    {
                    }
                    else
                        timestring = timestring.Replace(hour, (Convert.ToInt32(hour) + 12).ToString());

                }
                timestring = timestring.Remove(index - "午".Length, "午".Length * 2);
                return timestring;
            case 2:
            default:
                return timestring;
        }
    }

    struct Person
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    /// <summary>
    /// 调用样例
    /// </summary>
    public void DemoFunction()
    {
        #region 以List<T>为数据源调用
        List<Person> personList = new List<Person>() { 
        new Person(){Name="李四",Code="lisi"},new Person(){Name ="张三",Code="zhangsan"}};

        string SQl = "select top 0 * from Person";//获得目标表结构，数据库中的目标表需要以ID为主键
        DataSet ds = new DataSet();
        SqlConnection sqlcon = new SqlConnection() { ConnectionString = "写自己的连接串" };
        MyDBController.GetDataSet(new SqlConnection() { ConnectionString = sqlcon.ConnectionString }, SQl, ds, "Person");

        int updateNum, insertNum;
        MyDBController.AutoUpdateInsert<Person>(ds.Tables["Person"], personList, out updateNum, out insertNum);
        #endregion

        #region 以DataTable为数据源调用
        /*假设下面这个DataTable已经有数据了
         *现在需要满足以下条件：
         *该DataTable所有列必须和数据库中目标表结构一样，包括列名和排列顺序*/
        DataTable aimTable = new DataTable();

        //第一步:获取列名List
        List<string> colList = new List<string>();
        foreach (DataColumn col in aimTable.Columns)
        {
            colList.Add(col.ColumnName);
        }
        //第二步:在aimTable里增加ID映射列，并且赋值
        aimTable.Columns.Add(new DataColumn("IDNew"));
        foreach (DataRow row in aimTable.Rows)
        {
            row["IDNew"] = row["ID"];
        }
        //第三步:调用
        MyDBController.InsertSqlBulk(new SqlConnection() { ConnectionString = sqlcon.ConnectionString }, aimTable, colList, out updateNum, out insertNum);
        #endregion
    }
}