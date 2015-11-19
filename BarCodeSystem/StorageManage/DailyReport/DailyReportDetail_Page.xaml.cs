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

namespace BarCodeSystem.FileQuery.DailyReport
{
    /// <summary>
    /// DailyReportDetail_Page.xaml 的交互逻辑
    /// </summary>
    public partial class DailyReportDetail_Page : Page
    {
        List<FlowCardSubLists> fcslList = new List<FlowCardSubLists>();
        int loadcount = 0;

        //当前月份的天数
        int dayNum;

        DataTable dailyReportTable;
        DataTable beforeTable;
        DataSet ds = new DataSet();
        string year;
        string month;
        string dept_code;


        public DailyReportDetail_Page()
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
                dayNum = DateTime.DaysInMonth(DateTime.Now.Year,DateTime.Now.Month);
                //fcslList = GetDailyReportTable();
                fcslList = FlowCardSubLists.FetchFCS_DRInfo(Convert.ToDateTime(year + "-" + month + "-01"), Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1).AddSeconds(-1), dept_code);
                BingdingView();
                //CreateNewTale();
                loadcount++;
            }
        }

        /// <summary>
        /// 得到日报表数据,测试SQL用
        /// </summary>
        private List<FlowCardSubLists> GetDailyReportTable()
        {
            //起始时间
            DateTime dtime = Convert.ToDateTime(year + "-" + month + "-01");
            //结束时间
            DateTime dtime2 = dtime.AddMonths(1).AddDays(-1);

            string SQl = @"select " +
                "B.FC_Code," +
                "F.TRV_VersionCode," +
                "F.TRV_VersionName," +
                "B.FC_Amount," +
                "D.II_Code," +
                "D.II_Name," +
                "D.II_Spec," +
                "A.FCS_ProcessName," +
                "A.FCS_PersonName," +
                "A.FCS_ReportTime," +
                "C.TR_IsFirstProcess," +
                "C.TR_IsLastProcess," +
                "A.FCS_QulifiedAmount " +
                "from FlowCardSub as A " +
                " left join FlowCard as B on (A.FCS_FlowCardID = B.ID) " +
                " left join TechRoute as C on (A.FCS_TechRouteID = C.ID)" +
                " left join ItemInfo as D on (C.TR_ItemID = D.ID)" +
                " left join WorkCenter as E on (E.WC_Department_ID = C.TR_WorkCenterID)" +
                " left join TechRouteVersion as F on(B.FC_ItemTechVersionID = F.ID) " +
                " where " +
                " E.WC_Department_Code = '270106' " +
                " and A.FCS_ReportTime >= '2015-10-01' " +
                " and A.FCS_ReportTime <='2015-10-31'" +
                " and B.FC_CardState = 5 " +
                " and A.FCS_IsReported = 1";         

            MyDBController.GetConnection();
            beforeTable = MyDBController.GetDataSet(SQl, ds, "beforeTable").Tables["beforeTable"];
            MyDBController.CloseConnection();


            foreach (DataRow dr in beforeTable.Rows)
            {
                //FlowCardLists fcsl = new FlowCardLists();
                FlowCardSubLists fcsl = new FlowCardSubLists();
                fcsl.FC_Code = dr["FC_Code"].ToString();
                fcsl.II_Code = dr["II_Code"].ToString();
                fcsl.II_Name = dr["II_Name"].ToString();
                fcsl.II_Spec = dr["II_Spec"].ToString();
                fcsl.TRV_VersionCode = dr["TRV_VersionCode"].ToString();
                fcsl.TRV_VersionName = dr["TRV_VersionName"].ToString();
                fcsl.FCS_ProcessName = dr["FCS_ProcessName"].ToString();
                fcsl.FCS_PersonName = dr["FCS_PersonName"].ToString();
                fcsl.FCS_IsFirstProcess = Convert.ToBoolean(dr["TR_IsFirstProcess"].ToString());
                fcsl.FCS_IsLastProcess = Convert.ToBoolean(dr["TR_IsLastProcess"].ToString());
                fcsl.FCS_ReportTime = DateTime.Parse(dr["FCS_ReportTime"].ToString());
                fcsl.FCS_QulifiedAmount = (Int32)dr["FCS_QulifiedAmount"];
                fcslList.Add(fcsl);
            }

            return fcslList;


        }


        /// <summary>
        /// 创建新表并增加列
        /// </summary>
        private void CreateNewTale()
        {
            dailyReportTable = null;
            dailyReportTable = new DataTable();

            dailyReportTable.Columns.Add("料号", typeof(string));
            dailyReportTable.Columns.Add("料名", typeof(string));
            dailyReportTable.Columns.Add("版本名称", typeof(string));
            dailyReportTable.Columns.Add("月计划合计数", typeof(decimal));
            dailyReportTable.Columns.Add("累计完成数", typeof(decimal));
            dailyReportTable.Columns.Add("日计划累计数", typeof(decimal));
            dailyReportTable.Columns.Add("日累计差异数", typeof(decimal));
            dailyReportTable.Columns.Add("完成率",typeof(decimal));

            for (int i = 1; i <= dayNum; i++)
            {
                dailyReportTable.Columns.Add(i.ToString() + "日库存数", typeof(decimal));
                dailyReportTable.Columns.Add(i.ToString() + "日入库数", typeof(decimal));
                dailyReportTable.Columns.Add(i.ToString() + "日出库数", typeof(decimal));
            }

            DataRowCollection drc = beforeTable.Rows;
            foreach(DataRow row in drc)
            {
                DataRow r1 = dailyReportTable.NewRow();
                if (dailyReportTable.Select("料号 = '" + row["II_Code"].ToString() + "' and 版本名称 = '" + row["TRV_VersionName"].ToString() + "'").Length > 0)
                {
                    if (r1[DateTime.Parse(row["FCS_ReportTime"].ToString()).Day + "日入库数"] is DBNull)
                    {
                        r1[DateTime.Parse(row["FCS_ReportTime"].ToString()).Day + "日入库数"] = 0.0m;
                    }

                    r1[DateTime.Parse(row["FCS_ReportTime"].ToString()).Day + "日入库数"] = decimal.Parse(r1[DateTime.Parse(row["FCS_ReportTime"].ToString()).Day + "日入库数"].ToString()) + decimal.Parse(row["InAmount"].ToString());

                }
                else
                {
                    r1["料号"] = row["II_Code"];
                    r1["料名"] = row["II_Name"];
                    r1["版本名称"] = row["TRV_VersionName"];
                    r1[DateTime.Parse(row["FCS_ReportTime"].ToString()).Day + "日入库数"] = row["InAmount"];

                    dailyReportTable.Rows.Add(r1);
                    dailyReportTable.AcceptChanges();                  
                }
            }

            datagrid_DailyReportDetail.DataContext = dailyReportTable.DefaultView;
           
        }


        /// <summary>
        /// 由于此报表将每日报工数量作为一列,将每日报工数量绑定到grid需要很多字段属性,所以将List
        /// 先转换为datatable。
        /// </summary>
        private void BingdingView()
        {
            List<FlowCardSubLists> _fcslList = new List<FlowCardSubLists>();
            List<FlowCardSubLists> newfcslList = new List<FlowCardSubLists>();

            fcslList.GroupBy(p => p.FC_Code).ToList().ForEach(p => _fcslList.Add(p.FirstOrDefault(q => q.FCS_IsLastProcess.Equals(true))));

            beforeTable = new DataTable();
            beforeTable.Columns.Add("II_Code",typeof(string));
            beforeTable.Columns.Add("II_Name",typeof(string));
            beforeTable.Columns.Add("TRV_VersionName",typeof(string));
            beforeTable.Columns.Add("FCS_ReportTime",typeof(string));
            beforeTable.Columns.Add("FCS_QulifiedAmount",typeof(Int32));
            beforeTable = MyDBController.ListToDataTable(_fcslList,beforeTable);

            dailyReportTable = new DataTable();
            dailyReportTable.Columns.Add("料号", typeof(string));
            dailyReportTable.Columns.Add("料名", typeof(string));
            dailyReportTable.Columns.Add("版本名称", typeof(string));
            dailyReportTable.Columns.Add("月计划合计数", typeof(decimal));
            dailyReportTable.Columns.Add("累计完成数", typeof(decimal));
            dailyReportTable.Columns.Add("日计划累计数", typeof(decimal));
            dailyReportTable.Columns.Add("日累计差异数", typeof(decimal));
            dailyReportTable.Columns.Add("完成率", typeof(decimal));

            for (int i = 1; i <= dayNum; i++)
            {
                dailyReportTable.Columns.Add(i.ToString() + "日库存数", typeof(decimal));
                dailyReportTable.Columns.Add(i.ToString() + "日入库数", typeof(decimal));
                dailyReportTable.Columns.Add(i.ToString() + "日出库数", typeof(decimal));
            }

            foreach (DataRow row in beforeTable.Rows)
            {
                DataRow r1;
                if (dailyReportTable.Select("料号 = '" + row["II_Code"].ToString() + "' and 版本名称 = '" + row["TRV_VersionName"].ToString() + "'").Length > 0)
                {
                    r1 = dailyReportTable.Select("料号 = '" + row["II_Code"].ToString() + "' and 版本名称 = '" + row["TRV_VersionName"].ToString() + "'")[0];
                    if (r1[DateTime.Parse(row["FCS_ReportTime"].ToString()).Day + "日入库数"] is DBNull)
                    {
                        r1[DateTime.Parse(row["FCS_ReportTime"].ToString()).Day + "日入库数"] = 0.0m;
                    }

                    r1[DateTime.Parse(row["FCS_ReportTime"].ToString()).Day + "日入库数"] = decimal.Parse(r1[DateTime.Parse(row["FCS_ReportTime"].ToString()).Day + "日入库数"].ToString()) + decimal.Parse(row["FCS_QulifiedAmount"].ToString());
                }
                else
                {
                    r1 = dailyReportTable.NewRow();
                    r1["料号"] = row["II_Code"];
                    r1["料名"] = row["II_Name"];
                    r1["版本名称"] = row["TRV_VersionName"];
                    r1[DateTime.Parse(row["FCS_ReportTime"].ToString()).Day + "日入库数"] = row["FCS_QulifiedAmount"];

                    dailyReportTable.Rows.Add(r1);
                    dailyReportTable.AcceptChanges(); 
                }
            }

            datagrid_DailyReportDetail.DataContext = dailyReportTable.DefaultView;
        }



        /// <summary>
        /// 调用事件
        /// </summary>
        public void ShowDeptInfo(string year,string month,string dept_code)
        {
            this.year = year;
            this.month = month;
            this.dept_code = dept_code;
        }

        /// <summary>
        /// 导出功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_export_Click(object sender, RoutedEventArgs e)
        {
            QkRowChangeToColClass.CreateExcelFileForDataTable(dailyReportTable);

            System.Windows.MessageBox.Show("导出成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
