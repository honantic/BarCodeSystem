using BarCodeSystem.PublicClass.DatabaseEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BarCodeSystem.ProductDispatch.FlowCard
{
    /// <summary>
    /// DisPlanConfirm_Page3.xaml 的交互逻辑
    /// </summary>
    public partial class DisPlanConfirm_Page3 : Page
    {
        public DisPlanConfirm_Page3()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_disPlanList"></param>
        /// <param name="_disPlanVersionList"></param>
        public DisPlanConfirm_Page3(List<DisPlanLists> _disPlanList, List<DisPlanVersionLists> _disPlanVersionList, ClearDisPlanList _cdpl)
        {
            InitializeComponent();
            disPlanVersionList_New = _disPlanVersionList;
            disPlanList_New = _disPlanList;
            cdpl = _cdpl;
        }

        List<DisPlanVersionLists> disPlanVersionList_New, disPlanVersionList_Old;
        List<DisPlanLists> disPlanList_New, disPlanList_Old;
        DataSet ds = new DataSet();
        ClearDisPlanList cdpl;


        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            datagrid_PlanName.ItemsSource = FetchDisPlanVersion();
            FetchDisPlan(disPlanVersionList_Old);
        }

        /// <summary>
        /// 获取派工方案版本
        /// </summary>
        private List<DisPlanVersionLists> FetchDisPlanVersion()
        {
            disPlanVersionList_Old = new List<DisPlanVersionLists>();
            try
            {
                ds.Clear();
                string SQl = string.Format(@"Select [ID],[DPV_VersionName],[DPV_ItemID],[DPV_TechRouteVersionID] from [DisPlanVersion] where [DPV_ItemID]={0} and [DPV_TechRouteVersionID]={1}", disPlanVersionList_New[0].DPV_ItemID, disPlanVersionList_New[0].DPV_TechRouteVersionID);
                MyDBController.GetConnection();
                MyDBController.GetDataSet(SQl, ds, "DisPlanVersion");
                MyDBController.CloseConnection();

                foreach (DataRow row in ds.Tables["DisPlanVersion"].Rows)
                {
                    disPlanVersionList_Old.Add(new DisPlanVersionLists()
                    {
                        ID = Convert.ToInt64(row["ID"]),
                        DPV_ItemID = Convert.ToInt64(row["DPV_ItemID"]),
                        DPV_TechRouteVersionID = Convert.ToInt64(row["DPV_TechRouteVersionID"]),
                        DPV_VersionName = row["DPV_VersionName"].ToString()
                    });
                }
                return disPlanVersionList_Old;
            }
            catch (Exception)
            {
                return disPlanVersionList_Old;
            }
        }
        /// <summary>
        /// 获取派工方案详细
        /// </summary>
        /// <param name="disPlanVersion"></param>
        /// <returns></returns>
        private List<DisPlanLists> FetchDisPlan(List<DisPlanVersionLists> disPlanVersion)
        {
            disPlanList_Old = new List<DisPlanLists>();
            ds.Clear();
            MyDBController.GetConnection();
            foreach (DisPlanVersionLists item in disPlanVersion)
            {
                string SQl = string.Format(@"Select A.[ID],A.[DP_TechRouteID],A.[DP_PersonID],A.[DP_DisPlanVersionID],B.[P_Code],B.[P_Name],C.[TR_ProcessSequence],C.[TR_ProcessName] from [DisPlan]  A left join [Person] B on A.[DP_PersonID]=B.[ID] left join [TechRoute] C on A.[DP_TechRouteID] =C.[ID] where A.[DP_DisPlanVersionID]={0}", item.ID);
                MyDBController.GetDataSet(SQl, ds, "DisPlan");
            }
            MyDBController.CloseConnection();

            foreach (DataRow row in ds.Tables["DisPlan"].Rows)
            {
                disPlanList_Old.Add(new DisPlanLists()
                {
                    ID = Convert.ToInt64(row["ID"]),
                    DP_DisPlanVersionID = Convert.ToInt64(row["DP_DisPlanVersionID"]),
                    DP_PersonCode = row["P_Code"].ToString(),
                    DP_PersonID = Convert.ToInt64(row["DP_PersonID"]),
                    DP_PersonName = row["P_Name"].ToString(),
                    DP_ProcessName = row["TR_ProcessName"].ToString(),
                    DP_ProcessSequence = Convert.ToInt32(row["TR_ProcessSequence"]),
                    DP_TechRouteID = Convert.ToInt64(row["DP_TechRouteID"])
                });
            }
            return disPlanList_Old;
        }

        /// <summary>
        /// 选中方案事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_PlanName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagrid_PlanName.SelectedIndex != -1)
            {
                datagrid_PlanDetail.ItemsSource = disPlanList_Old.FindAll(p => p.DP_DisPlanVersionID.Equals(((DisPlanVersionLists)datagrid_PlanName.SelectedItem).ID));
            }
        }
        /// <summary>
        /// 保存方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_PlanName.SelectedIndex != -1)
            {
                DeleteFromDB();
                InsertIntoDB();
                if (MessageBox.Show("保存成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    ((DisPlanConfirm_Window)MyDBController.FindVisualParent<Window>(this)[0]).Close();
                    cdpl.Invoke();
                }
            }
            else
            {
                MessageBox.Show("请先选择一个方案！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 覆盖方案第一步，先删除
        /// </summary>
        private void DeleteFromDB()
        {
            int index = datagrid_PlanName.SelectedIndex;
            string SQl = string.Format(@"Delete from [DisPlan] where [DP_DisPlanVersionID]={0}", disPlanVersionList_Old[index].ID);
            MyDBController.GetConnection();
            MyDBController.ExecuteNonQuery(SQl);
            MyDBController.CloseConnection();
        }

        /// <summary>
        /// 覆盖第二步，插入数据
        /// </summary>
        private void InsertIntoDB()
        {
            try
            {
                MyDBController.GetConnection();
                ds = new DataSet();
                if (!ds.Tables.Contains("DisPlan"))
                {
                    string SQl = string.Format(@"Select top 0 [ID],[DP_TechRouteID],[DP_PersonID],[DP_DisPlanVersionID] from [DisPlan]");
                    MyDBController.GetDataSet(SQl, ds, "DisPlan");
                }
                else
                {
                    ds.Tables["DisPlan"].Clear();
                }

                List<string> colList = new List<string>();
                foreach (DataColumn col in ds.Tables["DisPlan"].Columns)
                {
                    colList.Add(col.ColumnName);
                }
                ds.Tables["DisPlan"].Columns.Add(new DataColumn("IDNew", typeof(Int64)));

                int index = datagrid_PlanName.SelectedIndex;
                foreach (DisPlanLists item in disPlanList_New)
                {
                    DataRow row = ds.Tables["DisPlan"].NewRow();
                    row["DP_TechRouteID"] = item.DP_TechRouteID;
                    row["DP_PersonID"] = item.DP_PersonID;
                    row["DP_DisPlanVersionID"] = disPlanVersionList_Old[index].ID;
                    ds.Tables["DisPlan"].Rows.Add(row);
                }

                int updateNum, insertNum;
                MyDBController.InsertSqlBulk(ds.Tables["DisPlan"], colList, out updateNum, out insertNum);
                MyDBController.CloseConnection();
            }
            catch (Exception)
            {
            }
        }
    }
}
