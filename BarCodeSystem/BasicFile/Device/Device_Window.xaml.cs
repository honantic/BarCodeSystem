using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Data;
using BarCodeSystem.PublicClass.HelperClass;

namespace BarCodeSystem
{
    /// <summary>
    /// Device_Window.xaml 的交互逻辑
    /// </summary>
    public partial class Device_Window : Window
    {
        public Device_Window()
        {
            InitializeComponent();
        }

        DataTable dt = new DataTable();
        DataSet ds = new DataSet();

        /// <summary>
        /// 设备清单列表，用来做搜索时的辅助变量
        /// </summary>
        List<DeviceLists> listBeforeSearch = new List<DeviceLists> { };

        /// <summary>
        /// 加载事件
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


            GetDeviceList();
        }

        /// <summary>
        /// 获得条码系统中设备清单
        /// </summary>
        private void GetDeviceList()
        {
            MyDBController.GetConnection();
            string SQl = @"SELECT A.[ID],A.[DD_Code],A.[DD_Name],A.[DD_Amount],A.[DD_BarCode],A.[DD_Version],A.[DD_WorkCenterID],
                        A.[DD_SourceType] ,A.[DD_IsValidated] ,
                        CASE A.[DD_SourceType] WHEN 0 THEN 'U9录入' WHEN 1 THEN '手工录入'  END AS [DD_SourceType_Show],
                        CASE A.[DD_IsValidated] WHEN 0 THEN '否' WHEN 1 THEN '是'  END AS [DD_IsValidated_Show],
                        B.[WC_Department_Code],B.[WC_Department_Name] 
                        FROM [DeviceDetail] A LEFT JOIN [WorkCenter] B
                        ON A.[DD_WorkCenterID]=B.[WC_Department_ID]";
            List<DeviceLists> dls = new List<DeviceLists> { };

            ds.Clear();
            listBeforeSearch.Clear();

            dt = MyDBController.GetDataSet(SQl, ds).Tables[0];
            MyDBController.CloseConnection();

            int x = dt.Rows.Count;

            for (int i = 0; i < x; i++)
            {
                DeviceLists dl = new DeviceLists();
                dl.D_ID = (Int64)dt.Rows[i]["ID"];
                dl.D_Code = dt.Rows[i]["DD_Code"].ToString();
                dl.D_Name = dt.Rows[i]["DD_Name"].ToString();
                dl.D_Amount = Convert.ToInt32(dt.Rows[i]["DD_Amount"]);
                dl.D_Version = dt.Rows[i]["DD_Version"].ToString();
                dl.D_BarCode = dt.Rows[i]["DD_BarCode"].ToString();
                dl.D_Department_ID = (Int64)dt.Rows[i]["DD_WorkCenterID"];
                dl.D_Department_Code = dt.Rows[i]["WC_Department_Code"].ToString();
                dl.D_Department_Name = dt.Rows[i]["WC_Department_Name"].ToString();
                dl.D_IsValidated_Show = dt.Rows[i]["DD_IsValidated_Show"].ToString();
                dl.D_IsValidated = (bool)dt.Rows[i]["DD_IsValidated"];
                dl.D_SourceType_Show = dt.Rows[i]["DD_SourceType_Show"].ToString();
                dl.D_SourceType = (int)dt.Rows[i]["DD_SourceType"];
                dl.IsSelected = false;
                dls.Add(dl);
                listBeforeSearch.Add(dl);
            }
            dls = dls.OrderBy(p => p.D_Code).ToList();
            listview1.ItemsSource = null;
            listview1.ItemsSource = dls;
        }


