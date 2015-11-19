using BarCodeSystem.ProductDispatch.FlowCard;
using BarCodeSystem.ProductDispatch.FlowCardReport;
using BarCodeSystem.PublicClass.DatabaseEntity;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace BarCodeSystem.FileQuery.FlowCardQuery
{
    /// <summary>
    /// ModifyWaySelect_Window.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyWaySelect_Window : Window
    {
        public ModifyWaySelect_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选择手动录入或者扫描录入的构造函数
        /// </summary>
        /// <param name="_sqi"></param>
        public ModifyWaySelect_Window(SubmitFlowCardQualitiesList _sfcqList,   List<FlowCardSubLists> _fcsList, ProcessNameLists _pnl)
        {
            InitializeComponent();
            sfcqList = _sfcqList;
            fcsList = _fcsList;
            pnl = _pnl;
        }
        /// <summary>
        /// 修改的工序
        /// </summary>
        ProcessNameLists pnl;

        /// <summary>
        /// 接收报废信息的委托
        /// </summary>
        SubmitFlowCardQualitiesList sfcqList;

        /// <summary>
        /// 保存报废信息的列表
        /// </summary>
        List<FlowCardQualityLists> fcqlList = new List<FlowCardQualityLists>();


        /// <summary>
        /// 保存要删除的报废信息的列表
        /// </summary>
        List<FlowCardQualityLists> fcqlDeleteList = new List<FlowCardQualityLists>();
        /// <summary>
        /// 流转卡行表信息
        /// </summary>
        List<FlowCardSubLists> fcsList;

        /// <summary>
        /// 加载次数
        /// </summary>
        int loadCount = 0;

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                fcqlList = FlowCardQualityLists.FetchFCQLByFCSInfo(fcsList);
                datagrid_AmountInfo.ItemsSource = fcqlList;
                loadCount++;
            }
        }


        /// <summary>
        /// 手动选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectByHand_Click(object sender, RoutedEventArgs e)
        {
            FlowCardQualityIssues_Window fcqi = new FlowCardQualityIssues_Window(AcceptQualityIssues, pnl);
            fcqi.ShowDialog();
        }

        /// <summary>
        /// 接收手动选择的委托函数
        /// </summary>
        /// <param name="qil"></param>
        private void AcceptQualityIssues(QualityIssuesLists qil)
        {
            bool flag = CheckIfExsist(qil);
            if (flag)
            {
                FlowCardQualityLists fcql = new FlowCardQualityLists() { FCQ_QulityIssueID = qil.ID, FCQ_FlowCardSubID = fcsList.FirstOrDefault().ID, QI_Name = qil.QI_Name, QI_Code = qil.QI_Code };
                fcqlList.Add(fcql);
            }
            datagrid_AmountInfo.ItemsSource = fcqlList;
            datagrid_AmountInfo.Items.Refresh();
        }

        /// <summary>
        /// 检查当前的报废原因是否已经包含了新输入的报废原因
        /// </summary>
        /// <param name="_qil"></param>
        /// <returns></returns>
        private bool CheckIfExsist(QualityIssuesLists _qil)
        {
            bool flag = true;
            foreach (FlowCardQualityLists item in fcqlList)
            {
                if (item.FCQ_QulityIssueID == _qil.ID)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        /// <summary>
        /// 扫描选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectByScanning_Click(object sender, RoutedEventArgs e)
        {
            FlowCardQualityIssues_Window fcqi = new FlowCardQualityIssues_Window(AcceptQualityIssuesList, pnl);
            fcqi.ShowDialog();
        }

        /// <summary>
        /// 接收扫描录入的委托函数
        /// </summary>
        /// <param name="qilList"></param>
        private void AcceptQualityIssuesList(List<QualityIssuesLists> qilList)
        {
            qilList.ForEach(
                p =>
                {
                    bool flag = CheckIfExsist(p);
                    if (flag)
                    {
                        FlowCardQualityLists fcql = new FlowCardQualityLists() { FCQ_QulityIssueID = p.ID, FCQ_FlowCardSubID = fcsList.FirstOrDefault().ID, QI_Name = p.QI_Name, QI_Code = p.QI_Code };
                        fcqlList.Add(fcql);
                    }
                });
            datagrid_AmountInfo.ItemsSource = fcqlList;
            datagrid_AmountInfo.Items.Refresh();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            sfcqList.Invoke(fcqlList);
            this.DialogResult = true;
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 拖拽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// 删除报废信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (datagrid_AmountInfo.SelectedIndex != -1)
            {
                fcqlDeleteList.Add((FlowCardQualityLists)datagrid_AmountInfo.SelectedItem);
                string message;
                bool flag = FlowCardQualityLists.DeleteInfo(fcqlDeleteList, out message);
                if (flag)
                {
                    fcqlList.Remove((FlowCardQualityLists)datagrid_AmountInfo.SelectedItem);
                    datagrid_AmountInfo.Items.Refresh();
                    fcqlDeleteList.Clear();
                    Xceed.Wpf.Toolkit.MessageBox.Show(message, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    fcqlDeleteList.Clear();
                    Xceed.Wpf.Toolkit.MessageBox.Show(message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            this.Cursor = Cursors.Arrow;
        }
    }
}
