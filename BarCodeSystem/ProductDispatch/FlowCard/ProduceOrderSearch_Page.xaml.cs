using BarCodeSystem.PublicClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarCodeSystem.ProductDispatch.FlowCard
{
    /// <summary>
    /// ProduceOrderSearch_Page.xaml 的交互逻辑
    /// </summary>
    public partial class ProduceOrderSearch_Page : Page
    {

        public ProduceOrderSearch_Page()
        {
            InitializeComponent();
        }

        public ProduceOrderSearch_Page(SubmitProduceOrderInfo _spoi)
        {
            InitializeComponent();
            spoi = _spoi;
        }
        #region 变量
        SubmitProduceOrderInfo spoi;
        List<ProduceOrderLists> pols = new List<ProduceOrderLists>();
        DataSet ds = new DataSet();
        #endregion
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            datagrid_ProduceOrderInfo.ItemsSource = FetchProduceOrderInfo();
        }

        private List<ProduceOrderLists> FetchProduceOrderInfo()
        {
            string SQl = string.Format(@"Select * from [ProduceOrder] where [PO_OrderAmount]>[PO_StartAmount]");//订单数量>开工数量
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "ProduceOrder");
            MyDBController.CloseConnection();

            DataRowCollection drc = ds.Tables["ProduceOrder"].Rows;
            foreach (DataRow row in drc)
            {
                pols.Add(new ProduceOrderLists()
                {
                    ID = Convert.ToInt64(row["ID"]),
                    PO_ID = Convert.ToInt64(row["PO_ID"]),
                    PO_Code = row["PO_Code"].ToString(),
                    PO_ItemID = row["PO_ItemID"].ToString(),
                    PO_ItemCode = row["PO_ItemCode"].ToString(),
                    PO_ItemSpec = row["PO_ItemSpec"].ToString(),
                    PO_ItemVersion = row["PO_ItemVersion"].ToString(),
                    PO_ProjectNum = row["PO_ProjectNum"].ToString(),
                    PO_WorkCenter = Convert.ToInt64(row["PO_WorkCenter"]),
                    PO_ItemName = row["PO_ItemName"].ToString(),
                    PO_Itemunit = row["PO_Itemunit"].ToString(),
                    PO_CreateTime = Convert.ToDateTime(row["PO_CreateTime"]),
                    PO_CreateBy = row["PO_CreateBy"].ToString(),
                    PO_DemandDate = Convert.ToDateTime(row["PO_DemandDate"]),
                    PO_ModifyTime = Convert.ToDateTime(row["PO_ModifyTime"]),
                    PO_ModifyBy = row["PO_ModifyBy"].ToString(),
                    PO_StartDate = Convert.ToDateTime(row["PO_StartDate"]),
                    PO_OrderAmount = Convert.ToInt32(row["PO_OrderAmount"]),
                    PO_StartAmount = Convert.ToInt32(row["PO_StartAmount"]),
                    PO_FinishedAmount = Convert.ToInt32(row["PO_FinishedAmount"]),
                    PO_OrderSource = Convert.ToInt32(row["PO_OrderSource"]),
                    PO_ProduceDepart = row["PO_ProduceDepart"].ToString(),
                    PO_IsReturn = Convert.ToBoolean(row["PO_IsReturn"]),
                });
            }
            return pols;
        }

        /// <summary>
        /// 提交按钮提交选定的生产订单信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            if (spoi != null && datagrid_ProduceOrderInfo.SelectedItem != null)
            {
                spoi.Invoke((ProduceOrderLists)datagrid_ProduceOrderInfo.SelectedItem);
            }
        }



        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            datagrid_ProduceOrderInfo.ItemsSource = FetchProduceOrderInfo();
            datagrid_ProduceOrderInfo.Items.Refresh();
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ProduceOrderSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtb_ProduceOrderInfo.Text))
            {
                string key = txtb_ProduceOrderInfo.Text;
                datagrid_ProduceOrderInfo.ItemsSource = pols.FindAll(p => p.PO_Code.IndexOf(key) != -1 || p.PO_ItemCode.IndexOf(key) != -1 || p.PO_ItemName.IndexOf(key) != -1 || p.PO_ItemSpec.IndexOf(key) != -1 || p.PO_OrderAmount.ToString().IndexOf(key) != -1 || p.PO_StartAmount.ToString().IndexOf(key) != -1);
            }
            else
            {
                datagrid_ProduceOrderInfo.ItemsSource = pols;
            }
        }
        /// <summary>
        /// 回车
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

    }
}
