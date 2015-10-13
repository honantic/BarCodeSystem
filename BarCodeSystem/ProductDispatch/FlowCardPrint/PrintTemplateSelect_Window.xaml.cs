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

namespace BarCodeSystem.ProductDispatch.FlowCardPrint
{
    /// <summary>
    /// PrintTemplateSelect_Window.xaml 的交互逻辑
    /// </summary>
    public partial class PrintTemplateSelect_Window : Window
    {
        public PrintTemplateSelect_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选定打印模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectTemplate_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// 拖动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_FatherGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
