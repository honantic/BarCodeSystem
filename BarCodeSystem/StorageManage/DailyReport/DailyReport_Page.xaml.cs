using BarCodeSystem.FileQuery.DailyReport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarCodeSystem.FileQuery.DailyReport_Page
{
    /// <summary>
    /// DailyReport.xaml 的交互逻辑
    /// </summary>
    public partial class DailyReport_Page : Page
    {
        int loadcount = 0;
        DataSet ds = new DataSet();
        List<WorkCenterLists> wcls = new List<WorkCenterLists>();
        string dept_code;




        public DailyReport_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadcount == 0)
            {
                //加载年份
                for (int i = 2015; i < 2100; i++)
                {
                    comobox_year.Items.Add(i.ToString());
                }

                comobox_year.Text = DateTime.Now.Year.ToString();
                comobox_month.Text = DateTime.Now.Month.ToString();


                ListBeforeSearch();
                loadcount++;
            }
        }

        /// <summary>
        /// 得到工作中心信息
        /// </summary>
        private void ListBeforeSearch()
        {

            string SQl = @"select " +
                " WC_Department_Code," +
                " WC_Department_Name," +
                " WC_Department_ShortName " +
                " from WorkCenter ";

            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "WorkCenter");
            MyDBController.CloseConnection();
        }


        /// <summary>
        /// 部门查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DeptSearch_Click(object sender, RoutedEventArgs e)
        {
            wcls.Clear();

            DataRowCollection drc = ds.Tables["WorkCenter"].Rows;

            foreach (DataRow row in drc)
            {
                WorkCenterLists wcl = new WorkCenterLists();
                wcl.department_code = row["WC_Department_Code"].ToString();
                wcl.department_name = row["WC_Department_Name"].ToString();
                wcl.department_shortname = row["WC_Department_ShortName"].ToString();

                wcls.Add(wcl);
            }
            datagrid_Dept.ItemsSource = null;
            datagrid_Dept.ItemsSource = wcls;
        }


        /// <summary>
        /// 部门选择按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Chose_Click(object sender, RoutedEventArgs e)
        {
            int x = datagrid_Dept.SelectedIndex;

            if (x > -1)
            {
                var k = (WorkCenterLists)datagrid_Dept.SelectedItem;

                txtb_DeptInfo.Text = k.department_name;
                dept_code = k.department_code;
            }
        }


        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Query_Click(object sender, RoutedEventArgs e)
        {
            if (txtb_DeptInfo.Text != "点击放大镜选择")
            {
                DailyReportDetail_Page drdp = new DailyReportDetail_Page();
                frame_Search.Navigate(drdp);
                drdp.ShowDeptInfo(comobox_year.Text,comobox_month.Text,dept_code);
            }
        }

        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_Dept_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn_Chose_Click(null,null);
        }

        /// <summary>
        /// datagrid选中信息改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_Dept_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_Chose_Click(sender,e);
        }
    }
}
