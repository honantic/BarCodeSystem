using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarCodeSystem.ProductDispatch.FlowCard
{
    /// <summary>
    /// ItemSearch_Page.xaml 的交互逻辑
    /// </summary>
    /// 

    ///接受料品查询函数的委托
    public delegate string SubmitItemInfo(string value);

    public partial class ItemSearch_Page : Page
    {
        SubmitItemInfo sii;

        public ItemSearch_Page()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_sii"></param>
        public ItemSearch_Page(SubmitItemInfo _sii)
        {
            InitializeComponent();
            this.sii = _sii;
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemSearch_Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void FetchItemInfo()
        {
            string SQl = string.Format(@"Select ID,II_Code,II_Name,II_Spec from ItemInfo");
        }

        /// <summary>
        /// 提交按钮，将所选中的料品信息传递到流转卡表头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            sii.Invoke("");
           //List<Page> pageList = MyDBController.FindVisualParent<Page>(this);
           //((FlowCard_Page)pageList[0]).txtb_ItemInfo.Text = this.txtb_ItemInfo.Text;
        }


    }
}
