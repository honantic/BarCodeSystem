using BarCodeSystem.FileQuery.FlowCardQuery;
using BarCodeSystem.ProductDispatch.FlowCard;
using BarCodeSystem.ProductDispatch.FlowCardClean;
using BarCodeSystem.ProductDispatch.FlowCardDistribute;
using BarCodeSystem.ProductDispatch.FlowCardPrint;
using BarCodeSystem.ProductDispatch.FlowCardReport;
using BarCodeSystem.ProductDispatch.FlowCardTransfer;
using BarCodeSystem.SalaryManage.QualityMonthlySalary;
using BarCodeSystem.StorageManage.FinishReport;
using BarCodeSystem.StorageManage.ScrapReport;
using BarCodeSystem.SystemManage;
using BarCodeSystem.TechRoute.TechRoute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;
using System.Linq;
using BarCodeSystem.BasicFile.QualityIssues;
using System.Data.SqlClient;
using BarCodeSystem.TechRoute.TechRouteWorkHourManually;
using BarCodeSystem.PublicClass.HelperClass;
using BarCodeSystem.FileQuery.ProduceOrderQuery;
using BarCodeSystem.SystemManage.AuthorityManagement;
using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.FileQuery.DailyReport_Page;
using System.Deployment.Application;

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


        public List<MenuItem> MainMenuItemList = new List<MenuItem>();
        List<UserAuthorityLists> AuthorityList = new List<UserAuthorityLists> { };
        DataSet ds = new DataSet();
        #endregion

        #region 初始化
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetBasicInfo();
            SetUser_Authority();
            GetOrgInfo();
            DBLog.WriteLoginRecord();
        }

        /// <summary>
        /// 基本系统参数设置
        /// </summary>
        private void SetBasicInfo()
        {
            MyDBController.Server = User_Info.server[1];
            MyDBController.Database = User_Info.database[1];
            MyDBController.Pwd = User_Info.pwd[1];
            MyDBController.Uid = User_Info.uid[1];
            System.Drawing.Rectangle rect =
                System.Windows.Forms.Screen.PrimaryScreen.Bounds;

            //获取当前工作区域的分辨率，用来动态设置系统中其他窗体的大小
            User_Info.ScreenHeight = rect.Height; //高（像素）
            User_Info.ScreenWidth = rect.Width;
            if (!User_Info.User_Code.Equals("admin"))
            {
                string SQl = string.Format(@"select A.[ID],A.[P_Code],A.[P_Name],A.[P_Position],B.[WC_Department_ID] ,B.[WC_Department_Code],B.[WC_Department_Name],B.[WC_Department_ShortName],C.[ID] as [Account_ID] from [Person] as A left join [WorkCenter] as B on A.[P_WorkCenterID]=B.[WC_Department_ID] left join [UserAccount] C on A.[P_Code]=C.[UA_LoginAccount] where [P_Code]='{0}'", User_Info.User_Code);
                DataSet ds = new DataSet();
                MyDBController.GetConnection();
                MyDBController.GetDataSet(SQl, ds, "Person");
                MyDBController.CloseConnection();

                try
                {
                    SQl = string.Format(@"select [id] from [Base_User] where [code]='{0}'", User_Info.User_Code);
                    SqlConnection sqlconn = new SqlConnection();
                    sqlconn.ConnectionString = string.Format(@"server={0};database={1};uid={2};pwd={3}", User_Info.server[0], User_Info.database[0], User_Info.uid[0], User_Info.pwd[0]);
                    User_Info.Account_ID = Convert.ToInt64(ds.Tables["Person"].Rows[0]["Account_ID"]);
                    User_Info.U9User_ID = Convert.ToInt64(MyDBController.ExecuteScalar(sqlconn, SQl));
                    User_Info.User_Name = ds.Tables["Person"].Rows[0]["P_Name"].ToString();
                    User_Info.User_Workcenter_Code = ds.Tables["Person"].Rows[0]["WC_Department_Code"].ToString();
                    User_Info.User_Workcenter_ShortName = ds.Tables["Person"].Rows[0]["WC_Department_ShortName"].ToString();
                    User_Info.User_WorkcenterName = ds.Tables["Person"].Rows[0]["WC_Department_Name"].ToString();
                    User_Info.P_Position = ds.Tables["Person"].Rows[0]["P_Position"].ToString();
                    User_Info.User_Workcenter_ID = Convert.ToInt64(ds.Tables["Person"].Rows[0]["WC_Department_ID"]);
                    User_Info.POType = "WG20101";
                }
                catch (Exception)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("该账号信息不全！请检查。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                //检查系统菜单是否有变更，有变更则更新数据库。
                SystemAuthority.CheckSystemAuthority();
            }
        }

        /// <summary>
        /// 获得U9组织信息并显示在主窗口
        /// </summary>
        private void GetOrgInfo()
        {
            string SQl = @"SELECT [ID],[OI_ID],[OI_Code],[OI_Name],[OI_Remark]
                            FROM [OrgInfo]";
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "OrgInfo");
            MyDBController.CloseConnection();

            if (ds.Tables["OrgInfo"].Rows.Count == 0)
            {
                this.Title = "生产条码系统";
            }
            else
            {
                User_Info.Org_Id = Convert.ToInt64(ds.Tables["OrgInfo"].Rows[0]["OI_ID"]);
                User_Info.Org_Info = ds.Tables["OrgInfo"].Rows[0]["OI_Name"].ToString();
                this.Title = User_Info.Org_Info + "生产条码系统";
            }


        }
        #endregion

        #region 菜单权限管控
        /// <summary>
        /// 获得系统主窗体的菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuItem> GetMainMenuItemList()
        {
            MainMenuItemList = new List<MenuItem>();
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
            if (!User_Info.User_Code.Equals("admin"))
            {
                //1.第一步，获得系统中菜单列表
                GetMainMenuItemList();
                //2.第二步，获得登陆账号菜单权限列表
                List<UserAuthorityLists> strList = GetUserAuthority();
                //3.对菜单进行权限管控
                EnableMenuItem(strList, MainMenuItemList);
            }
        }

        /// <summary>
        /// 获得系统账号菜单权限
        /// </summary>
        /// <returns></returns>
        private List<UserAuthorityLists> GetUserAuthority()
        {
            List<UserAuthorityLists> strList = new List<UserAuthorityLists>();
            strList = UserAuthorityLists.FetchUAListByUserID(User_Info.Account_ID);
            return strList;
        }

        /// <summary>
        /// 根据账号权限开启菜单选项
        /// </summary>
        /// <param name="strlist"></param>
        /// <param name="milist"></param>
        private void EnableMenuItem(List<UserAuthorityLists> strlist, List<MenuItem> milist)
        {
            int x = strlist.Count;
            int y = milist.Count;
            //List<string> miNameList = milist.Select(p => p.Name).ToList();
            milist.ForEach(p =>
            {
                if (!strlist.Exists(item => item.SA_AuthorityCode.Equals(p.Name)) && !p.HasItems)
                {
                    p.IsEnabled = false;
                }
            });
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
            //Authority_Window AW = new Authority_Window();
            //AW.Height = Math.Min(User_Info.ScreenHeight * 4 / 5, 800);
            //AW.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            //AW.Show();

            this.Cursor = Cursors.Wait;
            bool flag = false;
            foreach (LayoutAnchorable item in ldp.Children)
            {
                if (item.Title == "账号授权中心")
                {
                    flag = true;
                    item.IsSelected = true;
                    break;
                }
            }
            if (!flag)//MyDBController.FindVisualChild<FlowCardReport_Page>(this).Count == 0
            {
                Frame topFrame = new Frame();

                AuthorityManagement_Page am = new AuthorityManagement_Page() { ShowsNavigationUI = true };
                topFrame.Content = am;
                LayoutAnchorable la = new LayoutAnchorable();
                la.Title = "账号授权中心";
                la.Content = topFrame;

                ldp.Children.Add(la);
                la.IsSelected = true;
            }
            else
            {
            }
            this.Cursor = Cursors.Arrow;
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
            this.Cursor = Cursors.Wait;
            TechRoute_Window tr = new TechRoute_Window();
            tr.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            tr.Height = Math.Min(User_Info.ScreenHeight * 4 / 5, 800);
            tr.ShowDialog();
            this.Cursor = Cursors.Arrow;
        }


        /// <summary>
        /// 工艺路线工时管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_WorkHourManage_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            bool flag = false;
            foreach (LayoutAnchorable item in ldp.Children)
            {
                if (item.Title == "工时管理页面")
                {
                    flag = true;
                    item.IsSelected = true;
                    break;
                }
            }
            if (!flag)//MyDBController.FindVisualChild<FlowCardReport_Page>(this).Count == 0
            {
                Frame topFrame = new Frame();

                WorkHour_Page wh = new WorkHour_Page() { ShowsNavigationUI = true };
                topFrame.Content = wh;
                LayoutAnchorable la = new LayoutAnchorable();
                la.Title = "工时管理页面";
                la.Content = topFrame;

                ldp.Children.Add(la);
                la.IsSelected = true;
            }
            else
            {
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 工艺路线导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_TechRouteImport_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            TechRouteImport_Window tri = new TechRouteImport_Window();
            tri.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            tri.Height = Math.Min(User_Info.ScreenHeight * 4 / 5, 800);
            tri.ShowDialog();
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 工艺路线导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_TechRouteExport_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            TechRouteExport tre = new TechRouteExport();
            tre.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            tre.Height = Math.Min(User_Info.ScreenHeight * 4 / 5, 800);
            tre.ShowDialog();
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 组织信息点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_SysParaManage_Click(object sender, RoutedEventArgs e)
        {
            OrgInfoList_Window oi = new OrgInfoList_Window();
            oi.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            oi.Height = Math.Min(User_Info.ScreenHeight * 4 / 5, 800);
            oi.ShowDialog();
        }

        /// <summary>
        /// 质检分类点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_QualitySort_Click(object sender, RoutedEventArgs e)
        {
            QualitySort_Window qs = new QualitySort_Window();
            qs.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            qs.Height = Math.Min(User_Info.ScreenHeight * 4 / 5, 800);
            qs.ShowDialog();
        }
        #endregion

        #region 系统托盘操作等
        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Show(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Visible;
            this.ShowInTaskbar = true;
            this.Activate();
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hide(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Visibility = System.Windows.Visibility.Hidden;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close(object sender, EventArgs e)
        {
            DBLog.WriteLogoutRecord();
            MyDBController.CloseConnection();
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// 切换账号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_SwitchAccount_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            DBLog.WriteLogoutRecord();
            MyDBController.CloseConnection();
            this.notification.Close();
            this.Closing -= Window_Closing;
            this.Close();
            Login_Window lw = new Login_Window();
            lw.Show();
            this.Cursor = Cursors.Arrow;
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
            App.Current.MainWindow = this;

        }

        /// <summary>
        /// 退出菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_Close_Click(object sender, RoutedEventArgs e)
        {
            DBLog.WriteLogoutRecord();
            MyDBController.CloseConnection();
            notification.IsEnabled = false;
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
            if (this.Visibility == Visibility.Hidden)
            {
                this.Visibility = Visibility.Visible;
            }
            this.ShowInTaskbar = true;
            this.Topmost = true;
            this.WindowState = WindowState.Maximized;
            this.Topmost = false;
        }

        /// <summary>
        /// 退出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_Quit_Click(object sender, EventArgs e)
        {
            DBLog.WriteLogoutRecord();
            MyDBController.CloseConnection();
            notification.IsEnabled = false;
            App.Current.Shutdown();
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_Show_Click(object sender, EventArgs e)
        {
            if (this.Visibility == Visibility.Hidden)
            {
                this.Visibility = Visibility.Visible;
            }
            this.ShowInTaskbar = true;
            this.Topmost = true;
            this.WindowState = WindowState.Maximized;
            this.Topmost = false;
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

        #region 流转卡派工管理

        /// <summary>
        /// 流转卡报工
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_WorkCardReport_Click(object sender, RoutedEventArgs e)
        {

            #region 将各个page加载进工作区域
            this.Cursor = Cursors.Wait;
            bool flag = false;
            foreach (LayoutAnchorable item in ldp.Children)
            {
                if (item.Title == "流转卡报工")
                {
                    flag = true;
                    item.IsSelected = true;
                    break;
                }
            }
            if (!flag)//MyDBController.FindVisualChild<FlowCardReport_Page>(this).Count == 0
            {
                Frame topFrame = new Frame();
                FlowCardReport_Page fcr = new FlowCardReport_Page() { ShowsNavigationUI = true };
                topFrame.Content = fcr;
                LayoutAnchorable la = new LayoutAnchorable();
                la.Title = "流转卡报工";
                la.Content = topFrame;
                la.Closing += fcr.Closing;

                ldp.Children.Add(la);
                la.IsSelected = true;
            }
            else
            {
            }
            this.Cursor = Cursors.Arrow;
            #endregion
        }

        /// <summary>
        /// 派工
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_WorkCardCompose_Click(object sender, RoutedEventArgs e)
        {
            #region 将各个page加载进工作区域
            this.Cursor = Cursors.Wait;
            bool flag = false;
            foreach (LayoutAnchorable item in ldp.Children)
            {
                switch (item.Title)
                {
                    case "流转卡编制":
                        flag = true;
                        item.IsSelected = true;
                        break;
                    case "流转卡报工":
                        ((FlowCardReport_Page)((Frame)item.Content).Content).searchFrame.Content = null;
                        ((FlowCardReport_Page)((Frame)item.Content).Content).textb_SearchInfo.Text = "";
                        break;
                    case "流转卡分批":
                        ((FlowCardDistribute_Page)((Frame)item.Content).Content).frame_FlowCardSearch.Content = null;
                        ((FlowCardDistribute_Page)((Frame)item.Content).Content).textb_SearcInfo.Visibility = Visibility.Collapsed;
                        break;
                    default:
                        break;
                }
            }
            if (!flag)//MyDBController.FindVisualChild<FlowCard_Page>(this).Count == 0
            {
                Frame topFrame = new Frame();
                topFrame.Content = new FlowCard_Page() { ShowsNavigationUI = true };
                LayoutAnchorable la = new LayoutAnchorable();
                la.Title = "流转卡编制";
                la.Content = topFrame;

                ldp.Children.Add(la);
                la.IsSelected = true;
            }
            else
            {
            }
            this.Cursor = Cursors.Arrow;
            #endregion
        }

        /// <summary>
        /// 流转卡分批
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_WorkCardDis_Click(object sender, RoutedEventArgs e)
        {
            #region 将各个page加载进工作区域
            this.Cursor = Cursors.Wait;
            bool flag = false;
            foreach (LayoutAnchorable item in ldp.Children)
            {
                if (item.Title == "流转卡分批")
                {
                    flag = true;
                    item.IsSelected = true;
                    break;
                }
            }
            if (!flag)//MyDBController.FindVisualChild<FlowCardReport_Page>(this).Count == 0
            {
                Frame topFrame = new Frame();
                FlowCardDistribute_Page fcd = new FlowCardDistribute_Page() { ShowsNavigationUI = true };
                topFrame.Content = fcd;
                LayoutAnchorable la = new LayoutAnchorable();
                la.Title = "流转卡分批";
                la.Content = topFrame;

                ldp.Children.Add(la);
                la.IsSelected = true;
            }
            else
            {
            }
            this.Cursor = Cursors.Arrow;
            #endregion
        }


        /// <summary>
        /// 流转卡清卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_WorkCardClean_Click(object sender, RoutedEventArgs e)
        {
            #region 将各个page加载进工作区域
            this.Cursor = Cursors.Wait;
            bool flag = false;
            foreach (LayoutAnchorable item in ldp.Children)
            {
                if (item.Title == "流转卡清卡")
                {
                    flag = true;
                    item.IsSelected = true;
                    break;
                }
            }
            if (!flag)//MyDBController.FindVisualChild<FlowCardReport_Page>(this).Count == 0
            {
                Frame topFrame = new Frame();
                FlowCardClean_Page fcc = new FlowCardClean_Page() { ShowsNavigationUI = true };
                topFrame.Content = fcc;
                LayoutAnchorable la = new LayoutAnchorable();
                la.Title = "流转卡清卡";
                la.Content = topFrame;

                ldp.Children.Add(la);
                la.IsSelected = true;
            }
            else
            {
            }
            this.Cursor = Cursors.Arrow;
            #endregion
        }


        /// <summary>
        /// 流转卡转序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_WorkCardTransfer_Click(object sender, RoutedEventArgs e)
        {
            #region 将各个page加载进工作区域
            this.Cursor = Cursors.Wait;
            bool flag = false;

            #region  AvalonDock排他性尝试
            foreach (var item in dockingManager.Layout.RootPanel.Children)
            {
                if (item is LayoutAnchorablePane)
                {
                    var _item = (LayoutAnchorablePane)item;
                    foreach (var child in _item.Children)
                    {
                        if (child is LayoutAnchorable)
                        {
                            var x = (LayoutAnchorable)child;
                            if ((x).Title == "流转卡转序")
                            {
                                flag = true;
                                x.IsSelected = true;
                                break;
                            }
                        }
                    }
                }
                else if (item is LayoutDocumentPaneGroup)
                {
                    var _item = (LayoutDocumentPaneGroup)item;
                    foreach (var child in _item.Children)
                    {
                        foreach (var _child in child.Children)
                        {
                            if (_child is LayoutAnchorable)
                            {
                                var x = (LayoutAnchorable)_child;
                                if (x.Title == "流转卡转序")
                                {
                                    flag = true;
                                    x.IsSelected = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            foreach (LayoutAnchorable item in ldp.Children)
            {
                if (item.Title == "流转卡转序")
                {
                    flag = true;
                    item.IsSelected = true;
                    break;
                }
            }
            if (!flag)//MyDBController.FindVisualChild<FlowCardReport_Page>(this).Count == 0
            {
                Frame topFrame = new Frame();
                FlowCardTransfer_Page fct = new FlowCardTransfer_Page() { ShowsNavigationUI = true };
                topFrame.Content = fct;
                LayoutAnchorable la = new LayoutAnchorable();
                la.Title = "流转卡转序";
                la.Content = topFrame;

                ldp.Children.Add(la);
                la.IsSelected = true;
            }
            else
            {
            }
            this.Cursor = Cursors.Arrow;
            #endregion
        }
        #endregion

        #region 库存管理
        /// <summary>
        /// 日报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_DailyReport_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            bool flag = false;
            foreach (LayoutAnchorable item in ldp.Children)
            {
                if (item.Title == "完工日报表")
                {
                    flag = true;
                    item.IsSelected = true;
                    break;
                }
            }
            if (!flag)//MyDBController.FindVisualChild<FlowCardReport_Page>(this).Count == 0
            {
                Frame topFrame = new Frame();
                DailyReport_Page fcr = new DailyReport_Page() { ShowsNavigationUI = true };
                topFrame.Content = fcr;
                LayoutAnchorable la = new LayoutAnchorable();
                la.Title = "完工日报表";
                la.Content = topFrame;
                //la.Closing += fcr.Closing;

                ldp.Children.Add(la);
                la.IsSelected = true;
            }
            else
            {
            }
            this.Cursor = Cursors.Arrow;
        }
        /// <summary>
        /// 完工报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_FinishReport_Click(object sender, RoutedEventArgs e)
        {
            #region 将各个page加载进工作区域
            this.Cursor = Cursors.Wait;

            bool flag = false;
            foreach (LayoutAnchorable item in ldp.Children)
            {
                if (item.Title == "完工报告审核")
                {
                    flag = true;
                    item.IsSelected = true;
                    break;
                }
            }
            if (!flag)//MyDBController.FindVisualChild<FlowCardReport_Page>(this).Count == 0
            {
                Frame topFrame = new Frame();
                FinishReport_Page fcr = new FinishReport_Page() { ShowsNavigationUI = true };
                topFrame.Content = fcr;
                LayoutAnchorable la = new LayoutAnchorable();
                la.Title = "完工报告审核";
                la.Content = topFrame;
                //la.Closing += fcr.Closing;

                ldp.Children.Add(la);
                la.IsSelected = true;
            }
            else
            {
            }
            this.Cursor = Cursors.Arrow;
            #endregion
        }
        /// <summary>
        /// 缴废单审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_ScrapReport_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            #region 将各个page加载进工作区域
            bool flag = false;
            foreach (LayoutAnchorable item in ldp.Children)
            {
                if (item.Title == "缴废单审核")
                {
                    flag = true;
                    item.IsSelected = true;
                    break;
                }
            }
            if (!flag)//MyDBController.FindVisualChild<FlowCardReport_Page>(this).Count == 0
            {
                Frame topFrame = new Frame();
                ScrapReport_Page fcr = new ScrapReport_Page() { ShowsNavigationUI = true };
                topFrame.Content = fcr;
                LayoutAnchorable la = new LayoutAnchorable();
                la.Title = "缴废单审核";
                la.Content = topFrame;
                //la.Closing += fcr.Closing;

                ldp.Children.Add(la);
                la.IsSelected = true;
            }
            else
            {
            }
            this.Cursor = Cursors.Arrow;
            #endregion
        }
        #endregion

        #region 档案查询
        /// <summary>
        /// 流转卡查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_FlowCardQuery_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            bool flag = false;
            foreach (LayoutAnchorable item in ldp.Children)
            {
                if (item.Title == "流转卡查询")
                {
                    flag = true;
                    item.IsSelected = true;
                    break;
                }
            }
            if (!flag)//MyDBController.FindVisualChild<FlowCardReport_Page>(this).Count == 0
            {
                Frame topFrame = new Frame();

                FlowCardQuery_Page fcq = new FlowCardQuery_Page() { ShowsNavigationUI = true };
                topFrame.Content = fcq;
                LayoutAnchorable la = new LayoutAnchorable();
                la.Title = "流转卡查询";
                la.Content = topFrame;

                ldp.Children.Add(la);
                la.IsSelected = true;
            }
            else
            {
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 生产订单查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_ProduceOrderQuery_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            bool flag = false;
            foreach (LayoutAnchorable item in ldp.Children)
            {
                if (item.Title == "生产订单卡查询")
                {
                    flag = true;
                    item.IsSelected = true;
                    break;
                }
            }
            if (!flag)//MyDBController.FindVisualChild<FlowCardReport_Page>(this).Count == 0
            {
                Frame topFrame = new Frame();

                ProduceOrderQuery_Page poq = new ProduceOrderQuery_Page() { ShowsNavigationUI = true };
                topFrame.Content = poq;
                LayoutAnchorable la = new LayoutAnchorable();
                la.Title = "生产订单卡查询";
                la.Content = topFrame;

                ldp.Children.Add(la);
                la.IsSelected = true;
            }
            else
            {
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 不良品明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_BadProductDetail_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            bool flag = false;
            foreach (LayoutAnchorable item in ldp.Children)
            {
                if (item.Title == "不良品明细报表")
                {
                    flag = true;
                    item.IsSelected = true;
                    break;
                }
            }
            if (!flag)//MyDBController.FindVisualChild<FlowCardReport_Page>(this).Count == 0
            {
                Frame topFrame = new Frame();

                BadProductList_Page bpl = new BadProductList_Page() { ShowsNavigationUI = true };
                topFrame.Content = bpl;
                LayoutAnchorable la = new LayoutAnchorable();
                la.Title = "不良品明细报表";
                la.Content = topFrame;

                ldp.Children.Add(la);
                la.IsSelected = true;
            }
            else
            {
            }
            this.Cursor = Cursors.Arrow;
        }



        /// <summary>
        /// 不良品汇总
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_BadProductTotal_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            bool flag = false;
            foreach (LayoutAnchorable item in ldp.Children)
            {
                if (item.Title == "不良品汇总报表")
                {
                    flag = true;
                    item.IsSelected = true;
                    break;
                }
            }
            if (!flag)//MyDBController.FindVisualChild<FlowCardReport_Page>(this).Count == 0
            {
                Frame topFrame = new Frame();

                BadProductSummary_Page bps = new BadProductSummary_Page() { ShowsNavigationUI = true };
                topFrame.Content = bps;
                LayoutAnchorable la = new LayoutAnchorable();
                la.Title = "不良品汇总报表";
                la.Content = topFrame;

                ldp.Children.Add(la);
                la.IsSelected = true;
            }
            else
            {
            }
            this.Cursor = Cursors.Arrow;
        }

        #endregion

        #region 工资管理

        /// <summary>
        /// 月度质量奖赔信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_QualityMonthlySalary_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            bool flag = false;
            foreach (LayoutAnchorable item in ldp.Children)
            {
                if (item.Title == "质量奖赔管理")
                {
                    flag = true;
                    item.IsSelected = true;
                    break;
                }
            }
            if (!flag)//MyDBController.FindVisualChild<FlowCardReport_Page>(this).Count == 0
            {
                Frame topFrame = new Frame();

                QualityMonthlySalary_Page fcq = new QualityMonthlySalary_Page() { ShowsNavigationUI = true };
                topFrame.Content = fcq;
                LayoutAnchorable la = new LayoutAnchorable();
                la.Title = "质量奖赔管理";
                la.Content = topFrame;

                ldp.Children.Add(la);
                la.IsSelected = true;
            }
            else
            {
            }
            this.Cursor = Cursors.Arrow;
        }


        /// <summary>
        /// 员工工资查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_StaffSalaries_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            bool flag = false;
            foreach (LayoutAnchorable item in ldp.Children)
            {
                if (item.Title == "员工工资查询")
                {
                    flag = true;
                    item.IsSelected = true;
                    break;
                }
            }
            if (!flag)//MyDBController.FindVisualChild<FlowCardReport_Page>(this).Count == 0
            {
                Frame topFrame = new Frame();

                StaffSalaries_Page ss = new StaffSalaries_Page() { ShowsNavigationUI = true };
                topFrame.Content = ss;
                LayoutAnchorable la = new LayoutAnchorable();
                la.Title = "员工工资查询";
                la.Content = topFrame;

                ldp.Children.Add(la);
                la.IsSelected = true;
            }
            else
            {
            }
            this.Cursor = Cursors.Arrow;
        }
        #endregion

        /// <summary>
        /// 设置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_Settings_Click(object sender, RoutedEventArgs e)
        {
            //_10LinesFlowCard_Window _10lfc = new _10LinesFlowCard_Window("PT-20150919-0014");
            //_10lfc.ShowDialog();
            ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
            MessageBox.Show(User_Info.server[1] + "\r\n" + User_Info.database[1] + "\r\n" + User_Info.server[0] + "\r\n" + User_Info.database[0] + "\r\n" + "172.16.100.46" + "\r\n" + "程序集版本：" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\r\n" + ad.CurrentVersion.ToString());
        }
    }
}
