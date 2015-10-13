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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarCodeSystem.TechRoute.TechRouteWorkHourManually
{
    /// <summary>
    /// WorkHour_page.xaml 的交互逻辑
    /// </summary>
    public partial class WorkHour_Page : Page
    {
        public WorkHour_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 工作中心列表
        /// </summary>
        List<WorkCenterLists> wclList;

        /// <summary>
        /// 料品列表
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
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                InitWCGridInfo();
                loadCount++;
            }
        }

        /// <summary>
        /// 根据车间信息，初始化车间信息选择区域信息
        /// </summary>
        private void InitWCGridInfo()
        {
            wclList = WorkCenterLists.FetchWCInfo();
            int count = wclList.Count;
            GridLength gl = new GridLength(Math.Min(grid_WorkCenterSelect.ActualHeight / count, 50));
            if (count > 0)
            {
                foreach (WorkCenterLists item in wclList)
                {
                    RowDefinition rd = new RowDefinition() { Height = gl };
                    grid_WorkCenterSelect.RowDefinitions.Add(rd);
                    Button btn = new Button() { Name = item.department_shortname, Content = item.department_name, Width = 150, Height = 30, Cursor = Cursors.Hand };
                    btn.SetValue(Button.StyleProperty, Application.Current.FindResource("bd_LoginStyle"));
                    btn.Click += OnClick;
                    grid_WorkCenterSelect.Children.Add(btn);
                    Grid.SetRow(btn, grid_WorkCenterSelect.RowDefinitions.Count - 1);
                }
                grid_WorkCenterSelect.RowDefinitions.Add(new RowDefinition());
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("系统中没有车间信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClick(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            Int64 wcID = wclList.Find(p => p.department_shortname.Equals(((Button)sender).Name)).department_id;
            dg_ItemInfo.ItemsSource = null;
            dg_ItemInfo.ItemsSource = iilList = ItemInfoLists.FetchItemInfoByTechAndWC(wcID);
            dg_ItemInfo.Items.Refresh();
            dg_TechVersion.ItemsSource = null;
            dg_TechRouteInfo.ItemsSource = null;
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 搜索料号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_ItemInfo_KeyUp(object sender, KeyEventArgs e)
        {
            btn_ItemSearch_Click(null, null);
        }


        /// <summary>
        /// 搜索料号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ItemSearch_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            string key = txtb_ItemInfo.Text;
            if (!string.IsNullOrEmpty(key))
            {
                dg_ItemInfo.ItemsSource = iilList.FindAll(p => p.II_Code.IndexOf(key) != -1 || p.II_Name.IndexOf(key) != -1 || p.II_Spec.IndexOf(key) != -1);
                dg_ItemInfo.Items.Refresh();
            }
            this.Cursor = Cursors.Arrow;
        }
        /// <summary>
        /// 料品列表双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_ItemInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            Int64 _id = ((ItemInfoLists)dg_ItemInfo.SelectedItem).ID;
            dg_TechVersion.ItemsSource = tvList = TechVersion.FetchTechVersionByItemCode(_id);
            dg_TechVersion.Items.Refresh();
            dg_TechRouteInfo.ItemsSource = null;
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 搜索工艺路线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_TechRouteInfo_KeyUp(object sender, KeyEventArgs e)
        {
            btn_TechRouteSearch_Click(null, null);
        }

        /// <summary>
        /// 搜索工艺路线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TechRouteSearch_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            string key = txtb_TechRouteInfo.Text;
            if (!string.IsNullOrEmpty(key))
            {
                dg_TechVersion.ItemsSource = tvList.FindAll(p => p.TRV_VersionCode.IndexOf(key) != -1 || p.TRV_VersionName.IndexOf(key) != -1);
                dg_TechVersion.Items.Refresh();
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 工艺路线版本列表双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_TechVersion_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (dg_TechVersion.SelectedIndex != -1)
            {
                string versionCode = ((TechVersion)dg_TechVersion.SelectedItem).TRV_VersionCode;
                string itemCode = ((TechVersion)dg_TechVersion.SelectedItem).II_Code;
                dg_TechRouteInfo.ItemsSource = trlList = TechRouteLists.FetchTechRouteByItemCode(itemCode, versionCode);
                dg_TechRouteInfo.Items.Refresh();
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            bool flag = ItemCheck();
            if (flag)
            {
                List<WorkHourLists> whlList = GetPresentWHData();
                flag = DateCheck(whlList);
                if (flag)
                {
                    flag = WorkHourCheck();
                    if (flag)
                    {
                        SaveInfo(whlList);
                    }
                }
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 检查是否有信息可以保存
        /// </summary>
        /// <returns></returns>
        private bool ItemCheck()
        {
            return dg_TechRouteInfo.HasItems;
        }


        /// <summary>
        /// 保存信息
        /// </summary>
        private void SaveInfo(List<WorkHourLists> _whlList)
        {
            if (WorkHourLists.SaveWHInfo(_whlList))
            {
                dg_TechRouteInfo.ItemsSource = null;
                dp_EndDate.SelectedDate = dp_StartDate.SelectedDate = DateTime.Now;
            }
        }

        /// <summary>
        /// 将当前的工时信息转换成列表
        /// </summary>
        /// <returns></returns>
        private List<WorkHourLists> GetPresentWHData()
        {
            List<WorkHourLists> whlList = new List<WorkHourLists>();
            trlList.ForEach(
                p =>
                {
                    WorkHourLists whl = new WorkHourLists();
                    whl.WH_StartDate = Convert.ToDateTime(dp_StartDate.SelectedDate);
                    whl.WH_EndDate = Convert.ToDateTime(dp_EndDate.SelectedDate);
                    whl.WH_TechRouteID = p.ID;
                    whl.WH_WorkHour = p.TR_WorkHour;
                    whl.TR_ProcessName = p.TR_ProcessName;
                    whl.TR_ProcessSequence = p.TR_ProcessSequence;
                    whlList.Add(whl);
                }
                );
            return whlList;
        }

        /// <summary>
        /// 检查生效时间和失效时间
        /// </summary>
        /// <returns></returns>
        private bool DateCheck(List<WorkHourLists> _whlList)
        {
            bool flag = false;
            int result = DateTime.Compare(Convert.ToDateTime(dp_StartDate.SelectedDate), Convert.ToDateTime(dp_EndDate.SelectedDate));
            if (result < 0)
            {
                flag = WorkHourLists.CheckIfDateRight(_whlList);
            }
            else if (result == 0)
            {
                if (Xceed.Wpf.Toolkit.MessageBox.Show("生效时间和失效时间相同，确定要保存吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("失效时间早于生效时间，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return flag;
        }

        /// <summary>
        /// 检查工时是否正确
        /// </summary>
        /// <returns></returns>
        private bool WorkHourCheck()
        {
            bool flag = true;
            string message = "";
            foreach (TechRouteLists trl in trlList)
            {
                if (trl.TR_WorkHour <= 0)
                {
                    message += trl.TR_ProcessName + "\t\t工序工时为0\r\n";
                }
            }
            if (message.Length > 0)
            {
                message += "是否要继续";
                if (Xceed.Wpf.Toolkit.MessageBox.Show(message, "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                }
                else
                {
                    flag = false;
                }
            }
            return flag;
        }

        /// <summary>
        /// 重新填写
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Rewrite_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (ItemCheck())
            {
                trlList.ForEach(p => { p.TR_WorkHour = 0.000M; });
                dg_TechRouteInfo.Items.Refresh();
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 查看历史数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ViewHistory_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (trlList != null && trlList.Count > 0)
            {
                string itemCode = trlList[0].TR_ItemCode;
                string techVersionCode = trlList[0].TRV_VersionCode;
                List<WorkHourLists> whlList = GetPresentWHData();
                WorkHourHistory_Window whh = new WorkHourHistory_Window(itemCode, techVersionCode, whlList);
                whh.ShowDialog();
            }
            this.Cursor = Cursors.Arrow;
        }
    }
}
