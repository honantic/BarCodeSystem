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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarCodeSystem.TechRoute.TechRoute
{
    /// <summary>
    /// Interaction logic for TechRoute_Page.xaml
    /// </summary>
    public partial class TechRoute_Page : Page
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TechRoute_Page()
        {
            InitializeComponent();
        }

        #region 变量
        /// <summary>
        /// 车间列表
        /// </summary>
        List<WorkCenterLists> wclList;

        /// <summary>
        /// 鼠标点击时间
        /// </summary>
        DateTime clickTime = DateTime.Now;

        /// <summary>
        /// 加载次数
        /// </summary>
        int loadCount = 0;

        /// <summary>
        /// 当前工艺路线信息页面
        /// </summary>
        TechRouteCurrent_Page trc = new TechRouteCurrent_Page();
        #endregion

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (loadCount == 0)
            {
                SetInitInfo();
                loadCount++;
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 获取系统信息
        /// </summary>
        private void SetInitInfo()
        {
            FetchWCInfo();
            frame_TechInfoFrame.Content = trc;
        }

        /// <summary>
        /// 获取车间信息
        /// </summary>
        private void FetchWCInfo()
        {
            wclList = WorkCenterLists.FetchWCInfo();
            GenerateWCButton(wclList);
        }

        /// <summary>
        /// 根据车间列表动态生成按钮
        /// </summary>
        /// <param name="_wclList"></param>
        private void GenerateWCButton(List<WorkCenterLists> _wclList)
        {
            if (_wclList.Count > 0)
            {
                GridLength gl = new GridLength(Math.Min(grid_WorkCenterInfo.ActualHeight / (_wclList.Count + 1), 60));
                Button btn = new Button() { Name = "btn_AllWorkCenter", Content = "所有车间", Width = 125, Height = 25, Cursor = Cursors.Arrow };
                AppendChild(btn, grid_WorkCenterInfo, gl);
                _wclList.ForEach(p =>
                {
                    btn = new Button() { Name = p.department_shortname, Content = p.department_name, Width = 125, Height = 25 };
                    AppendChild(btn, grid_WorkCenterInfo, gl);
                });
                if (!User_Info.User_Code.Equals("admin"))
                {
                    MyDBController.FindVisualChild<Button>(grid_WorkCenterInfo).ForEach(p =>
                    {
                        if (!p.Name.Equals(User_Info.User_Workcenter_ShortName))
                        {
                            p.Visibility = Visibility.Hidden;
                        }
                    });
                }
            }
        }

        /// <summary>
        /// 将按钮放到指定的grid中
        /// </summary>
        /// <param name="_btn">按钮</param>
        /// <param name="_grid">grid</param>
        /// <param name="_gl">行高</param>
        private void AppendChild(Button _btn, Grid _grid, GridLength _gl)
        {
            RowDefinition rd = new RowDefinition() { Height = _gl };
            _grid.RowDefinitions.Add(rd);
            _grid.Children.Add(_btn);
            _btn.SetValue(Button.StyleProperty, Application.Current.FindResource("bd_SelectStyle"));
            Grid.SetRow(_btn, _grid.RowDefinitions.Count - 1);
            _btn.Click += _btn_Click;
        }

        /// <summary>
        /// 车间按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _btn_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            bool flag = ClickConfirm();
            if (flag)
            {
                if (IsLegalClick(DateTime.Now))
                {
                    if (((Button)e.OriginalSource).Name.Equals("btn_AllWorkCenter"))
                    {
                        trc.FetchItemInfo();
                    }
                    else
                    {
                        string _wcCode = wclList.Find(p => p.department_shortname.Equals(((Button)e.OriginalSource).Name)).department_code;
                        trc.FetchItemInfo(_wcCode);
                    }
                }
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 对按钮点击事件进行检查
        /// </summary>
        /// <returns></returns>
        private bool ClickConfirm()
        {
            bool flag = true;
            try
            {
                if (frame_TechInfoFrame.Content is TechRouteNew_Page)
                {
                    ((TechRouteNew_Page)frame_TechInfoFrame.Content).btn_Cancel_Click(null, null);
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        /// <summary> 
        /// 检查是否快速双击
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private bool IsLegalClick(DateTime dateTime)
        {
            if ((dateTime - clickTime).TotalMilliseconds < System.Windows.Forms.SystemInformation.DoubleClickTime * 2)
            {
                clickTime = dateTime;
                return false;
            }
            else
            {
                clickTime = dateTime;
                return true;
            }
        }
    }
}
