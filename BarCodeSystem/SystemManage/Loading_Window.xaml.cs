using System.Windows;

namespace BarCodeSystem.SystemManage
{
    /// <summary>
    /// Loading_Window.xaml 的交互逻辑
    /// </summary>
    public partial class Loading_Window : Window
    {
        public Loading_Window()
        {
            HaveInstance = true;
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_info">要显示的信息</param>
        public Loading_Window(string _info)
        {
            HaveInstance = true;
            Info = _info;
            InitializeComponent();
        }

        /// <summary>
        /// 是否拥有实例
        /// </summary>
        public static bool HaveInstance = false;

        /// <summary>
        /// 显示的信息
        /// </summary>
        private string Info;

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Info))
            {
                textb_Info.Text = Info;
            }
        }
    }
}
