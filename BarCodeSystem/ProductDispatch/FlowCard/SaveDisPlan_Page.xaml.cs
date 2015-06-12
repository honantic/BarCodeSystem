using BarCodeSystem.PublicClass.DatabaseEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BarCodeSystem.ProductDispatch.FlowCard
{
    /// <summary>
    /// SaveDisPlan_Page.xaml 的交互逻辑
    /// </summary>
    public partial class SaveDisPlan_Page : Page
    {
        public SaveDisPlan_Page()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 保存派工方案的构造函数
        /// </summary>
        public SaveDisPlan_Page(List<DisPlanLists> _disPlanList, List<DisPlanVersionLists> _disPlanVersionList)
        {
            InitializeComponent();
            disPlanList = _disPlanList;
            disPlanVersionList = _disPlanVersionList;
            textb_Header.Text = "保存派工方案信息";
            btn_Refresh.Visibility = panel_Search.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 选取派工方案的构造函数
        /// </summary>
        /// <param name="_techRouteVersionID"></param>
        /// <param name="_itemID"></param>
        public SaveDisPlan_Page(Int64 _techRouteVersionID, Int64 _itemID, FillDisPlan _fillDis)
        {
            InitializeComponent();
            techRouteVersionID = _techRouteVersionID;
            itemID = _itemID;
            textb_Header.Text = "筛选派工方案信息";
            Panel.SetZIndex(btn_SelectDisPlan, 1);
            fillDis = _fillDis;
        }
        #region 变量
        Int64 techRouteVersionID, itemID;
        List<DisPlanLists> disPlanList;
        List<DisPlanVersionLists> disPlanVersionList;
        List<string> colList = new List<string>();
        DataSet ds = new DataSet();
        FillDisPlan fillDis;
        #endregion
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (textb_Header.Text.Equals("保存派工方案信息"))
            {
                datagrid_DisPlanVer.ItemsSource = disPlanVersionList;
                datagrid_DisPlanInfo.ItemsSource = disPlanList;
            }
            else
            {
                datagrid_DisPlanVer.ItemsSource = FetchDisPlanVersion();
                FetchDisPlan(disPlanVersionList);
            }
        }



        #region 保存派工方案
        /// <summary>
        /// 保存派工方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveDisPlan_Click(object sender, RoutedEventArgs e)
        {
            ShowConfirmWindow(IsPlanExist());
        }

        /// <summary>
        /// 检查当前料品+版本 的派工方案在数据库中是否存在，不存在则返回false，存在则返回true
        /// </summary>
        /// <returns></returns>
        private bool IsPlanExist()
        {
            bool flag = false;
            try
            {
                string SQl = string.Format(@"Select Count(*) from [DisPlanVersion] where [DPV_ItemID]={0} and [DPV_TechRouteVersionID]={1}", disPlanVersionList.Count == 0 ? 0 : disPlanVersionList[0].DPV_ItemID, disPlanVersionList.Count == 0 ? 0 : disPlanVersionList[0].DPV_TechRouteVersionID);
                MyDBController.GetConnection();
                SqlDataReader reader = MyDBController.GetDataReader(SQl);
                while (reader.Read())
                {
                    flag = Convert.ToInt32(reader[0]) > 0 ? true : false;
                }
                MyDBController.CloseConnection();
            }
            catch (Exception)
            {
            }

            return flag;
        }

        /// <summary>
        /// 根据检查结果设置确认窗口的内容
        /// </summary>
        /// <param name="flag"></param>
        private void ShowConfirmWindow(bool flag)
        {
            DisPlanConfirm_Window dpw = new DisPlanConfirm_Window();
            switch (flag)
            {
                case false:
                    dpw.frame_Confirm.Navigate(new DisPlanConfirm_Page2(disPlanList, disPlanVersionList, ClearList));
                    dpw.ShowDialog();
                    break;
                case true:
                    dpw.frame_Confirm.Navigate(new DisPlanConfirm_Page1(disPlanList, disPlanVersionList, ClearList));
                    dpw.ShowDialog();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 将派工方案存入数据库，最后执行的.
        /// 没用到，都在ConfirmPage里面执行了
        /// </summary>
        public void ClearList()
        {
            datagrid_DisPlanInfo.ItemsSource = null;
            datagrid_DisPlanVer.ItemsSource = null;
            //disPlanList.Clear();
        }
        #endregion


        #region 选取派工方案
        /// <summary>
        /// 选取派工方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectDisPlan_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_DisPlanVer.SelectedIndex != -1)
            {
                if (MessageBox.Show("将会覆盖现有的派工人员信息，是否继续？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    fillDis.Invoke((List<DisPlanLists>)datagrid_DisPlanInfo.ItemsSource);
                }
            }
            else
            {
                MessageBox.Show("请选择一个派工方案！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// 回车快捷搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_DisPlanInfo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_DisPlanSearch_Click(sender, e);
            }
        }
        /// <summary>
        /// 筛选版本信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DisPlanSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtb_DisPlanInfo.Text))
            {
                string key = txtb_DisPlanInfo.Text;
                datagrid_DisPlanInfo.ItemsSource = null;
                datagrid_DisPlanVer.ItemsSource = disPlanVersionList.FindAll(p => p.DPV_ItemName.IndexOf(key) != -1 || p.DPV_ItemCode.IndexOf(key) != -1 || p.DPV_VersionName.IndexOf(key) != -1);
            }
            else
            {
                datagrid_DisPlanVer.ItemsSource = disPlanVersionList;
            }
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            datagrid_DisPlanInfo.ItemsSource = null;
            datagrid_DisPlanVer.ItemsSource = disPlanVersionList;
        }

        /// <summary>
        /// 获取派工方案版本
        /// </summary>
        private List<DisPlanVersionLists> FetchDisPlanVersion()
        {
            try
            {
                ds.Clear();
                disPlanVersionList = new List<DisPlanVersionLists>();
                string SQl = string.Format(@"Select A.[ID],A.[DPV_VersionName],A.[DPV_ItemID],A.[DPV_TechRouteVersionID],B.[II_Name],B.[II_Code] from [DisPlanVersion] A left join [ItemInfo] B on A.[DPV_ItemID]=B.[ID] where [DPV_ItemID]={0} and [DPV_TechRouteVersionID]={1}", itemID, techRouteVersionID);
                MyDBController.GetConnection();
                MyDBController.GetDataSet(SQl, ds, "DisPlanVersion");
                MyDBController.CloseConnection();

                foreach (DataRow row in ds.Tables["DisPlanVersion"].Rows)
                {
                    disPlanVersionList.Add(new DisPlanVersionLists()
                    {
                        ID = Convert.ToInt64(row["ID"]),
                        DPV_ItemID = Convert.ToInt64(row["DPV_ItemID"]),
                        DPV_ItemName = row["II_Name"].ToString(),
                        DPV_ItemCode = row["II_Code"].ToString(),
                        DPV_TechRouteVersionID = Convert.ToInt64(row["DPV_TechRouteVersionID"]),
                        DPV_VersionName = row["DPV_VersionName"].ToString()
                    });
                }
                return disPlanVersionList;
            }
            catch (Exception)
            {
                return disPlanVersionList;
            }
        }
        /// <summary>
        /// 获取派工方案详细
        /// </summary>
        /// <param name="disPlanVersion"></param>
        /// <returns></returns>
        private List<DisPlanLists> FetchDisPlan(List<DisPlanVersionLists> disPlanVersion)
        {
            ds.Clear();
            disPlanList = new List<DisPlanLists>();
            MyDBController.GetConnection();
            foreach (DisPlanVersionLists item in disPlanVersion)
            {
                string SQl = string.Format(@"Select A.[ID],A.[DP_TechRouteID],A.[DP_PersonID],A.[DP_DisPlanVersionID],B.[P_Code],B.[P_Name],C.[TR_ProcessSequence],C.[TR_ProcessName] from [DisPlan]  A left join [Person] B on A.[DP_PersonID]=B.[ID] left join [TechRoute] C on A.[DP_TechRouteID] =C.[ID] where A.[DP_DisPlanVersionID]={0}", item.ID);
                MyDBController.GetDataSet(SQl, ds, "DisPlan");
            }
            MyDBController.CloseConnection();

            foreach (DataRow row in ds.Tables["DisPlan"].Rows)
            {
                disPlanList.Add(new DisPlanLists()
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
            return disPlanList;
        }

        /// <summary>
        /// 选中版本事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_DisPlanVer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagrid_DisPlanVer.SelectedIndex != -1)
            {
                datagrid_DisPlanInfo.ItemsSource = disPlanList.FindAll(p => p.DP_DisPlanVersionID.Equals(((DisPlanVersionLists)datagrid_DisPlanVer.SelectedItem).ID));
            }
        }
        #endregion
    }
}
