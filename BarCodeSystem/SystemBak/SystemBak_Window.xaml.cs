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

namespace BarCodeSystem.SystemBak
{
    /// <summary>
    /// SystemBak_Window.xaml 的交互逻辑
    /// </summary>
    public partial class SystemBak_Window : Window
    {
        public SystemBak_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 开始备份
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_BeginBak_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 备份工艺路线
        /// </summary>
        private void BackupTechRoute()
        { }

        /// <summary>
        /// 备份工艺路线版本
        /// </summary>
        private void BackupTechRouteVersion()
        { }

        /// <summary>
        /// 备份流转卡
        /// </summary>
        private void BackupFlowCard()
        { }
    }
}
