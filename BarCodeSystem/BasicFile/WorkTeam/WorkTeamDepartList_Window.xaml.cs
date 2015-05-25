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
    /// WorkTeamDepartList_Window.xaml 的交互逻辑
    /// </summary>
    public partial class WorkTeamDepartList_Window : Window
    {
        List<WorkCenterLists> wcl = new List<WorkCenterLists> { };
         DataTable dt = new DataTable();
         DataSet ds = new DataSet();
        //选中的部门编码，名称
        public string choosedCode,choosedName;
        public Int64 choosedID;
        public WorkTeamDepartList_Window()
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

            GetWorkCenterList();
            lv_listview1.ItemsSource = null;
            lv_listview1.ItemsSource = wcl;
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// 选择按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Chose_Click(object sender, RoutedEventArgs e)
        {
            WorkCenterLists item = lv_listview1.SelectedItem as WorkCenterLists;
            if (item != null)
            {
                choosedCode = item.department_code;
                choosedName = item.department_name;
                choosedID = item.department_id;
                this.DialogResult = true;
            }
        }

        /// <summary>
        /// 获取条码系统部门清单
        /// </summary>
        /// <returns></returns>
        private List<WorkCenterLists> GetWorkCenterList()
        {
            MyDBController.GetConnection();
            string SQl = "SELECT [WC_Department_ID], [WC_Department_Code],[WC_Department_Name] FROM [WorkCenter]";
            dt = MyDBController.GetDataSet(SQl, ds).Tables[0];
            MyDBController.GetConnection();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                WorkCenterLists wcls = new WorkCenterLists();
                wcls.department_code = dt.Rows[i]["WC_Department_Code"].ToString();
                wcls.department_name = dt.Rows[i]["WC_Department_Name"].ToString();
                wcls.department_id = (Int64)dt.Rows[i]["WC_Department_ID"];
                wcl.Add(wcls);
            }
            return wcl;
        }


        /// <summary>
        /// 列表双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lv_listview1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Point formPoint = e.GetPosition(this);
            if (formPoint.Y > 60 && formPoint.Y < 90)//表头部分不做双击响应
            {
            }
            else
            {
                btn_Chose_Click(sender, e);
            }
        }


    }
}
