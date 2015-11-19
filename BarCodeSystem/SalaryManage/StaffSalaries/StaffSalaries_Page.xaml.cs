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
    /// StaffSalaries_Page.xaml 的交互逻辑
    /// </summary>
    public partial class StaffSalaries_Page : Page
    {
        //展示员工工资信息
        List<StaffSalariesList> ssls = new List<StaffSalariesList>();

        /// <summary>
        /// 汇总工资列表
        /// </summary>
        List<StaffSalariesList> totalsslList = new List<StaffSalariesList>();
        //得到选中部门的信息
        WorkCenterLists wls = new WorkCenterLists();

        List<WorkCenterLists> wcList = new List<WorkCenterLists>();

        //工资汇总表
        DataTable datatable = new DataTable();

        //奖惩信息表
        DataTable datatable2 = new DataTable();
        DataSet ds = new DataSet();
        DateTime dtime = new DateTime(), dtime2 = new DateTime();
        string dept_code;


        int loadcount = 0;


        public StaffSalaries_Page()
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
                wcList = WorkCenterLists.FetchWCInfo();
                datagrid_Dept.ItemsSource = wcList;
                loadcount++;
            }


        }


        /// <summary>
        /// 获取月度质量将赔表信息
        /// </summary>
        /// <param name="dtimestr">起始日期</param>
        /// <param name="dtime2str">结束日期</param>
        private void QualitySalary(string dtimestr, string dtime2str)
        {
            string SQl = @"select " +
                "ID," +
                "QMS_Code," +
                "QMS_EffectiveTime," +
                "QMS_WorkerCenterCode," +
                "QMS_WorkerCenterName," +
                "QMS_PersonCode," +
                "QMS_PersonName," +
                "QMS_QualityMoney," +
                "QMS_SundryMoney," +
                "QMS_CreateByCode," +
                "QMS_CreateByName," +
                "QMS_CreateOn," +
                "QMS_ModifyBy," +
                "QMS_ModifyOn," +
                "QMS_HasCalculated " +
                " from  QualityMonthlySalary" +
                " where " +
                " QMS_EffectiveTime >= '" + dtimestr + "'" +
                " and QMS_EffectiveTime <= '" + dtime2str + "'" +
                " and QMS_WorkerCenterCode = '" + wls.department_code + "'";

            MyDBController.GetConnection();
            datatable2 = MyDBController.GetDataSet(SQl, ds, "QualitySalary").Tables["QualitySalary"];
            MyDBController.CloseConnection();
        }

        /// <summary>
        /// 获得员工工资明细表
        /// </summary>
        private void SalariesSum()
        {
            MyDBController.GetConnection();


            string SQl = @"select " +
                    "B.FCS_PersonCode," +
                    "B.FCS_PersonName," +
                    "C.WC_Department_Name," +
                    "A.FC_Code," +
                    "B.FCS_FlowCardID," +
                    "B.FCS_TechRouteID," +
                    "B.FCS_ProcessID," +
                    "B.FCS_ProcessName," +
                    "B.FCS_ReportTime," +
                    "FCS_QulifiedAmount," +
                    "E.WH_WorkHour " +
                    " from FlowCard as A " +
                    " left join FlowCardSub as B on (A.ID = B.FCS_FlowCardID) " +
                    " left join WorkCenter as C on (A.FC_WorkCenter = C.WC_Department_ID) " +
                    " left join TechRoute as D on (D.ID = B.FCS_TechRouteID) " +
                    " left join WorkHour as E on (D.ID = E.WH_TechRouteID)  " +
                    " where " +
                    " CONVERT(date, B.FCS_ReportTime) >='" + dtime.ToString("yyyy/MM/dd HH:MM:ss") + "'" +
                    " and CONVERT(date,B.FCS_ReportTime) <='" + dtime2.ToString("yyyy/MM/dd HH:MM:ss") + "'" +
                    " and B.FCS_IsReported='true'" +
                    " and C.WC_Department_Code = '" + wls.department_code + "'" +
                    " and E.ID = (select MAX(F.id) from WorkHour F where F.WH_TechRouteID=D.ID and CONVERT(date, F.WH_StartDate)<=CONVERT(date, B.FCS_ReportTime) " +
                    " and CONVERT(date, F.WH_EndDate)>=CONVERT(date, B.FCS_ReportTime))";




            datatable = MyDBController.GetDataSet(SQl, ds, "StaffSalaries").Tables["StaffSalaries"];

            MyDBController.CloseConnection();
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
                this.Cursor = Cursors.Wait;
                StaffSalariesDetail_Page ssdp = new StaffSalariesDetail_Page(comobox_year.Text, comobox_month.Text, dept_code);
                frame_Search.Navigate(ssdp);
                this.Cursor = Cursors.Arrow;
            }

        }

        /// <summary>
        /// 明细事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Detail_Click(object sender, RoutedEventArgs e)
        {

            //if (datagrid_Salaries.SelectedIndex > -1)
            //{
            //    var x = (StaffSalariesList)datagrid_Salaries.SelectedItem;
            //    StaffSalariesDetail_Page ssd = new StaffSalariesDetail_Page();
            //    frame_Search.Navigate(ssd);
            //    ssd.ShowDeepInfo(ssls, ((StaffSalariesList)datagrid_Salaries.SelectedItem).PersonCode);
            //}

        }

        /// <summary>
        /// 部门选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DeptSearch_Click(object sender, RoutedEventArgs e)
        {
            //frame_Search.Navigate(new DeptSearch_Page());
        }

        public void ShowDeptInfo(WorkCenterLists wls)
        {
            this.wls = wls;
            txtb_DeptInfo.Text = wls.department_name.ToString();
        }

        /// <summary>
        /// 双击事件，快捷选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_Salaries_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (datagrid_Salaries.SelectedIndex != -1)
            //{
            //    btn_Detail_Click(null, null);
            //}
        }

        /// <summary>
        /// 文本框内容改变，自动搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_SearchKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (txtb_SearchKey.Text.Length > 0)
            //{
            //    string key = txtb_SearchKey.Text;
            //    datagrid_Salaries.ItemsSource = totalsslList.FindAll(p => p.PersonCode.IndexOf(key) != -1 || p.PersonName.IndexOf(key) != -1);
            //}
            //else
            //{
            //    datagrid_Salaries.ItemsSource = totalsslList;
            //}
        }

        /// <summary>
        /// 导出按钮
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

                System.Windows.MessageBox.Show("导出成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }


        /// <summary>
        /// 部门选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_Dept_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn_Chose_Click(sender, e);
        }

        /// <summary>
        /// 部门按钮选择事件
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

    }
}
