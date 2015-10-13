using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Data;
using BarCodeSystem.PublicClass.HelperClass;

namespace BarCodeSystem
{
    /// <summary>
    /// U9User_Window.xaml 的交互逻辑
    /// </summary>
    public partial class U9User_Window : Window
    {
        DataSet ds = new DataSet();
        List<UserAccountLists> U9UserList = new List<UserAccountLists> { };
        List<UserAccountLists> BCSUserList = new List<UserAccountLists> { };
        List<UserAccountLists> ImportedUserList = new List<UserAccountLists> { };
        public U9User_Window()
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
            GetU9UserList();
            GetBCSUserList();
            U9ToBCSUserList();
        }

        /// <summary>
        /// 获取U9账号列表
        /// </summary>
        /// <returns></returns>
        private List<UserAccountLists> GetU9UserList()
        {
            U9UserList.Clear();
            ClearTable("U9UserList");
            WebService.Service ws = new WebService.Service();
            ds = ws.GetU9UserList("");
            ds.Tables["U9UserList"].Columns.Add("ID", typeof(Int64));
            ds.Tables["U9UserList"].Columns.Add("UA_IsValidated", typeof(bool));
            ds.Tables["U9UserList"].Columns.Add("UA_Verify", typeof(bool));
            ds.Tables["U9UserList"].Columns.Add("UA_PassWord", typeof(string));
            ds.Tables["U9UserList"].Columns.Add("IDNew", typeof(Int64));
            ds.Tables["U9UserList"].Columns["ID"].SetOrdinal(0);
            ds.Tables["U9UserList"].Columns["User_Code"].ColumnName = "UA_LoginAccount";
            ds.Tables["U9UserList"].Columns["User_DisplayName"].ColumnName = "UA_UserName";
            ds.Tables["U9UserList"].AcceptChanges();

            int x = ds.Tables["U9UserList"].Rows.Count;
            for (int i = 0; i < x; i++)
            {
                UserAccountLists ual = new UserAccountLists
                {
                    UA_LoginAccount = ds.Tables["U9UserList"].Rows[i]["UA_LoginAccount"].ToString().Trim(),
                    UA_UserName = ds.Tables["U9UserList"].Rows[i]["UA_UserName"].ToString().Trim()
                };
                U9UserList.Add(ual);
            }

            DBLog _dbLog = new DBLog();
            _dbLog.DBL_Content = User_Info.User_Name + "|在U9账号导入界面中，查询U9账号列表";
            _dbLog.DBL_OperateBy = User_Info.User_Code;
            _dbLog.DBL_OperateTime = DateTime.Now.ToString();
            _dbLog.DBL_OperateType = OperateType.Select;
            DBLog.WriteDBLog(_dbLog);
            return U9UserList;
        }

        /// <summary>
        /// 获取条码系统账号列表
        /// </summary>
        /// <returns></returns>
        private List<UserAccountLists> GetBCSUserList()
        {
            BCSUserList.Clear();
            ClearTable("BCSUserList");
            MyDBController.GetConnection();
            string SQl = string.Format(@"Select [ID],[UA_LoginAccount],[UA_UserName],[UA_IsValidated],[UA_Verify],[UA_PassWord],[ID] as [IDNew] from [UserAccount]");
            MyDBController.GetDataSet(SQl, ds, "BCSUserList");
            MyDBController.CloseConnection();
            int x = ds.Tables["BCSUserList"].Rows.Count;
            for (int i = 0; i < x; i++)
            {
                UserAccountLists ual = new UserAccountLists
                {
                    ID = (Int64)ds.Tables["BCSUserList"].Rows[i]["ID"],
                    UA_LoginAccount = ds.Tables["BCSUserList"].Rows[i]["UA_LoginAccount"].ToString().Trim(),
                    UA_UserName = ds.Tables["BCSUserList"].Rows[i]["UA_UserName"].ToString().Trim(),
                    UA_IsValidated = (bool)ds.Tables["BCSUserList"].Rows[i]["UA_IsValidated"]
                };
                BCSUserList.Add(ual);
            }
            listview2.ItemsSource = null;
            listview2.ItemsSource = BCSUserList;

            DBLog _dbLog = new DBLog();
            _dbLog.DBL_Content = User_Info.User_Name + "|在U9账号导入界面中，查询条码系统账号列表";
            _dbLog.DBL_OperateBy = User_Info.User_Code;
            _dbLog.DBL_OperateTime = DateTime.Now.ToString();
            _dbLog.DBL_OperateType = OperateType.Select;
            _dbLog.DBL_OperateTable = "UserAccount";
            DBLog.WriteDBLog(_dbLog);
            return BCSUserList;
        }

