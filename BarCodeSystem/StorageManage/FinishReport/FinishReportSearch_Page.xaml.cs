using BarCodeSystem.ProductDispatch.FlowCard;
using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.ValueConverters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BarCodeSystem.StorageManage.FinishReport
{
    /// <summary>
    /// FinishReportSearch_Page.xaml 的交互逻辑
    /// </summary>
    public partial class FinishReportSearch_Page : Page
    {
        public FinishReportSearch_Page()
        {
            InitializeComponent();
        }

        public FinishReportSearch_Page(SubmitFinishReport _sfr)
        {
            InitializeComponent();
            sfr = _sfr;
        }

        public FinishReportSearch_Page(SubmitFinishReport _sfr, Int64 _fcID, bool _autoStart)
        {
            InitializeComponent();
            fcID = _fcID;
            autoStart = _autoStart;
            sfr = _sfr;
            if (autoStart)
            {
                object sender = new object();
                RoutedEventArgs e = new RoutedEventArgs();
                Page_Loaded(sender, e);
                if (datagrid_FinishReport.HasItems)
                {
                    datagrid_FinishReport.SelectedIndex = 0;
                    btn_Select_Click(sender, e);
                }
            }
        }

        Int64 fcID = -1;
        bool autoStart;
        DataSet ds = new DataSet();
        SubmitFinishReport sfr;

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FetchFinishReport();
        }

        /// <summary>
        /// 获取条码系统中的信息
        /// </summary>
        private void FetchFinishReport()
        {
            string SQl = "";
            if (fcID == -1)
            {
                SQl = string.Format(@"select * from [FinishReport] where [FR_State] =0");
            }
            else
            {
                SQl = string.Format(@"select * from [FinishReport] where [FR_FlowCardID]={0}", fcID);
            }
            ds = new DataSet();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FinishReport");
            MyDBController.CloseConnection();

            List<FinishReportList> frls = new List<FinishReportList>();
            foreach (DataRow row in ds.Tables["FinishReport"].Rows)
            {
                FinishReportList frl = new FinishReportList();
                frl.ID = Convert.ToInt64(row["ID"]);
                frl.FR_Code = row["FR_Code"].ToString();
                frl.FR_ProduceOrderID = row["FR_ProduceOrderID"] is DBNull ? -1 : Convert.ToInt64(row["FR_ProduceOrderID"]);
                frl.FR_ProduceOrderCode = row["FR_ProduceOrderCode"].ToString();
                frl.FR_FlowCardID = row["FR_FlowCardID"] is DBNull ? -1 : Convert.ToInt64(row["FR_FlowCardID"]);
                frl.FR_FlowCardCode = row["FR_FlowCardCode"].ToString();
                frl.FR_ItemID = row["FR_ItemID"] is DBNull ? -1 : Convert.ToInt64(row["FR_ItemID"]);
                frl.FR_ItemName = row["FR_ItemName"].ToString();
                frl.FR_ItemCode = row["FR_ItemCode"].ToString();
                frl.FR_ItemSpec = row["FR_ItemSpec"].ToString();
                frl.FR_ProjectCode = row["FR_ProjectCode"].ToString();
                frl.FR_OperateType = row["FR_OperateType"] is DBNull ? -1 : Convert.ToInt32(row["FR_OperateType"]);
                frl.FR_WorkCenterID = row["FR_WorkCenterID"] is DBNull ? -1 : Convert.ToInt64(row["FR_WorkCenterID"]);
                frl.FR_WorkCenterName = row["FR_WorkCenterName"].ToString();
                frl.FR_FinishTime = row["FR_FinishTime"] is DBNull ? new DateTime() : Convert.ToDateTime(row["FR_FinishTime"]);
                frl.FR_WarehouseID = row["FR_WarehouseID"] is DBNull ? -1 : Convert.ToInt64(row["FR_WarehouseID"]);
                frl.FR_QualifiedAmount = row["FR_QualifiedAmount"] is DBNull ? 0 : Convert.ToInt32(row["FR_QualifiedAmount"]);
                frl.FR_ScrappedAmount = row["FR_ScrappedAmount"] is DBNull ? 0 : Convert.ToInt32(row["FR_ScrappedAmount"]);
                frl.FR_StockAmount = row["FR_StockAmount"] is DBNull ? 0 : Convert.ToInt32(row["FR_StockAmount"]);
                frl.FR_State = Convert.ToInt32(row["FR_State"]);
                frl.FR_CreateTime = row["FR_CreateTime"] is DBNull ? new DateTime() : Convert.ToDateTime(row["FR_CreateTime"]);
                frl.FR_CreateBy = row["FR_CreateBy"].ToString();
                frl.FR_CheckTime = row["FR_CheckTime"] is DBNull ? new DateTime() : Convert.ToDateTime(row["FR_CheckTime"]);
                frl.FR_CheckBy = row["FR_CheckBy"].ToString();
                frls.Add(frl);
            }
            datagrid_FinishReport.ItemsSource = frls;
        }

        /// <summary>
        /// 选择项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_FinishReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagrid_FinishReport.SelectedIndex != -1)
            {
                FinishReportList item = (FinishReportList)datagrid_FinishReport.SelectedItem;
                txtb_Checkby.Text = item.FR_CheckBy;
                txtb_CreateBy.Text = item.FR_CreateBy;
                txtb_CreateTime.Text = item.FR_CreateTime.ToString();
                txtb_FinishReportState.Text = (new FinishReportStateConverter()).Convert(item.FR_State, typeof(string), null, new System.Globalization.CultureInfo("")).ToString(); ;
                txtb_FlowCardCode.Text = item.FR_FlowCardCode;
                txtb_ItemInfo.Text = item.FR_ItemCode + "|" + item.FR_ItemName + "|" + item.FR_ItemSpec;
                txtb_QualifiedAmount.Text = item.FR_QualifiedAmount.ToString();
                txtb_ScrapAmount.Text = item.FR_ScrappedAmount.ToString();
                txtb_ProjectCode.Text = item.FR_ProjectCode;
                txtb_SourceOrder.Text = item.FR_ProduceOrderCode;
                txtb_WorkCenter.Text = item.FR_WorkCenterName;
            }
         
        }

        /// <summary>
        /// 选定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Select_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_FinishReport.SelectedIndex != -1)
            {
                sfr.Invoke((FinishReportList)datagrid_FinishReport.SelectedItem);
            }
        }
    }
}
