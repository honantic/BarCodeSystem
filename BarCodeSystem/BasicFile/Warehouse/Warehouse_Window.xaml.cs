using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Data;
using System.Linq;

namespace BarCodeSystem
{
    /// <summary>
    /// Warehouse_Window.xaml 的交互逻辑
    /// </summary>
    public partial class Warehouse_Window : Window
    {
        public Warehouse_Window()
        {
            InitializeComponent();
        }

        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        List<WarehouseLists> listBeforeSearch = new List<WarehouseLists> { };
        /// <summary>
        /// 窗体加载事件
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



            GetWarehouseList();
        }


        /// <summary>
        /// 获取条系统仓库清单
        /// </summary>
        private void GetWarehouseList()
        {
            MyDBController.GetConnection();
            List<WarehouseLists> whls = new List<WarehouseLists> { };
            listBeforeSearch.Clear();
            ds.Clear();
            string SQl = @"SELECT [ID],[W_ID],[W_Code],[W_Name],
                        case [W_SourceType]
                            when 0 THEN 'U9录入'
                            when 1 THEN '手工录入'
                            ELSE '其他' END
                            as [W_SourceType_Show], 
                        case [W_IsValidated]
                            when 0 THEN '否'
                            when 1 THEN '是'
                            ELSE '未知' END
                            as [W_IsValidated_Show],
                        [W_SourceType] ,
                        [W_IsValidated] 
                        FROM Warehouse";

            dt = MyDBController.GetDataSet(SQl, ds).Tables[0];
            MyDBController.CloseConnection();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                WarehouseLists whl = new WarehouseLists();
                whl.W_ID = (Int64)dt.Rows[i]["W_ID"];
                whl.W_Code = dt.Rows[i]["W_Code"].ToString();
                whl.W_Name = dt.Rows[i]["W_Name"].ToString();
                whl.W_SourceType_Show = dt.Rows[i]["W_SourceType_Show"].ToString();
                whl.W_IsValidated_Show = dt.Rows[i]["W_IsValidated_Show"].ToString();
                whl.W_IsValidated = (bool)dt.Rows[i]["W_IsValidated"];
                whl.W_SourceType = (int)dt.Rows[i]["W_SourceType"];
                whls.Add(whl);
                listBeforeSearch.Add(whl);
            }
            listBeforeSearch = listBeforeSearch.OrderBy(p => p.W_Code).ToList();
            whls = whls.OrderBy(p => p.W_Code).ToList();
            listview1.ItemsSource = null;
            listview1.ItemsSource = whls;
        }

        /// <summary>
        /// 修改选中的记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            WarehouseModify_Window whm = new WarehouseModify_Window();
            whm.Height = Math.Min(User_Info.ScreenHeight, 400);
            whm.Width = Math.Min(User_Info.ScreenWidth, 600);
            foreach (WarehouseLists item in listview1.Items)
            {
                if (item.IsSelected)
                {
                    whm.whl = item;
                    whm.ShowDialog();
                    if ((bool)whm.DialogResult)
                    {
                        GetWarehouseList();
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// 导入U9仓库信息，条码系统中存在的信息不做显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            WarehouseImport_Window whi = new WarehouseImport_Window();
            whi.Height = Math.Min(User_Info.ScreenHeight, 600);
            whi.Width = Math.Min(User_Info.ScreenWidth, 600);
            whi.BCSWarehouse = dt;
            whi.ShowDialog();
            if ((bool)whi.DialogResult)
            {
                GetWarehouseList();
            }
        }

        /// <summary>
        /// 启用选中的仓库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ValidateSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            MyDBController.GetConnection();
            string SQl = "";
            int count = 0;
            foreach (WarehouseLists item in listview1.Items)
            {
                if (item.IsSelected)
                {
                    item.W_IsValidated = true;
                    SQl = string.Format(@"UPDATE [Warehouse] SET [W_IsValidated]='{1}'
                                    WHERE [W_ID]={1}", item.W_IsValidated, item.W_ID);

                    try
                    {
                        count += MyDBController.ExecuteNonQuery(SQl);
                    }
                    catch (Exception ee)
                    {

                        MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

            if (count > 0)
            {
                MessageBox.Show("成功启用" + count + "个仓库！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            MyDBController.CloseConnection();
            this.Cursor = Cursors.Arrow;
            GetWarehouseList();
        }


        /// <summary>
        /// 双击记录，修改信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WarehouseLists item = listview1.SelectedItem as WarehouseLists;

            Point formPoint = e.GetPosition(this);
            if (formPoint.Y > 60 && formPoint.Y < 90)//表头部分不做双击响应
            {
            }
            else
            {
                if (item != null)
                {
                    WarehouseModify_Window whm = new WarehouseModify_Window();
                    whm.Height = Math.Min(User_Info.ScreenHeight, 400);
                    whm.Width = Math.Min(User_Info.ScreenWidth, 600);
                    whm.whl = item;
                    whm.ShowDialog();

                    if ((bool)whm.DialogResult)
                    {
                        GetWarehouseList();
                    }
                }
                else
                {

                }
            }

        }

        /// <summary>
        /// 刷新列表按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RefreshList_Click(object sender, RoutedEventArgs e)
        {
            GetWarehouseList();
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
            List<WarehouseLists> whls = new List<WarehouseLists> { };
            if (txtb_SearchKey.Text.Length > 0)
            {
                string key = txtb_SearchKey.Text;
                IEnumerable<WarehouseLists> IEwhls =
                    from item in listBeforeSearch
                    where (item.W_Code.IndexOf(key) != -1 || item.W_Name.IndexOf(key) != -1 ||
                        item.W_IsValidated_Show.IndexOf(key) != -1 || item.W_SourceType_Show.IndexOf(key) != -1)
                    select item;
                foreach (WarehouseLists item in IEwhls)
                {
                    whls.Add(item);
                }
            }

            listview1.ItemsSource = null;
            listview1.ItemsSource = whls;
        }


        /// <summary>
        /// 搜索文本框文本改变事件，关联搜索按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_SearchKey_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            btn_Search_Click(sender, e);
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (WarehouseLists item in listview1.Items)
            {
                item.IsSelected = true;
            }
            listview1.Items.Refresh();
        }


        /// <summary>
        /// 重选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReSelect_Click(object sender, RoutedEventArgs e)
        {
            foreach (WarehouseLists item in listview1.Items)
            {
                item.IsSelected = false;
            }
            listview1.Items.Refresh();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
