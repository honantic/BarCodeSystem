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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarCodeSystem
{
    /// <summary>
    /// StaffSalariesDetail_Page.xaml 的交互逻辑
    /// </summary>
    public partial class StaffSalariesDetail_Page : Page
    {
        //存储传过来的值
        StaffSalariesList ssl_s = new StaffSalariesList();

        //用于前台展示
        List<StaffSalariesList> ssls = new List<StaffSalariesList>();
        DataSet ds = new DataSet();
        DateTime start_Date;
        DateTime end_Date;
        string workcent_code = "";


        public StaffSalariesDetail_Page()
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
            SalariesInfo();
        }


        private void SalariesInfo()
        {
            try
            {
                datagrid_SalariesInfo.ItemsSource = FetchSalariesInfo();

            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private List<StaffSalariesList> FetchSalariesInfo()
        {

            ds.Clear();
            ssls.Clear();

            string SQl = @"select " +
                "B.FCS_PersonCode, " +
                "B.FCS_PersonName," +
                "A.FC_Code," +
                "B.FCS_ProcessName," +
                "B.FCS_BeginAmount," +
                "B.FCS_QulifiedAmount," +
                "B.FCS_ScrappedAmount," +
                "B.FCS_PieceAmount/B.FCS_PieceDivNum *D.TR_WorkHour as Salary " +
                " from FlowCard as A " +
                " left join FlowCardSub as B on (A.ID = B.FCS_FlowCradID) " +
                " left join WorkCenter as C on (A.FC_WorkCenter = C.WC_Department_ID) " +
                " left join TechRoute as D on (D.ID = B.FCS_TechRouteID) " +
                " where " +
                " A.FC_CreateTime >= '" + start_Date + "'" +
                " and A.FC_CreateTime <='" + end_Date + "'" +
                " and C.WC_Department_Code = '" + workcent_code + "'" +
                " and B.FCS_PersonCode = '" + ssl_s.SS_PersonCode + "'";


            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "SalariesDetail");
            MyDBController.CloseConnection();


            DataRowCollection drc = ds.Tables["SalariesDetail"].Rows;

            try
            {
                foreach (DataRow row in drc)
                {
                    StaffSalariesList ssl = new StaffSalariesList();

                    //wcl.department_code = row["WC_Department_Code"].ToString();
                    //wcl.department_name = row["WC_Department_Name"].ToString();
                    //wcl.department_shortname = row["WC_Department_ShortName"].ToString();

                    ssl.SS_PersonCode = row["FCS_PersonCode"].ToString();
                    ssl.SS_PersonName = row["FCS_PersonName"].ToString();
                    ssl.FC_Code = row["FC_Code"].ToString();
                    ssl.ProcessName = row["FCS_ProcessName"].ToString();
                    ssl.BeginAmount = Int32.Parse(row["FCS_BeginAmount"].ToString());
                    ssl.QulifiedAmount = Int32.Parse(row["FCS_QulifiedAmount"].ToString());
                    ssl.ScrappedAmount = Int32.Parse(row["FCS_ScrappedAmount"].ToString());
                    ssl.SS_Salary = decimal.Parse(row["Salary"].ToString());

                    ssls.Add(ssl);

                }
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }



            return ssls;
        }



        public void ShowDeptInfo(StaffSalariesList ssl, DateTime start_Date, DateTime end_Date, string workcent_code)
        {
            this.ssl_s = ssl;
            this.start_Date = start_Date;
            this.end_Date = end_Date;
            this.workcent_code = workcent_code;
        }
    }
}
