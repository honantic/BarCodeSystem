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
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Data;
using BarCodeSystem.PublicClass.HelperClass;

namespace BarCodeSystem
{
    /// <summary>
    /// Authority_Window.xaml 的交互逻辑
    /// </summary>
    public partial class Authority_Window : Window
    {
        private bool IsChanged = false;
        private bool IsSaved = false;
        int count = 0;
        DataSet ds = new DataSet();

        Int64? AccountID;

        /// <summary>
        /// 账号权限列表
        /// </summary>
        List<UserAuthorityLists> AuthorityList = new List<UserAuthorityLists> { };

        /// <summary>
        /// 已经查询过权限的账号列表
        /// </summary>
        List<string> AccountList = new List<string> { };

        /// <summary>
        /// 单选框列表，用来操作窗体中的单选框
        /// </summary>
        List<CheckBox> cbl = new List<CheckBox> { };

        /// <summary>
        /// 账号列表,用来显示账号信息
        /// </summary>
        List<UserAccountLists> uals = new List<UserAccountLists> { };

        public Authority_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetInitShow();

            //去除关闭按钮
            //2.在装载事件中加入
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

        }


        //去除关闭按钮
        //1.Window 类中申明
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        /// <summary>
        /// 初始化信息
        /// </summary>
        private void SetInitShow()
        {
            ExpandAllExpander();
            GetBCSAccount();
        }


        /// <summary>
        /// 根据当前账号的权限，对权限列表进行管控。即当前账号没有权限的菜单，操作者就没有相应权限的授权功能
        /// </summary>
        private void SetCheckBoxReadOnly()
        {
            if (!User_Info.User_Code.Equals("admin"))
            {
                List<UserAuthorityLists> ualList = UserAuthorityLists.FetchUAListByUserID(User_Info.Account_ID);
                List<string> uaNameList = new List<string>();
                ualList.ForEach(p => { uaNameList.Add(p.SA_AuthorityName.Split('_')[1]); });
                List<CheckBox> cbList = MyDBController.FindVisualChild<CheckBox>(this);
                cbList.ForEach(
                    p =>
                    {
                        if (uaNameList.Contains(p.Name.Split('_')[1]) || p.Name.Equals("CB_SelectAll"))
                        {

                        }
                        else
                        {
                            p.IsEnabled = false;
                        }
                    });
            }
        }


        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            if (IsChanged && !IsSaved)
            {
                if (MessageBox.Show("您还没有保存，确定要退出吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information)
                    == MessageBoxResult.OK)
                {
                    this.DialogResult = true;
                }
            }
            else
            {
                this.DialogResult = false;
            }
        }

        /// <summary>
        /// 监听所有单选框IsChecked状态
        /// </summary>
        private void BindCheckBoxChange()
        {
            cbl = MyDBController.FindVisualChild<CheckBox>(this);
            foreach (CheckBox cb in cbl)
            {
                cb.Click += new RoutedEventHandler(cb_Click);
            }
        }

        /// <summary>
        /// 单选框点击事件，点击说明IsChecked改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_Click(object sender, RoutedEventArgs e)
        {
            IsChanged = true;
        }

        /// <summary>
        /// 展开所有的折叠框
        /// </summary>
        private void ExpandAllExpander()
        {
            foreach (Expander item in MyDBController.FindVisualChild<Expander>(this))
            {
                item.IsExpanded = true;
            }
        }

