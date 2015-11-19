using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.ValueConverters;
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
using System.Windows.Shapes;

namespace BarCodeSystem
{
    /// <summary>
    /// TechRouteExport.xaml 的交互逻辑
    /// </summary>
    public partial class TechRouteExport : Window
    {

        DataTable table = new DataTable();
        DataSet ds1 = new DataSet();

        List<TechRouteImportList> trils = new List<TechRouteImportList> { };
        
        public TechRouteExport()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗口加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //这段代码在正式环境中将被注释掉，测试用
            //MyDBController.Server = User_Info.server[1];
            //MyDBController.Database = User_Info.database[1];
            //MyDBController.Pwd = User_Info.pwd[1];
            //MyDBController.Uid = User_Info.uid[1];

            MyDBController.GetConnection();

            SearchTechRouteInfo();

            MyDBController.CloseConnection();
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Query_Click(object sender, RoutedEventArgs e)
        {

            trils.Clear();
            listview1.ItemsSource = null;

            DataRowCollection  drc = table.Rows;

            ReportWayConverter rwc = new ReportWayConverter();
            TrueOrFalseConverter twc = new TrueOrFalseConverter();

            foreach(DataRow row in drc)
            {
                TechRouteImportList tril = new TechRouteImportList();

                tril.II_Code = row["TR_ItemCode"].ToString();
                tril.II_Name = row["II_Name"].ToString();
                tril.II_Spec = row["II_Spec"].ToString();
                tril.II_Version = row["II_Version"].ToString();
                tril.TRV_VersionCode = row["TRV_VersionCode"].ToString();
                tril.TRV_VersionName = row["TRV_VersionName"].ToString();
                tril.TRV_IsDefaultVer = twc.Convert(row["TRV_IsDefaultVer"], typeof(int), null, new System.Globalization.CultureInfo("")).ToString();
                tril.TRV_ReportWay = row["TRV_ReportWay"].ToString();
                tril.TRV_IsBackVersion = twc.Convert(row["TRV_IsBackVersion"], typeof(int), null, new System.Globalization.CultureInfo("")).ToString();
                tril.TRV_ReportWay = rwc.Convert(row["TRV_ReportWay"], typeof(int), null, new System.Globalization.CultureInfo("")).ToString();
                tril.WC_Department_Code = row["WC_Department_Code"].ToString();
                tril.WC_Department_Name = row["WC_Department_Name"].ToString();
                tril.TR_ProcessSequence = int.Parse(row["TR_ProcessSequence"].ToString());
                tril.TR_ProcessCode = row["TR_ProcessCode"].ToString();
                tril.TR_ProcessName = row["TR_ProcessName"].ToString();
                tril.TR_IsTestProcess = twc.Convert(row["TR_IsTestProcess"], typeof(int), null, new System.Globalization.CultureInfo("")).ToString();
                tril.TR_DefaultCheckPersonName = row["TR_DefaultCheckPersonName"].ToString();
                tril.TR_BindingProcess = int.Parse(row["TR_BindingProcess"].ToString());
                tril.WH_WorkHour = decimal.Parse(row["TR_WorkHour"].ToString());

                trils.Add(tril);

            }

            listview1.ItemsSource = trils;

        }


        /// <summary>
        /// 得到条码系统工艺路线数据
        /// </summary>
        private void SearchTechRouteInfo()
        {
            string SQl = @"select " +
                "A.[TR_ItemCode]," +
                "C.[II_Name]," +
                "C.[II_Spec]," +
               // "C.[II_UnitName]," +
                "C.[II_Version]," +
                "B.[TRV_VersionCode]," +
                "B.[TRV_VersionName]," +
                "B.[TRV_IsDefaultVer]," +
                "B.[TRV_ReportWay]," +
                "B.TRV_IsBackVersion,"+
                "D.WC_Department_Code," +
                "D.[WC_Department_Name]," +
                "A.[TR_ProcessSequence]," +
                "A.[TR_ProcessName]," +
                "A.[TR_ProcessCode]," +
                "A.[TR_IsTestProcess]," +
                "A.[TR_DefaultCheckPersonName]," +
                "A.[TR_BindingProcess]," +
                "A.[TR_WorkHour]" +
                "FROM [TechRoute] A " +
                "LEFT JOIN [TechRouteVersion] B ON A.[TR_ItemID]=B.[TRV_ItemID] AND A.[TR_VersionID]=B.[ID]" +
                "LEFT JOIN [ItemInfo] C ON A.[TR_ItemID]=C.[ID] " +
                "LEFT JOIN [WorkCenter] D ON A.[TR_WorkCenterID]=D.[WC_Department_ID] " +
                "ORDER BY A.[TR_ItemCode]," +
                "B.[TRV_IsDefaultVer] desc," +
                "A.[TR_VersionID]," +
                "A.[TR_ProcessSequence]";

            table = MyDBController.GetDataSet(SQl, ds1, "TechRouteInfo").Tables["TechRouteInfo"];

        }

        /// <summary>
        /// 导出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Export_Click(object sender, RoutedEventArgs e)
        {
            QkRowChangeToColClass.CreateExcelFileForDataTable(table);
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
