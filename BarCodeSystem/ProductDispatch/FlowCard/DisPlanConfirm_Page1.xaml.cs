using BarCodeSystem.PublicClass.DatabaseEntity;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BarCodeSystem.ProductDispatch.FlowCard
{
    /// <summary>
    /// DisPlanConfirm_Page1.xaml 的交互逻辑
    /// </summary>
    public partial class DisPlanConfirm_Page1 : Page
    {
        public DisPlanConfirm_Page1()
        {
            InitializeComponent();
        }

        public DisPlanConfirm_Page1(List<DisPlanLists> _disPlanList, List<DisPlanVersionLists> _disPlanVersionList, ClearDisPlanList _cdpl)
        {
            InitializeComponent();
            disPlanVersionList = _disPlanVersionList;
            disPlanList = _disPlanList;
            cdpl = _cdpl;
        }

        List<DisPlanVersionLists> disPlanVersionList;
        List<DisPlanLists> disPlanList;
        ClearDisPlanList cdpl;

        /// <summary>
        /// 保存为新的派工方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveAsNew_Click(object sender, RoutedEventArgs e)
        {
            DisPlanConfirm_Page2 dpcp2 = new DisPlanConfirm_Page2(disPlanList, disPlanVersionList, cdpl);
            ((Frame)MyDBController.FindVisualParent<Frame>(this)[0]).Navigate(dpcp2);
        }
        /// <summary>
        /// 覆盖原有的派工方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveAsOld_Click(object sender, RoutedEventArgs e)
        {
            DisPlanConfirm_Page3 dpcp3 = new DisPlanConfirm_Page3(disPlanList, disPlanVersionList, cdpl);
            ((Frame)MyDBController.FindVisualParent<Frame>(this)[0]).Navigate(dpcp3);
        }
    }
}
