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

        //传递过来明细表
        List<StaffSalariesList> sumssls;

        //经过筛选的明细表
        List<StaffSalariesList> sslss = new List<StaffSalariesList>();

        string Select_PersonCode = "";

        int loadcount = 0;


        public StaffSalariesDetail_Page()
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
                SalariesInfo();



                decimal sumSalary = 0m;
                sumSalary = sslss.Sum(p => p.Salary);
                           
                tbk_tip.Text = "共有" + sslss.Count + "条数据,共计" + sumSalary + "元";
                loadcount++;
            }

            
        }


        /// <summary>
        /// 绑定人员工资明细数据到datagrid;
        /// </summary>
        private void SalariesInfo()
        {
            try
            {
                datagrid_SalariesInfo.ItemsSource = FetchSalariesInfo2();
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// 根据人员编码得到工资明细表
        /// </summary>
        /// <returns></returns>
        private List<StaffSalariesList> FetchSalariesInfo2()
        {
            try
            {

                sslss.Clear();

                foreach (StaffSalariesList item in sumssls)
                {
                    if (item.PersonCode.Equals(Select_PersonCode))
                    {
                        StaffSalariesList ssl = new StaffSalariesList();

                        ssl.PersonCode = item.PersonCode;
                        ssl.PersonName = item.PersonName;
                        ssl.FC_Code = item.FC_Code;
                        ssl.ProcessName = item.ProcessName;
                        ssl.ReportTime = item.ReportTime;
                        ssl.QulifiedAmount = item.QulifiedAmount;
                        ssl.PeopleAmount = item.PeopleAmount;
                        ssl.WorkHour = item.WorkHour;
                        ssl.Salary = item.Salary;

                        sslss.Add(ssl);
                    }
                }

                return sslss;
            }
            catch (Exception e)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(e.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);

                return null;
            }     
        }



        public void ShowDeepInfo(List<StaffSalariesList> ssls,string Person_Code)
        {
            this.sumssls = ssls;
            this.Select_PersonCode = Person_Code;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_SalariesInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }


        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Export_Click(object sender, RoutedEventArgs e)
        {
            if (sslss.Count > 0)
            {
                DataTable table = MyDBController.ListToDataTable<StaffSalariesList>(sslss);

                table.Columns.Remove("IsSelected");
                table.Columns.Remove("BeginAmount");
                table.Columns.Remove("FC_ID");
                table.Columns.Remove("TR_ID");
                table.Columns.Remove("ProcessID");
                table.Columns.Remove("Department_Name");
                table.Columns.Remove("ScrappedAmount");

                table.Columns["Salary"].SetOrdinal(8);

                table.Columns["PersonName"].ColumnName = "员工名称";
                table.Columns["PersonCode"].ColumnName = "员工编码";
                table.Columns["ProcessName"].ColumnName = "工序";
                table.Columns["FC_Code"].ColumnName = "流转卡编码";
                table.Columns["ReportTime"].ColumnName = "报工时间";
                table.Columns["WorkHour"].ColumnName = "工时";
                table.Columns["QulifiedAmount"].ColumnName = "完工数量";
                table.Columns["PeopleAmount"].ColumnName = "几件人数";
                table.Columns["Salary"].ColumnName = "工资";



                QkRowChangeToColClass qk = new QkRowChangeToColClass();
                qk.OutToExcel(table);

                System.Windows.MessageBox.Show("导出成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }


        }
    }
}
