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
    /// ItemInfoImport_Window.xaml 的交互逻辑
    /// </summary>
    public partial class ItemInfoImport_Window : Window
    {
        public ItemInfoImport_Window()
        {
            InitializeComponent();
        }

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        List<ItemInfoLists> listBefroeSearch = new List<ItemInfoLists> { };
        List<ItemInfoLists> listBeenImported = new List<ItemInfoLists> { };//导入条码系统的列表
        /// <summary>
        /// 父窗体传值
        /// </summary>
        public DataTable existItem
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
            //这段代码在正式环境中将被注释掉，测试用
            MyDBController.Server = User_Info.server[1];
            MyDBController.Database = User_Info.database[1];
            MyDBController.Pwd = User_Info.pwd[1];
            MyDBController.Uid = User_Info.uid[1];
        }

        /// <summary>
        /// 获取U9料品清单，条码系统中已经存在的料品不做显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_U9ItemList_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            listBefroeSearch.Clear();

            ds.Clear();
            WebService.ServiceSoapClient ws = new WebService.ServiceSoapClient();
            ds = ws.GetItemmasterModifiedOnlist_ForMES(User_Info.User_Org_Code[0],"");
            dt = ds.Tables[0];

            int x = existItem.Rows.Count;
            int y = dt.Rows.Count;
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    if (dt.Rows[i]["itemmaster_code"].ToString()==
                        existItem.Rows[j]["II_Code"].ToString())
                    {
                        dt.Rows[i].Delete();
                        break;
                    }
                }
            }
            #region 调整由webservice获得的datatable，使之符合条码系统的表结构
            dt.TableName = "ItemInfo";
            dt.Columns["itemmaster_code"].ColumnName = "II_Code";
            dt.Columns["itemmaster_name"].ColumnName = "II_Name";
            dt.Columns["itemmaster_specs"].ColumnName = "II_Spec";
            dt.Columns["itemmaster_descflexfield"].ColumnName = "II_Version";
            dt.Columns["itemmaster_uom"].ColumnName = "II_UnitName";
            dt.Columns["itemmaster_uom_code"].ColumnName = "II_UnitCode";
            dt.Columns["itemmaster_uom_id"].ColumnName = "II_UnitID";
            dt.Columns.Remove("Organization_id");
            dt.Columns.Remove("Organization_code");
            dt.Columns.Remove("Organization_name");
            dt.Columns.Remove("itemmaster_id");
            dt.Columns.Remove("modifiedon");

            dt.Columns.Add("ID",typeof(Int64));


            dt.Columns["ID"].SetOrdinal(0);
            dt.Columns["II_Code"].SetOrdinal(1);
            dt.Columns["II_Spec"].SetOrdinal(2);
            dt.Columns["II_Version"].SetOrdinal(3);
            dt.Columns["II_Name"].SetOrdinal(4);
            dt.Columns["II_UnitID"].SetOrdinal(5);
            dt.Columns["II_UnitCode"].SetOrdinal(6);
            dt.Columns["II_UnitName"].SetOrdinal(7);
            dt.AcceptChanges();
            #endregion

            #region 将dt转换成listview数据源List<ItemInfoLists>
            x = dt.Rows.Count;//更改后的dt行数
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
                listBefroeSearch.Add(iil);
            }
            listview1.ItemsSource = listBefroeSearch;
            #endregion

            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (ItemInfoLists item in listview1.Items)
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
            foreach (ItemInfoLists item in listview1.Items)
            {
                item.IsSelected = false;
            }
            listview1.Items.Refresh();
        }

        /// <summary>
        /// 搜索文本框文本改变事件，关联搜索按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_SearchKey_TextChanged(object sender, TextChangedEventArgs e)
        {
                btn_Search_Click(sender,e);
        }


        /// <summary>
        /// 搜索按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            listview1.ItemsSource = null;
            listview1.ItemsSource = listBefroeSearch;
            if (txtb_SearchKey.Text.Length > 0)
            {
                string key = txtb_SearchKey.Text;
                
                List<ItemInfoLists> iils = new List<ItemInfoLists> { };

                IEnumerable<ItemInfoLists> IEiils =
                    from item in listBefroeSearch
                    where (item.II_Code.IndexOf(key) != -1 || item.II_Name.IndexOf(key) != -1 ||
                        item.II_Spec.IndexOf(key) != -1 || item.II_UnitCode.IndexOf(key) != -1 ||
                        item.II_UnitID.ToString().IndexOf(key) != -1 || item.II_UnitName.IndexOf(key) != -1 ||
                        item.II_Version.IndexOf(key) != -1)
                    select item;
                foreach (ItemInfoLists item in IEiils)
                {
                    iils.Add(item);
                }

                listview1.ItemsSource = iils;
            }
            else
            {    
            }
        }


        /// <summary>
        /// 选中的料品导入条码系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            DataTable temp = dt.Clone();
            List<string> colList = new List<string> { "ID", "II_Code", "II_Spec", "II_Version", "II_Name", 
                "II_UnitID","II_UnitCode", "II_UnitName"};

            listBeenImported.Clear();
            for (int i = 0; i < listview1.Items.Count; i++)
            {
                ItemInfoLists iil = listview1.Items[i] as ItemInfoLists;
                if (iil.IsSelected)
                {
                    DataRow dr = temp.NewRow();
                    dr["II_Code"] = iil.II_Code;
                    dr["II_Spec"] = iil.II_Spec;
                    dr["II_Version"] = iil.II_Version;
                    dr["II_Name"] = iil.II_Name;
                    dr["II_UnitID"] = iil.II_UnitID;
                    dr["II_UnitCode"] = iil.II_UnitCode;
                    dr["II_UnitName"] = iil.II_UnitName;
                    temp.Rows.Add(dr);
                    listBeenImported.Add(iil);
                }
                else
                {
                    
                }
            }
            MyDBController.GetConnection();
            int updateNum = 0, insertNum = 0;
            MyDBController.InsertSqlBulk(temp,colList,out updateNum,out insertNum);
            MyDBController.CloseConnection();
            string message = string.Format(@"共更新 {0} 个料品信息！"+"\n新增 {1} 个料品信息！",updateNum,insertNum);
            MessageBox.Show(message,"提示",MessageBoxButton.OK,MessageBoxImage.Information);
            listBefroeSearch.RemoveAll(HasBeenImported);
            listview1.ItemsSource = listBefroeSearch;
        }

        /// <summary>
        /// listBefroeSearch.RemoveAll的匹配函数
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool HasBeenImported(ItemInfoLists item)
        {
            bool HasBeenImported = false;
            foreach (ItemInfoLists xitem in listBeenImported)
            {
                if (xitem.II_Code==item.II_Code)
                {
                    HasBeenImported = true;
                    break;
                }
            }
            return HasBeenImported;
        }

    }
}
