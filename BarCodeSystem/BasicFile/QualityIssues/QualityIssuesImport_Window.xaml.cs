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
    /// QualityIssuesImport_Window.xaml 的交互逻辑
    /// </summary>
    public partial class QualityIssuesImport_Window : Window
    {
        DataTable copiedData;
        public DataTable existedQualityIssues//条码系统已经存在的质量档案，来自父窗体传值
        {
            get;
            set;
        }
        public QualityIssuesImport_Window()
        {
            InitializeComponent();
        }

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
        /// 将excel 中数据复制到copiedData中
        /// </summary>
        private DataTable GetCopyTable()
        {
            string columlist = "QI_Code,QI_Name,QI_BarCode,QI_IsItemIssue_Show,QI_IsProduceIssue_Show,QI_IsPreviousIssue_Show";
            copiedData = new DataTable("QualityIssue");//用来显示的
            QkRowChangeToColClass qk = new QkRowChangeToColClass();

            //为显示表添加列
            copiedData.Columns.Add("QI_Code", typeof(string));
            copiedData.Columns.Add("QI_Name", typeof(string));
            copiedData.Columns.Add("QI_BarCode", typeof(string));
            copiedData.Columns.Add("QI_IsItemIssue_Show", typeof(string));
            copiedData.Columns.Add("QI_IsProduceIssue_Show", typeof(string));
            copiedData.Columns.Add("QI_IsPreviousIssue_Show", typeof(string));
            qk.write_excel_date_to_temp_table(copiedData, columlist);
            return copiedData;
        }

        /// <summary>
        /// 对复制过来的数据进行处理，转换成listview数据源
        /// QI_IsItemIssue默认否。是否材料问题;QI_IsProduceIssue 默认是。是否加工问题;
        /// QI_IsPreviousIssue  默认否。是否上到工序问题
        /// </summary>
        /// <param name="dt"></param>
        private void HandleCopiedData(DataTable dt)
        {
            List<QualityIssuesLists> qils = new List<QualityIssuesLists> { };
            int x = dt.Rows.Count;
            dt.Columns.Add("QI_IsItemIssue", typeof(bool));
            dt.Columns.Add("QI_IsProduceIssue", typeof(bool));
            dt.Columns.Add("QI_IsPreviousIssue", typeof(bool));
            for (int i = 0; i < x; i++)
            {
                QualityIssuesLists qil = new QualityIssuesLists();
                qil.QI_Code = dt.Rows[i]["QI_Code"].ToString();
                qil.QI_Name = dt.Rows[i]["QI_Name"].ToString();
                qil.QI_BarCode = dt.Rows[i]["QI_BarCode"].ToString();
                qil.QI_IsItemIssue = false;
                qil.QI_IsItemIssue_Show = "否";
                qil.QI_IsProduceIssue = true;
                qil.QI_IsProduceIssue_Show = "是";
                qil.QI_IsPreviousIssue = false;
                qil.QI_IsPreviousIssue_Show = "否";
                qils.Add(qil);
                dt.Rows[i]["QI_IsItemIssue"] = false;
                dt.Rows[i]["QI_IsProduceIssue"] = true;
                dt.Rows[i]["QI_IsPreviousIssue"] = false;
            }
            listview1.ItemsSource = qils;
        }

        /// <summary>
        /// 复制的数据导出到excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OutPrint_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (copiedData.Rows.Count > 0)
            {
                QkRowChangeToColClass.CreateExcelFileForDataTable(copiedData);
                System.Windows.MessageBox.Show("共导出" + copiedData.Rows.Count + "条数据到Excel", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("没有数据！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Cursor = Cursors.Arrow;
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
        /// 将复制的数据导入条码系统，条码编号已经存在的执行update语句，不存在的
        /// 执行insert语句
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            /* 本函数核心方法是，MyDBController.InsertSqlBulk，该方法的使用需要注意以下几点：
             * 1.方法有两个参数，参数1为包含数据的datatable，参数2为datatable中update和insert涉及到的全部的列名的数组集合
             * 2.datatable的tablename必须和数据库中目标表的表名一样，如果目标表中有主键，则需要在本datatable中
             *     新建一列和主键对应的影射列，列名为"主键名"+"New"，影射列数据和主键数据一样，但是没有主键约束*/
            this.Cursor = Cursors.Wait;
            List<string> colList = new List<string> { "ID", "QI_Code", "QI_Name", "QI_BarCode", "QI_IsItemIssue", 
                "QI_IsProduceIssue", "QI_IsPreviousIssue" };
            
            copiedData.Columns.Add("ID", typeof(Int64));//数据表主键，复制过来的表里面没有，需要加上一列
            copiedData.Columns["ID"].SetOrdinal(0);//ID为第一列
            copiedData.Columns.Add("IDNew",typeof(Int64));//主键影射列

            copiedData.Columns.Remove("QI_IsItemIssue_Show");//去除数据表中的展示列
            copiedData.Columns.Remove("QI_IsProduceIssue_Show");
            copiedData.Columns.Remove("QI_IsPreviousIssue_Show");

            int x = copiedData.Rows.Count;
            int y = existedQualityIssues.Rows.Count;

            //循环检查是否有质量档案已经存在，存在，则将ID值赋值给IDNew
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (copiedData.Rows[i]["QI_Code"].ToString()
                        == existedQualityIssues.Rows[j]["QI_Code"].ToString())
                    {
                        copiedData.Rows[i]["IDNew"] = existedQualityIssues.Rows[j]["ID"];
                        break;
                    }
                }
            }
            MyDBController.GetConnection();
            int updateNum = 0, insertNum = 0;
            MyDBController.InsertSqlBulk(copiedData,colList,out updateNum,out insertNum);
            MyDBController.CloseConnection();
            string message = string.Format(@"共更新 {0} 条质量档案！\n新增 {1} 条质量档案！",updateNum,insertNum);
            MessageBox.Show(message,"提示",MessageBoxButton.OK,MessageBoxImage.Information);
            this.Cursor = Cursors.Arrow;
        }


        /// <summary>
        /// listview1 的Ctr+V事件委托
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
