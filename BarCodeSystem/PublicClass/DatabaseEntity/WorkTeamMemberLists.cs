using BarCodeSystem.PublicClass.HelperClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;

namespace BarCodeSystem.PublicClass.DatabaseEntity
{
    public class WorkTeamMemberLists
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 班组ID
        /// </summary>
        public Int64 WTM_WorkTeamID { get; set; }
        /// <summary>
        /// 班组工作中心名称
        /// </summary>
        public string WTM_WorkCenterName { get; set; }
        /// <summary>
        /// 帮组工作中心ID
        /// </summary>
        public Int64 WTM_WorkCenterID { get; set; }
        /// <summary>
        /// 人员ID
        /// </summary>
        public Int64 WTM_MemberPersonID { get; set; }
        /// <summary>
        /// 人员编号
        /// </summary>
        public string WTM_MemberPersonCode { get; set; }
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string WTM_MemberPersonName { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public int WTM_SortOrder { get; set; }
        /// <summary>
        /// 保留字段
        /// </summary>
        public string WT_ReservedSegment { get; set; }
        /// <summary>
        /// 班组名称
        /// </summary>
        public string WTM_WorkTeamName { get; set; }
        /// <summary>
        /// 班组编号
        /// </summary>
        public string WTM_WorkTeamCode { get; set; }

        /// <summary>
        /// 当前班组是否显示
        /// </summary>
        public bool WTM_IsShown { get; set; }
        /// <summary>
        /// 根据工作中心id，获取班组成员列表的列表
        /// </summary>
        /// <param name="_workcenterID"></param>
        /// <returns></returns>
        public static List<List<WorkTeamMemberLists>> FetchWorkTeamMemberInfo(Int64 _workcenterID)
        {
            DataSet ds = new DataSet();
            List<List<WorkTeamMemberLists>> teamMemberList = new List<List<WorkTeamMemberLists>>();
            string SQl = string.Format(@"Select B.[ID],B.[WTM_WorkTeamID],A.[WT_Code],A.[WT_Name],A.[WT_IsShown],B.[WTM_MemberPersonID],C.[P_Name],C.[P_Code],D.[WC_Department_Name],D.[WC_Department_ID] from [WorkTeam] A left join [WorkTeamMember] B on a.[ID]=b.[WTM_WorkTeamID] left join [Person] C on b.[WTM_MemberPersonID]=c.[ID] left join [WorkCenter] D on A.[WT_WorkCenterID]=D.[WC_Department_ID] where A.[WT_WorkCenterID]={0}", _workcenterID);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "WorkTeamMember");
            MyDBController.CloseConnection();

            List<WorkTeamMemberLists> wtmList = new List<WorkTeamMemberLists>();
            try
            {
                foreach (DataRow row in ds.Tables["WorkTeamMember"].Rows)
                {
                    wtmList.Add(new WorkTeamMemberLists()
                    {
                        ID = Convert.ToInt64(row["ID"]),
                        WTM_IsShown = row["WT_IsShown"] is DBNull ? true : Convert.ToBoolean(row["WT_IsShown"]),
                        WTM_WorkTeamID = Convert.ToInt64(row["WTM_WorkTeamID"]),
                        WTM_WorkTeamCode = row["WT_Code"].ToString(),
                        WTM_WorkTeamName = row["WT_Name"].ToString(),
                        WTM_MemberPersonID = Convert.ToInt64(row["WTM_MemberPersonID"]),
                        WTM_MemberPersonName = row["P_Name"].ToString(),
                        WTM_MemberPersonCode = row["P_Code"].ToString(),
                        WTM_WorkCenterID = Convert.ToInt64(row["WC_Department_ID"]),
                        WTM_WorkCenterName = row["WC_Department_Name"].ToString()
                    });
                }

                List<Int64> wtID = wtmList.Distinct(new ListComparer<WorkTeamMemberLists>((p1, p2) => { return p1.WTM_WorkTeamID.Equals(p2.WTM_WorkTeamID); })).Select(p => p.WTM_WorkTeamID).ToList();
                wtID.ForEach((p) => { teamMemberList.Add(new List<WorkTeamMemberLists>()); });
                wtmList.ForEach(
                    p =>
                    {
                        int index = wtID.IndexOf(p.WTM_WorkTeamID);
                        teamMemberList[index].Add(p);
                    });
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            return teamMemberList;
        }

        /// <summary>
        /// 根据工作中心id，获取班组成员列表
        /// </summary>
        /// <param name="_workcenterID"></param>
        /// <returns></returns>
        public static List<WorkTeamMemberLists> FetchWorkTeamInfo(Int64 _workcenterID)
        {
            DataSet ds = new DataSet();
            List<WorkTeamMemberLists> teamMemberList = new List<WorkTeamMemberLists>();
            string SQl = string.Format(@"Select B.[ID],B.[WTM_WorkTeamID],A.[WT_Code],A.[WT_Name],A.[WT_IsShown],B.[WTM_MemberPersonID],C.[P_Name],C.[P_Code],D.[WC_Department_Name],D.[WC_Department_ID] from [WorkTeam] A left join [WorkTeamMember] B on a.[ID]=b.[WTM_WorkTeamID] left join [Person] C on b.[WTM_MemberPersonID]=c.[ID] left join [WorkCenter] D on A.[WT_WorkCenterID]=D.[WC_Department_ID] where A.[WT_WorkCenterID]={0}", _workcenterID);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "WorkTeamMember");
            MyDBController.CloseConnection();
            foreach (DataRow row in ds.Tables["WorkTeamMember"].Rows)
            {
                teamMemberList.Add(new WorkTeamMemberLists()
                {
                    ID = Convert.ToInt64(row["ID"]),
                    WTM_IsShown = row["WT_IsShown"] is DBNull ? true : Convert.ToBoolean(row["WT_IsShown"]),
                    WTM_WorkTeamID = Convert.ToInt64(row["WTM_WorkTeamID"]),
                    WTM_WorkTeamCode = row["WT_Code"].ToString(),
                    WTM_WorkTeamName = row["WT_Name"].ToString(),
                    WTM_MemberPersonID = Convert.ToInt64(row["WTM_MemberPersonID"]),
                    WTM_MemberPersonName = row["P_Name"].ToString(),
                    WTM_MemberPersonCode = row["P_Code"].ToString(),
                    WTM_WorkCenterID = Convert.ToInt64(row["WC_Department_ID"]),
                    WTM_WorkCenterName = row["WC_Department_Name"].ToString()
                });
            }
            return teamMemberList;
        }


