﻿using System;
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
using System.Text.RegularExpressions;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace BarCodeSystem
{
    /// <summary>
    /// ProcessNameModify_Window.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessNameModify_Window : Window
    {

        
        /// <summary>
        /// 来自父窗体传值，修改工序信息的时候使用
        /// </summary>
        public ProcessNameLists pnl
        {
            get;
            set;
        }


        /// <summary>
        /// 来自父窗体传值，系统中已有的工序名单
        /// </summary>
        public DataTable existProcess
        {
            get;
            set;
        }

        //是否允许将当前信息存入数据库
        bool canImport = true;

        //检验结果错误的时候，文本框闪烁次数
        int twinklecount = 0;

        //闪烁用的计时器
        System.Windows.Forms.Timer t1 = new System.Windows.Forms.Timer();


        public ProcessNameModify_Window()
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
            //这段代码在正式环境中将被注释掉，测试用
            MyDBController.Server = User_Info.server[1];
            MyDBController.Database = User_Info.database[1];
            MyDBController.Pwd = User_Info.pwd[1];
            MyDBController.Uid = User_Info.uid[1];

            //去除关闭按钮
            //2.在装载事件中加入
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            t1.Interval = 100;
            t1.Tick += new EventHandler(t1_Tick);

            //如果pnl 不为空，说明目前为工序信息修改窗体
            //将pnl信息填入窗体中各个文本框之中
            if (pnl != null)
            {
                this.Title = "工序信息修改窗口";
                txtb_Code.Text = pnl.PN_Code;
                txtb_Name.Text = pnl.PN_Name;
                txtb_Code.IsReadOnly = true;
            }
            else
            {
                this.Title = "工序信息新增窗口";
            }
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
        /// 文本框双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_Name_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.SelectAll();
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (canImport&&txtb_Name.Text.Length>0)
            {
                string SQl = "";
                string mess = "";
                if (this.Title == "工序信息修改窗口")
                {
                    SQl = string.Format(@"UPDATE [ProcessName] SET [PN_Code]='{0}',[PN_Name]='{1}' WHERE 
                                 [PN_Code]='{0}'",txtb_Code.Text,txtb_Name.Text);
                    mess = "成功修改工序信息!";
                }
                else
                {
                    SQl =string.Format( "INSERT INTO [ProcessName]([PN_Code],[PN_NAME]) VALUES('{0}','{1}')"
                                ,txtb_Code.Text,txtb_Name.Text);
                    mess = "成功新增工序信息！";
                }

                MyDBController.GetConnection();
                try
                { 
                    MyDBController.ExecuteNonQuery(SQl);
                    if (MessageBox.Show(mess, "提示", MessageBoxButton.OK, MessageBoxImage.Information)==
                        MessageBoxResult.OK)
                    {
                        this.DialogResult = true;
                    } 
                }
                catch(Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
                
                MyDBController.CloseConnection();
            }
        }

        /// <summary>
        /// 编码文本框失去焦点的时候校验输入的编码是否正确
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_Code_LostFocus(object sender, RoutedEventArgs e)
        {
            canImport = true;
            if (this.Title == "工序信息新增窗口")
            {
                if (Regex.IsMatch(txtb_Code.Text, User_Info.pattern[0]) &&
                    CheckPNCode(txtb_Code.Text))
                {

                }
                else
                {
                    twinklecount = 0;
                    t1.Enabled = true;
                    canImport = false;
                }
            }
            else
            {
            }

        }

        /// <summary>
        /// 检查code 文本框中的文本是否已经存在，存在返回false，否则返回true
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool CheckPNCode(string key)
        {
            bool IsRightCode = true;
            int x = existProcess.Rows.Count;
            for (int i = 0; i < x; i++)
            {
                if (key == existProcess.Rows[i]["PN_Code"].ToString())
                {
                    IsRightCode = false;
                    break;
                }
            }
            return IsRightCode;
        }


        /// <summary>
        /// 利用Timer来实现文本框边框闪烁效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void t1_Tick(object sender, EventArgs e)
        {
            if (twinklecount < 7)
            {
                txtb_Code.BorderThickness = new Thickness(1);
                switch (twinklecount % 2)
                {
                    case 0:
                        txtb_Code.BorderBrush = Brushes.Salmon;
                        break;
                    case 1:
                        txtb_Code.BorderBrush = txtb_Name.BorderBrush;
                        break;

                    default:
                        break;
                }
                twinklecount++;
            }
            else
            {
                t1.Enabled = false;
            }

        }

        /// <summary>
        /// code文本框获得焦点，清除样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_Code_GotFocus(object sender, RoutedEventArgs e)
        {
            t1.Enabled = false;
            txtb_Code.BorderBrush = txtb_Name.BorderBrush;
        }
    }
}
