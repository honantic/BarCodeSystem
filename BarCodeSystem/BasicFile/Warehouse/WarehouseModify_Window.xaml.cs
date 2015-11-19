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
using BarCodeSystem.PublicClass.HelperClass;
using System.Data;

namespace BarCodeSystem
{
    /// <summary>
    /// WarehouseModify_Window.xaml 的交互逻辑
    /// </summary>
    public partial class WarehouseModify_Window : Window
    {
        /// <summary>
        /// 当前选择工作中心ID,来自工作中心传值或数据库
        /// </summary>
        Int64 txtbwcid;

        /// <summary>
        /// 得到仓库所有信息
        /// </summary>
        List<WarehouseLists> whList;

        /// <summary>
        /// 当前仓库变量，来自父窗体传值
        /// </summary>
        public WarehouseLists whl
        {
            get;
            set;
        }

        public WarehouseModify_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
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
            //MyDBController.GetConnection();


            //去除关闭按钮
            //2.在装载事件中加入
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            SetInit();
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
        /// 初始化显示
        /// </summary>
        private void SetInit()
        {
            txtb_Code.Text = whl.W_Code;
            txtb_Name.Text = whl.W_Name;
            txtb_workcenter.Text = whl.WC_Department_Name;
            if (whl.W_SourceType == 1)
            {
                rbtn_FromBcs.IsChecked = true;
            }
            else
            {
                rbtn_FromU9.IsChecked = true;
            }

            if (whl.W_IsValidated)
            {
                rbtn_Yes.IsChecked = true;
            }
            else
            {
                rbtn_No.IsChecked = true;
            }

            if (whl.W_IsDefault)
            {
                rbtn_isDefaultYes.IsChecked = true;
            }
            else
            {
                rbtn_isDefaultNo.IsChecked = true;
            }
            whList = WarehouseLists.Fetch_WInfo();
            txtbwcid = whList.Find(p => p.W_Code.Equals(whl.W_Code)).W_WorkCenterID;
        }


        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            bool x = (bool)rbtn_Yes.IsChecked;
            bool x2 = (bool)rbtn_isDefaultYes.IsChecked;
            //int count = 0;
            string SQl = string.Format(@"UPDATE [Warehouse] SET [W_IsValidated]='{0}',[W_WorkCenterID] = '{1}',[W_IsDefault] = '{2}'
                                    WHERE [W_ID]={3}", x, txtbwcid, x2, whl.W_ID);
            DBLog _dbLog = new DBLog();
            _dbLog.DBL_OperateBy = User_Info.User_Code + "|" + User_Info.User_Name;
            _dbLog.DBL_OperateTable = "Warehouse";
            _dbLog.DBL_OperateTime = DateTime.Now.ToString();
            _dbLog.DBL_OperateType = OperateType.Update;
            _dbLog.DBL_AssociateID = whl.W_ID.ToString();
            _dbLog.DBL_Content = "启用仓库：" + whl.W_Code;
            try
            {
                MyDBController.GetConnection();
                MyDBController.ExecuteNonQuery(SQl);
                //count = MyDBController.ExecuteNonQuery(SQl);

                if (!string.IsNullOrEmpty(txtb_workcenter.Text.Trim()) && rbtn_isDefaultYes.IsChecked == true)
                {
                    if (whList.Exists(p => p.WC_Department_Name.Equals(txtb_workcenter.Text.Trim()) && p.W_IsDefault))
                    {
                        WarehouseLists _whl = new WarehouseLists();
                        _whl = whList.Find(p => p.WC_Department_Name.Equals(txtb_workcenter.Text.Trim()) && p.W_IsDefault);
                        SQl = string.Format(@"UPDATE [Warehouse] SET [W_IsDefault] = '{0}'
                                    WHERE [W_ID]={1}", false, _whl.W_ID);
                        MyDBController.ExecuteNonQuery(SQl);
                    }
                }
                MessageBox.Show("修改成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                DBLog.WriteDBLog(_dbLog);
                this.DialogResult = true;
                MyDBController.CloseConnection();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
      

        /// <summary>
        /// 文本框双击全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_Name_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox txtb = (TextBox)sender;
            txtb.SelectAll();
        }

        /// <summary>
        /// 退出窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// 关联工作中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtbk_workcenter_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WorkTeamDepartList_Window wtdl = new WorkTeamDepartList_Window();
            wtdl.ShowDialog();
            if ((bool)wtdl.DialogResult)
            {
                this.txtbwcid = wtdl.choosedID;
                txtb_workcenter.Text = wtdl.choosedName;
            }
        }
    }
}
