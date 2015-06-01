using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock;
using System.ComponentModel;
using System.Windows.Media;
using BarCodeSystem.ProductDispatch.FlowCard;

namespace BarCodeSystem
{
    /// <summary>
    /// Main_Window.xaml 的交互逻辑
    /// </summary>
    public partial class Main_Window : Window
    {
        public Main_Window()
        {
            InitializeComponent();
        }

        #region 变量


        private List<MenuItem> MainMenuItemList = new List<MenuItem> { };
        List<string> AuthorityList = new List<string> { };
        DataSet ds = new DataSet();
        #endregion


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Title = User_Info.User_ID;
            MyDBController.Server = User_Info.server[1];
            MyDBController.Database = User_Info.database[1];
            MyDBController.Pwd = User_Info.pwd[1];
            MyDBController.Uid = User_Info.uid[1];

            System.Drawing.Rectangle rect =
                System.Windows.Forms.Screen.PrimaryScreen.Bounds;

            //获取当前工作区域的分辨率，用来动态设置系统中其他窗体的大小
            User_Info.ScreenHeight = rect.Height; //高（像素）
            User_Info.ScreenWidth = rect.Width;

            User_Info.User_ID = 1;
            User_Info.User_Name = "钱康";

            SetUser_Authority();
            //AvalonSetting();
        }

        #region 菜单权限管控
        /// <summary>
        /// 获得系统主窗体的菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuItem> GetMainMenuItemList()
        {
            foreach (MenuItem item in System_Menu.Items)
            {
                EnumChildMenu(item);
            }
            return MainMenuItemList;
        }

        /// <summary>
        /// 递归获得所有菜单项
        /// </summary>
        /// <param name="item"></param>
        private void EnumChildMenu(MenuItem item)
        {
            if (item.Items.Count > 0)
            {
                MainMenuItemList.Add(item);
                foreach (var items in item.Items)
                {
                    if (items is MenuItem)
                        EnumChildMenu((MenuItem)items);
                }
            }
            else
            {
                MainMenuItemList.Add(item);
            }
        }


        /// <summary>
        /// 根据账号做菜单权限管控
        /// </summary>
        private void SetUser_Authority()
        {
            //1.第一步，获得系统中菜单列表
            GetMainMenuItemList();
            //2.第二步，获得登陆账号菜单权限列表
            GetUserAuthority();
            //3.对菜单进行权限管控
            EnableMenuItem(AuthorityList, MainMenuItemList);
        }

