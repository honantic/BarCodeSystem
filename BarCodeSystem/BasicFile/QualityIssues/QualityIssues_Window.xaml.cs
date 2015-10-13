using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Data;
using BarCodeSystem.BasicFile.QualityIssues;
using BarCodeSystem.PublicClass.HelperClass;

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
            listBeforeSearch = QualityIssuesLists.FetchBCSQualityIssueInfo(User_Info.User_Workcenter_ID);
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
                listBeforeSearch = QualityIssuesLists.FetchBCSQualityIssueInfo(User_Info.User_Workcenter_ID);
                listview1.ItemsSource = null;
                listview1.ItemsSource = listBeforeSearch;
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
                    listBeforeSearch = QualityIssuesLists.FetchBCSQualityIssueInfo(User_Info.User_Workcenter_ID);
                    listview1.ItemsSource = null;
                    listview1.ItemsSource = listBeforeSearch;
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
                        listBeforeSearch = QualityIssuesLists.FetchBCSQualityIssueInfo(User_Info.User_Workcenter_ID);
                        listview1.ItemsSource = null;
                        listview1.ItemsSource = listBeforeSearch;
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
                listBeforeSearch = QualityIssuesLists.FetchBCSQualityIssueInfo(User_Info.User_Workcenter_ID);
                listview1.ItemsSource = null;
                listview1.ItemsSource = listBeforeSearch;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            List<QualityIssuesLists> qilList = new List<QualityIssuesLists>();
            foreach (QualityIssuesLists item in listview1.Items)
            {
                if (item.IsSelected)
                {
                    qilList.Add(item);
                }
            }
            if (Xceed.Wpf.Toolkit.MessageBox.Show("确定要删除这" + qilList.Count + "条记录吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                MyDBController.GetConnection();
                DBLog _dbLog = new DBLog();
                _dbLog.DBL_OperateBy = User_Info.User_Code + "|" + User_Info.User_Name;
                _dbLog.DBL_OperateTable = "QualityIssue";
                _dbLog.DBL_OperateTime = DateTime.Now.ToString();
                _dbLog.DBL_OperateType = OperateType.Delete;
                _dbLog.DBL_Content = "删除质量问题原因:";

                qilList.ForEach(
                    p =>
                    {
                        string SQl = string.Format(@"delete from [QualityIssue] where [ID]={0}", p.ID);
                        _dbLog.DBL_Content += p.QI_Code + p.QI_Name + "|";
                        _dbLog.DBL_AssociateCode += p.QI_Code + "|";
                        _dbLog.DBL_AssociateID += p.ID.ToString() + "|";
                        try
                        {
                            MyDBController.ExecuteNonQuery(SQl);
                        }
                        catch (Exception)
                        {
                        }
                    });
                MyDBController.CloseConnection();
                DBLog.WriteDBLog(_dbLog);
                Xceed.Wpf.Toolkit.MessageBox.Show("删除成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Print_Click(object sender, RoutedEventArgs e)
        {
            List<QualityIssuesLists> qilList = new List<QualityIssuesLists>();
            foreach (QualityIssuesLists item in listview1.Items)
            {
                if (item.IsSelected)
                {
                    qilList.Add(item);
                }
            }
            if (qilList.Count == 0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请选择需要打印的条目！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                QualityIssuesPrint_Window qip = new QualityIssuesPrint_Window(qilList);
                qip.ShowDialog();
            }
        }
    }
}
