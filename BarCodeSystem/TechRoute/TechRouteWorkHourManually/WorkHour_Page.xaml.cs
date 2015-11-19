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
        #region 变量
        /// <summary>
        /// 工作中心列表
        /// </summary>
        List<WorkCenterLists> wclList;

        /// <summary>
        /// 料品列表
        /// </summary>
        List<ItemInfoLists> iilList;

        /// <summary>
        /// 最新工时列表
        /// </summary>
        List<WorkHourLists> whlList;


        /// <summary>
        /// 新增的工时列表
        /// </summary>
        List<WorkHourLists> tempWHLList;

        /// <summary>
        /// 工艺路线版本列表
        /// </summary>
        List<TechVersion> tvList;

        /// <summary>
        /// 工时的历史数据
        /// </summary>
        List<WorkHourLists> whlHistoryList;

        /// <summary>
        /// 加载次数
        /// </summary>
        int loadCount = 0;

        /// <summary>
        /// 是否保存了更改的数据
        /// </summary>
        private bool haveSaved = true;

        /// <summary>
        /// 当前工艺路线是否有工时信息
        /// </summary>
        private bool haveWHInfo = true;
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
                InitWCGridInfo();
                SetReadOnlyProperties(false);
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
        /// 改变只读属性、可见性
        /// </summary>
        /// <param name="_flag"></param>
        private void SetReadOnlyProperties(bool _flag)
        {
            dg_TechRouteInfo.IsReadOnly = !_flag;
            dp_EndDate.IsEnabled = dp_StartDate.IsEnabled = _flag;
            btn_Cancel.Visibility = btn_Save.Visibility = btn_Rewrite.Visibility = _flag ? Visibility.Visible : Visibility.Hidden;
            haveSaved = !_flag;

        }
        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClick(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            bool flag = true;
            if (!haveSaved)
            {
                MessageBoxResult mbs = Xceed.Wpf.Toolkit.MessageBox.Show("新的工时信息没有保存，是否要放弃？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mbs != MessageBoxResult.Yes)
                {
                    flag = false;
                }
            }
            if (flag)
            {
                Int64 wcID = wclList.Find(p => p.department_shortname.Equals(((Button)sender).Name)).department_id;
                dg_ItemInfo.ItemsSource = null;
                dg_ItemInfo.ItemsSource = iilList = ItemInfoLists.FetchItemInfoByTechAndWCID(wcID);
                dg_ItemInfo.Items.Refresh();
                dg_TechVersion.ItemsSource = null;
                dg_TechRouteInfo.ItemsSource = null;
                ClearWHInfo();
                SetReadOnlyProperties(false);
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 每次更改选中内容的时候，清除工时信息
        /// </summary>
        private void ClearWHInfo()
        {
            if (tempWHLList != null)
            {
                tempWHLList.Clear();
            }
            if (whlHistoryList != null)
            {
                whlHistoryList.Clear();
            }
            if (whlList != null)
            {
                whlList.Clear();
            }
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
            if (dg_ItemInfo.SelectedIndex != -1)
            {
                bool flag = true;
                if (!haveSaved)
                {
                    MessageBoxResult mbs = Xceed.Wpf.Toolkit.MessageBox.Show("新的工时信息没有保存，是否要放弃？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (mbs != MessageBoxResult.Yes)
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    Int64 _id = ((ItemInfoLists)dg_ItemInfo.SelectedItem).ID;
                    dg_TechVersion.ItemsSource = tvList = TechVersion.FetchTechVersionByItemID(_id);
                    dg_TechVersion.Items.Refresh();
                    dg_TechRouteInfo.ItemsSource = null;
                    ClearWHInfo();
                    SetReadOnlyProperties(false);
                }
            }
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
                bool flag = true;
                if (!haveSaved)
                {
                    MessageBoxResult mbs = Xceed.Wpf.Toolkit.MessageBox.Show("新的工时信息没有保存，是否要放弃？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (mbs != MessageBoxResult.Yes)
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    SetReadOnlyProperties(false);
                    string versionCode = ((TechVersion)dg_TechVersion.SelectedItem).TRV_VersionCode;
                    string itemCode = ((TechVersion)dg_TechVersion.SelectedItem).II_Code;
                    whlHistoryList = WorkHourLists.FetchWHInfo(itemCode, versionCode, out haveWHInfo);
                    whlList = whlHistoryList.FindAll(p => p.WH_EndDate.Date.Equals(whlHistoryList.Max(item => item.WH_EndDate.Date)));
                    ShowWHInfo(whlList);
                }
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 展示工时信息
        /// </summary>
        /// <param name="_list"></param>
        private void ShowWHInfo(List<WorkHourLists> _list)
        {
            dg_TechRouteInfo.ItemsSource = _list;
            dg_TechRouteInfo.Items.Refresh();
            if (_list.Count > 0)
            {
                dp_EndDate.SelectedDate = _list.First().WH_EndDate;
                dp_StartDate.SelectedDate = _list.FirstOrDefault().WH_StartDate;
            }

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
                flag = DateCheck((List<WorkHourLists>)dg_TechRouteInfo.ItemsSource);
                if (flag)
                {
                    flag = WorkHourCheck();
                    if (flag)
                    {
                        SaveInfo((List<WorkHourLists>)dg_TechRouteInfo.ItemsSource);
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
                SetReadOnlyProperties(false);
                if (tempWHLList != null)
                {
                    tempWHLList.Clear();
                }
                dg_TechVersion_MouseDoubleClick(null, null);
            }
        }

        /// <summary>
        /// 点击新增工时信息的时候，生成新的工时信息的列表
        /// </summary>
        /// <returns></returns>
        private List<WorkHourLists> GenerateNewWHData()
        {
            List<WorkHourLists> _list = new List<WorkHourLists>();
            whlList.ForEach(
                p =>
                {
                    WorkHourLists whl = new WorkHourLists();
                    whl.WH_StartDate = Convert.ToDateTime(dp_EndDate.SelectedDate).AddDays(1);
                    whl.WH_EndDate = whl.WH_StartDate.AddYears(1);
                    whl.WH_TechRouteID = p.WH_TechRouteID;
                    whl.WH_WorkHour = p.WH_WorkHour;
                    whl.TR_ProcessName = p.TR_ProcessName;
                    whl.TR_ProcessSequence = p.TR_ProcessSequence;
                    whl.WC_Department_Name = p.WC_Department_Name;
                    whl.TRV_VersionCode = p.TRV_VersionCode;
                    whl.TRV_VersionName = p.TRV_VersionName;
                    whl.TR_DefaultCheckPersonName = p.TR_DefaultCheckPersonName;
                    whl.TR_ItemCode = p.TR_ItemCode;
                    whl.TR_ItemID = p.TR_ItemID;
                    whl.TR_ProcessCode = p.TR_ProcessCode;
                    _list.Add(whl);
                }
                );
            return _list;
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
            foreach (WorkHourLists trl in whlList)
            {
                if (trl.WH_WorkHour <= 0)
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
                ((List<WorkHourLists>)dg_TechRouteInfo.ItemsSource).ForEach(p => { p.WH_WorkHour = 0.000M; });
                //tempWHLList.ForEach(p => { p.WH_WorkHour = 0.000M; });
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
            if (whlList != null && whlList.Count > 0)
            {
                string itemCode = whlList[0].TR_ItemCode;
                string techVersionCode = whlList[0].TRV_VersionCode;
                if (tempWHLList != null && tempWHLList.Count > 0)
                {
                    WorkHourHistory_Window whh = new WorkHourHistory_Window(itemCode, techVersionCode, tempWHLList);
                    whh.ShowDialog();
                }
                else
                {
                    WorkHourHistory_Window whh = new WorkHourHistory_Window(itemCode, techVersionCode, whlList);
                    whh.ShowDialog();
                }
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 添加新的工时信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddNew_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (ItemCheck())
            {
                SetReadOnlyProperties(true);
                if (haveWHInfo)
                {
                    tempWHLList = GenerateNewWHData();
                    ShowWHInfo(tempWHLList);
                }
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 放弃
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            SetReadOnlyProperties(false);
            ShowWHInfo(whlList);
            if (tempWHLList != null)
            {
                tempWHLList.Clear();
            }
            this.Cursor = Cursors.Arrow;
        }
    }
}
