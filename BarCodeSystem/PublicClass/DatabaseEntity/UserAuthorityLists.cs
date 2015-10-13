using BarCodeSystem.PublicClass.HelperClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BarCodeSystem
{
    public class UserAuthorityLists
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }

        /// <summary>
        /// 账号ID
        /// </summary>
        public Int64 UA_UserAccountID
        {
            get;
            set;
        }

        /// <summary>
        /// 账号
        /// </summary>
        public string UA_LoginAccount
        {
            get;
            set;
        }


        /// <summary>
        /// 权限ID
        /// </summary>
        public Int64 UA_SysAuthorityID
        {
            get;
            set;
        }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string SA_AuthorityName
        {
            get;
            set;
        }

        /// <summary>
        /// 根据账号id获取权限列表
        /// </summary>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public static List<UserAuthorityLists> FetchUAListByUserID(Int64 _userID)
        {
            List<UserAuthorityLists> AuthorityList = new List<UserAuthorityLists>();
            DataSet ds = new DataSet();
            string SQl = string.Format(@"select A.[ID],A.[UA_UserAccountID],A.[UA_SysAuthorityID],B.[SA_AuthorityName],C.[UA_LoginAccount],C.[UA_UserName],C.[ID] from  [UserAuthority] A left join [SystemAuthority] B on A.[UA_SysAuthorityID]=B.[ID] left join [UserAccount] C on A.[UA_UserAccountID]=C.id where A.[UA_UserAccountID] = {0}", _userID);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "UA_AuthorityName");
            MyDBController.CloseConnection();

            int x = ds.Tables["UA_AuthorityName"].Rows.Count;


            foreach (DataRow row in ds.Tables["UA_AuthorityName"].Rows)
            {
                UserAuthorityLists ual = new UserAuthorityLists();
                ual.ID = Convert.ToInt64(row["ID"]);
                ual.UA_LoginAccount = row["UA_LoginAccount"].ToString();
                ual.SA_AuthorityName = row["SA_AuthorityName"].ToString();
                ual.UA_SysAuthorityID = Convert.ToInt64(row["UA_SysAuthorityID"]);
                ual.UA_UserAccountID = Convert.ToInt64(row["UA_UserAccountID"]);
                AuthorityList.Add(ual);
            }
            DBLog _dbLog = new DBLog();
            _dbLog.DBL_Content = User_Info.User_Name + "|在权限管理界面，查询条码系统账号权限列表，条码系统账号id为：" + _userID.ToString();
            _dbLog.DBL_OperateBy = User_Info.User_Code;
            _dbLog.DBL_OperateTime = DateTime.Now.ToString();
            _dbLog.DBL_OperateType = OperateType.Select;
            _dbLog.DBL_OperateTable = "UserAccount";
            DBLog.WriteDBLog(_dbLog);
            return AuthorityList;
        }

        /// <summary>
        /// 根据登陆账号名获取权限，一般是员工编号
        /// </summary>
        /// <param name="account">登陆账号</param>
        /// <returns></returns>
        public static List<UserAuthorityLists> FetchUAListByAccount(string account)
        {
            List<UserAuthorityLists> AuthorityList = new List<UserAuthorityLists>();
            DataSet ds = new DataSet();
            string SQl = string.Format(@"select A.[ID],A.[UA_UserAccountID],A.[UA_SysAuthorityID],B.[SA_AuthorityName],C.[UA_LoginAccount],C.[UA_UserName],C.[ID] from  [UserAuthority] A left join [SystemAuthority] B on A.[UA_SysAuthorityID]=B.[ID] left join [UserAccount] C on A.[UA_UserAccountID]=C.id where C.[UA_LoginAccount] = {0}", account);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "UA_AuthorityName");
            MyDBController.CloseConnection();

            int x = ds.Tables["UA_AuthorityName"].Rows.Count;
            foreach (DataRow row in ds.Tables["UA_AuthorityName"].Rows)
            {
                UserAuthorityLists ual = new UserAuthorityLists();
                ual.ID = Convert.ToInt64(row["ID"]);
                ual.UA_LoginAccount = row["UA_LoginAccount"].ToString();
                ual.SA_AuthorityName = row["SA_AuthorityName"].ToString();
                ual.UA_SysAuthorityID = Convert.ToInt64(row["UA_SysAuthorityID"]);
                ual.UA_UserAccountID = Convert.ToInt64(row["UA_UserAccountID"]);
                AuthorityList.Add(ual);
            }

            DBLog _dbLog = new DBLog();
            _dbLog.DBL_Content = User_Info.User_Name + "|在权限管理界面，查询条码系统账号权限列表，条码系统账号编号为：" + account;
            _dbLog.DBL_OperateBy = User_Info.User_Code;
            _dbLog.DBL_OperateTime = DateTime.Now.ToString();
            _dbLog.DBL_OperateType = OperateType.Select;
            _dbLog.DBL_OperateTable = "UserAccount";
            DBLog.WriteDBLog(_dbLog);
            return AuthorityList;
        }
    }
}
