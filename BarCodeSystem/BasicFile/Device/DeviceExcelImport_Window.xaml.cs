using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Data;
using BarCodeSystem.PublicClass.HelperClass;

namespace BarCodeSystem
{
    /// <summary>
    /// DeviceExcelImport_Window.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceExcelImport_Window : Window
    {

        DataTable copiedData = new DataTable();
        List<DeviceLists> dls;
        bool canImport = true;

        /// <summary>
        /// 父窗体传值
        /// </summary>
        public DataTable exisitDevice
        {
            get;
            set;
        }

        public DeviceExcelImport_Window()
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
        /// 复制按钮
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
            string columlist = "DD_Code,DD_Name,DD_Amount,DD_BarCode,DD_Version,DD_WorkCenterCode";
            copiedData = new DataTable();
            QkRowChangeToColClass qk = new QkRowChangeToColClass();

            copiedData.Columns.Add("DD_Code", typeof(string));
            copiedData.Columns.Add("DD_Name", typeof(string));
            copiedData.Columns.Add("DD_Amount", typeof(int));
            copiedData.Columns.Add("DD_BarCode", typeof(string));
            copiedData.Columns.Add("DD_Version", typeof(string));
            copiedData.Columns.Add("DD_WorkCenterCode", typeof(string));

            try
            {
                qk.write_excel_date_to_temp_table(copiedData, columlist);
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            copiedData.Columns.Add("DD_WorkCenterID", typeof(Int64));
            copiedData.Columns.Add("DD_SourceType", typeof(int));
            copiedData.Columns.Add("DD_IsValidated", typeof(bool));
            for (int i = 0; i < copiedData.Rows.Count; i++)
            {
                copiedData.Rows[i]["DD_SourceType"] = 1;
                copiedData.Rows[i]["DD_IsValidated"] = false;
                copiedData.Rows[i]["DD_BarCode"] = copiedData.Rows[i]["DD_Code"];
            }


        }

        /// <summary>
        /// 检查复制的各项数据的部门code 是否正确
        /// 正确的将DeviceLists的isRightDepart赋值为true
        ///不正确的将DeviceLists的isRightDepart赋值为false
        /// </summary>
        /// <param name="copied"></param>
        /// <returns></returns>
        private List<DeviceLists> CheckIfRightDepart(DataTable copied)
        {
            MyDBController.GetConnection();
            string SQl = @" SELECT  [WC_Department_ID],[WC_Department_Code],[WC_Department_Name] FROM [WorkCenter]";
            DataTable SYSDepartCode = MyDBController.GetDataSet(SQl).Tables[0];
            MyDBController.CloseConnection();
            dls = new List<DeviceLists> { };
            bool IfRight = false;
            canImport = true;
            string error1 = "工作中心编码/设备名称/设备编码 任意一列不能为空";
            string error2 = "工作中心编码有误";
            /*为复制进来的数据添加一列标识列
             *当导入数据存在错误的时候，导出错误数据到Excel
             *该标识列可以为操作人员提供参考*/
            copied.Columns.Add("IfRight", typeof(string));
            copiedData.Columns.Add("ID", typeof(Int64));
            copiedData.Columns["ID"].SetOrdinal(0);
            copiedData.Columns.Add("IDNew", typeof(Int64));
            copiedData.TableName = "DeviceDetail";

            for (int i = 0; i < copied.Rows.Count; i++)
            {
                IfRight = false;
                for (int j = 0; j < SYSDepartCode.Rows.Count; j++)
                {
                    if (copied.Rows[i]["DD_WorkCenterCode"] == null && copied.Rows[i]["DD_WorkCenterCode"].ToString() == "" &&
                        copied.Rows[i]["DD_Name"].ToString() == "" && copied.Rows[i]["DD_Name"] == null &&
                        copied.Rows[i]["DD_Code"].ToString() == "" && copied.Rows[i]["DD_Code"] == null)
                    {
                        copied.Rows[i]["IfRight"] = error1;
                        DeviceLists dl = new DeviceLists();
                        dl.isRightDepart = false;
                        dl.D_Name = copied.Rows[i]["DD_Name"].ToString();
                        dl.D_Code = copied.Rows[i]["DD_Code"].ToString();
                        dl.D_Amount = Convert.ToInt32(copied.Rows[i]["DD_Amount"]);
                        dl.D_BarCode = copied.Rows[i]["DD_BarCode"].ToString();
                        dl.D_Version = copied.Rows[i]["DD_Version"].ToString();
                        dl.D_Department_Code = copied.Rows[i]["DD_WorkCenterCode"].ToString();
                        dl.D_Department_Name = SYSDepartCode.Rows[j]["WC_Department_Name"].ToString();
                        dl.D_Department_ID = (Int64)SYSDepartCode.Rows[j]["WC_Department_ID"];
                        dl.D_IsValidated_Show = copied.Rows[i]["DD_IsValidated"].ToString();
                        dl.D_SourceType_Show = copied.Rows[i]["DD_SourceType"].ToString();
                        dls.Add(dl);
                        break;
                    }
                    else
                    {
                        if (copied.Rows[i]["DD_WorkCenterCode"].ToString() ==
                        SYSDepartCode.Rows[j]["WC_Department_Code"].ToString())
                        {
                            DeviceLists dl = new DeviceLists();
                            dl.isRightDepart = true;
                            dl.D_Name = copied.Rows[i]["DD_Name"].ToString();
                            dl.D_Code = copied.Rows[i]["DD_Code"].ToString();
                            dl.D_Amount = Convert.ToInt32(copied.Rows[i]["DD_Amount"]);
                            dl.D_BarCode = copied.Rows[i]["DD_BarCode"].ToString();
                            dl.D_Version = copied.Rows[i]["DD_Version"].ToString();
                            dl.D_Department_Code = copied.Rows[i]["DD_WorkCenterCode"].ToString();
                            dl.D_Department_Name = SYSDepartCode.Rows[j]["WC_Department_Name"].ToString();
                            dl.D_Department_ID = (Int64)SYSDepartCode.Rows[j]["WC_Department_ID"];
                            copied.Rows[i]["DD_WorkCenterID"] = (Int64)SYSDepartCode.Rows[j]["WC_Department_ID"];
                            dl.D_IsValidated_Show = "否";
                            dl.D_SourceType_Show = "手工录入";
                            dl.D_IsValidated = false;
                            dl.D_SourceType = 1;
                            dls.Add(dl);
                            IfRight = true;
                            break;
                        }
                    }
                }

                if (!IfRight)
                {
                    DeviceLists dl = new DeviceLists();
                    dl.isRightDepart = false;
                    dl.D_Name = copied.Rows[i]["DD_Name"].ToString();
                    dl.D_Code = copied.Rows[i]["DD_Code"].ToString();
                    dl.D_Amount = Convert.ToInt32(copied.Rows[i]["DD_Amount"]);
                    dl.D_BarCode = copied.Rows[i]["DD_BarCode"].ToString();
                    dl.D_Version = copied.Rows[i]["DD_Version"].ToString();
                    dl.D_Department_Code = copied.Rows[i]["DD_WorkCenterCode"].ToString();
                    dl.D_IsValidated_Show = copied.Rows[i]["DD_IsValidated"].ToString();
                    dl.D_SourceType_Show = copied.Rows[i]["DD_SourceType"].ToString();
                    dls.Add(dl);
                    copied.Rows[i]["IfRight"] = error2;
                    canImport = false;
                }
            }

            copiedData.Columns.Remove("IfRight");
            copiedData.Columns.Remove("DD_WorkCenterCode");

            return dls;
        }

        /// <summary>
        /// 数据导入到条码系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            if (canImport && copiedData.Rows.Count > 0)
            {

                CheckIfDeviceExisit(copiedData, exisitDevice);
            }

            else
            {
                MessageBox.Show("数据存在错误，或者没有导入数据！\n请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// 导入条码系统前检查系统中人员信息，已经存在采取
        /// update方式放入数据库，不存在的采取insert方式
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        private void CheckIfDeviceExisit(DataTable copiedtable, DataTable existtable)
        {
            this.Cursor = Cursors.Wait;
            List<string> cloList = new List<string> { "ID", "DD_Code", "DD_Name", "DD_Amount", "DD_BarCode", "DD_Version", "DD_WorkCenterID", "DD_SourceType", "DD_IsValidated" };

            int x = copiedtable.Rows.Count;
            int y = existtable.Rows.Count;

            List<DBLog> _dbLogList = new List<DBLog>();
            for (int i = 0; i < x; i++)
            {
                DBLog _dbLog = new DBLog();
                _dbLog.DBL_OperateBy = User_Info.User_Code + "|" + User_Info.User_Name;
                _dbLog.DBL_OperateTable = "DeviceDetail";
                _dbLog.DBL_OperateTime = DateTime.Now.ToString();
                for (int j = 0; j < y; j++)
                {
                    if (copiedtable.Rows[i]["DD_Code"].ToString() ==
                        existtable.Rows[j]["DD_Code"].ToString())
                    {
                        copiedtable.Rows[i]["IDNew"] = existtable.Rows[j]["ID"];
                        _dbLog.DBL_OperateType = OperateType.Update;
                        _dbLog.DBL_AssociateID = existtable.Rows[j]["ID"].ToString();
                        _dbLog.DBL_AssociateCode = existtable.Rows[j]["DD_Code"].ToString();
                        _dbLog.DBL_Content = "更新设备信息，设备编码为：" + existtable.Rows[j]["DD_Code"].ToString();
                        break;
                    }
                }
                if (string.IsNullOrEmpty(_dbLog.DBL_Content))
                {
                    _dbLog.DBL_OperateType = OperateType.Insert;
                    _dbLog.DBL_AssociateCode = copiedtable.Rows[i]["DD_Code"].ToString();
                    _dbLog.DBL_Content = "新增设备信息，设备编码为：" + copiedtable.Rows[i]["DD_Code"].ToString().ToString();
                }
                _dbLogList.Add(_dbLog);
            }

            MyDBController.GetConnection();
            int updateNum, insertNum;
            MyDBController.InsertSqlBulk(copiedtable, cloList, out updateNum, out insertNum);
            MessageBox.Show("\n 共新增:" + insertNum + "条记录,更新:" + updateNum + "条记录！");
            MyDBController.CloseConnection();

            DBLog.WriteDBLog(_dbLogList);

            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OutPrint_Click(object sender, RoutedEventArgs e)
        {
            if (copiedData.Rows.Count > 0)
            {
                bool flag = QkRowChangeToColClass.CreateExcelFileForDataTable(copiedData);
                if (flag)
                {
                    MessageBox.Show("共导出" + copiedData.Rows.Count + "条数据到Excel", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("没有数据！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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


        /// <summary>
        /// listview1 的Ctrl+V事件
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
        /// window 的 key_down事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            AddHandler(Keyboard.KeyDownEvent, (KeyEventHandler)HandlekeyDownEvent);
        }

    }
}
