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
    /// QueryByDate_Page.xaml 的交互逻辑
    /// </summary>
    public partial class QueryByDate_Page : Page
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public QueryByDate_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 查询流转卡的构造函数
        /// </summary>
        /// <param name="_sfc">接收指定流转卡信息的委托函数</param>
        public QueryByDate_Page(SubmitFlowCard _sfc)
        {
            InitializeComponent();
            sfc = _sfc;
        }

        #region 变量
        /// <summary>
        /// 查询流转卡的委托
        /// </summary>
        SubmitFlowCard sfc;

        /// <summary>
        /// 选定的流转卡主表信息
        /// </summary>
        FlowCardLists fc;

        /// <summary>
        /// 选定的流转卡行表信息列表
        /// </summary>
        List<FlowCardSubLists> fcsls;

        /// <summary>
        /// 选定的流转卡使用的工艺路线版本
        /// </summary>
        TechVersion tv;

        /// <summary>
        /// 加载次数
        /// </summary>
        int loadCount = 0;

        /// <summary>
        /// 上次选中的标号
        /// </summary>
        int lastIndex = -1;
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
                text_Content.Text = "<-返回";
                loadCount++;
                combox_QueryCondition.ItemsSource = new List<string>() { "编制日期", "审核日期" };
            }
        }

        /// <summary>
        /// 搜索流转卡信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FlowCardSearch_Click(object sender, RoutedEventArgs e)
        {
            if (dp_StartDate.Value == null || dp_EndDate.Value == null)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("任何日期不能为空！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                datagrid_FlowCardList.ItemsSource = FlowCardLists.FetchFC_InfoByDate(Convert.ToDateTime(dp_StartDate.Value), Convert.ToDateTime(dp_EndDate.Value), combox_QueryCondition.SelectedIndex);
                if (datagrid_FlowCardList.Items.Count == 1)
                {
                    datagrid_FlowCardList.SelectedIndex = 0;
                    btn_Select_Click(null, null);
                }
                else if (datagrid_FlowCardList.Items.Count == 0)
                {
                    MyDBController.FindVisualParent<FlowCardQuery_Page>(this).ForEach(p => p.ClearInfo());
                }
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
