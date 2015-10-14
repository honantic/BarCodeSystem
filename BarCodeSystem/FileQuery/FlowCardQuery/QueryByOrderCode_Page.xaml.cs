using BarCodeSystem.ProductDispatch.FlowCard;
using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.HelperClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Linq;
using BarCodeSystem.PublicClass;

namespace BarCodeSystem.FileQuery.FlowCardQuery
{
    /// <summary>
    /// QueryByOrderCode_Page.xaml 的交互逻辑
    /// </summary>
    public partial class QueryByOrderCode_Page : Page
    {
        public QueryByOrderCode_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_sfc"></param>
        public QueryByOrderCode_Page(SubmitFlowCard _sfc)
        {
            InitializeComponent();
            sfc = _sfc;
        }

        /// <summary>
        /// 查询流转卡的委托
        /// </summary>
        SubmitFlowCard sfc;

        int loadCount = 0;
        int lastIndex = -1;
        List<FlowCardSubLists> fcsls;
        FlowCardLists fc;
        TechVersion tv;
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                text_Content.Text = "<-返回";
                txtb_SourceOrderCode.Focus();
                loadCount++;
            }
        }

        /// <summary>
        /// 快捷搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_SourceOrderCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_FlowCardSearch_Click(sender, e);
            }
        }

        /// <summary>
        /// 搜索流转卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FlowCardSearch_Click(object sender, RoutedEventArgs e)
        {
            bool flag = ProduceOrderLists.CheckForCode(txtb_SourceOrderCode.Text);
            if (flag)
            {
                FetchFLowCardInfo(txtb_SourceOrderCode.Text);
                lastIndex = -1;
            }
        }

        /// <summary>
        /// 根据生产订单号搜索流转卡
        /// </summary>
        /// <param name="_orderCode"></param>
        private void FetchFLowCardInfo(string _orderCode)
        {
            datagrid_FlowCardList.ItemsSource = FlowCardLists.FetchFC_InfoByOrderCode(_orderCode);
            if (datagrid_FlowCardList.Items.Count == 1)
            {
                datagrid_FlowCardList.SelectedIndex = 0;
                btn_Select_Click(null, null);
            }
            else if (datagrid_FlowCardList.Items.Count == 0)
            {
                MyDBController.FindVisualParent<FlowCardQuery_Page>(this).ForEach(p => p.ClearInfo());
            }
        }

        /// <summary>
        /// 退回到选择界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            List<QueryWaySelect_Page> qwsl = MyDBController.FindVisualParent<QueryWaySelect_Page>(this);
            if (qwsl.Count > 0)
            {
                qwsl[0].frame_QueryWay.GoBack();
            }
            List<FlowCardQuery_Page> fcql = MyDBController.FindVisualParent<FlowCardQuery_Page>(this);
            if (fcql.Count > 0)
            {
                fcql[0].ClearInfo();
            }
        }

        /// <summary>
        /// 双击选定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_FlowCardList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btn_Select_Click(sender, e);
        }

        /// <summary>
        /// 选定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Select_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (datagrid_FlowCardList.SelectedIndex != -1 && datagrid_FlowCardList.SelectedIndex != lastIndex)
            {
                fc = (FlowCardLists)datagrid_FlowCardList.SelectedItem;
                fcsls = FlowCardSubLists.FetchFCS_InfoByFC(fc);
                fcsls.Sort(new ListComparer<FlowCardSubLists>((p1, p2) => p1.FCS_ProcessSequanece.CompareTo(p2.FCS_ProcessSequanece)));
                tv = TechVersion.FetchTechVersion(fc.FC_ItemTechVersionID, 1);
                sfc.Invoke(fc, fcsls, tv);
                lastIndex = datagrid_FlowCardList.SelectedIndex;
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 选中项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_FlowCardList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            datagrid_FlowCardList_MouseDoubleClick(null, null);
        }
    }
}
