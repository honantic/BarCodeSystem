using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace BarCodeSystem
{
    /// <summary>
    /// BadProductListDetail_Page.xaml 的交互逻辑
    /// </summary>
    public partial class BadProductListDetail_Page : Page
    {
        //不良品明细显示表
        DataTable btable;

        DataSet ds = new DataSet();
        string Dept_Code;
        string Start_Date;
        string End_Date;
        string Item_Space;
        int loadCount = 0;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BadProductListDetail_Page()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Dept_Code"></param>
        /// <param name="Start_Date"></param>
        /// <param name="End_Date"></param>
        /// <param name="Item_Space"></param>
        public BadProductListDetail_Page(string Dept_Code, string Start_Date, string End_Date, string Item_Space)
        {
            InitializeComponent();
            this.Dept_Code = Dept_Code;
            this.Start_Date = Start_Date;
            this.End_Date = End_Date;
            this.Item_Space = Item_Space;
        }



        /// <summary>
        /// 页面弹出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                try
                {
                    ListBeforeSearch();
                }
                catch (Exception ee)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                CreatAndShowTable();
                loadCount++;
            }

        }

        /// <summary>
        /// 得到质量原因列表、不良品明细表
        /// </summary>
        private void ListBeforeSearch()
        {
            End_Date += " 23:59:59";

            string SQl = @" select " +
                " distinct B.QI_Code, " +
                " B.QI_Name " +
                "from FlowCardQuality as A " +
                "left join QualityIssue as B on (A.FCQ_QulityIssueID = B.ID) " +
                "left join FlowCardSub as C on (A.FCQ_FlowCardSubID = C.ID) " +
                "left join FlowCard as D on (C.FCS_FlowCardID = D.ID) " +
                "left join ItemInfo as E on (D.FC_ItemID = E.ID) " +
                " left join WorkCenter as F on (D.FC_WorkCenter = F.WC_Department_ID) " +
                " where " +
                " F.WC_Department_Code = '" + Dept_Code + "' " +
                " and D.FC_CreateTime >= '" + Start_Date + "' " +
                " and D.FC_CreateTime <= '" + End_Date + "' " +
                " and D.FC_CardState = 5 " +
                " and C.FCS_IsReported = 1 " +
                "order by B.QI_Code Asc";
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "QualityIssue");

            SQl = @" select " +
                "D.FC_CreateTime, " +
                "E.II_Code," +
                "F.WC_Department_Code," +
                "E.II_Name," +
                "E.II_Version," +
                "C.FCS_ProcessName," +
                "C.FCS_BeginAmount," +
                "C.FCS_QulifiedAmount," +
                "A.FCQ_ScrapAmount," +
                "B.QI_Name " +
                "from FlowCardQuality as A " +
                "left join QualityIssue as B on (A.FCQ_QulityIssueID = B.ID) " +
                "left join FlowCardSub as C on (A.FCQ_FlowCardSubID = C.ID) " +
                "left join FlowCard as D on (C.FCS_FlowCardID = D.ID) " +
                "left join ItemInfo as E on (D.FC_ItemID = E.ID) " +
                "left join WorkCenter as F on (D.FC_WorkCenter = F.WC_Department_ID) " +
                "where " +
                " F.WC_Department_Code = '" + Dept_Code + "'" +
                " and D.FC_CreateTime >= '" + Start_Date + "'" +
                " and D.FC_CreateTime <= '" + End_Date + "'" +
                " and D.FC_CardState = 5 " +
                " and C.FCS_IsReported = 1";
            if (!string.IsNullOrEmpty(Item_Space))
            {
                SQl += " and E.II_Version like '%" + Item_Space + "%'";
            }



            MyDBController.GetDataSet(SQl, ds, "table");
            MyDBController.CloseConnection();
        }



        /// <summary>
        /// 新建并展示不良品明细表
        /// </summary>
        private void CreatAndShowTable()
        {
            btable = null;
            btable = new DataTable();
            DataTable table = ds.Tables["table"];
            btable.Columns.Add("日期", typeof(string));
            btable.Columns.Add("料号", typeof(string));
            btable.Columns.Add("料名", typeof(string));
            btable.Columns.Add("型号", typeof(string));
            btable.Columns.Add("工序名称", typeof(string));
            btable.Columns.Add("投入数量", typeof(int));
            btable.Columns.Add("合格数量", typeof(int));


            DataRowCollection drc = ds.Tables["QualityIssue"].Rows;

            foreach (DataRow row in drc)
            {
                if (row["QI_Name"] is DBNull || btable.Columns.Contains(row["QI_Name"].ToString()))
                {

                }
                else
                {
                    btable.Columns.Add(row["QI_Name"].ToString(), typeof(string));
                }
            }

            drc = ds.Tables["table"].Rows;

            foreach (DataRow row in drc)
            {
                DataRow r1 = btable.NewRow();

                if (row["QI_Name"] is DBNull)
                {

                }
                else
                {
                    r1["日期"] = Convert.ToDateTime(row["FC_CreateTime"]).ToShortDateString();
                    r1["料号"] = row["II_Code"].ToString();
                    r1["料名"] = row["II_Name"].ToString();
                    r1["型号"] = row["II_Version"].ToString();
                    r1["工序名称"] = row["FCS_ProcessName"].ToString();
                    r1["投入数量"] = row["FCS_BeginAmount"].ToString();
                    r1["合格数量"] = row["FCS_QulifiedAmount"].ToString();

                    r1[row["QI_Name"].ToString()] = row["FCQ_ScrapAmount"].ToString();


                    btable.Rows.Add(r1);
                    btable.AcceptChanges();
                }

            }


            datagrid_BadProductListDetail.DataContext = btable.DefaultView;

        }



        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void export_btn_Click(object sender, RoutedEventArgs e)
        {
            QkRowChangeToColClass.CreateExcelFileForDataTable(btable);
        }
    }
}
