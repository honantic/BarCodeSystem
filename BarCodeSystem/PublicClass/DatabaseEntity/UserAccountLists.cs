using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace BarCodeSystem
{
    public class UserAccountLists
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
        /// 登陆账号
        /// </summary>
        public string UA_LoginAccount
        {
            get;
            set;
        }

        /// <summary>
        /// 登陆账号的姓名
        /// </summary>
        public string UA_UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool UA_IsValidated
        {
            get;
            set;
        }

        /// <summary>
        /// 登陆账号部门ID
        /// </summary>
        public Int64 UA_DepartmentID
        {
            get;
            set;
        }

        /// <summary>
        /// 登陆账号部门名称
        /// </summary>
        public string UA_DepartmentName
        {
            get;
            set;
        }
        /// <summary>
        /// 岗位
        /// </summary>
        public string P_Position { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get;
            set;
        }

        /// <summary>
        /// 根据车间id获取账号列表
        /// </summary>
        /// <param name="_WCID"></param>
        /// <returns></returns>
        public static List<UserAccountLists> FetchUAInfoByWCID(Int64 _WCID)
        {
            List<UserAccountLists> ualList = new List<UserAccountLists>();
            string SQl = string.Format("select A.[ID],A.[UA_LoginAccount],A.[UA_UserName],A.[UA_IsValidated],B.[P_Position],C.[WC_Department_ID],C.[WC_Department_Name],C.[WC_Department_Code] from [UserAccount] A left join [Person] B on A.[UA_LoginAccount]=B.[P_Code] left join [WorkCenter] C on B.[P_WorkCenterID]=C.[WC_Department_ID] where B.[P_WorkCenterID]={0} and A.[UA_LoginAccount] !='admin'", _WCID);
            DataSet ds = new DataSet();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "UserAccount");
            MyDBController.CloseConnection();
            foreach (DataRow row in ds.Tables["UserAccount"].Rows)
            {
                UserAccountLists ual = new UserAccountLists();
                ual.ID = Convert.ToInt64(row["ID"]);
                ual.UA_LoginAccount = row["UA_LoginAccount"].ToString();
                ual.UA_UserName = row["UA_UserName"].ToString();
                ual.UA_DepartmentName = row["WC_Department_Name"].ToString();
                ual.UA_DepartmentID = Convert.ToInt64(row["WC_Department_ID"]);
                ual.UA_IsValidated = Convert.ToBoolean(row["UA_IsValidated"]);
                ual.P_Position = row["P_Position"].ToString();
                ualList.Add(ual);
            }
            return ualList;
        }

        /// <summary>
        /// 获取所有账号权限列表
        /// </summary>
        /// <returns></returns>
        public static List<UserAccountLists> FetchUAInfoByWCID()
        {
            List<UserAccountLists> ualList = new List<UserAccountLists>();
            string SQl = string.Format("select A.[ID],A.[UA_LoginAccount],A.[UA_UserName],A.[UA_IsValidated],B.[P_Position],C.[WC_Department_ID],C.[WC_Department_Name],C.[WC_Department_Code] from [UserAccount] A left join [Person] B on A.[UA_LoginAccount]=B.[P_Code] left join [WorkCenter] C on B.[P_WorkCenterID]=C.[WC_Department_ID] where B.[P_WorkCenterID] is not null and A.[UA_LoginAccount] !='admin'");
            DataSet ds = new DataSet();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "UserAccount");
            MyDBController.CloseConnection();
            foreach (DataRow row in ds.Tables["UserAccount"].Rows)
            {
                UserAccountLists ual = new UserAccountLists();
                ual.ID = Convert.ToInt64(row["ID"]);
                ual.UA_LoginAccount = row["UA_LoginAccount"].ToString();
                ual.UA_UserName = row["UA_UserName"].ToString();
                ual.UA_DepartmentName = row["WC_Department_Name"].ToString();
                ual.UA_DepartmentID = Convert.ToInt64(row["WC_Department_ID"]);
                ual.UA_IsValidated = Convert.ToBoolean(row["UA_IsValidated"]);
                ual.P_Position = row["P_Position"].ToString();
                ualList.Add(ual);
            }
            return ualList;
        }
    }
}
