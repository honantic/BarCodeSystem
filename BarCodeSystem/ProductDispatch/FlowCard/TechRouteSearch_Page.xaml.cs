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
        #endregion

        #region 加载事件
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            datagrid_RouteVersion.ItemsSource = FetchTechRouteInfo(itemCode);
            if (datagrid_RouteVersion.Items.Count == 0)
            {
                label_ErrorInfo.Visibility = Visibility.Visible;
            }
        }
        #endregion

        /// <summary>
        /// 选定工艺路线，提交工艺路线信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_RouteVersion.ItemsSource != null && datagrid_RouteProcess.ItemsSource != null)
            {
                string version = ((TechVersion)datagrid_RouteVersion.SelectedItem).TRV_Version;
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
            string SQl = string.Format(@"Select A.[ID],A.[TRV_Version] from [TechRouteVersion] A left join [ItemInfo] B on A.[TRV_ItemID]=B.[ID]  where B.[II_Code]='{0}'", str);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "TechRouteVersion");

            foreach (DataRow row in ds.Tables["TechRouteVersion"].Rows)
            {
                tvl.Add(new TechVersion()
                {
                    TR_VersionID = Convert.ToInt64(row["ID"]),
                    TRV_Version = row["TRV_Version"].ToString()
                });
            }

            //工艺路线的工序信息
            SQl = string.Format(@"Select A.[ID],A.[TR_ItemID],A.[TR_VersionID],A.[TR_ProcessSequence],A.[TR_ProcessName],A.[TR_ProcessCode],A.[TR_ProcessID],A.[TR_IsReportPoint],A.[TR_IsExProcess],A.[TR_WorkCenterID],A.[TR_IsFirstProcess],A.[TR_IsLastProcess],A.[TR_WagePerPiece],A.[TR_WorkHour],A.[TR_WageAllotScheme],A.[TR_AllotFormulaID],A.[TR_IsReportDevice],A.[TR_IsDeviceCharging],B.[WC_Department_Name] from [TechRoute] A left join [WorkCenter] B on A.[TR_WorkCenterID]=B.[WC_Department_ID] where [TR_ItemCode]='{0}' order by  [TR_VersionID] ,[TR_ProcessSequence]", itemCode);
            MyDBController.GetDataSet(SQl, ds, "TechRouteInfo");
            MyDBController.CloseConnection();

            foreach (DataRow row in ds.Tables["TechRouteInfo"].Rows)
            {
                trls.Add(new TechRouteLists()
                {
                    TR_VersionID = Convert.ToInt64(row["TR_VersionID"]),
                    TR_ProcessSequence = Convert.ToInt32(row["TR_ProcessSequence"]),
                    TR_ProcessName = row["TR_ProcessName"].ToString(),
                    TR_WagePerPiece = Convert.ToDecimal(row["TR_WagePerPiece"]),
                    TR_IsFirstProcess = Convert.ToBoolean(row["TR_IsFirstProcess"]),
                    TR_IsLastProcess = Convert.ToBoolean(row["TR_IsLastProcess"]),
                    WC_Department_Name = row["WC_Department_Name"].ToString()
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
                long versionID = ((TechVersion)datagrid_RouteVersion.SelectedItem).TR_VersionID;
                processGridSource = trls.FindAll(p => p.TR_VersionID == versionID);
                datagrid_RouteProcess.ItemsSource = processGridSource;
            }
            catch (Exception)
            {
            }
        }

    }
}