        /// <summary>
        /// 获取条码系统账号信息
        /// </summary>
        private void GetBCSAccount()
        {
            ds.Clear();
            string SQl = string.Format(@"SELECT [ID],[UA_LoginAccount],[UA_UserName],[UA_IsValidated] FROM
                                [UserAccount] where [UA_IsValidated]='true' and [UA_LoginAccount] !='admin'");
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "UserAccount");
            MyDBController.CloseConnection();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                UserAccountLists ual = new UserAccountLists()
                {
                    ID = (Int64)ds.Tables["UserAccount"].Rows[i]["ID"],
                    UA_LoginAccount = ds.Tables["UserAccount"].Rows[i]["UA_LoginAccount"].ToString(),
                    UA_IsValidated = (bool)ds.Tables["UserAccount"].Rows[i]["UA_IsValidated"],
                    UA_UserName = ds.Tables["UserAccount"].Rows[i]["UA_UserName"].ToString()
                };
                uals.Add(ual);
            }
            DBLog _dbLog = new DBLog();
            _dbLog.DBL_Content = User_Info.User_Name + "|在权限管理界面中，查询条码系统账号列表";
            _dbLog.DBL_OperateBy = User_Info.User_Code;
            _dbLog.DBL_OperateTime = DateTime.Now.ToString();
            _dbLog.DBL_OperateType = OperateType.Select;
            _dbLog.DBL_OperateTable = "UserAccount";
            DBLog.WriteDBLog(_dbLog);
            Listview1.ItemsSource = null;
            Listview1.ItemsSource = uals;
        }

        /// <summary>
        /// 窗体鼠标移动事件，第一次移动的时候为所有单选框加载监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (count == 0)
            {
                SetCheckBoxReadOnly();
                BindCheckBoxChange();
                count++;
            }
        }

        /// <summary>
        /// 全选 单选框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            foreach (CheckBox item in cbl)
            {
                if (item.IsEnabled)
                {
                    item.IsChecked = cb.IsChecked;
                }
            }
        }

        /// <summary>
        /// 选中项改变的时候,修改右边文本框信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Listview1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int x = Listview1.SelectedIndex;
            if (x > -1)
            {
                txtb_Account.Text = ((UserAccountLists)Listview1.Items[x]).UA_LoginAccount;
                txtb_Name.Text = ((UserAccountLists)Listview1.Items[x]).UA_UserName;
                txtb_Department.Text = ((UserAccountLists)Listview1.Items[x]).UA_DepartmentName;
                txtb_IsValidated.Text = ((UserAccountLists)Listview1.Items[x]).UA_IsValidated.ToString();
            }

        }


        /// <summary>
        /// 获得权限按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_GetAccountAuthority_Click(object sender, RoutedEventArgs e)
        {
            if (Listview1.SelectedItem != null)
            {
                string Account = txtb_Account.Text.Trim();
                //GetAccountAu(Account);
                //将已经查询过的账号的权限情况存下来，再次查询的时候直接从查询结果的列表中获得
                //不需要再进行一次数据库的链接查询操作，减少资源消耗
                if (!IsAccountExist(Account))
                {
                    AuthorityList = UserAuthorityLists.FetchUAListByAccount(Account);
                    //GetAccountAu(Account);
                }
                DisplayAthourity(Account);
            }
            else
            {
                MessageBox.Show("请在左边的列表中选择一个账号！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// 获得当前的账号的权限信息
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        private List<UserAuthorityLists> GetAccountAu(string Account)
        {
            //AuthorityList.Clear();
            string SQl = string.Format(@"select A.[SA_AuthorityName],B.[ID],B.[UA_UserAccountID],B.[UA_SysAuthorityID],C.[UA_LoginAccount] 
                                from [SystemAuthority] A
                                left join [UserAuthority] B on A.[ID]=B.[UA_SysAuthorityID]
                                left join [UserAccount] C on B.[UA_UserAccountID] = C.[ID]
                                where C.[UA_LoginAccount] = '{0}'", Account);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "UA_AuthorityName");
            MyDBController.CloseConnection();

            int x = ds.Tables["UA_AuthorityName"].Rows.Count;
            for (int i = 0; i < x; i++)
            {
                UserAuthorityLists ual = new UserAuthorityLists()
                {
                    SA_AuthorityName = ds.Tables["UA_AuthorityName"].Rows[i]["SA_AuthorityName"].ToString().Trim(),
                    ID = (Int64)ds.Tables["UA_AuthorityName"].Rows[i]["ID"],
                    UA_LoginAccount = ds.Tables["UA_AuthorityName"].Rows[i]["UA_LoginAccount"].ToString().Trim(),
                    UA_UserAccountID = (Int64)ds.Tables["UA_AuthorityName"].Rows[i]["UA_UserAccountID"],
                    UA_SysAuthorityID = (Int64)ds.Tables["UA_AuthorityName"].Rows[i]["UA_SysAuthorityID"]
                };
                AuthorityList.Add(ual);
            }
            AccountList.Add(Account);
            DBLog _dbLog = new DBLog();
            _dbLog.DBL_Content = User_Info.User_Name + "|在权限管理界面中，查询条码系统账号列表";
            _dbLog.DBL_OperateBy = User_Info.User_Code;
            _dbLog.DBL_OperateTime = DateTime.Now.ToString();
            _dbLog.DBL_OperateType = OperateType.Select;
            _dbLog.DBL_OperateTable = "UserAccount";
            DBLog.WriteDBLog(_dbLog);
            return AuthorityList;
        }

        /// <summary>
        /// 检查当前账号是否已经查询过了，查询过了返回true，否则返回false
        /// </summary>
        /// <returns></returns>
        private bool IsAccountExist(string Account)
        {
            bool IsExist = false;
            foreach (string item in AccountList)
            {
                if (string.Equals(item, Account))
                {
                    IsExist = true;
                    break;
                }
            }
            return IsExist;
        }


        /// <summary>
        /// 在窗体中展示当前账号的权限信息，拥有的权限，在相应的单选框上打钩
        /// </summary>
        /// <param name="Account"></param>
        private void DisplayAthourity(string Account)
        {
            IEnumerable<UserAuthorityLists> uals =
                from item in AuthorityList
                where string.Equals(item.UA_LoginAccount, Account)
                select item;

            foreach (CheckBox cb in cbl)
            {
                cb.IsChecked = false;
            }

            foreach (UserAuthorityLists item in uals)
            {
                foreach (CheckBox cb in cbl)
                {
                    string str1 = cb.Name.Split('_')[1];
                    string str2 = item.SA_AuthorityName.Split('_')[1];
                    if (string.Equals(str1, str2))
                    {
                        cb.IsChecked = true;
                        break;
                    }
                }
            }
            label_AcountInfo.Text = string.Format("|当前账号为:{0}|姓名为:{1}", Account, txtb_Name.Text);
            AccountID = ((UserAccountLists)Listview1.SelectedItem).ID;
        }

        /// <summary>
        /// 搜索框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_SearchKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            string key = txtb_SearchKey.Text.Trim();
            Listview1.ItemsSource = null;
            Listview1.ItemsSource = uals;
            if (!string.IsNullOrEmpty(key))
            {
                try
                {
                    IEnumerable<UserAccountLists> list =
                        from item in uals
                        where item.UA_LoginAccount.IndexOf(key) != -1
                            || item.UA_UserName.IndexOf(key) != -1 || item.UA_IsValidated.ToString().IndexOf(key) != -1
                        select item;
                    Listview1.ItemsSource = list;
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (AccountID != null && IsChanged)
            {
                IEnumerable<string> AuList = GetChoosedAu();
                DataTable dt = GetSysAu();
                if (AuList.Count<string>() == 0)
                {
                    if (MessageBox.Show("该账号没有任何权限\n确定要保存吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information)
                        == MessageBoxResult.OK)
                    {
                        SaveAu(AuList, dt);
                    }
                }
                else
                {
                    SaveAu(AuList, dt);
                }
            }
            else
            {
                string info = (AccountID == null) ? "请先选择一个账号！" : "您没有对该账号做任何权限修改！";
                MessageBox.Show(info, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 获得需要保存的权限清单
        /// </summary>
        private IEnumerable<string> GetChoosedAu()
        {
            IEnumerable<string> AuList =
                    from CheckBox item in cbl
                    where (bool)item.IsChecked
                    select item.Name.Split('_')[1];
            return AuList;
        }

        /// <summary>
        /// 获得系统的权限清单
        /// </summary>
        /// <returns></returns>
        private DataTable GetSysAu()
        {
            string SQl = string.Format("select [ID],[SA_AuthorityName] from SystemAuthority");
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "SystemAuthority");
            MyDBController.CloseConnection();
            return ds.Tables["SystemAuthority"];
        }

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="AuList"></param>
        /// <param name="SysAu"></param>
        private void SaveAu(IEnumerable<string> AuList, DataTable SysAu)
        {
            try
            {
                int x = SysAu.Rows.Count;
                MyDBController.GetConnection();
                string SQl = string.Format(@"delete from [UserAuthority] where UA_UserAccountID={0}", AccountID);
                MyDBController.ExecuteNonQuery(SQl);
                DBLog _dbLog = new DBLog();
                _dbLog.DBL_Content = User_Info.User_Name + "|在权限管理界面中，删除表 [UserAuthority]中[UA_UserAccountID]为" + AccountID.ToString() + "的账号的权限";
                _dbLog.DBL_OperateBy = User_Info.User_Code;
                _dbLog.DBL_OperateTime = DateTime.Now.ToString();
                _dbLog.DBL_OperateType = OperateType.Delete;
                _dbLog.DBL_OperateTable = "UserAccount";
                DBLog.WriteDBLog(_dbLog);
                #region
                DataTable dt = new DataTable();
                dt.TableName = "UserAuthority";
                dt.Columns.Add("ID", typeof(Int64));
                dt.Columns.Add("UA_UserAccountID", typeof(Int64));
                dt.Columns.Add("UA_SysAuthorityID", typeof(Int64));
                dt.Columns.Add("IDNew", typeof(Int64));
                List<string> colList = new List<string> { "ID", "UA_UserAccountID", "UA_SysAuthorityID" };
                #endregion

                #region
                _dbLog = new DBLog();
                _dbLog.DBL_Content = User_Info.User_Name + "|在权限管理界面中，在表 [UserAuthority]中新增[UA_UserAccountID]为" + AccountID.ToString() + "的账号的权限";
                _dbLog.DBL_OperateBy = User_Info.User_Code;
                _dbLog.DBL_OperateTime = DateTime.Now.ToString();
                _dbLog.DBL_OperateType = OperateType.Insert;
                _dbLog.DBL_OperateTable = "UserAccount";
                foreach (string item in AuList)
                {
                    for (int i = 0; i < x; i++)
                    {
                        if (string.Equals(item, SysAu.Rows[i]["SA_AuthorityName"].ToString().Split('_')[1]))
                        {
                            DataRow dr = dt.NewRow();
                            dr["UA_UserAccountID"] = AccountID;
                            dr["UA_SysAuthorityID"] = (Int64)SysAu.Rows[i]["ID"];
                            _dbLog.DBL_AssociateID += string.IsNullOrEmpty(_dbLog.DBL_AssociateID) ? "权限ID为：" + dr["UA_SysAuthorityID"].ToString() + "|" : dr["UA_SysAuthorityID"] + "|";
                            dt.Rows.Add(dr);
                            break;
                        }
                    }
                }
                int updateNum, insertNum;
                MyDBController.InsertSqlBulk(dt, colList, out  updateNum, out  insertNum);
                DBLog.WriteDBLog(_dbLog);
                #endregion

                MyDBController.CloseConnection();
                IsSaved = true;
                if (MessageBox.Show("保存成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information) ==
                    MessageBoxResult.OK)
                {
                    RefreshAuList(AuList);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("保存失败！" + ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshAuList(IEnumerable<string> list)
        {
            IEnumerable<UserAuthorityLists> uals =
                from item in AuthorityList
                where item.UA_UserAccountID != AccountID
                select item;
            AuthorityList.Clear();
            foreach (UserAuthorityLists item in uals)
            {
                AuthorityList.Add(item);
            }
            foreach (string item in list)
            {
                AuthorityList.Add(new UserAuthorityLists { UA_LoginAccount = txtb_Account.Text.Trim(), UA_UserAccountID = (Int64)AccountID, SA_AuthorityName = "item_" + item });
            }
        }
    }
}
