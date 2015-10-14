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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarCodeSystem.FileQuery.FlowCardQuery
{
    /// <summary>
    /// FLowCardCheck_Page.xaml 的交互逻辑
    /// </summary>
    public partial class FlowCardCheck_Page : Page
    {
        public FlowCardCheck_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FlowCardCheck_Page(SubmitFlowCard _sfc)
        {
            InitializeComponent();
            sfc = _sfc;
        }

        /// <summary>
        /// 查询流转卡的委托
        /// </summary>
        SubmitFlowCard sfc;

        List<FlowCardSubLists> fcsls;
        FlowCardLists fc;
        TechVersion tv;

        List<FlowCardLists> sourceFCList = new List<FlowCardLists>();
        int loadCount = 0;
        int lastIndex = -1;
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                text_Content.Text = "<-返回";
                txtb_FlowCardCode.Focus();
                List<FlowCardQuery_Page> fcq = MyDBController.FindVisualParent<FlowCardQuery_Page>(this);
                if (fcq != null && fcq.Count > 0)
                {
                    fcq[0].btn_Check.Visibility = Visibility.Visible;
                }
                FetchFlowCardInfo();
                loadCount++;
            }
        }

        /// <summary>
        /// 退回到选择界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            List<QueryWaySelect_Page> qwsl = MyDBController.FindVisualParent<QueryWaySelect_Page>(this);
            if (qwsl.Count > 0)
            {
                qwsl[0].frame_QueryWay.GoBack();
            }
            List<FlowCardQuery_Page> fcql = MyDBController.FindVisualParent<FlowCardQuery_Page>(this);
            if (fcql.Count > 0)
            {
                fcql[0].ClearInfo();
            }

            List<FlowCardQuery_Page> fcq = MyDBController.FindVisualParent<FlowCardQuery_Page>(this);
            if (fcq != null && fcq.Count > 0)
            {
                fcq[0].btn_Check.Visibility = fcq[0].btn_ModifyData.Visibility = Visibility.Hidden;
            }
        }


        /// <summary>
        /// 快捷搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_FlowCardCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_FlowCardSearch_Click(sender, e);
            }
        }

        /// <summary>
        /// 获取流转卡信息
        /// </summary>
        /// <param name="_fcCode"></param>
        public void FetchFlowCardInfo()
        {
             sourceFCList = FlowCardLists.FetchFC_InfoByState(2);
             datagrid_FlowCardList.ItemsSource = sourceFCList.OrderBy(p => p.FC_Code);
        }


        /// <summary>
        /// 选定流转卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Select_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (datagrid_FlowCardList.SelectedIndex != -1 && datagrid_FlowCardList.SelectedIndex != lastIndex)
            {
                fc = (FlowCardLists)datagrid_FlowCardList.SelectedItem;
                tv = TechVersion.FetchTechVersion(fc.FC_ItemTechVersionID, 1);
                fcsls = FlowCardSubLists.FetchFCS_InfoByFC(fc);
                fcsls.Sort(new ListComparer<FlowCardSubLists>((p1, p2) => p1.FCS_ProcessSequanece.CompareTo(p2.FCS_ProcessSequanece)));
                sfc.Invoke(fc, fcsls, tv);
                lastIndex = datagrid_FlowCardList.SelectedIndex;
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 搜索流转卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FlowCardSearch_Click(object sender, RoutedEventArgs e)
        {
            datagrid_FlowCardList.ItemsSource=sourceFCList.FindAll(p=>p.FC_Code.IndexOf(txtb_FlowCardCode.Text)!=-1);
            if (datagrid_FlowCardList.Items.Count == 1)
            {
                datagrid_FlowCardList.SelectedIndex = 0;
                btn_Select_Click(null, null);
            }
            else if (datagrid_FlowCardList.Items.Count == 0)
            {
                MyDBController.FindVisualParent<FlowCardQuery_Page>(this).ForEach(p => p.ClearInfo());
            }
            lastIndex = -1;
        }

        /// <summary>
        /// 双击选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_FlowCardList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btn_Select_Click(sender, e);
        }

        /// <summary>
        /// 选中项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_FlowCardList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            datagrid_FlowCardList_MouseDoubleClick(null, null);
        }
    }
}