        /// <summary>
        /// 根据班组id，获取班组成员列表
        /// </summary>
        /// <param name="_workteamid"></param>
        /// <returns></returns>
        public static List<WorkTeamMemberLists> FetchWorkTeamInfo(Int64 _workteamid, bool _isTeam = true)
        {
            DataSet ds = new DataSet();
            List<WorkTeamMemberLists> teamMemberList = new List<WorkTeamMemberLists>();
            string SQl = string.Format(@"Select B.[ID],B.[WTM_WorkTeamID],A.[WT_Code],A.[WT_Name],A.[WT_IsShown],B.[WTM_MemberPersonID],C.[P_Name],C.[P_Code],D.[WC_Department_Name],D.[WC_Department_ID] from [WorkTeam] A left join [WorkTeamMember] B on a.[ID]=b.[WTM_WorkTeamID] left join [Person] C on b.[WTM_MemberPersonID]=c.[ID] left join [WorkCenter] D on A.[WT_WorkCenterID]=D.[WC_Department_ID] where A.[ID]={0}", _workteamid);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "WorkTeamMember");
            MyDBController.CloseConnection();
            foreach (DataRow row in ds.Tables["WorkTeamMember"].Rows)
            {
                teamMemberList.Add(new WorkTeamMemberLists()
                {
                    ID = Convert.ToInt64(row["ID"]),
                    WTM_IsShown = row["WT_IsShown"] is DBNull ? true : Convert.ToBoolean(row["WT_IsShown"]),
                    WTM_WorkTeamID = Convert.ToInt64(row["WTM_WorkTeamID"]),
                    WTM_WorkTeamCode = row["WT_Code"].ToString(),
                    WTM_WorkTeamName = row["WT_Name"].ToString(),
                    WTM_MemberPersonID = Convert.ToInt64(row["WTM_MemberPersonID"]),
                    WTM_MemberPersonName = row["P_Name"].ToString(),
                    WTM_MemberPersonCode = row["P_Code"].ToString(),
                    WTM_WorkCenterID = Convert.ToInt64(row["WC_Department_ID"]),
                    WTM_WorkCenterName = row["WC_Department_Name"].ToString()
                });
            }
            return teamMemberList;
        }


        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="_wtmlList"></param>
        public static bool SaveInfo(List<WorkTeamMemberLists> _wtmlList)
        {
            bool flag = false;
            if (_wtmlList.Count > 0)
            {
                DataSet ds = new DataSet();
                string SQl = "select top 0 * from [WorkTeamMember] ";
                MyDBController.GetConnection();
                MyDBController.GetDataSet(SQl, ds, "WorkTeamMember");
                flag = MyDBController.InsertSqlBulk(_wtmlList, ds.Tables["WorkTeamMember"]);
                MyDBController.CloseConnection();
                if (flag)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("保存成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("保存失败，请重试!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return flag;
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="_wtmlList"></param>
        public static void DeleteInfo(List<WorkTeamMemberLists> _wtmlList)
        {
            if (_wtmlList.Count > 0)
            {
                DataSet ds = new DataSet();
                string message = "";
                MyDBController.GetConnection();
                string SQl = "select top 0 * from [WorkTeamMember] ";
                MyDBController.GetConnection();
                MyDBController.GetDataSet(SQl, ds, "WorkTeamMember");
                bool flag = MyDBController.DeleteSqlBulk(_wtmlList, ds.Tables["WorkTeamMember"], out message);
                if (flag)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(message, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                MyDBController.CloseConnection();
            }
        }
    }
}
