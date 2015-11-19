using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.HelperClass;
using BarCodeSystem.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// DeptSearch_Page.xaml 的交互逻辑
    /// </summary>
    public partial class StaffSalariesDetail_Page : Page
    {

        List<WorkCenterLists> wcls = new List<WorkCenterLists>();
        DataSet ds = new DataSet();


        List<StaffSalariesList> totalsslList = new List<StaffSalariesList>();
        List<StaffSalariesList> ssls = new List<StaffSalariesList>();
        string year;
        string month;
        string dept_code;
        int loadcount = 0;

        /// <summary>
        /// 默认构造方法
        /// </summary>
        public StaffSalariesDetail_Page()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="dept_code"></param>
        public StaffSalariesDetail_Page(string year, string month, string dept_code)
        {
            InitializeComponent();
            this.year = year;
            this.month = month;
            this.dept_code = dept_code;
        }

        /// <summary>
        /// 查看明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_StaffSalariesDetial.SelectedIndex > -1)
            {
                this.Cursor = Cursors.Wait;
                var x = (StaffSalariesList)datagrid_StaffSalariesDetial.SelectedItem;
                StaffSalariesDetailView_Page ssdv = new StaffSalariesDetailView_Page(ssls, ((StaffSalariesList)datagrid_StaffSalariesDetial.SelectedItem).PersonCode);
                frame_Detail.Navigate(ssdv);
                this.Cursor = Cursors.Arrow;
            }
        }


        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (loadcount == 0)
            {
                ssls = StaffSalariesList.Fetch_SSInfo(Convert.ToDateTime(year + "-" + month + "-01"), Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1).AddDays(-1), dept_code);
                BingDingList();
                loadcount++;
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 使用LINQ筛选数据并绑定
        /// </summary>
        private void BingDingList()
        {
            Thread th = new Thread(() =>
            {
                Loading_Window lw = new Loading_Window("正在计算员工工资，请稍等");
                lw.ShowDialog();
            });
            Task t = new Task(() =>
            {
                foreach (StaffSalariesList item in ssls)
                {
                    item.PeopleAmount = (decimal)ssls.FindAll(p => p.FC_ID == item.FC_ID && p.TR_ID == item.TR_ID && p.ProcessID == item.ProcessID).Count;
                    item.Salary = decimal.Round(item.QulifiedAmount / item.PeopleAmount * item.WorkHour / 100, 3);
                }
                totalsslList = MyDBController.ListCopy<StaffSalariesList>(ssls.Distinct(new ListComparer<StaffSalariesList>((p1, p2) => p1.PersonCode.Equals(p2.PersonCode))).ToList());
                totalsslList.ForEach(
                     p =>
                     {
                         p.Salary = ssls.FindAll(item => item.PersonCode.Equals(p.PersonCode)).Sum(var => var.Salary);
                     });
            });
            t.Start();
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            while (!t.IsCompleted)
            {
            }
            th.Abort();
            datagrid_StaffSalariesDetial.ItemsSource = totalsslList;
        }


        private List<WorkCenterLists> FetchDeptInfo()
        {
            ds.Clear();

            string SQl = string.Format(@"select WC_Department_Code,WC_Department_Name,WC_Department_ShortName from WorkCenter");

            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "DeptInfo");
            MyDBController.CloseConnection();

            wcls.Clear();

            DataRowCollection drc = ds.Tables["DeptInfo"].Rows;

            try
            {
                foreach (DataRow row in drc)
                {
                    WorkCenterLists wcl = new WorkCenterLists();

                    wcl.department_code = row["WC_Department_Code"].ToString();
                    wcl.department_name = row["WC_Department_Name"].ToString();
                    wcl.department_shortname = row["WC_Department_ShortName"].ToString();

                    wcls.Add(wcl);

                }
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            return wcls;
        }

        /// <summary>
        /// 部门信息搜索功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ProduceOrderSearch_Click(object sender, RoutedEventArgs e)
        {
            //if (string.IsNullOrEmpty(txtb_DeptInfoSearch.Text))
            //{
            //    datagrid_DeptInfo.ItemsSource = wcls;
            //    datagrid_DeptInfo.Items.Refresh();
            //}
            //else
            //{
            //    string key = txtb_DeptInfoSearch.Text;
            //    datagrid_DeptInfo.ItemsSource = wcls.FindAll( p => p.department_code.IndexOf(key) != -1 || p.department_name.IndexOf(key) != -1 || p.department_shortname.IndexOf(key) != -1);
            //    datagrid_DeptInfo.Items.Refresh();
            //}
        }

        /// <summary>
        /// 回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_ProduceOrderInfo_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    btn_ProduceOrderSearch_Click(sender, e);
            //}
        }

        /// <summary>
        /// 双击快捷选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_DeptInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (datagrid_DeptInfo.SelectedIndex != -1)
            //{
            //    btn_Submit_Click(null, null);
            //}
        }


        /// <summary>
        /// 导出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Export_Click(object sender, RoutedEventArgs e)
        {
            if (ssls.Count > 0)
            {
                DataTable table = MyDBController.ListToDataTable<StaffSalariesList>(totalsslList);
                table.Columns.Remove("IsSelected");
                table.Columns.Remove("ProcessName");
                table.Columns.Remove("BeginAmount");
                table.Columns.Remove("QulifiedAmount");
                table.Columns.Remove("ScrappedAmount");
                table.Columns.Remove("PeopleAmount");
                table.Columns.Remove("FC_Code");
                table.Columns.Remove("Department_Name");
                table.Columns.Remove("ReportTime");
                table.Columns.Remove("WorkHour");
                table.Columns.Remove("FC_ID");
                table.Columns.Remove("TR_ID");
                table.Columns.Remove("ProcessID");
                table.Columns["PersonName"].ColumnName = "员工名称";
                table.Columns["PersonCode"].ColumnName = "员工编码";
                table.Columns["Salary"].ColumnName = "工资";
                QkRowChangeToColClass.CreateExcelFileForDataTable(table);

            }
        }

        /// <summary>
        /// 双击查看明细事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_StaffSalariesDetial_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn_Submit_Click(sender,e);
        }

        /// <summary>
        /// grid选中改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_StaffSalariesDetial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //btn_Submit_Click(sender, e);
        }

        /// <summary>
        /// 搜索员工工资
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_StaffSalariesSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtb_StaffSalariesSearch.Text))
            {
                datagrid_StaffSalariesDetial.ItemsSource = totalsslList;
                datagrid_StaffSalariesDetial.Items.Refresh();
            }
            else
            {
                string key = txtb_StaffSalariesSearch.Text;
                datagrid_StaffSalariesDetial.ItemsSource = totalsslList.FindAll(p => p.PersonCode.IndexOf(key) != -1 || p.PersonName.IndexOf(key) != -1);
                datagrid_StaffSalariesDetial.Items.Refresh();
            }
        }
    }
}
