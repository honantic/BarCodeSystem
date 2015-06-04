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
    /// QualitySort_Window.xaml 的交互逻辑
    /// </summary>
    public partial class QualitySort_Window : Window
    {

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        List<QualitySortList> listBeforeSearch = new List<QualitySortList> { };

        public QualitySortList qrl;
        public QualitySort_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗口加载程序
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

            GetBCSQualitySortLists();
        }

        private void GetBCSQualitySortLists()
        {
            MyDBController.GetConnection();

            string SQl = @"select ID,QS_Code,QS_Name from  QualitySort";
            dt = MyDBController.GetDataSet(SQl, ds, "QualitySort").Tables["QualitySort"];
            MyDBController.CloseConnection();

            int x = dt.Rows.Count;
            for (int i = 0; i < x; i++)
            {
                QualitySortList qsl = new QualitySortList();
                qsl.ID = (Int64)dt.Rows[i]["ID"];
                qsl.QS_Name = dt.Rows[i]["QS_Name"].ToString();
                qsl.QS_Code = dt.Rows[i]["QS_Code"].ToString();
                listBeforeSearch.Add(qsl);
            }
            listview1.ItemsSource = null;
            listview1.ItemsSource = listBeforeSearch;
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

            if (txtb_SearchKey.Text.Length > 0)
            {
                string key = txtb_SearchKey.Text;
                #region
                //List<QualitySortList> quls = new List<QualitySortList> { };
                //IEnumerable<QualitySortList> IEpnls =
                //    from item in listBeforeSearch
                //    where (item.QS_Code.IndexOf(key) != -1 || item.QS_Name.IndexOf(key) != -1)
                //    select item;
                //foreach (QualitySortList item in IEpnls)
                //{
                //    quls.Add(item);
                //}
                #endregion

                listview1.ItemsSource = null;
                listview1.ItemsSource = from item in listBeforeSearch
                                        where (item.QS_Code.IndexOf(key) != -1 || item.QS_Name.IndexOf(key) != -1)
                                        select item;
            }
        }

        /// <summary>
        /// 重选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReSelect_Click(object sender, RoutedEventArgs e)
        {
            foreach (QualitySortList item in listview1.Items)
            {
                item.IsSelected = false;
            }
            listview1.Items.Refresh();
        }
        /// <summary>
        /// 全选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (QualitySortList item in listview1.Items)
            {
                item.IsSelected = true;
            }
            //listview1.ItemsSource = null;
            //listview1.ItemsSource = listBeforeSearch;
            listview1.Items.Refresh();

        }

        /// <summary>
        /// 导入按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            int x = 0;

            foreach (QualitySortList item in listview1.Items)
            {
                if (item.IsSelected)
                {
                    x++;
                }
            }


            if (x == 0)
            {
                MessageBox.Show("请先选择一个质检分类", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (x > 1)
            {
                MessageBox.Show("只能选择一个质检分类进行修改", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                QualitySortModify_Window qsw = new QualitySortModify_Window() { qsl = listBeforeSearch.Find(p => p.IsSelected == true) };
                qsw.Title = "质检分类修改窗口";
                qsw.ShowDialog();


                if ((bool)qsw.DialogResult)
                {
                    ds.Clear();
                    listBeforeSearch.Clear();
                    GetBCSQualitySortLists();
                }
            }
        }
        /// <summary>
        /// 增加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            QualitySortModify_Window qsw = new QualitySortModify_Window();
            qsw.Title = "质检分类新增窗口";
            qsw.ShowDialog();

            if ((bool)qsw.DialogResult)
            {
                ds.Clear();
                listBeforeSearch.Clear();
                GetBCSQualitySortLists();
            }
        }
        /// <summary>
        /// 文本框文本改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_SearchKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            btn_Search_Click(sender, e);
        }

        /// <summary>
        /// listview1鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview1_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            QualitySortModify_Window qsw = new QualitySortModify_Window() { qsl = (QualitySortList)listview1.SelectedItem };
            qsw.Title = "质检分类修改窗口";
            qsw.ShowDialog();


            if ((bool)qsw.DialogResult)
            {
                ds.Clear();
                listBeforeSearch.Clear();
                GetBCSQualitySortLists();
            }
        }
        /// <summary>
        /// 删除信息按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            int x = 0;

            foreach (QualitySortList item in listview1.Items)
            {
                if (item.IsSelected)
                {
                    x++;
                }
            }


            if (x == 0)
            {
                MessageBox.Show("请先选择一个质检分类", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (x > 1)
            {
                MessageBox.Show("只能选择一个质检分类进行删除", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                QualitySortModify_Window qsw = new QualitySortModify_Window() { qsl = listBeforeSearch.Find(p => p.IsSelected == true) };
                qsw.Title = "质检分类删除窗口";
                qsw.ShowDialog();


                if ((bool)qsw.DialogResult)
                {
                    ds.Clear();
                    listBeforeSearch.Clear();
                    GetBCSQualitySortLists();
                }
            }
        }
    }
}
