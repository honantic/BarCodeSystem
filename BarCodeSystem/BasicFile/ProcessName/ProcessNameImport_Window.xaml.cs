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
    /// ProcessNameImport_Window.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessNameImport_Window : Window
    {

        DataTable copiedData = new DataTable();
        bool canImport = true;

        /// <summary>
        /// 父窗体传值
        /// </summary>
        public DataTable exisitProcessName
        {
            get;
            set;
        }
        public ProcessNameImport_Window()
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
            HandleCopiedData(GetCopyTable());
        }


        /// <summary>
        /// 导入条码系统按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {

            if (canImport && copiedData.Rows.Count>0)
            {
                /* 本函数核心方法是，MyDBController.InsertSqlBulk，该方法的使用需要注意以下几点：
                 * 1.方法有两个参数，参数1为包含数据的datatable，参数2为datatable中update和insert涉及到的全部的列名的数组集合
                 * 2.datatable的tablename必须和数据库中目标表的表名一样，如果目标表中有主键，则需要在本datatable中
                 *     新建一列和主键对应的影射列，列名为"主键名"+"New"，影射列数据和主键数据一样，但是没有主键约束*/
                this.Cursor = Cursors.Wait;
                List<string> colList = new List<string> { "ID", "PN_Code", "PN_Name" };

                copiedData.Columns.Add("ID", typeof(Int64));//数据表主键，复制过来的表里面没有，需要加上一列
                copiedData.Columns["ID"].SetOrdinal(0);//ID为第一列
                copiedData.Columns.Add("IDNew", typeof(Int64));//主键影射列


                int x = copiedData.Rows.Count;
                int y = exisitProcessName.Rows.Count;

                //循环检查是否有质量档案已经存在，存在，则将ID值赋值给IDNew
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        if (copiedData.Rows[i]["PN_Code"].ToString()
                            == exisitProcessName.Rows[j]["PN_Code"].ToString())
                        {
                            copiedData.Rows[i]["IDNew"] = exisitProcessName.Rows[j]["ID"];
                            break;
                        }
                    }
                }
                MyDBController.GetConnection();
                int updateNum = 0, insertNum = 0;
                MyDBController.InsertSqlBulk(copiedData, colList, out updateNum, out insertNum);
                MyDBController.CloseConnection();
                string message = string.Format(@"共更新 {0} 条质量档案！" + "\n新增 {1} 条质量档案！", updateNum, insertNum);
                if (MessageBox.Show(message, "提示", MessageBoxButton.OK, MessageBoxImage.Information) ==
                    MessageBoxResult.OK)
                {
                    this.Cursor = Cursors.Arrow;
                    this.DialogResult = true;
                }
            }
            else
            {
                MessageBox.Show("导入的数据有误！请检查是否有记录为空值！","提示",MessageBoxButton.OK,MessageBoxImage.Error);
            }

        }


        /// <summary>
        /// 将excel 中数据复制到copiedData中
        /// </summary>
        private DataTable GetCopyTable()
        {
            string columlist = "PN_Code,PN_Name";
            copiedData = new DataTable("ProcessName");//用来显示的
            QkRowChangeToColClass qk = new QkRowChangeToColClass();

            //为显示表添加列
            copiedData.Columns.Add("PN_Code", typeof(string));
            copiedData.Columns.Add("PN_Name", typeof(string));

            qk.write_excel_date_to_temp_table(copiedData, columlist);
            return copiedData;
        }

        /// <summary>
        /// 对复制过来的数据进行处理，转换成listview数据源
        /// </summary>
        /// <param name="dt"></param>
        private void HandleCopiedData(DataTable dt)
        {
            List<ProcessNameLists> pnls = new List<ProcessNameLists> { };
            int x = dt.Rows.Count;
            canImport = true;

            for (int i = 0; i < x; i++)
            {
                ProcessNameLists pnl = new ProcessNameLists();
                pnl.PN_Code = dt.Rows[i]["PN_Code"].ToString();
                pnl.PN_Name = dt.Rows[i]["PN_Name"].ToString();
                pnls.Add(pnl);
                if (dt.Rows[i]["PN_Code"].ToString() == "" || dt.Rows[i]["PN_Name"].ToString()=="")
                {
                    canImport = false;
                }
            }
            listview1.ItemsSource = pnls;
        }

        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OutPrint_Click(object sender, RoutedEventArgs e)
        {
            if (copiedData.Rows.Count > 0)
            {
                QkRowChangeToColClass.CreateExcelFileForDataTable(copiedData);
                MessageBox.Show("共导出" + copiedData.Rows.Count + "条数据到Excel", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("没有数据！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
        /// 窗体keydown事件，添加快捷键委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            AddHandler(Keyboard.KeyDownEvent,(KeyEventHandler)HandleKeyDownEvent);
        }

        /// <summary>
        /// Ctr+V 事件委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.V&&(Keyboard.Modifiers & ModifierKeys.Control)==(ModifierKeys.Control))
            {
                RoutedEventArgs ee = new RoutedEventArgs();
                btn_Copy_Click(sender,ee);
            }
        }


    }
}
