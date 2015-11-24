using BarCodeSystem.ProductDispatch.FlowCard;
using BarCodeSystem.PublicClass;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BarCodeSystem.FileQuery.ProduceOrderQuery
{
    /// <summary>
    /// QueryByOrderCode_Page.xaml 的交互逻辑
    /// </summary>
    public partial class QueryByWorkCenter_Page : Page
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public QueryByWorkCenter_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 查询生产订单的构造函数
        /// </summary>
        /// <param name="_spol"></param>
        public QueryByWorkCenter_Page(SubmitProduceOrderList _spol)
        {
            InitializeComponent();
            spol = _spol;
        }

        #region  变量
        /// <summary>
        /// 加载次数 
        /// </summary>
        private int loadCount = 0;

        /// <summary>
        /// 工作中心列表
        /// </summary>
        List<WorkCenterLists> wclList;

        /// <summary>
        /// 生产订单列表
        /// </summary>
        List<ProduceOrderLists> polList = new List<ProduceOrderLists>();

        /// <summary>
        /// 查询生产订单的委托函数
        /// </summary>
        SubmitProduceOrderList spol;
        #endregion
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                btn_Back.Content = "<-退后";
                InitWCGridInfo();
                loadCount++;
            }
        }

        /// <summary>
        /// 根据车间信息，初始化车间信息选择区域信息
        /// </summary>
        private void InitWCGridInfo()
        {
            wclList = WorkCenterLists.FetchWCInfo();
            int count = wclList.Count;
            GridLength gl = new GridLength(Math.Min(grid_WorkCenterInfo.ActualHeight / (count + 1), 60));
            if (count > 0)
            {
                RowDefinition rd = new RowDefinition() { Height = gl };
                Button btn = new Button() { Cursor = Cursors.Hand, Name = "btn_AllWorkCenter", Content = "所有车间", Width = 125, Height = 25 };
                grid_WorkCenterInfo.RowDefinitions.Add(rd);
                btn.SetValue(Button.StyleProperty, Application.Current.FindResource("bd_SelectStyle"));
                btn.Click += OnClick;
                grid_WorkCenterInfo.Children.Add(btn);
                Grid.SetRow(btn, grid_WorkCenterInfo.RowDefinitions.Count - 1);
                foreach (WorkCenterLists item in wclList)
                {
                    rd = new RowDefinition() { Height = gl };
                    grid_WorkCenterInfo.RowDefinitions.Add(rd);
                    btn = new Button() { Cursor = Cursors.Hand, Name = item.department_shortname, Content = item.department_name, Width = 125, Height = 25 };
                    btn.SetValue(Button.StyleProperty, Application.Current.FindResource("bd_SelectStyle"));
                    btn.Click += OnClick;
                    grid_WorkCenterInfo.Children.Add(btn);
                    Grid.SetRow(btn, grid_WorkCenterInfo.RowDefinitions.Count - 1);

                }
                grid_WorkCenterInfo.RowDefinitions.Add(new RowDefinition());
                MyDBController.FindVisualChild<Button>(grid_WorkCenterInfo).ForEach(p =>
                {
                    if (!User_Info.User_Code.Equals("admin"))
                    {
                        if (p.Name.Equals(User_Info.User_Workcenter_ShortName))
                        {
                            p.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            p.Visibility = Visibility.Hidden;
                        }
                    }
                });
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("系统中没有车间信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClick(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            string key = ((Button)e.OriginalSource).Name;
            if (key.Equals("btn_AllWorkCenter"))
            {
                polList = ProduceOrderLists.FetchProduceOrderInfo("", 1);
            }
            else
            {
                polList = ProduceOrderLists.FetchProduceOrderInfo(wclList.Find(p => p.department_shortname.Equals(key)).department_code, 1);
            }
            spol.Invoke(polList);
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 后退
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            MyDBController.FindVisualParent<Frame>(this).ForEach(p =>
            {
                if (p.Name.Equals("frame_QueryWay"))
                {
                    p.GoBack();
                }
            });
            MyDBController.FindVisualParent<ProduceOrderQuery_Page>(this).ForEach(p => p.ClearInfo());
        }
    }
}
