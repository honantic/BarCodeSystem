using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Data;
using System.Windows.Controls;

namespace BarCodeSystem
{
    /// <summary>
    /// WorkTeamModify_Window.xaml 的交互逻辑
    /// </summary>
    public partial class WorkTeamModify_Window : Window
    {

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        //工作中心变量，用来接收父窗体传值。
        public WorkTeamLists wt = new WorkTeamLists();

        //条码系统班组清单dt，接受自父窗体，用来检查保存的班组
        //编码是否存在，存在则执行update语句，不存在则执行insert 语句
        public DataTable workteam = new DataTable();

        //部门编码文本框内容检验标识
        bool IsRightCode = false;

        //检验结果错误的时候，文本框闪烁次数
        int twinklecount = 0;

        //闪烁用的计时器
        System.Windows.Forms.Timer t1 = new System.Windows.Forms.Timer();

        public WorkTeamModify_Window()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //这段代码在正式程序中将被注释掉，测试用
            MyDBController.Server = User_Info.server[1];
            MyDBController.Database = User_Info.database[1];
            MyDBController.Uid = User_Info.uid[1];
            MyDBController.Pwd = User_Info.pwd[1];

      
            //去除关闭按钮
            //2.在装载事件中加入
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            //获得部门清单，传递给快速选择部门编码的子窗体
            GetWorkCenterList();
            t1.Interval = 100;
            t1.Tick += new EventHandler(t1_Tick);

