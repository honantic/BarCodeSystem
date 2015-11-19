using BarCodeSystem.FileQuery.InputOutput;
using BarCodeSystem.PublicClass.DatabaseEntity;
using System;
using System.Collections.Generic;
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
using System.Data;

namespace BarCodeSystem.FileQuery.InputOutPut
{
    /// <summary>
    /// StoreReportDetailView_Page.xaml 的交互逻辑
    /// </summary>
    public partial class StoreReportDetailView_Page : Page
    {
        List<FlowCardSubLists> fcsList = new List<FlowCardSubLists>();
        List<FlowCardSubLists> _fcsList = new List<FlowCardSubLists>();
        List<FlowCardSubLists> fcsListNew = new List<FlowCardSubLists>();
        List<TechRouteLists> trList = new List<TechRouteLists>();
        List<FlowCardSubLists> fcsListShow = new List<FlowCardSubLists>();
        int selectSeq;
        int loadcount = 0;


        /// <summary>
        /// 默认构造函数
        /// </summary>
        public StoreReportDetailView_Page()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="fcsList"></param>
        public StoreReportDetailView_Page(List<FlowCardSubLists> fcsList, int selectSeq, List<TechRouteLists> trList)
        {
            InitializeComponent();
            this.fcsList = fcsList;
            this.trList = trList;
            this.selectSeq = selectSeq;
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
                GetTechRouteIsCountingProcess();
                BingDingList();
                txb_Tip.Text = "共" + fcsListShow.Count + "行" + ",共计产出" + fcsListShow.Sum(p => p.FCS_QulifiedAmount);
                loadcount++;
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
                fcsl.TRV_VersionCode = tr.TRV_VersionCode;
                fcsl.TRV_VersionName = tr.TRV_VersionName;
                fcsl.FCS_ProcessSequanece = tr.TR_ProcessSequence;
                fcsl.FCS_ProcessCode = tr.TR_ProcessCode;
                fcsl.FCS_ProcessName = tr.TR_ProcessName;
                fcsListNew.Add(fcsl);
            }
        }

        /// <summary>
        /// 使用LINQ将筛选后的结果绑定到datagrid
        /// </summary>
        public void BingDingList()
        {
            _fcsList = fcsListNew.FindAll(p => p.II_Code.Equals(fcsList.First().II_Code) && p.TRV_VersionCode.Equals(fcsList.First().TRV_VersionCode));
            int nextCountingSeq = _fcsList.FirstOrDefault(p => p.FCS_ProcessSequanece > selectSeq).FCS_ProcessSequanece;
            int preCountingSeq = _fcsList.Last(p => p.FCS_ProcessSequanece <= selectSeq).FCS_ProcessSequanece;
            fcsList.GroupBy(p => p.FC_Code).ToList().ForEach(p =>
                {
                    fcsListShow.Add(p.OrderBy( q => q.FCS_ProcessSequanece).ToList().Last());
                 }
                );
            fcsListShow = fcsListShow.FindAll(p => p.FCS_ProcessSequanece < nextCountingSeq && p.FCS_ProcessSequanece >= preCountingSeq).OrderBy(p => p.FC_Code).ThenBy(p => p.II_Code).ThenBy(p => p.FCS_ProcessSequanece).ToList();
            grid_StoreReportDetailView.ItemsSource = fcsListShow;
        }


        /// <summary>
        /// 导出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_export_Click(object sender, RoutedEventArgs e)
        {
            if (fcsList.Count > 0)
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("FC_Code", typeof(string));
                dt.Columns.Add("II_Code", typeof(string));
                dt.Columns.Add("II_Name", typeof(string));
                dt.Columns.Add("TRV_VersionCode", typeof(string));
                dt.Columns.Add("TRV_VersionName", typeof(string));
                dt.Columns.Add("FCS_ProcessSequanece", typeof(string));
                dt.Columns.Add("FCS_ProcessName", typeof(string));
                dt.Columns.Add("FCS_QulifiedAmount", typeof(string));
                dt.Columns.Add("FCS_IsReported", typeof(string));
                dt = MyDBController.ListToDataTable(fcsList, dt);
                dt.Columns["FC_Code"].ColumnName = "流转卡号";
                dt.Columns["II_Code"].ColumnName = "料号";
                dt.Columns["II_Name"].ColumnName = "料名";
                dt.Columns["TRV_VersionCode"].ColumnName = "版本号";
                dt.Columns["TRV_VersionName"].ColumnName = "版本名称";
                dt.Columns["FCS_ProcessSequanece"].ColumnName = "工序号";
                dt.Columns["FCS_ProcessName"].ColumnName = "工序名称";
                dt.Columns["FCS_QulifiedAmount"].ColumnName = "产出数";
                dt.Columns["FCS_IsReported"].ColumnName = "是否报工";
                QkRowChangeToColClass.CreateExcelFileForDataTable(dt);
            }
        }

        /// <summary>
        /// 返回事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            List<StoreReport_Page> srdp = MyDBController.FindVisualParent<StoreReport_Page>(this);
            if (srdp.Count > 0)
            {
                srdp[0].frame_Search.GoBack();
            }
        }
    }
}
