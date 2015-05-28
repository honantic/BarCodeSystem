using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            InitFlowCardHeader();
        }


        /// <summary>
        /// 初始化流转卡表头，为Combobox关联items。带入编制人员信息
        /// </summary>
        private void InitFlowCardHeader()
        {
            List<string> typeList = new List<string>() { "普通流转卡", "分批流转卡", "无来源流转卡" };
            cb_FlowCardType.ItemsSource = typeList;
            textb_CreatedBy.Text ="  "+ User_Info.User_Name;
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
            if (!textb_SearchInfo.Text.Equals("料品筛选"))
            {
                Frame frame_SearchInfo = new Frame();
                gb_SearchInfo.Content = frame_SearchInfo;
                textb_SearchInfo.Text = "料品筛选";
                ItemSearch_Page isp = new ItemSearch_Page(SetItemInfo);
                frame_SearchInfo.Navigate(isp);
            }
        }

        /// <summary>
        /// 料品信息查询函数委托实例
        /// </summary>
        /// <returns></returns>
        public string SetItemInfo(string value)
        {
            this.txtb_ItemInfo.Text = value;
            return value;
        }

        /// <summary>
        /// 料品工艺路线信息查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TechRouteSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtb_ItemInfo.Text))
            {
                string itemCode = txtb_ItemInfo.Text.Split('|')[0].Trim();
                if (Regex.IsMatch(itemCode,User_Info.pattern[0]))
                {
                    if (!textb_SearchInfo.Text.Equals("工艺路线筛选"))
                    {
                        Frame frame_SearchInfo = new Frame();
                        gb_SearchInfo.Content = frame_SearchInfo;
                        textb_SearchInfo.Text = "工艺路线筛选";
                        TechRouteSearch_Page trsp = new TechRouteSearch_Page(itemCode, FecthTechRouteInfo);
                        frame_SearchInfo.Navigate(trsp);
                    }
                }
            }
            else
            {

            }
        }

        /// <summary>
        /// 工艺路线查询委托函数实例
        /// </summary>
        /// <param name="value"></param>
        /// <param name="trls"></param>
        private void FecthTechRouteInfo(string value, List<TechRouteLists> trls)
        {
            txtb_TechRouteVersion.Text = value;
            datagrid_TechRouteInfo.ItemsSource = trls;
        }

        /// <summary>
        /// 生产订单查询按钮，只能查询 订单数量>派工数量 的生产订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SourceOrderSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!textb_SearchInfo.Text.Equals("生产订单筛选"))
            {
                Frame frame_SearchInfo = new Frame();
                gb_SearchInfo.Content = frame_SearchInfo;
                textb_SearchInfo.Text = "生产订单筛选";
                ItemSearch_Page isp = new ItemSearch_Page(SetItemInfo);
                frame_SearchInfo.Navigate(isp);
            }
        }


        #endregion





    }
}
