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

namespace BarCodeSystem
{
    /// <summary>
    /// BadProductSummary_Page.xaml 的交互逻辑
    /// </summary>
    public partial class BadProductSummary_Page : Page
    {


        DataSet ds = new DataSet();
        string Dept_Code;
        List<WorkCenterLists> wcls = new List<WorkCenterLists>();
        int loadCount = 0;

        public BadProductSummary_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗口加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                ListBeforeSearch();
                loadCount++;
            }
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

                Dept_Code = k.department_code;
            }
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Query_Click(object sender, RoutedEventArgs e)
        {
            BadProductSummaryDetail_Page bp = new BadProductSummaryDetail_Page();

            frame_Search.Navigate(bp);

            bp.ShowDeptInfo(Dept_Code, datepicker_StartDate.Text, datepicker_EndDate.Text, txtb_SearchKey.Text);
        }


        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_Dept_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn_Chose_Click(sender, e);
        }
    }
}
