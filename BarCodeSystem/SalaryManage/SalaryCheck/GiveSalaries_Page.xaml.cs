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

namespace BarCodeSystem.FileQuery.GiveSalaries
{
    /// <summary>
    /// GiveSalaries_Page.xaml 的交互逻辑
    /// </summary>
    public partial class GiveSalaries_Page : Page
    {

        int loadcount = 0;
        DataSet ds = new DataSet();
        List<WorkCenterLists> wcls = new List<WorkCenterLists>();
        string dept_code;

        public GiveSalaries_Page()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 页面加载程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadcount == 0)
            {
                //加载年份并默认当前年份和月份为当前时间
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
        /// 部门选择事件
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
            this.Cursor = Cursors.Wait;
            if (txtb_DeptInfo.Text != "点击放大镜选择")
            {
                GiveSalariesDetail_Page gsp = new GiveSalariesDetail_Page(dept_code,comobox_year.Text,comobox_month.Text);
                frame_Search.Navigate(gsp);
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// datagrid选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_Dept_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn_Chose_Click(null, null);
        }
    }
}
