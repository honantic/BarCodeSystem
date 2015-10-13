using System;
using System.Collections.Generic;
using System.Data;
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
    /// TechRouteSearchPage.xaml 的交互逻辑
    /// </summary>
    public partial class TechRouteSearch_Page : Page
    {
        SubmitTechRouteInfo stri;
        string itemCode;
        public TechRouteSearch_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="_stri"></param>
        public TechRouteSearch_Page(string value, SubmitTechRouteInfo _stri)
        {
            InitializeComponent();
            stri = _stri;
            itemCode = value;
            textb_PageTitle.Text = value + textb_PageTitle.Text;
        }

        #region 变量
        DataSet ds = new DataSet();
        List<TechVersion> tvl = new List<TechVersion>();
        List<TechRouteLists> trls = new List<TechRouteLists>();
        List<TechRouteLists> processGridSource = new List<TechRouteLists>();
        int loadCount = 0;
        #endregion

        #region 加载事件
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                datagrid_RouteVersion.ItemsSource = FetchTechRouteInfo(itemCode);
                if (datagrid_RouteVersion.Items.Count == 0)
                {
                    label_ErrorInfo.Visibility = Visibility.Visible;
                    Xceed.Wpf.Toolkit.MessageBox.Show("该料品没有工艺路线信息！请先维护！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                FecthDefaultInfo();
                loadCount++;
            }
        }
        #endregion

        /// <summary>
        /// 在页面加载的时候，获取该料品的默认工艺路线信息，直接返填
        /// </summary>
        private void FecthDefaultInfo()
        {
            for (int i = 0; i < tvl.Count; i++)
            {
                if (tvl[i].TRV_IsDefaultVer)
                {
                    datagrid_RouteVersion.SelectedIndex = i;
                    break;
                }
            }
            object sender = new object();
            RoutedEventArgs e = new RoutedEventArgs();
            btn_Submit_Click(sender, e);
        }

        /// <summary>
        /// 选定工艺路线，提交工艺路线信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_RouteVersion.ItemsSource != null && datagrid_RouteProcess.ItemsSource != null)
            {
                string version = ((TechVersion)datagrid_RouteVersion.SelectedItem).TRV_VersionCode;
                stri.Invoke(version, processGridSource);
            }
        }


        /// <summary>
        /// 获取指定料品的工艺路线版本信息，以及个版本的工序信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private List<TechVersion> FetchTechRouteInfo(string str)
        {
            //工艺路线版本datagrid数据源
            string SQl = string.Format(@"Select A.[ID],A.[TRV_VersionCode],A.[TRV_VersionName],A.[TRV_IsDefaultVer] from [TechRouteVersion] A left join [ItemInfo] B on A.[TRV_ItemID]=B.[ID]  where B.[II_Code]='{0}'", str);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "TechRouteVersion");

            foreach (DataRow row in ds.Tables["TechRouteVersion"].Rows)
            {
                tvl.Add(new TechVersion()
                {
                    ID = Convert.ToInt64(row["ID"]),
                    TRV_VersionCode = row["TRV_VersionCode"].ToString(),
                    TRV_VersionName = row["TRV_VersionName"].ToString(),
                    TRV_IsDefaultVer = (bool)row["TRV_IsDefaultVer"]
                });
            }

            //工艺路线的工序信息
            SQl = string.Format(@"Select A.[ID],A.[TR_ItemID],A.[TR_ItemCode],A.[TR_VersionID],A.[TR_ProcessSequence],A.[TR_ProcessName],A.[TR_ProcessCode],A.[TR_ProcessID],A.[TR_WorkHour],A.[TR_IsReportPoint],A.[TR_IsExProcess],A.[TR_WorkCenterID],A.[TR_IsFirstProcess],A.[TR_IsLastProcess],A.[TR_IsTestProcess],A.[TR_IsBackProcess],A.[TR_DefaultCheckPersonName],A.[TR_BindingProcess],A.[TR_IsReportDevice],A.[TR_IsDeviceCharging],B.[WC_Department_Name],C.[II_Name],D.[TRV_VersionCode],D.[TRV_VersionName] from [TechRoute] A left join [WorkCenter] B on A.[TR_WorkCenterID]=B.[WC_Department_ID] left join [ItemInfo] C on A.[TR_ItemID]=C.[ID] left join [TechRouteVersion] D on A.[TR_VersionID]=D.[ID] where [TR_ItemCode]='{0}' order by  [TR_VersionID] ,[TR_ProcessSequence]", itemCode);
            MyDBController.GetDataSet(SQl, ds, "TechRouteInfo");
            MyDBController.CloseConnection();

            foreach (DataRow row in ds.Tables["TechRouteInfo"].Rows)
            {
                trls.Add(new TechRouteLists()
                {
                    ID = Convert.ToInt64(row["ID"]),
                    TR_ProcessID = Convert.ToInt64(row["TR_ProcessID"]),
                    TR_VersionID = Convert.ToInt64(row["TR_VersionID"]),
                    TR_ProcessSequence = Convert.ToInt32(row["TR_ProcessSequence"]),
                    TR_ProcessName = row["TR_ProcessName"].ToString(),
                    TR_IsFirstProcess = Convert.ToBoolean(row["TR_IsFirstProcess"]),
                    TR_IsLastProcess = Convert.ToBoolean(row["TR_IsLastProcess"]),
                    TR_WorkCenterID = Convert.ToInt64(row["TR_WorkCenterID"]),
                    WC_Department_Name = row["WC_Department_Name"].ToString(),
                    TR_WorkHour = Convert.ToDecimal(row["TR_WorkHour"]),
                    TR_ItemID = Convert.ToInt64(row["TR_ItemID"]),
                    TR_ItemCode = row["TR_ItemCode"].ToString(),
                    II_Name = row["II_Name"].ToString(),
                    TRV_VersionCode = row["TRV_VersionCode"].ToString(),
                    TRV_VersionName = row["TRV_VersionName"].ToString(),
                    TR_IsTestProcess = Convert.ToBoolean(row["TR_IsTestProcess"]),
                    TR_IsBackProcess = Convert.ToBoolean(row["TR_IsBackProcess"])
                });
            }
            return tvl;
        }

        /// <summary>
        /// 工艺路线版本列表选中项更改的时候，更新工序信息列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_RouteVersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                long versionID = ((TechVersion)datagrid_RouteVersion.SelectedItem).ID;
                processGridSource = trls.FindAll(p => p.TR_VersionID == versionID);
                datagrid_RouteProcess.ItemsSource = processGridSource;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            datagrid_RouteVersion.ItemsSource = FetchTechRouteInfo(itemCode);
            if (datagrid_RouteVersion.Items.Count == 0)
            {
                label_ErrorInfo.Visibility = Visibility.Visible;
            }
            else
            {
                label_ErrorInfo.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 快捷选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_RouteVersion_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btn_Submit_Click(null, null);
        }

    }
}
