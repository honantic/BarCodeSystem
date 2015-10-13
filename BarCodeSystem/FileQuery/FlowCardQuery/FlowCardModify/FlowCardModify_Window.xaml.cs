using BarCodeSystem.FileQuery.FlowCardQuery.FlowCardModify;
using BarCodeSystem.ProductDispatch.FlowCard;
using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Xceed.Wpf.Toolkit;

namespace BarCodeSystem.FileQuery.FlowCardQuery
{
    /// <summary>
    /// FlowCardModify_Window.xaml 的交互逻辑
    /// </summary>
    public partial class FlowCardModify_Window : Window
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FlowCardModify_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 修改数据的构造函数
        /// </summary>
        /// <param name="_fc">流转卡信息</param>
        /// <param name="_fcsls">流转卡行表信息</param>
        public FlowCardModify_Window(FlowCardLists _fc, List<FlowCardSubLists> _fcsls, TechVersion _tv, SubmitFlowCard _sfc)
        {
            InitializeComponent();
            fc = _fc;
            fcsls = _fcsls;
            sfc = _sfc;
            tv = _tv;
        }


        #region 变量
        /// <summary>
        /// 流转卡信息
        /// </summary>
        FlowCardLists fc;
        /// <summary>
        /// 流转卡行表信息
        /// </summary>
        List<FlowCardSubLists> fcsls;

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        TechVersion tv;

        /// <summary>
        /// 接收流转卡信息的委托函数
        /// </summary>
        SubmitFlowCard sfc;