        /// <summary>
        /// 全选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            List<DeviceLists> dls = new List<DeviceLists> { };
            foreach (DeviceLists item in listview1.Items)
            {
                item.IsSelected = true;
                dls.Add(item);
            }
            listview1.ItemsSource = null;
            listview1.ItemsSource = dls;
        }

        /// <summary>
        /// 启用选中的设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Validate_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            List<string> colList = new List<string> { "ID", "DD_Code", "DD_Name", "DD_Amount", "DD_BarCode", "DD_Version", "DD_WorkCenterID", "DD_SourceType", "DD_IsValidated" };
            List<DeviceLists> dls = new List<DeviceLists> { };

            DataTable temp = dt.Clone();
            temp.TableName = "DeviceDetail";
            temp.Columns.Remove("DD_SourceType_Show");
            temp.Columns.Remove("DD_IsValidated_Show");
            temp.Columns.Remove("WC_Department_Name");
            temp.Columns.Remove("WC_Department_Code");
            temp.Columns.Add("IDNew", typeof(Int64));

            DBLog _dbLog = new DBLog();
            _dbLog.DBL_OperateBy = User_Info.User_Code + "|" + User_Info.User_Name;
            _dbLog.DBL_OperateTable = "DeviceDetail";
            _dbLog.DBL_OperateTime = DateTime.Now.ToString();
            _dbLog.DBL_OperateType = OperateType.Update;
            _dbLog.DBL_Content = "启用设备的编号：";

            foreach (DeviceLists item in listview1.Items)
            {
                if (item.IsSelected)
                {
                    item.D_IsValidated = true;
                    item.D_IsValidated_Show = "是";
                    DataRow temprow = temp.NewRow();
                    temprow["ID"] = item.D_ID;
                    temprow["DD_Code"] = item.D_Code;
                    temprow["DD_Name"] = item.D_Name;
                    temprow["DD_Amount"] = item.D_Amount;
                    temprow["DD_Version"] = item.D_Version;
                    temprow["DD_BarCode"] = item.D_BarCode;
                    temprow["DD_WorkCenterID"] = item.D_Department_ID;
                    temprow["DD_SourceType"] = item.D_SourceType;
                    temprow["DD_IsValidated"] = item.D_IsValidated;
                    temprow["IDNew"] = item.D_ID;
                    temp.Rows.Add(temprow);
                    _dbLog.DBL_Content += item.D_Code + ",";
                    _dbLog.DBL_AssociateCode += item.D_Code + ",";
                }
                else
                {

                }
            }

            if (temp.Rows.Count > 0)
            {
                MyDBController.GetConnection();
                int updateNum = 0, insertNum = 0;
                try
                {
                    MyDBController.InsertSqlBulk(temp, colList, out updateNum, out insertNum);
                }
                catch (Exception ee)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                MyDBController.CloseConnection();
                string message = string.Format(@"共成功启用" + updateNum + "个设备！");
                DBLog.WriteDBLog(_dbLog);
                MessageBox.Show(message, "提示", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {
                MessageBox.Show("请选中至少一条设备信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            this.Cursor = Cursors.Arrow;
            GetDeviceList();
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

            List<DeviceLists> dls = new List<DeviceLists> { };
            IEnumerable<DeviceLists> IEdls = new List<DeviceLists> { };

            if (txtb_SearchKey.Text != "")
            {
                string key = txtb_SearchKey.Text;
                IEdls =
                from item in listBeforeSearch
                where (item.D_Department_Code.IndexOf(key) != -1 || item.D_Department_Name.IndexOf(key) != -1 ||
                        item.D_Code.IndexOf(key) != -1 || item.D_Name.IndexOf(key) != -1 ||
                        item.D_IsValidated_Show.IndexOf(key) != -1 || item.D_SourceType_Show.IndexOf(key) != -1)
                select item;

                foreach (DeviceLists item in IEdls)
                {
                    dls.Add(item);
                }
                listview1.ItemsSource = null;
                listview1.ItemsSource = dls;
            }
            else
            {

            }
        }


        /// <summary>
        /// 搜索框文本改变事件，关联搜索按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_SearchKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            btn_Search_Click(sender, e);
        }


        /// <summary>
        /// Excel导入设备信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ExcelImprto_Click(object sender, RoutedEventArgs e)
        {
            DeviceExcelImport_Window dei = new DeviceExcelImport_Window();
            dei.Height = Math.Min(User_Info.ScreenHeight, 600);
            dei.Width = Math.Min(User_Info.ScreenWidth, 600);
            dei.exisitDevice = dt;
            dei.ShowDialog();
            if ((bool)dei.DialogResult)
            {
                GetDeviceList();
            }
        }

        /// <summary>
        /// 重选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReSelect_Click(object sender, RoutedEventArgs e)
        {
            List<DeviceLists> dls = new List<DeviceLists> { };
            foreach (DeviceLists item in listview1.Items)
            {
                item.IsSelected = false;
                dls.Add(item);
            }

            listview1.ItemsSource = null;
            listview1.ItemsSource = dls;
        }
    }
}
