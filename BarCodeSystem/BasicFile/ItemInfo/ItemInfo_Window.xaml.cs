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
using System.Windows.Shapes;
using System.Data;

namespace BarCodeSystem
{
    /// <summary>
    /// ItemInfo_Window.xaml 的交互逻辑
    /// </summary>
    public partial class ItemInfo_Window : Window
    {
        public ItemInfo_Window()
        {
            InitializeComponent();
        }

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        List<ItemInfoLists> listBeforeSearch = new List<ItemInfoLists> { };
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //这段代码在正式环境中将被注释掉，测试用
            MyDBController.Server = User_Info.server[1];
            MyDBController.Database = User_Info.database[1];
            MyDBController.Pwd = User_Info.pwd[1];
            MyDBController.Uid = User_Info.uid[1];

            GetBCSItemInfoList();
        }


        /// <summary>
        /// 获得条码系统料品清单
        /// </summary>
        private void GetBCSItemInfoList()
        {
            MyDBController.GetConnection();

            string SQl = @"SELECT [ID],[II_Code],[II_Spec],[II_Version],[II_Name],[II_UnitID],[II_UnitCode],[II_UnitName]
                            FROM [ItemInfo]";
            dt = MyDBController.GetDataSet(SQl, ds, "ItemInfo").Tables["ItemInfo"];
            MyDBController.CloseConnection();
            listBeforeSearch.Clear();
            int x = dt.Rows.Count;
            for (int i = 0; i < x; i++)
            {
                ItemInfoLists iil = new ItemInfoLists();
                iil.II_Code = dt.Rows[i]["II_Code"].ToString();
                iil.II_Spec = dt.Rows[i]["II_Spec"].ToString();
                iil.II_Version = dt.Rows[i]["II_Version"].ToString();
                iil.II_Name = dt.Rows[i]["II_Name"].ToString();
                iil.II_UnitID = (Int64)dt.Rows[i]["II_UnitID"];
                iil.II_UnitCode = dt.Rows[i]["II_UnitCode"].ToString();
                iil.II_UnitName = dt.Rows[i]["II_UnitName"].ToString();
                listBeforeSearch.Add(iil);
            }
            listview1.ItemsSource = listBeforeSearch;
        }

        /// <summary>
        /// 导入U9料品清单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            ItemInfoImport_Window iii = new ItemInfoImport_Window();
            iii.Height = Math.Min(User_Info.ScreenHeight, 600);
            iii.Width = Math.Min(User_Info.ScreenWidth, 600);
            iii.existItem = dt;
            iii.ShowDialog();
            if ((bool)iii.DialogResult)
            {
                this.GetBCSItemInfoList();
            }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 搜索按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            listview1.ItemsSource = null;
            listview1.ItemsSource = listBeforeSearch;
            if (txtb_SearchKey.Text.Length >0)
            {
                string key = txtb_SearchKey.Text;
                List<ItemInfoLists> iils = new List<ItemInfoLists> { };
                foreach (ItemInfoLists item in listview1.Items)
                {
                    if (item.II_Code.IndexOf(key) != -1 || item.II_Name.IndexOf(key) != -1 ||
                        item.II_Spec.IndexOf(key) != -1 || item.II_UnitCode.IndexOf(key) !=-1||
                        item.II_UnitID.ToString().IndexOf(key) != -1 || item.II_UnitName.IndexOf(key) != -1 ||
                        item.II_Version.IndexOf(key) != -1)
                    {
                        iils.Add(item);
                    }
                }

                listview1.ItemsSource = iils;
            }
        }


        /// <summary>
        /// 搜索文本框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_SearchKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            btn_Search_Click(sender,e);
        }
    }
}
