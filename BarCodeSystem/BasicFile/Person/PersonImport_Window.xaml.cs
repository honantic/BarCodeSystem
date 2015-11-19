using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Data;
using BarCodeSystem.PublicClass.HelperClass;

namespace BarCodeSystem
{
    /// <summary>
    /// PersonImport_Window.xaml 的交互逻辑
    /// </summary>
    public partial class PersonImport_Window : Window
    {
        /// <summary>
        /// 复制进来的数据，转换成指定格式的datatable
        /// </summary>
        DataTable copiedData, fixedData;
        List<PersonLists> pls;
        bool canImport = true;
        /// <summary>
        /// 父窗体传值
        /// </summary>
        public DataTable exisitPerson
        {
            get;
            set;
        }

        public PersonImport_Window()
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
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 导入条码系统按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            if (copiedData != null)
            {
                if (canImport && copiedData.Rows.Count > 0)
                {
                    this.Cursor = System.Windows.Input.Cursors.Wait;
                    CheckIfPersonExisit(pls, exisitPerson);
                    this.Cursor = System.Windows.Input.Cursors.Arrow;
                }

                else
                {
                    System.Windows.MessageBox.Show("数据存在错误，或者没有导入数据！\n请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// 将excel中数据复制到当前listview中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Copy_Click(object sender, RoutedEventArgs e)
        {
            GetCopyTable();
            listview1.ItemsSource = CheckIfRightDepart(copiedData);
        }

        /// <summary>
        /// 将excel 中数据复制到copiedData中
        /// </summary>
        private void GetCopyTable()
        {
            string columlist = "P_Code,P_Name,P_Position,WC_Department_Code,WC_Department_Name";
            copiedData = new DataTable("Person");//用来显示的
            fixedData = new DataTable("Person");
            QkRowChangeToColClass qk = new QkRowChangeToColClass();

            copiedData.Columns.Add("P_Code", typeof(string));
            copiedData.Columns.Add("P_Name", typeof(string));
            copiedData.Columns.Add("P_Position", typeof(string));
            copiedData.Columns.Add("WC_Department_Code", typeof(string));
            copiedData.Columns.Add("WC_Department_Name", typeof(string));
            qk.write_excel_date_to_temp_table(copiedData, columlist);
        }

        /// <summary>
        /// 导入条码系统前检查系统中人员信息，已经存在采取
        /// update方式放入数据库，不存在的采取insert方式
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        private void CheckIfPersonExisit(List<PersonLists> copied, DataTable exsistperson)
        {
            fixedData = copiedData.Clone();

            DataRow[] dr = copiedData.Select();
            for (int i = 0; i < dr.Length; i++)
            {
                fixedData.ImportRow(dr[i]);
            }

            fixedData.Columns.Remove("WC_Department_Code");
            fixedData.Columns.Remove("WC_Department_Name");
            fixedData.Columns.Remove("IfRight");


            int x = exisitPerson.Rows.Count;
            int y = fixedData.Rows.Count;
            fixedData.Columns.Add("ID", typeof(Int64));//人员表主键，MyDBController.InsertSqlBulk方法需要
            fixedData.Columns["ID"].SetOrdinal(0);//设置列顺序
            fixedData.Columns.Add("IDNew", typeof(Int64));//人员表主键影射列，MyDBController.InsertSqlBulk方法需要

            DBLog _dbLog = new DBLog();
            _dbLog.DBL_OperateBy = User_Info.User_Code;
            _dbLog.DBL_OperateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            _dbLog.DBL_OperateType = OperateType.Import;
            _dbLog.DBL_Content = User_Info.User_Name + "|Excel导入人员信息" + "|" + User_Info.User_WorkcenterName + "|" + User_Info.P_Position;
            for (int i = 0; i < y; i++)
            {
                bool IsExist = false;
                for (int j = 0; j < x; j++)
                {
                    if (exisitPerson.Rows[j]["P_Code"].ToString() ==
                        fixedData.Rows[i]["P_Code"].ToString())
                    {
                        fixedData.Rows[i]["ID"] = exisitPerson.Rows[j]["ID"];
                        fixedData.Rows[i]["IDNew"] = exisitPerson.Rows[j]["ID"];
                        IsExist = true;
                        break;
                    }
                }
                _dbLog.DBL_AssociateCode += fixedData.Rows[i]["P_Code"].ToString() + fixedData.Rows[i]["P_Name"].ToString() + "|";
                if (!IsExist)
                {
                    fixedData.Rows[i]["IDNew"] = -1;
                }
            }
            List<string> colList = new List<string> { };
            colList.Add("ID");
            colList.Add("P_Code");
            colList.Add("P_Name");

            colList.Add("P_Position");

            colList.Add("P_WorkCenterID");
            MyDBController.GetConnection();
            int updateNum = 0, insertNum = 0;
            MyDBController.InsertSqlBulk(fixedData, colList, out updateNum, out insertNum);
            MyDBController.CloseConnection();

            DBLog.WriteDBLog(_dbLog);

            string message = string.Format(@"共更新 {0} 个人员信息！\n 新增 {1} 个人员信息！", updateNum, insertNum);
            if (MessageBox.Show(message, "提示", MessageBoxButton.OK, MessageBoxImage.Information) ==
                MessageBoxResult.OK)
            {
                this.DialogResult = true;
            }
        }


        /// <summary>
        /// 检查复制的各项数据的部门code 是否正确
        /// 正确的将personlists的isRightDepart赋值为true
        ///不正确的将personlists的isRightDepart赋值为false
        /// </summary>
        /// <param name="copied"></param>
        /// <returns></returns>
        private List<PersonLists> CheckIfRightDepart(DataTable copied)
        {
            MyDBController.GetConnection();
            string SQl = @" SELECT  [WC_Department_ID],[WC_Department_Code] FROM [WorkCenter]";
            DataTable SYSDepartCode = MyDBController.GetDataSet(SQl).Tables[0];
            MyDBController.CloseConnection();
            pls = new List<PersonLists> { };
            bool IfRight = false;
            canImport = true;
            string error1 = "部门编码/人员姓名/人员编码 任意一列不能为空";
            string error2 = "部门编码有误";
            string error3 = "岗位信息有误";
            /*为复制进来的数据添加一列标识列
             *当导入数据存在错误的时候，导出错误数据到Excel
             *该标识列可以为操作人员提供参考*/
            copied.Columns.Add("IfRight", typeof(string));
            copied.Columns.Add("P_WorkCenterID", typeof(Int64));
            for (int i = 0; i < copied.Rows.Count; i++)
            {
                IfRight = false;
                for (int j = 0; j < SYSDepartCode.Rows.Count; j++)
                {
                    if (copied.Rows[i]["WC_Department_Code"] == null && copied.Rows[i]["WC_Department_Code"].ToString() == "" &&
                        copied.Rows[i]["P_Name"].ToString() == "" && copied.Rows[i]["P_Name"] == null &&
                        copied.Rows[i]["P_Code"].ToString() == "" && copied.Rows[i]["P_Code"] == null)
                    {
                        copied.Rows[i]["IfRight"] = error1;
                        break;
                    }
                    else
                    {
                        if (copied.Rows[i]["WC_Department_Code"].ToString() ==
                        SYSDepartCode.Rows[j]["WC_Department_Code"].ToString())
                        {

                            if (copied.Rows[i]["P_Position"].ToString().Equals("操作工") || copied.Rows[i]["P_Position"].ToString().Equals("检验员") || string.IsNullOrEmpty(copied.Rows[i]["P_Position"].ToString()))
                            {
                                PersonLists pl = new PersonLists();
                                pl.isRightDepart = true;
                                pl.name = copied.Rows[i]["P_Name"].ToString();
                                pl.code = copied.Rows[i]["P_Code"].ToString();

                                pl.position = copied.Rows[i]["P_Position"].ToString();

                                pl.departCode = copied.Rows[i]["WC_Department_Code"].ToString();
                                pl.departName = copied.Rows[i]["WC_Department_Name"].ToString();
                                pl.departid = (Int64)SYSDepartCode.Rows[j]["WC_Department_ID"];
                                copiedData.Rows[i]["P_WorkCenterID"] = (Int64)SYSDepartCode.Rows[j]["WC_Department_ID"];
                                pls.Add(pl);
                                IfRight = true;
                                break;
                            }
                            else
                            {
                                copied.Rows[i]["IfRight"] = error3;
                                IfRight = false;
                            }

                        }
                    }

                }

                if (!IfRight)
                {
                    PersonLists pl = new PersonLists();
                    pl.isRightDepart = false;
                    pl.name = copied.Rows[i]["P_Name"].ToString();
                    pl.code = copied.Rows[i]["P_Code"].ToString();

                    pl.position = copied.Rows[i]["P_Position"].ToString();

                    pl.departCode = copied.Rows[i]["WC_Department_Code"].ToString();
                    pl.departName = copied.Rows[i]["WC_Department_Name"].ToString();
                    pls.Add(pl);

                    if (!copied.Rows[i]["IfRight"].ToString().Equals(error3))
                        copied.Rows[i]["IfRight"] = error2;
                    canImport = false;
                }
            }
            return pls;
        }

        /// <summary>
        /// 导出数据按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OutPrint_Click(object sender, RoutedEventArgs e)
        {
            if (copiedData.Rows.Count > 0)
            {
                QkRowChangeToColClass.CreateExcelFileForDataTable(copiedData);
                System.Windows.MessageBox.Show("共导出" + copiedData.Rows.Count + "条数据到Excel", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("没有数据！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// listview1 的Ctr+V事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandlekeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && (Keyboard.Modifiers & (ModifierKeys.Control)) == (ModifierKeys.Control))
            {
                RoutedEventArgs ee = new RoutedEventArgs();
                btn_Copy_Click(sender, ee);
            }
        }

        /// <summary>
        /// window de key_down事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            AddHandler(Keyboard.KeyDownEvent, (KeyEventHandler)HandlekeyDownEvent);
        }


    }
}