        /// <summary>
        /// ds中单个DataTable清除
        /// </summary>
        /// <param name="TableName"></param>
        private void ClearTable(string TableName)
        {
            foreach (DataTable item in ds.Tables)
            {
                if (string.Equals(item.TableName, TableName))
                {
                    item.Clear();
                    return;
                }
            }
        }

        /// <summary>
        /// 获取U9用户清单 按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_GetU9User_Click(object sender, RoutedEventArgs e)
        {
            U9ToBCSUserList();
        }

        /// <summary>
        /// 对获取的U9清单进行过滤，已经存在条码系统的中的账号排除
        /// </summary>
        /// <returns></returns>
        private List<UserAccountLists> U9ToBCSUserList()
        {
            U9UserList.RemoveAll(IsExist);
            listview1.ItemsSource = null;
            listview1.ItemsSource = U9UserList;
            return U9UserList;
        }

        /// <summary>
        /// List.Remove 方法重载
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsExist(UserAccountLists item)
        {
            bool IsExist = false;
            foreach (UserAccountLists iteml in BCSUserList)
            {
                if (string.Equals(item.UA_LoginAccount, iteml.UA_LoginAccount))
                {
                    IsExist = true;
                    break;
                }
            }
            return IsExist;
        }

        /// <summary>
        /// 搜索框文本改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_SearchKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            listview1.ItemsSource = U9UserList;
            string key = txtb_SearchKeyTab1.Text.Trim();
            IEnumerable<UserAccountLists> uals =
                from item in U9UserList
                where item.UA_LoginAccount.IndexOf(key) != -1 || item.UA_UserName.IndexOf(key) != -1
                        || item.UA_IsValidated.ToString().ToLower().IndexOf(key) != -1
                select item;
            listview1.ItemsSource = uals;
        }

        /// <summary>
        /// 获取条码系统账号 按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DisplayBCSUser_Click(object sender, RoutedEventArgs e)
        {
            listview2.ItemsSource = null;
            listview2.ItemsSource = GetBCSUserList();
        }


        /// <summary>
        /// 全选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (UserAccountLists item in listview1.ItemsSource)
            {
                item.IsSelected = true;
            }
            listview1.Items.Refresh();
        }


        /// <summary>
        /// 重选 按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReSelect_Click(object sender, RoutedEventArgs e)
        {
            foreach (UserAccountLists item in listview1.ItemsSource)
            {
                item.IsSelected = false;
            }
            listview1.Items.Refresh();
        }

        /// <summary>
        /// 导入U9用户按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ImportUser_Click(object sender, RoutedEventArgs e)
        {
            DataTable temp = FindItemInDT(GetSelectedItem(listview1), ds.Tables["U9UserList"], 0);
            temp.TableName = "UserAccount";
            if (temp.Rows.Count > 100)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("选择的账号不能超过100个！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                List<string> colList = new List<string> { "ID", "UA_LoginAccount", "UA_UserName", "UA_IsValidated", "UA_Verify", "UA_PassWord" };
                int updateNum = 0, InsertNum = 0;
                MyDBController.GetConnection();
                MyDBController.InsertSqlBulk(temp, colList, out  updateNum, out InsertNum);
                string info = string.Format(@"共成功导入条码系统 {0} 个账号！", InsertNum);
                MyDBController.CloseConnection();
                if (MessageBox.Show(info, "提示", MessageBoxButton.OK, MessageBoxImage.Information) ==
                    MessageBoxResult.OK)
                {
                    RefreshAfterImport(0);
                }

                DBLog _dbLog = new DBLog();
                _dbLog.DBL_Content = User_Info.User_Name + "|在U9账号导入界面中，将U9账号导入条码系统中，新增账号的ID和Code在DBL_AssociateID和DBL_AssociateCode中";
                _dbLog.DBL_OperateBy = User_Info.User_Code;
                _dbLog.DBL_OperateTime = DateTime.Now.ToString();
                _dbLog.DBL_OperateType = OperateType.Insert;
                _dbLog.DBL_OperateTable = "UserAccount";
                foreach (DataRow row in temp.Rows)
                {
                    _dbLog.DBL_AssociateID += string.IsNullOrEmpty(_dbLog.DBL_AssociateID) ? row["ID"].ToString() : "|" + row["ID"].ToString();
                    _dbLog.DBL_AssociateCode += string.IsNullOrEmpty(_dbLog.DBL_AssociateCode) ? row["UA_LoginAccount"].ToString() : "|" + row["UA_LoginAccount"].ToString();
                }
                DBLog.WriteDBLog(_dbLog);
            }
        }

        /// <summary>
        /// 启用用户按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Validate_Click(object sender, RoutedEventArgs e)
        {
            DataTable temp = FindItemInDT(GetSelectedItem(listview2), ds.Tables["BCSUserList"], 1);
            temp.TableName = "UserAccount";
            if (temp.Rows.Count > 100)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("选择的账号不能超过100个！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                List<string> colList = new List<string> { "ID", "UA_LoginAccount", "UA_UserName", "UA_IsValidated", "UA_Verify", "UA_PassWord" };
                int updateNum = 0, InsertNum = 0;
                MyDBController.GetConnection();
                MyDBController.InsertSqlBulk(temp, colList, out  updateNum, out InsertNum);
                string info = string.Format(@"共成功启用 {0} 个条码系统账号！", updateNum);
                MyDBController.CloseConnection();
                if (MessageBox.Show(info, "提示", MessageBoxButton.OK, MessageBoxImage.Information) ==
                    MessageBoxResult.OK)
                {
                    RefreshAfterImport(1);
                }

                DBLog _dbLog = new DBLog();
                _dbLog.DBL_Content = User_Info.User_Name + "|在U9账号导入界面中，启用条码系统账号，启用账号的ID和Code在DBL_AssociateID和DBL_AssociateCode中";
                _dbLog.DBL_OperateBy = User_Info.User_Code;
                _dbLog.DBL_OperateTime = DateTime.Now.ToString();
                _dbLog.DBL_OperateType = OperateType.Update;
                _dbLog.DBL_OperateTable = "UserAccount";
                foreach (DataRow row in temp.Rows)
                {
                    _dbLog.DBL_AssociateID += string.IsNullOrEmpty(_dbLog.DBL_AssociateID) ? row["ID"].ToString() : "|" + row["ID"].ToString();
                    _dbLog.DBL_AssociateCode += string.IsNullOrEmpty(_dbLog.DBL_AssociateCode) ? row["UA_LoginAccount"].ToString() : "|" + row["UA_LoginAccount"].ToString();
                }
                DBLog.WriteDBLog(_dbLog);
            }
        }

        /// <summary>
        /// 获得勾选的item
        /// </summary>
        /// <returns></returns>
        private List<UserAccountLists> GetSelectedItem(ListView lv)
        {
            ImportedUserList.Clear();
            foreach (UserAccountLists item in lv.ItemsSource)
            {
                if (item.IsSelected)
                {
                    ImportedUserList.Add(item);
                }
            }

            return ImportedUserList;
        }

        /// <summary>
        /// 获得将勾选的item在DataTable中找到，用来作为调用            
        /// MyDBController.InsertSqlBulk的参数之一
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable FindItemInDT(List<UserAccountLists> list, DataTable dt, int tabNum)
        {
            int x = dt.Rows.Count;
            int y = list.Count;
            DataTable temp = dt.Clone();
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (string.Equals(list[j].UA_LoginAccount, dt.Rows[i]["UA_LoginAccount"].ToString().Trim()))
                    {
                        dt.Rows[i]["IDNew"] = dt.Rows[i]["ID"];
                        dt.Rows[i]["UA_IsValidated"] = (tabNum == 0) ? false : true;
                        temp.ImportRow(dt.Rows[i]);
                        break;
                    }
                }
            }
            return temp;
        }

        /// <summary>
        /// 每次导入后刷新列表
        /// </summary>
        private void RefreshAfterImport(int tabNum)
        {
            switch (tabNum)
            {
                case 0:
                    U9UserList.RemoveAll(HasBennImported);
                    listview1.ItemsSource = null;
                    listview1.ItemsSource = U9UserList;
                    break;

                case 1:
                    foreach (UserAccountLists item in GetSelectedItem(listview2))
                    {
                        item.IsSelected = false;
                        item.UA_IsValidated = true;
                    }
                    listview2.Items.Refresh();
                    break;

                default:
                    break;
            }

        }

        /// <summary>
        /// List.RemoveAll方法重载。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool HasBennImported(UserAccountLists item)
        {
            bool HasBennImported = false;
            foreach (UserAccountLists iteml in ImportedUserList)
            {
                if (string.Equals(item.UA_LoginAccount, iteml.UA_LoginAccount))
                {
                    HasBennImported = true;
                    break;
                }
            }
            return HasBennImported;
        }

        /// <summary>
        /// 点击行,自动帮选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = (ListView)sender;
            foreach (var item in lv.SelectedItems)
            {
                ListViewItem iteml = (ListViewItem)lv.ItemContainerGenerator.ContainerFromItem(item);
                List<CheckBox> cbs = MyDBController.FindVisualChild<CheckBox>(iteml);
                foreach (CheckBox cb in cbs)
                {
                    cb.IsChecked = !cb.IsChecked;
                }
            }
        }

        /// <summary>
        /// 第二个tab页的全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectAllTab2_Click(object sender, RoutedEventArgs e)
        {
            List<CheckBox> cbs = MyDBController.FindVisualChild<CheckBox>(listview2);
            foreach (CheckBox cb in cbs)
            {
                cb.IsChecked = true;
            }
        }

        /// <summary>
        /// 第二个tab页的重选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReSelectTab2_Click(object sender, RoutedEventArgs e)
        {
            List<CheckBox> cbs = MyDBController.FindVisualChild<CheckBox>(listview2);
            foreach (CheckBox cb in cbs)
            {
                cb.IsChecked = false;
            }
        }

        /// <summary>
        /// 第二个tab页搜索框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_SearchKeyTab2_TextChanged(object sender, TextChangedEventArgs e)
        {
            listview2.ItemsSource = BCSUserList;
            string key = txtb_SearchKeyTab2.Text.Trim();
            IEnumerable<UserAccountLists> uals =
                from item in BCSUserList
                where item.UA_LoginAccount.IndexOf(key) != -1 || item.UA_UserName.IndexOf(key) != -1
                        || item.UA_IsValidated.ToString().ToLower().IndexOf(key) != -1
                select item;
            listview2.ItemsSource = uals;
        }

        /// <summary>
        /// 删除选中账号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            int x = GetSelectedItem(listview2).Count;
            if (x > 1)
            {
                string info = string.Format("当前选中了{0}个账号！你确定要全部删除吗?\n删除操作不可恢复，而且操作时间较长。", x);
                if (MessageBox.Show(info, "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information)
                    == MessageBoxResult.OK)
                {
                    ExacuteDel();
                }
            }
            else
            {
                ExacuteDel();
            }
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        private void ExacuteDel()
        {
            int count = 0;
            MyDBController.GetConnection();

            DBLog _dbLog = new DBLog();
            _dbLog.DBL_Content = User_Info.User_Name + "|在U9账号导入界面中，删除账号，删除账号的ID和Code在DBL_AssociateID和DBL_AssociateCode中。并且在权限表中，删除了相关的权限。";
            _dbLog.DBL_OperateBy = User_Info.User_Code;
            _dbLog.DBL_OperateTime = DateTime.Now.ToString();
            _dbLog.DBL_OperateType = OperateType.Delete;
            _dbLog.DBL_OperateTable = "UserAccount|UserAuthority";
            foreach (UserAccountLists item in ImportedUserList)
            {
                string SQl = string.Format(@"delete from [UserAccount] where [ID]={0}", item.ID);
                count += MyDBController.ExecuteNonQuery(SQl);
                SQl = string.Format(@"delete from [UserAuthority] where [UA_UserAccountID]={0}", item.ID);
                MyDBController.ExecuteNonQuery(SQl);
                _dbLog.DBL_AssociateID += string.IsNullOrEmpty(_dbLog.DBL_AssociateID) ? item.ID.ToString() : "|" + item.ID.ToString();
                _dbLog.DBL_AssociateCode += string.IsNullOrEmpty(_dbLog.DBL_AssociateCode) ? item.UA_LoginAccount : "|" + item.UA_LoginAccount;
            }
            DBLog.WriteDBLog(_dbLog);
            MyDBController.CloseConnection();
            string successInfo = string.Format("共成功删除{0}个账号！", ImportedUserList.Count);
            string failInfo = string.Format("共成功删除{0}个账号！\n{1}个账号删除失败！", count, ImportedUserList.Count - count);

            string info = (ImportedUserList.Count - count) == 0 ? successInfo : failInfo;

            if (MessageBox.Show(info, "提示", MessageBoxButton.OK, MessageBoxImage.Information) ==
                MessageBoxResult.OK)
            {
                GetBCSUserList();
            }

        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
