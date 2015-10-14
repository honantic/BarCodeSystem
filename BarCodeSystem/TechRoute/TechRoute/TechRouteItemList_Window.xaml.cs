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
        }




        /// <summary>
        /// 获得条码系统中尚没有工艺路线的料品清单
        /// </summary>
        private void GetBCSItemList()
        {
            MyDBController.GetConnection();
            ds.Clear();
            string SQl = @" SELECT [ID],[II_Code],[II_Name] FROM [ItemInfo] WHERE [ID] NOT IN 
                    ( SELECT DISTINCT [TRV_ItemID] FROM TechRouteVersion)";
            dt = MyDBController.GetDataSet(SQl, ds, "item").Tables["item"];
            MyDBController.CloseConnection();

            iils = new List<ItemInfoLists> { };
            int x = dt.Rows.Count;
            for (int i = 0; i < x; i++)
            {
                ItemInfoLists iil = new ItemInfoLists();
                iil.II_Code = dt.Rows[i]["II_Code"].ToString();
                iil.II_Name = dt.Rows[i]["II_Name"].ToString();
                iil.ID = (Int64)dt.Rows[i]["ID"];
                iils.Add(iil);
            }

            listview1.ItemsSource = null;
            listview1.ItemsSource = iils.OrderBy(p => p.II_Code);
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
            Point formPoint = e.GetPosition(this);
            if (formPoint.Y > 60 && formPoint.Y < 90)//表头部分不做响应
            {

            }
            else
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
