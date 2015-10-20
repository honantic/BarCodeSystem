using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.ValueConverters;
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

namespace BarCodeSystem.SystemManage.AuthorityManagement
{
    /// <summary>
    /// AuthorityManagement_Page.xaml 的交互逻辑
    /// </summary>
    public partial class AuthorityManagement_Page : Page
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public AuthorityManagement_Page()
        {
            InitializeComponent();
        }
        #region 变量
        /// <summary>
        /// 加载次数
        /// </summary>
        private int loadCount = 0;

        /// <summary>
        /// 布尔转换器
        /// </summary>
        TrueOrFalseConverter tofc = new TrueOrFalseConverter();

        /// <summary>
        /// 账号列表
        /// </summary>
        private List<UserAccountLists> ualList = new List<UserAccountLists>();

        /// <summary>
        /// 系统菜单权限列表
        /// </summary>
        List<SystemAuthority> saList = new List<SystemAuthority>();

        /// <summary>
        /// 系统菜单列表
        /// </summary>
        List<MenuItem> sysMenuItemList = new List<MenuItem>();
        /// <summary>
        /// checkbox列表
        /// </summary>
        List<CheckBox> cbList = new List<CheckBox>();

        /// <summary>
        /// expander列表
        /// </summary>
        List<Expander> expList = new List<Expander>();

