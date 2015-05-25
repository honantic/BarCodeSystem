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

namespace BarCodeSystem
{
    /// <summary>
    /// QualityIssuesModify_Window.xaml 的交互逻辑
    /// </summary>
    public partial class QualityIssuesModify_Window : Window
    {
        /// <summary>
        /// 质量信息档案,接收父窗体传值
        /// </summary>
        public QualityIssuesLists qil
        {
            get;
            set;
        }

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        System.Windows.Forms.Timer t1 = new System.Windows.Forms.Timer();

        bool IsExist = false;//闪烁标识
        int TwinkleCount = 0;//闪烁次数

        public QualityIssuesModify_Window()
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
            //这段代码在正式环境中将被注释掉，此处供测试用
            MyDBController.Server = User_Info.server[1];
            MyDBController.Database = User_Info.database[1];
            MyDBController.Pwd = User_Info.pwd[1];
            MyDBController.Uid = User_Info.uid[1];

            t1.Interval = 100;
            t1.Tick += new EventHandler(t1_Tick);
            //去除关闭按钮
            //2.在装载事件中加入
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            InitShow();
            
        }


        //去除关闭按钮
        //1.Window 类中申明
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


        private void InitShow()
        {
            if (qil!=null)//修改窗体，三个文本框只读
            {
                txtb_BarCode.Text = qil.QI_BarCode;
                txtb_Code.Text = qil.QI_Code;
                txtb_Name.Text = qil.QI_Name;
                txtb_Name.IsReadOnly = txtb_Code.IsReadOnly = txtb_BarCode.IsReadOnly = true;

                rbtn_IsItemIssueYes.IsChecked = qil.QI_IsItemIssue;
                rbtn_IsItemIssueNo.IsChecked = !qil.QI_IsItemIssue;

                rbtn_IsPreviousIssueYes.IsChecked = qil.QI_IsPreviousIssue;
                rbtn_IsPreviousIssueNo.IsChecked = !qil.QI_IsPreviousIssue;

                rbtn_IsProduceIssueYes.IsChecked = qil.QI_IsProduceIssue;
                rbtn_IsProduceIssueNo.IsChecked = !qil.QI_IsProduceIssue;
            }

            MyDBController.GetConnection();
            string SQl = "SELECT [ID],[QI_Code] FROM [QualityIssue]";
            dt = MyDBController.GetDataSet(SQl, ds, "QualityIssue").Tables["QualityIssue"];
            MyDBController.CloseConnection();
        }

        /// <summary>
        /// 文本框双击选择全部文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_Code_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.SelectAll();
        }


        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            MyDBController.GetConnection();
            CheckCode();
            string SQl = "";
            if (this.Title == "质量档案新增窗口")
            {
                if (!IsExist)
                {
                    SQl = string.Format(@"INSERT INTO [QualityIssue]([QI_Code],[QI_Name],[QI_BarCode],[QI_IsItemIssue],[QI_IsProduceIssue],[QI_IsPreviousIssue])
                                VALUES('{0}','{1}','{2}','{3}','{4}','{5}')"
                                , txtb_Code.Text, txtb_Name.Text, txtb_BarCode.Text, rbtn_IsItemIssueYes.IsChecked, rbtn_IsProduceIssueYes.IsChecked, rbtn_IsPreviousIssueYes.IsChecked);
                    int x = MyDBController.ExecuteNonQuery(SQl);
                    if (x>0)
                    {
                        if (MessageBox.Show("信息新增成功！","提示",MessageBoxButton.OK,MessageBoxImage.Information)==
                            MessageBoxResult.OK)
                        {
                            this.DialogResult = true;
                        }  
                    }
                }
                else
                {
                    MessageBox.Show("该档案编码已经存在，请更改！","提示",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
            else 
            {
                SQl = string.Format(@"UPDATE [QualityIssue] SET [QI_IsItemIssue]='{0}',[QI_IsProduceIssue]='{1}',[QI_IsPreviousIssue]='{2}'
                            WHERE [ID]={3}",rbtn_IsItemIssueYes.IsChecked,rbtn_IsProduceIssueYes.IsChecked,rbtn_IsPreviousIssueYes.IsChecked,qil.ID);
                int x = MyDBController.ExecuteNonQuery(SQl);
                if (x > 0)
                {
                    if (MessageBox.Show("信息修改成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information) ==
                        MessageBoxResult.OK)
                    {
                        this.DialogResult = true;
                    }
                }
            }
            MyDBController.CloseConnection();
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

        /// <summary>
        /// 重新填写
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReWrite_Click(object sender, RoutedEventArgs e)
        {
            if (this.Title=="质量档案新增窗口")//新增的时候全部文本框均可编辑
            {
                txtb_BarCode.Text = txtb_Code.Text 
                    = txtb_Name.Text = "";
                rbtn_IsItemIssueNo.IsChecked = rbtn_IsItemIssueYes.IsChecked = rbtn_IsPreviousIssueNo.IsChecked =
                    rbtn_IsPreviousIssueYes.IsChecked = rbtn_IsProduceIssueNo.IsChecked = rbtn_IsProduceIssueYes.IsChecked
                    = false;
            }
            else//修改的时候，编码、名称、条码文本框不可编辑
            {
                rbtn_IsItemIssueNo.IsChecked = rbtn_IsItemIssueYes.IsChecked = rbtn_IsPreviousIssueNo.IsChecked =
                    rbtn_IsPreviousIssueYes.IsChecked = rbtn_IsProduceIssueNo.IsChecked = rbtn_IsProduceIssueYes.IsChecked
                    = false;
            }
        }

        /// <summary>
        /// 新增的时候，编码文本框失去焦点事件，自动检查新增的质量档案编码是否存在，存在则闪烁报错
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_Code_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Title=="质量档案新增窗口")
            {
                bool checkresult = CheckCode();
                if (checkresult)
                {
                    t1.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 检查编码文本框的逻辑
        /// </summary>
        /// <returns></returns>
        private bool CheckCode()
        {         
            int x = dt.Rows.Count;
            IsExist= false;
            TwinkleCount = 0;
            if (txtb_Code.Text.Length>0)
            {
                for (int i = 0; i < x; i++)
                {
                    if (dt.Rows[i]["QI_Code"].ToString() == txtb_Code.Text)
                    {
                        IsExist = true;
                        break;
                    }
                }
                return IsExist;
            }
            else
            {
                IsExist = true;
                return IsExist;
            }    
        }

        /// <summary>
        /// timer t1闪烁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void t1_Tick(object sender, EventArgs e)
        {
            int maxcount = 7;//最大闪烁次数
            if (TwinkleCount < maxcount)
            {
                if (TwinkleCount % 2 == 0)
                {
                    txtb_Code.BorderBrush = new SolidColorBrush(Colors.Salmon);
                    txtb_Code.BorderThickness = new Thickness(1);
                }
                else
                {
                    txtb_Code.BorderBrush = txtb_BarCode.BorderBrush;
                }
                TwinkleCount++;
            }
            else
            {
                t1.Enabled = false;
            }
        }

        /// <summary>
        /// 获得焦点，即时清除效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_Code_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            t1.Enabled = false;
            tb.BorderBrush = txtb_Name.BorderBrush;
        }
    }
}
