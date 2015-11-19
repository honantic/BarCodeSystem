using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCodeSystem.PublicClass.HelperClass
{
    public class DBLog
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }

        /// <summary>
        /// 操作者姓名
        /// </summary>
        public string DBL_OperateBy { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public string DBL_OperateTime { get; set; }

        /// <summary>
        /// 操作的表名
        /// </summary>
        public string DBL_OperateTable { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public OperateType DBL_OperateType { get; set; }

        /// <summary>
        /// 操作表中相关的ID
        /// </summary>
        public string DBL_AssociateID { get; set; }

        /// <summary>
        /// 操作表中相关的Code
        /// </summary>
        public string DBL_AssociateCode { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string DBL_Content { get; set; }


        /// <summary>
        /// 将日志写入数据库中
        /// </summary>
        /// <param name="_dbLog">单个日志</param>
        public static void WriteDBLog(DBLog _dbLog)
        {
            DataSet ds = new DataSet();
            string SQl = string.Format("select top 0 * from [DBLog]");
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "DBLog");
            DataRow row = ds.Tables["DBLog"].NewRow();
            row["DBL_OperateBy"] = _dbLog.DBL_OperateBy;
            row["DBL_OperateTime"] = _dbLog.DBL_OperateTime;
            row["DBL_OperateTable"] = _dbLog.DBL_OperateTable;
            row["DBL_OperateType"] = _dbLog.DBL_OperateType;
            row["DBL_AssociateID"] = _dbLog.DBL_AssociateID;
            row["DBL_AssociateCode"] = _dbLog.DBL_AssociateCode;
            row["DBL_Content"] = _dbLog.DBL_Content;
            ds.Tables["DBLog"].Rows.Add(row);
            MyDBController.InsertSqlBulk(ds.Tables["DBLog"]);
            MyDBController.CloseConnection();
        }

        /// <summary>
        /// 将日志写入数据库中
        /// </summary>
        /// <param name="_dbLogList">日志列表</param>
        public static void WriteDBLog(List<DBLog> _dbLogList)
        {
            DataSet ds = new DataSet();
            string SQl = string.Format("select top 0 * from [DBLog]");
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "DBLog");
            foreach (DBLog _dbLog in _dbLogList)
            {
                DataRow row = ds.Tables["DBLog"].NewRow();
                row["DBL_OperateBy"] = _dbLog.DBL_OperateBy;
                row["DBL_OperateTime"] = _dbLog.DBL_OperateTime;
                row["DBL_OperateTable"] = _dbLog.DBL_OperateTable;
                row["DBL_OperateType"] = _dbLog.DBL_OperateType;
                row["DBL_AssociateID"] = _dbLog.DBL_AssociateID;
                row["DBL_AssociateCode"] = _dbLog.DBL_AssociateCode;
                row["DBL_Content"] = _dbLog.DBL_Content;
                ds.Tables["DBLog"].Rows.Add(row);
            }
            MyDBController.InsertSqlBulk(ds.Tables["DBLog"]);
            MyDBController.CloseConnection();
        }

        /// <summary>
        /// 登录日志
        /// </summary>
        public static void WriteLoginRecord()
        {
            DBLog _dbLog = new DBLog();
            _dbLog.DBL_OperateBy = User_Info.User_Code;
            _dbLog.DBL_OperateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            _dbLog.DBL_OperateType = OperateType.Login;
            _dbLog.DBL_Content = User_Info.User_Name + "|登录操作" + "|" + User_Info.User_WorkcenterName + "|" + User_Info.P_Position;
            DBLog.WriteDBLog(_dbLog);
        }

        /// <summary>
        /// 退出登录日志
        /// </summary>
        public static void WriteLogoutRecord()
        {
            DBLog _dbLog = new DBLog();
            _dbLog.DBL_OperateBy = User_Info.User_Code;
            _dbLog.DBL_OperateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            _dbLog.DBL_OperateType = OperateType.Logout;
            _dbLog.DBL_Content = User_Info.User_Name + "|退出登录操作" + "|" + User_Info.User_WorkcenterName + "|" + User_Info.P_Position;
            DBLog.WriteDBLog(_dbLog);
        }
    }
    /// <summary>
    /// 操作类型，增删查改
    /// </summary>
    public enum OperateType
    {
        /// <summary>
        /// 新增
        /// </summary>
        Insert = 0,
        /// <summary>
        /// 修改
        /// </summary>
        Update = 1,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 2,
        /// <summary>
        /// 查询
        /// </summary>
        Select = 3,
        /// <summary>
        /// 登录
        /// </summary>
        Login = 4,
        /// <summary>
        /// 登出
        /// </summary>
        Logout = 5,
        /// <summary>
        /// Excel导入
        /// </summary>
        Import = 6
    };
}
