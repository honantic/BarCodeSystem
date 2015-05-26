using System;
using System.Collections.Generic;
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
    /// FlowCrad_Page.xaml 的交互逻辑
    /// </summary>
    public partial class FlowCard_Page : Page
    {
        public FlowCard_Page()
        {
            InitializeComponent();
        }

        private void btn_ItemSearch_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("?");
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitFlowCardType();
        }


        /// <summary>
        /// 初始化流转卡类型，为Combobox关联items
        /// </summary>
        private void InitFlowCardType()
        {
            List<string> typeList = new List<string>() { "普通流转卡", "分批流转卡", "无来源流转卡" };
            cb_FlowCardType.ItemsSource = typeList;
        }

        /// <summary>
        /// 页面大小改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CardHeaderGrid.Width = Math.Max((this.ActualWidth) * 0.8, 600);
        }


    }
}
