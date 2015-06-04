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
    /// OrgInfoImport_Window.xaml 的交互逻辑
    /// </summary>
    public partial class OrgInfoImport_Window : Window
    {

        DataTable dt = new DataTable();
        DataTable U9dt = new DataTable();
        DataSet ds = new DataSet();


        List<OrgInfoList> U9listBeforeSearch = new List<OrgInfoList> { };

        public OrgInfoImport_Window()
        {
            InitializeComponent();
        }

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

            string SQl = @"SELECT [ID],[OI_ID],[OI_Code],[OI_Name],[OI_Remark] FROM [OrgInfo]";
            dt = MyDBController.GetDataSet(SQl, ds, "OrgInfo").Tables["OrgInfo"];

            //因条码测试数据库上没有U9的表，故建了一张表模拟U9组织表，正式环境中将被注释掉，测试用
            SQl = @"select A.ID,A.Org_Code,A.Org_Name from Test_Org as A";


            //正式环境连接语句
            //string SQl = @"select A1.[Name],A.[Code],A.[ID]
            //              from Base_Organization as A 
            //              left join [Base_Organization_Trl] as A1 on (A1.SysMlFlag = 'zh-CN') and (A.[ID] = A1.[ID])";

            U9dt = MyDBController.GetDataSet(SQl, ds, "U9OrgInfo").Tables["U9OrgInfo"];
            MyDBController.CloseConnection();
            U9listBeforeSearch.Clear();
            int x = U9dt.Rows.Count;
            for (int i = 0; i < x; i++)
            {
                if (dt.Select("OI_Code = '" + U9dt.Rows[i]["Org_Code"].ToString() + "'").Length == 0)
                {
                    OrgInfoList oil = new OrgInfoList();
                    oil.OI_ID = (Int64)U9dt.Rows[i]["ID"];
                    oil.OI_Code = U9dt.Rows[i]["Org_Code"].ToString();
                    oil.OI_Name = U9dt.Rows[i]["Org_Name"].ToString();
                    U9listBeforeSearch.Add(oil);
                }
            }
            listview1.ItemsSource = U9listBeforeSearch;
            
        }
        /// <summary>
        /// 导入按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            DataRow r1,r2;
            DataTable OrgInfo = new DataTable();
            OrgInfo.TableName = "OrgInfo";

            MyDBController.GetConnection();
            dt.Clear();
            string SQl = @"SELECT [ID],[OI_ID],[OI_Code],[OI_Name],[OI_Remark] FROM [OrgInfo]";
            dt = MyDBController.GetDataSet(SQl, ds, "OrgInfo").Tables["OrgInfo"];
            MyDBController.CloseConnection();



            OrgInfo.Columns.Add("ID",typeof(Int64));
            OrgInfo.Columns.Add("OI_ID", typeof(Int64));
            OrgInfo.Columns.Add("OI_Code", typeof(string));
            OrgInfo.Columns.Add("OI_Name", typeof(string));
            OrgInfo.Columns.Add("OI_Remark", typeof(string));
            OrgInfo.Columns.Add("IDNew",typeof(Int64));

            foreach (OrgInfoList item in U9listBeforeSearch)
            {
                if (item.IsSelected)
                {
                    r1 = OrgInfo.NewRow();


                    if (dt.Select("OI_Code = '" + item.OI_Code + "'").Length > 0)
                    {
                        r2 = dt.Select("OI_Code = '" + item.OI_Code + "'")[0];
                        r1["ID"] = r2["ID"];
                        r1["IDNew"] = r2["ID"];
                    }
                    else
                    {
                        r1["ID"] = item.ID;
                    }
                    r1["OI_ID"] = item.OI_ID;
                    r1["OI_Code"] = item.OI_Code;
                    r1["OI_Name"] = item.OI_Name;
                    r1["OI_Remark"] = "";

                    OrgInfo.Rows.Add(r1);
                    OrgInfo.AcceptChanges();

                }
            }

            List<string> cloList = new List<string> { "ID", "OI_ID", "OI_Code", "OI_Name","OI_Remark" };

            MyDBController.GetConnection();
            int updateNum, insertNum;
            MyDBController.InsertSqlBulk(OrgInfo, cloList, out updateNum, out insertNum);
            MyDBController.CloseConnection();
            MessageBox.Show("更新了" +updateNum.ToString() + "条数据 , 导入了" + insertNum.ToString() + "条数据");

            //foreach (OrgInfoList item in U9listBeforeSearch)
            //{
            //    if (item.IsSelected)
            //    {
            //        U9listBeforeSearch.RemoveAll(p =>p.OI_Code == item.OI_Code );
            //    }
            //}

            U9listBeforeSearch.RemoveAll(p => p.IsSelected == true);

            listview1.Items.Refresh();
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
        /// 全选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AllSelect_Click(object sender, RoutedEventArgs e)
        {
            foreach (OrgInfoList item in listview1.Items)
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
            foreach (OrgInfoList item in listview1.Items)
            {
                item.IsSelected = false;
            }
            listview1.Items.Refresh();
        }
    }
}
