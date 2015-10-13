using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BarCodeSystem.TechRoute.TechRoute
{
    /// <summary>
    /// TechRouteWorkHourImportItem_Window.xaml 的交互逻辑
    /// </summary>
    public partial class TechRouteWorkHourImportItem_Window : Window
    {

        DataSet ds = new DataSet();
        DataTable bc_iteminfo = new DataTable();

        //用于显示在listview1上
        List<ItemInfoLists> iils = new List<ItemInfoLists>();
       
        //用于返回给主界面
        public ItemInfoLists iil_return = new ItemInfoLists();

        string[] error = {"",
                             "必须且只能选择一个料品"};

        public TechRouteWorkHourImportItem_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 确认选择按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Confirm_Click(object sender, RoutedEventArgs e)
        {

            
            int x = 0;

            foreach (ItemInfoLists item in listview1.Items)
            {
                if (item.IsSelected)
                {
                    x++;
                }
            }


            if (x == 1)
            {
                foreach (ItemInfoLists item in listview1.Items)
                {
                    if (item.IsSelected)
                    {
                        //iil_return.ID = item.ID;
                        //iil_return.II_Code = item.II_Code;
                        //iil_return.II_Spec = item.II_Spec;
                        //iil_return.II_Version = item.II_Version;
                        //iil_return.II_Name = item.II_Name;
                        //iil_return.II_UnitCode = item.II_UnitCode;
                        //iil_return.II_UnitID = item.II_UnitID;
                        //iil_return.II_UnitName = item.II_UnitName;

                        iil_return = item;
                        
                    }
                }

                this.DialogResult = true;
                this.Close();

            }
            else
            {
                MessageBox.Show(error[1], "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }




        }
        /// <summary>
        /// 全选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AllSelect_Click(object sender, RoutedEventArgs e)
        {
            foreach (ItemInfoLists item in listview1.Items)
            {
                item.IsSelected = true;
            }
            listview1.Items.Refresh();
        }
        /// <summary>
        /// 重选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReSelect_Click(object sender, RoutedEventArgs e)
        {
            foreach (ItemInfoLists item in listview1.Items)
            {
                item.IsSelected = false;
            }
            listview1.Items.Refresh();
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
        /// 窗口加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetBCSItemInfoList();
        }

        /// <summary>
        /// 获取条码系统料品信息表
        /// </summary>
        private void GetBCSItemInfoList()
        {
            MyDBController.GetConnection();

            string SQl = @"select ID,II_Code,II_Spec,II_Version,II_Name,II_UnitID,II_UnitCode,II_UnitName,II_QualitySortID from ItemInfo";

            bc_iteminfo = MyDBController.GetDataSet(SQl, ds, "ItemInfo").Tables["ItemInfo"];

            MyDBController.CloseConnection();

            int count = bc_iteminfo.Rows.Count;

            for (int i = 0; i < count; i++)
            {
                ItemInfoLists iil = new ItemInfoLists();

                iil.ID = (Int64)bc_iteminfo.Rows[i]["ID"];
                iil.II_Code = bc_iteminfo.Rows[i]["II_Code"].ToString();
                iil.II_Spec = bc_iteminfo.Rows[i]["II_Spec"].ToString();
                iil.II_Version = bc_iteminfo.Rows[i]["II_Version"].ToString();
                iil.II_Name = bc_iteminfo.Rows[i]["II_Name"].ToString();
                iil.II_UnitID = (Int64)bc_iteminfo.Rows[i]["II_UnitID"];
                iil.II_UnitCode = bc_iteminfo.Rows[i]["II_UnitCode"].ToString();
                iil.II_UnitName = bc_iteminfo.Rows[i]["II_UnitName"].ToString();
                //iil.II_QualitySortID = (Int64)bc_iteminfo.Rows[i]["II_QualitySortID"];

                iils.Add(iil);

            }

            listview1.ItemsSource = null;
            listview1.ItemsSource = iils;
           
        }
    }

}
