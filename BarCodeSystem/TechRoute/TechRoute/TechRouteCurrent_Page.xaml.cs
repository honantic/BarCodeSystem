using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace BarCodeSystem.TechRoute.TechRoute
{
    /// <summary>
    /// Interaction logic for TechRouteCurrent_Page.xaml
    /// </summary>
    public partial class TechRouteCurrent_Page : Page
    {
        public TechRouteCurrent_Page()
        {
            InitializeComponent();
        }
        #region 变量
        /// <summary>
        /// 拥有工艺路线的料品信息列表
        /// </summary>
        List<ItemInfoLists> iilList;

        /// <summary>
        /// 工艺路线列表
        /// </summary>
        List<TechRouteLists> trlList;

        /// <summary>
        /// 工艺路线版本列表
        /// </summary>
        List<TechVersion> tvList;

        /// <summary>
        /// 加载次数
        /// </summary>
        int loadCount = 0;
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
        /// 获取料品信息
        /// </summary>
        public void FetchItemInfo(params string[] _wcCode)
        {
            iilList = ItemInfoLists.FetchItemInfoByTechAndWCCode(_wcCode);
            dg_ItemInfo.ItemsSource = iilList;
            dg_ItemInfo.Items.Refresh();
            dg_TechRouteInfo.ItemsSource = null;
            dg_TechVersionInfo.ItemsSource = null;
            if (iilList.Count == 1)
            {
                dg_ItemInfo.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 快捷搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_ItemInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_ItemInfo_Click(null, null);
            }
        }

        /// <summary>
        /// 搜索料品信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ItemInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (iilList != null)
            {
                dg_ItemInfo.ItemsSource = iilList.FindAll(p => p.II_Name.IndexOf(tb_ItemInfo.Text) != -1 || p.II_Code.IndexOf(tb_ItemInfo.Text) != -1);
                dg_ItemInfo.Items.Refresh();
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 料品列表双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_ItemInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (dg_ItemInfo.SelectedIndex != -1)
            {
                DisplayTVInfoByIIInfo((ItemInfoLists)dg_ItemInfo.SelectedItem);
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 料品列表选中项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_ItemInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dg_ItemInfo_MouseDoubleClick(null, null);
        }

        /// <summary>
        /// 根据选中的料品信息，展示其工艺路线版本信息
        /// </summary>
        /// <param name="iil"></param>
        private void DisplayTVInfoByIIInfo(ItemInfoLists iil)
        {
            if (iil != null)
            {
                FetchTechVersionInfo(iil);
                dg_TechVersionInfo.ItemsSource = tvList;
                dg_TechVersionInfo.Items.Refresh();
                dg_TechRouteInfo.ItemsSource = null;
                dg_TechRouteInfo.Items.Refresh();
                if (tvList.Count == 1)
                {
                    dg_TechVersionInfo.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// 工艺版本表格加载行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_TechVersionInfo_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            TechVersion tv = (TechVersion)e.Row.DataContext;
            if (tv.TRV_HasFlowCard)
            {
                //e.Row.Background = Brushes.Green;
            }
            else
            {
                e.Row.Background = Brushes.Salmon;
            }
        }


        /// <summary>
        /// 获取工艺路线版本信息
        /// </summary>
        private void FetchTechVersionInfo(ItemInfoLists _iil)
        {
            tvList = TechVersion.FetchTechVersionByItemCode(_iil.II_Code);
        }


        /// <summary>
        /// 工艺路线版本列表双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_TechVersionInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dg_TechVersionInfo.SelectedIndex != -1)
            {
                DisplayTRInfoByTVInfo((TechVersion)dg_TechVersionInfo.SelectedItem);
            }
        }

        /// <summary>
        /// 工艺路线版本列表选中项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_TechVersionInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dg_TechVersionInfo_MouseDoubleClick(null, null);
        }

        /// <summary>
        /// 根据选中的版本信息展示工艺路线详细信息
        /// </summary>
        /// <param name="tv"></param>
        private void DisplayTRInfoByTVInfo(TechVersion tv)
        {
            if (tv != null)
            {
                FetchTechRouteInfo(tv);
                dg_TechRouteInfo.ItemsSource = trlList;
                dg_TechRouteInfo.Items.Refresh();
            }
        }

        /// <summary>
        /// 获取工艺路线信息
        /// </summary>
        private void FetchTechRouteInfo(TechVersion _tv)
        {
            trlList = TechRouteLists.FetchTechRouteByVersion(_tv);
        }

        /// <summary>
        /// 新增工艺版本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddNewVersion_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (dg_ItemInfo.SelectedIndex != -1)
            {
                TechRouteNew_Page trn = new TechRouteNew_Page((ItemInfoLists)dg_ItemInfo.SelectedItem);
                MyDBController.FindVisualParent<TechRoute_Page>(this).ForEach(p => p.frame_TechInfoFrame.Navigate(trn));
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 新增料品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddNewItem_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            MyDBController.FindVisualParent<TechRoute_Page>(this).ForEach(p => p.frame_TechInfoFrame.Content = new TechRouteNew_Page());
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 修改版本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ModifyTechRoute_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (dg_ItemInfo.SelectedIndex != -1 && dg_TechVersionInfo.SelectedIndex != -1 && dg_TechRouteInfo.HasItems)
            {
                TechRouteNew_Page trn = new TechRouteNew_Page((ItemInfoLists)dg_ItemInfo.SelectedItem, (TechVersion)dg_TechVersionInfo.SelectedItem, trlList);
                MyDBController.FindVisualParent<TechRoute_Page>(this).ForEach(p => p.frame_TechInfoFrame.Navigate(trn));
            }
            this.Cursor = Cursors.Arrow;
        }
    }
}
