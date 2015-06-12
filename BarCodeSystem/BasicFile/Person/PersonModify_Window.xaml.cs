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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Collections.ObjectModel;

namespace BarCodeSystem
{
    /// <summary>
    /// Personmodify_Window.xaml 的交互逻辑
    /// </summary>
    public partial class PersonModify_Window : Window
    {
        /// <summary>
        /// 调用快捷获取部门清单界面的时候传值用
        /// </summary>
        DataTable workcenter = new DataTable();

        /// <summary>
        /// 默认数据集
        /// </summary>
        DataSet ds = new DataSet();

        /// <summary>
        /// 人员列表，接收父窗体传值，用来回填父窗体
        /// 列表中的人员信息用来检验修改后的人员编码在系统中是否存在
        /// 不存在则以insert方式更新，存在则以update方式更新
        /// </summary>
        public ObservableCollection<PersonLists> pls = new ObservableCollection<PersonLists> { };

        /// <summary>
        /// 人员变量，接收父窗体传值，表示选中的人员
        /// </summary>
        public PersonLists pl = new PersonLists();


        //部门编码文本框内容检验标识
        bool IsRightCode = false;

        //检验结果错误的时候，文本框闪烁次数
        int twinklecount = 0;

        //闪烁用的计时器
        System.Windows.Forms.Timer t1 = new System.Windows.Forms.Timer();

        public PersonModify_Window()
        {
            InitializeComponent();
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //这段代码在正式环境中将被注释掉，测试用
            //MyDBController.Server = User_Info.server[1];
            //MyDBController.Database = User_Info.database[1];
            //MyDBController.Pwd = User_Info.pwd[1];
            //MyDBController.Uid = User_Info.uid[1];


            //获得部门清单，传递给快速选择部门编码的子窗体
            GetBCSWorkCenterList();

            InitShow();
            //去除关闭按钮
            //2.在装载事件中加入
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);


