using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.ValueConverters;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using BarCodeSystem.PublicClass.HelperClass;
using System.Data;
using System;
using BarCodeSystem.ProductDispatch.FlowCardPrint;
using System.Windows;
using System.Windows.Input;

namespace BarCodeSystem.FileQuery.FlowCardQuery
{
    /// <summary>
    /// FlowCardQuery_Page.xaml 的交互逻辑
    /// </summary>
    public partial class FlowCardQuery_Page : Page
    {
        public FlowCardQuery_Page()
        {
            InitializeComponent();
        }
        #region 变量
        /// <summary>
        /// 加载次数
        /// </summary>
        int loadCount = 0;

        /// <summary>
        /// 流转卡信息
        /// </summary>
        FlowCardLists fc;

        /// <summary>
        /// 流转卡行表信息
        /// </summary>
        List<FlowCardSubLists> fcsls;

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        TechVersion tv;
        #endregion
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                frame_SelectWay.Navigate(new QueryWaySelect_Page(ShowFCInfo));
                loadCount++;
                btn_ModifyData.Visibility = Visibility.Hidden;
                btn_Check.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 展示选中的流转卡相关信息
        /// </summary>
        /// <param name="_fc"></param>
        /// <param name="_fcslist"></param>
        /// <param name="_tv"></param>
        private void ShowFCInfo(FlowCardLists _fc, List<FlowCardSubLists> _fcslist, TechVersion _tv)
        {
            #region 变量
            fc = _fc;
            fcsls = _fcslist;
            tv = _tv;
            #endregion

            #region 主表信息
            txtb_Amount.Text = _fc.FC_Amount.ToString();
            txtb_CardState.Text = (new FlowCardStateConverter()).Convert(_fc.FC_CardState, typeof(string), null, new System.Globalization.CultureInfo("")).ToString();
            txtb_CreateBy.Text = _fc.FC_CreateBy;
            txtb_CreateOn.Text = _fc.FC_CreateTime.ToString();
            txtb_FlowCardCode.Text = _fc.FC_Code;
            txtb_FlowCardType.Text = (new FlowCardTypeConverter()).Convert(_fc.FC_CardType, typeof(string), null, new System.Globalization.CultureInfo("")).ToString();
            txtb_ItemInfo.Text = _fc.PO_ItemCode + " | " + _fc.PO_ItemName + " | " + _fc.PO_ItemSpec;
            txtb_SourceOrderCode.Text = _fc.PO_Code;
            txtb_WorkCenter.Text = _fc.WC_Department_Name;
            txtb_VersionCode.Text = _fc.TRV_VersionName;
            #endregion

            #region 行表信息
            ShowFCSInfo(_fcslist);
            FetchQualityInfo(_fc);
            #endregion

            #region 工艺路线信息
            datagrid_TechRoute.ItemsSource = FetchTechInfo(_tv);
            #endregion
        }

        /// <summary>
        /// 行表信息
        /// </summary>
        /// <param name="_fcslist"></param>
        private void ShowFCSInfo(List<FlowCardSubLists> _fcslist)
        {
            List<FlowCardSubLists> newfcslist = new List<FlowCardSubLists>();
            newfcslist = MyDBController.ListCopy<FlowCardSubLists>(_fcslist).Distinct(new ListComparer<FlowCardSubLists>((x, y) => x.FCS_TechRouteID.Equals(y.FCS_TechRouteID))).ToList();

            foreach (FlowCardSubLists item in newfcslist)
            {
                foreach (FlowCardSubLists fcsl in _fcslist.FindAll(p => p.FCS_TechRouteID.Equals(item.FCS_TechRouteID) && !p.FCS_PersonCode.Equals(item.FCS_PersonCode)))
                {
                    item.FCS_PersonName += string.IsNullOrEmpty(item.FCS_PersonName) ? fcsl.FCS_PersonName : "、" + fcsl.FCS_PersonName;
                }
            }
            datagrid_FlowCardSub.ItemsSource = newfcslist;
        }


