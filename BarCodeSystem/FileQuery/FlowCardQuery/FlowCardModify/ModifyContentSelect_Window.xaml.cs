using BarCodeSystem.ProductDispatch.FlowCard;
using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.ValueConverters;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BarCodeSystem.FileQuery.FlowCardQuery
{
    /// <summary>
    /// ModifyContentSelect_Window.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyContentSelect_Window : Window
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ModifyContentSelect_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选择修改内容的构造函数
        /// </summary>
        /// <param name="_fc">流转卡信息</param>
        /// <param name="_fcsls">流转卡行表信息</param>
        /// <param name="_sfcs">展示信息的委托函数</param>
        public ModifyContentSelect_Window(FlowCardLists _fc, List<FlowCardSubLists> _fcsls, SubmitFlowCardSub _sfcs, SubmitFlowCard _sfc, TechVersion _tv)
        {
            InitializeComponent();
            fc = _fc;
            fcsls = _fcsls;
            sfcs = _sfcs;
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
        /// 接收流转卡行表信息的委托函数,修改报工信息用的
        /// </summary>
        SubmitFlowCardSub sfcs;

        /// <summary>
        /// 接收流转卡信息的委托函数,修改派工数量用的
        /// </summary>
        SubmitFlowCard sfc;

        /// <summary>
        /// 工艺版本
        /// </summary>
        TechVersion tv;

        #endregion

        /// <summary>
        /// 加载函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

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
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 选择修改派工数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ModifyFCAmount_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            bool flag = CheckFCState(fc);
            if (flag)
            {
                grid_ContentSelectGrid.Visibility = Visibility.Hidden;
                grid_FCAmountModifyGrid.Visibility = Visibility.Visible;
                txtb_OriginFCAmount.Text = fc.FC_Amount.ToString();
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 检查当前的流转卡是否处于能够被修改派工数量的状态
        /// </summary>
        /// <param name="_fc"></param>
        /// <returns></returns>
        private bool CheckFCState(FlowCardLists _fc)
        {
            bool flag = true;
            string messag = (new FlowCardStateConverter()).Convert(_fc.FC_CardState, typeof(string), null, new System.Globalization.CultureInfo("")).ToString();
            if (messag.Equals("开工"))
            {

            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("当前流转卡状态为:" + messag + "！\r\n要重新设置派工数量请先清卡！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                flag = false;
            }
            return flag;
        }
        /// <summary>
        /// 选择修改报工信息
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ModifyReportInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (fc != null && fcsls != null)
            {
                FlowCardModify_Window fm = new FlowCardModify_Window(fc, fcsls, tv, sfc);
                fm.ShowDialog();
                if (fm.DialogResult == true)
                {
                    this.Close();
                }
                else
                {
                }
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (iud_NewFCAmount.Value == null)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请设置新的数量！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (Convert.ToInt32(iud_NewFCAmount.Value).Equals(fc.FC_Amount))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("新的数量和原来的数量一样！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                FlowCardLists _newFC = MyDBController.ValueCopy<FlowCardLists>(fc);
                _newFC.FC_Amount = Convert.ToInt32(iud_NewFCAmount.Value);
                int diff = _newFC.FC_Amount - fc.FC_Amount;
                bool flag = FlowCardLists.UpdateFCAmount(_newFC, fc);
                if (flag)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("保存成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    fcsls.ForEach(p => p.FCS_BeginAmount += diff);
                    sfc.Invoke(_newFC, fcsls, tv);
                    this.Close();
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("保存失败，请重试！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            this.Cursor = Cursors.Arrow;
        }



        /// <summary>
        /// 放弃修改派工数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CancelMofidy_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            grid_FCAmountModifyGrid.Visibility = Visibility.Hidden;
            grid_ContentSelectGrid.Visibility = Visibility.Visible;
            this.Cursor = Cursors.Arrow;
        }
    }
}
