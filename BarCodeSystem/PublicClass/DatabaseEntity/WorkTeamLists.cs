using BarCodeSystem.PublicClass.DatabaseEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;

namespace BarCodeSystem
{
    public class WorkTeamLists
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }

        //班组编码
        public string WT_Code
        {
            get;
            set;
        }

        //班组名称
        public string WT_Name
        {
            get;
            set;
        }

        //工作中心ID
        public Int64 WT_WorkCenterID
        {
            get;
            set;
        }


        //工作中心编码
        public string workcenterCode
        {
            get;
            set;
        }

        //工作中心名称
        public string workcenterName
        {
            get;
            set;
        }


        /// <summary>
        /// 是否展示
        /// </summary>
        public bool IsShown { get; set; }

        /// <summary>
        /// 检查当前编码是否在班组表中存在
        /// </summary>
        /// <param name="_key">编码</param>
        /// <param name="_wcID">可选参数，车间id</param>
        /// <returns></returns>
        public static bool CheckIfCodeExsist(string _key, Int64 _wcID = -1)
        {
            bool flag = false;
            string SQl = "";
            if (_wcID == -1)
            {
                SQl = string.Format("Select count(*) from WorkTeam where [WT_Code]='{0}' ", _key);
            }
            else
            {
                SQl = string.Format("Select count(*) from WorkTeam where [WT_Code]='{0}' and [WT_WorkCenterID]={1}", _key, _wcID);
            }
            MyDBController.GetConnection();
            int count = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));
            MyDBController.CloseConnection();
            flag = count > 0;
            return flag;
        }


        /// <summary>
        /// 根据班组编码获取人员列表
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        public static List<PersonLists> FetchPersonListByCode(string _key)
        {
            DataSet ds = new DataSet();
            List<PersonLists> plList = new List<PersonLists>();
            string SQl = string.Format("Select A.[P_Code],A.[P_Name],A.[ID] from  [Person] A left join [WorkTeamMember] B on A.[ID]=B.[WTM_MemberPersonID] left join [WorkTeam] C on B.[WTM_WorkTeamID]=C.[ID] where C.[WT_Code]='{0}'", _key);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "Person");
            MyDBController.CloseConnection();
            foreach (DataRow row in ds.Tables["Person"].Rows)
            {
                PersonLists pl = new PersonLists();
                pl.name = row["P_Name"].ToString();
                pl.code = row["P_Code"].ToString();
                pl.ID = Convert.ToInt64(row["ID"]);
                plList.Add(pl);
            }
            return plList;
        }

        /// <summary>
        /// 检验当前人员列表是否与系统中某个班组成员列表项匹配，匹配成功，则返回当前班组名称，在前台显示的时候替换掉，匹配失败，返回空字符串
        /// </summary>
        /// <param name="pl">人员列表</param>
        /// <returns></returns>
        public static string CheckIfMultiPersonMatchWT(List<PersonLists> pl)
        {
            #region 朴素的写法，通用。
            string WT_Name = string.Empty;
            List<List<WorkTeamMemberLists>> sysWorkTeamInfo = WorkTeamMemberLists.FetchWorkTeamMemberInfo(User_Info.User_Workcenter_ID);
            if (pl.Count > 0)
            {
                foreach (List<WorkTeamMemberLists> wtml in sysWorkTeamInfo)
                {
                    string str1 = "", str2 = "";
                    wtml.OrderBy(p => p.WTM_MemberPersonCode).ToList().ForEach(p => { str1 += str1.Length == 0 ? p.WTM_MemberPersonCode : "、" + p.WTM_MemberPersonCode; });
                    pl.OrderBy(p => p.code).ToList().ForEach(p => { str2 += str2.Length == 0 ? p.code : "、" + p.code; });
                    if (str1.Equals(str2))
                    {
                        WT_Name = wtml.FirstOrDefault().WTM_WorkTeamName;
                        break;
                    }
                }
            }
            return WT_Name;
            #endregion
            #region 装逼的写法，但是不能用break跳出循环，不理想
            //techRoutePerson.ForEach(
            //    pl =>
            //    {
            //        if (pl.Count > 0)
            //        {
            //            sysWorkTeamInfo.ForEach(
            //                wtml =>
            //                {
            //                    string str1 = "", str2 = "";

            //                    pl.OrderBy(p => p.code).ToList().ForEach(p => { str1 += str1.Length == 0 ? p.code : "、" + p.code; });

            //                    wtml.OrderBy(p => p.WTM_MemberPersonCode).ToList().ForEach(p => { str2 += str2.Length == 0 ? p.WTM_MemberPersonCode : "、" + p.WTM_MemberPersonCode; });

            //                    if (str1.Equals(str2))
            //                    {
            //                        int index = techRoutePerson.IndexOf(pl) + 1;
            //                        Xceed.Wpf.Toolkit.MessageBox.Show("第" + index.ToString() + "行人员信息有匹配的班组，系统将自动后台替换掉，不影响派工！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            //                    }
            //                }
            //                );
            //        }
            //    });
            #endregion

        }


        /// <summary>
        /// 检验当前人员列表是否与系统中某个班组成员列表项匹配，匹配成功，则返回当前班组名称，在前台显示的时候替换掉，匹配失败，返回空字符串
        /// </summary>
        /// <param name="fcsls"></param>
        /// <returns></returns>
        public static string CheckIfMultiPersonMatchWT(List<FlowCardSubLists> fcsls)
        {
            #region 朴素的写法，通用。
            string WT_Name = string.Empty;
            List<List<WorkTeamMemberLists>> sysWorkTeamInfo = WorkTeamMemberLists.FetchWorkTeamMemberInfo(User_Info.User_Workcenter_ID);
            if (fcsls.Count > 0)
            {
                foreach (List<WorkTeamMemberLists> wtml in sysWorkTeamInfo)
                {
                    string str1 = "", str2 = "";
                    wtml.OrderBy(p => p.WTM_MemberPersonCode).ToList().ForEach(p => { str1 += str1.Length == 0 ? p.WTM_MemberPersonCode : "、" + p.WTM_MemberPersonCode; });
                    fcsls.OrderBy(p => p.FCS_PersonCode).ToList().ForEach(p => { str2 += str2.Length == 0 ? p.FCS_PersonCode : "、" + p.FCS_PersonCode; });
                    if (str1.Equals(str2))
                    {
                        WT_Name = wtml.FirstOrDefault().WTM_WorkTeamName;
                        break;
                    }
                }
            }
            return WT_Name;
            #endregion
            #region 装逼的写法，但是不能用break跳出循环，不理想
            //techRoutePerson.ForEach(
            //    pl =>
            //    {
            //        if (pl.Count > 0)
            //        {
            //            sysWorkTeamInfo.ForEach(
            //                wtml =>
            //                {
            //                    string str1 = "", str2 = "";

            //                    pl.OrderBy(p => p.code).ToList().ForEach(p => { str1 += str1.Length == 0 ? p.code : "、" + p.code; });

            //                    wtml.OrderBy(p => p.WTM_MemberPersonCode).ToList().ForEach(p => { str2 += str2.Length == 0 ? p.WTM_MemberPersonCode : "、" + p.WTM_MemberPersonCode; });

            //                    if (str1.Equals(str2))
            //                    {
            //                        int index = techRoutePerson.IndexOf(pl) + 1;
            //                        Xceed.Wpf.Toolkit.MessageBox.Show("第" + index.ToString() + "行人员信息有匹配的班组，系统将自动后台替换掉，不影响派工！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            //                    }
            //                }
            //                );
            //        }
            //    });
            #endregion
        }

        /// <summary>
        /// 根据车间id获取班组列表
        /// </summary>
        /// <param name="_wcID"></param>
        /// <returns></returns>
        public static List<WorkTeamLists> FetchWTInfoByWCID(Int64 _wcID = -1)
        {
            string SQl = "";
            if (_wcID == -1)
            {
                SQl = "select A.[ID],A.[WT_Code],A.[WT_Name],A.[WT_WorkCenterID],A.[WT_IsShown],B.[WC_Department_Code],B.[WC_Department_Name] from [WorkTeam] A left join [WorkCenter] B on A.[WT_WorkCenterID]=B.[WC_Department_ID]";
            }
            else
            {
                SQl = string.Format("select A.[ID],A.[WT_Code],A.[WT_Name],A.[WT_WorkCenterID],A.[WT_IsShown],B.[WC_Department_Code],B.[WC_Department_Name] from [WorkTeam] A left join [WorkCenter] B on A.[WT_WorkCenterID]=B.[WC_Department_ID] where A.[WT_WorkCenterID]={0}", _wcID);
            }
            return ExecuteSQlCommand(SQl);
        }

        /// <summary>
        /// 执行查询班组信息的sql 命令
        /// </summary>
        /// <param name="_command"></param>
        /// <returns></returns>
        private static List<WorkTeamLists> ExecuteSQlCommand(string _command)
        {
            DataSet ds = new DataSet();
            List<WorkTeamLists> _wtlList = new List<WorkTeamLists>();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(_command, ds, "WorkTeam");
            MyDBController.CloseConnection();
            foreach (DataRow row in ds.Tables["WorkTeam"].Rows)
            {
                WorkTeamLists wtl = new WorkTeamLists();
                wtl.ID = Convert.ToInt64(row["ID"]);
                wtl.workcenterCode = row["WC_Department_Code"].ToString();
                wtl.workcenterName = row["WC_Department_Name"].ToString();
                wtl.IsShown = row["WT_IsShown"] is DBNull ? true : Convert.ToBoolean(row["WT_IsShown"]);
                wtl.WT_Code = row["WT_Code"].ToString();
                wtl.WT_Name = row["WT_Name"].ToString();
                wtl.WT_WorkCenterID = Convert.ToInt64(row["WT_WorkCenterID"]);
                _wtlList.Add(wtl);
            }
            return _wtlList;
        }

        /// <summary>
        /// 保存班组信息
        /// </summary>
        /// <param name="_wtl"></param>
        /// <returns></returns>
        public static bool SaveInfo(WorkTeamLists _wtl)
        {
            bool flag = false;
            DataSet ds = new DataSet();
            string SQl = string.Format("select top 0 * from [WorkTeam]");
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "WorkTeam");
            List<WorkTeamLists> wtlList = new List<WorkTeamLists>() { _wtl };
            flag = MyDBController.InsertSqlBulk<WorkTeamLists>(wtlList, ds.Tables["WorkTeam"]);
            MyDBController.CloseConnection();
            return flag;
        }


        /// <summary>
        /// 根据班组编号和车间id获取班组的ID
        /// </summary>
        /// <param name="_wtCode"></param>
        /// <param name="_wcID"></param>
        /// <returns></returns>
        public static List<WorkTeamLists> FetchWTID(string _wtCode, Int64 _wcID)
        {
            string SQl = string.Format("select A.[ID],A.[WT_Code],A.[WT_Name],A.[WT_WorkCenterID],A.[WT_IsShown],B.[WC_Department_Code],B.[WC_Department_Name] from [WorkTeam] A left join [WorkCenter] B on A.[WT_WorkCenterID]=B.[WC_Department_ID] where A.[WT_Code]='{0}' and  A.[WT_WorkCenterID]={1}", _wtCode, _wcID);
            return ExecuteSQlCommand(SQl);
        }
    }
}
