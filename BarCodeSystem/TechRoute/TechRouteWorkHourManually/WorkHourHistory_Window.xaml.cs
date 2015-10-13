using BarCodeSystem.PublicClass.DatabaseEntity;
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
using System.Windows.Shapes;

namespace BarCodeSystem.TechRoute.TechRouteWorkHourManually
{
    /// <summary>
    /// WorkHourHistory_Window.xaml 的交互逻辑
    /// </summary>
    public partial class WorkHourHistory_Window : Window
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public WorkHourHistory_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取工时的构造函数
        /// </summary>
        /// <param name="_itemCode"></param>
        /// <param name="_techVersionCode"></param>
        public WorkHourHistory_Window(string _itemCode, string _techVersionCode, List<WorkHourLists> _whlList)
        {
            InitializeComponent();
            itemCode = _itemCode;
            techVersionCode = _techVersionCode;
            whlList = _whlList;
        }
        /// <summary>
        /// 料品编码
        /// </summary>
        string itemCode;
        /// <summary>
        /// 工艺路线版本编码
        /// </summary>
        string techVersionCode;
        /// <summary>
        /// 当前输入的工时列表
        /// </summary>
        List<WorkHourLists> whlList;
        /// <summary>
        /// 工时的历史数据
        /// </summary>
        List<WorkHourLists> whlHistoryList;
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(itemCode) && !string.IsNullOrEmpty(techVersionCode))
            {
                FetchHistoryData(itemCode, techVersionCode);
            }
            if (whlList != null && whlList.Count > 0)
            {
                dg_PresentData.ItemsSource = whlList;
            }
        }

        /// <summary>
        /// 获取工时的历史数据
        /// </summary>
        /// <param name="_itemCode"></param>
        /// <param name="_techVersionCode"></param>
        private void FetchHistoryData(string _itemCode, string _techVersionCode)
        {
            dg_HistoryData.ItemsSource = whlHistoryList = WorkHourLists.FetchWHInfo(_itemCode, _techVersionCode);
        }

        /// <summary>
        /// 拖拽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// 在历史数据中搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SearchInHistory_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            dg_PresentData.ItemsSource = whlList;
            dg_PresentData.Items.Refresh();
            dg_HistoryData.ItemsSource = SearchDataOnCondition(whlHistoryList);
            dg_HistoryData.Items.Refresh();
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 在全部中搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SearchInAll_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            dg_PresentData.ItemsSource = SearchDataOnCondition(whlList);
            dg_PresentData.Items.Refresh();
            dg_HistoryData.ItemsSource = SearchDataOnCondition(whlHistoryList);
            dg_HistoryData.Items.Refresh();
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 根据输入的搜索条件返回搜索结果
        /// </summary>
        /// <returns></returns>
        private List<WorkHourLists> SearchDataOnCondition(List<WorkHourLists> sourceList)
        {
            List<WorkHourLists> newwhlList = new List<WorkHourLists>();
            if (!string.IsNullOrEmpty(txtb_SearchProcess.Text))
            {
                newwhlList = sourceList.FindAll(p => p.TR_ProcessName.IndexOf(txtb_SearchProcess.Text) != -1);
            }
            else
            {
                newwhlList = sourceList;
            }
            if (dp_StartDateAfter.SelectedDate != null)
            {
                newwhlList = newwhlList.FindAll(p => DateTime.Compare(Convert.ToDateTime(dp_StartDateAfter.SelectedDate), p.WH_StartDate) <= 0);
            }
            if (dp_StartDateBefore.SelectedDate != null)
            {
                newwhlList = newwhlList.FindAll(p => DateTime.Compare(Convert.ToDateTime(dp_StartDateBefore.SelectedDate), p.WH_StartDate) >= 0);
            }
            if (dp_EndtDateAfter.SelectedDate != null)
            {
                newwhlList = newwhlList.FindAll(p => DateTime.Compare(Convert.ToDateTime(dp_EndtDateAfter.SelectedDate), p.WH_EndDate) <= 0);
            }
            if (dp_EndDateBefore.SelectedDate != null)
            {
                newwhlList.FindAll(p => DateTime.Compare(Convert.ToDateTime(dp_EndDateBefore.SelectedDate), p.WH_EndDate) >= 0);
            }
            return newwhlList;
        }

        /// <summary>
        /// 重写
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReWrite_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            txtb_SearchProcess.Text = "";
            dp_EndDateBefore.SelectedDate = dp_EndtDateAfter.SelectedDate = dp_StartDateAfter.SelectedDate = dp_StartDateBefore.SelectedDate = null;
            this.Cursor = Cursors.Arrow;
        }
    }
}
