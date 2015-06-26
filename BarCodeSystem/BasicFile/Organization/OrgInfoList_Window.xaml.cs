using BarCodeSystem.PublicClass;
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

namespace BarCodeSystem
{
    /// <summary>
    /// OrgInfoList_Window.xaml 的交互逻辑
    /// </summary>
    public partial class OrgInfoList_Window : Window
    {
        //系统全局变量 ID、组织ID、组织编码、组织名称、备注
        public static Int64 ID;
        public static Int64 OI_ID;
        public static string OI_Code;
        public static string OI_Name;
        public static string OI_Remark;



        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        List<OrgInfoList> listBeforeSearch = new List<OrgInfoList> { };

        public OrgInfoList_Window()
        {
            InitializeComponent();
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
        /// 导入按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            OrgInfoImport_Window oiiw = new OrgInfoImport_Window();
            oiiw.ShowDialog();

            if ((bool)oiiw.DialogResult)
            {
                listview1.ItemsSource = null;
                listview1.Items.Refresh();
                ds.Clear();

                GetBCSOrgInfoList();
            }
        }

        /// <summary>
        /// 窗口加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //这段代码在正式环境中将被注释掉，测试用
            //MyDBController.Server = User_Info.server[1];
            //MyDBController.Database = User_Info.database[1];
            //MyDBController.Pwd = User_Info.pwd[1];
            //MyDBController.Uid = User_Info.uid[1];

            GetBCSOrgInfoList();
        }

        private void GetBCSOrgInfoList()
        {
            MyDBController.GetConnection();

            string SQl = @"SELECT [ID],[OI_ID],[OI_Code],[OI_Name],[OI_Remark]
                            FROM [OrgInfo]";
            dt = MyDBController.GetDataSet(SQl, ds, "OrgInfo").Tables["OrgInfo"];
            MyDBController.CloseConnection();

            listBeforeSearch.Clear();
            int x = dt.Rows.Count;
            for (int i = 0; i < x; i++)
            {
                OrgInfoList oil = new OrgInfoList();
                oil.ID = (Int64)dt.Rows[i]["ID"];
                oil.OI_ID = (Int64)dt.Rows[i]["OI_ID"];
                oil.OI_Code = dt.Rows[i]["OI_Code"].ToString();
                oil.OI_Name = dt.Rows[i]["OI_Name"].ToString();
                oil.OI_Remark = dt.Rows[i]["OI_Remark"].ToString();
                listBeforeSearch.Add(oil);
            }
            listview1.ItemsSource = null;
            listview1.ItemsSource = listBeforeSearch;
        }
        /// <summary>
        /// listview1鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview1_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OrgInfoModify_Window oimw = new OrgInfoModify_Window() { oil = (OrgInfoList)listview1.SelectedItem };
            oimw.Title = "组织信息修改窗口";
            oimw.ShowDialog();

            if ((bool)oimw.DialogResult)
            {
                listview1.ItemsSource = null;
                listview1.Items.Refresh();
                ds.Clear();

                GetBCSOrgInfoList();
            }
        }

        /// <summary>
        /// 重选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReSelect_Click(object sender, RoutedEventArgs e)
        {
            foreach (OrgInfoList item in listview1.Items)
            {
                item.IsSelected = false;
            }
            listview1.Items.Refresh();
        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            int isselected = 0;

            foreach (OrgInfoList item in listview1.Items)
            {
                if (item.IsSelected)
                {
                    isselected++;
                }
            }

            if (isselected <= 0)
            {
                MessageBox.Show("必须选一个组织", "无法保存", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (isselected > 1)
            {
                MessageBox.Show("只能选一个组织", "无法保存", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                foreach (OrgInfoList item in listview1.Items)
                {
                    if (item.IsSelected)
                    {
                        ID = Convert.ToInt64(item.ID);
                        OI_ID = Convert.ToInt64(item.OI_ID);
                        OI_Code = item.OI_Code;
                        OI_Name = item.OI_Name;
                        OI_Remark = item.OI_Remark;
                    }
                }
                MessageBox.Show("保存成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            int x = listview1.SelectedIndex;
            if (x > -1)
            {
                OrgInfoModify_Window oimw = new OrgInfoModify_Window() { oil = (OrgInfoList)listview1.SelectedItem };
                oimw.Title = "组织信息修改窗口";
                oimw.ShowDialog();

                if ((bool)oimw.DialogResult)
                {
                    listview1.ItemsSource = null;
                    listview1.Items.Refresh();
                    ds.Clear();

                    GetBCSOrgInfoList();
                }
            }
            else
            {
                MessageBox.Show("请选择一项进行修改!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            int x = listview1.SelectedIndex;
            if (x > -1)
            {
                OrgInfoModify_Window oimw = new OrgInfoModify_Window() { oil = (OrgInfoList)listview1.SelectedItem };
                oimw.Title = "组织信息删除窗口";
                oimw.ShowDialog();

                if ((bool)oimw.DialogResult)
                {
                    listview1.ItemsSource = null;
                    listview1.Items.Refresh();
                    ds.Clear();

                    GetBCSOrgInfoList();
                }
            }
            else
            {
                MessageBox.Show("请选择一项进行删除!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
