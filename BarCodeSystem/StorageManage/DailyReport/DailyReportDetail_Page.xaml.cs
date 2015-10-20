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
                GetDailyReportTable();
                CreateNewTale();
                loadcount++;
            }
        }

        /// <summary>
        /// 得到日报表数据
        /// </summary>
        private void GetDailyReportTable()
        {
            //起始时间
            DateTime dtime = Convert.ToDateTime(year + "-" + month + "-01");
            //结束时间
            DateTime dtime2 = dtime.AddMonths(1).AddDays(-1);

            string SQl = @"select " +
                "D.II_Code," +
                "D.II_Name," +
                "C.TRV_VersionName," +
                "CONVERT(date,A.FC_CreateTime) as CreateTime," +
                " sum (B.FCS_QulifiedAmount) as InAmount " +
                " from FlowCard as A " +
                " left join FlowCardSub as B on (A.ID = B.FCS_FlowCradID)" +
                " left join TechRouteVersion as C on(A.FC_ItemTechVersionID = C.ID)" +
                " left join ItemInfo as D on (C.TRV_ItemID = D.ID)" +
                " left join WorkCenter as E on (A.FC_WorkCenter = E.WC_Department_ID) "+
                " where " +
                " CONVERT(date,A.FC_CreateTime) >= '" + dtime.ToString("yyyy/MM/dd HH:MM:ss") + "'" +
                " and CONVERT(date,A.FC_CreateTime) <= '" + dtime2.ToString("yyyy/MM/dd HH:MM:ss") + "'" +
                " and E.WC_Department_Code = '" + dept_code + "'" +
                " group by" +
                " D.II_Code," +
                " D.II_Name," +
                " C.TRV_VersionName," +
                " CONVERT(date,A.FC_CreateTime)" +
                " order by D.II_Code";

            MyDBController.GetConnection();
            beforeTable = MyDBController.GetDataSet(SQl, ds, "beforeTable").Tables["beforeTable"];
            MyDBController.CloseConnection();

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
                    if (r1[DateTime.Parse(row["CreateTime"].ToString()).Day + "日入库数"] is DBNull)
                    {
                        r1[DateTime.Parse(row["CreateTime"].ToString()).Day + "日入库数"] = 0.0m;
                    }

                    r1[DateTime.Parse(row["CreateTime"].ToString()).Day + "日入库数"] = decimal.Parse(r1[DateTime.Parse(row["CreateTime"].ToString()).Day + "日入库数"].ToString()) + decimal.Parse(row["InAmount"].ToString());

                }
                else
                {
                    r1["料号"] = row["II_Code"];
                    r1["料名"] = row["II_Name"];
                    r1["版本名称"] = row["TRV_VersionName"];
                    r1[DateTime.Parse(row["CreateTime"].ToString()).Day + "日入库数"] = row["InAmount"];

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
            QkRowChangeToColClass qk = new QkRowChangeToColClass();
            qk.OutToExcel(dailyReportTable);

            System.Windows.MessageBox.Show("导出成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
