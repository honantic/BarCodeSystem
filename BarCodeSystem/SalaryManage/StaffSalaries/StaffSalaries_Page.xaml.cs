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
    /// StaffSalaries_Page.xaml 的交互逻辑
    /// </summary>
    public partial class StaffSalaries_Page : Page
    {
        //展示员工工资信息
        List<StaffSalariesList> ssls = new List<StaffSalariesList>();

        //得到选中部门的信息
        WorkCenterLists wls = new WorkCenterLists();
        DataTable datatable = new DataTable();
        DataTable datatable2 = new DataTable();
        DataSet ds = new DataSet();
        DateTime dtime, dtime2;

        string year;
        string month;


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
            //加载年份
            for (int i = 2015; i < 2100; i++)
            {
                comobox_year.Items.Add(i.ToString());
            }
        }


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
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Query_Click(object sender, RoutedEventArgs e)
        {
            year = comobox_year.Text;
            month = comobox_month.Text;

            dtime = Convert.ToDateTime(year + "-" + month + "-01 00:00:00");//每月第一天
            string dtimestr = MyDBController.HandleTimeInfo(dtime.ToString());

            dtime2 = dtime.AddMonths(1).AddDays(-1).AddSeconds(86399);//每月最后一天最后一秒
            string dtime2str = MyDBController.HandleTimeInfo(dtime2.ToString());


            if (txtb_DeptInfo.Text != "点击放大镜选择" && comobox_year.Text != null && comobox_month.Text != null)
            {
                ssls.Clear();


                QualitySalary(dtimestr, dtime2str);

                MyDBController.GetConnection();
                string SQl = @"select " +
                    "B.FCS_PersonCode," +
                    "B.FCS_PersonName," +
                    "C.WC_Department_Name," +
                    "SUM(B.FCS_PieceAmount/B.FCS_PieceDivNum *D.TR_WorkHour) as Salary " +
                    "from FlowCard as A " +
                    " left join FlowCardSub as B on (A.ID = B.FCS_FlowCradID) " +
                    " left join WorkCenter as C on (A.FC_WorkCenter = C.WC_Department_ID) " +
                    " left join TechRoute as D on (D.ID = B.FCS_TechRouteID) " +
                    " where B.FCS_PieceDivNum <>0  " +
                    " and A.FC_CreateTime >= '" + dtimestr + "'" +
                    " and A.FC_CreateTime <= '" + dtime2str + "'" +
                    " and C.WC_Department_Code = '" + wls.department_code + "'" +
                    " group by B.FCS_PersonCode,B.FCS_PersonName,C.WC_Department_Name";

                datatable = MyDBController.GetDataSet(SQl, ds, "StaffSalaries").Tables["StaffSalaries"];
                MyDBController.CloseConnection();


                DataRowCollection drc = ds.Tables["StaffSalaries"].Rows;

                foreach (DataRow row in drc)
                {
                    if (datatable2.Select("QMS_PersonCode = '" + row["FCS_PersonCode"].ToString() + "'").Length > 0)
                    {
                        DataRow r1 = datatable2.Select("QMS_PersonCode = '" + row["FCS_PersonCode"].ToString() + "'")[0];

                        row["Salary"] = decimal.Parse(row["Salary"].ToString()) + decimal.Parse(r1["QMS_QualityMoney"].ToString());
                    }
                }



                drc = ds.Tables["StaffSalaries"].Rows;

                foreach (DataRow row in drc)
                {
                    StaffSalariesList ssl = new StaffSalariesList();

                    ssl.SS_PersonCode = row["FCS_PersonCode"].ToString();
                    ssl.SS_PersonName = row["FCS_PersonName"].ToString();
                    ssl.SS_Salary = decimal.Parse(row["Salary"].ToString());
                    ssls.Add(ssl);
                }

                datagrid_Salaries.ItemsSource = ssls;
                datagrid_Salaries.Items.Refresh();
            }

        }

        /// <summary>
        /// 明细事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Detail_Click(object sender, RoutedEventArgs e)
        {

            if (datagrid_Salaries.SelectedIndex > -1)
            {
                var x = (StaffSalariesList)datagrid_Salaries.SelectedItem;
                //MyDBController.FindVisualChild<StaffSalariesDetail_Page>(this).FirstOrDefault().ShowDeptInfo(x, dtime, dtime2, wls.department_code);
                StaffSalariesDetail_Page ssd = new StaffSalariesDetail_Page();
                frame_Search.Navigate(ssd);
                ssd.ShowDeptInfo(x, dtime, dtime2, wls.department_code);
            }

        }

        /// <summary>
        /// 部门选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DeptSearch_Click(object sender, RoutedEventArgs e)
        {
            frame_Search.Navigate(new DeptSearch_Page());
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
            if (datagrid_Salaries.SelectedIndex != -1)
            {
                btn_Detail_Click(null, null);
            }
        }

        /// <summary>
        /// 文本框内容改变，自动搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_SearchKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtb_SearchKey.Text.Length > 0)
            {
                string key = txtb_SearchKey.Text;
                datagrid_Salaries.ItemsSource = ssls.FindAll(p => p.SS_PersonCode.IndexOf(key) != -1 || p.SS_PersonName.IndexOf(key) != -1);
            }
            else
            {
                datagrid_Salaries.ItemsSource = ssls;
            }
        }

    }
}
