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

namespace BarCodeSystem.FileQuery.InputOutput
{
    /// <summary>
    /// InputOutput_Page.xaml 的交互逻辑
    /// </summary>
    public partial class InputOutput_Page : Page
    {

        List<WorkCenterLists> wcls = new List<WorkCenterLists>();
        DataSet ds = new DataSet();
        string dept_code;
        int loadcount = 0;

        public InputOutput_Page()
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
                //加载年份并默认当前年份和月份为当前时间
                for (int i = 2015; i < 2100; i++)
                {
                    comobox_year.Items.Add(i.ToString());
                }
                comobox_year.Text = DateTime.Now.Year.ToString();
                comobox_month.Text = DateTime.Now.Month.ToString();
                loadcount++;
            }
        }

        /// <summary>
        /// 部门搜索事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DeptSearch_Click(object sender, RoutedEventArgs e)
        {
            wcls = WorkCenterLists.FetchWCInfo();
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
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Query_Click(object sender, RoutedEventArgs e)
        {
            if (txtb_DeptInfo.Text != "点击放大镜选择")
            {
                InputOutputDetail_Page iopd = new InputOutputDetail_Page(comobox_year.Text,comobox_month.Text,dept_code);
                frame_Search.Navigate(iopd);
            }
        }

        /// <summary>
        /// datagrid双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_Dept_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn_Chose_Click(sender,e);
        }
    }
}
