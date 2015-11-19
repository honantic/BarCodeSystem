using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.HelperClass;
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
    /// InputOutputDetail_Page.xaml 的交互逻辑
    /// </summary>
    public partial class InputOutputDetail_Page : Page
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        List<FlowCardSubLists> fcslList = new List<FlowCardSubLists>();
        List<FlowCardSubLists> fcslListSum = new List<FlowCardSubLists>();
        int loadcount = 0;
        string year;
        string month;
        string dept_code;


        /// <summary>
        /// 默认构造函数
        /// </summary>
        public InputOutputDetail_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="dept_code"></param>
        public InputOutputDetail_Page(string year,string month,string dept_code)
        {
            InitializeComponent();
            this.year = year;
            this.month = month;
            this.dept_code = dept_code;
        }

        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(loadcount == 0)
            {
                //ListBeforeSearch();
                fcslList = FlowCardSubLists.FetchFCS_IOInfo(Convert.ToDateTime(year + "-" + month + "-01"), Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1).AddDays(-1), dept_code);
                BingDingList();
                txb_Tip.Text = "共"+ fcslListSum.Count + "条数据,合计投入数:" + fcslListSum.Sum(p => p.FC_Amount).ToString() + ",合计产出数:" + fcslListSum.Sum(p => p.FCS_QulifiedAmount).ToString();
                loadcount++;
            }
        }


        /// <summary>
        /// 读取数据并实例化
        /// </summary>
        private void ListBeforeSearch()
        {
            string SQl = @"select " +
                "B.FC_Code," +
                "B.FC_Amount," +
                "D.II_Code," +
                "D.II_Name," +
                "D.II_Spec," +
                "A.FCS_ProcessName," +
                "A.FCS_PersonName," +
                "C.TR_IsFirstProcess," +
                "C.TR_IsLastProcess," +
                "A.FCS_BeginAmount," +
                "A.FCS_QulifiedAmount " +
                " from FlowCardSub as A " +
                " left join FlowCard as B on (A.FCS_FlowCardID = B.ID)" +
                " left join TechRoute as C on (A.FCS_TechRouteID = C.ID)" +
                " left join ItemInfo as D on (C.TR_ItemID = D.ID)" +
                " left join WorkCenter as E on (E.WC_Department_ID = C.TR_WorkCenterID)" +
                " where " +
                " E.WC_Department_Code = '270106'  " +
                " and A.FCS_ReportTime >= '2015-10-01 00:00:00'" +
                " and A.FCS_ReportTime <='2015-10-31 23:59:59' " +
                " and B.FC_CardState = 5 " +
                " and A.FCS_IsReported = 1";

            MyDBController.GetConnection();
            dt = MyDBController.GetDataSet(SQl, ds, "InputOutput").Tables["InputOutput"];
            MyDBController.CloseConnection();

            foreach (DataRow dr in dt.Rows)
            {
                FlowCardSubLists fcsl = new FlowCardSubLists();
                fcsl.FC_Code = dr["FC_Code"].ToString();
                fcsl.FC_Amount = Int32.Parse(dr["FC_Amount"].ToString());
                fcsl.II_Code = dr["II_Code"].ToString();
                fcsl.II_Name = dr["II_Name"].ToString();
                fcsl.II_Spec = dr["II_Spec"].ToString();
                fcsl.FCS_ProcessName = dr["FCS_ProcessName"].ToString();
                fcsl.FCS_PersonName = dr["FCS_PersonName"].ToString();
                fcsl.FCS_IsFirstProcess = Convert.ToBoolean(dr["TR_IsFirstProcess"].ToString());
                fcsl.FCS_IsLastProcess = Convert.ToBoolean(dr["TR_IsLastProcess"].ToString());
                fcsl.FCS_BeginAmount = Int32.Parse(dr["FCS_BeginAmount"].ToString());
                fcsl.FCS_QulifiedAmount = Int32.Parse(dr["FCS_QulifiedAmount"].ToString());
                fcslList.Add(fcsl);
            }
        }


        /// <summary>
        /// 使用LINQ将数据绑定到datagrid
        /// </summary>
        private void BingDingList()
        {
            List<FlowCardSubLists> _fcsl = new List<FlowCardSubLists>();
            //foreach (IGrouping<string, FlowCardSubLists> item in fcslList.GroupBy(p => p.FC_Code))
            //{
            //    _fcsl.Add(item.FirstOrDefault(p => p.FCS_IsLastProcess == true));

            //}
            fcslList.GroupBy(p => p.FC_Code).ToList().ForEach(p => _fcsl.Add(p.FirstOrDefault(q => q.FCS_IsLastProcess == true)));
            //fcslListSum = MyDBController.ListCopy<FlowCardSubLists>(_fcsl.Distinct(new ListComparer<FlowCardSubLists>((p1, p2) => p1.II_Code.Equals(p2.II_Code))).ToList());
            fcslListSum = _fcsl.Distinct(new ListComparer<FlowCardSubLists>((p1, p2) => p1.II_Code.Equals(p2.II_Code))).ToList();
            fcslListSum.ForEach(
                p =>
                {
                    p.FC_Amount = _fcsl.FindAll(n => n.FC_Code.Equals(p.FC_Code)).Sum(var => var.FC_Amount);
                    p.FCS_QulifiedAmount = _fcsl.FindAll(n => n.FC_Code.Equals(p.FC_Code)).Sum(var => var.FCS_QulifiedAmount);
                }
                );

            datagrid_InputOutDetail.ItemsSource = fcslListSum;
           
        }

        /// <summary>
        /// 导出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_export_Click(object sender, RoutedEventArgs e)
        {
            if (fcslListSum.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("II_Code", typeof(string));
                dt.Columns.Add("II_Name",typeof(string));
                dt.Columns.Add("II_Spec",typeof(string));
                dt.Columns.Add("FC_Amount",typeof(int));
                dt.Columns.Add("FCS_QulifiedAmount",typeof(int));
                dt = MyDBController.ListToDataTable<FlowCardSubLists>(fcslListSum, dt);
                dt.Columns.Add("上期结存",typeof(int)).SetOrdinal(3);
                dt.Columns.Add("投入合计",typeof(int)).SetOrdinal(5);
                dt.Columns.Add("本期结存",typeof(int)).SetOrdinal(7);
                dt.Columns.Add("本月调差",typeof(int)).SetOrdinal(8);
                dt.Columns.Add("产出合计", typeof(int)).SetOrdinal(9);
                dt.Columns.Add("合格品率", typeof(int)).SetOrdinal(10);
                dt.Columns.Add("数量率", typeof(int)).SetOrdinal(11);
                dt.Columns.Add("数量差异数", typeof(int)).SetOrdinal(12);
                dt.Columns.Add("差异率", typeof(int)).SetOrdinal(13);
                dt.Columns.Add("原因分析", typeof(int)).SetOrdinal(14);
                dt.Columns["II_Code"].ColumnName = "料号";
                dt.Columns["II_Name"].ColumnName = "料名";
                dt.Columns["II_Spec"].ColumnName = "规格";
                dt.Columns["FC_Amount"].ColumnName = "投入数";
                dt.Columns["FCS_QulifiedAmount"].ColumnName = "产出数";
                QkRowChangeToColClass.CreateExcelFileForDataTable(dt);
            }
        }
    }
}
