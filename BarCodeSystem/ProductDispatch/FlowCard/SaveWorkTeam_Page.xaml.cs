using BarCodeSystem.PublicClass.DatabaseEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarCodeSystem.ProductDispatch.FlowCard
{
    /// <summary>
    /// SaveWorkTeam_Page.xaml 的交互逻辑
    /// </summary>
    public partial class SaveWorkTeam_Page : Page
    {
        #region 变量
        List<PersonLists> personList = new List<PersonLists>();
        Int64 departID = 0;
        string departName = "";
        List<WorkTeamMemberLists> teamMemberList = new List<WorkTeamMemberLists>();
        DataSet ds = new DataSet();
        #endregion

        #region 构造函数
        public SaveWorkTeam_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 筛选班组的构造函数
        /// </summary>
        /// <param name="_departID"></param>
        public SaveWorkTeam_Page(Int64 _departID)
        {
            InitializeComponent();
            departID = _departID;
            textb_Header.Text = "选择班组信息";
            Panel.SetZIndex(panel_AddTeamCode, 0);
            Panel.SetZIndex(panel_AddTeamName, 0);
        }

        /// <summary>
        /// 保存班组的构造函数
        /// </summary>
        /// <param name="_personList"></param>
        /// <param name="_departID"></param>
        public SaveWorkTeam_Page(List<PersonLists> _personList, Int64 _departID, string _departName)
        {
            InitializeComponent();
            personList = _personList;
            departID = _departID;
            departName = _departName;
            textb_Header.Text = "保存班组信息";
            Panel.SetZIndex(btn_SaveWorkTeam, 1);
            btn_Refresh.Visibility = Visibility.Hidden;
        }
        #endregion

        #region 加载初始化
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            datagrid_WorkTeamInfo.ItemsSource = string.IsNullOrEmpty(departName) ? FetchWorkTeamInfo() : SaveWorkTeamInit();
        }

        /// <summary>
        /// 保存班组的初始化事件
        /// </summary>
        private List<WorkTeamMemberLists> SaveWorkTeamInit()
        {
            foreach (PersonLists item in personList)
            {
                teamMemberList.Add(new WorkTeamMemberLists()
                {
                    WTM_WorkCenterID = departID,
                    WTM_WorkCenterName = departName,
                    WTM_MemberPersonCode = item.code,
                    WTM_MemberPersonName = item.name,
                });
            }
            return teamMemberList;
        }

        /// <summary>
        /// 选取班组的初始化事件
        /// </summary>
        /// <returns></returns>
        private List<WorkTeamMemberLists> FetchWorkTeamInfo()
        {
            string SQl = string.Format(@"Select A.[ID],A.[WT_Code],A.[WT_Name],B.[WTM_MemberPersonID],C.[P_Name],C.[P_Code],D.[WC_Department_Name],D[WC_Department_ID] from [WorkTeam] A left join [WorkTeamMember] B on a.[ID]=b.[WTM_WorkTeamID] left join [Person] on b.[WTM_MemberPersonID]=c.[ID] left join [WorkCenter] D on A.[WT_WorkCenterID]=D.[WC_Department_ID] where A.[WT_WorkCenterID]={0}", departID);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "WorkTeamMember");
            MyDBController.CloseConnection();
            foreach (DataRow row in ds.Tables["WorkTeamMember"].Rows)
            {
                teamMemberList.Add(new WorkTeamMemberLists()
                {
                    WTM_WorkTeamID = Convert.ToInt64(row["ID"]),
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
        #endregion

        /// <summary>
        /// 保存班组信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveWorkTeam_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfCanSave())
            {
                SaveTeam();
                SaveTeamMember();
            }
        }

        /// <summary>
        /// 保存班组人员到WorkTeamMember表
        /// </summary>
        private void SaveTeamMember()
        {
            MyDBController.GetConnection();
            string SQl = "Select top 0 * from [WorkTeamMember]";
            MyDBController.GetDataSet(SQl, ds, "WorkTeamMember");
            List<string> colList = new List<string>();
            foreach (DataColumn col in ds.Tables["WorkTeamMember"].Columns)
            {
                colList.Add(col.ColumnName);
            }
            ds.Tables["WorkTeamMember"].Columns.Add(new DataColumn("IDNew", typeof(Int64)));

            foreach (WorkTeamMemberLists item in teamMemberList)
            {
                DataRow row = ds.Tables["WorkTeamMember"].NewRow();
                row["WTM_WorkTeamID"] = item.WTM_WorkTeamID;
                row["WTM_MemberPersonID"] = item.WTM_MemberPersonID;
                row["WTM_SortOrder"] = item.WTM_SortOrder;
                row["WT_ReservedSegment"] = item.WT_ReservedSegment;
                ds.Tables["WorkTeamMember"].Rows.Add(row);
            }
            int updateNum, insertNum;
            MyDBController.InsertSqlBulk(ds.Tables["WorkTeamMember"], colList, out updateNum, out insertNum);
            MyDBController.CloseConnection();
        }

        /// <summary>
        /// 报存班组信息到WorkTeam表
        /// </summary>
        private void SaveTeam()
        {
            MyDBController.GetConnection();
            string SQl = "Select top 0 * from [WorkTeam]";
            MyDBController.GetDataSet(SQl, ds, "WorkTeam");
            List<String> colList = new List<string>();
            foreach (DataColumn col in ds.Tables["WorkTeam"].Columns)
            {
                colList.Add(col.ColumnName);
            }
            ds.Tables["WorkTeam"].Columns.Add(new DataColumn("IDNew", typeof(Int64)));

            DataRow row = ds.Tables["WorkTeam"].NewRow();
            row["WT_Code"] = teamMemberList[0].WTM_WorkTeamCode;
            row["WT_Name"] = teamMemberList[0].WTM_WorkTeamName;
            row["WT_WorkCenterID"] = teamMemberList[0].WTM_WorkCenterID;
            ds.Tables["WorkTeam"].Rows.Add(row);

            int updateNum, insertNum;
            MyDBController.InsertSqlBulk(ds.Tables["WorkTeam"], colList, out updateNum, out insertNum);

            SQl = string.Format(@"Select [ID] from [WorkTeam] where [WT_Code]='{0}'", teamMemberList[0].WTM_WorkTeamCode);
            SqlDataReader reader = MyDBController.GetDataReader(SQl);
            while (reader.Read())
            {
                foreach (WorkTeamMemberLists item in teamMemberList)
                {
                    item.WTM_WorkTeamID = Convert.ToInt64(reader["ID"]);
                }
            }
            MyDBController.CloseConnection();
        }

        /// <summary>
        /// 当前班组编号和名称不能为空,编号不能重复
        /// </summary>
        /// <returns></returns>
        private bool CheckIfCanSave()
        {
            bool flag = true;
            if (teamMemberList.Count > 0)
            {
                foreach (WorkTeamMemberLists item in teamMemberList)
                {
                    if (string.IsNullOrEmpty(item.WTM_WorkTeamCode) || string.IsNullOrEmpty(item.WTM_WorkTeamName))
                    {
                        flag = false;
                        MessageBox.Show("请为每一个人添加班组信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                }
                if (flag)
                {
                    string SQl = string.Format("Select ID from [WorkTeam] where [WT_Code]='{0}'", teamMemberList[0].WTM_WorkTeamCode);
                    MyDBController.GetConnection();
                    int count = MyDBController.ExecuteNonQuery(SQl);
                    if (count > 0)
                    {
                        MessageBox.Show("当前班组编号重复，不可用！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        flag = false;
                    }
                    MyDBController.CloseConnection();
                }
            }
            else
            {
                flag = false;
            }

            return flag;
        }

        /// <summary>
        /// 批量设置编号的按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TeamCodeAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtb_TeamCodeInfo.Text))
            {
                MessageBox.Show("您没有输入编号或者您输入的编号为空");
            }
            else
            {
                string SQl = string.Format("Select ID from [WorkTeam] where [WT_Code]='{0}'", txtb_TeamCodeInfo.Text.Trim());
                MyDBController.GetConnection();
                int count = MyDBController.ExecuteNonQuery(SQl);
                MyDBController.CloseConnection();
                if (count > 0)
                {
                    MessageBox.Show("当前班组编号重复，不可用！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    foreach (WorkTeamMemberLists item in teamMemberList)
                    {
                        item.WTM_WorkTeamCode = txtb_TeamCodeInfo.Text.Trim();
                    }
                    datagrid_WorkTeamInfo.Items.Refresh();
                }
            }

        }

        /// <summary>
        /// 回车快捷搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_TeamCodeInfo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_TeamCodeAdd_Click(sender, e);
            }
        }

        /// <summary>
        /// 搜索班组信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TeamSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtb_TeamInfo_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Submit_Click(object sender, RoutedEventArgs e)
        {

        }





    }
}
