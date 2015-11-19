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

namespace BarCodeSystem.ProductDispatch.FlowCardReproduce
{
    /// <summary>
    /// Interaction logic for FlowCardReproduce_Page.xaml
    /// </summary>
    public partial class FlowCardReproduce_Page : Page
    {
        public FlowCardReproduce_Page()
        {
            InitializeComponent();
        }
        #region 变量
        /// <summary>
        /// 加载次数
        /// </summary>
        int loadCount = 0;

        /// <summary>
        /// 获取的可供返工的流转卡信息
        /// </summary>
        List<FlowCardLists> fclList = new List<FlowCardLists>();

        /// <summary>
        /// 返工流转卡具体的数据来源
        /// </summary>
        List<FlowCardQualityLists> fcqlList = new List<FlowCardQualityLists>();
        #endregion
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                SetComboboxItems();
                btn_GoBack.Content = "<-后退";
                loadCount++;
            }
        }

        /// <summary>
        /// 初始化下拉框选项
        /// </summary>
        private void SetComboboxItems()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            List<string> yearList = new List<string>();
            List<string> monthList = new List<string>();
            for (int i = -1; i < 3; i++)
            {
                yearList.Add((year + i).ToString() + "年");
            }
            for (int i = 1; i < 13; i++)
            {
                monthList.Add(i.ToString() + "月");
            }
            combobox_Year.ItemsSource = yearList;
            combobox_Month.ItemsSource = monthList;
            combobox_Year.SelectedValue = year.ToString() + "年";
            combobox_Month.SelectedValue = month.ToString() + "月";
        }


        /// <summary>
        /// 返回按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_GoBack_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 计算可返工的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_GetReproduceInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (combobox_Month.SelectedIndex != -1 && combobox_Year.SelectedIndex != -1)
            {
                string year = combobox_Year.SelectedValue.ToString().Replace("年", "");
                string month = combobox_Month.SelectedValue.ToString().Replace("月", "");
                FetchReproduceInfo(year, month);
                frame_Content.Navigate(new ReproduceCalculate_Page(fclList, fcqlList));
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 获取系统中可返工的信息
        /// </summary>
        /// <param name="_year"></param>
        /// <param name="_month"></param>
        private void FetchReproduceInfo(string _year, string _month)
        {
            fclList = FlowCardLists.FetchReproduceFCInfo(_year, _month);
            fcqlList = FlowCardQualityLists.FetchReproduceFCQInfo(_year, _month);
        }
    }
}
