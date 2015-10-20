using BarCodeSystem.PublicClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using BarCodeSystem.PublicClass.HelperClass;
using System.Threading;
using BarCodeSystem.SystemManage;

namespace BarCodeSystem.ProductDispatch.FlowCard
{
    /// <summary>
    /// ProduceOrderSearch_Page.xaml 的交互逻辑
    /// </summary>
    public partial class ProduceOrderSearch_Page : Page
    {

        public ProduceOrderSearch_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 搜索生产订单的构造函数
        /// </summary>
        /// <param name="_spoi"></param>
        public ProduceOrderSearch_Page(SubmitProduceOrderInfo _spoi)
        {
            InitializeComponent();
            spoi = _spoi;
        }
        #region 变量
        SubmitProduceOrderInfo spoi;
        List<ProduceOrderLists> pols = new List<ProduceOrderLists>();
        DataSet ds = new DataSet();
        int loadCount = 0;
        delegate void DeleFunc();
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
                OrderInfo();
                this.Cursor = Cursors.Arrow;
                loadCount++;
            }
        }

        /// <summary>
        /// 任务继续
        /// </summary>
        private void OrderInfo()
        {
            try
            {
                datagrid_ProduceOrderInfo.ItemsSource = pols = ProduceOrderLists.FetchProduceOrderInfo();
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            datagrid_ProduceOrderInfo.Items.Refresh();
        }


        /// <summary>
        /// 提交按钮提交选定的生产订单信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            if (spoi != null && datagrid_ProduceOrderInfo.SelectedItem != null)
            {
                spoi.Invoke((ProduceOrderLists)datagrid_ProduceOrderInfo.SelectedItem);
            }
        }



        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            datagrid_ProduceOrderInfo.ItemsSource = ProduceOrderLists.FetchProduceOrderInfo();
            datagrid_ProduceOrderInfo.Items.Refresh();
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ProduceOrderSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtb_ProduceOrderInfo.Text))
            {
                datagrid_ProduceOrderInfo.ItemsSource = pols;
                datagrid_ProduceOrderInfo.Items.Refresh();
            }
            else
            {
                string key = txtb_ProduceOrderInfo.Text;
                datagrid_ProduceOrderInfo.ItemsSource = pols.FindAll(p => p.PO_Code.IndexOf(key) != -1 || p.PO_ItemCode.IndexOf(key) != -1 || p.PO_ItemName.IndexOf(key) != -1 || p.PO_ItemSpec.IndexOf(key) != -1 || p.PO_OrderAmount.ToString().IndexOf(key) != -1 || p.PO_StartAmount.ToString().IndexOf(key) != -1);
                datagrid_ProduceOrderInfo.Items.Refresh();
            }
        }
        /// <summary>
        /// 回车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_ProduceOrderInfo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_ProduceOrderSearch_Click(sender, e);
            }
        }

        /// <summary>
        /// 快捷选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_ProduceOrderInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (datagrid_ProduceOrderInfo.SelectedIndex != -1)
            {
                btn_Submit_Click(null, null);
            }
        }

    }
}
