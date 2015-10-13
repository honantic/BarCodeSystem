using BarCodeSystem.ProductDispatch.FlowCard;
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

namespace BarCodeSystem.StorageManage.ScrapReport
{
    /// <summary>
    /// ScrapReport_Page.xaml 的交互逻辑
    /// </summary>
    public partial class ScrapReport_Page : Page
    {
        public ScrapReport_Page()
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
        /// 搜索流转卡号对应的缴废单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FlowCardSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtb_FlowCardSearch.Text))
            {
                textb_SearchHeader.Visibility = Visibility.Visible;
                frame_SearchScrapReport.Content = new ScrapReportSearch_Page(ShowScrapReport);
            }
            else
            {
                Int64 fcID;
                bool flag = CheckForCode(txtb_FlowCardSearch.Text, out fcID);
                if (flag)
                {
                    textb_SearchHeader.Visibility = Visibility.Visible;
                    ScrapReportSearch_Page item = new ScrapReportSearch_Page(ShowScrapReport, fcID, true);
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("您输入的流转卡号有误！\n\r或者，该流转卡对应的完工报告已经审核！\n\r请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// 接收缴费单的委托
        /// </summary>
        /// <param name="srl"></param>
        private void ShowScrapReport(ScrapReportList srl)
        {
            frame_SearchScrapReport.Content = null;
            textb_SearchHeader.Visibility = Visibility.Collapsed;
            txtb_CheckBy.Text = User_Info.User_Name;
            txtb_CheckOn.Text = DateTime.Now.ToString();
            txtb_CreateBy.Text = srl.SR_CreateBy;
            txtb_CreateOn.Text = srl.SR_CreateOn.ToString();
            txtb_FinishTime.Text = srl.SR_EntranceTime.ToString();
            txtb_FlowCardSearch.Text = srl.SR_FlowCardCode;
            txtb_ItemInfo.Text = srl.SR_ItemCode + "|" + srl.SR_ItemName + "|" + srl.SR_ItemSpec;
            txtb_WorkCenter.Text = srl.SR_WorkCenterName;
            integer_ScrapAmount.Value = srl.SR_ScrapAmount;
            integer_StockAmount.Value = srl.SR_ScrapAmount;
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
        /// 检查输入的流转卡号是否有对应的未审核的完工报告
        /// </summary>
        private bool CheckForCode(string _str, out Int64 fcID)
        {
            fcID = -1;
            string SQl = string.Format(@"select count(*) from [ScrapReport] A left join [FlowCard] B on A.[SR_FlowCardID]=B.[ID] where B.[FC_Code]='{0}'", _str);
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
        /// 审核按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Check_Click(object sender, RoutedEventArgs e)
        {
            Xceed.Wpf.Toolkit.MessageBox.Show("此功能尚待开发，需要等待U9接口提供完毕！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }


    }
}