        /// <summary>
        /// 获得系统账号菜单权限
        /// </summary>
        /// <returns></returns>
        private List<string> GetUserAuthority()
        {
            string SQl = string.Format(@"select A.[SA_AuthorityName] from [SystemAuthority] A
                                left join [UserAuthority] B on A.[ID]=B.[UA_SysAuthorityID]
                                where B.[UA_UserAccountID] = {0}", User_Info.User_ID);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "UA_AuthorityName");
            MyDBController.CloseConnection();

            int x = ds.Tables["UA_AuthorityName"].Rows.Count;
            for (int i = 0; i < x; i++)
            {
                AuthorityList.Add(ds.Tables["UA_AuthorityName"].Rows[i][0].ToString().Trim());
            }
            return AuthorityList;
        }

        /// <summary>
        /// 根据账号权限开启菜单选项
        /// </summary>
        /// <param name="strlist"></param>
        /// <param name="milist"></param>
        private void EnableMenuItem(List<string> strlist, List<MenuItem> milist)
        {
            int x = strlist.Count;
            int y = milist.Count;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (string.Equals(strlist[i], milist[j].Name))
                    {
                        milist[j].IsEnabled = true;
                    }
                }

            }
        }
        #endregion

        #region 菜单点击事件
        /// <summary>
        /// 导入U9账号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_U9Account_Click(object sender, RoutedEventArgs e)
        {
            U9User_Window UUW = new U9User_Window();
            UUW.Height = Math.Min(User_Info.ScreenHeight * 4 / 5, 800);
            UUW.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            UUW.ShowDialog();
        }

        /// <summary>
        /// 授权中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_AccountAuthority_Click(object sender, RoutedEventArgs e)
        {
            Authority_Window AW = new Authority_Window();
            AW.Height = Math.Min(User_Info.ScreenHeight * 4 / 5, 800);
            AW.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            AW.ShowDialog();
        }

        /// <summary>
        /// 工作中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_WorkCenterImport_Click(object sender, RoutedEventArgs e)
        {
            WorkCenter_Window wc = new WorkCenter_Window();
            wc.Height = Math.Min(User_Info.ScreenHeight * 4 / 5, 800);
            wc.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            wc.ShowDialog();
        }

        /// <summary>
        /// 员工中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_StaffInfoManage_Click(object sender, RoutedEventArgs e)
        {
            Person_Window p = new Person_Window();
            p.Height = Math.Min(User_Info.ScreenHeight * 4 / 5, 800);
            p.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            p.ShowDialog();
        }

        /// <summary>
        /// 班组中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_WorkTeamManage_Click(object sender, RoutedEventArgs e)
        {
            WorkTeam_Window wt = new WorkTeam_Window();
            wt.Height = Math.Min(User_Info.ScreenHeight * 4 / 5, 800);
            wt.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            wt.ShowDialog();
        }

        /// <summary>
        /// 生产线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_BeltlineManage_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 报工设备管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_WorkEquipManage_Click(object sender, RoutedEventArgs e)
        {
            Device_Window d = new Device_Window();
            d.Height = Math.Min(User_Info.ScreenHeight * 4 / 5, 800);
            d.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            d.ShowDialog();
        }

        /// <summary>
        /// 仓库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_WarehouseManage_Click(object sender, RoutedEventArgs e)
        {
            Warehouse_Window w = new Warehouse_Window();
            w.Height = Math.Min(User_Info.ScreenHeight * 4 / 5, 800);
            w.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            w.ShowDialog();
        }

        /// <summary>
        /// 质量档案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_QualityFileManage_Click(object sender, RoutedEventArgs e)
        {
            QualityIssues_Window qi = new QualityIssues_Window();
            qi.Height = Math.Min(User_Info.ScreenHeight * 4 / 5, 800);
            qi.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            qi.ShowDialog();
        }

        /// <summary>
        /// 料品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_MaterialManage_Click(object sender, RoutedEventArgs e)
        {
            ItemInfo_Window ii = new ItemInfo_Window();
            ii.Height = Math.Min(User_Info.ScreenHeight * 4 / 5, 800);
            ii.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            ii.ShowDialog();
        }

        /// <summary>
        /// 工序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_ProcedureManage_Click(object sender, RoutedEventArgs e)
        {
            ProcessName_Window pn = new ProcessName_Window();
            pn.Height = Math.Min(User_Info.ScreenHeight * 4 / 5, 800);
            pn.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            pn.ShowDialog();
        }

        /// <summary>
        /// 工艺路线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_TechRouteManage_Click(object sender, RoutedEventArgs e)
        {
            TechRoute_Window tr = new TechRoute_Window();
            tr.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            tr.Height = Math.Min(User_Info.ScreenHeight * 4 / 5, 800);
            tr.ShowDialog();
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = WindowState.Minimized;
            this.ShowInTaskbar = false;
        }

        /// <summary>
        /// 退出菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_Close_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }


        /// <summary>
        /// 关于作者
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_About_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 双击系统托盘小图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNotificationAreaIconDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.WindowState = WindowState.Maximized;
        }

        /// <summary>
        /// 退出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_Quit_Click(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_Show_Click(object sender, EventArgs e)
        {
            Show();
        }

        /// <summary>
        /// 设置~
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_Options_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 利用AvalonDocking对工作区域进行管理
        private void item_FileQuery1_Click(object sender, RoutedEventArgs e)
        {

            #region 将各个page加载进工作区域


            if (dockingManager.Layout.ActiveContent == null && MyDBController.FindVisualChild<FlowCard_Page>(dockingManager).Count == 0)
            {
                Frame topFrame = new Frame();
                topFrame.Content = new FlowCard_Page() { ShowsNavigationUI = true };
                LayoutAnchorable la = new LayoutAnchorable();
                la.Title = "流转卡编制";
                la.Content = topFrame;
                la.Closing += la_Closing;

                ldp.Children.Add(la);
                la.IsSelected = true;
            }
            else
            {
                ((FlowCard_Page)MyDBController.FindVisualChild<FlowCard_Page>(dockingManager)[0]).Focus();
            }
            #endregion
        }

        /// <summary>
        /// 标签页关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void la_Closing(object sender, CancelEventArgs e)
        {
            object cm = dockingManager.DataContext;
            if (MessageBox.Show("关我干啥呢？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {

            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 对MainWindow的AvalonDock进行设置
        /// </summary>
        private void AvalonSetting()
        {
            lp.Children.Add(ldp);
            lr.RootPanel = lp;
            //lr.RootPanel.Children.Add(lp);//不能用这个方法，用这个方法，AvalonDock会自动为lr添加一个GridSpliter，会把lr分成左右两部分。很恶心。
            dockingManager.Layout = lr;
        }


        #endregion


    }
}
