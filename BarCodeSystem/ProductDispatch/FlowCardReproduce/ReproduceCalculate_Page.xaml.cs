using BarCodeSystem.ProductDispatch.FlowCard;
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

namespace BarCodeSystem.ProductDispatch.FlowCardReproduce
{
    /// <summary>
    /// Interaction logic for ReproduceCalculate_Page.xaml
    /// </summary>
    public partial class ReproduceCalculate_Page : Page
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ReproduceCalculate_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 展示可返工的流转卡信息构造函数
        /// </summary>
        /// <param name="_fclList"></param>
        /// <param name="_fcqList"></param>
        public ReproduceCalculate_Page(List<FlowCardLists> _fclList, List<FlowCardQualityLists> _fcqList)
        {
            InitializeComponent();
            fclList = _fclList;
            fcqList = _fcqList;
        }


        #region 变量
        /// <summary>
        /// 流转卡信息列表
        /// </summary>
        List<FlowCardLists> fclList;
        /// <summary>
        /// 流转卡质量问题信息列表
        /// </summary>
        List<FlowCardQualityLists> fcqList;
        /// <summary>
        /// 加载次数
        /// </summary>
        int loadCount = 0;
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
                DislayInfo(fclList, fcqList);
                loadCount++;
            }
        }

        /// <summary>
        /// 展示可返工的信息
        /// </summary>
        /// <param name="_fclList"></param>
        /// <param name="_fcqList"></param>
        private void DislayInfo(List<FlowCardLists> _fclList, List<FlowCardQualityLists> _fcqList)
        {
            dg_ReproduceFCInfo.ItemsSource = _fcqList;
            ICollectionView view = CollectionViewSource.GetDefaultView(dg_ReproduceFCInfo.ItemsSource);
            view.GroupDescriptions.Add(new PropertyGroupDescription("II_Code"));
            view.GroupDescriptions.Add(new PropertyGroupDescription("QI_Name"));
            GC.Collect();
            view = null;
        }

        /// <summary>
        /// 列表双击，生成返工流转卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_ReproduceFCInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btn_GenerateReproduceFC_Click(null, null);
        }

        /// <summary>
        /// 生成返工流转卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_GenerateReproduceFC_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (dg_ReproduceFCInfo.SelectedIndex != -1)
            {
                FlowCardQualityLists fcql = (FlowCardQualityLists)dg_ReproduceFCInfo.SelectedItem;
                List<FlowCardQualityLists> _temFCQLList = fcqList.FindAll(p => p.II_Code.Equals(fcql.II_Code) && p.QI_Code.Equals(fcql.QI_Code));
                List<FlowCardLists> _tempFCLList = fclList.FindAll(p => _temFCQLList.Exists(item=>item.FC_Code.Equals(p.FC_Code)));
                MyDBController.FindVisualParent<FlowCardReproduce_Page>(this).ForEach(p => p.frame_Content.Navigate(new FlowCard_Page(_tempFCLList, _temFCQLList)));
            }
            this.Cursor = Cursors.Arrow;
        }
    }
}
