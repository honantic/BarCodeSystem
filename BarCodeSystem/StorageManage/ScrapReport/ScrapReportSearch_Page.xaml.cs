using BarCodeSystem.ProductDispatch.FlowCard;
using BarCodeSystem.PublicClass.DatabaseEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BarCodeSystem.StorageManage.ScrapReport
{
    /// <summary>
    /// ScrapReportSearch_Page.xaml 的交互逻辑
    /// </summary>
    public partial class ScrapReportSearch_Page : Page
    {
        public ScrapReportSearch_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 搜索缴费单构造函数
        /// </summary>
        /// <param name="_ssr"></param>
        public ScrapReportSearch_Page(SubmitScrapReport _ssr)
        {
            InitializeComponent();
            ssr = _ssr;
        }

        /// <summary>
        /// 自动搜索构造函数
        /// </summary>
        /// <param name="_ssr"></param>
        /// <param name="_fcID"></param>
        /// <param name="_autoStart"></param>
        public ScrapReportSearch_Page(SubmitScrapReport _ssr, Int64 _fcID, bool _autoStart)
        {
            InitializeComponent();
            ssr = _ssr;
            fcID = _fcID;
            autoStart = _autoStart;
            if (autoStart)
            {
                object sender = new object();
                RoutedEventArgs e = new RoutedEventArgs();
                Page_Loaded(sender, e);
                if (datagrid_ScrapReport.HasItems)
                {
                    datagrid_ScrapReport.SelectedItem = 0;
                    btn_Select_Click(sender, e);
                }
            }
        }


        SubmitScrapReport ssr;
        Int64 fcID = -1;
        bool autoStart;

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FetchScrapReport();
        }

        private void FetchScrapReport()
        {
            string SQl = "";
            DataSet ds = new DataSet();
            if (fcID == -1)
            {
                SQl = string.Format(@"select * from [ScrapReport] where [SR_State]=0");
            }
            else
            {
                SQl = string.Format(@"select * from [ScrapReport] where [SR_FlowCardID]={0}", fcID);
            }
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "ScrapReport");
            MyDBController.CloseConnection();

            List<ScrapReportList> srls = new List<ScrapReportList>();
            foreach (DataRow row in ds.Tables["ScrapReport"].Rows)
            {
                ScrapReportList srl = new ScrapReportList();
                srl.ID = Convert.ToInt64(row["ID"]);
                srl.SR_OrgID = row["SR_OrgID"] is DBNull ? -1 : Convert.ToInt64(row["SR_OrgID"]);
                srl.SR_FlowCardID = row["SR_FlowCardID"] is DBNull ? -1 : Convert.ToInt64(row["SR_FlowCardID"]);
                srl.SR_FlowCardCode = row["SR_FlowCardCode"].ToString();
                srl.SR_ItemID = row["SR_ItemID"] is DBNull ? -1 : Convert.ToInt64(row["SR_ItemID"]);
                srl.SR_ItemName = row["SR_ItemName"].ToString();
                srl.SR_ItemCode = row["SR_ItemCode"].ToString();
                srl.SR_ItemSpec = row["SR_ItemSpec"].ToString();
                srl.SR_Code = row["SR_Code"].ToString();
                srl.SR_Type = row["SR_Type"] is DBNull ? 0 : Convert.ToInt32(row["SR_Type"]);
                srl.SR_ScrapAmount = row["SR_ScrapAmount"] is DBNull ? 0 : Convert.ToInt32(row["SR_ScrapAmount"]);
                srl.SR_CheckAmount = row["SR_CheckAmount"] is DBNull ? 0 : Convert.ToInt32(row["SR_CheckAmount"]);
                srl.SR_EntranceTime = Convert.ToDateTime(row["SR_EntranceTime"]);
                srl.SR_WorkCenterID = row["SR_WorkCenterID"] is DBNull ? -1 : Convert.ToInt64(row["SR_WorkCenterID"]);
                srl.SR_WorkCenterName = row["SR_WorkCenterName"].ToString();
                srl.SR_State = row["SR_State"] is DBNull ? 0 : Convert.ToInt32(row["SR_State"]);
                srl.SR_WareHouseID = row["SR_WareHouseID"] is DBNull ? -1 : Convert.ToInt64(row["SR_WareHouseID"]);
                srl.SR_CreateBy = row["SR_CreateBy"].ToString();
                srl.SR_CreateOn = Convert.ToDateTime(row["SR_CreateOn"]);
                srl.SR_CheckBy = row["SR_CheckBy"].ToString();
                srl.SR_CheckOn = Convert.ToDateTime(row["SR_CheckOn"]);
                srls.Add(srl);
            }

            datagrid_ScrapReport.ItemsSource = srls;
            if (srls.Count > 0)
            {
                datagrid_ScrapReport.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 选定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Select_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_ScrapReport.SelectedIndex > -1)
            {
                ssr.Invoke((ScrapReportList)datagrid_ScrapReport.SelectedItem);
            }
        }

        /// <summary>
        /// 选中项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_ScrapReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagrid_ScrapReport.SelectedIndex != -1)
            {
                ScrapReportList item = (ScrapReportList)datagrid_ScrapReport.SelectedItem;
                txtb_FlowCardCode.Text = item.SR_FlowCardCode;
                txtb_CreateBy.Text = item.SR_CreateBy;
                txtb_CreateTime.Text = item.SR_CreateOn.ToString();
                txtb_ItemInfo.Text = item.SR_ItemCode + "|" + item.SR_ItemName + "|" + item.SR_ItemSpec;
                txtb_WorkCenter.Text = item.SR_WorkCenterName;
                txtb_ScrapAmount.Text = item.SR_ScrapAmount.ToString();
            }
        }
    }
}
