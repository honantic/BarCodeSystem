using BarCodeSystem.ProductDispatch.FlowCard;
using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.HelperClass;
using BarCodeSystem.PublicClass.ValueConverters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;

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

        /// <summary>
        /// 选定所有可报工的流转卡
        /// </summary>
        /// <param name="_sfc"></param>
        public FlowCardSearch_Page(SubmitFlowCard _sfc)
        {
            InitializeComponent();
            sfc = _sfc;
        }

        /// <summary>
        /// 流转卡报工的时候搜索指定的流转卡信息的构造函数
        /// </summary>
        /// <param name="_sfc"></param>
        /// <param name="_code">Report标记</param>
        /// <param name="_fccode">流转卡编号</param>
        /// <param name="_autostart">自动选择标记，设置为true</param>
        public FlowCardSearch_Page(SubmitFlowCard _sfc, string _code, string _fccode, bool _autostart)
        {
            InitializeComponent();
            sfc = _sfc;
            code = _code;
            fccode = _fccode;
            if (_autostart)
            {
                object _sender = new object();
                RoutedEventArgs _e = new RoutedEventArgs();
                Page_Loaded(_sender, _e);
                if (datagrid_FlowCard.HasItems)
                {
                    datagrid_FlowCard.SelectedIndex = 0;
                    btn_Select_Click(_sender, _e);
                }
            }
        }


        /// <summary>
        /// 流转卡分批、清卡，搜索指定流转卡编号的构造函数
        /// </summary>
        /// <param name="_sfc"></param>
        /// <param name="_code">清卡分批的时候设置为All</param>
        /// <param name="_autoStart">设置为true自动选择,设置为false手动选择</param>
        public FlowCardSearch_Page(SubmitFlowCard _sfc, int _state, string _code, bool _autoStart)
        {
            InitializeComponent();
            sfc = _sfc;
            code = _code;
            state = _state;
            if (_autoStart)
            {
                object _sender = new object();
                RoutedEventArgs _e = new RoutedEventArgs();
                Page_Loaded(_sender, _e);
                if (datagrid_FlowCard.HasItems)
                {
                    datagrid_FlowCard.SelectedIndex = 0;
                    btn_Select_Click(_sender, _e);
                }
            }
        }

        ///// <summary>
        ///// 流转卡分批、清卡，搜索所有流转卡构造函数
        ///// </summary>
        ///// <param name="_sfc"></param>
        ///// <param name="_state">设置为响应的状态，分批->0,清卡->1</param>
        ///// <param name="_code">设置为All</param>
        //public FlowCardSearch_Page(SubmitFlowCard _sfc, int _state, string _code)
        //{
        //    InitializeComponent();
        //    sfc = _sfc;
        //    code = _code;
        //    state = _state;
        //}


        /// <summary>
        /// 流转卡转序，搜索指定流转卡编号的构造函数
        /// </summary>
        /// <param name="_sfc"></param>
        /// <param name="_state">设置为2</param>
        /// <param name="_code">设置为All</param>
        /// <param name="_isTransfer">是否搜索转序，必须设置为true</param>
        /// <param name="_autoStart">自动获取,设置为true的时候自动开始,设置为false的时候手动选择</param>
        public FlowCardSearch_Page(SubmitFlowCard _sfc, int _state, string _code, bool _isTransfer, bool _autoStart)
        {
            InitializeComponent();
            sfc = _sfc;
            code = _code;
            state = _state;
            isTransfer = _isTransfer;
            if (_autoStart)
            {
                object _sender = new object();
                RoutedEventArgs _e = new RoutedEventArgs();
                Page_Loaded(_sender, _e);
                if (datagrid_FlowCard.HasItems)
                {
                    datagrid_FlowCard.SelectedIndex = 0;
                    btn_Select_Click(_sender, _e);
                }
            }
        }

        /// <summary>
        /// 清卡、转序、分批的时候流转卡编号，报工的时候的Report标记
        /// </summary>
        string code = "";

        /// <summary>
        /// 报工的时候编号
        /// </summary>
        string fccode = "";

        /// <summary>
        /// 要搜索的流转卡状态
        /// </summary>
        int state = 0;

        /// <summary>
        /// 是否搜索的是可转序的流转卡
        /// </summary>
        bool isTransfer = false;


        SubmitFlowCard sfc;
        List<FlowCardLists> fcls = new List<FlowCardLists>();
        List<FlowCardSubLists> fcsls = new List<FlowCardSubLists>();
        int lastindex = -1;
        int loadCount = 0;
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                FetchFlowCardInfo();
                loadCount++;
            }
        }

        /// <summary>
        /// 获取系统中未完工的流转卡信息
        /// </summary>
        private bool FetchFlowCardInfo()
        {
            try
            {
                string SQl = "";
                DataSet ds = new DataSet();
                if (isTransfer)
                {
                    #region 转序用
                    if (code.Equals("All"))//搜索所有可转序的流转卡
                    {
                        SQl = string.Format(@"Select A.[ID],A.[FC_CardType],[FC_SourceOrderID],A.[FC_Code],A.[FC_ItemID],A.[FC_ItemTechVersionID],A.[FC_Amount],A.[FC_WorkCenter],A.[FC_CardState],A.[FC_DistriSourceCard],A.[FC_FlowNum],A.[FC_CreateBy],A.[FC_CreateTime],A.[FC_CheckBy],A.[FC_CheckTime],A.[FC_BCSOrderID],A.[FC_FirstProcessNum],B.[PO_ItemCode],B.[PO_ItemName],B.[PO_ItemSpec],B.[PO_Code],C.[TRV_VersionCode],C.[TRV_VersionName],D.[WC_Department_Name] from [FlowCard] A left join [ProduceOrder] B on A.[FC_SourceOrderID]=B.[PO_ID] left join [TechRouteVersion] C on A.[FC_ItemTechVersionID]=C.[ID] left join [WorkCenter] D on A.[FC_WorkCenter]=D.[WC_Department_ID] where A.[FC_CardState] ={0} and  (select  max ([FCS_UnprocessedAm]) from [FlowCardSub] where [FCS_FlowCradID]=A.[ID]) > 0", state);
                    }
                    else
                    {
                        SQl = string.Format(@"Select A.[ID],A.[FC_CardType],[FC_SourceOrderID],A.[FC_Code],A.[FC_ItemID],A.[FC_ItemTechVersionID],A.[FC_Amount],A.[FC_WorkCenter],A.[FC_CardState],A.[FC_DistriSourceCard],A.[FC_FlowNum],A.[FC_CreateBy],A.[FC_CreateTime],A.[FC_CheckBy],A.[FC_CheckTime],A.[FC_BCSOrderID],A.[FC_FirstProcessNum],B.[PO_ItemCode],B.[PO_ItemName],B.[PO_ItemSpec],B.[PO_Code],C.[TRV_VersionCode],C.[TRV_VersionName],D.[WC_Department_Name] from [FlowCard] A left join [ProduceOrder] B on A.[FC_SourceOrderID]=B.[PO_ID] left join [TechRouteVersion] C on A.[FC_ItemTechVersionID]=C.[ID] left join [WorkCenter] D on A.[FC_WorkCenter]=D.[WC_Department_ID] where A.[FC_CardState] ={0} and [FC_Code]='{1}' and  (select  max ([FCS_UnprocessedAm]) from [FlowCardSub] where [FCS_FlowCradID]=A.[ID]) > 0", state, code);
                    }
                    #endregion
                }
                else
                {
                    #region 报工、清卡、分批用
                    if (string.IsNullOrEmpty(code))//取出开立或者报工状态的流转卡，报工界面用
                    {
                        SQl = string.Format(@"Select A.[ID],A.[FC_CardType],[FC_SourceOrderID],A.[FC_Code],A.[FC_ItemID],A.[FC_ItemTechVersionID],A.[FC_Amount],A.[FC_WorkCenter],A.[FC_CardState],A.[FC_DistriSourceCard],A.[FC_FlowNum],A.[FC_CreateBy],A.[FC_CreateTime],A.[FC_CheckBy],A.[FC_CheckTime],A.[FC_BCSOrderID],A.[FC_FirstProcessNum],B.[PO_ItemCode],B.[PO_ItemName],B.[PO_ItemSpec],B.[PO_Code],C.[TRV_VersionCode],C.[TRV_VersionName],D.[WC_Department_Name] from [FlowCard] A left join [ProduceOrder] B on A.[FC_SourceOrderID]=B.[PO_ID] left join [TechRouteVersion] C on A.[FC_ItemTechVersionID]=C.[ID] left join [WorkCenter] D on A.[FC_WorkCenter]=D.[WC_Department_ID] where A.[FC_CardState] != 2 and A.[FC_CardState] != 3 and A.[FC_CardState] != 4 and [FC_CardState] != 5");
                    }
                    else if (code.Equals("All"))//取出所有指定状态下的所有流转卡，清卡界面、分批界面使用
                    {
                        SQl = string.Format(@"Select A.[ID],A.[FC_CardType],[FC_SourceOrderID],A.[FC_Code],A.[FC_ItemID],A.[FC_ItemTechVersionID],A.[FC_Amount],A.[FC_WorkCenter],A.[FC_CardState],A.[FC_DistriSourceCard],A.[FC_FlowNum],A.[FC_CreateBy],A.[FC_CreateTime],A.[FC_CheckBy],A.[FC_CheckTime],A.[FC_BCSOrderID],A.[FC_FirstProcessNum],B.[PO_ItemCode],B.[PO_ItemName],B.[PO_ItemSpec],B.[PO_Code],C.[TRV_VersionCode],C.[TRV_VersionName],D.[WC_Department_Name] from [FlowCard] A left join [ProduceOrder] B on A.[FC_SourceOrderID]=B.[PO_ID] left join [TechRouteVersion] C on A.[FC_ItemTechVersionID]=C.[ID] left join [WorkCenter] D on A.[FC_WorkCenter]=D.[WC_Department_ID] where A.[FC_CardState] ={0}", state);
                    }
                    else if (code.Equals("Report"))
                    {
                        SQl = string.Format(@"Select A.[ID],A.[FC_CardType],[FC_SourceOrderID],A.[FC_Code],A.[FC_ItemID],A.[FC_ItemTechVersionID],A.[FC_Amount],A.[FC_WorkCenter],A.[FC_CardState],A.[FC_DistriSourceCard],A.[FC_FlowNum],A.[FC_CreateBy],A.[FC_CreateTime],A.[FC_CheckBy],A.[FC_CheckTime],A.[FC_BCSOrderID],A.[FC_FirstProcessNum],B.[PO_ItemCode],B.[PO_ItemName],B.[PO_ItemSpec],B.[PO_Code],C.[TRV_VersionCode],C.[TRV_VersionName],D.[WC_Department_Name] from [FlowCard] A left join [ProduceOrder] B on A.[FC_SourceOrderID]=B.[PO_ID] left join [TechRouteVersion] C on A.[FC_ItemTechVersionID]=C.[ID] left join [WorkCenter] D on A.[FC_WorkCenter]=D.[WC_Department_ID] where A.[FC_CardState] != 2 and A.[FC_CardState] != 3 and A.[FC_CardState] != 4 and [FC_CardState] != 5 and [FC_Code]='{0}'", fccode);
                    }
                    else//取出所有指定状态下的指定编号流转卡，清卡界面、分批界面使用
                    {
                        SQl = string.Format(@"Select A.[ID],A.[FC_CardType],[FC_SourceOrderID],A.[FC_Code],A.[FC_ItemID],A.[FC_ItemTechVersionID],A.[FC_Amount],A.[FC_WorkCenter],A.[FC_CardState],A.[FC_DistriSourceCard],A.[FC_FlowNum],A.[FC_CreateBy],A.[FC_CreateTime],A.[FC_CheckBy],A.[FC_CheckTime],A.[FC_BCSOrderID],A.[FC_FirstProcessNum],B.[PO_ItemCode],B.[PO_ItemName],B.[PO_ItemSpec],B.[PO_Code],C.[TRV_VersionCode],C.[TRV_VersionName],D.[WC_Department_Name] from [FlowCard] A left join [ProduceOrder] B on A.[FC_SourceOrderID]=B.[PO_ID] left join [TechRouteVersion] C on A.[FC_ItemTechVersionID]=C.[ID] left join [WorkCenter] D on A.[FC_WorkCenter]=D.[WC_Department_ID] where A.[FC_CardState]={0} and A.[FC_Code] ='{1}'", state, code);
                    }
                    #endregion
                }


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
                        FC_ItemID = Convert.ToInt64(row["FC_ItemID"]),
                        FC_FlowNum = Convert.ToInt32(row["FC_FlowNum"]),
                        FC_DistriSourceCard = Convert.ToInt64(row["FC_DistriSourceCard"]),
                        FC_ItemTechVersionID = Convert.ToInt64(row["FC_ItemTechVersionID"]),
                        FC_SourceOrderID = Convert.ToInt64(row["FC_SourceOrderID"]),
                        FC_WorkCenter = Convert.ToInt64(row["FC_WorkCenter"]),
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
                        PO_Code = row["PO_Code"].ToString(),
                        FC_BCSOrderID = row["FC_BCSOrderID"] is DBNull ? -1 : Convert.ToInt64(row["FC_BCSOrderID"]),
                        FC_FirstProcessNum = row["FC_FirstProcessNum"] is DBNull ? -1 : Convert.ToInt32(row["FC_FirstProcessNum"])
                    });
                }
                datagrid_FlowCard.ItemsSource = fcls;
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
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
                    int count = MyDBController.FindVisualParent<FlowCardReport_Page>(this).Count;
                    if (count > 0)
                    {
                        (MyDBController.FindVisualParent<FlowCardReport_Page>(this)[0]).isNewFlowCard = true;
                    }
                    lastindex = datagrid_FlowCard.SelectedIndex;
                    FlowCardLists fc = (FlowCardLists)datagrid_FlowCard.SelectedItem;
                    Int64 flowCardID = fc.ID;
                    fcsls = FlowCardSubLists.FetchFCS_InfoByFC_Id(flowCardID);
                    TechVersion techVersion = TechVersion.FetchTechVersion(fc.FC_ItemTechVersionID, 1);
                    if (techVersion != null)
                    {
                        fcsls.Sort(new ListComparer<FlowCardSubLists>((p1, p2) => p1.FCS_ProcessSequanece.CompareTo(p2.FCS_ProcessSequanece)));
                        sfc.Invoke(fc, fcsls, techVersion);
                        if (count > 0)
                        {
                            (MyDBController.FindVisualParent<FlowCardReport_Page>(this)[0]).isNewFlowCard = false;
                        }
                    }
                }
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 双击快捷选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_FlowCard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btn_Select_Click(sender, e);
        }
    }
}
