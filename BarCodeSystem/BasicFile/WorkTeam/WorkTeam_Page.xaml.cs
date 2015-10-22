using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.SystemManage;
using BarCodeSystem.TechRoute.TechRoute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace BarCodeSystem.BasicFile.WorkTeam
{
    /// <summary>
    /// Interaction logic for WorkTeam_Page.xaml
    /// </summary>
    public partial class WorkTeam_Page : Page
    {
        public WorkTeam_Page()
        {
            InitializeComponent();
        }
        #region
        /// <summary>
        /// 加载次数
        /// </summary>
        int loadCount = 0;

        /// <summary>
        /// 工作中心列表
        /// </summary>
        List<WorkCenterLists> wclList;

        /// <summary>
        /// 总班组成员信息列表
        /// </summary>
        List<WorkTeamMemberLists> wtmlList = new List<WorkTeamMemberLists>();

        /// <summary>
        /// 当前选中班组的成员信息列表
        /// </summary>
        List<WorkTeamMemberLists> wtmlTempList = new List<WorkTeamMemberLists>();

        /// <summary>
        /// 当前选中班组中被删除的成员列表
        /// </summary>
        List<WorkTeamMemberLists> wtmlDeleteList = new List<WorkTeamMemberLists>();

        /// <summary>
        /// 班组列表
        /// </summary>
        List<WorkTeamLists> wtlList = new List<WorkTeamLists>();

        /// <summary>
        /// 上次双击选择的班组成员
        /// </summary>
        WorkTeamMemberLists lastWTML = new WorkTeamMemberLists();

        /// <summary>
        /// 上次选择的班组信息
        /// </summary>
        WorkTeamLists lastWTL = new WorkTeamLists();

        /// <summary>
        /// 新班组的班组成员列表
        /// </summary>
        List<WorkTeamMemberLists> wtmlNewList = new List<WorkTeamMemberLists>();

        /// <summary>
        /// 新班组
        /// </summary>
        WorkTeamLists newWTL = new WorkTeamLists();

        /// <summary>
        /// 当前班组信息是否更改
        /// </summary>
        bool isNew = false;

        /// <summary>
        /// 保存班组的时候，是否已经检查过编号
        /// </summary>
        bool haveChecked = false;
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
                ChangeDisplayProperty(false);
                //SetButtonVisiblity();
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
            GridLength gl = new GridLength(Math.Min(grid_WorkCenterInfo.ActualHeight / (count + 1), 60));
            if (count > 0)
            {
                RowDefinition rd = new RowDefinition() { Height = gl };
                Button btn = new Button() { Name = "btn_AllWorkCenter", Content = "所有车间", Width = 125, Height = 25, Cursor = Cursors.Arrow };
                grid_WorkCenterInfo.RowDefinitions.Add(rd);
                btn.SetValue(Button.StyleProperty, Application.Current.FindResource("bd_SelectStyle"));
                btn.Click += OnClick;
                grid_WorkCenterInfo.Children.Add(btn);
                Grid.SetRow(btn, grid_WorkCenterInfo.RowDefinitions.Count - 1);
                foreach (WorkCenterLists item in wclList)
                {
                    rd = new RowDefinition() { Height = gl };
                    grid_WorkCenterInfo.RowDefinitions.Add(rd);
                    btn = new Button() { Name = item.department_shortname, Content = item.department_name, Width = 125, Height = 25, Cursor = Cursors.Arrow };
                    btn.SetValue(Button.StyleProperty, Application.Current.FindResource("bd_SelectStyle"));
                    btn.Click += OnClick;
                    grid_WorkCenterInfo.Children.Add(btn);
                    Grid.SetRow(btn, grid_WorkCenterInfo.RowDefinitions.Count - 1);
                }
                grid_WorkCenterInfo.RowDefinitions.Add(new RowDefinition());
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("系统中没有车间信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 设置按钮的启用与否
        /// </summary>
        private void SetButtonVisiblity()
        {
            if (!User_Info.User_Code.Equals("admin"))
            {
                MyDBController.FindVisualChild<Button>(grid_WorkCenterInfo).ForEach(p =>
                {
                    if (!p.Name.Equals(User_Info.User_Workcenter_ShortName))
                    {
                        p.Visibility = Visibility.Hidden;
                    }
                });
            }
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClick(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            string key = ((Button)e.OriginalSource).Name;
            try
            {
                #region
                bool flag = IsNewChecking();
                if (flag)
                {
                    Thread th1 = new Thread(() => { Loading_Window lw = new Loading_Window("正在获取班组信息，请稍后"); lw.ShowDialog(); });
                    th1.SetApartmentState(ApartmentState.STA);
                    th1.Start();
                    if (key.Equals("btn_AllWorkCenter"))
                    {
                        wtmlList = new List<WorkTeamMemberLists>();
                        wtlList = WorkTeamLists.FetchWTInfoByWCID();
                        wclList.ForEach(
                            p =>
                            {
                                wtmlList.AddRange(WorkTeamMemberLists.FetchWorkTeamInfo(p.department_id));
                            });
                        dg_WorkTeamTotal.ItemsSource = wtmlList;
                        dg_WorkTeamTotal.Items.Refresh();
                        ICollectionView view = CollectionViewSource.GetDefaultView(dg_WorkTeamTotal.ItemsSource);
                        view.GroupDescriptions.Add(new PropertyGroupDescription("WTM_WorkCenterName"));
                        view.GroupDescriptions.Add(new PropertyGroupDescription("WTM_WorkTeamCode"));
                    }
                    else
                    {
                        Int64 _wcID = wclList.Find(p => p.department_shortname.Equals(key)).department_id;
                        wtlList = WorkTeamLists.FetchWTInfoByWCID(_wcID);
                        dg_WorkTeamTotal.ItemsSource = wtmlList = WorkTeamMemberLists.FetchWorkTeamInfo(_wcID);
                        dg_WorkTeamTotal.Items.Refresh();
                        ICollectionView view = CollectionViewSource.GetDefaultView(dg_WorkTeamTotal.ItemsSource);
                        view.GroupDescriptions.Add(new PropertyGroupDescription("WTM_WorkTeamCode"));
                    }
                    th1.Abort();
                    GC.Collect();
                    ClearInfo();
                    ChangeDisplayProperty(false);
                }
                #endregion
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 汇总列表双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_WorkTeamTotal_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (dg_WorkTeamTotal.SelectedIndex != -1)
            {
                WorkTeamMemberLists wtml = (WorkTeamMemberLists)dg_WorkTeamTotal.SelectedItem;
                lastWTL = wtlList.Find(p => p.ID.Equals(wtml.WTM_WorkTeamID));
                if (!wtml.WTM_WorkTeamID.Equals(lastWTML.WTM_WorkTeamID))
                {
                    bool flag = IsNewChecking();
                    if (flag)
                    {
                        ShowSelectedWTInfo(lastWTL, wtmlList.FindAll(p => p.WTM_WorkTeamID.Equals(wtml.WTM_WorkTeamID)));
                        lastWTML = wtml;
                    }
                }
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 当正在新增班组的时候，双击班组或者点击车间按钮的时候进行的检查
        /// </summary>
        /// <returns></returns>
        private bool IsNewChecking()
        {
            MessageBoxResult bms = new MessageBoxResult();
            if (isNew)
            {
                bms = Xceed.Wpf.Toolkit.MessageBox.Show("新增班组信息还没有保存，是否要继续？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }
            if (bms == MessageBoxResult.No)
            {
                return false;
            }
            else
            {
                ChangeDisplayProperty(false);
                isNew = false;
                haveChecked = false;
                return true;
            }
        }

        /// <summary>
        /// 展示信息
        /// </summary>
        /// <param name="_wtmlList"></param>
        private void ShowSelectedWTInfo(WorkTeamLists _wtl, List<WorkTeamMemberLists> _wtmlList)
        {
            dg_WorkTeamDetail.ItemsSource = wtmlTempList = _wtmlList;
            dg_WorkTeamDetail.Items.Refresh();
            tb_WorkCenter.Text = _wtl.workcenterName;
            tb_WTCode.Text = _wtl.WT_Code;
            tb_WTName.Text = _wtl.WT_Name;
            tb_MemberCount.Text = _wtmlList.Count.ToString();
            tsb_IsShown.IsChecked = _wtl.IsShown;
        }

        /// <summary>
        /// 清除展示的信息
        /// </summary>
        private void ClearInfo()
        {
            dg_WorkTeamDetail.ItemsSource = null;
            wtmlTempList.Clear();
            dg_WorkTeamDetail.Items.Refresh();
            tb_WorkCenter.Text = "";
            tb_WTCode.Text = "";
            tb_WTName.Text = "";
            tb_MemberCount.Text = "";
            tsb_IsShown.IsCheckedChanged -= tsb_IsShown_IsCheckedChanged;
            tsb_IsShown.IsChecked = false;
            tsb_IsShown.IsCheckedChanged += tsb_IsShown_IsCheckedChanged;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ModifyPerson_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (dg_WorkTeamDetail.HasItems)
            {
                ChangeDisplayProperty(true);
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 更改展示/修改信息的时候  控件的只读属性、可见性等等
        /// </summary>
        /// <param name="_flag"></param>
        /// <param name="_type">新增还是修改</param>
        private void ChangeDisplayProperty(bool _flag, string _type = "")
        {
            if (_flag)
            {
                tsb_IsShown.IsEnabled = true;
                tb_WTCode.IsReadOnly = false;
                tb_WTName.IsReadOnly = false;
                tb_WTName.Background = tb_WTCode.Background = Brushes.White;
                grid_WTDetail.Visibility = Visibility.Visible;
                if (_type.Equals("新增"))
                {
                    tb_WTCode.Text = tb_WTName.Text = "";
                }
            }
            else
            {
                tsb_IsShown.IsEnabled = false;
                tb_WTCode.IsReadOnly = true;
                tb_WTName.IsReadOnly = true;
                tb_WTName.Background = tb_WTCode.Background = Brushes.LightGray;
                grid_WTDetail.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 是否显示更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsb_IsShown_IsCheckedChanged(object sender, EventArgs e)
        {
            if (tsb_IsShown.IsChecked != true)
            {
                MessageBoxResult mbs = Xceed.Wpf.Toolkit.MessageBox.Show("确定要隐藏该班组吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mbs == MessageBoxResult.No)
                {
                    tsb_IsShown.IsChecked = true;
                }
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ChangeDisplayProperty(false);
            wtmlDeleteList.Clear();
            wtmlNewList.Clear();
            ShowSelectedWTInfo(lastWTL, wtmlTempList);
            isNew = false;
            haveChecked = false;
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 添加班组人员
        /// </summary>
        /// <param name="sender"></paramche>
        /// <param name="e"></param>
        private void btn_AddPerson_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            if (isNew)
            {
                if (string.IsNullOrEmpty(tb_WTName.Text) || string.IsNullOrEmpty(tb_WTCode.Text))
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("班组名称或者编号不能为空！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    flag = false;
                }
                else
                {
                    if (!haveChecked)
                    {
                        flag = CheckForCode(tb_WTCode.Text.Trim());
                    }
                    else
                    {
                    }
                }
            }
            if (flag)
            {
                TechRouteCheckPerson_Window trcp = new TechRouteCheckPerson_Window("操作工", "手工", AcceptPersonInfo);
                trcp.ShowDialog();
            }

        }

        /// <summary>
        /// 接受添加的成员的构造函数
        /// </summary>
        /// <param name="_plList"></param>
        private void AcceptPersonInfo(List<PersonLists> _plList)
        {
            List<WorkTeamMemberLists> _list = new List<WorkTeamMemberLists>();
            if (!isNew)
            {
                _list = wtmlTempList;
            }
            else
            {
                _list = wtmlNewList;
            }
            foreach (PersonLists pl in _plList)
            {
                if (!_list.Exists(p => p.WTM_MemberPersonCode.Equals(pl.code)))
                {
                    WorkTeamMemberLists wtml = new WorkTeamMemberLists();
                    wtml.WTM_MemberPersonCode = pl.code;
                    wtml.WTM_MemberPersonID = pl.ID;
                    wtml.WTM_MemberPersonName = pl.name;
                    wtml.WTM_WorkCenterID = pl.departid;
                    if (_list.Count > 0)
                    {
                        wtml.WTM_WorkTeamCode = _list.FirstOrDefault().WTM_WorkTeamCode;
                        wtml.WTM_WorkTeamID = _list.FirstOrDefault().WTM_WorkTeamID;
                        wtml.WTM_WorkTeamName = _list.FirstOrDefault().WTM_WorkTeamName;
                        wtml.WTM_WorkCenterName = _list.FirstOrDefault().WTM_WorkCenterName;
                    }
                    _list.Add(wtml);
                }
            }
            dg_WorkTeamDetail.ItemsSource = _list;
            dg_WorkTeamDetail.Items.Refresh();
            tb_MemberCount.Text = dg_WorkTeamDetail.Items.Count.ToString();
        }

        /// <summary>
        /// 删除人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DeletePerson_Click(object sender, RoutedEventArgs e)
        {
            if (dg_WorkTeamDetail.SelectedIndex != -1)
            {
                if (isNew)
                {
                    wtmlTempList.RemoveAt(dg_WorkTeamDetail.SelectedIndex);
                    dg_WorkTeamDetail.Items.Refresh();
                    tb_MemberCount.Text = dg_WorkTeamDetail.Items.Count.ToString();
                }
                else
                {
                    wtmlDeleteList.Add(wtmlTempList[dg_WorkTeamDetail.SelectedIndex]);
                    wtmlTempList.RemoveAt(dg_WorkTeamDetail.SelectedIndex);
                    dg_WorkTeamDetail.Items.Refresh();
                    tb_MemberCount.Text = dg_WorkTeamDetail.Items.Count.ToString();
                }
            }
        }

        /// <summary>
        /// 保存更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (dg_WorkTeamDetail.HasItems)
            {
                if (!isNew)
                {
                    AcceptWTChange(lastWTL);
                    WorkTeamLists.SaveInfo(lastWTL);
                    WorkTeamMemberLists.DeleteInfo(wtmlDeleteList);
                    WorkTeamMemberLists.SaveInfo(wtmlTempList);
                    ChangeDisplayProperty(false);
                    wtmlDeleteList.Clear();
                }
                else
                {
                    bool flag = false;
                    flag = WorkTeamLists.SaveInfo(newWTL);
                    if (flag)
                    {
                        SetNewWTMLInfo(WorkTeamLists.FetchWTID(newWTL.WT_Code, newWTL.WT_WorkCenterID).FirstOrDefault());
                        WorkTeamMemberLists.SaveInfo(wtmlNewList);
                        dg_WorkTeamDetail.Items.Refresh();
                        ChangeDisplayProperty(false);
                    }
                }
                isNew = false;
                haveChecked = false;
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 接受班组信息的更改
        /// </summary>
        /// <param name="_wtl"></param>
        private void AcceptWTChange(WorkTeamLists _wtl)
        {
            _wtl.IsShown = (tsb_IsShown.IsChecked == true);
            _wtl.WT_Code = tb_WTCode.Text;
            _wtl.WT_Name = tb_WTName.Text;
        }

        /// <summary>
        /// 新增班组信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddNewWT_Click(object sender, RoutedEventArgs e)
        {
            if (User_Info.User_Code.Equals("admin"))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("admin账号不能新增班组！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ChangeDisplayProperty(true, "新增");
                wtmlNewList.Clear();
                newWTL = GenerateNewWT();
                ShowNewWTInfo(newWTL);
                isNew = true;
            }
        }

        /// <summary>
        /// 展示新增的班组信息
        /// </summary>
        /// <param name="_wtl"></param>
        private void ShowNewWTInfo(WorkTeamLists _wtl)
        {
            tb_MemberCount.Text = "0";
            tb_WorkCenter.Text = _wtl.workcenterName;
            tsb_IsShown.IsChecked = _wtl.IsShown;
            dg_WorkTeamDetail.ItemsSource = wtmlNewList;
            dg_WorkTeamDetail.Items.Refresh();
            wtmlTempList.Clear();
            wtmlDeleteList.Clear();
        }
        /// <summary>
        /// 生成新的班组信息
        /// </summary>
        /// <returns></returns>
        private WorkTeamLists GenerateNewWT()
        {
            WorkTeamLists _wtl = new WorkTeamLists();
            _wtl.WT_WorkCenterID = User_Info.User_Workcenter_ID;
            _wtl.workcenterCode = User_Info.User_Workcenter_Code;
            _wtl.workcenterName = User_Info.User_WorkcenterName;
            _wtl.IsShown = true;
            return _wtl;
        }

        /// <summary>
        /// 设置新班组成员的班组id
        /// </summary>
        /// <param name="_wtl"></param>
        private void SetNewWTMLInfo(WorkTeamLists _wtl)
        {
            wtmlNewList.ForEach(p => { p.WTM_WorkTeamID = _wtl.ID; p.WTM_WorkCenterName = _wtl.workcenterName; p.WTM_WorkTeamCode = _wtl.WT_Code; p.WTM_WorkTeamName = _wtl.WT_Name; });
        }
        /// <summary>
        /// 检查班组编号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CheckForWTCode_Click(object sender, RoutedEventArgs e)
        {
            bool flag = false;
            if (isNew)
            {
                if (string.IsNullOrEmpty(tb_WTCode.Text))
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("班组编号不能为空！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    flag = CheckForCode(tb_WTCode.Text.Trim());
                    if (flag)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("该班组编号可以使用！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                }
            }
        }

        /// <summary>
        /// 检查输入的班组编号是否存在
        /// </summary>
        /// <param name="_wtCode"></param>
        /// <returns></returns>
        private bool CheckForCode(string _wtCode)
        {
            bool flag = !WorkTeamLists.CheckIfCodeExsist(tb_WTCode.Text.Trim(), User_Info.User_Workcenter_ID);
            haveChecked = flag;
            if (!flag)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("该班组编号已经存在，请更换编号！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                newWTL.WT_Code = tb_WTCode.Text;
                newWTL.WT_Name = tb_WTName.Text;
            }
            return flag;
        }
    }
}
