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
    /// QueryByPOCode_Page.xaml 的交互逻辑
    /// </summary>
    public partial class QueryByPOCode_Page : Page
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public QueryByPOCode_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 查询生产订单的构造函数
        /// </summary>
        /// <param name="_spol"></param>
        public QueryByPOCode_Page(SubmitProduceOrderList _spol)
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
                tb_POInfo.Focus();
                loadCount++;
            }
        }

        /// <summary>
        /// 根据订单号搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_POSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tb_POInfo.Text))
            {
                string key = tb_POInfo.Text;
                polList = ProduceOrderLists.FetchProduceOrderInfo(key, 0);
                if (polList.Count > 0)
                {
                    spol.Invoke(polList);
                }
            }
        }

        /// <summary>
        /// 快捷搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_POInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_POSearch_Click(null, null);
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

    }
}
