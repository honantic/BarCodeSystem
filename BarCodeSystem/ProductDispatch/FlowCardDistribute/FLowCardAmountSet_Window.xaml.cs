using BarCodeSystem.ProductDispatch.FlowCard;
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
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace BarCodeSystem.ProductDispatch.FlowCardDistribute
{
    /// <summary>
    /// FLowCardAmountSet_Window.xaml 的交互逻辑
    /// </summary>
    public partial class FlowCardAmountSet_Window : Window
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FlowCardAmountSet_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置数量的构造函数
        /// </summary>
        public FlowCardAmountSet_Window(int _cardCount, int _cardAmount, SubmitDistributeFCInfo _sdfci)
        {
            InitializeComponent();
            cardCount = _cardCount;
            cardAmount = _cardAmount;
            sdfci = _sdfci;
        }

        #region 变量
        /// <summary>
        /// w委托函数
        /// </summary>
        SubmitDistributeFCInfo sdfci;
        /// <summary>
        /// 流转卡总数量
        /// </summary>
        int cardCount;

        /// <summary>
        /// 流转卡派工总数
        /// </summary>
        int cardAmount;
        #endregion

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitFirstSetDataSource(cardCount, cardAmount);
        }

        /// <summary>
        /// 初始化第一组下拉框数据源
        /// </summary>
        /// <param name="_firstSetCount"></param>
        /// <param name="_firstSetAmount"></param>
        private void InitFirstSetDataSource(int _cardCount, int _cardAmount)
        {
            int_UD_FirstSetCount.Maximum = _cardCount;
        }


        /// <summary>
        /// 拖动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            MyDBController.FindVisualChild<ComboBox>(this).ForEach(p =>
            {
                if (p.SelectedIndex == -1)
                {
                    flag = false;
                }
            });

            if (flag)
            {
                int firstSetCount = Convert.ToInt32(int_UD_FirstSetCount.Value);
                int firstSetAmount = Convert.ToInt32(int_UD_FirstSetAmount.Value);
                int secSetCount = Convert.ToInt32(int_UD_SecSetCount.Value);
                int secSetAmount = Convert.ToInt32(int_UD_SecSetAmount.Value);
                sdfci.Invoke(firstSetCount, firstSetAmount, secSetCount, secSetAmount);
                this.Close();
            }
        }

        /// <summary>
        /// 第一组流转卡数量改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void int_UD_FirstSetCount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (int_UD_SecSetCount != null)
            {
                int_UD_SecSetCount.Value = cardCount - int_UD_FirstSetCount.Value;
                int_UD_FirstSetAmount.Maximum = cardAmount / int_UD_FirstSetCount.Value;
            }
        }

        /// <summary>
        /// 第一组流转卡派工数量改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void int_UD_FirstSetAmount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (int_UD_SecSetCount != null)
            {
                if (int_UD_SecSetCount.Value != 0)
                {
                    int_UD_SecSetAmount.Maximum = (cardAmount - int_UD_FirstSetCount.Value * int_UD_FirstSetAmount.Value) / int_UD_SecSetCount.Value;
                }
            }
        }
    }
}
