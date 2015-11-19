using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.HelperClass;
using BarCodeSystem.SalaryManage.SalaryCheck;
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
    /// GiveSalariesDetail_Page.xaml 的交互逻辑
    /// </summary>
    public partial class GiveSalariesDetail_Page : Page
    {

        int loadcount = 0;//页面加载次数
        DataSet ds = new DataSet();
        DataTable tb = new DataTable();
        string dept_code;
        string year;
        string month;
        DateTime dtime;//起始时间
        DateTime dtime2;//结束时间

        List<FlowCardSubLists> fcsll = new List<FlowCardSubLists>();

        List<FlowCardSubLists> fcsllsum = new List<FlowCardSubLists>();

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public GiveSalariesDetail_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 查询工资的构造函数
        /// </summary>
        /// <param name="dept_code"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public GiveSalariesDetail_Page(string dept_code, string year, string month)
        {
            InitializeComponent();
            this.dept_code = dept_code;
            this.year = year;
            this.month = month;
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
                //ListBeforeSearch();

                fcsll = FlowCardSubLists.FetchFCS_Info(Convert.ToDateTime(year + "-" + month + "-01"), Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1).AddDays(-1), dept_code);
                BindingList();

                txb_Tip.Text = "共" + fcsllsum.Count + "行," + "应发工资" + fcsllsum.Sum(p => p.TotalPayMount).ToString("c") + "元,实发工资" + fcsllsum.Sum(p => p.FinalPayMount).ToString("c") + "元";
                loadcount++;
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 根据SQL语得到表并将其存入类文件,测试SQL语句用
        /// </summary>
        private void ListBeforeSearch()
        {
            dtime = Convert.ToDateTime(year + "-" + month + "-01");//每月第一天
            dtime2 = dtime.AddMonths(1).AddDays(-1);//每月最后一天最后一秒

            string SQl = "select " +
                "F.II_Code," +
                "F.II_Name," +
                "F.II_Spec," +
                "A.FCS_ProcessName," +
                "A.FCS_QulifiedAmount," +
                "B.FC_Code," +
                "D.WH_WorkHour, " +
                "B.ID as FC_ID," +
                "C.ID as TR_ID," +
                "A.FCS_ProcessID as ProcessID " +
                "from FlowCardSub as A " +
                "left join  FlowCard as B on (A.FCS_FlowCardID = B.ID) " +
                "left join  TechRoute as C on (A.FCS_TechRouteID = C.ID) " +
                "left join  WorkHour as D on (D.WH_TechRouteID = C.ID) " +
                "left join WorkCenter as E on (E.WC_Department_ID = C.TR_WorkCenterID) " +
                "left join ItemInfo as F on (F.ID = C.TR_ItemID) " +
                " where " +
                " E.WC_Department_Code = '" + dept_code + "'" +
                " and A.FCS_ReportTime >= '" + dtime.ToString("yyyy-MM-dd HH:MM:ss") + "'" +
                " and A.FCS_ReportTime <= '" + dtime2.ToString("yyyy-MM-dd HH:MM:ss") + "'" +
                " and D.ID = (select MAX(ID) from WorkHour as G  where G.WH_TechRouteID = C.ID  " +
                " and CONVERT(date, G.WH_StartDate)<=CONVERT(date, A.FCS_ReportTime) " +
                " and CONVERT(date, G.WH_EndDate)>=CONVERT(date, A.FCS_ReportTime)) " +
                " and A.FCS_IsReported = 1" +
                " and B.FC_CardState = 5";

            MyDBController.GetConnection();
            tb = MyDBController.GetDataSet(SQl, ds, "GiveSalaries").Tables["GiveSalaries"];
            MyDBController.CloseConnection();

            foreach (DataRow dr in tb.Rows)
            {
                FlowCardSubLists fcsl = new FlowCardSubLists();
                fcsl.FCS_FlowCardID = (Int64)dr["FC_ID"];
                fcsl.FCS_TechRouteID = (Int64)dr["TR_ID"];
                fcsl.FCS_ProcessID = (Int64)dr["ProcessID"];

                fcsl.FC_Code = dr["FC_Code"].ToString();
                fcsl.II_Code = dr["II_Code"].ToString();
                fcsl.II_Name = dr["II_Name"].ToString();
                fcsl.II_Spec = dr["II_Spec"].ToString();

                fcsl.FCS_ProcessName = dr["FCS_ProcessName"].ToString();
                fcsl.WH_WorkHour = decimal.Parse(dr["WH_WorkHour"].ToString());
                fcsl.FCS_QulifiedAmount = Convert.ToInt32(dr["FCS_QulifiedAmount"].ToString());

                fcsll.Add(fcsl);
            }

        }


        /// <summary>
        /// 将数据绑定到datagrid
        /// </summary>
        private void BindingList()
        {
            List<FlowCardSubLists> newfcsll = new List<FlowCardSubLists>();

            foreach (IGrouping<long, FlowCardSubLists> item in fcsll.GroupBy(p => p.FCS_FlowCardID))
            {
                List<FlowCardSubLists> _fcslList = item.Distinct(new ListComparer<FlowCardSubLists>((p1, p2) => p1.FCS_TechRouteID.Equals(p2.FCS_TechRouteID))).ToList();
                newfcsll.AddRange(_fcslList);

            }
            //fcsll.GroupBy(p => p.FC_ID).ToList().ForEach(p => p.Distinct(new ListComparer<FlowCardSubLists>((p1, p2) => p1.FCS_TechRouteID.Equals(p2.FCS_TechRouteID))).ToList().ForEach(item => newfcsll.Add(item)));


            fcsllsum = MyDBController.ListCopy<FlowCardSubLists>(newfcsll.Distinct(new ListComparer<FlowCardSubLists>((p1, p2) => p1.II_Code.Equals(p2.II_Code) && p1.FCS_ProcessName.Equals(p2.FCS_ProcessName))).ToList());

            fcsllsum.ForEach(
                p =>
                {
                    p.FCS_QulifiedAmount = newfcsll.FindAll(item => item.FCS_ProcessName.Equals(p.FCS_ProcessName) && item.II_Code.Equals(p.II_Code)).Sum(var => var.FCS_QulifiedAmount);
                    p.TotalPayMount = decimal.Round(p.WH_WorkHour * p.FCS_QulifiedAmount / 100, 2);
                    p.FinalPayMount = decimal.Round(p.TotalPayMount, 0);

                });


            datagrid_GiveSalariesDetail.ItemsSource = fcsllsum;
        }


        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_export_Click(object sender, RoutedEventArgs e)
        {
            if (fcsllsum.Count > 0)
            {

                DataTable table = new DataTable();
                table.Columns.Add("II_Code", typeof(string));
                table.Columns.Add("II_Name", typeof(string));
                table.Columns.Add("II_Spec", typeof(string));
                table.Columns.Add("FCS_ProcessName", typeof(string));
                table.Columns.Add("FCS_QulifiedAmount", typeof(decimal));

                table.Columns.Add("成品出库数", typeof(decimal));//成品出库数
                table.Columns.Add("WH_WorkHour", typeof(decimal));
                table.Columns.Add("TotalPayMount", typeof(decimal));
                table.Columns.Add("WH_WorkHour2", typeof(decimal));
                table.Columns.Add("FinalPayMount", typeof(decimal));

                table = MyDBController.ListToDataTable<FlowCardSubLists>(fcsllsum, table);

                table.Columns.Add("数量", typeof(decimal));
                table.Columns.Add("实发工资", typeof(decimal));
                table.Columns.Add("调拨工时", typeof(decimal));
                table.Columns.Add("质量奖赔", typeof(decimal));
                table.Columns.Add("其它", typeof(decimal));
                table.Columns.Add("合计实发工资", typeof(decimal));
                table.Columns.Add("事业部核定工资与实际发放差额", typeof(decimal));
                table.Columns.Add("公司核定工资与实际发放差额", typeof(decimal));

                table.Columns["II_Code"].ColumnName = "料号";
                table.Columns["II_Name"].ColumnName = "料名";
                table.Columns["II_Spec"].ColumnName = "型号";
                table.Columns["FCS_ProcessName"].ColumnName = "工序";
                table.Columns["FCS_QulifiedAmount"].ColumnName = "每道工序入库数量";
                table.Columns["WH_WorkHour"].ColumnName = "单件工资(分)";
                table.Columns["TotalPayMount"].ColumnName = "单件应发工资";
                table.Columns["WH_WorkHour2"].ColumnName = "单件工资(分 )";
                table.Columns["FinalPayMount"].ColumnName = "单件实发工资";

                QkRowChangeToColClass.CreateExcelFileForDataTable(table);

            }
        }

        /// <summary>
        /// 查看明细事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_details_Click(object sender, RoutedEventArgs e)
        {
            if(datagrid_GiveSalariesDetail.SelectedIndex > -1)
            {
                this.Cursor = Cursors.Wait;
                var x = (FlowCardSubLists)datagrid_GiveSalariesDetail.SelectedItem;
                frame_Detail.Navigate(new GiveSalariesDetailView_Page(fcsll.FindAll(p => p.II_Code.Equals(x.II_Code) && p.FCS_ProcessName.Equals(x.FCS_ProcessName))));
                this.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// datagrid鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_GiveSalariesDetail_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn_details_Click(sender,e);
        }

    }
}
