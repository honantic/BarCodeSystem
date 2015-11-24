using BarCodeSystem.PublicClass;
using BarCodeSystem.PublicClass.DatabaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace BarCodeSystem.FileQuery.ProduceOrderQuery
{
    /// <summary>
    /// ProduceOrderQuery_Page.xaml 的交互逻辑
    /// </summary>
    public partial class ProduceOrderQuery_Page : Page
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ProduceOrderQuery_Page()
        {
            InitializeComponent();
        }

        #region 变量
        /// <summary>
        /// 加载次数
        /// </summary>
        private int loadCount = 0;

        /// <summary>
        /// 生产订单列表
        /// </summary>
        List<ProduceOrderLists> polList = new List<ProduceOrderLists>();

        /// <summary>
        /// 流转卡列表
        /// </summary>
        List<FlowCardLists> fclList = new List<FlowCardLists>();


        /// <summary>
        /// 流转卡行表列表
        /// </summary>
        List<FlowCardSubLists> fcslList = new List<FlowCardSubLists>();

        /// <summary>
        /// 报废信息列表
        /// </summary>
        List<FlowCardQualityLists> fcqlList = new List<FlowCardQualityLists>();

        /// <summary>
        /// 当前选中的生产订单信息
        /// </summary>
        ProduceOrderLists _selectedPOL = new ProduceOrderLists();
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
                frame_QueryWaySelect.Navigate(new QueryWaySelect_Page(AcceptProduceOrderInfo));
                if (User_Info.P_Position.Equals("检验员") || User_Info.User_Code.Equals("admin"))
                {
                    SetVisibleProperties(false);
                }
                loadCount++;
            }
        }

        /// <summary>
        /// 接收查询的生产订单的委托函数
        /// </summary>
        /// <param name="polList"></param>
        private void AcceptProduceOrderInfo(List<ProduceOrderLists> _polList)
        {
            this.Cursor = Cursors.Wait;
            polList = _polList;
            dg_POInfo.ItemsSource = polList;
            dg_POInfo.Items.Refresh();
            if (dg_POInfo.Items.Count == 1)
            {
                dg_POInfo.SelectedIndex = 0;
            }
            textb_POCount.Text = string.Format("共有 {0} 条生产订单！", polList.Count);
            this.Cursor = Cursors.Arrow;
        }
        /// <summary>
        /// 快捷搜索生产订单信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_ProduceOrderInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_ProduceOrderSearch_Click(null, null);
            }
        }

        /// <summary>
        /// 搜索生产订单信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ProduceOrderSearch_Click(object sender, RoutedEventArgs e)
        {
            string key = tb_ProduceOrderInfo.Text;
            dg_POInfo.ItemsSource = polList.FindAll(p => p.PO_Code.Contains(key));
            dg_POInfo.Items.Refresh();
            textb_POCount.Text = string.Format("共有 {0} 条生产订单！", dg_POInfo.Items.Count);
        }

        /// <summary>
        /// 生产订单列表双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_POInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (dg_POInfo.SelectedItem != null)
            {
                _selectedPOL = (ProduceOrderLists)dg_POInfo.SelectedItem;
                ShowPOInfo(_selectedPOL);
                FetchFCInfoByPOInfo(_selectedPOL);
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 生产订单列表选中项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_POInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dg_POInfo_MouseDoubleClick(null, null);
        }

        /// <summary>
        /// 展示选中的生产订单相关信息
        /// </summary>
        /// <param name="_pol"></param>
        private void ShowPOInfo(ProduceOrderLists _pol)
        {
            tb_BCSCreateTime.Text = _pol.PO_BCSCreateTime.ToString();
            tb_CreateTime.Text = _pol.PO_CreateTime.ToString();
            tb_DepartmentName.Text = _pol.PO_ProduceDepartName;
            tb_FinishedAmount.Text = _pol.PO_FinishedAmount.ToString();
            tb_ItemInfo.Text = _pol.PO_ItemCode + " | " + _pol.PO_ItemName + " | " + _pol.PO_ItemSpec;
            tb_ItemVersion.Text = _pol.PO_ItemVersion;
            tb_OrderAmount.Text = _pol.PO_OrderAmount.ToString();
            tb_POCode.Text = _pol.PO_Code;
            tb_ProjectCode.Text = _pol.PO_ProjectCode;
            tb_StartAmount.Text = _pol.PO_StartAmount.ToString();
            tb_ProjectName.Text = _pol.PO_ProjectName;
            CB_IsShown.IsChecked = _pol.PO_IsShown;
            tb_Remark.Text = _pol.PO_Remark;
        }

        /// <summary>
        /// 获取选中生产订单的流转卡列表
        /// </summary>
        /// <param name="pol"></param>
        private void FetchFCInfoByPOInfo(ProduceOrderLists pol)
        {
            fclList = FlowCardLists.FetchFC_InfoByOrderCode(pol.PO_Code);
            dg_FlowCardInfo.ItemsSource = fclList;
            dg_FlowCardInfo.Items.Refresh();
            if (dg_FlowCardInfo.Items.Count == 1)
            {
                dg_FlowCardInfo.SelectedIndex = 0;
            }
            else
            {
                dg_FlowCardSubInfo.ItemsSource = null;
            }
            textb_FCCountInfo.Text = string.Format("共有 {0} 条流转卡信息！", fclList.Count);
        }
        /// <summary>
        /// 快捷搜索流转卡信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_FCInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_FCSearch_Click(null, null);
            }
        }
        /// <summary>
        /// 搜索流转卡信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FCSearch_Click(object sender, RoutedEventArgs e)
        {
            string key = tb_FCInfo.Text;
            dg_FlowCardInfo.ItemsSource = fclList.FindAll(p => p.FC_Code.Contains(key));
            dg_FlowCardInfo.Items.Refresh();
            textb_FCCountInfo.Text = string.Format("共有 {0} 条流转卡信息！", dg_FlowCardInfo.Items.Count);
        }

        /// <summary>
        /// 流转卡列表双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_FlowCardInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (dg_FlowCardInfo.SelectedItem != null)
            {
                FetchFCSInfoByFCInfo((FlowCardLists)dg_FlowCardInfo.SelectedItem);
                FetchFCQInfoByFCInfo((FlowCardLists)dg_FlowCardInfo.SelectedItem);
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 流转卡列表选中项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_FlowCardInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dg_FlowCardInfo_MouseDoubleClick(null, null);
        }

        /// <summary>
        /// 根据选中的流转卡，获取行表信息
        /// </summary>
        /// <param name="_fc"></param>
        private void FetchFCSInfoByFCInfo(FlowCardLists _fc)
        {
            fcslList = FlowCardSubLists.FetchFCS_InfoByFC_Id(_fc.ID);
            dg_FlowCardSubInfo.ItemsSource = fcslList;
            ICollectionView view = CollectionViewSource.GetDefaultView(dg_FlowCardSubInfo.ItemsSource);
            view.GroupDescriptions.Add(new PropertyGroupDescription("FCS_ProcessName"));
            dg_FlowCardSubInfo.Items.Refresh();
        }

        /// <summary>
        /// 根据选中的流转卡，获取报废信息
        /// </summary>
        /// <param name="_fc"></param>
        private void FetchFCQInfoByFCInfo(FlowCardLists _fc)
        {
            fcqlList = FlowCardQualityLists.FetchFCQByFlowCardInfo(_fc);
            dg_QualityInfo.ItemsSource = fcqlList;
            dg_QualityInfo.Items.Refresh();
        }

        /// <summary>
        /// 清除显示信息
        /// </summary>
        public void ClearInfo()
        {
            dg_QualityInfo.ItemsSource = null;
            dg_POInfo.ItemsSource = null;
            dg_FlowCardSubInfo.ItemsSource = null;
            dg_FlowCardInfo.ItemsSource = null;
            MyDBController.FindVisualChild<TextBox>(this).ForEach(p => p.Text = "");
            textb_FCCountInfo.Text = "";
            textb_POCount.Text = "";
        }

        /// <summary>
        /// 修改生产订单信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ModifyPO_Click(object sender, RoutedEventArgs e)
        {
            SetVisibleProperties(true);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            _selectedPOL.PO_Remark = tb_Remark.Text;
            _selectedPOL.PO_IsShown = Convert.ToBoolean(CB_IsShown.IsChecked);
            bool flag = ProduceOrderLists.SaveInfo(_selectedPOL);
            if (flag)
            {
                SetVisibleProperties(false);
                Xceed.Wpf.Toolkit.MessageBox.Show("保存成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                _selectedPOL = ProduceOrderLists.FetchProduceOrderInfo(_selectedPOL.PO_Code, 0).FirstOrDefault();
                ShowPOInfo(_selectedPOL);
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("保存失败，请重试!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            SetVisibleProperties(false);
            ShowPOInfo(_selectedPOL);
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 修改生产订单信息的时候的设置可见性、操作性的函数
        /// </summary>
        /// <param name="_isModification"></param>
        private void SetVisibleProperties(bool _isModification)
        {
            CB_IsShown.IsEnabled = _isModification;
            grid_SaveCancel.Visibility = _isModification ? Visibility.Visible : Visibility.Hidden;
            btn_ModifyPO.Visibility = (!_isModification) ? Visibility.Visible : Visibility.Hidden;
            tb_Remark.IsReadOnly = !_isModification;
            tb_Remark.Background = _isModification ? Brushes.White : Brushes.LightGray;
        }
    }
}
