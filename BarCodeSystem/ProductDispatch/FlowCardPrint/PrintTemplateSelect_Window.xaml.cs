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
        /// 选择打印模板构造函数
        /// </summary>
        /// <param name="_fcCode"></param>
        public PrintTemplateSelect_Window( string _fcCode)
        {
            InitializeComponent();
            fcCode = _fcCode;
        }

        string fcCode = "";

        /// <summary>
        /// 选定打印模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectTemplate_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (listb_PrintTemplate.SelectedIndex != -1)
            {
                switch (((ListBoxItem)listb_PrintTemplate.SelectedItem).Name)
                {
                    case "_10LinesTemplate":
                        _10LinesFlowCard_Window _10LFC = new _10LinesFlowCard_Window(fcCode);
                        _10LFC.ShowDialog();
                        break;
                    case "_20LinesTemplate":
                        _20LinesFlowCard_Window _20LFC = new _20LinesFlowCard_Window(fcCode);
                        _20LFC.ShowDialog();
                        break;
                    default:
                        break;
                }
            }
            this.Cursor = Cursors.Arrow;
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

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listb_PrintTemplate_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btn_SelectTemplate_Click(null, null);
        }
    }
}
