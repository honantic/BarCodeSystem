using BarCodeSystem.PublicClass.DatabaseEntity;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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


        public BadProductListDetail_Page()
        {
            InitializeComponent();
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
                ListBeforeSearch();

                //AddColumns();

                //ShowTable();
                CreatAndShowTable();
                loadCount++;
            }

        }

        /// <summary>
        /// 得到质量原因列表、不良品明细表
        /// </summary>
        private void ListBeforeSearch()
        {
            string SQl = @" select " +
                " distinct B.QI_Code, " +
                " B.QI_Name " +
                //" from FlowCard as A " +
                //" left join FlowCardSub as B on (A.ID = B.FCS_FlowCradID) " +
                //" left join ItemInfo as F on (B.FCS_ItemId = F.ID) " +
                //" left join WorkCenter as C on (A.FC_WorkCenter = C.WC_Department_ID) " +
                //" left join TechRoute as D on (D.ID = B.FCS_TechRouteID) " +
                //" left join FlowCardQuality as E on (E.FCQ_QulityIssueID = B.ID) " +
                //" left join QualityIssue as G on (E.FCQ_QulityIssueID = G.ID) " +
                //" order by G.QI_Code Asc";
                "from FlowCardQuality as A " +
                "left join QualityIssue as B on (A.FCQ_QulityIssueID = B.ID) " +
                "left join FlowCardSub as C on (A.FCQ_FlowCardSubID = C.ID) " +
                "left join FlowCard as D on (C.FCS_FlowCradID = D.ID) " +
                "left join ItemInfo as E on (D.FC_ItemID = E.ID) " +
                " left join WorkCenter as F on (D.FC_WorkCenter = F.WC_Department_ID) " +
                "order by B.QI_Code Asc";


            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "QualityIssue");


            //SQl = @" select " +
            //    "A.FC_CreateTime ," +
            //    "F.II_Code," +
            //    "F.II_Name," +
            //    "F.II_Version ," +
            //    "B.FCS_ProcessName," +
            //    "B.FCS_BeginAmount," +
            //    "B.FCS_QulifiedAmount ," +
            //    "B.FCS_ScrappedAmount," +
            //    //"G.QI_Code," +
            //    "G.QI_Name " +
            //    "from FlowCard as A " +
            //    " left join FlowCardSub as B on (A.ID = B.FCS_FlowCradID) " +
            //    " left join ItemInfo as F on (B.FCS_ItemId = F.ID) " +
            //    " left join WorkCenter as C on (A.FC_WorkCenter = C.WC_Department_ID)  " +
            //    " left join TechRoute as D on (D.ID = B.FCS_TechRouteID) " +
            //    " left join FlowCardQuality as E on (E.FCQ_QulityIssueID = B.ID) " +
            //    " left join QualityIssue as G on (E.FCQ_QulityIssueID = G.ID) " +
            //    " where " +
            //    " C.WC_Department_Code = '" + Dept_Code + "'" +
            //    " and FC_CreateTime >= '" + Start_Date + "'" +
            //    " and FC_CreateTime <= '" + End_Date + "'";

            //if (!string.IsNullOrEmpty(Item_Space))
            //{
            //    SQl += " and F.II_Spec like '%" + Item_Space + "%'";
            //}

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
                "left join FlowCard as D on (C.FCS_FlowCradID = D.ID) " +
                "left join ItemInfo as E on (D.FC_ItemID = E.ID) " +
                "left join WorkCenter as F on (D.FC_WorkCenter = F.WC_Department_ID) " +
                "where " +
                " F.WC_Department_Code = '" + Dept_Code + "'" +
                " and D.FC_CreateTime >= '" + Start_Date + "'" +
                " and D.FC_CreateTime <= '" + End_Date + "'";


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
                if (row["QI_Name"] is DBNull)
                {

                }
                else
                {
                    //datagrid_BadProductListDetail.Columns.Add(new DataGridTextColumn() { Header = row["QI_Name"].ToString(), Binding = new System.Windows.Data.Binding(row["QI_Name"].ToString()) });
                    if (btable.Columns.Contains(row["QI_Name"].ToString()))
                    {
                    }
                    else
                    {
                        btable.Columns.Add(row["QI_Name"].ToString(), typeof(string));
                    }
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
                    //r1["日期"] = row["FC_CreateTime"].ToString().Substring(0,11);
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


        public void ShowDeptInfo(string Dept_Code, string Start_Date, string End_Date, string Item_Space)
        {
            this.Dept_Code = Dept_Code;
            this.Start_Date = Start_Date;
            this.End_Date = End_Date;
            this.Item_Space = Item_Space;
        }


        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void export_btn_Click(object sender, RoutedEventArgs e)
        {
            //QkRowChangeToColClass.CreateExcelFileForDataTable(btable);
            QkRowChangeToColClass qk = new QkRowChangeToColClass();
            qk.OutToExcel(btable);

            System.Windows.MessageBox.Show("导出成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