            //如果wt 不为空，说明目前为班组信息修改窗体
            //将wt信息填入窗体中各个文本框之中
            if (wt!=null)
            {
                txtb_DepartCode.Text = wt.workcenterCode;
                txtb_DepartName.Text = wt.workcenterName;
                txtb_TeamCode.Text = wt.WT_Code;
                txtb_TeamName.Text = wt.WT_Name;
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
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            MyDBController.GetConnection();
            if (txtb_TeamName.Text==""||txtb_TeamCode.Text==""||
                txtb_DepartCode.Text==""||txtb_DepartName.Text=="")
            {
                System.Windows.MessageBox.Show("各项信息均不能为空！\n请检查!","提示",MessageBoxButton.OK,MessageBoxImage.Error);
            }

            else
            {
                if (IsRightCode)
                {
                    if (dt.Rows.Count > 0)
                    {
                        bool Isexisit = false;
                        for (int i = 0; i < workteam.Rows.Count; i++)
                        {
                            #region 数据已存在，执行更新操作
                            if (workteam.Rows[i]["WC_Department_Code"].ToString() == txtb_TeamCode.Text)
                            {
                                Isexisit = true;
                                string SQl = string.Format(@"UPDATE [WorkTeam] SET [WT_Code]='{0}', [WT_Name]='{1}',
                                        [WT_WorkCenterID]={2}", txtb_TeamCode.Text, txtb_TeamName.Text,
                                                                  (Int64)dt.Rows[i]["WC_Department_ID"]);
                                try
                                {
                                    int t =
                                    MyDBController.ExecuteNonQuery(SQl);
                                    if (t > 0)
                                    {
                                        if (MessageBox.Show("班组信息更新成功！", "提示", MessageBoxButton.OK,
                                            MessageBoxImage.Information)
                                            == MessageBoxResult.OK)
                                        {
                                            this.DialogResult = true;
                                        }
                                    }
                                    break;
                                }
                                catch (Exception ee)
                                {
                                    if (MessageBox.Show("班组信息更新失败！\n" + ee.Message + "\n请重新尝试！", "提示",
                                        MessageBoxButton.OK, MessageBoxImage.Information)
                                        == MessageBoxResult.OK)
                                    {
                                        this.DialogResult = false;
                                        break;
                                    }
                                }
                            }
                            #endregion

                            #region 班组信息不存在，执行插入操作
                            if (!Isexisit)
                            {
                                DataRow[] dr = workteam.Select(string.Format("[WC_Department_Code]='{0}'", txtb_DepartCode.Text));
                                Int64 x =(Int64)dr[0]["WC_Department_ID"];

                                string SQl = string.Format(@"INSERT INTO [WorkTeam] ( [WT_Code], [WT_Name],
                                                [WT_WorkCenterID]) VALUES('{0}','{1}',{2})", 
                                                txtb_TeamCode.Text, txtb_TeamName.Text,x );
                                try
                                {
                                    int t =
                                    MyDBController.ExecuteNonQuery(SQl);
                                    if (t > 0)
                                    {
                                        if (MessageBox.Show("班组信息新增成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information)
                                            == MessageBoxResult.OK)
                                        {
                                            this.DialogResult = true;
                                        }
                                    }
                                    break;
                                }
                                catch (Exception ee)
                                {
                                    if (MessageBox.Show("班组信息新增失败！\n" + ee.Message + "\n请重新尝试！", "提示", MessageBoxButton.OK, MessageBoxImage.Information)
                                        == MessageBoxResult.OK)
                                    {
                                        this.DialogResult = false;
                                        break;
                                    }
                                }
                            }
                            #endregion
                        }
                    }

                    else
                    {
                        #region 当前条码系统没有班组信息，直接执行插入操作
                        DataRow[] dr = workteam.Select(string.Format("[WC_Department_Code]='{0}'", txtb_DepartCode.Text));
                        Int64 x = (Int64)dr[0]["WC_Department_ID"];

                        string SQl = string.Format(@"INSERT INTO [WorkTeam] ( [WT_Code], [WT_Name],
                                                [WT_WorkCenterID]) VALUES('{0}','{1}',{2})", txtb_TeamCode.Text, txtb_TeamName.Text, x);
                        try
                        {
                            int t =
                            MyDBController.ExecuteNonQuery(SQl);
                            if (t > 0)
                            {
                                if (MessageBox.Show("班组信息新增成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information)
                                    == MessageBoxResult.OK)
                                {
                                    this.DialogResult = true;
                                }
                            }
                        }
                        catch (Exception ee)
                        {
                            if (MessageBox.Show("班组信息新增失败！\n" + ee.Message + "\n请重新尝试！", "提示", MessageBoxButton.OK, MessageBoxImage.Information)
                                == MessageBoxResult.OK)
                            {
                                this.DialogResult = false;
                            }
                        }
                        #endregion
                    }

                }
                else
                {
                    System.Windows.MessageBox.Show("部门编码有误，不能保存！\n请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            MyDBController.CloseConnection();
        }


        /// <summary>
        /// 快捷获取部门清单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_GetName_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = System.Windows.Input.Cursors.Wait;
            WorkTeamDepartList_Window wtd = new WorkTeamDepartList_Window();
            wtd.ShowDialog();
            this.Cursor = System.Windows.Input.Cursors.Arrow;

            if ((bool)wtd.DialogResult)
            {
                //将获得的部门编码、名称赋值，并且不允许修改
                txtb_DepartCode.Text = wtd.choosedCode;
                txtb_DepartName.Text = wtd.choosedName;
                txtb_DepartCode.IsReadOnly = true;
                txtb_DepartName.IsReadOnly = true;

                //还原部门编码文本框的边框样式，修改部门编码、名称的提示信息
                txtb_DepartCode.BorderBrush = txtb_DepartName.BorderBrush;
                System.Windows.Controls.ToolTip tt = new System.Windows.Controls.ToolTip();
                tt.Content = "咦？怎么不能修改了呢？";
                txtb_DepartCode.ToolTip = txtb_DepartName.ToolTip = tt;

                //快捷获取的部门编码肯定是正确的
                IsRightCode = true;
            }
        }

        /// <summary>
        /// 重新填写，清除一切文字和样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReWrite_Click(object sender, RoutedEventArgs e)
        {
            txtb_DepartCode.Text= txtb_DepartName.Text=txtb_TeamCode.Text=txtb_TeamName.Text= "";
            txtb_DepartName.ToolTip = txtb_DepartCode.ToolTip = null;
            txtb_DepartCode.BorderBrush=txtb_DepartName.BorderBrush;
            txtb_DepartCode.BorderThickness = new Thickness(1);
            txtb_DepartCode.IsReadOnly =txtb_DepartName.IsReadOnly= false;
        }



        /// <summary>
        /// 部门编码框获得焦点，清除效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_DepartCode_GotFocus(object sender, RoutedEventArgs e)
        {
            t1.Enabled = false;
            txtb_DepartCode.BorderBrush = txtb_DepartName.BorderBrush;
        }


        /// <summary>
        /// 部门编码框失去焦点
        /// 部门编码一栏输入完毕的时候检查是否正确
        /// 编码错误的时候，调用Timer来实现提示效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_DepartCode_LostFocus(object sender, RoutedEventArgs e)
        {
            IsRightCode = false;
            twinklecount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["WC_Department_Code"].ToString()==txtb_DepartCode.Text)
                {
                    IsRightCode =true;
                    break;
                }
            }

            if (!IsRightCode)
            {
                t1.Enabled = true;
                System.Windows.Controls.ToolTip tt = new System.Windows.Controls.ToolTip();
                tt.Content = "输入的编码有误！请检查";
                txtb_DepartCode.ToolTip = tt;
            }
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
                txtb_DepartCode.BorderThickness = new Thickness(2);
                switch (twinklecount %2)
                {
                    case 0:
                        txtb_DepartCode.BorderBrush = Brushes.Salmon;
                        break;
                    case 1:
                        txtb_DepartCode.BorderBrush = txtb_DepartName.BorderBrush;
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
        /// 获取部门清单，传给子窗体
        /// </summary>
        /// <returns></returns>
        private DataTable GetWorkCenterList()
        {
            MyDBController.GetConnection();
            string SQl = "SELECT [WC_Department_Code],[WC_Department_Name] FROM [WorkCenter]";
            dt = MyDBController.GetDataSet(SQl, ds).Tables[0];
            MyDBController.GetConnection();
            return dt;
        }

        /// <summary>
        /// 文本框获得焦点事件，Tab获得焦点时默认选取全部文本内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_TeamName_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtb = (TextBox)sender;
            txtb.SelectAll() ;
        }

        /// <summary>
        /// 双击全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_TeamName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox txtb = (TextBox)sender;
            txtb.SelectAll();
        }




    }
}
