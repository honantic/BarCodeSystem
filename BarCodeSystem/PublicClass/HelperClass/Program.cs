using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xceed.Wpf.Toolkit;

namespace BarCodeSystem
{
    public static class Program
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public static void Main()
        {
            Process instance = RunningInstance();
            if (instance == null)
            {
                BarCodeSystem.App app = new BarCodeSystem.App();
                app.InitializeComponent();
                app.Run();
            }
            else
            {
                HandleRunningInstance(instance);
            }

        }

        /// <summary>
        /// 获取进程
        /// </summary>
        /// <returns></returns>
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);

            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (process.MainModule.FileName == current.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 处理进程
        /// </summary>
        /// <param name="instance"></param>
        public static void HandleRunningInstance(Process instance)
        {
            MyProcess mp = new MyProcess();
            IntPtr x = mp.GetMainWindowHandle(instance.Id);
            Application.EnableVisualStyles();
           
            //确保窗体不是最大化或者最小化
            ShowWindowAsync(x, WS_SHOWNORMAL);

            //将实例放置到前台
            SetForegroundWindow(instance.MainWindowHandle);
        }

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private const int WS_SHOWNORMAL = 3;

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String lpclassName, String lpWindowText);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam);


        /// <summary>
        /// 该函数返回桌面窗口的句柄。桌面窗口覆盖整个屏幕。桌面窗口是一个要在其上绘制所有的图标和其他窗口的区域。
        /// 【说明】获得代表整个屏幕的一个窗口（桌面窗口）句柄.
        /// </summary>
        /// <returns>返回值：函数返回桌面窗口的句柄。</returns>
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// 该函数返回与指定窗口有特定关系（如Z序或所有者）的窗口句柄。
        /// 函数原型：HWND GetWindow（HWND hWnd，UNIT nCmd）；
        /// </summary>
        /// <param name="hWnd">窗口句柄。要获得的窗口句柄是依据nCmd参数值相对于这个窗口的句柄。</param>
        /// <param name="uCmd">说明指定窗口与要获得句柄的窗口之间的关系。该参数值参考GetWindowCmd枚举。</param>
        /// <returns>返回值：如果函数成功，返回值为窗口句柄；如果与指定窗口有特定关系的窗口不存在，则返回值为NULL。
        /// 若想获得更多错误信息，请调用GetLastError函数。
        /// 备注：在循环体中调用函数EnumChildWindow比调用GetWindow函数可靠。调用GetWindow函数实现该任务的应用程序可能会陷入死循环或退回一个已被销毁的窗口句柄。
        /// 速查：Windows NT：3.1以上版本；Windows：95以上版本；Windows CE：1.0以上版本；头文件：winuser.h；库文件：user32.lib。
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCmd uCmd);

        /// <summary>
        /// 窗口与要获得句柄的窗口之间的关系。
        /// </summary>
        enum GetWindowCmd : uint
        {
            /// <summary>
            /// 返回的句柄标识了在Z序最高端的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该句柄标识了在Z序最高端的最高端窗口；
            /// 如果指定窗口是顶层窗口，则该句柄标识了在z序最高端的顶层窗口：
            /// 如果指定窗口是子窗口，则句柄标识了在Z序最高端的同属窗口。
            /// </summary>
            GW_HWNDFIRST = 0,
            /// <summary>
            /// 返回的句柄标识了在z序最低端的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该柄标识了在z序最低端的最高端窗口：
            /// 如果指定窗口是顶层窗口，则该句柄标识了在z序最低端的顶层窗口；
            /// 如果指定窗口是子窗口，则句柄标识了在Z序最低端的同属窗口。
            /// </summary>
            GW_HWNDLAST = 1,
            /// <summary>
            /// 返回的句柄标识了在Z序中指定窗口下的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该句柄标识了在指定窗口下的最高端窗口：
            /// 如果指定窗口是顶层窗口，则该句柄标识了在指定窗口下的顶层窗口；
            /// 如果指定窗口是子窗口，则句柄标识了在指定窗口下的同属窗口。
            /// </summary>
            GW_HWNDNEXT = 2,
            /// <summary>
            /// 返回的句柄标识了在Z序中指定窗口上的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该句柄标识了在指定窗口上的最高端窗口；
            /// 如果指定窗口是顶层窗口，则该句柄标识了在指定窗口上的顶层窗口；
            /// 如果指定窗口是子窗口，则句柄标识了在指定窗口上的同属窗口。
            /// </summary>
            GW_HWNDPREV = 3,
            /// <summary>
            /// 返回的句柄标识了指定窗口的所有者窗口（如果存在）。
            /// GW_OWNER与GW_CHILD不是相对的参数，没有父窗口的含义，如果想得到父窗口请使用GetParent()。
            /// 例如：例如有时对话框的控件的GW_OWNER，是不存在的。
            /// </summary>
            GW_OWNER = 4,
            /// <summary>
            /// 如果指定窗口是父窗口，则获得的是在Tab序顶端的子窗口的句柄，否则为NULL。
            /// 函数仅检查指定父窗口的子窗口，不检查继承窗口。
            /// </summary>
            GW_CHILD = 5,
            /// <summary>
            /// （WindowsNT 5.0）返回的句柄标识了属于指定窗口的处于使能状态弹出式窗口（检索使用第一个由GW_HWNDNEXT 查找到的满足前述条件的窗口）；
            /// 如果无使能窗口，则获得的句柄与指定窗口相同。
            /// </summary>
            GW_ENABLEDPOPUP = 6
        }

        /*GetWindowCmd指定结果窗口与源窗口的关系，它们建立在下述常数基础上：
              GW_CHILD
              寻找源窗口的第一个子窗口
              GW_HWNDFIRST
              为一个源子窗口寻找第一个兄弟（同级）窗口，或寻找第一个顶级窗口
              GW_HWNDLAST
              为一个源子窗口寻找最后一个兄弟（同级）窗口，或寻找最后一个顶级窗口
              GW_HWNDNEXT
              为源窗口寻找下一个兄弟窗口
              GW_HWNDPREV
              为源窗口寻找前一个兄弟窗口
              GW_OWNER
              寻找窗口的所有者
         */
        public const int WM_LBUTTONDBLCLK = 515;
        public const int MK_LBUTTON = 1;
    }




    public class MyProcess
    {
        private bool haveMainWindow = false;
        private IntPtr mainWindowHandle = IntPtr.Zero;
        private int processId = 0;

        public delegate bool EnumThreadWindowsCallback(IntPtr hWnd, IntPtr lParam);

        public IntPtr GetMainWindowHandle(int processId)
        {
            if (!this.haveMainWindow)
            {
                this.mainWindowHandle = IntPtr.Zero;
                this.processId = processId;
                EnumThreadWindowsCallback callback = new EnumThreadWindowsCallback(this.EnumWindowsCallback);
                EnumWindows(callback, IntPtr.Zero);
                GC.KeepAlive(callback);

                this.haveMainWindow = true;
            }
            return this.mainWindowHandle;
        }

        private bool EnumWindowsCallback(IntPtr handle, IntPtr extraParameter)
        {
            int num;
            GetWindowThreadProcessId(new HandleRef(this, handle), out num);
            if ((num == this.processId) && this.IsMainWindow(handle))
            {
                this.mainWindowHandle = handle;
                return false;
            }
            return true;
        }

        private bool IsMainWindow(IntPtr handle)
        {
            return (!(GetWindow(new HandleRef(this, handle), 4) != IntPtr.Zero) && IsWindowVisible(new HandleRef(this, handle)));
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool EnumWindows(EnumThreadWindowsCallback callback, IntPtr extraData);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetWindow(HandleRef hWnd, int uCmd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindowVisible(HandleRef hWnd);
    }
}
