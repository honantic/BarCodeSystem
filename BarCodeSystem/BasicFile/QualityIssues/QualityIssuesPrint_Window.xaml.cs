using System;
using System.Collections.Generic;
using System.IO;
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

namespace BarCodeSystem.BasicFile.QualityIssues
{
    /// <summary>
    /// QualityIssuesPrint_Window.xaml 的交互逻辑
    /// </summary>
    public partial class QualityIssuesPrint_Window : Window
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public QualityIssuesPrint_Window()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 默认构造函数
        /// </summary>
        public QualityIssuesPrint_Window(List<QualityIssuesLists> _qilList)
        {
            InitializeComponent();
            qilList = _qilList;
        }

        /// <summary>
        /// 需要打印的质量问题信息列表
        /// </summary>
        List<QualityIssuesLists> qilList;

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateQIBarCode();
        }

        /// <summary>
        /// 生成质量问题信息条码
        /// </summary>
        private void GenerateQIBarCode()
        {
            if (qilList != null)
            {
                qilList.ForEach(
                    p =>
                    {
                        string fileName = User_Info.FetchBarCodeImage(p.QI_Code);
                        ListBoxItem lbi = new ListBoxItem() { Height = 100, Width = 200, Margin = new Thickness(50, 10, 50, 10) };
                        StackPanel sp = new StackPanel() { Orientation = Orientation.Vertical };
                        sp.Children.Add(new GifImageLib.GifImage() { Source = fileName, Height = 60 });
                        sp.Children.Add(new TextBlock() { Text = p.QI_Name });
                        lbi.Content = sp;//new GifImageLib.GifImage() { Source = fileName };
                        listb_PrintTemplate.Items.Add(lbi);
                    });
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Print_Click(object sender, RoutedEventArgs e)
        {
            if (qilList.Count == 0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("没有任何可供打印的内容", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                PrintDialog pd = new PrintDialog();
                if (pd.ShowDialog() == true)
                {
                    pd.PrintVisual(grid_CenterGrid, "质量问题条码");
                }
            }
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
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
