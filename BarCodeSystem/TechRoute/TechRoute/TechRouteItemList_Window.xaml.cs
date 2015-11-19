using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Data;

namespace BarCodeSystem
{
    /// <summary>
    /// TechRouteItemAdd_Window.xaml 的交互逻辑
    /// </summary>
    public partial class TechRouteItemList_Window : Window
    {
        public TechRouteItemList_Window()
        {
            InitializeComponent();
        }

        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        List<ItemInfoLists> iils = new List<ItemInfoLists> { };

        /// <summary>
        /// 选中的料品名称
        /// </summary>
        public string II_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 选中的料品规格
        /// </summary>
        public string II_Spec
        { get; set; }

        /// <summary>
        /// 选中的料品型号
        /// </summary>
        public string II_Version
        { get; set; }

        /// <summary>
        /// 选中的料品编码
        /// </summary>
        public string II_Code
        {
            get;
            set;
        }

        /// <summary>
        /// 选中的料品ID
        /// </summary>
        public Int64 II_ID
        {
            get;
            set;
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ////这段代码在正式环境中将被注释掉，测试用
            //MyDBController.Server = User_Info.server[1];
            //MyDBController.Database = User_Info.database[1];
            //MyDBController.Pwd = User_Info.pwd[1];
            //MyDBController.Uid = User_Info.uid[1];

            GetBCSItemList();
            txtb_SearchKey.Focus();
        }




        /// <summary>
        /// 获得条码系统中尚没有工艺路线的料品清单
        /// </summary>
        private void GetBCSItemList()
        {
            listview1.ItemsSource = iils = ItemInfoLists.FetchItemInfoWithoutTechRoute();
        }


        /// <summary>
        /// 选中一条记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Chose_Click(object sender, RoutedEventArgs e)
        {
            ItemInfoLists iil = listview1.SelectedItem as ItemInfoLists;
            if (iil != null)
            {
                II_Name = iil.II_Name;
                II_Code = iil.II_Code;
                II_Spec = iil.II_Spec;
                II_Version = iil.II_Version;
                II_ID = iil.ID;
                this.DialogResult = true;
            }
        }

        /// <summary>
        /// 双击列表，快捷选中记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listview1.SelectedIndex != -1)
            {
                btn_Chose_Click(sender, e);
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// 搜索料号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WatermarkTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string key = txtb_SearchKey.Text;
                listview1.ItemsSource = iils.Where(p => p.II_Code.Contains(key) || p.II_Name.Contains(key));
            }
            else
            {

            }
        }
    }
}
