using BarCodeSystem.ProductDispatch.FlowCard;
using BarCodeSystem.PublicClass;
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

namespace BarCodeSystem.FileQuery.ProduceOrderQuery
{
    /// <summary>
    /// QueryByItemCode_Page.xaml 的交互逻辑
    /// </summary>
    public partial class QueryByItemCode_Page : Page
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public QueryByItemCode_Page()
        {
            InitializeComponent();
        }

        public QueryByItemCode_Page(SubmitProduceOrderList _spol)
        {
            InitializeComponent();
            spol = _spol;
        }

        #region  变量
        /// <summary>
        /// 加载次数 
        /// </summary>
        private int loadCount = 0;

        /// <summary>
        /// 生产订单列表
        /// </summary>
        List<ProduceOrderLists> polList = new List<ProduceOrderLists>();

        /// <summary>
        /// 查询生产订单的委托函数
        /// </summary>
        SubmitProduceOrderList spol;
        #endregion

        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                btn_Back.Content = "<-退后";
                tb_ItemInfo.Focus();
                loadCount++;
            }
        }
        /// <summary>
        /// 退回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {

            MyDBController.FindVisualParent<Frame>(this).ForEach(p =>
            {
                if (p.Name.Equals("frame_QueryWay"))
                {
                    p.GoBack();
                }
            });
            MyDBController.FindVisualParent<ProduceOrderQuery_Page>(this).ForEach(p => p.ClearInfo());
        }

        /// <summary>
        /// 快捷搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_ItemInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_itemSearch_Click(null, null);
            }
        }
        /// <summary>
        /// 根据料号搜索生产订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_itemSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tb_ItemInfo.Text))
            {
                string key = tb_ItemInfo.Text;
                polList = ProduceOrderLists.FetchProduceOrderInfo(key, 2);
                if (polList.Count > 0)
                {
                    spol.Invoke(polList);
                }
            }
        }
    }
}
