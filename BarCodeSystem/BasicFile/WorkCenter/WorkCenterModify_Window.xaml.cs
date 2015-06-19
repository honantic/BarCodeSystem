using System;
using System.Windows;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Input;

namespace BarCodeSystem
{
    /// <summary>
    /// WorkCenterModify_Window.xaml 的交互逻辑
    /// </summary>
    public partial class WorkCenterModify_Window : Window
    {

        //中作中心编码
        public WorkCenterLists dept_info
        {
            get;
            set;
        }

        DataTable dt = new DataTable();
        public WorkCenterModify_Window()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //这段代码在正式环境中将被注释掉，此处供测试用
            MyDBController.Server = User_Info.server[1];
            MyDBController.Database = User_Info.database[1];
            MyDBController.Pwd = User_Info.pwd[1];
            MyDBController.Uid = User_Info.uid[1];


            //去除关闭按钮
            //2.在装载事件中加入
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            GetBCSWorkCenterInfo();
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
        /// 从条码系统中取出指定的工作中心信息
        /// 放到修改窗体的初始化界面
        /// </summary>
        private void GetBCSWorkCenterInfo()
        {
            txtb_code.Text = dept_info.department_code;
            //txtb_name.Text = dept_info.department_name;
            txtb_modifyby.Text = dept_info.lastoperateby;
            txtb_modifytime.Text = dept_info.lastoperatetime;
            rbtn_isvalidated.IsChecked = dept_info.isvalidated_DB;
            rbtn_notvalidated.IsChecked = !dept_info.isvalidated_DB;
            rbtn_isordercontrol.IsChecked = dept_info.isordercontroled_DB;
            rbtn_notordercontrol.IsChecked = !dept_info.isordercontroled_DB;
            rbtn_isworkcenter.IsChecked = dept_info.isworkcenter_DB;
            rbtn_notworkcenter.IsChecked = !dept_info.isworkcenter_DB;

            txt_name.Text = dept_info.department_name;
            txt_department_shortname.Text = dept_info.department_shortname;
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Quit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            //MyDBController.GetConnection();
            //this.Cursor = Cursors.Wait;



            if (string.IsNullOrEmpty(txt_department_shortname.Text) || string.IsNullOrEmpty(txt_name.Text))
            {
                MessageBox.Show("保存信息不能为空!", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            }
            else
            {
                dept_info.isvalidated_DB = (bool)rbtn_isvalidated.IsChecked ? true : false;
                dept_info.isordercontroled_DB = (bool)rbtn_isordercontrol.IsChecked ? true : false;
                dept_info.isworkcenter_DB = (bool)rbtn_isworkcenter.IsChecked ? true : false;

                dept_info.department_shortname = txt_department_shortname.Text.Trim();
                dept_info.department_name = txt_name.Text.Trim();
                MyDBController.GetConnection();
                this.Cursor = Cursors.Wait;

                string SQl = string.Format(@"UPDATE [WorkCenter]  SET [WC_IsValidated]='{0}',
                                [WC_IsOrderControled]='{1}',[WC_IsWorkCenter]='{2}',[WC_LastOperateTime]='{3}',
                                [WC_LastOprateBy]='{4}',[WC_Department_ShortName] = '{5}',[WC_Department_Name] = '{6}' WHERE [WC_Department_ID]={7}", dept_info.isvalidated_DB,
                    dept_info.isordercontroled_DB, dept_info.isworkcenter_DB,
                    DateTime.Now, User_Info.User_Name, dept_info.department_shortname, dept_info.department_name, dept_info.department_id);
                try
                {
                    MyDBController.ExecuteNonQuery(SQl);
                    MessageBoxResult result = MessageBox.Show("保存成功", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    if (result == MessageBoxResult.OK)
                    {
                        this.DialogResult = true;
                    }


                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                MyDBController.CloseConnection();
                this.Cursor = Cursors.Arrow;
            }

        }


    }
}
