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

        public FlowCardQualityIssues_Window(SubmitQualityIssue _sqi)
        {
            InitializeComponent();
            sqi = _sqi;
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
        public QualityIssuesLists qil;

        private void FetchQualityInfo()
        {
            DataSet ds = new DataSet();
            string SQl = "Select [ID],[QI_Code],[QI_Name],[QI_BarCode] from [QualityIssue]";
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "QualityIssue");
            MyDBController.CloseConnection();

            List<QualityIssuesLists> list = new List<QualityIssuesLists>();
            foreach (DataRow row in ds.Tables["QualityIssue"].Rows)
            {
                list.Add(new QualityIssuesLists()
                {
                    ID = Convert.ToInt64(row["ID"]),
                    QI_Code = row["QI_Code"].ToString(),
                    QI_Name = row["QI_Name"].ToString(),
                    QI_BarCode = row["QI_BarCode"].ToString()
                });
            }
            listview1.ItemsSource = list;
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
                if (Xceed.Wpf.Toolkit.MessageBox.Show("该操作将会对流转卡质量信息做出修改，且不可逆，请确定是否要继续？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    sqi.Invoke((QualityIssuesLists)listview1.SelectedItem);
                    this.DialogResult = true;
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
            this.DialogResult = false;
        }
    }
}
