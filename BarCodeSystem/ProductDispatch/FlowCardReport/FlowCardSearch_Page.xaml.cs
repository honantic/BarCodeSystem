using BarCodeSystem.ProductDispatch.FlowCard;
using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.ValueConverters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace BarCodeSystem.ProductDispatch.FlowCardReport
{
    /// <summary>
    /// FlowCardSearch_Page.xaml 的交互逻辑
    /// </summary>
    public partial class FlowCardSearch_Page : Page
    {
        public FlowCardSearch_Page()
        {
            InitializeComponent();
        }

        public FlowCardSearch_Page(SubmitFlowCard _sfc)
        {
            InitializeComponent();
            sfc = _sfc;
        }

        SubmitFlowCard sfc;
        List<FlowCardLists> fcls = new List<FlowCardLists>();
        List<FlowCardSubLists> fcsls = new List<FlowCardSubLists>();
        int lastindex = -1;
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FetchFlowCardInfo();
        }

        /// <summary>
        /// 获取系统中未完工的流转卡信息
        /// </summary>
        private bool FetchFlowCardInfo()
        {
            try
            {
                DataSet ds = new DataSet();
                string SQl = string.Format(@"Select A.[ID],A.[FC_CardType],A.[FC_Code],A.[FC_Amount],A.[FC_CardState],A.[FC_CreateBy],A.[FC_CreateTime],A.[FC_CheckBy],A.[FC_CheckTime],B.[PO_ItemCode],B.[PO_ItemName],B.[PO_ItemSpec],B.[PO_Code],C.[TRV_VersionCode],C.[TRV_VersionName],D.[WC_Department_Name] from [FlowCard] A left join [ProduceOrder] B on A.[FC_SourceOrderID]=B.[PO_ID] left join [TechRouteVersion] C on A.[FC_ItemTechVersionID]=C.[ID] left join [WorkCenter] D on A.[FC_WorkCenter]=D.[WC_Department_ID] where A.[FC_CardState] != 2");
                MyDBController.GetConnection();
                MyDBController.GetDataSet(SQl, ds, "FlowCard");
                MyDBController.CloseConnection();
                foreach (DataRow row in ds.Tables["FlowCard"].Rows)
                {
                    fcls.Add(new FlowCardLists()
                    {
                        FC_Amount = Convert.ToInt32(row["FC_Amount"]),
                        FC_CardState = Convert.ToInt32(row["FC_CardState"]),
                        FC_CardType = Convert.ToInt32(row["FC_CardType"]),
                        FC_CheckBy = row["FC_CheckBy"].ToString(),
                        FC_CheckTime = Convert.ToDateTime(row["FC_CheckTime"]),
                        FC_Code = row["FC_Code"].ToString(),
                        FC_CreateBy = row["FC_CreateBy"].ToString(),
                        FC_CreateTime = Convert.ToDateTime(row["FC_CreateTime"]),
                        ID = Convert.ToInt64(row["ID"]),
                        PO_ItemCode = row["PO_ItemCode"].ToString(),
                        PO_ItemName = row["PO_ItemName"].ToString(),
                        PO_ItemSpec = row["PO_ItemSpec"].ToString(),
                        WC_Department_Name = row["WC_Department_Name"].ToString(),
                        TRV_VersionCode = row["TRV_VersionCode"].ToString(),
                        TRV_VersionName = row["TRV_VersionName"].ToString(),
                        PO_Code = row["PO_Code"].ToString()
                    });
                }
                datagrid_FlowCard.ItemsSource = fcls;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 列表选中项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_FlowCard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagrid_FlowCard.SelectedIndex != -1)
            {
                FlowCardLists fcl = (FlowCardLists)datagrid_FlowCard.SelectedItem;
                txtb_Checkby.Text = fcl.FC_CheckBy;
                txtb_CreateBy.Text = fcl.FC_CreateBy;
                txtb_CreateTime.Text = fcl.FC_CreateTime.ToShortDateString();
                txtb_FlowCardCode.Text = fcl.FC_Code;
                txtb_ItemInfo.Text = fcl.PO_ItemCode + " | " + fcl.PO_ItemName + " | " + fcl.PO_ItemSpec;
                txtb_StartAmount.Text = fcl.FC_Amount.ToString();
                txtb_TechRouteVersion.Text = fcl.TRV_VersionName;
                txtb_WorkCenter.Text = fcl.WC_Department_Name;
                txtb_SourceOrder.Text = fcl.PO_Code;
                txtb_TechRouteVersion.Text = fcl.TRV_VersionName;
                FlowCardStateConverter fcsc = new FlowCardStateConverter();
                txtb_FlowCardState.Text = fcsc.Convert(fcl.FC_CardState, typeof(string), null, new CultureInfo("")).ToString();

                FlowCardTypeConverter fctc = new FlowCardTypeConverter();
                txtb_FlowCardType.Text = fctc.Convert(fcl.FC_CardType, typeof(string), null, new CultureInfo("")).ToString();
            }
        }

        /// <summary>
        /// 选定流转卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Select_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (datagrid_FlowCard.SelectedIndex != -1)
            {
                if (datagrid_FlowCard.SelectedIndex != lastindex)
                {
                    (MyDBController.FindVisualParent<FlowCardReport_Page>(this)[0]).isNewFlowCard = true;
                    lastindex = datagrid_FlowCard.SelectedIndex;
                    FlowCardLists fc = (FlowCardLists)datagrid_FlowCard.SelectedItem;
                    Int64 flowCardID = fc.ID;
                    FetchFlowCardSubInfo(flowCardID);
                    TechVersion techVersion = FetchTechVersion();
                    if (techVersion != null)
                    {
                        //sfc(fc, fcsls, techVersion);
                        sfc.Invoke(fc, fcsls, techVersion);
                        //sfc.BeginInvoke(fc, fcsls, techVersion, null, null);
                        (MyDBController.FindVisualParent<FlowCardReport_Page>(this)[0]).isNewFlowCard = false;
                    }
                }
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 获取选定流转卡行表信息
        /// </summary>
        /// <returns></returns>
        private List<FlowCardSubLists> FetchFlowCardSubInfo(Int64 flowCardID)
        {
            DataSet ds = new DataSet();
            string SQl = string.Format(@"Select A.[ID],A.[FCS_FlowCradID],A.[FCS_ItemId],A.[FCS_TechRouteID],A.[FCS_ProcessID],A.[FCS_ProcessName],A.[FCS_PersonCode],A.[FCS_PersonName],A.[FCS_BeginAmount],A.[FCS_QulifiedAmount],A.[FCS_ScrappedAmount],A.[FCS_UnprocessedAm],A.[FCS_CheckByID],A.[FCS_CheckByName],A.[FCS_PieceAmount],A.[FCS_PieceDivNum],A.[FCS_IsFirstProcess],A.[FCS_IsLastProcess],A.[FCS_IsReported],B.[TR_ProcessSequence] from [FlowCardSub] A left join [TechRoute] B on A.[FCS_TechRouteID]=B.[ID] where [FCS_FlowCradID]={0}", flowCardID);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCardSub");
            MyDBController.CloseConnection();
            fcsls.Clear();
            foreach (DataRow row in ds.Tables["FlowCardSub"].Rows)
            {
                fcsls.Add(new FlowCardSubLists()
                {
                    ID = Convert.ToInt64(row["ID"]),
                    FCS_FlowCradID = Convert.ToInt64(row["FCS_FlowCradID"]),
                    FCS_ItemId = Convert.ToInt64(row["FCS_ItemId"]),
                    FCS_TechRouteID = Convert.ToInt64(row["FCS_TechRouteID"]),
                    FCS_ProcessID = Convert.ToInt64(row["FCS_ProcessID"]),
                    FCS_ProcessName = row["FCS_ProcessName"].ToString(),
                    FCS_PersonCode = row["FCS_PersonCode"].ToString(),
                    FCS_PersonName = row["FCS_PersonName"].ToString(),
                    FCS_BeginAmount = Convert.ToInt32(row["FCS_BeginAmount"]),
                    FCS_QulifiedAmount = Convert.ToInt32(row["FCS_QulifiedAmount"]),
                    FCS_ScrappedAmount = Convert.ToInt32(row["FCS_ScrappedAmount"]),
                    FCS_UnprocessedAm = Convert.ToInt32(row["FCS_UnprocessedAm"]),
                    FCS_CheckByID = Convert.ToInt64(row["FCS_CheckByID"]),
                    FCS_CheckByName = row["FCS_CheckByName"].ToString(),
                    FCS_PieceAmount = Convert.ToInt32(row["FCS_PieceAmount"]),
                    FCS_PieceDivNum = Convert.ToInt32(row["FCS_PieceDivNum"]),
                    FCS_IsFirstProcess = Convert.ToBoolean(row["FCS_IsFirstProcess"]),
                    FCS_IsLastProcess = Convert.ToBoolean(row["FCS_IsLastProcess"]),
                    FCS_IsReported = Convert.ToBoolean(row["FCS_IsReported"]),
                    FCS_ProcessSequanece = Convert.ToInt32(row["TR_ProcessSequence"])
                });
            }
            return fcsls;
        }

        /// <summary>
        /// 获取工艺版本信息
        /// </summary>
        /// <returns></returns>
        private TechVersion FetchTechVersion()
        {
            try
            {
                DataSet ds = new DataSet();
                string SQl = string.Format(@"Select A.[ID],A.[TRV_ReportWay],A.[TRV_IsBackVersion],A.[TRV_IsSpecialVersion] from [TechRouteVersion] A left join [FlowCard] B on A.[ID] = B.[FC_ItemTechVersionID] where B.[ID]={0}", fcsls[0].FCS_FlowCradID);
                MyDBController.GetConnection();
                MyDBController.GetDataSet(SQl, ds, "TechRouteVersion");
                MyDBController.CloseConnection();

                DataRow row = ds.Tables["TechRouteVersion"].Rows[0];
                return new TechVersion()
                {
                    ID = Convert.ToInt64(row["ID"]),
                    TRV_ReportWay = Convert.ToInt32(row["TRV_ReportWay"]),
                    TRV_IsSpecialVersion = Convert.ToBoolean(row["TRV_IsSpecialVersion"]),
                    TRV_IsBackVersion = Convert.ToBoolean(row["TRV_IsBackVersion"]),
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
