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
    /// BadProductList_Page.xaml 的交互逻辑
    /// </summary>
    public partial class BadProductList_Page : Page
    {

        DataSet ds = new DataSet();
        string Dept_Code;

        List<WorkCenterLists> wcls = new List<WorkCenterLists>();

        int loadcount = 0;

        public BadProductList_Page()
        {
            InitializeComponent();
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
        /// 料品型号筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_SearchKey_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// 页面窗口加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            if (loadcount == 0)
            {
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
            if (txtb_DeptInfo.Text != "点击放大镜选择" && !string.IsNullOrEmpty(datepicker_StartDate.Text) && !string.IsNullOrEmpty(datepicker_EndDate.Text))
            {
                BadProductListDetail_Page bp = new BadProductListDetail_Page(Dept_Code, Convert.ToDateTime(datepicker_StartDate.SelectedDate).ToString("yyyy/MM/dd"), Convert.ToDateTime(datepicker_EndDate.SelectedDate).ToString("yyyy/MM/dd"), txtb_SearchKey.Text);
                frame_Search.Navigate(bp);
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请检查车间信息和日期信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
