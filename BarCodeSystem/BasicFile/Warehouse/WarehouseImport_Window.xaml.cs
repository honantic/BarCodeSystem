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
using System.Windows.Interop;
using System.Runtime.InteropServices;
using BarCodeSystem.PublicClass.HelperClass;

namespace BarCodeSystem
{
    /// <summary>
    /// WarehouseImport_Window.xaml 的交互逻辑
    /// </summary>
    public partial class WarehouseImport_Window : Window
    {


        public WarehouseImport_Window()
        {
            InitializeComponent();

        }

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        /// <summary>
        /// 条码系统的仓库信息，来自父窗体传值
        /// </summary>
        public DataTable BCSWarehouse = new DataTable();

        List<WarehouseLists> listBeforeSearch = new List<WarehouseLists> { };

        /// <summary>
        /// 窗体加载
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
        /// 获取U9仓库清单，排除掉条码系统中已经存在的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FetchU9List_Click(object sender, RoutedEventArgs e)
        {
            MyDBController.GetConnection();
            this.Cursor = Cursors.Wait;
            List<WarehouseLists> whls = new List<WarehouseLists> { };
            WebService.Service ws = new WebService.Service();
            ds = ws.GetWhlist_ForMES(User_Info.User_Org_Code[0], "", "");

            dt = ds.Tables[0];
            int x = dt.Rows.Count;
            int y = BCSWarehouse.Rows.Count;

            listBeforeSearch.Clear();
            for (int i = 0; i < x; i++)
            {
                bool IsExisit = false;
                for (int j = 0; j < y; j++)
                {
                    if (dt.Rows[i]["wh_code"].ToString() == BCSWarehouse.Rows[j]["W_Code"].ToString())
                    {
                        IsExisit = true;
                        break;
                    }
                }
                if (!IsExisit)
                {
                    WarehouseLists whl = new WarehouseLists();
                    whl.W_Code = dt.Rows[i]["wh_code"].ToString();
                    whl.W_Name = dt.Rows[i]["wh_name"].ToString();
                    whl.W_ID = (Int64)dt.Rows[i]["wh_id"];
                    whl.W_SourceType_Show = "U9系统导入";
                    listBeforeSearch.Add(whl);
                    whls.Add(whl);
                }
            }
            MyDBController.CloseConnection();
            listview1.ItemsSource = null;
            listview1.ItemsSource = whls;
            this.Cursor = Cursors.Arrow;
        }


        /// <summary>
        /// 全选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            List<WarehouseLists> whls = new List<WarehouseLists> { };

            foreach (WarehouseLists item in listview1.Items)
            {
                item.IsSelected = true;
                whls.Add(item);
            }
            listview1.Items.Refresh();
        }

        /// <summary>
        /// 重选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReSelectAll_Click(object sender, RoutedEventArgs e)
        {
            List<WarehouseLists> whls = new List<WarehouseLists> { };

            foreach (WarehouseLists item in listview1.Items)
            {
                item.IsSelected = false;
                whls.Add(item);
            }
            listview1.Items.Refresh();
        }

        /// <summary>
        /// 反选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReverseSelect_Click(object sender, RoutedEventArgs e)
        {
            List<WarehouseLists> whls = new List<WarehouseLists> { };

            foreach (WarehouseLists item in listview1.Items)
            {
                item.IsSelected = !item.IsSelected;
                whls.Add(item);
            }
            listview1.Items.Refresh();
        }

        /// <summary>
        /// 搜索按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            List<WarehouseLists> whls = new List<WarehouseLists> { };
            if (txtb_Search.Text != "")
            {
                string key = txtb_Search.Text;
                foreach (WarehouseLists item in listBeforeSearch)
                {
                    if (item.W_Code.IndexOf(key) != -1 || item.W_ID.ToString().IndexOf(key) != -1 ||
                        item.W_Name.IndexOf(key) != -1)
                    {
                        whls.Add(item);
                    }
                }
                listview1.ItemsSource = null;
                listview1.ItemsSource = whls;
            }
            else
            {
                listview1.ItemsSource = null;
                listview1.ItemsSource = listBeforeSearch;
            }
        }

        /// <summary>
        /// 搜索框文本改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            btn_Search_Click(sender, e);
        }


        /// <summary>
        /// 导入条码系统按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Improt_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            string SQl = "";
            int count = 0;
            MyDBController.GetConnection();
            List<DBLog> _dbLogList = new List<DBLog>();
            foreach (WarehouseLists item in listview1.Items)
            {
                if (item.IsSelected)
                {
                    item.W_IsValidated = false;
                    item.W_SourceType = 0;
                    SQl = string.Format(@"INSERT INTO [Warehouse]([W_ID],[W_Code],[W_Name],[W_SourceType],[W_IsValidated])
                                    VALUES({0},'{1}','{2}','{3}','{4}')"
                                    , item.W_ID, item.W_Code, item.W_Name, item.W_SourceType, item.W_IsValidated);
                    DBLog _dbLog = new DBLog();
                    _dbLog.DBL_OperateBy = User_Info.User_Code + "|" + User_Info.User_Name;
                    _dbLog.DBL_OperateTable = "Warehouse";
                    _dbLog.DBL_OperateType = OperateType.Insert;
                    _dbLog.DBL_Content = "新增设备:" + item.W_Code;
                    _dbLog.DBL_AssociateCode = item.W_Code;
                    _dbLogList.Add(_dbLog);
                    try
                    {
                        count += MyDBController.ExecuteNonQuery(SQl);
                        listBeforeSearch.Remove(item);
                    }
                    catch (Exception EE)
                    {

                        throw EE;
                    }
                }
                else
                {
                }
            }
            MyDBController.CloseConnection();
            if (count > 0)
            {
                MessageBox.Show("共导入" + count + "个工作中心！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                DBLog.WriteDBLog(_dbLogList);
            }
            else
            {
                MessageBox.Show("请选择要导入的信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Cursor = Cursors.Arrow;
            listview1.ItemsSource = null;
            listview1.ItemsSource = listBeforeSearch;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
