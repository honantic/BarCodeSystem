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

namespace BarCodeSystem
{
    /// <summary>
    /// ProcessNameList_Window.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessNameList_Window : Window
    {
        public ProcessNameList_Window()
        {
            InitializeComponent();
        }


        DataTable dt = new DataTable();
        DataSet ds = new DataSet();

        /// <summary>
        /// 选中的工序编码
        /// </summary>
        public string PN_Code
        {
            get;
            set;
        }

        /// <summary>
        /// 选中的工序名称
        /// </summary>
        public string PN_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 选中的工序的号
        /// </summary>
        public string PN_CodeInWorkCenter
        {
            get;
            set;
        }

        /// <summary>
        /// 选中的工序ID
        /// </summary>
        public Int64 PN_ID
        {
            get;
            set;
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

            GetBCSProcessNameList();
        }

        /// <summary>
        /// 获得条码系统中的工序清单
        /// </summary>
        private void GetBCSProcessNameList()
        {
            MyDBController.GetConnection();
            string SQl = string.Format(@"SELECT [ID],[PN_Code],[PN_Name],[PN_CodeInWorkCenter],[PN_WorkCenterID] FROM [ProcessName] where [PN_WorkCenterID]={0}", User_Info.User_Workcenter_ID);
            dt = MyDBController.GetDataSet(SQl, ds, "ProcessName").Tables["ProcessName"];
            MyDBController.CloseConnection();

            List<ProcessNameLists> pnls = new List<ProcessNameLists> { };
            int x = dt.Rows.Count;
            for (int i = 0; i < x; i++)
            {
                ProcessNameLists pnl = new ProcessNameLists();
                pnl.ID = (Int64)dt.Rows[i]["ID"];
                pnl.PN_Name = dt.Rows[i]["PN_Name"].ToString();
                pnl.PN_Code = dt.Rows[i]["PN_Code"].ToString();
                pnl.PN_CodeInWorkCenter = dt.Rows[i]["PN_CodeInWorkCenter"].ToString();
                if (!(dt.Rows[i]["PN_WorkCenterID"] is DBNull))
                {
                    pnl.PN_WorkCenterID = Convert.ToInt64(dt.Rows[i]["PN_WorkCenterID"]);
                }
                pnls.Add(pnl);
            }

            listview1.ItemsSource = null;
            listview1.ItemsSource = pnls;
        }

        /// <summary>
        /// 选择按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Chose_Click(object sender, RoutedEventArgs e)
        {
            ProcessNameLists pnl = listview1.SelectedItem as ProcessNameLists;
            if (pnl != null)
            {
                PN_Name = pnl.PN_Name;
                PN_Code = pnl.PN_Code;
                PN_ID = pnl.ID;
                PN_CodeInWorkCenter = pnl.PN_CodeInWorkCenter;
                this.DialogResult = true;
            }
        }

        /// <summary>
        /// 双击列表，快捷选中记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Point formPoint = e.GetPosition(this);
            if (formPoint.Y > 60 && formPoint.Y < 90)//表头部分不做响应
            {

            }
            else
            {
                btn_Chose_Click(sender, e);
            }
        }


        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