        /// <summary>
        /// 获取当前的流转卡的质量信息
        /// </summary>
        /// <param name="_fc"></param>
        private void FetchQualityInfo(FlowCardLists _fc)
        {
            datagrid_QualityInfo.ItemsSource = FlowCardQualityLists.FetchFCQByFlowCardInfo(_fc);
            datagrid_QualityInfo.Items.Refresh();
        }


        /// <summary>
        /// 获取工艺路线信息
        /// </summary>
        /// <param name="_tv"></param>
        /// <returns></returns>
        private List<TechRouteLists> FetchTechInfo(TechVersion _tv)
        {
            List<TechRouteLists> trls = new List<TechRouteLists>();
            if (_tv != null)
            {
                DataSet ds = new DataSet();
                string SQl = string.Format(@"Select * from [TechRoute] where [TR_VersionID]={0}", _tv.ID);
                MyDBController.GetConnection();
                MyDBController.GetDataSet(SQl, ds, "TechRoute");
                MyDBController.CloseConnection();
                foreach (DataRow row in ds.Tables["TechRoute"].Rows)
                {
                    try
                    {
                        trls.Add(new TechRouteLists()
                        {
                            TR_ProcessSequence = Convert.ToInt32(row["TR_ProcessSequence"]),
                            TR_ProcessName = row["TR_ProcessName"].ToString(),
                            TR_IsFirstProcess = Convert.ToBoolean(row["TR_IsFirstProcess"]),
                            TR_IsLastProcess = Convert.ToBoolean(row["TR_IsLastProcess"]),
                            TR_IsBackProcess = Convert.ToBoolean(row["TR_IsBackProcess"]),
                            TR_IsTestProcess = Convert.ToBoolean(row["TR_IsTestProcess"]),
                            TR_DefaultCheckPersonName = row["TR_DefaultCheckPersonName"].ToString()
                        }
                        );
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            return trls;
        }

        /// <summary>
        /// 清除现有展示的信息
        /// </summary>
        public void ClearInfo()
        {
            datagrid_FlowCardSub.ItemsSource = null;
            datagrid_TechRoute.ItemsSource = null;
            datagrid_QualityInfo.ItemsSource = null;
            fc = null;
            fcsls = null;
            tv = null;
            MyDBController.FindVisualChild<TextBox>(gb_FCInfo).ForEach(p => p.Text = "");
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Print_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtb_FlowCardCode.Text))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请先选择流转卡信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _10LinesFlowCard_Window _10lfc = new _10LinesFlowCard_Window(txtb_FlowCardCode.Text);
                _10lfc.ShowDialog();
            }
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ModifyData_Click(object sender, RoutedEventArgs e)
        {
            if (fc != null && fcsls != null)
            {
                ModifyContentSelect_Window mcs = new ModifyContentSelect_Window(fc, fcsls, ShowFCSInfo, ShowFCInfo, tv);
                mcs.ShowDialog();
            }
        }

        /// <summary>
        /// 审核流转卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Check_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            bool flag = true;
            MessageBoxResult mbs = Xceed.Wpf.Toolkit.MessageBox.Show("是否需要生成U9完工报告？", "提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (mbs != MessageBoxResult.Cancel)
            {
                if (mbs == MessageBoxResult.Yes)
                {
                    int qualifiedAmount = fcsls.LastOrDefault().FCS_QulifiedAmount;
                    Int64 orderID = fc.FC_SourceOrderID;
                    flag = FlowCardLists.CreateU9POFinishReprot(qualifiedAmount, orderID);
                }
                if (flag)
                {
                    fc.FC_CardState = 5;
                    List<FlowCardLists> _fcList = new List<FlowCardLists>() { fc };
                    FlowCardLists.UpdateFCInfo(_fcList);
                    this.ClearInfo();
                    MyDBController.FindVisualChild<FLowCardCheck_Page>(this).ForEach(p => p.FetchFlowCardInfo());
                }
            }
            this.Cursor = Cursors.Arrow;
        }
    }
}