        /// <summary>
        /// 账号权限列表
        /// </summary>
        List<UserAuthorityLists> uauthoritylList = new List<UserAuthorityLists>();
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
                FetchAccountInfoList(User_Info.User_Workcenter_ID);
                tb_SearchInfo.Focus();
                InitSACheckBox();
                btn_Save.Visibility = Visibility.Hidden;
                btn_ReSet.Visibility = Visibility.Hidden;
                btn_Cancel.Visibility = Visibility.Hidden;
                btn_Modify.Visibility = Visibility.Hidden;
                loadCount++;
            }
        }

        /// <summary>
        /// 根据车间id获取账号列表
        /// </summary>
        private void FetchAccountInfoList(Int64 _WCID)
        {
            if (User_Info.User_Code.Equals("admin"))
            {
                ualList = UserAccountLists.FetchUAInfoByWCID();
            }
            else
            {
                ualList = UserAccountLists.FetchUAInfoByWCID(_WCID);
            }
            ualList = ualList.FindAll(p => p.UA_LoginAccount != User_Info.User_Code && p.UA_IsValidated);
            dg_UserAccountInfo.ItemsSource = ualList;
        }

        /// <summary>
        /// 获取条码系统菜单信息，并且根据菜单信息初始化checkbox
        /// </summary>
        private void InitSACheckBox()
        {
            uauthoritylList = UserAuthorityLists.FetchUAListByAccount(User_Info.User_Code);
            saList = SystemAuthority.FetchSAInfo();
            sysMenuItemList = ((Main_Window)App.Current.MainWindow).GetMainMenuItemList();
            FormCheckBox(saList, sysMenuItemList);
        }


        /// <summary>
        /// 初始化checkbox
        /// </summary>
        /// <param name="_saList"></param>
        private void FormCheckBox(List<SystemAuthority> _saList, List<MenuItem> _miList)
        {
            _miList.ForEach(
                  p =>
                  {
                      SystemAuthority sa = _saList.Find(item => item.SA_AuthorityCode.Equals(p.Name));
                      if (p.HasItems)
                      {
                          Expander exp = new Expander() { Header = sa.SA_AuthorityName, Cursor = Cursors.Hand, IsExpanded = true };
                          exp.SetValue(Button.StyleProperty, Application.Current.FindResource("Exp_NormalStyle"));
                          StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };
                          exp.Content = sp;
                          expList.Add(exp);
                          sp_AuthorityList.Children.Add(exp);
                      }
                      else
                      {
                          if (p.Visibility == Visibility.Visible)
                          {
                              CheckBox cb = new CheckBox() { Name = sa.SA_AuthorityCode, Content = sa.SA_AuthorityName, Cursor = Cursors.Hand };
                              cb.SetValue(CheckBox.StyleProperty, Application.Current.FindResource("CB_NormalStyle"));
                              cb.IsEnabled = false;
                              ((StackPanel)(expList.Last().Content)).Children.Add(cb);
                              cbList.Add(cb);
                          }
                      }
                  }
                );
            //saList.ForEach(
            //    p =>
            //    {
            //        if (p.SA_HasItems)
            //        {
            //            Expander exp = new Expander() { Header = p.SA_AuthorityName, Cursor = Cursors.Hand, IsExpanded = true };
            //            exp.SetValue(Button.StyleProperty, Application.Current.FindResource("Exp_NormalStyle"));
            //            StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };
            //            exp.Content = sp;
            //            expList.Add(exp);
            //            sp_AuthorityList.Children.Add(exp);
            //        }
            //        else
            //        {
            //            if (p.SA_IsVisible)
            //            {
            //                CheckBox cb = new CheckBox() { Name = p.SA_AuthorityCode, Content = p.SA_AuthorityName, Cursor = Cursors.Hand };
            //                cb.SetValue(CheckBox.StyleProperty, Application.Current.FindResource("CB_NormalStyle"));
            //                cb.IsEnabled = false;
            //                ((StackPanel)(expList.Last().Content)).Children.Add(cb);
            //                cbList.Add(cb);
            //            }
            //        }
            //    });

        }

        /// <summary>
        /// 搜索账号信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_UserAccountSearch_Click(object sender, RoutedEventArgs e)
        {
            string key = tb_SearchInfo.Text;
            dg_UserAccountInfo.ItemsSource = ualList.FindAll(p => p.UA_LoginAccount.IndexOf(key) != -1 || p.UA_UserName.IndexOf(key) != -1);
            dg_UserAccountInfo.Items.Refresh();
        }

        /// <summary>
        /// 快捷搜索账号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_SearchInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_UserAccountSearch_Click(null, null);
            }
        }

        /// <summary>
        /// 选定账号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Select_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (dg_UserAccountInfo.SelectedIndex != -1)
            {
                ShowSelectedAccountInfo((UserAccountLists)dg_UserAccountInfo.SelectedItem);
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_UserAccountInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btn_Select_Click(null, null);
        }


        /// <summary>
        /// 根据登录账号的权限列表，启用checkbox，暂时用不到，在修改的时候启用
        /// </summary>
        private void EnableCheckBoxByAuthority()
        {
            if (!User_Info.User_Code.Equals("admin"))
            {
                uauthoritylList = UserAuthorityLists.FetchUAListByAccount(User_Info.User_Code);
                uauthoritylList.ForEach(
                   p =>
                   {
                       if (cbList.Count(item => item.Name.Equals(p.SA_AuthorityCode)) > 0)
                       {
                           cbList.Find(item => item.Name.Equals(p.SA_AuthorityCode)).IsEnabled = true;
                       }
                   }
                   );
            }
        }

        /// <summary>
        /// 展示选中的账号信息
        /// </summary>
        private void ShowSelectedAccountInfo(UserAccountLists _ual)
        {
            tb_DepartmentName.Text = _ual.UA_DepartmentName;
            tb_IsValidated.Text = tofc.Convert(_ual.UA_IsValidated, typeof(string), null, new System.Globalization.CultureInfo("")).ToString();
            tb_LoginAccount.Text = _ual.UA_LoginAccount;
            tb_UserName.Text = _ual.UA_UserName;
            tb_Position.Text = _ual.P_Position;
            List<UserAuthorityLists> _ualList = UserAuthorityLists.FetchUAListByAccount(_ual.UA_LoginAccount);
            cbList.ForEach(p => p.IsChecked = false);
            _ualList.ForEach(
                p =>
                {
                    if (cbList.Exists(item => item.Name.Equals(p.SA_AuthorityCode)))
                    {
                        CheckBox cb = cbList.Find(item => item.Name.Equals(p.SA_AuthorityCode));
                        cb.IsChecked = true;
                    }
                });
            ChangCBState(false);
            btn_Save.Visibility = Visibility.Hidden;
            btn_ReSet.Visibility = Visibility.Hidden;
            btn_Cancel.Visibility = Visibility.Hidden;
            btn_Modify.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 全选/取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            cbList.ForEach(p => { p.IsChecked = CB_SelectAll.IsChecked & p.IsEnabled; });
        }

        /// <summary>
        /// 折叠/展开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ExpandAll_Click(object sender, RoutedEventArgs e)
        {
            if (btn_ExpandAll.Content.Equals("全部折叠"))
            {
                expList.ForEach(p => { p.IsExpanded = false; });
                btn_ExpandAll.Content = "全部展开";
            }
            else if (btn_ExpandAll.Content.Equals("全部展开"))
            {
                expList.ForEach(p => { p.IsExpanded = true; });
                btn_ExpandAll.Content = "全部折叠";
            }
            else
            {
            }
        }

        /// <summary>
        /// 重置选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReSet_Click(object sender, RoutedEventArgs e)
        {
            cbList.ForEach(p => p.IsChecked = false);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            List<UserAuthorityLists> _ualList = new List<UserAuthorityLists>();
            UserAccountLists userAccount = (UserAccountLists)dg_UserAccountInfo.SelectedItem;
            cbList.ForEach(
                p =>
                {
                    if (p.IsChecked == true)
                    {
                        SystemAuthority sa = saList.Find(item => item.SA_AuthorityCode.Equals(p.Name));
                        _ualList.Add(new UserAuthorityLists() { SA_AuthorityCode = sa.SA_AuthorityCode, SA_AuthorityName = sa.SA_AuthorityName, UA_LoginAccount = userAccount.UA_LoginAccount, UA_SysAuthorityID = sa.ID, UA_UserAccountID = userAccount.ID });
                    }
                });
            bool flag = UserAuthorityLists.SaveUAInfo(_ualList);
            if (flag)
            {
                btn_Save.Visibility = Visibility.Hidden;
                btn_Cancel.Visibility = Visibility.Hidden;
                btn_ReSet.Visibility = Visibility.Hidden;
                ChangCBState(false);
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            btn_Save.Visibility = Visibility.Visible;
            btn_ReSet.Visibility = Visibility.Visible;
            btn_Cancel.Visibility = Visibility.Visible;
            ChangCBState(true);
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 改变cb的状态
        /// </summary>
        /// <param name="_state"></param>
        private void ChangCBState(bool _state)
        {
            cbList.ForEach(
               p =>
               {
                   if (User_Info.User_Code.Equals("admin"))
                   {
                       p.IsEnabled = _state;
                   }
                   else
                   {
                       if (uauthoritylList.Count(item => item.SA_AuthorityCode.Equals(p.Name)) > 0)
                       {
                           p.IsEnabled = _state;
                       }
                   }
               });
        }

        /// <summary>
        /// 放弃修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            btn_Save.Visibility = Visibility.Hidden;
            btn_ReSet.Visibility = Visibility.Hidden;
            btn_Cancel.Visibility = Visibility.Hidden;
            ChangCBState(false);
            this.Cursor = Cursors.Arrow;
        }
    }
}
