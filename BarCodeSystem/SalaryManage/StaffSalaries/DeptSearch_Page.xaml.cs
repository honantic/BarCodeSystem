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
    /// DeptSearch_Page.xaml 的交互逻辑
    /// </summary>
    public partial class DeptSearch_Page : Page
    {

        List<WorkCenterLists> wcls = new List<WorkCenterLists>();
        DataSet ds = new DataSet();

        public DeptSearch_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 确认事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            var x = (WorkCenterLists)datagrid_DeptInfo.SelectedItem;
             MyDBController.FindVisualParent<StaffSalaries_Page>(this).FirstOrDefault().ShowDeptInfo(x);
        }

        /// <summary>
        /// 刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            datagrid_DeptInfo.ItemsSource = FetchDeptInfo();
            datagrid_DeptInfo.Items.Refresh();
        }

        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DeptInfo();

        }

        private void DeptInfo()
        {
            try
            {
                datagrid_DeptInfo.ItemsSource = FetchDeptInfo();

            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<WorkCenterLists> FetchDeptInfo()
        {
            ds.Clear();

             string SQl = string.Format(@"select WC_Department_Code,WC_Department_Name,WC_Department_ShortName from WorkCenter");

            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "DeptInfo");
            MyDBController.CloseConnection();

            wcls.Clear();

            DataRowCollection drc = ds.Tables["DeptInfo"].Rows;

            try
            {
                foreach (DataRow row in drc)
                {
                    WorkCenterLists wcl = new WorkCenterLists();

                    wcl.department_code = row["WC_Department_Code"].ToString();
                    wcl.department_name = row["WC_Department_Name"].ToString();
                    wcl.department_shortname = row["WC_Department_ShortName"].ToString();

                    wcls.Add(wcl);

                }
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            return wcls;
        }

        /// <summary>
        /// 部门信息搜索功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ProduceOrderSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtb_DeptInfoSearch.Text))
            {
                //datagrid_ProduceOrderInfo.ItemsSource = pols;
                //datagrid_ProduceOrderInfo.Items.Refresh();

                datagrid_DeptInfo.ItemsSource = wcls;
                datagrid_DeptInfo.Items.Refresh();
            }
            else
            {
                //string key = txtb_ProduceOrderInfo.Text;
                //datagrid_ProduceOrderInfo.ItemsSource = pols.FindAll(p => p.PO_Code.IndexOf(key) != -1 || p.PO_ItemCode.IndexOf(key) != -1 || p.PO_ItemName.IndexOf(key) != -1 || p.PO_ItemSpec.IndexOf(key) != -1 || p.PO_OrderAmount.ToString().IndexOf(key) != -1 || p.PO_StartAmount.ToString().IndexOf(key) != -1);
                //datagrid_ProduceOrderInfo.Items.Refresh();

                string key = txtb_DeptInfoSearch.Text;
                datagrid_DeptInfo.ItemsSource = wcls.FindAll( p => p.department_code.IndexOf(key) != -1 || p.department_name.IndexOf(key) != -1 || p.department_shortname.IndexOf(key) != -1);
                datagrid_DeptInfo.Items.Refresh();
            }
        }

        /// <summary>
        /// 回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_ProduceOrderInfo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_ProduceOrderSearch_Click(sender, e);
            }
        }

        /// <summary>
        /// 双击快捷选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_DeptInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (datagrid_DeptInfo.SelectedIndex != -1)
            {
                btn_Submit_Click(null, null);
            }
        }
    }
}
