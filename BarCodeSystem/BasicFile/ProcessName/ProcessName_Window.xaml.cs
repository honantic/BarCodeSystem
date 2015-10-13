using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Data;
using BarCodeSystem.PublicClass.HelperClass;

namespace BarCodeSystem
{
    /// <summary>
    /// ProcessName_Window.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessName_Window : Window
    {
        public ProcessName_Window()
        {
            InitializeComponent();
        }

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        List<ProcessNameLists> listBeforeSearch = new List<ProcessNameLists> { };

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //这段代码在正式环境中将被注释掉，测试用
            //MyDBController.Server = User_Info.server[1];
            //MyDBController.Database = User_Info.database[1];
            //MyDBController.Pwd = User_Info.pwd[1];
            //MyDBController.Uid = User_Info.uid[1];
            this.Title = User_Info.User_WorkcenterName + this.Title;
            GetBCSProcessLists();
        }

        /// <summary>
        /// 获取条码系统的工序信息
        /// </summary>
        private void GetBCSProcessLists()
        {
            MyDBController.GetConnection();
            ds.Clear();
            listBeforeSearch.Clear();
            string SQl = string.Format(@"SELECT [ID],[PN_Code],[PN_Name],[PN_CodeInWorkCenter],[PN_WorkCenterID] FROM [ProcessName] where [PN_WorkCenterID]={0}", User_Info.User_Workcenter_ID);
            dt = MyDBController.GetDataSet(SQl, ds, "ProcessName").Tables[0];
            MyDBController.CloseConnection();

            int x = dt.Rows.Count;
            for (int i = 0; i < x; i++)
            {
                ProcessNameLists pnl = new ProcessNameLists();
                pnl.ID = (Int64)dt.Rows[i]["ID"];
                pnl.PN_Name = dt.Rows[i]["PN_Name"].ToString();
                pnl.PN_Code = dt.Rows[i]["PN_Code"].ToString();
                pnl.PN_CodeInWorkCenter = dt.Rows[i]["PN_CodeInWorkCenter"].ToString();
                if (!(dt.Rows[i]["PN_WorkCenterID"] is DBNull))
                {
                    pnl.PN_WorkCenterID = Convert.ToInt64(dt.Rows[i]["PN_WorkCenterID"]);
                }
                listBeforeSearch.Add(pnl);
            }
            listBeforeSearch = listBeforeSearch.OrderBy(p => p.PN_Code).ToList();
            listview1.ItemsSource = null;
            listview1.ItemsSource = listBeforeSearch;
        }

        /// <summary>
        /// 增加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            ProcessNameModify_Window pnm = new ProcessNameModify_Window();
            pnm.existProcess = dt;
            pnm.ShowDialog();
            if ((bool)pnm.DialogResult)
            {
                GetBCSProcessLists();
            }
        }

        /// <summary>
        /// 修改按钮,当勾选的信息和listview.selecteditem不一致的时候
        /// 优先勾选的，当有多条勾选的时候，优先排序考前的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            ProcessNameModify_Window pnm = new ProcessNameModify_Window();
            ProcessNameLists pnl = null;
            foreach (ProcessNameLists item in listview1.Items)
            {
                if (item.IsSelected)
                {
                    pnl = item;
                    break;
                }
            }

            if (pnl != null)
            {
                pnm.pnl = pnl;
                pnm.existProcess = dt;
                pnm.ShowDialog();
                if ((bool)pnm.DialogResult)
                {
                    GetBCSProcessLists();
                }
            }
            else
            {
                pnl = listview1.SelectedItem as ProcessNameLists;
                if (pnl != null)
                {
                    pnm.pnl = pnl;
                    pnm.existProcess = dt;
                    pnm.ShowDialog();
                    if ((bool)pnm.DialogResult)
                    {
                        GetBCSProcessLists();
                    }
                }

            }
        }

        /// <summary>
        /// 导入按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            ProcessNameImport_Window pni = new ProcessNameImport_Window();
            pni.exisitProcessName = dt;
            pni.ShowDialog();
            if ((bool)pni.DialogResult)
            {
                GetBCSProcessLists();
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (ProcessNameLists item in listview1.Items)
            {
                item.IsSelected = true;
            }
            listview1.Items.Refresh();
        }

        /// <summary>
        /// 重选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReSelect_Click(object sender, RoutedEventArgs e)
        {
            foreach (ProcessNameLists item in listview1.Items)
            {
                item.IsSelected = false;
            }
            listview1.Items.Refresh();
        }

        /// <summary>
        /// 搜索文本框改变事件，关联搜索按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_SearchKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            btn_Search_Click(sender, e);
        }

        /// <summary>
        /// 搜索按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            listview1.ItemsSource = null;
            listview1.ItemsSource = listBeforeSearch;

            if (txtb_SearchKey.Text.Length > 0)
            {
                string key = txtb_SearchKey.Text;
                List<ProcessNameLists> pnls = new List<ProcessNameLists> { };
                IEnumerable<ProcessNameLists> IEpnls =
                    from item in listBeforeSearch
                    where (item.PN_Code.IndexOf(key) != -1 || item.PN_Name.IndexOf(key) != -1)
                    select item;
                foreach (ProcessNameLists item in IEpnls)
                {
                    pnls.Add(item);
                }
                listview1.ItemsSource = null;
                listview1.ItemsSource = pnls;
            }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// listview1 鼠标双击事件，关联修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Point formPoint = e.GetPosition(this);
            if (formPoint.Y > 60 && formPoint.Y < 90)//表头部分不作相应
            {

            }
            else
            {
                btn_Modify_Click(sender, e);
            }
        }

        /// <summary>
        /// 删除选中信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            bool flag = false;
            foreach (ProcessNameLists pnl in listview1.ItemsSource)
            {
                if (pnl.IsSelected)
                {
                    flag = true;
                    break;
                }
            }

            if (flag)
            {
                if (Xceed.Wpf.Toolkit.MessageBox.Show("确定要删除选定项吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    MyDBController.GetConnection();
                    List<ProcessNameLists> pnList = new List<ProcessNameLists>();
                    List<DBLog> _dbLogList = new List<DBLog>();
                    foreach (ProcessNameLists pnl in listview1.ItemsSource)
                    {
                        if (pnl.IsSelected)
                        {
                            string SQl = string.Format(@"delete from [ProcessName] where [ID]={0}", pnl.ID);
                            MyDBController.ExecuteNonQuery(SQl);
                            DBLog _dbLog = new DBLog();
                            _dbLog.DBL_OperateBy = User_Info.User_Code + "|" + User_Info.User_Name;
                            _dbLog.DBL_OperateTable = "ProcessName";
                            _dbLog.DBL_OperateTime = DateTime.Now.ToString();
                            _dbLog.DBL_OperateType = OperateType.Delete;
                            _dbLog.DBL_Content = "删除工序:" + pnl.PN_Code;
                            _dbLogList.Add(_dbLog);
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row["ID"].ToString() == pnl.ID.ToString())
                                {
                                    dt.Rows.Remove(row);
                                    break;
                                }
                            }

                        }
                        else
                        {
                            pnList.Add(pnl);
                        }
                    }
                    Xceed.Wpf.Toolkit.MessageBox.Show("删除成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    listview1.ItemsSource = pnList;
                    listview1.Items.Refresh();
                    MyDBController.CloseConnection();
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("没有选定任何要删除的项！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Cursor = Cursors.Arrow;
        }


    }
}
