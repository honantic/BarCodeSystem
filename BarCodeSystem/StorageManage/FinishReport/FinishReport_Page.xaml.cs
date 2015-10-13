using BarCodeSystem.PublicClass.DatabaseEntity;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BarCodeSystem.StorageManage.FinishReport
{
    /// <summary>
    /// FinishReport_Page.xaml 的交互逻辑
    /// </summary>
    public partial class FinishReport_Page : Page
    {
        public FinishReport_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtb_FlowCardSearch.Focus();
        }

        /// <summary>
        /// 搜索流转卡号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FlowCardSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtb_FlowCardSearch.Text))
            {
                textb_SearchHeader.Visibility = Visibility.Visible;
                frame_SearchFinishReport.Content = new FinishReportSearch_Page(ShowFinishReport);
            }
            else
            {
                Int64 fcID;
                bool flag = CheckForCode(txtb_FlowCardSearch.Text, out fcID);
                if (flag)
                {
                    textb_SearchHeader.Visibility = Visibility.Visible;
                    FinishReportSearch_Page item = new FinishReportSearch_Page(ShowFinishReport, fcID, true);
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("您输入的流转卡号有误！\n\r或者，该流转卡对应的完工报告已经审核！\n\r请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// 展示选取的流转卡信息
        /// </summary>
        /// <param name="frl"></param>
        private void ShowFinishReport(FinishReportList frl)
        {
            textb_SearchHeader.Visibility = Visibility.Collapsed;
            frame_SearchFinishReport.Content = null;
            txtb_FlowCardSearch.Text = frl.FR_FlowCardCode;
            txtb_POCode.Text = frl.FR_ProduceOrderCode;
            txtb_ItemInfo.Text = frl.FR_ItemCode + "|" + frl.FR_ItemName + "|" + frl.FR_ItemSpec;
            txtb_ProjectCode.Text = frl.FR_ProjectCode;
            txtb_WorkCenter.Text = frl.FR_WorkCenterName;
            integer_QualifiedAmount.Value = frl.FR_QualifiedAmount;
            txtb_ScrapAmount.Text = frl.FR_ScrappedAmount.ToString();
            txtb_FinishTime.Text = frl.FR_FinishTime.ToString();
            txtb_CreateOn.Text = frl.FR_CreateTime.ToString();
            txtb_CreateBy.Text = frl.FR_CreateBy;
            txtb_CheckBy.Text = frl.FR_CheckBy;
            txtb_CheckOn.Text = DateTime.Now.ToString();
            integer_StockAmount.Value = frl.FR_StockAmount;
            txtb_CheckBy.Text = User_Info.User_Name;
        }

        /// <summary>
        /// 检查输入的流转卡号是否有对应的未审核的完工报告
        /// </summary>
        private bool CheckForCode(string _str, out Int64 fcID)
        {
            fcID = -1;
            string SQl = string.Format(@"select count(*) from [FinishReport] A left join [FlowCard] B on A.[FR_FlowCardID]=B.[ID] where B.[FC_Code]='{0}'", _str);
            MyDBController.GetConnection();
            if (Convert.ToInt32(MyDBController.ExecuteScalar(SQl)) > 0)
            {
                SQl = string.Format(@"select [ID] from [FlowCard] where [FC_Code]='{0}'", _str);
                try
                {
                    fcID = Convert.ToInt64(MyDBController.ExecuteScalar(SQl));
                }
                catch (Exception)
                {
                }
                MyDBController.CloseConnection();
                return true;
            }
            else
            {
                MyDBController.CloseConnection();
                return false;
            }
        }

        /// <summary>
        /// 快捷搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_FlowCardSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_FlowCardSearch_Click(sender, e);
            }
        }

        /// <summary>
        /// 审核,进行如下操作：
        /// 1、调用U9完工报告接口，将该完工报告信息填入U9系统。获取U9系统中的完工报告编号。
        /// 2、将条码系统中的完工报告设置为已审核，同时将编号设置为U9系统生成的完工报告编号。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Check_Click(object sender, RoutedEventArgs e)
        {
            string SQl = string.Format(@"");
            Xceed.Wpf.Toolkit.MessageBox.Show("此功能尚待开发，需要等待U9接口提供完毕", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
