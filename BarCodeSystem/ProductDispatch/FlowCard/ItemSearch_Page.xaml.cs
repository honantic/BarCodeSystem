using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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


    public partial class ItemSearch_Page : Page
    {
        /// <summary>
        /// 委托实例，用来承接构造函数参数
        /// </summary>
        SubmitItemInfo sii;

        public ItemSearch_Page()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 构造函数,带了一个返回类型为String的委托
        /// </summary>
        /// <param name="_sii"></param>
        public ItemSearch_Page(SubmitItemInfo _sii)
        {
            InitializeComponent();
            this.sii = _sii;
        }

        #region 变量
        DataSet ds = new DataSet();
        List<ItemInfoLists> listBeforeSearch = new List<ItemInfoLists>();
        #endregion

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemSearch_Page_Loaded(object sender, RoutedEventArgs e)
        {
            datagrid_ItemInfo.ItemsSource = FetchItemInfo();
        }

        /// <summary>
        /// 获取条码系统中料品信息
        /// </summary>
        private List<ItemInfoLists> FetchItemInfo()
        {
            string SQl = string.Format(@"Select A.[ID],A.[II_Code],A.[II_Name],A.[II_Spec] from [ItemInfo] A where A.[ID] in (select distinct TRV_ItemID from TechRouteVersion)");
            ds.Clear();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "ItemInfo");
            MyDBController.CloseConnection();

            listBeforeSearch = new List<ItemInfoLists>();
            foreach (DataRow row in ds.Tables["ItemInfo"].Rows)
            {
                listBeforeSearch.Add(new ItemInfoLists()
                {
                    ID = Convert.ToInt64(row["ID"]),
                    II_Code = row["II_Code"].ToString(),
                    II_Name = row["II_Name"].ToString(),
                    II_Spec = row["II_Spec"].ToString()
                });
            }
            return listBeforeSearch;
        }

        /// <summary>
        /// 提交按钮，将所选中的料品信息传递到流转卡表头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            #region 调用委托进行传值，返回选中料品信息
            if (datagrid_ItemInfo.SelectedIndex != -1)
            {
                ItemInfoLists iil = (ItemInfoLists)datagrid_ItemInfo.SelectedItem;
                string info = iil.II_Code + " | " + iil.II_Name + " | " + iil.II_Spec;
                sii.Invoke(info);//调用委托,向父级传值
            }
            #endregion

            #region 利用VisualTreeHelper向上寻找父级,传值。这里不用，采用委托方式
            //List<Page> pageList = MyDBController.FindVisualParent<Page>(this);
            //((FlowCard_Page)pageList[0]).txtb_ItemInfo.Text = this.txtb_ItemInfo.Text;
            #endregion
        }

        /// <summary>
        /// 搜索料品信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ItemSearch_Click(object sender, RoutedEventArgs e)
        {
            List<ItemInfoLists> iils = new List<ItemInfoLists>();
            if (string.IsNullOrEmpty(txtb_ItemInfo.Text.Trim()))
            {
                datagrid_ItemInfo.ItemsSource = listBeforeSearch;
            }
            else
            {
                string key =txtb_ItemInfo.Text.Trim();
                iils = listBeforeSearch.FindAll( x=> x.II_Code.IndexOf(key)!= -1||x.II_Name.IndexOf(key) != -1
                            ||x.II_Spec.IndexOf(key) != -1
                    );
                datagrid_ItemInfo.ItemsSource = iils;
            }
        }

        /// <summary>
        /// 文本框回车事件关联
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_ItemInfo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                btn_ItemSearch_Click(sender,e);
            }
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            datagrid_ItemInfo.ItemsSource = FetchItemInfo();
            datagrid_ItemInfo.Items.Refresh();
        }


    }
}
