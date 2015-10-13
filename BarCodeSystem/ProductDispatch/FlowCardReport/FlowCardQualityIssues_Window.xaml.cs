using BarCodeSystem.ProductDispatch.FlowCard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace BarCodeSystem.ProductDispatch.FlowCardReport
{
    /// <summary>
    /// FlowCardQualityIssues_Page.xaml 的交互逻辑
    /// </summary>
    public partial class FlowCardQualityIssues_Window : Window
    {
        public FlowCardQualityIssues_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 报工手动选择质量信息
        /// </summary>
        /// <param name="_sqi"></param>
        public FlowCardQualityIssues_Window(SubmitQualityIssue _sqi)
        {
            InitializeComponent();
            sqi = _sqi;
        }

        /// <summary>
        /// 报工扫描质量信息
        /// </summary>
        public FlowCardQualityIssues_Window(SubmitQualityIssueList _sqil)
        {
            InitializeComponent();
            sqil = _sqil;
        }
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FetchQualityInfo();
        }

        SubmitQualityIssue sqi;
        SubmitQualityIssueList sqil;
        public QualityIssuesLists qil;
        List<QualityIssuesLists> list1, list2 = new List<QualityIssuesLists>();

        /// <summary>
        /// 获取质量信息
        /// </summary>
        private void FetchQualityInfo()
        {
            list1 = QualityIssuesLists.FetchBCSQualityIssueInfo(User_Info.User_Workcenter_ID);
            listview1.ItemsSource = list1;
            if (sqi != null)
            {
                grid_Two.Visibility = Visibility.Collapsed;
            }
            else
            {
                grid_One.Visibility = Visibility.Collapsed;
                txtb_QualityIssueCode.Focus();
            }
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Point formPoint = e.GetPosition(listview1);
            if (formPoint.Y > 0 && formPoint.Y < 20)//表头部分不做响应
            {

            }
            else
            {
                btn_Select_Click(sender, e);
            }
        }

        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Select_Click(object sender, RoutedEventArgs e)
        {
            if (listview1.SelectedIndex != -1)
            {
                sqi.Invoke((QualityIssuesLists)listview1.SelectedItem);
                this.DialogResult = true;
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// 拖拽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Ensure_Click(object sender, RoutedEventArgs e)
        {
            if (list2.Count > 0)
            {
                sqil.Invoke(list2);
                this.DialogResult = true;
            }
        }

        /// <summary>
        /// 扫描条码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Improt_Click(object sender, RoutedEventArgs e)
        {
            bool flag = CheckForCode(txtb_QualityIssueCode.Text);
            if (flag)
            {
                AddQualityInfo(txtb_QualityIssueCode.Text);
                txtb_QualityIssueCode.Text = "";
            }
        }

        /// <summary>
        /// 保存扫描的信息
        /// </summary>
        /// <param name="_code"></param>
        private void AddQualityInfo(string _code)
        {
            if (list2.Count == 0)
            {
                list2.Add(list1.Find(p => p.QI_Code.Equals(_code)));
            }
            else
            {
                if (list2.Exists(p => p.QI_Code.Equals(_code)))
                {

                }
                else
                {
                    list2.Add(list1.Find(p => p.QI_Code.Equals(_code)));
                }
            }
            listview2.ItemsSource = list2;
            listview2.Items.Refresh();
        }

        /// <summary>
        /// 检查扫描录入的质量信息条码
        /// </summary>
        /// <param name="_code"></param>
        /// <returns></returns>
        private bool CheckForCode(string _code)
        {
            bool flag = false;
            if (list1.Exists(p => p.QI_Code.Equals(_code)))
            {
                flag = true;
            }
            else
                Xceed.Wpf.Toolkit.MessageBox.Show("该质量条码不存在！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            return flag;
        }

        /// <summary>
        /// 快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_QualityIssueCode_KeyDown(object sender, KeyEventArgs e)
        {

        }

        /// <summary>
        /// 回车录入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_QualityIssueCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_Improt_Click(sender, e);
            }
        }
    }
}