        List<FlowCardSubLists> newfcsls = new List<FlowCardSubLists>();
        #endregion

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (fcsls != null && fc != null)
            {
                InitDateInfo();
            }
        }

        /// <summary>
        /// 初始信息
        /// </summary>
        private void InitDateInfo()
        {
            newfcsls = MyDBController.ListCopy(fcsls);
            int track = 0;
            int count = newfcsls.Count;
            List<string> nameList = new List<string>();
            string name = "";
            foreach (FlowCardSubLists item in newfcsls)
            {
                if (track != item.FCS_ProcessSequanece)
                {
                    track = item.FCS_ProcessSequanece;
                    if (string.IsNullOrEmpty(name))
                    {
                        name += item.FCS_PersonName + "、";
                        if (newfcsls.IndexOf(item) == newfcsls.Count - 1)
                        {
                            nameList.Add(name);
                        }
                    }
                    else
                    {
                        nameList.Add(name);
                        name = item.FCS_PersonName + "、";
                    }
                }
                else
                {
                    name += item.FCS_PersonName + "、";
                    if (newfcsls.IndexOf(item) == newfcsls.Count - 1)
                    {
                        nameList.Add(name);
                    }
                }
            }
            List<FlowCardSubLists> tempList = newfcsls.Distinct(new ListComparer<FlowCardSubLists>((x, y) => { return x.FCS_ProcessSequanece.Equals(y.FCS_ProcessSequanece); })).ToList();
            tempList.ForEach(p => p.FCS_PersonName = nameList[tempList.IndexOf(p)]);
            datagrid_FlowCardSub.ItemsSource = tempList;
            datagrid_FlowCardSub.Items.Refresh();
        }


        /// <summary>
        /// 拖动事件
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
            try
            {
                AcceptNewData(newfcsls, fcsls);
                bool flag = FlowCardSubLists.SaveFCSInfo(fcsls);
                if (flag)
                {
                    sfc.Invoke(fc, fcsls, tv);
                    this.DialogResult = true;
                }
            }
            catch (Exception)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("保存失败，请重试！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 保存前将新数据保存到list中
        /// </summary>
        /// <param name="_newList"></param>
        /// <param name="_fcslList"></param>
        private void AcceptNewData(List<FlowCardSubLists> _newList, List<FlowCardSubLists> _fcslList)
        {
            _newList.Sort(new ListComparer<FlowCardSubLists>((p1, p2) => p1.FCS_ProcessSequanece.CompareTo(p2.FCS_ProcessSequanece)));
            _fcslList.Sort(new ListComparer<FlowCardSubLists>((p1, p2) => p1.FCS_ProcessSequanece.CompareTo(p2.FCS_ProcessSequanece)));
            int count = _newList.Count;
            for (int i = 0; i < count; i++)
            {
                _fcslList[i].FCS_QulifiedAmount = _newList[i].FCS_QulifiedAmount;
                _fcslList[i].FCS_ScrappedAmount = _newList[i].FCS_ScrappedAmount;
                _fcslList[i].FCS_AddAmount = _newList[i].FCS_AddAmount;
                _fcslList[i].FCS_BeginAmount = _newList[i].FCS_BeginAmount;
                _fcslList[i].FCS_UnprocessedAm = _newList[i].FCS_UnprocessedAm;
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            InitDateInfo();
        }

        /// <summary>
        /// 报废数量改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void int_ScrappAmount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (datagrid_FlowCardSub.SelectedIndex != -1)
            {
                FlowCardSubLists fcs = (FlowCardSubLists)datagrid_FlowCardSub.SelectedItem;
                if (fcs.FCS_BeginAmount - fcs.FCS_UnprocessedAm - Convert.ToInt32(e.NewValue) + fcs.FCS_AddAmount < 0)//
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("合格数已经小于零！\r\n请重新输入！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Handled = true;
                    ((IntegerUpDown)sender).Value = 0;
                    ((IntegerUpDown)sender).Text = "0";
                }
                else
                {
                    fcs.FCS_QulifiedAmount = fcs.FCS_BeginAmount + fcs.FCS_AddAmount - fcs.FCS_ScrappedAmount - fcs.FCS_UnprocessedAm;
                }
            }
        }

        /// <summary>
        /// 待处理数数量改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void int_UnprocessedAm_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (datagrid_FlowCardSub.SelectedIndex != -1)
            {
                FlowCardSubLists fcs = (FlowCardSubLists)datagrid_FlowCardSub.SelectedItem;
                if (fcs.FCS_IsReported)
                {
                    if (fcs.FCS_BeginAmount - fcs.FCS_ScrappedAmount - Convert.ToInt32(e.NewValue) + fcs.FCS_AddAmount < 0)//
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("合格数已经小于零！\r\n请重新输入！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        e.Handled = true;
                        ((IntegerUpDown)sender).Value = 0;
                        ((IntegerUpDown)sender).Text = "0";
                    }
                    else
                    {
                        fcs.FCS_QulifiedAmount = fcs.FCS_BeginAmount + fcs.FCS_AddAmount - fcs.FCS_ScrappedAmount - fcs.FCS_UnprocessedAm;
                    }
                }
                else
                {
                    e.Handled = true;
                    Xceed.Wpf.Toolkit.MessageBox.Show("改道工序没有报工，不允许修改待处理信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    BarCodeSystem.PublicClass.HelperClass.KeyboardSimulation.Press(Key.Escape);
                    BarCodeSystem.PublicClass.HelperClass.KeyboardSimulation.Release(Key.Escape);
                }
            }
        }


        /// <summary>
        /// 点击修改报废数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ModifyScrapAmount_Click(object sender, RoutedEventArgs e)
        {
            FlowCardSubLists fcs = (FlowCardSubLists)datagrid_FlowCardSub.SelectedItem;
            if (fcs.FCS_IsReported)
            {
                ModifyWaySelect_Window mws = new ModifyWaySelect_Window(_fcs: fcs, _sfcqList: AcceptScrapInfo);
                mws.ShowDialog();
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("改道工序没有报工，不允许修改报废信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                BarCodeSystem.PublicClass.HelperClass.KeyboardSimulation.Press(Key.Escape);
                BarCodeSystem.PublicClass.HelperClass.KeyboardSimulation.Release(Key.Escape);
            }
        }

        /// <summary>
        /// 接收报废信息的委托函数
        /// </summary>
        /// <param name="qilList"></param>
        private void AcceptScrapInfo(List<FlowCardQualityLists> fcqlList)
        {
            bool flag = FlowCardQualityLists.UpdateFCQInfo(fcqlList);
            FlowCardSubLists fcs = (FlowCardSubLists)datagrid_FlowCardSub.SelectedItem;
            if (flag)
            {
                int count = 0;
                fcqlList.ForEach(p => count += p.FCQ_ScrapAmount);
                fcsls.FindAll(p => p.FCS_ProcessSequanece == fcs.FCS_ProcessSequanece).ForEach(p => p.FCS_ScrappedAmount = count);
            }
            InitDateInfo();
        }

        /// <summary>
        /// 应用，将数据改变应用到整个列表中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            ApplyDataChange(newfcsls);
            datagrid_FlowCardSub.Items.Refresh();
        }

        /// <summary>
        /// 将某条工序数据的更改应用到后续的工序中
        /// </summary>
        /// <param name="_list"></param>
        private void ApplyDataChange(List<FlowCardSubLists> _list)
        {
            var sequenceList = _list.Select(p => p.FCS_ProcessSequanece).Distinct();
            for (int i = 0; i < _list.Count; i++)
            {
                for (int j = 0; j < sequenceList.Count(); j++)
                {
                    if (_list[i].FCS_ProcessSequanece == sequenceList.ElementAt(j))
                    {
                        if (i == 0)
                        {

                        }
                        else
                        {
                            if (_list[i].FCS_ProcessSequanece != _list[i - 1].FCS_ProcessSequanece)
                            {
                                _list[i].FCS_BeginAmount = _list[i - 1].FCS_QulifiedAmount + _list[i - 1].FCS_AddAmount;
                            }
                            else
                            {
                                _list[i].FCS_BeginAmount = _list[i - 1].FCS_BeginAmount;
                                _list[i].FCS_ScrappedAmount = _list[i - 1].FCS_ScrappedAmount;
                                _list[i].FCS_UnprocessedAm = _list[i - 1].FCS_UnprocessedAm;
                            }
                        }
                        _list[i].FCS_QulifiedAmount = _list[i].FCS_BeginAmount - _list[i].FCS_ScrappedAmount - _list[i].FCS_UnprocessedAm;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 修改人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ModifyPerson_Click(object sender, RoutedEventArgs e)
        {
            FCPersonModify_Window fcpm = new FCPersonModify_Window(fc, fcsls.FindAll(p => p.FCS_ProcessSequanece.Equals(((FlowCardSubLists)datagrid_FlowCardSub.SelectedItem).FCS_ProcessSequanece)), RecieveNewPersonInfo);
            fcpm.ShowDialog();
        }

        /// <summary>
        /// 接收修改后的行表信息的委托函数
        /// </summary>
        /// <param name="_fcslList"></param>
        private void RecieveNewPersonInfo(List<FlowCardSubLists> _fcslList)
        {
            fcsls = _fcslList;
            InitDateInfo();
        }
    }
}
