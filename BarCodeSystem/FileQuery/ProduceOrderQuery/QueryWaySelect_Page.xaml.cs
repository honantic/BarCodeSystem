using BarCodeSystem.ProductDispatch.FlowCard;
using BarCodeSystem.PublicClass;
using BarCodeSystem.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarCodeSystem.FileQuery.ProduceOrderQuery
{
    /// <summary>
    /// QueryWaySelect_Page.xaml 的交互逻辑
    /// </summary>
    public partial class QueryWaySelect_Page : Page
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public QueryWaySelect_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 查询生产订单的构造函数
        /// </summary>
        /// <param name="_spol"></param>
        public QueryWaySelect_Page(SubmitProduceOrderList _spol)
        {
            InitializeComponent();
            spol = _spol;
        }


        #region  变量
        /// <summary>
        /// 加载次数
        /// </summary>
        private int loadCount = 0;

        /// <summary>
        /// 查询生产订单的委托函数
        /// </summary>
        SubmitProduceOrderList spol;
        #endregion
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                loadCount++;
            }
        }

        /// <summary>
        /// 按照车间查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_QueryByWorkCenter_Click(object sender, RoutedEventArgs e)
        {
            frame_QueryWay.Navigate(new QueryByWorkCenter_Page(spol));
        }

        /// <summary>
        /// 按照订单号查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_QueryByOrderCode_Click(object sender, RoutedEventArgs e)
        {
            frame_QueryWay.Navigate(new QueryByPOCode_Page(spol));
        }

        /// <summary>
        /// 按照料号搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_QueryByItemCode_Click(object sender, RoutedEventArgs e)
        {
            frame_QueryWay.Navigate(new QueryByItemCode_Page(spol));
        }

        /// <summary>
        /// 按照流转卡编号搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_QueryByFlowCode_Click(object sender, RoutedEventArgs e)
        {
            frame_QueryWay.Navigate(new QueryByFCCode_Page(spol));
        }

        /// <summary>
        /// 修改生产订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ModifyProduceOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 从U9系统中同步生产订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FetchPOFromU9_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            Thread t3 = new Thread(() => { Loading_Window lw = new Loading_Window("正在同步U9生产订单数据，这可能需要一点时间，请稍等") { }; lw.ShowDialog(); }) { };
            t3.SetApartmentState(ApartmentState.STA);
            t3.Start();
            TaskFactory tf = new TaskFactory();
            Task t1 = tf.StartNew(ProduceOrderLists.FetchPOFromU9);
            while (!t1.IsCompleted)
            {

            }
            t3.Abort();
            Xceed.Wpf.Toolkit.MessageBox.Show("同步成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Cursor = Cursors.Wait;
        }
    }
}
