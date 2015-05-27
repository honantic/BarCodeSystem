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

        #region 变量设置
        /// <summary>
        /// 获取料品信息的函数，返回信息为 料号+名称+规格
        /// 委托在ItemSearch_Page中执行，返回值填在表头的产品信息中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        delegate string FetchItemInfo();
        #endregion

        #region 初始化设置
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
            //CardHeaderGrid.Width = Math.Max((this.ActualWidth) * 0.8, 600);
        }
        #endregion

        #region 流转卡表头操作
        /// <summary>
        /// 料品信息查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ItemSearch_Click(object sender, RoutedEventArgs e)
        {
            List<Page> li = MyDBController.FindVisualChild<Page>(frame_SearchInfo);
            if (li.Count == 0)
            {
                textb_SearchInfo.Text = "料品筛选";
                ItemSearch_Page isp = new ItemSearch_Page(SetItemInfo);
                frame_SearchInfo.Navigate(isp);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SetItemInfo(string value)
        {
            this.txtb_ItemInfo.Text = value;
            return value;
        }
        #endregion
    }
}
