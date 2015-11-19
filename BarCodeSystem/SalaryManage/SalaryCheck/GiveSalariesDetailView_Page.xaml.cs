using BarCodeSystem.FileQuery.GiveSalaries;
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

namespace BarCodeSystem.SalaryManage.SalaryCheck
{
    /// <summary>
    /// GiveSalariesDetailView_Page.xaml 的交互逻辑
    /// </summary>
    public partial class GiveSalariesDetailView_Page : Page
    {
        List<FlowCardSubLists> fcsl = new List<FlowCardSubLists>();
        List<FlowCardSubLists> fcslList = new List<FlowCardSubLists>();
        int loadcount = 0;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public GiveSalariesDetailView_Page()
        {
            InitializeComponent();
        }

        public GiveSalariesDetailView_Page(List<FlowCardSubLists> fcsl)
        {
            InitializeComponent();
            this.fcsl = fcsl;
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
                BingDingList();
                tbk_tip.Text = "共" + fcslList.Count + "行,共计入库数量:" + fcslList.Sum(p => p.FCS_QulifiedAmount).ToString();
                loadcount++;
            }
        }

        /// <summary>
        /// 使用LINQ将数据绑定到datagrid
        /// </summary>
        private void BingDingList()
        {
            fcsl.GroupBy(p => p.FCS_FlowCardID).ToList().ForEach( p => p.Distinct(new ListComparer<FlowCardSubLists>((n1,n2) => n1.FCS_TechRouteID.Equals(n2.FCS_TechRouteID))).ToList().ForEach(item
                => fcslList.Add(item)));
            datagrid_DetailView.ItemsSource = fcslList;
        }


        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Export_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("FC_Code",typeof(string));
            dt.Columns.Add("II_Code",typeof(string));
            dt.Columns.Add("II_Name",typeof(string));
            dt.Columns.Add("II_Spec",typeof(string));
            dt.Columns.Add("FCS_ProcessName",typeof(string));
            dt.Columns.Add("FCS_QulifiedAmount",typeof(int));
            dt.Columns.Add("WH_WorkHour",typeof(decimal));
            dt = MyDBController.ListToDataTable(fcslList,dt);
            dt.Columns["FC_Code"].ColumnName = "流转卡号";
            dt.Columns["II_Code"].ColumnName = "料号";
            dt.Columns["II_Name"].ColumnName = "料名";
            dt.Columns["II_Spec"].ColumnName = "规格";
            dt.Columns["FCS_ProcessName"].ColumnName = "工序名称";
            dt.Columns["FCS_QulifiedAmount"].ColumnName = "完工数量";
            dt.Columns["WH_WorkHour"].ColumnName = "工时";
            QkRowChangeToColClass.CreateExcelFileForDataTable(dt);
        }

        /// <summary>
        /// 返回事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            List<GiveSalariesDetail_Page> gsld = MyDBController.FindVisualParent<GiveSalariesDetail_Page>(this);
            if (gsld.Count > 0)
            {
                gsld[0].frame_Detail.GoBack();
            }
     
        }
    }
}
