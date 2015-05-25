using System.Collections.Generic;
using System.Windows;
using System.Data;
using System;
using System.Linq;

namespace BarCodeSystem
{
    /// <summary>
    /// WorkTeam_Window.xaml 的交互逻辑
    /// </summary>
    public partial class WorkTeam_Window : Window
    {
        //班组信息列表，窗体listview数据源
        List<WorkTeamLists> listBeforeSearch = new List<WorkTeamLists> { };

        //全局变量，条码系统班组清单dt，ds
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();

        public WorkTeam_Window()
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

            //这段代码在正式程序中将被注释掉，测试用
            MyDBController.Server = User_Info.server[1];
            MyDBController.Database = User_Info.database[1];
            MyDBController.Uid = User_Info.uid[1];
            MyDBController.Pwd = User_Info.pwd[1];

      
            GetworkTeamList();
            listview1.ItemsSource = null;
            listview1.ItemsSource = listBeforeSearch;
        }

        /// <summary>
        /// 获取条码系统班组信息
        /// </summary>
        private void GetworkTeamList()
        {


            MyDBController.GetConnection();
            string SQl = @"SELECT A.[WC_Department_ID], A.[WC_Department_Code],A.[WC_Department_Name],B.[WT_Code],B.[WT_Name]
                    FROM  [WorkCenter] A LEFT JOIN  [WorkTeam] B
                    ON A.[WC_Department_ID] = B.[WT_WorkCenterID]";
            dt.Clear();
            ds.Clear();
            listBeforeSearch.Clear();
            dt = MyDBController.GetDataSet(SQl, ds).Tables[0];
            MyDBController.CloseConnection();
            //为wtl赋值
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["WT_Code"].ToString() != "")
                {
                    WorkTeamLists wt = new WorkTeamLists();
                    wt.workcenterCode = dt.Rows[i]["WC_Department_Code"].ToString();
                    wt.workcenterName = dt.Rows[i]["WC_Department_Name"].ToString();
                    wt.WT_Name = dt.Rows[i]["WT_Name"].ToString();
                    wt.WT_Code = dt.Rows[i]["WT_Code"].ToString();
                    listBeforeSearch.Add(wt);
                }

            }
        }

        /// <summary>
        /// 修改班组按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            WorkTeamLists item = listview1.SelectedItem as WorkTeamLists;
            if (item == null)
            {
                MessageBox.Show("你没有选择任何班组！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else 
            {
                WorkTeamModify_Window wtm = new WorkTeamModify_Window();
                wtm.Title = "班组信息修改窗口";
                wtm.Height = Math.Min(User_Info.ScreenHeight,400);
                wtm.Width = Math.Min(User_Info.ScreenWidth,600);
                wtm.wt = item;
                wtm.workteam = dt;
                wtm.ShowDialog();

                //修改成功，更新视图
                if ((bool)wtm.DialogResult)
                {
                    GetworkTeamList();
                    listview1.ItemsSource = null;
                    listview1.ItemsSource = listBeforeSearch;
                }
            }
        }

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
        /// 添加班组按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            WorkTeamModify_Window wtm = new WorkTeamModify_Window();
            wtm.Title = "班组信息新增窗口";
            wtm.Height = Math.Min(User_Info.ScreenHeight, 400);
            wtm.Width = Math.Min(User_Info.ScreenWidth, 600);
            wtm.workteam = dt;
            wtm.ShowDialog();

            //新增成功，更新视图
            if ((bool)wtm.DialogResult)
            {
                GetworkTeamList();
                listview1.ItemsSource = null;
                listview1.ItemsSource = listBeforeSearch;
            }
        }

        /// <summary>
        /// 搜索功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            listview1.ItemsSource = null;
            listview1.ItemsSource = listBeforeSearch;
            if (txtb_Search.Text !="")
            {
                string text = txtb_Search.Text;
                List<WorkTeamLists> wtl = new List<WorkTeamLists> { };
                IEnumerable<WorkTeamLists> IEwtl =
                    from item in listBeforeSearch
                    where (item.WT_Code.IndexOf(text) != -1 || item.WT_Name.IndexOf(text) != -1 ||
                        item.workcenterCode.IndexOf(text) != -1 || item.workcenterName.IndexOf(text) != -1)
                    select item;
                foreach (WorkTeamLists item in IEwtl)
                {
                    wtl.Add(item);
                }
                listview1.ItemsSource = null;
                listview1.ItemsSource = wtl;
            }
            else 
            {

            }
        }


        /// <summary>
        /// 搜索回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_Search_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key==System.Windows.Input.Key.Enter)
            {
                btn_Search_Click(sender,e);
            }
        }


        /// <summary>
        /// 搜索框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_Search_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            btn_Search_Click(sender, e);
        }

        /// <summary>
        /// listview 双击,修改双击信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview1_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point formPoint = e.GetPosition(this);
            if (formPoint.Y > 60 && formPoint.Y < 90)//表头部分不做双击响应
            {
            }
            else
            {
                btn_Modify_Click(sender, e);
            }
        }

    }
}
