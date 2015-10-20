using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

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
        /// 计时器
        /// </summary>
        Timer t1 = new Timer() { Interval = 500 };

        /// <summary>
        /// 闪烁次数
        /// </summary>
        int tickCount = 0;
        /// <summary>
        /// 省略号列表
        /// </summary>
        List<string> strList = new List<string>();
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
                t1.Tick += t1_Tick;
                strList = new List<string>() { ".", "..", "...", "....", "....." };
                t1.Enabled = true;
            }
        }

        /// <summary>
        /// 闪烁事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void t1_Tick(object sender, System.EventArgs e)
        {
            string key = strList[tickCount % 5];
            textb_Info.Text = Info + key;
            tickCount++;
        }

        /// <summary>
        /// 拖动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                //this.DragMove();
            }
        }


    }
}
