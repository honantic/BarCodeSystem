using BarCodeSystem.ProductDispatch.FlowCard;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BarCodeSystem.FileQuery.FlowCardQuery
{
    /// <summary>
    /// QueryWaySelect_Page.xaml 的交互逻辑
    /// </summary>
    public partial class QueryWaySelect_Page : Page
    {
        public QueryWaySelect_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public QueryWaySelect_Page(SubmitFlowCard _sfc)
        {
            InitializeComponent();
            sfc = _sfc;
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                if (User_Info.P_Position == "检验员")
                {
                }
                else
                {
                    btn_CheckFlowCard.Visibility = Visibility.Hidden;
                    btn_ModifyFlowCard.Visibility = Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// 加载次数
        /// </summary>
        int loadCount = 0;

        /// <summary>
        /// 接收流转卡的委托
        /// </summary>
        SubmitFlowCard sfc;
        /// <summary>
        /// 按照流转卡编号筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_QueryByFlowCode_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            frame_QueryWay.Navigate(new QueryByFlowCardCode_Page(sfc));
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 按照料号筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_QueryByItemCode_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            frame_QueryWay.Navigate(new QueryByItemCode_Page(sfc));
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 按照生产订单编号筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_QueryByOrderCode_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            frame_QueryWay.Navigate(new QueryByOrderCode_Page(sfc));
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 按照日期筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_QueryByDate_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            frame_QueryWay.Navigate(new QueryByDate_Page(sfc));
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 修改流转卡数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ModifyFlowCard_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            frame_QueryWay.Navigate(new FlowCardModify_Page(sfc));
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 审核流转卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CheckFlowCard_Click(object sender, RoutedEventArgs e)
        {

            this.Cursor = Cursors.Wait;
            frame_QueryWay.Navigate(new FlowCardCheck_Page(sfc));
            this.Cursor = Cursors.Arrow;
        }


    }
}
