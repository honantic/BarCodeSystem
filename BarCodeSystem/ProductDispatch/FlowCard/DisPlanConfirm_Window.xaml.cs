using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Animation;

namespace BarCodeSystem.ProductDispatch.FlowCard
{
    /// <summary>
    /// DisPlanConfirm_Window.xaml 的交互逻辑
    /// </summary>
    public partial class DisPlanConfirm_Window : Window
    {
        public DisPlanConfirm_Window()
        {
            InitializeComponent();
        }

        //去除关闭按钮
        //1.Window 类中申明
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        Storyboard storyboard_Big = new Storyboard(), storyboard_Small = new Storyboard();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //去除关闭按钮
            //2.在装载事件中加入
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            this.Top = (Screen.PrimaryScreen.WorkingArea.Height / 2 - frame_Confirm.ActualHeight / 2) / 4;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width / 2 - frame_Confirm.ActualWidth / 2);

            SetAnimation();
        }

        /// <summary>
        /// 完成按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Finish_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// content改变事件？
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frame_Confirm_ContentRendered(object sender, EventArgs e)
        {
            if (frame_Confirm.HasContent)
            {
                if (((Page)frame_Confirm.Content).Title.Equals("覆盖派工方案"))
                {
                    this.Width = 450;
                    storyboard_Big.Begin();
                }
                else
                {
                    this.Width = 350;
                    storyboard_Small.Begin();
                }
            }
        }

        /// <summary>
        /// 动画
        /// </summary>
        private void SetAnimation()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.To = 650;
            da.Duration = TimeSpan.FromSeconds(0.5);
            Storyboard.SetTarget(da, this);
            Storyboard.SetTargetProperty(da, new PropertyPath("Height"));
            storyboard_Big.Children.Add(da);

            da = new DoubleAnimation();
            da.To = 350;
            da.Duration = TimeSpan.FromSeconds(0.5);
            Storyboard.SetTarget(da, this);
            Storyboard.SetTargetProperty(da, new PropertyPath("Height"));
            storyboard_Small.Children.Add(da);
        }
    }
}