            t1.Interval = 100;
            t1.Tick += new EventHandler(t1_Tick);
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
        /// 利用Timer来实现文本框边框闪烁效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void t1_Tick(object sender, EventArgs e)
        {
            if (twinklecount < 7)
            {
                txtb_WorkCenterCode.BorderThickness = new Thickness(2);
                switch (twinklecount % 2)
                {
                    case 0:
                        txtb_WorkCenterCode.BorderBrush = Brushes.Salmon;
                        break;
                    case 1:
                        txtb_WorkCenterCode.BorderBrush = txtb_WorkCenterName.BorderBrush;
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
        /// 保存按钮,检查人员编码是否存在,存在以update方式更新,不存在,以insert方式更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            //如果IsRightCode为false,调用部门编码文本框市区焦点事件,检查
            if (!IsRightCode)
            {
                txtb_WorkCenterCode_LostFocus(sender,e);
            }

            //1.IsRightCode本身为true 2.检查过后再校验IsRightCode为true.
            if (IsRightCode)
            {
                string SQl = "";
                bool IsExisit = false;
                foreach (PersonLists item in pls)
                {
                    if (txtb_PersonCode.Text == item.code)
                    {
                        IsExisit = true;
                        break;
                    }
                }
                if (IsExisit)
                {
                    MyDBController.GetConnection();
                    SQl = string.Format(@"UPDATE [PERSON] SET [P_NAME]='{0}',[P_WorkCenterID]={1} WHERE [P_Code]='{2}'"
                                        ,txtb_PersonName.Text,pl.departid,txtb_PersonCode.Text);
                    try
                    {
                        int count= MyDBController.ExecuteNonQuery(SQl);
                        MyDBController.CloseConnection();
                        if (MessageBox.Show("成功更新"+count+"条人员信息！","提示",MessageBoxButton.OK,MessageBoxImage.Information)
                            ==MessageBoxResult.OK)
                        {
                            this.DialogResult = true;
                        }

                    }
                    catch (Exception ee)
                    {
                        MyDBController.CloseConnection();
                        MessageBox.Show(ee.Message,"提示",MessageBoxButton.OK,MessageBoxImage.Error);
                    }
                }
                else
                {
                    MyDBController.GetConnection();
                    SQl = string.Format(@"INSERT INTO [PERSON]([P_Code],[P_Name],[P_WorkCenterID]) 
                                        VALUES('{0}','{1}',{2})",txtb_PersonCode.Text,txtb_PersonName.Text,pl.departid);
                    try
                    {
                        int count=MyDBController.ExecuteNonQuery(SQl);
                        MyDBController.CloseConnection();
                        if (MessageBox.Show("成功新增" + count + "条人员信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Information)
                            == MessageBoxResult.OK)
                        {
                            this.DialogResult = true;
                        }
                    }
                    catch (Exception ee)
                    {
                        MyDBController.CloseConnection();
                        MessageBox.Show(ee.Message,"提示",MessageBoxButton.OK,MessageBoxImage.Error);;
                    }
                }
            }
        }

        /// <summary>
        ///  重新填写
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReWrite_Click(object sender, RoutedEventArgs e)
        {
            txtb_WorkCenterName.Text = txtb_WorkCenterCode.Text = txtb_PersonName.Text = txtb_PersonCode.Text = "";
            txtb_WorkCenterCode.ToolTip = txtb_WorkCenterName.ToolTip = null;
            txtb_WorkCenterCode.BorderBrush = txtb_WorkCenterName.BorderBrush;
            txtb_WorkCenterCode.BorderThickness = new Thickness(1);
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// 获取部门编码，用来传值给快捷获取部门编码的窗体
        /// </summary>
        private void GetBCSWorkCenterList()
        {
            MyDBController.GetConnection();
            string SQl = "SELECT [WC_Department_ID],[WC_Department_Code],[WC_Department_Name] FROM [WorkCenter]";
            ds.Clear();
            workcenter = MyDBController.GetDataSet(SQl, ds).Tables[0];
            MyDBController.CloseConnection();
        }

        /// <summary>
        /// 快捷获取部门编码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_GetName_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WorkTeamDepartList_Window wtdl = new WorkTeamDepartList_Window();
            wtdl.ShowDialog();
            if ((bool)wtdl.DialogResult)
            {
                txtb_WorkCenterCode.Text = wtdl.choosedCode;
                txtb_WorkCenterName.Text = wtdl.choosedName;
                txtb_WorkCenterCode.IsReadOnly = txtb_WorkCenterName.IsReadOnly = true;

                ToolTip tl = new ToolTip();
                tl.Content = "咦？怎么不能修改了呢？";
                txtb_WorkCenterCode.ToolTip = txtb_WorkCenterName.ToolTip = tl;
            }
        }

        /// <summary>
        /// 初始化展示信息
        /// </summary>
        private void InitShow()
        {
            if (pl != null)
            {
                txtb_PersonCode.Text = pl.code;
                txtb_PersonName.Text = pl.name;
                txtb_WorkCenterCode.Text = pl.departCode;
                txtb_WorkCenterName.Text = pl.departName;
            }
        }


        /// <summary>
        /// 文本框双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_WorkCenterName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.SelectAll();
        }

        /// <summary>
        /// 部门编码文本框失去焦点的时候,检验文本框里面的编码是否在条码系统中存在
        /// 不存在则报错
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_WorkCenterCode_LostFocus(object sender, RoutedEventArgs e)
        {
            IsRightCode = false;
            twinklecount = 0;
            if (txtb_WorkCenterCode.Text != "")
            {
                string key = txtb_WorkCenterCode.Text;
                int x = workcenter.Rows.Count;
                for (int i = 0; i < x; i++)
                {
                    if (key == workcenter.Rows[i]["WC_Department_Code"].ToString())
                    {
                        IsRightCode = true;
                        pl.departid = (Int64)workcenter.Rows[i]["WC_Department_ID"];
                        break;
                    }
                }
            }
            else
            { 
            }

            if (!IsRightCode)
            {
                System.Windows.Controls.ToolTip tt = new System.Windows.Controls.ToolTip();
                tt.Content = "输入的编码有误！请检查";
                txtb_WorkCenterCode.ToolTip = tt;
                t1.Enabled = true;
            }
        }

        /// <summary>
        /// 部门编码文本框获得焦点，清除一切样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_WorkCenterCode_GotFocus(object sender, RoutedEventArgs e)
        {
            t1.Enabled = false;
            txtb_WorkCenterCode.BorderBrush = txtb_WorkCenterName.BorderBrush;
            txtb_WorkCenterCode.BorderThickness = new Thickness(1);
        }

    }
}
