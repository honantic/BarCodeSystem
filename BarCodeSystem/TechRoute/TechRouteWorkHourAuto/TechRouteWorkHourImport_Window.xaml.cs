using BarCodeSystem.PublicClass.DatabaseEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    /// TechRouteWorkHourImport_Window.xaml 的交互逻辑
    /// </summary>
    public partial class TechRouteWorkHourImport_Window : Window
    {


        List<TechRouteImportList> trils = new List<TechRouteImportList>();

        ItemInfoLists iil = new ItemInfoLists();

        DataSet ds = new DataSet();
        DataTable bc_workhour = new DataTable();
        DataTable bc_techroute = new DataTable();
        DataTable bc_techrouteforquery = new DataTable();

        DataTable copiedData;

        string[] error = {"",                                       //0

                          "条码系统中不存在符合该料品版本的工序!",  //1
                          "工时的开始时间不能小于等于已有的开始时间!",//2
                          "列表中没有数据或没有进行检测",          //3
                          "料号或版本号或工序号或开始时间或结束时间或工时不能为空!" //4
                         }; 



        //数据是否正确
        bool IsRight = false;
        //是否有数据
        bool IsEmpty = false;

        public TechRouteWorkHourImport_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗口加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetBCTechRouteList();
            GetBCWorkHourList();
        }

        /// <summary>
        /// 导入按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            //导入前检测
            CheckIfRight();

            DataRow r1, r2,r3,r4;

            if (IsRight)
            {

                //新建工时表
                DataTable WorkHour = new DataTable("WorkHour");
                WorkHour.Columns.Add("ID", typeof(Int64));
                WorkHour.Columns.Add("WH_TechRouteID", typeof(Int64));
                WorkHour.Columns.Add("WH_WorkHour", typeof(decimal));
                WorkHour.Columns.Add("WH_StartDate", typeof(DateTime));
                WorkHour.Columns.Add("WH_EndDate", typeof(DateTime));
                WorkHour.Columns.Add("IDNew", typeof(Int64));


                //新建工艺路线表
                DataTable TechRoute = new DataTable("TechRoute");
                TechRoute.Columns.Add("ID", typeof(Int64));
                TechRoute.Columns.Add("TR_ItemID", typeof(Int64));
                TechRoute.Columns.Add("TR_ItemCode", typeof(string));
                TechRoute.Columns.Add("TR_VersionID", typeof(Int64));
                TechRoute.Columns.Add("TR_ProcessSequence", typeof(int));
                TechRoute.Columns.Add("TR_ProcessName", typeof(string));
                TechRoute.Columns.Add("TR_ProcessCode", typeof(string));
                TechRoute.Columns.Add("TR_ProcessID", typeof(Int64));
                TechRoute.Columns.Add("TR_WorkHour", typeof(decimal));
                TechRoute.Columns.Add("TR_IsReportPoint", typeof(bool));
                TechRoute.Columns.Add("TR_IsExProcess", typeof(bool));
                TechRoute.Columns.Add("TR_WorkCenterID", typeof(Int64));
                TechRoute.Columns.Add("TR_IsFirstProcess", typeof(bool));
                TechRoute.Columns.Add("TR_IsLastProcess", typeof(bool));
                TechRoute.Columns.Add("TR_IsTestProcess", typeof(bool));
                TechRoute.Columns.Add("TR_IsBackProcess", typeof(bool));
                TechRoute.Columns.Add("TR_DefaultCheckPersonName", typeof(string));
                TechRoute.Columns.Add("TR_BindingProcess", typeof(int));
                TechRoute.Columns.Add("TR_IsReportDevice", typeof(bool));
                TechRoute.Columns.Add("TR_IsDeviceCharging", typeof(bool));
                TechRoute.Columns.Add("IDNew", typeof(Int64));


                //填充工时表中的其它信息
                foreach (TechRouteImportList item in trils)
                {
                    r1 = WorkHour.NewRow();
                    r1["WH_TechRouteID"] = bc_techroute.Select("II_Code = '" + item.II_Code + "' and TRV_VersionCode = '" + item.TRV_VersionCode + " 'and TR_ProcessSequence = '" + item.TR_ProcessSequence + "'")[0]["ID"];
                    r1["WH_WorkHour"] = item.WH_WorkHour;
                    r1["WH_StartDate"] = item.WH_StartDate;
                    r1["WH_EndDate"] = item.WH_EndDate;

                    WorkHour.Rows.Add(r1);
                    WorkHour.AcceptChanges();

                }


                string maxtime = "";
                //找到同一个料品版本同一个工序的行,并将开始时间的最大值提取出来
                foreach (TechRouteImportList item in trils)
                {
                    r1 = bc_techroute.Select("II_Code = '" + item.II_Code + "' and TRV_VersionCode = '" + item.TRV_VersionCode + " 'and TR_ProcessSequence = '" + item.TR_ProcessSequence + "'")[0];

                    string SQl = string.Format(@"select MAX(WH_StartDate) as maxtime from [WorkHour] where WH_TechRouteID =  {0}", r1["ID"]);

                    MyDBController.GetConnection();

                    maxtime = MyDBController.ExecuteScalar(SQl).ToString();

                    MyDBController.CloseConnection();

                    //在工时表中找到开始时间最大值对应的结束时间,并将他改为导入开始时间的前一秒,并将这一行写入工时更新表中
                    r2 = bc_workhour.Select("WH_TechRouteID = " + r1["ID"] + " and WH_StartDate = '" + maxtime + "'")[0];

                    r3 = WorkHour.NewRow();

                    r3["ID"] = r2["ID"];
                    r3["WH_TechRouteID"] = r2["WH_TechRouteID"];
                    r3["WH_WorkHour"] = r2["WH_WorkHour"];
                    r3["WH_StartDate"] = r2["WH_StartDate"];
                    r3["WH_EndDate"] = DateTime.Parse(item.WH_StartDate).AddSeconds(-1);
                    r3["IDNew"] = r2["ID"];

                    WorkHour.Rows.Add(r3);
                    WorkHour.AcceptChanges();

                    //同时将改动后的工时更新到工艺路线表中
                    r4 = TechRoute.NewRow();

                    r4["ID"] = r1["ID"];
                    r4["TR_ItemID"] = r1["TR_ItemID"];
                    r4["TR_ItemCode"] = r1["TR_ItemCode"];
                    r4["TR_VersionID"] = r1["TR_VersionID"];
                    r4["TR_ProcessSequence"] = r1["TR_ProcessSequence"];
                    r4["TR_ProcessName"] = r1["TR_ProcessName"];
                    r4["TR_ProcessCode"] = r1["TR_ProcessCode"];
                    r4["TR_ProcessID"] = r1["TR_ProcessID"];
                    r4["TR_WorkHour"] = item.WH_WorkHour;
                    r4["TR_IsReportPoint"] = r1["TR_IsReportPoint"];
                    r4["TR_IsExProcess"] = r1["TR_IsExProcess"];
                    r4["TR_WorkCenterID"] = r1["TR_WorkCenterID"];
                    r4["TR_IsFirstProcess"] = r1["TR_IsFirstProcess"];
                    r4["TR_IsLastProcess"] = r1["TR_IsLastProcess"];
                    r4["TR_IsTestProcess"] = r1["TR_IsTestProcess"];
                    r4["TR_IsBackProcess"] = r1["TR_IsBackProcess"];
                    r4["TR_DefaultCheckPersonName"] = r1["TR_DefaultCheckPersonName"];
                    r4["TR_BindingProcess"] = r1["TR_BindingProcess"];
                    r4["TR_IsReportDevice"] = r1["TR_IsReportDevice"];
                    r4["TR_IsDeviceCharging"] = r1["TR_IsDeviceCharging"];
                    r4["IDNew"] = r1["ID"];

                    TechRoute.Rows.Add(r4);
                    TechRoute.AcceptChanges();


                    maxtime = "";
                }

                //插入更新工时表
                List<string> cloList = new List<string> { "ID", "WH_TechRouteID", "WH_WorkHour", "WH_StartDate", "WH_EndDate" };
                MyDBController.GetConnection();
                int updateNum, insertNum;
                MyDBController.InsertSqlBulk(WorkHour, cloList, out updateNum, out insertNum);
                MyDBController.CloseConnection();


                //更新工艺路线表
                List<string> cloList2 = new List<string> { 
                    "ID", 
                    "TR_ItemID", 
                    "TR_ItemCode", 
                    "TR_VersionID", 
                    "TR_ProcessSequence", 
                    "TR_ProcessName", 
                    "TR_ProcessCode", 
                    "TR_ProcessID", 
                    "TR_WorkHour", 
                    "TR_IsReportPoint", 
                    "TR_IsExProcess", 
                    "TR_WorkCenterID", 
                    "TR_IsFirstProcess", 
                    "TR_IsLastProcess", 
                    "TR_IsTestProcess", 
                    "TR_IsBackProcess", 
                    "TR_DefaultCheckPersonName", 
                    "TR_BindingProcess",
                    "TR_IsReportDevice", 
                    "TR_IsDeviceCharging" };

                MyDBController.GetConnection();
                MyDBController.InsertSqlBulk(TechRoute, cloList2, out updateNum, out insertNum);
                MyDBController.CloseConnection();



                //更新工艺路线查询表、工艺路线表、工时表
                GetBCTechRouteListForQuery();
                GetBCTechRouteList();
                GetBCWorkHourList();

                MessageBox.Show("导入成功", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
        }
        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Export_Click(object sender, RoutedEventArgs e)
        {
            DataTable templet = new DataTable();
            templet.TableName = "templet";

            templet.Columns.Add("料号", typeof(string));
            templet.Columns.Add("料名", typeof(string));
            templet.Columns.Add("规格", typeof(string));
            templet.Columns.Add("型号", typeof(string));

            templet.Columns.Add("版本编号", typeof(string));
            templet.Columns.Add("版本名称", typeof(string));

            templet.Columns.Add("工序号", typeof(int));
            templet.Columns.Add("工序名称", typeof(string));
            templet.Columns.Add("开始时间", typeof(string));
            templet.Columns.Add("工时", typeof(decimal));
            templet.Columns.Add("结束时间", typeof(string));



            if (listview1.Items.Count > 0)
            {
                foreach (TechRouteImportList item in trils)
                {

                    DataRow r1 = templet.NewRow();


                    r1["料号"] = item.II_Code;
                    r1["料名"] = item.II_Name;
                    r1["规格"] = item.II_Spec;
                    r1["型号"] = item.II_Version;
                    r1["版本编号"] = item.TRV_VersionCode;
                    r1["版本名称"] = item.TRV_VersionName;
                    r1["工序号"] = item.TR_ProcessSequence;
                    r1["工序名称"] = item.TR_ProcessName;
                    r1["开始时间"] = item.WH_StartDate;
                    r1["工时"] = item.WH_WorkHour;
                    r1["结束时间"] = item.WH_EndDate;

                    templet.Rows.Add(r1);
                    templet.AcceptChanges();

                }

               
            }

            QkRowChangeToColClass.CreateExcelFileForDataTable(templet);

            MessageBox.Show("导出成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

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
        //料品快捷查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TechRouteWorkHourImportItem_Window trwhiiw = new TechRouteWorkHourImportItem_Window();
            trwhiiw.ShowDialog();

            if ((bool)trwhiiw.DialogResult)
            {
                this.iil = trwhiiw.iil_return;

                txtb_ItemCode.Text = iil.II_Code;
            }
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Query_Click(object sender, RoutedEventArgs e)
        {


            trils.Clear();

            GetBCTechRouteListForQuery();

            int count = bc_techrouteforquery.Rows.Count;

            string dt = DateTime.Now.ToString("yyyy-MM-dd");

            for (int i = 0; i < count; i++)
            {

                TechRouteImportList tril = new TechRouteImportList();
                tril.Line_Number = i + 1;
                tril.II_Code = bc_techrouteforquery.Rows[i]["II_Code"].ToString();
                tril.II_Name = bc_techrouteforquery.Rows[i]["II_Name"].ToString();
                tril.II_Spec = bc_techrouteforquery.Rows[i]["II_Spec"].ToString();
                tril.II_Version = bc_techrouteforquery.Rows[i]["II_Version"].ToString();

                tril.TRV_VersionCode = bc_techrouteforquery.Rows[i]["TRV_VersionCode"].ToString();
                tril.TRV_VersionName = bc_techrouteforquery.Rows[i]["TRV_VersionName"].ToString();

                tril.TR_ProcessSequence = (int)bc_techrouteforquery.Rows[i]["TR_ProcessSequence"];
                tril.TR_ProcessCode = bc_techrouteforquery.Rows[i]["TR_ProcessCode"].ToString();
                tril.TR_ProcessName = bc_techrouteforquery.Rows[i]["TR_ProcessName"].ToString();

                tril.WH_StartDate = bc_techrouteforquery.Rows[i]["WH_StartDate"].ToString();
                tril.WH_WorkHour = (decimal)bc_techrouteforquery.Rows[i]["WH_WorkHour"];
                tril.WH_EndDate = bc_techrouteforquery.Rows[i]["WH_EndDate"].ToString();   

                trils.Add(tril);
            }

            listview1.ItemsSource = null;
            listview1.ItemsSource = trils;

            listview1.Items.Refresh();
        }

        /// <summary>
        /// 得到条码系统工艺路线,用于查询界面
        /// </summary>
        private void GetBCTechRouteListForQuery()
        {

            bc_techrouteforquery.Clear();

            MyDBController.GetConnection();

            string SQl = @"SELECT " +
                "C.II_Code," +
                "C.II_Name," +
                "C.II_Spec," +
                "C.II_Version," +
                "B.TRV_VersionCode," +
                "B.TRV_VersionName," +
                "A.TR_ProcessSequence," +
                "A.TR_ProcessCode," +
                "A.TR_ProcessName," +
                "E.WH_StartDate," +
                "E.WH_WorkHour," +
                "E.WH_EndDate " +
                "FROM [TechRoute] A " +
                "LEFT JOIN [TechRouteVersion] B ON A.[TR_ItemID]=B.[TRV_ItemID] AND A.[TR_VersionID]=B.[ID] " +
                "LEFT JOIN [ItemInfo] C ON A.[TR_ItemID]=C.[ID] " +
                "LEFT JOIN [WorkCenter] D ON A.[TR_WorkCenterID]=D.[WC_Department_ID] " +
                "LEFT JOIN [WorkHour] E ON A.ID =E.WH_TechRouteID " +
                "where E.[WH_StartDate] in (select MAX([WH_StartDate]) from [WorkHour] where [WH_TechRouteID]=E.[WH_TechRouteID]) ";

            if (txtb_ItemCode.Text.Trim().Length > 0)
            {
                SQl += " and C.II_Code = '" + txtb_ItemCode.Text.Trim() + "'";
            }

            if (txtb_ProcessName.Text.Trim().Length > 0)
            {
                SQl += " and A.TR_ProcessName = '" + txtb_ProcessName.Text.Trim() + "'";
            }
            if (txtb_WorkHour.Text.Trim().Length > 0)
            {
                SQl += " and E.WH_WorkHour = " + txtb_WorkHour.Text.Trim();
            }


                SQl += " ORDER BY A.[TR_ItemCode]," +
                        " B.[TRV_IsDefaultVer] desc," +
                        " A.[TR_ProcessSequence] ";




           bc_techrouteforquery = MyDBController.GetDataSet(SQl, ds, "bc_techrouteforquery").Tables["bc_techrouteforquery"];

            MyDBController.CloseConnection();
        }

        /// <summary>
        /// 得到条码系统工艺路线,只取到每道工序的最大工时行
        /// </summary>
        private void GetBCTechRouteList()
        {
            bc_techroute.Clear();

            MyDBController.GetConnection();

            string SQl = @"SELECT  " +
                "A.ID," +
                "C.II_Code," +
                "C.II_Name," +
                "C.II_Spec," +
                "C.II_Version," +
                "B.TRV_VersionCode," +
                "B.TRV_VersionName," +

                "A.TR_ItemID," +
                "A.TR_VersionID," +


                "A.TR_ProcessSequence," +
                "A.TR_ProcessCode," +
                "A.TR_ProcessName," +
                "A.TR_ItemCode," +
                "A.TR_ProcessID," +
                "A.TR_IsReportPoint," +
                "A.TR_IsExProcess," +
                "TR_IsTestProcess," +
                "A.TR_WorkCenterID," +
                "A.TR_IsFirstProcess," +
                "A.TR_IsLastProcess," +
                "A.TR_IsBackProcess," +
                "A.TR_DefaultCheckPersonName," +
                "A.TR_BindingProcess," +
                "A.TR_IsReportDevice," +
                "A.TR_IsDeviceCharging," +
                

                "E.WH_StartDate," +
                "E.WH_WorkHour," +
                "E.WH_EndDate " +
                "FROM [TechRoute] A  " +
                "LEFT JOIN [TechRouteVersion] B ON A.[TR_ItemID]=B.[TRV_ItemID] AND A.[TR_VersionID]=B.[ID] " +
                "LEFT JOIN [ItemInfo] C ON A.[TR_ItemID]=C.[ID] " +
                "LEFT JOIN [WorkCenter] D ON A.[TR_WorkCenterID]=D.[WC_Department_ID] " +
                "LEFT JOIN [WorkHour] E ON A.ID =E.WH_TechRouteID " +
                "where E.[WH_StartDate] in (select MAX([WH_StartDate]) from [WorkHour] where [WH_TechRouteID]=E.[WH_TechRouteID]) " +
                "ORDER BY A.[TR_ItemCode]," +
                "B.[TRV_IsDefaultVer] desc," +
                "A.[TR_ProcessSequence] ";

            bc_techroute = MyDBController.GetDataSet(SQl, ds, "bc_techroute").Tables["bc_techroute"];

            MyDBController.CloseConnection();
        }

        /// <summary>
        /// 获取条码系统工时表
        /// </summary>
        private void GetBCWorkHourList()
        {
            bc_workhour.Clear();

            MyDBController.GetConnection();

            string SQl = @"SELECT " +
                " ID," +
                " WH_TechRouteID," +
                " WH_WorkHour," +
                " WH_StartDate," +
                " WH_EndDate " +
                " from [WorkHour]";


            bc_workhour = MyDBController.GetDataSet(SQl, ds, "bc_workhour").Tables["bc_workhour"];

            MyDBController.CloseConnection();
        }


        /// <summary>
        /// 复制按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Copy_Click(object sender, RoutedEventArgs e)
        {
            listview1.ItemsSource = null;
            listview1.Items.Clear();
            listview1.Items.Refresh();

            trils.Clear();

            GetCopyTable();



            for (int i = 0; i < copiedData.Rows.Count; i++)
            {
                TechRouteImportList tril = new TechRouteImportList();
                tril.Line_Number = i + 1;
                tril.II_Code = copiedData.Rows[i]["II_Code"].ToString();
                tril.II_Name = copiedData.Rows[i]["II_Name"].ToString();
                tril.II_Spec = copiedData.Rows[i]["II_Spec"].ToString();
                tril.II_Version = copiedData.Rows[i]["II_Version"].ToString();

                tril.TRV_VersionCode = copiedData.Rows[i]["TRV_VersionCode"].ToString();
                tril.TRV_VersionName = copiedData.Rows[i]["TRV_VersionName"].ToString();

                tril.TR_ProcessSequence = (int)copiedData.Rows[i]["TR_ProcessSequence"];
                tril.TR_ProcessName = copiedData.Rows[i]["TR_ProcessName"].ToString();

                tril.WH_StartDate = copiedData.Rows[i]["WH_StartDate"].ToString();
                tril.WH_WorkHour = (decimal)copiedData.Rows[i]["WH_WorkHour"];
                tril.WH_EndDate = copiedData.Rows[i]["WH_EndDate"].ToString();

                trils.Add(tril);
            }

            listview1.ItemsSource = null;
            listview1.ItemsSource = trils;

            IsRight = false;
            IsEmpty = false;
        }


        /// <summary>
        /// 将excel 中数据复制到copiedData中
        /// </summary>
        private void GetCopyTable()
        {
            string columlist = "II_Code," +                 //料品名称
                              "II_Name," +                 //料品名称
                              "II_Spec," +                 //料品规格
                              "II_Version," +              //料品型号

                              "TRV_VersionCode," +              //工艺路线版本号
                              "TRV_VersionName," +          //工艺路线版本名称

                              "TR_ProcessSequence," +      //工序号
                              "TR_ProcessName," +          //工序名称
                              "WH_StartDate," +        //开始时间
                              "WH_WorkHour," +            //工时
                              "WH_EndDate";                 //结束时间

            copiedData = new DataTable("TechRouteImport");//用来显示的

            QkRowChangeToColClass qk = new QkRowChangeToColClass();

            copiedData.Columns.Add("II_Code", typeof(string));
            copiedData.Columns.Add("II_Name", typeof(string));
            copiedData.Columns.Add("II_Spec", typeof(string));
            copiedData.Columns.Add("II_Version", typeof(string));

            copiedData.Columns.Add("TRV_VersionCode", typeof(string));
            copiedData.Columns.Add("TRV_VersionName", typeof(string));


            copiedData.Columns.Add("TR_ProcessSequence", typeof(int));
            copiedData.Columns.Add("TR_ProcessName", typeof(string));

            copiedData.Columns.Add("WH_StartDate",typeof(string));
            copiedData.Columns.Add("WH_WorkHour", typeof(decimal));
            copiedData.Columns.Add("WH_EndDate",typeof(string));

            qk.write_excel_date_to_temp_table(copiedData, columlist);
        }


        /// <summary>
        /// 检测是否输入正确,能否导入
        /// </summary>
        private void CheckIfRight()
        {

            string maxtime = "";

            if (listview1.Items.Count > 0)
            {


                foreach (TechRouteImportList item in trils)
                {

                    if (string.IsNullOrEmpty(item.II_Code) || string.IsNullOrEmpty(item.TRV_VersionCode) || string.IsNullOrEmpty(item.TR_ProcessSequence.ToString()) || string.IsNullOrEmpty(item.WH_StartDate) || string.IsNullOrEmpty(item.WH_EndDate) || string.IsNullOrEmpty(item.WH_WorkHour.ToString()))
                    {
                        //料号、版本号、工序号、开始时间、结束时间、工时则不能为空
                        item.Error_Remarks = error[4];
                    }
                    else
                    {
                        if (bc_techroute.Select("II_Code = '" + item.II_Code + "' and TRV_VersionCode = '" + item.TRV_VersionCode + "' and TR_ProcessSequence = '" + item.TR_ProcessSequence + "'").Length > 0)
                        {

                            maxtime = bc_techroute.Select("II_Code = '" + item.II_Code + "' and TRV_VersionCode = '" + item.TRV_VersionCode + "' and TR_ProcessSequence = '" + item.TR_ProcessSequence + "'")[0]["WH_StartDate"].ToString();

                            string Input = Convert.ToDateTime(item.WH_StartDate).ToString("yyyy/MM/dd");
                            string max = Convert.ToDateTime(maxtime).ToString("yyyy/MM/dd");

                            DateTime input1 = Convert.ToDateTime(Input);
                            DateTime max1 = Convert.ToDateTime(max);

                            if (input1 <= max1)
                            {
                                //报错:工时的开始时间不能小于等于已有的开始时间
                                item.Error_Remarks = error[2];

                            }
                            else
                            {
                                //成功
                                IsRight = true;
                            }
                        }
                        else
                        {
                            //报错:条码系统中不存在符合该料品版本的工序
                            item.Error_Remarks = error[1];
                        }
                    }

                    maxtime = "";
                }
            }
            else
            {
                //报错:列表中没有数据或没有通过检测
                IsEmpty = true;
                MessageBox.Show(error[3], "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                             
            }

            if (!IsRight && !IsEmpty)
            {
                TechRouteWorkHourImportError_window trwhie = new TechRouteWorkHourImportError_window() { trils = this.trils };
                trwhie.ShowDialog();
            }
            
        }


    
    }
}
