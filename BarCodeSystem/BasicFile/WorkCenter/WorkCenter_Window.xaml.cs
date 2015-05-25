using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Data;
using System.Windows.Forms;

namespace BarCodeSystem
{
    /// <summary>
    /// WorkCenter_Window.xaml 的交互逻辑
    /// </summary>
    public partial class WorkCenter_Window : Window
    {
        public WorkCenter_Window()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //这段代码在正式程序中将被注释掉，测试用
            //MyDBController.Server = User_Info.server[1];
            //MyDBController.Database = User_Info.database[1];
            //MyDBController.Uid = User_Info.uid[1];
            //MyDBController.Pwd = User_Info.pwd[1];


            listView1.ItemsSource=GetWorkCenterList() ;
        }

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <returns></returns>
        private List<WorkCenterLists> GetWorkCenterList()
        {
            MyDBController.GetConnection();
            DataTable dt;
            DataSet ds = new DataSet();
            List<WorkCenterLists> wcl = new List<WorkCenterLists> { };
            #region 获取条码系统工作中心数据
            string SQLCom = @"SELECT 
                               [WC_Department_ID],
                               [WC_LastOprateBy],
                               [WC_Department_Name],
                               [WC_LastOperateTime],
                               case [WC_IsValidated]
                               when 1 then '是'
                               when 0 then '否'
                               end
                               [WC_IsValidated],
                               [WC_IsValidated] as [WC_IsValidated_DB],
                               case [WC_IsOrderControled]
                               when 1 then '是'
                               when 0 then '否'
                               end
                               [WC_IsOrderControled],
                               [WC_IsOrderControled] as [WC_IsOrderControled_DB],
                               case [WC_IsWorkCenter]
                               when 1 then '是'
                               when 0 then '否'
                               end
                               [WC_IsWorkCenter],
                               [WC_IsWorkCenter] as [WC_IsWorkCenter_DB],
                               [WC_Department_Code]
                          FROM [WorkCenter]";
            ds = MyDBController.GetDataSet(SQLCom, ds,"WorkCenter");
            MyDBController.CloseConnection();
            dt = ds.Tables[0];
            #endregion

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                WorkCenterLists wcl1 = new WorkCenterLists();
                wcl1.department_id = (Int64)dt.Rows[i]["WC_Department_ID"];
                wcl1.department_code = dt.Rows[i]["WC_Department_Code"].ToString();
                wcl1.department_name = dt.Rows[i]["WC_Department_Name"].ToString();
                wcl1.isordercontroled = dt.Rows[i]["WC_IsOrderControled"].ToString();
                wcl1.isworkcenter = dt.Rows[i]["WC_IsWorkCenter"].ToString();
                wcl1.isvalidated = dt.Rows[i]["WC_IsValidated"].ToString();
                wcl1.lastoperateby = dt.Rows[i]["WC_LastOprateBy"].ToString();
                wcl1.lastoperatetime = dt.Rows[i]["WC_LastOperateTime"].ToString() ;
                wcl1.isordercontroled_DB = (bool)dt.Rows[i]["WC_IsOrderControled_DB"];
                wcl1.isvalidated_DB = (bool)dt.Rows[i]["WC_IsValidated_DB"];
                wcl1.isworkcenter_DB = (bool)dt.Rows[i]["WC_IsWorkCenter_DB"];
                wcl.Add(wcl1);
            }
            return wcl;
        }


        /// <summary>
        /// 导入工作中心按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            WorkCenterImport_Window wci = new WorkCenterImport_Window();
            wci.Height = Math.Min(User_Info.ScreenHeight * 3 / 5, 600);
            wci.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            bool? result=wci.ShowDialog();
            if (result==true)
            {
                Window_Loaded(sender,e);
            }
        }

        /// <summary>
        /// 修改工作中心信息按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            WorkCenterLists item =listView1.SelectedItem as WorkCenterLists;
            if (item != null)
            {
                WorkCenterModify_Window wcm = new WorkCenterModify_Window();
                wcm.Height =Math.Min( User_Info.ScreenHeight * 2 / 5,400);
                wcm.Width = Math.Min( User_Info.ScreenWidth * 3 / 5,600);
                wcm.dept_info = item;
                wcm.ShowDialog();

                if ((bool)wcm.DialogResult)
                {
                    listView1.ItemsSource = null;
                    listView1.ItemsSource = GetWorkCenterList();
                }
            }
            else 
            {
                System.Windows.MessageBox.Show("请选择一个工作中心！","提示",MessageBoxButton.OK,MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// 双击事件,修改点击的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point formPoint = e.GetPosition(this);
            if (formPoint.Y>60 &&formPoint.Y<90)//表头部分不做双击响应
            {                
            }
            else
            {
                btn_Modify_Click(sender, e);
            }
            
        }


        /// <summary>
        /// listview1左键点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
