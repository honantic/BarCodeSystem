using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Data;

namespace BarCodeSystem
{
    /// <summary>
    /// QualityIssues_Window.xaml 的交互逻辑
    /// </summary>
    public partial class QualityIssues_Window : Window
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        List<QualityIssuesLists> listBeforeSearch = new List<QualityIssuesLists> { };

        public QualityIssues_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //MyDBController.Server = User_Info.server[1];
            //MyDBController.Database = User_Info.database[1];
            //MyDBController.Pwd = User_Info.pwd[1];
            //MyDBController.Uid = User_Info.uid[1];
            GetBCSQualityList();

        }

        /// <summary>
        /// 获取条码系统质量信息清单
        /// </summary>
        private void GetBCSQualityList()
        {
            MyDBController.GetConnection();
            listBeforeSearch.Clear();
            ds.Clear();
            //            string SQl = @"SELECT [ID],[QI_Code],[QI_Name],[QI_BarCode],[QI_IsItemIssue],[QI_IsProduceIssue],[QI_IsPreviousIssue],
            //                            CASE [QI_IsItemIssue] WHEN 0 THEN '否' WHEN 1 THEN '是' END AS [QI_IsItemIssue_Show],
            //                            CASE [QI_IsProduceIssue] WHEN 0 THEN '否' WHEN 1 THEN '是' END AS [QI_IsProduceIssue_Show],
            //                            CASE [QI_IsPreviousIssue] WHEN 0 THEN '否' WHEN 1 THEN '是' END AS[QI_IsPreviousIssue_Show]
            //                            FROM [QualityIssue]";

            string SQl = @"SELECT [ID],[QI_Code],[QI_Name],[QI_BarCode] FROM [QualityIssue]";
            MyDBController.GetDataSet(SQl, ds, "QualityIssue");
            dt = ds.Tables["QualityIssue"];
            int x = dt.Rows.Count;
            for (int i = 0; i < x; i++)
            {
                QualityIssuesLists qil = new QualityIssuesLists();
                qil.ID = (Int64)dt.Rows[i]["ID"];
                qil.QI_Code = dt.Rows[i]["QI_Code"].ToString();
                qil.QI_Name = dt.Rows[i]["QI_Name"].ToString();
                qil.QI_BarCode = dt.Rows[i]["QI_BarCode"].ToString();
                //qil.QI_IsItemIssue = (bool)dt.Rows[i]["QI_IsItemIssue"];
                //qil.QI_IsPreviousIssue = (bool)dt.Rows[i]["QI_IsPreviousIssue"];
                //qil.QI_IsProduceIssue = (bool)dt.Rows[i]["QI_IsProduceIssue"];
                //qil.QI_IsItemIssue_Show = dt.Rows[i]["QI_IsItemIssue_Show"].ToString();
                //qil.QI_IsPreviousIssue_Show = dt.Rows[i]["QI_IsPreviousIssue_Show"].ToString();
                //qil.QI_IsProduceIssue_Show = dt.Rows[i]["QI_IsProduceIssue_Show"].ToString();
                listBeforeSearch.Add(qil);
            }
            listBeforeSearch = listBeforeSearch.OrderBy(p => p.QI_Code).ToList();
            listview1.ItemsSource = null;
            listview1.ItemsSource = listBeforeSearch;
        }

        /// <summary>
        /// 导入按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            QualityIssuesImport_Window qii = new QualityIssuesImport_Window();
            qii.Height = Math.Min(User_Info.ScreenHeight, 600);
            qii.Width = Math.Min(User_Info.ScreenWidth, 600);
            qii.existedQualityIssues = dt;
            qii.ShowDialog();
            if ((bool)qii.DialogResult)
            {
                GetBCSQualityList();
            }
        }

        /// <summary>
        /// 修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            QualityIssuesLists qil = null;
            foreach (QualityIssuesLists item in listview1.Items)
            {
                if (item.IsSelected)
                {
                    qil = item;
                    break;
                }
            }

            if (qil != null)
            {
                QualityIssuesModify_Window qim = new QualityIssuesModify_Window();
                qim.Height = Math.Min(User_Info.ScreenHeight, 400);
                qim.Width = Math.Min(User_Info.ScreenWidth, 600);
                qim.qil = qil;
                qim.Title = "质量档案修改窗口";
                qim.ShowDialog();
                if ((bool)qim.DialogResult)
                {
                    GetBCSQualityList();
                }
            }
            else
            {
                qil = listview1.SelectedItem as QualityIssuesLists;
                if (qil != null)
                {
                    QualityIssuesModify_Window qim = new QualityIssuesModify_Window();
                    qim.Height = Math.Min(User_Info.ScreenHeight, 400);
                    qim.Width = Math.Min(User_Info.ScreenWidth, 600);
                    qim.qil = qil;
                    qim.Title = "质量档案修改窗口";
                    qim.ShowDialog();
                    if ((bool)qim.DialogResult)
                    {
                        GetBCSQualityList();
                    }
                }
            }
        }

        /// <summary>
        /// 全选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (QualityIssuesLists item in listview1.Items)
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
            foreach (QualityIssuesLists item in listview1.Items)
            {
                item.IsSelected = false;
            }
            listview1.Items.Refresh();
        }

        /// <summary>
        /// 搜索文本框文本改变事件，关联搜索按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_SearchKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            btn_Search_Click(sender, e);
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

            List<QualityIssuesLists> qils = new List<QualityIssuesLists> { };
            if (txtb_SearchKey.Text.Length > 0)
            {
                string key = txtb_SearchKey.Text;
                IEnumerable<QualityIssuesLists> IEqils =
                    from item in listBeforeSearch
                    where (item.QI_Name.IndexOf(key) != -1 || item.QI_Code.IndexOf(key) != -1)
                    select item;
                foreach (QualityIssuesLists item in IEqils)
                {
                    qils.Add(item);
                }
                listview1.ItemsSource = null;
                listview1.ItemsSource = qils;
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
        /// 列表双击事件，关联修改按钮事件
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
                //btn_Modify_Click(sender, e);
            }
        }

        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            QualityIssuesModify_Window qim = new QualityIssuesModify_Window();
            qim.Height = Math.Min(User_Info.ScreenHeight, 400);
            qim.Width = Math.Min(User_Info.ScreenWidth, 600);
            qim.Title = "质量档案新增窗口";
            qim.ShowDialog();
            if ((bool)qim.DialogResult)
            {
                GetBCSQualityList();
            }
        }
    }
}
