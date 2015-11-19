using BarCodeSystem.FileQuery.InputOutPut;
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
    public partial class StoreReportDetail_Page : Page
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        List<TechRouteLists> trList = new List<TechRouteLists>();
        List<FlowCardSubLists> fcslList = new List<FlowCardSubLists>();
        List<FlowCardSubLists> _fcsl = new List<FlowCardSubLists>();
        List<FlowCardSubLists> fcslListSum = new List<FlowCardSubLists>();
        int loadcount = 0;
        string year;
        string month;
        string dept_code;


        public delegate void DeFrame(Page childPage);
        DeFrame df;


        /// <summary>
        /// 默认构造函数
        /// </summary>
        public StoreReportDetail_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="dept_code"></param>
        public StoreReportDetail_Page(string year, string month, string dept_code, DeFrame df)
        {
            InitializeComponent();
            this.year = year;
            this.month = month;
            this.dept_code = dept_code;
            this.df = df;

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
                trList = TechRouteLists.FetchTechRouteCheckIsCountingProcess();
                GetTechRouteIsCountingProcess();
                fcslList = FlowCardSubLists.FetchFCS_SRInfo(Convert.ToDateTime(year + "-" + month + "-01"), Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1).AddSeconds(-1), dept_code);
                BingDingList();
                txb_Tip.Text = "共" + fcslListSum.Count + "条数据," + ",合计产出数:" + fcslListSum.Sum(p => p.FCS_QulifiedAmount).ToString();
                loadcount++;
            }
            this.Cursor = Cursors.Arrow;
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
                "F.TRV_VersionCode," +
                "F.TRV_VersionName," +
                "C.TR_ProcessCode," +
                "C.TR_ProcessSequence," +
                "A.FCS_ProcessName," +
                "A.FCS_PersonName," +
                "C.TR_IsFirstProcess," +
                "C.TR_IsLastProcess," +
                "A.FCS_BeginAmount," +
                "A.FCS_QulifiedAmount," +
                "A.FCS_IsReported" +
                " from FlowCardSub as A " +
                " left join FlowCard as B on (A.FCS_FlowCradID = B.ID)" +
                " left join TechRoute as C on (A.FCS_TechRouteID = C.ID)" +
                " left join ItemInfo as D on (C.TR_ItemID = D.ID)" +
                " left join WorkCenter as E on (E.WC_Department_ID = C.TR_WorkCenterID)" +
                " left join TechRouteVersion as F on (f.ID = B.FC_ItemTechVersionID)" +
                " where " +
                " E.WC_Department_Code = '270106'  " +
                " and A.FCS_ReportTime >= '2015-10-01 00:00:00'" +
                " and A.FCS_ReportTime <='2015-10-31 23:59:59' ";


            MyDBController.GetConnection();
            dt = MyDBController.GetDataSet(SQl, ds, "StoreReport").Tables["StoreReport"];
            MyDBController.CloseConnection();

            foreach (DataRow dr in dt.Rows)
            {
                FlowCardSubLists fcsl = new FlowCardSubLists();
                fcsl.FC_Code = dr["FC_Code"].ToString();
                fcsl.FC_Amount = Int32.Parse(dr["FC_Amount"].ToString());
                fcsl.II_Code = dr["II_Code"].ToString();
                fcsl.II_Name = dr["II_Name"].ToString();
                fcsl.II_Spec = dr["II_Spec"].ToString();
                fcsl.TRV_VersionCode = dr["TRV_VersionCode"].ToString();
                fcsl.TRV_VersionName = dr["TRV_VersionName"].ToString();
                //fcsl.TR_ProcessSequence = Int32.Parse(dr["TR_ProcessSequence"].ToString());
                fcsl.FCS_ProcessSequanece = Int32.Parse(dr["TR_ProcessSequence"].ToString());
                fcsl.FCS_ProcessCode = dr["TR_ProcessCode"].ToString();
                fcsl.FCS_ProcessName = dr["FCS_ProcessName"].ToString();
                fcsl.FCS_PersonName = dr["FCS_PersonName"].ToString();
                fcsl.FCS_IsFirstProcess = Convert.ToBoolean(dr["TR_IsFirstProcess"].ToString());
                fcsl.FCS_IsLastProcess = Convert.ToBoolean(dr["TR_IsLastProcess"].ToString());
                fcsl.FCS_BeginAmount = (int)dr["FCS_BeginAmount"];
                fcsl.FCS_QulifiedAmount = (int)dr["FCS_QulifiedAmount"];
                fcsl.FCS_IsReported = Convert.ToBoolean(dr["FCS_IsReported"].ToString());
                fcslList.Add(fcsl);
            }
        }


        /// <summary>
        /// 得到料品各个版本关键工序表
        /// </summary>
        private void GetTechRouteIsCountingProcess()
        {
            foreach (TechRouteLists tr in trList)
            {
                FlowCardSubLists fcsl = new FlowCardSubLists();
                fcsl.II_Code = tr.TR_ItemCode;
                fcsl.II_Name = tr.II_Name;
                fcsl.TRV_VersionCode = tr.TRV_VersionCode;
                fcsl.TRV_VersionName = tr.TRV_VersionName;
                fcsl.FCS_ProcessSequanece = tr.TR_ProcessSequence;
                fcsl.FCS_ProcessCode = tr.TR_ProcessCode;
                fcsl.FCS_ProcessName = tr.TR_ProcessName;
                fcslListSum.Add(fcsl);
            }
        }



        /// <summary>
        /// 使用LINQ将数据绑定到datagrid
        /// </summary>
        private void BingDingList()
        {
            fcslList.GroupBy(p => p.FC_Code).ToList().ForEach(p => _fcsl.AddRange(p.Distinct(new ListComparer<FlowCardSubLists>((m, n) => m.FCS_ProcessSequanece.Equals(n.FCS_ProcessSequanece))).ToList()));
            _fcsl.GroupBy(p => p.FC_Code).ToList().ForEach(p =>
                {
                    FlowCardSubLists fcsl = p.OrderBy(q => q.FCS_ProcessSequanece).ToList().Last();

                    int nowSeq = fcsl.FCS_ProcessSequanece;
                    string ic = fcsl.II_Code;
                    string trv = fcsl.TRV_VersionCode;
                    decimal qty = fcsl.FCS_QulifiedAmount;
                    string fccode = fcsl.FC_Code;
                    int nextCountingSeq = trList.FirstOrDefault(o => o.TR_ItemCode.Equals(fcsl.II_Code) && o.TRV_VersionCode.Equals(fcsl.TRV_VersionCode) && o.TR_ProcessSequence >fcsl.FCS_ProcessSequanece).TR_ProcessSequence;
                    int preCountingSeq = trList.Last(o => o.TR_ItemCode.Equals(fcsl.II_Code) && o.TRV_VersionCode.Equals(fcsl.TRV_VersionCode) && o.TR_ProcessSequence <=fcsl.FCS_ProcessSequanece).TR_ProcessSequence;

                    fcslListSum.Find(o => o.II_Code.Equals(fcsl.II_Code) && o.TRV_VersionCode.Equals(fcsl.TRV_VersionCode) && o.FCS_ProcessSequanece < nextCountingSeq && o.FCS_ProcessSequanece >= preCountingSeq).FCS_QulifiedAmount += fcsl.FCS_QulifiedAmount;
                }
                );
            fcslListSum.RemoveAll(p => p.FCS_QulifiedAmount == 0);
            datagrid_StoreReportDetail.ItemsSource = fcslListSum;
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
                dt.Columns.Add("II_Name", typeof(string));
                dt.Columns.Add("TRV_VersionCode", typeof(string));
                dt.Columns.Add("TRV_VersionName", typeof(string));
                dt.Columns.Add("FCS_ProcessSequanece", typeof(string));
                dt.Columns.Add("FCS_ProcessName", typeof(string));
                //dt.Columns.Add("FC_Amount",typeof(int));
                dt.Columns.Add("FCS_QulifiedAmount", typeof(int));
                dt = MyDBController.ListToDataTable<FlowCardSubLists>(fcslListSum, dt);
                dt.Columns.Add("上期结存", typeof(int)).SetOrdinal(3);
                dt.Columns.Add("投入合计", typeof(int)).SetOrdinal(5);
                dt.Columns.Add("本期结存", typeof(int)).SetOrdinal(7);
                dt.Columns.Add("本月调差", typeof(int)).SetOrdinal(8);
                dt.Columns.Add("产出合计", typeof(int)).SetOrdinal(9);
                dt.Columns.Add("合格品率", typeof(int)).SetOrdinal(10);
                dt.Columns.Add("数量率", typeof(int)).SetOrdinal(11);
                dt.Columns.Add("数量差异数", typeof(int)).SetOrdinal(12);
                dt.Columns.Add("差异率", typeof(int)).SetOrdinal(13);
                dt.Columns.Add("原因分析", typeof(int)).SetOrdinal(14);
                dt.Columns["II_Code"].ColumnName = "料号";
                dt.Columns["II_Name"].ColumnName = "料名";
                dt.Columns["TRV_VersionCode"].ColumnName = "版本号";
                dt.Columns["TRV_VersionName"].ColumnName = "版本名称";
                dt.Columns["FCS_ProcessSequanece"].ColumnName = "工序号";
                dt.Columns["FCS_ProcessName"].ColumnName = "工序名称";
                //dt.Columns["FC_Amount"].ColumnName = "投入数";
                dt.Columns["FCS_QulifiedAmount"].ColumnName = "产出数";
                QkRowChangeToColClass.CreateExcelFileForDataTable(dt);
            }
        }


        /// <summary>
        /// 查看明细事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_detail_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_StoreReportDetail.SelectedIndex > -1)
            {
                var x = (FlowCardSubLists)datagrid_StoreReportDetail.SelectedItem;
                StoreReportDetailView_Page srdv = new StoreReportDetailView_Page(_fcsl.FindAll(p => p.II_Code.Equals(x.II_Code) && p.TRV_VersionCode.Equals(x.TRV_VersionCode)), x.FCS_ProcessSequanece, trList);
                MyDBController.FindVisualParent<StoreReport_Page>(this).ForEach(p => p.frame_Search.Navigate(srdv));
            }
        }

        /// <summary>
        /// datagrid鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_StoreReportDetail_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                btn_detail_Click(sender, e);
            }
        }
    }
}
