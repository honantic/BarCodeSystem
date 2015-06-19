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
using System.Data;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace BarCodeSystem
{
    /// <summary>
    /// WorkCenterImport_Window.xaml 的交互逻辑
    /// </summary>
    public partial class WorkCenterImport_Window : Window
    {
        //U9数据来源
        DataTable U9dt = new DataTable();
        DataSet ds = new DataSet();
        //条码数据来源
        DataTable BCSdt = new DataTable();
        List<WorkCenterLists> listbeforesearch = new List<WorkCenterLists> { };
        public WorkCenterImport_Window()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 装载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //去除关闭按钮
            //2.在装载事件中加入
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            //这段代码在正式环境中将被注释掉，测试用
            //MyDBController.Server = User_Info.server[1];
            //MyDBController.Database = User_Info.database[1];
            //MyDBController.Pwd = User_Info.pwd[1];
            //MyDBController.Uid = User_Info.uid[1];

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
        /// 为item绑定数据源
        /// </summary>
        private void BindingItemSource()
        {
            U9dt = GetU9DepartmentList();
            BCSdt = GetBCSDepartmentList();
            bool isexsist = false;
            List<WorkCenterLists> wcl = new List<WorkCenterLists> { };
            listbeforesearch.Clear();

            for (int i = 0; i < U9dt.Rows.Count; i++)
            {
                isexsist = false;
                for (int j = 0; j < BCSdt.Rows.Count; j++)
                {
                    if (U9dt.Rows[i]["department_code"].ToString() == BCSdt.Rows[j]["WC_Department_Code"].ToString())
                    {
                        isexsist = true;
                        break;
                    }

                }
                if (!isexsist)
                {
                    WorkCenterLists item = new WorkCenterLists();
                    item.department_name = U9dt.Rows[i]["department_name"].ToString();
                    item.department_code = U9dt.Rows[i]["department_code"].ToString();
                    item.department_id = (Int64)U9dt.Rows[i]["department_id"];
                    wcl.Add(item);
                    listbeforesearch.Add(item);
                }   
                
            }
            listview1.ItemsSource = null;
            listview1.ItemsSource = wcl;
        }

        /// <summary>
        /// 获取U9部门清单
        /// </summary>
        /// <returns></returns>
        private DataTable GetU9DepartmentList()
        {
            WebService.ServiceSoapClient ws = new WebService.ServiceSoapClient();
            ds = ws.GetDepartmentlist_ForMES(User_Info.User_Org_Code[0], "", "");
            return ds.Tables[0];
        }

        /// <summary>
        /// 获取条码系统部门清单
        /// </summary>
        /// <returns></returns>
        private DataTable GetBCSDepartmentList()
        {
            MyDBController.GetConnection();
            string SQl = "select WC_Department_Code from [WorkCenter] ";
            MyDBController.GetDataSet(SQl, ds, "WC_Department_Code");
            BCSdt = ds.Tables["WC_Department_Code"];
            MyDBController.CloseConnection();
            return BCSdt;
        }

        /// <summary>
        /// 如果从 ListView 控件中添加或移除 ListViewItem，
        /// 您必须更新 ListViewItem 控件以便重新创建交替的 Background 颜色。
        /// 下面的示例演示如何更新 ListViewItem 控件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewColumn_Selected(object sender, RoutedEventArgs e)
        {
            ICollectionView dataView =
                CollectionViewSource.GetDefaultView(listview1.ItemsSource);
            dataView.Refresh();
        }


        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }


        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReverseSelect_Click(object sender, RoutedEventArgs e)
        {
            List<WorkCenterLists> wcl = new List<WorkCenterLists> { };
            foreach (WorkCenterLists item in listview1.Items)
            {
                item.IsSelected = !item.IsSelected;
                wcl.Add(item);
            }
            listview1.ItemsSource = null;
            listview1.ItemsSource = wcl;
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            List<WorkCenterLists> wcl = new List<WorkCenterLists> { };
            foreach (WorkCenterLists item in listview1.Items)
            {
                item.IsSelected = true;
                wcl.Add(item);
            }
            listview1.ItemsSource = null;
            listview1.ItemsSource = wcl;
        }

        /// <summary>
        /// 重选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Reselect_Click(object sender, RoutedEventArgs e)
        {
            List<WorkCenterLists> wcl = new List<WorkCenterLists> { };
            foreach (WorkCenterLists item in listview1.Items)
            {
                item.IsSelected = false;
                wcl.Add(item);
            }
            listview1.ItemsSource = null;
            listview1.ItemsSource = wcl;
        }


        /// <summary>
        /// 确定导入按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Imprt_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait; 
            string SQl = "";
            int count = 0;
            MyDBController.GetConnection();
            foreach (WorkCenterLists item in listview1.Items)
            {       
                if (item.IsSelected)
                {
                    item.isvalidated = item.isworkcenter = item.isordercontroled = "false";
                    item.lastoperateby = User_Info.User_Name;
                    item.lastoperatetime_DB = DateTime.Now;
                    SQl = string.Format(@"insert into [WorkCenter](WC_Department_Code,WC_Department_Name,WC_Department_ShortName,
                                    WC_Department_ID,WC_IsValidated,WC_IsOrderControled,
                                    WC_IsWorkCenter,WC_LastOperateTime,WC_LastOprateBy)
                                    values('{0}','{1}',{2},'{3}','{4}','{5}','{6}','{7}','{8}')", item.department_code,
                    item.department_name, "''",item.department_id, item.isvalidated,
                    item.isordercontroled, item.isworkcenter, item.lastoperatetime_DB, item.lastoperateby);
                    try
                    {
                        MyDBController.ExecuteNonQuery(SQl);
                        listbeforesearch.Remove(item);
                        count++;
                    }
                    catch (Exception ee)
                    {
                        throw ee;
                    }
                }
                else
                {
                }

            }
            if (count>0)
            {
                MessageBox.Show("共导入"+count+"个工作中心！","提示",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("请选择要导入的信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MyDBController.CloseConnection();
            this.Cursor = Cursors.Arrow;
            listview1.ItemsSource = null;
            listview1.ItemsSource = listbeforesearch;
        }


        /// <summary>
        /// 获取U9部门清单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Fetch_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            //progressbar1.Visibility = Visibility.Visible;
            BindingItemSource();
            if (listview1.Items.Count==0)
            {
                MessageBox.Show("U9中所有相关部门已经在\n\n条码系统工作中心列表中！","提示",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 搜索工作中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            listview1.ItemsSource = null;
            listview1.ItemsSource = listbeforesearch;
            if (txtb_Search.Text.Length >0)
	        {
                string key = txtb_Search.Text;
		        List<WorkCenterLists> wcl = new List<WorkCenterLists> { };
                IEnumerable<WorkCenterLists> IEwcl =
                    from item in listbeforesearch
                    where item.department_name.IndexOf(txtb_Search.Text) != -1
                    select item;

                foreach (WorkCenterLists item in IEwcl)
	            {                   
		            wcl.Add(item);       
	            }
                listview1.ItemsSource=null;
                listview1.ItemsSource=wcl;
	        }
            else
	        {

	        }
        }


        /// <summary>
        /// listview选中项改变的时候
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = listview1.SelectedIndex;
        }

    }
}
