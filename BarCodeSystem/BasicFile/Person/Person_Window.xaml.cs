using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;
using System.Collections.ObjectModel;

namespace BarCodeSystem
{
    /// <summary>
    /// Person_Window.xaml 的交互逻辑
    /// </summary>
    public partial class Person_Window : Window
    {
        DataSet ds;
        DataTable dt;
        ObservableCollection<PersonLists> listBeforeSearch = new ObservableCollection<PersonLists> { };
        public Person_Window()
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


            GetBCSPersonList();
        }

        private void GetBCSPersonList()
        {
            MyDBController.GetConnection();
            List<PersonLists> pls = new List<PersonLists> { };
            ds = new DataSet();

            listBeforeSearch.Clear();
            //这段SQl用来展示数据
            string SQl = string.Format(@"select B.[P_Code]
		                        ,B.[P_Name]
                                ,B.[P_Position]
		                        ,A.[WC_Department_Code]
		                        ,A.[WC_Department_Name]
                                ,A.[WC_Department_ID]
	                        from [WorkCenter] A
	                        left join [Person] B
	                            on B.[P_WorkCenterID]=A.[WC_Department_ID]");
            MyDBController.GetDataSet(SQl, ds, "PersonInfo");

            //这段SQl用来获取导入窗口需要用到的指定结构的表
            SQl = string.Format(@"select B.[ID]
                                ,B.[P_Code]
		                        ,B.[P_Name]
                                ,B.[P_WorkCenterID]
	                        from [Person] B");
            MyDBController.GetDataSet(SQl, ds, "Person");
            MyDBController.CloseConnection();
            dt = ds.Tables["PersonInfo"];
            int x = dt.Rows.Count;
            for (int i = 0; i < x; i++)
            {
                if (dt.Rows[i]["P_Code"].ToString() != "" && dt.Rows[i]["P_Name"].ToString() != "")
                {
                    PersonLists pl = new PersonLists();
                    pl.code = dt.Rows[i]["P_Code"].ToString();
                    pl.name = dt.Rows[i]["P_Name"].ToString();

                    pl.position = dt.Rows[i]["P_Position"].ToString();

                    pl.departName = dt.Rows[i]["WC_Department_Name"].ToString();
                    pl.departCode = dt.Rows[i]["WC_Department_Code"].ToString();
                    pl.departid = (Int64)dt.Rows[i]["WC_Department_ID"];
                    listBeforeSearch.Add(pl);
                    pls.Add(pl);
                }
            }
            pls = pls.OrderBy(p => p.code).ToList();
            listview1.DataContext = pls;
        }



        /// <summary>
        /// 导入按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            PersonImport_Window pi = new PersonImport_Window();
            pi.Height = Math.Min(User_Info.ScreenHeight * 3 / 5, 600);
            pi.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            pi.exisitPerson = ds.Tables["Person"];
            pi.ShowDialog();
            if ((bool)pi.DialogResult)
            {
                GetBCSPersonList();
            }
        }

        /// <summary>
        /// 全选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectAll_Click(object sender, RoutedEventArgs e)
        {

            foreach (PersonLists item in listview1.Items)
            {
                item.IsSelected = true;
            }
            listview1.Items.Refresh();
        }

        /// <summary>
        /// 重选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReSelect_Click(object sender, RoutedEventArgs e)
        {
            foreach (PersonLists item in listview1.Items)
            {
                item.IsSelected = false;
            }
            listview1.Items.Refresh();
        }


        /// <summary>
        /// 搜索框文本改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_SearchKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            btn_Search_Click(sender, e);
        }


        /// <summary>
        /// 搜索按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<PersonLists> pls = new ObservableCollection<PersonLists> { };
            listview1.ItemsSource = null;
            listview1.ItemsSource = listBeforeSearch;
            if (txtb_SearchKey.Text != "")
            {
                string key = txtb_SearchKey.Text;
                IEnumerable<PersonLists> IEpls =
                    from item in listBeforeSearch
                    where (item.name.IndexOf(key) != -1 || item.code.IndexOf(key) != -1
                         || item.departCode.IndexOf(key) != -1 || item.departName.IndexOf(key) != -1)
                    select item;
                foreach (PersonLists item in IEpls)
                {
                    pls.Add(item);
                }
                listview1.ItemsSource = null;
                listview1.ItemsSource = pls;
            }
            else { }
        }

        /// <summary>
        /// 修改选中信息按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            PersonModify_Window pm = new PersonModify_Window();
            pm.Height = Math.Min(User_Info.ScreenHeight * 2 / 5, 400);
            pm.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            PersonLists pl = null;

            foreach (PersonLists item in listview1.Items)
            {
                if (item.IsSelected)
                {
                    pl = item;
                    break;
                }
            }

            if (pl != null)
            {
                pm.pl = pl;
                pm.pls = listBeforeSearch;
                pm.ShowDialog();
                if ((bool)pm.DialogResult)
                {
                    GetBCSPersonList();
                }
            }
            else
            {
                pl = listview1.SelectedItem as PersonLists;
                if (pl != null)
                {
                    pm.pl = pl;
                    pm.pls = listBeforeSearch;
                    pm.ShowDialog();
                    if ((bool)pm.DialogResult)
                    {
                        GetBCSPersonList();
                    }
                }
            }
        }

        /// <summary>
        /// 列表双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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

        /// <summary>
        /// 新增人员信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            PersonModify_Window pm = new PersonModify_Window();
            pm.Height = Math.Min(User_Info.ScreenHeight * 2 / 5, 400);
            pm.Width = Math.Min(User_Info.ScreenWidth * 3 / 5, 600);
            pm.pls = listBeforeSearch;
            pm.Title = "人员新增窗口";
            pm.ShowDialog();
            if ((bool)pm.DialogResult)
            {
                GetBCSPersonList();
            }
        }

        /// <summary>
        /// 导出Excel模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Export_Click(object sender, RoutedEventArgs e)
        {
            DataTable templet = new DataTable();
            templet.TableName = "templet";

            templet.Columns.Add("员工编号", typeof(string));
            templet.Columns.Add("员工姓名", typeof(string));
            templet.Columns.Add("岗位", typeof(string));
            templet.Columns.Add("工作中心编号", typeof(string));
            templet.Columns.Add("工作中心名称", typeof(string));

            QkRowChangeToColClass.CreateExcelFileForDataTable(templet);

            MessageBox.Show("导出成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
