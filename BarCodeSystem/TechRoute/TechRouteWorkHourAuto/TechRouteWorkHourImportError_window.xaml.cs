using BarCodeSystem.PublicClass.DatabaseEntity;
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
using System.Windows.Shapes;

namespace BarCodeSystem.TechRoute.TechRoute
{
    /// <summary>
    /// TechRouteWorkHourImportError_window.xaml 的交互逻辑
    /// </summary>
    public partial class TechRouteWorkHourImportError_window : Window
    {


        public List<TechRouteImportList> trils;

        public TechRouteWorkHourImportError_window()
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
            txtb_ErrorContent.Text = "";

            foreach (TechRouteImportList item in trils)
            {
                if (item.Error_Remarks != null)
                {
                    txtb_ErrorContent.Text += "第" + item.Line_Number + "行：" + item.Error_Remarks + "\n";
                }

            }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
