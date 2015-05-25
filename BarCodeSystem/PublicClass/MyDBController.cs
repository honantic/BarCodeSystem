using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;
using System.Text;
using System.Diagnostics;
using System.Windows.Media;
/// <summary>
///MyDBController 的摘要说明
/// </summary>
public class MyDBController
{
	public MyDBController()
	{}
		
    
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

    public static void GetConnection()          //连接数据库
    {
        try
        {
            string M_str_connstr = "server=" + Server + ";database=" 
                + Database + ";uid="
                + Uid + ";pwd=" + Pwd;          //数据库连接字符串
            M_scn_myConn = new SqlConnection(M_str_connstr);
            M_scn_myConn.Open();
        }
        catch                                   //异常处理
        { 
        }
    }

    public static void CloseConnection()        //关闭数据库连接
    {
        if (M_scn_myConn.State==ConnectionState.Open)
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
        SqlCommand M_scm = new SqlCommand(commandstring,M_scn_myConn);
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
    ///  执行命令，返回结果集的第一行第一列
    /// </summary>
    /// <param name="commandstring">SQL命令字符串</param>
    /// <returns>结果集的第一行第一列</returns>
    public static object ExecuteScalar(string commandstring)
    {
        return CreateCommand(commandstring).ExecuteScalar();
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
    /// 根据SQL命令commandstring返回默认结果集DataSet,内部DataTable名称未指定
    /// </summary>
    /// <param name="commandstring">SQL命令字符串</param>
    /// <returns>默认结果集DataSet</returns>
    public static DataSet GetDataSet(string commandstring)
    {
        SqlDataAdapter M_sda = new SqlDataAdapter(commandstring,M_scn_myConn);
        try
        {
            M_sda.Fill(M_ds);
        }
        catch (Exception ee)
        {

            MessageBox.Show(ee.Message,"错误提示",MessageBoxButton.OK,MessageBoxImage.Error);
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
        SqlDataAdapter M_sda = new SqlDataAdapter(commandstring,M_scn_myConn);
        M_sda.Fill(M_ds,tablename);
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
        SqlDataAdapter M_sda = new SqlDataAdapter(commandstring,M_scn_myConn);
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
        M_sda.Fill(data_set,tablename);
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
        SqlCommand scm = new SqlCommand(sql,M_scn_myConn);
        SqlParameter imgsp = new SqlParameter("@imgBytes",SqlDbType.Image);//设置参数的值
        imgsp.Value = (byte[])bytes;
        scm.Parameters.Add(imgsp);
        return scm.ExecuteNonQuery();
    }
    

    
    /// <summary>
    /// 将更新的数据存入目标数据表。参数DataTable为数据表，其表名、列顺序等等一定要和数据库中要更新的目标表一致
    /// 如果目标表有主键，则在dt原有的列后面为每一列主键添加一个数据类型、数据一样的影射列，影射列名命名规则为
    /// "主键名"+"New"。参数List<string>为数据库目标表中要变动的列，包括insert 和update所涉及到的所有列
    /// </summary>
    /// <param name="dt">源数据表</param>
    /// <param name="colList">数据库目标表中要变动的列，包括insert 和update所涉及到的所有列</param>
    /// <returns></returns>
    public static void InsertSqlBulk(DataTable dt,List<string> colList ,out int updateNum,out int insertNum)
    {
        updateNum = insertNum = 0;
        try
        {
            using (SqlConnection conn = MyDBController.M_scn_myConn)
            {
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
                        if (colList[i]!=pk.Rows[j]["Name"].ToString())
                        {
                            filedsetstr += colList[i]+"= t."+colList[i];
                            colmunstr += colList[i];
                            colmunstrs += colList[i];
                        }
                    }

                    if (i > 0)
                    {
                        filedstr += ",";
                    }
                    filedstr += "s."+colList[i];
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
                    string newColName = pk.Rows[i]["name"].ToString()+"New";
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
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message,"提示",MessageBoxButton.OK,MessageBoxImage.Error);
        }         
    }


    /// <summary>
    /// 利用visualtreehelper寻找窗体中的控件
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
}