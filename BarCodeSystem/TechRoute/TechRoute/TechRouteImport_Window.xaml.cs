using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.HelperClass;
using BarCodeSystem.PublicClass.ValueConverters;
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
    /// TechRouteImport_Window.xaml 的交互逻辑
    /// </summary>
    public partial class TechRouteImport_Window : Window
    {

        
        DataTable bar_iteminfo = new DataTable();
        DataTable bar_techrouteversion = new DataTable();
        DataTable bar_techroute = new DataTable();
        DataTable bar_workcenter = new DataTable();
        DataTable bar_workhour = new DataTable();
        DataTable bar_processname = new DataTable();


        DataTable U9ItemmasterModifiedOnTable;
        DataTable U9DepartmentTable;
        DataSet ds = new DataSet();


        //存放U9部门信息
        DataSet ds1 = new DataSet();


        //复制进来的数据，转化成制定格式的datatable 
        DataTable copiedData, fixedData;

        List<TechRouteImportList> trils = new List<TechRouteImportList> { };


        //数据是否通过检测
        bool IsRigth = true;

        //数据是否进行检测
        bool IsCheck = false;


        public TechRouteImport_Window()
        {
            InitializeComponent();
        }


        string[] error = {   "",                                            //0
                             "料号在条码系统中不存在!",                    //1
                             "车间编码在条码系统中不存在!",             //2
                             "是否默认版本,是否返工版本,是否测试工序应输入‘是’或‘否’",  //3
                             "报工方式应输入‘流水线’或‘离散’",    //4
                             "绑定工序不能为本道工序",                 //5
                             "绑定工序在后台工艺路线表中不存在",         //6
                             "绑定工序在条码系统中不存在",              //7
                             "数据不存在或没有通过检测",               //8
                             "请先复制数据!",                        //9
                             "绑定工序号不能为0或空",                  //10
                             "料品只有一个默认工艺路线版本",         //11
                             "一个工艺路线版本中不能出现重复的工序号",  //12
                             "工序在条码系统中不存在"          //13
                         };                        


        string[] Bool = { "是", "否" };
        string[] TRV_ReportWay = { "流水线", "离散" };

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
            GetBCSItemTechRoute();
        }
        /// <summary>
        /// 得到料品信息表、工作中心表、工艺路线表、工艺路线版本表信息
        /// </summary>
        private void GetBCSItemTechRoute()
        {
            MyDBController.GetConnection();

            GetU9_ItemInfo();
            GetU9_Department();
            

            GetBarCode_ItemInfo();
            GetBarCode_WorkCenter();
            GetBarCode_TechRouteVersion();
            GetBar_TechRoute();
            //GetBar_WorkHour();
            GetBar_ProcessName();

            MyDBController.CloseConnection();

        }

        /// <summary>
        /// 得到U9料品信息表
        /// </summary>
        private void GetU9_ItemInfo()
        {
            WebService.Service ws = new WebService.Service();
            ds = ws.GetItemmasterModifiedOnlist_ForMES(User_Info.User_Org_Code[0], "");
            U9ItemmasterModifiedOnTable = ds.Tables["U9ItemmasterModifiedOnTable"];

            
        }


        private void GetU9_Department()
        {
            WebService.Service ws = new WebService.Service();
            //由于调用webservice接口返回的是一个ds,故需要新建ds
            ds1 = ws.GetDepartmentlist_ForMES(User_Info.User_Org_Code[0], "", "");
            U9DepartmentTable = ds1.Tables["U9DepartmentTable"];

        }

        /// <summary>
        /// 得到条码系统料品信息
        /// </summary>
        private void GetBarCode_ItemInfo()
        {
            string SQl = @"select ID,II_Code,II_Spec,II_Version,II_Name,II_UnitID,II_UnitCode,II_UnitName,II_QualitySortID from ItemInfo";
            bar_iteminfo = MyDBController.GetDataSet(SQl, ds, "bar_iteminfo").Tables["bar_iteminfo"];
        }
        /// <summary>
        /// 得到条码系统工作中心
        /// </summary>
        private void GetBarCode_WorkCenter()
        {
            string SQl = @"SELECT " +
                    "ID," +
                    "WC_Department_Code," +
                    "WC_Department_Name," +
                    "WC_Department_ShortName," +
                    "WC_Department_ID," +
                    "WC_IsValidated, " +
                    "WC_IsOrderControled," +
                    "WC_IsWorkCenter," +
                    "WC_LastOperateTime," +
                    "WC_LastOprateBy," +
                    "WC_ReservedSegment1," +
                    "WC_ReservedSegment2," +
                    "WC_ReservedSegment3" +
                    " from WorkCenter";
                     bar_workcenter = MyDBController.GetDataSet(SQl, ds, "barcode_workcenter").Tables["barcode_workcenter"];
        }

        /// <summary>
        /// 得到条码系统工艺路线版本表
        /// </summary>
        private void GetBarCode_TechRouteVersion()
        {
            string SQl = @"SELECT " +
                " ID," +
                " TRV_ItemID," +
                " TRV_VersionCode," +
                " TRV_VersionName," +
                " TRV_ReportWay," +
                " TRV_IsBackVersion," +
                " TRV_IsDefaultVer ," +
                " TRV_IsValidated," +
                " TRV_IsSpecialVersion " +
                " from TechRouteVersion";
                bar_techrouteversion = MyDBController.GetDataSet(SQl, ds, "barcode_techrouteversion").Tables["barcode_techrouteversion"];
        }

        /// <summary>
        /// 得到条码系统工艺路线表
        /// </summary>
        private void GetBar_TechRoute()
        {
            string SQl = @"SELECT " +
                "  ID," +
                "  TR_ItemID," +
                "  TR_ItemCode," +
                "  TR_VersionID," +
                "  TR_ProcessSequence," +
                "  TR_ProcessName," +
                "  TR_ProcessCode," +
                "  TR_ProcessID," +
                "  TR_WorkHour," +
                "  TR_IsReportPoint," +
                "  TR_IsExProcess," +
                "  TR_WorkCenterID," +
                "  TR_IsFirstProcess," +
                "  TR_IsLastProcess," +
                "  TR_IsTestProcess," +
                "  TR_IsBackProcess," +
                "  TR_DefaultCheckPersonName," +
                "  TR_BindingProcess," +
                "  TR_IsReportDevice," +
                "  TR_IsDeviceCharging " +
                "  from TechRoute";

            bar_techroute = MyDBController.GetDataSet(SQl, ds, "barcode_techroute").Tables["barcode_techroute"];
        }


        /// <summary>
        /// 得到条码系统工时价表
        /// </summary>
        private void GetBar_WorkHour()
        {
            string SQl = @"SELECT " +
                "ID," +
                "WH_TechRouteID," +
                "WH_WorkHour," +
                "WH_StartDate," +
                "WH_EndDate " +
                " from WorkHour";


            bar_workhour = MyDBController.GetDataSet(SQl, ds, "barcode_workhour").Tables["barcode_workhour"];
        }

        /// <summary>
        /// 得到条码系统工序表
        /// </summary>
        private void GetBar_ProcessName()
        {
            string SQl = @"SELECT " +
                    " ID," +
                    " PN_Code," +
                    " PN_Name " +
                    " from ProcessName";

            bar_processname = MyDBController.GetDataSet(SQl, ds, "bar_processname").Tables["bar_processname"];

        }



        

        /// <summary>
        /// 复制按钮,将excel中的数据复制到listview1中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Copy_Click(object sender, RoutedEventArgs e)
        {
            listview1.ItemsSource = null;
            listview1.Items.Clear();
            listview1.Items.Refresh();
            trils.Clear();

            IsRigth = true;
            IsCheck = false;
            GetCopyTable();

            

            for (int i = 0; i < copiedData.Rows.Count; i++)
            {

                TechRouteImportList tril = new TechRouteImportList();

                tril.Line_Number = i+1;

                tril.II_Code = copiedData.Rows[i]["II_Code"].ToString();
                tril.II_Name = copiedData.Rows[i]["II_Name"].ToString();
                tril.II_Spec = copiedData.Rows[i]["II_Spec"].ToString();
                tril.II_Version = copiedData.Rows[i]["II_Version"].ToString();


                tril.TRV_VersionCode = copiedData.Rows[i]["TRV_VersionCode"].ToString();
                tril.TRV_VersionName = copiedData.Rows[i]["TRV_VersionName"].ToString();
                tril.TRV_IsDefaultVer = copiedData.Rows[i]["TRV_IsDefaultVer"].ToString();
                tril.TRV_ReportWay = copiedData.Rows[i]["TRV_ReportWay"].ToString();
                tril.TRV_IsBackVersion = copiedData.Rows[i]["TRV_IsBackVersion"].ToString();

                tril.WC_Department_Code = copiedData.Rows[i]["WC_Department_Code"].ToString();
                tril.WC_Department_Name = copiedData.Rows[i]["WC_Department_Name"].ToString();

                tril.TR_ProcessSequence = (int)copiedData.Rows[i]["TR_ProcessSequence"];
                tril.TR_ProcessCode = copiedData.Rows[i]["TR_ProcessCode"].ToString();
                tril.TR_ProcessName = copiedData.Rows[i]["TR_ProcessName"].ToString();
                tril.TR_IsTestProcess = copiedData.Rows[i]["TR_IsTestProcess"].ToString();
                tril.TR_DefaultCheckPersonName = copiedData.Rows[i]["TR_DefaultCheckPersonName"].ToString();
                tril.TR_BindingProcess = (int)copiedData.Rows[i]["TR_BindingProcess"];
                tril.WH_WorkHour = (decimal)copiedData.Rows[i]["WH_WorkHour"];

                trils.Add(tril);

            }
            listview1.ItemsSource = null;
            listview1.ItemsSource = trils;

            IsCheck = false;

        }


        /// <summary>
        /// 检测料号、车间编码是否存在于条码系统和U9系统
        /// 检测是否默认版本,是否返工版本,是否测试工序,输入数据是否为"是"或"否"
        /// 检测包工方式,输入是否我"流水线"或"离散"
        /// 检测绑定工序是否为该料品此道工序以外的工序
        /// 检测条码系统中是否存在此工序号,不存在则插入
        /// </summary>
        private void CheckIfRight()
        {

            //listview1中是否有数据
            if (listview1.Items.Count > 0)
            {

                foreach (TechRouteImportList item in trils)
                {
                    List<TechRouteImportList> test = new List<TechRouteImportList> { };
                    //判断料品的只有一个默认工艺路线版本

                    if (item.TRV_IsDefaultVer.Equals("是"))
                    {
                        test = trils.FindAll(P => P.II_Code == item.II_Code && P.TRV_VersionCode != item.TRV_VersionCode && P.TRV_IsDefaultVer.Equals("是"));

                        if (test.Count > 0)
                        {
                            item.Error_Remarks = error[11];
                            IsRigth = false;
                        }
                    }

                    //判断同一个工艺路线中是否出现相同的版本号
                    test.Clear();

                    test = trils.FindAll(P => P.II_Code ==item.II_Code &&  P.TRV_VersionCode == item.TRV_VersionCode && P.TR_ProcessSequence == item.TR_ProcessSequence );

                    if (test.Count > 1)
                    {
                        item.Error_Remarks = error[12];
                        IsRigth = false;
                    }



                    //检测条码系统中是否存在此料号
                    if (bar_iteminfo.Select("II_Code = '" + item.II_Code + "'").Length == 0)
                    {
                        //U9中是否存在此料号,存在则插入条码系统,不存在则报错
                        //if (U9ItemmasterModifiedOnTable.Select("itemmaster_code = '" + item.II_Code + "'").Length > 0)
                        //{
                            //MyDBController.GetConnection();
                            //DataRow r1 = U9ItemmasterModifiedOnTable.Select("itemmaster_code = '" + item.II_Code + "'")[0];
                            //string SQl = string.Format(@"INSERT INTO [ItemInfo](II_Code,II_Spec,II_Version,II_Name,II_UnitID,II_UnitCode,II_UnitName,II_QualitySortID)
                            //VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", r1["itemmaster_code"].ToString(), r1["itemmaster_specs"].ToString(), r1["itemmaster_descflexfield"].ToString(),
                            //r1["itemmaster_name"].ToString(),r1["itemmaster_uom_id"], r1["itemmaster_uom_code"].ToString(), r1["itemmaster_uom"].ToString(), "");

                            //MyDBController.ExecuteNonQuery(SQl);
                            //MyDBController.CloseConnection();
                            //DBLog _dbLog = new DBLog();
                            //_dbLog.DBL_OperateBy = User_Info.User_Code;
                            //_dbLog.DBL_OperateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            //_dbLog.DBL_OperateType = OperateType.Insert;
                            //_dbLog.DBL_Content = User_Info.User_Name + "|新增料品信息" + "|" + User_Info.User_WorkcenterName + "|" + User_Info.P_Position + "。新增的料品编号记录在DBL_AssociateCode";
                            //_dbLog.DBL_AssociateCode = r1["itemmaster_code"].ToString();
                            //_dbLog.DBL_OperateTable = "ItemInfo";
                            //DBLog.WriteDBLog(_dbLog);

                            ////每次新料品插入后更新
                            //MyDBController.GetConnection();
                            //bar_iteminfo.Clear();
                            //GetBarCode_ItemInfo();
                        //    //MyDBController.CloseConnection();
                        //}
                        //else
                        //{

                        //}

                        item.Error_Remarks = error[1];
                        IsRigth = false;
                    }
                    //检测条码系统中是否存在此车间编码
                    if (bar_workcenter.Select("WC_Department_Code = '" + item.WC_Department_Code + "'").Length == 0)
                    {
                        //U9系统中是否存在此车间编码,存在则插入条码系统,不存在则报错
                        //if (U9DepartmentTable.Select("department_code = '" + item.WC_Department_Code + "'").Length > 0)
                        //{
//                            MyDBController.GetConnection();
//                            DataRow r1 = U9DepartmentTable.Select("department_code = '" + item.WC_Department_Code + "'")[0];

//                            string x = User_Info.User_Name;
//                            string y = DateTime.Now.ToString();

//                            string SQl = string.Format(@"INSERT INTO [WorkCenter](WC_Department_Code,WC_Department_Name,WC_Department_ShortName,WC_Department_ID,WC_IsValidated,WC_IsOrderControled,
//                        WC_IsWorkCenter,WC_LastOperateTime,WC_LastOprateBy,WC_ReservedSegment1,WC_ReservedSegment2,WC_ReservedSegment3)
//                        VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')", r1["department_code"].ToString(), r1["department_name"].ToString(), "", r1["department_id"],
//                            0, 0, 0, DateTime.Now, User_Info.User_Name,"","","");

//                            MyDBController.ExecuteNonQuery(SQl);
//                            MyDBController.CloseConnection();

//                            DBLog _dbLog = new DBLog();
//                            _dbLog.DBL_OperateBy = User_Info.User_Code;
//                            _dbLog.DBL_OperateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
//                            _dbLog.DBL_OperateType = OperateType.Insert;
//                            _dbLog.DBL_Content = User_Info.User_Name + "|新增工作中心信息" + "|" + User_Info.User_WorkcenterName + "|" + User_Info.P_Position + "。新增的工作中心编号记录在DBL_AssociateCode";
//                            _dbLog.DBL_AssociateCode = r1["department_code"].ToString();
//                            _dbLog.DBL_OperateTable = "WorkCenter";
//                            DBLog.WriteDBLog(_dbLog);
//                            //每次插入工作部门后更新

//                            MyDBController.GetConnection();
//                            bar_workcenter.Clear();
//                            GetBarCode_WorkCenter();
//                            MyDBController.CloseConnection();
                        //}
                        //else
                        //{
                        //    item.Error_Remarks = error[2];
                        //    IsRigth = false;
                        //}

                        item.Error_Remarks = error[2];
                        IsRigth = false;

                    }
                    //检测 是否默认版本,是否返工版本,是否测试工序应输入‘是’或‘否’
                    if (!Bool.Contains(item.TRV_IsDefaultVer) || !Bool.Contains(item.TRV_IsBackVersion) || !Bool.Contains(item.TR_IsTestProcess))
                    {
                        item.Error_Remarks = error[3];
                        IsRigth = false;
                    }
                    //检测报工方式应输入‘流水线’或‘离散’
                    if (!TRV_ReportWay.Contains(item.TRV_ReportWay))
                    {
                        item.Error_Remarks = error[4];
                        IsRigth = false;
                    }



                    //检测工序号在条码系统中是否存在,存在则更新,不存在插入
                    if (bar_processname.Select("PN_Code ='" + item.TR_ProcessCode + "'").Length == 0)
                    {
                        //MyDBController.GetConnection();

                        //string SQl = string.Format(@"INSERT INTO [ProcessName](PN_Code,PN_Name)VALUES('{0}','{1}')", item.TR_ProcessCode, item.TR_ProcessName);
                        //MyDBController.ExecuteNonQuery(SQl);

                        ////每次插入工序后更新
                        //bar_processname.Clear();
                        //GetBar_ProcessName();

                        //MyDBController.CloseConnection();
                        //DBLog _dbLog = new DBLog();
                        //_dbLog.DBL_OperateBy = User_Info.User_Code;
                        //_dbLog.DBL_OperateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        //_dbLog.DBL_OperateType = OperateType.Insert;
                        //_dbLog.DBL_Content = User_Info.User_Name + "|新增工序信息" + "|" + User_Info.User_WorkcenterName + "|" + User_Info.P_Position + "。新增的工序信息记录在DBL_AssociateCode";
                        //_dbLog.DBL_AssociateCode = item.TR_ProcessCode + "|" + item.TR_ProcessName;
                        //_dbLog.DBL_OperateTable = "WorkCenter";
                        //DBLog.WriteDBLog(_dbLog);

                        item.Error_Remarks = error[13];
                        IsRigth = false;
                    }


                    //检测绑定工序号:检测条件:绑定工序号不能为0
                    if (item.TR_BindingProcess != 0)
                    {

                        int count = copiedData.Select("II_Code = '" + item.II_Code + "' and TRV_VersionCode = '" + item.TRV_VersionCode + "' and TR_ProcessSequence = '" + item.TR_BindingProcess + "'").Length;

                        //绑定工序在中间表中不存在,并且在数据库中也不存在则报错
                        if (count == 0)
                        {
                            Int64 TRV_ItemID = (Int64)bar_iteminfo.Select("II_Code = '" + item.II_Code + "'")[0]["ID"];
                            Int64 TR_VersionID = 0;
                                if(bar_techrouteversion.Select("TRV_ItemID = '" + TRV_ItemID + "' and  TRV_VersionCode = '" + item.TRV_VersionCode + "'").Length > 0)
                                {
                                    TR_VersionID = (Int64)bar_techrouteversion.Select("TRV_ItemID = '" + TRV_ItemID + "' and  TRV_VersionCode = '" + item.TRV_VersionCode + "'")[0]["ID"];
                                }
                            int count2 = bar_techroute.Select("TR_ItemCode = '" + item.II_Code + "'and TR_VersionID = '" + TR_VersionID + "'and TR_ProcessSequence = '" + item.TR_BindingProcess + "'").Length;
                            if (count2 == 0)
                            {
                                item.Error_Remarks = error[6];
                                IsRigth = false;
                            }
                        }
                        else
                        {
                            //绑定工序和此道工序号相同则报错
                            if (item.TR_BindingProcess == item.TR_ProcessSequence)
                            {
                                item.Error_Remarks = error[5];
                                IsRigth = false;
                            }                           
                        }

                    }
                    else
                    { 

                    }

                }

                if (IsRigth)
                {
                    MessageBox.Show("检测完成:暂未发现格式错误!");

                    IsCheck = true;
                }
                else
                {
                    TechRouteImportError_Windows trie = new TechRouteImportError_Windows(){trils = this.trils};
                    trie.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show(error[8], "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                IsRigth = false;
            }
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
                               "TRV_IsDefaultVer," +         //工艺路线是否默认版本
                               "TRV_ReportWay," +            //报工方式 0:流水线 1:离散
                               "TRV_IsBackVersion," +       //是否返工版本

                               "WC_Department_Code," +      //工作中心编码
                               "WC_Department_Name," +      //工作中心名称

                               "TR_ProcessSequence," +      //工序号
                               "TR_ProcessCode," +          //工序编码
                               "TR_ProcessName," +          //工序名称
                               "TR_IsTestProcess," +        //是否测试工序
                               "TR_DefaultCheckPersonName," + //默认检验员
                               "TR_BindingProcess," +          //绑定工序
                               "WH_WorkHour";             //工时



            copiedData = new DataTable("TechRouteImport");//用来显示的
            fixedData = new DataTable("TechRouteImport2");
            QkRowChangeToColClass qk = new QkRowChangeToColClass();


            copiedData.Columns.Add("II_Code", typeof(string));
            copiedData.Columns.Add("II_Name", typeof(string));
            copiedData.Columns.Add("II_Spec", typeof(string));
            copiedData.Columns.Add("II_Version", typeof(string));

            copiedData.Columns.Add("TRV_VersionCode", typeof(string));
            copiedData.Columns.Add("TRV_VersionName", typeof(string));
            copiedData.Columns.Add("TRV_IsDefaultVer", typeof(string));
            copiedData.Columns.Add("TRV_ReportWay", typeof(string));
            copiedData.Columns.Add("TRV_IsBackVersion", typeof(string));


            copiedData.Columns.Add("WC_Department_Code", typeof(string));
            copiedData.Columns.Add("WC_Department_Name", typeof(string));


            copiedData.Columns.Add("TR_ProcessSequence", typeof(int));
            copiedData.Columns.Add("TR_ProcessCode", typeof(string));
            copiedData.Columns.Add("TR_ProcessName", typeof(string));
            copiedData.Columns.Add("TR_IsTestProcess", typeof(string));
            copiedData.Columns.Add("TR_DefaultCheckPersonName", typeof(string));
            copiedData.Columns.Add("TR_BindingProcess", typeof(int));
            copiedData.Columns.Add("WH_WorkHour", typeof(decimal));




            qk.write_excel_date_to_temp_table(copiedData, columlist);
        }
        /// <summary>
        /// 导入按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Import_Click(object sender, RoutedEventArgs e)
        {
            //导入前先检测
            //btn_Check_Click(sender, e);


            this.Cursor = Cursors.Wait;

            DataRow r1, r2, r3,r4;
            if (IsRigth  && IsCheck)
            {
                //新建工艺路线版本表,未去除重复版
                DataTable TechRouteVersion_1 = new DataTable("TechRouteVersion_1");
                TechRouteVersion_1.Columns.Add("ID", typeof(Int64));                      //
                TechRouteVersion_1.Columns.Add("TRV_ItemID", typeof(Int64));              //
                TechRouteVersion_1.Columns.Add("TRV_VersionCode", typeof(string));
                TechRouteVersion_1.Columns.Add("TRV_VersionName", typeof(string));
                TechRouteVersion_1.Columns.Add("TRV_ReportWay", typeof(int));
                TechRouteVersion_1.Columns.Add("TRV_IsDefaultVer", typeof(bool));
                TechRouteVersion_1.Columns.Add("TRV_IsValidated", typeof(bool));
                TechRouteVersion_1.Columns.Add("TRV_IsBackVersion", typeof(bool));
                TechRouteVersion_1.Columns.Add("TRV_IsSpecialVersion", typeof(bool));
                TechRouteVersion_1.Columns.Add("IDNew", typeof(Int64));
                


                //新建工艺路线表
                DataTable TechRoute = new DataTable("TechRoute");
                TechRoute.Columns.Add("ID", typeof(Int64));                          //
                TechRoute.Columns.Add("TR_ItemID", typeof(Int64));
                TechRoute.Columns.Add("TR_ItemCode", typeof(string));
                TechRoute.Columns.Add("TR_VersionID", typeof(Int64));               //
                TechRoute.Columns.Add("TR_ProcessSequence", typeof(int));
                TechRoute.Columns.Add("TR_ProcessName", typeof(string));
                TechRoute.Columns.Add("TR_ProcessCode", typeof(string));
                TechRoute.Columns.Add("TR_ProcessID", typeof(Int64));               //
                TechRoute.Columns.Add("TR_WorkHour", typeof(decimal));
                TechRoute.Columns.Add("TR_IsReportPoint", typeof(bool));
                TechRoute.Columns.Add("TR_IsExProcess", typeof(bool));
                TechRoute.Columns.Add("TR_WorkCenterID", typeof(Int64));            //
                TechRoute.Columns.Add("TR_IsFirstProcess", typeof(bool));
                TechRoute.Columns.Add("TR_IsLastProcess", typeof(bool));
                TechRoute.Columns.Add("TR_IsTestProcess", typeof(bool));
                TechRoute.Columns.Add("TR_IsBackProcess", typeof(bool));
                TechRoute.Columns.Add("TR_DefaultCheckPersonName", typeof(string));
                TechRoute.Columns.Add("TR_BindingProcess", typeof(int));
                TechRoute.Columns.Add("TR_IsReportDevice", typeof(bool));
                TechRoute.Columns.Add("TR_IsDeviceCharging", typeof(bool));
                TechRoute.Columns.Add("IDNew", typeof(Int64));

                //新建价目表
                DataTable WorkHour = new DataTable("WorkHour");
                WorkHour.Columns.Add("ID", typeof(Int64));                  //
                WorkHour.Columns.Add("WH_TechRouteID", typeof(Int64));      //
                WorkHour.Columns.Add("WH_WorkHour", typeof(decimal));
                WorkHour.Columns.Add("WH_StartDate", typeof(DateTime));
                WorkHour.Columns.Add("WH_EndDate", typeof(DateTime));
                WorkHour.Columns.Add("IDNew", typeof(Int64));


                ReportWayConverter rwc = new ReportWayConverter();
                foreach (TechRouteImportList item in trils)
                {
                    r1 = TechRouteVersion_1.NewRow();
                    r1["TRV_VersionCode"] = item.TRV_VersionCode;
                    r1["TRV_VersionName"] = item.TRV_VersionName;
                    r1["TRV_ReportWay"] = rwc.ConvertBack(item.TRV_ReportWay, typeof(int), null, new System.Globalization.CultureInfo(""));
                    r1["TRV_IsDefaultVer"] = rwc.ConvertStringBack(item.TRV_IsDefaultVer, typeof(bool), null, new System.Globalization.CultureInfo(""));
                    r1["TRV_IsValidated"] = 0;
                    r1["TRV_IsBackVersion"] = rwc.ConvertStringBack(item.TRV_IsBackVersion, typeof(bool), null, new System.Globalization.CultureInfo(""));
                    r1["TRV_IsSpecialVersion"] = 0;

                    TechRouteVersion_1.Rows.Add(r1);
                    TechRouteVersion_1.AcceptChanges();

                    r2 = TechRoute.NewRow();
                    r2["TR_ItemCode"] = item.II_Code;
                    r2["TR_ProcessSequence"] = item.TR_ProcessSequence;
                    r2["TR_ProcessName"] = item.TR_ProcessName;
                    r2["TR_ProcessCode"] = item.TR_ProcessCode;
                    r2["TR_WorkHour"] = item.WH_WorkHour;
                    r2["TR_IsReportPoint"] = 1;
                    r2["TR_IsExProcess"] = 0;
                    r2["TR_IsFirstProcess"] = 0;
                    r2["TR_IsLastProcess"] = 0;
                    r2["TR_IsTestProcess"] = rwc.ConvertStringBack(item.TR_IsTestProcess, typeof(bool), null, new System.Globalization.CultureInfo(""));
                    r2["TR_IsBackProcess"] = 0;
                    r2["TR_DefaultCheckPersonName"] = item.TR_DefaultCheckPersonName;
                    r2["TR_BindingProcess"] = item.TR_BindingProcess;
                    r2["TR_IsReportDevice"] = 0;
                    r2["TR_IsDeviceCharging"] = 0;
                    TechRoute.Rows.Add(r2);
                    TechRoute.AcceptChanges();

                    r3 = WorkHour.NewRow();
                    //r3["WH_StartDate"] = DateTime.Now;
                    r3["WH_StartDate"] = DateTime.Now.ToString("yyyy-MM-dd");
                    r3["WH_EndDate"] = DateTime.Parse("9999-01-01");
                    r3["WH_WorkHour"] = item.WH_WorkHour;
                    WorkHour.Rows.Add(r3);
                    WorkHour.AcceptChanges();                 

                }


                //填充工艺路线版本表中TRV_ItemID,ID.
                int count = TechRouteVersion_1.Rows.Count;

                for (int i = 0; i < count; i++)
                {
                    r1 = TechRouteVersion_1.Rows[i];
                 
                    //r2 = bar_iteminfo.Select("II_Code = '" + r1["II_Code"].ToString() + "'")[0];
                    //r2 = copiedData.Select("II_Code = '" + r1["II_Code"].ToString() + "'")[i];

                    r2 = copiedData.Rows[i];

                    r1["TRV_ItemID"] = bar_iteminfo.Select("II_Code = '" + r2["II_Code"] + "'")[0]["ID"];

                    //在条码系统中寻找是否有相同工艺路线版本号信息,有则将ID提取出来
                    if (bar_techrouteversion.Select("TRV_VersionCode = '" + r1["TRV_VersionCode"] + "' and TRV_ItemID ='" + r1["TRV_ItemID"] + "'").Length > 0)
                    {
                        r1["ID"] = bar_techrouteversion.Select("TRV_VersionCode = '" + r1["TRV_VersionCode"] + "' and TRV_ItemID ='" + r1["TRV_ItemID"] + "'")[0]["ID"];
                        r1["IDNew"] = r1["ID"];
                    }
                    
                }


                //去掉工艺路线版本表里的重复行
                //Int64 flage_itemid = 0;
                //string flag_versioncode = "";

                //for (int i = 0; i < TechRouteVersion.Rows.Count; i++)
                //{
                //    r1 = TechRouteVersion.Rows[i];
                //    if (i == 0)
                //    {
                //        flag_versioncode = TechRouteVersion.Rows[i]["TRV_VersionCode"].ToString();
                //        flage_itemid = (Int64)TechRouteVersion.Rows[i]["TRV_ItemID"];
                //    }
                //    else
                //    {
                //        if (flag_versioncode.Equals(r1["TRV_VersionCode"]) && (flage_itemid == (Int64)r1["TRV_ItemID"]))
                //        {
                //            TechRouteVersion.Rows[i].Delete();
                //            //TechRouteVersion.Rows.RemoveAt(i);
                //            //TechRouteVersion.AcceptChanges();
                //        }
                //        else
                //        {
                //            flag_versioncode = TechRouteVersion.Rows[i]["TRV_VersionCode"].ToString();
                //            flage_itemid = (Int64)TechRouteVersion.Rows[i]["TRV_ItemID"];
                //        }
                //    }
                //}
                //TechRouteVersion.AcceptChanges();


                //去掉工艺路线版本里的重复行
                DataView dv = new DataView(TechRouteVersion_1);
                DataTable TechRouteVersion = dv.ToTable(true, "ID", "TRV_ItemID", "TRV_VersionCode", "TRV_VersionName", "TRV_ReportWay", "TRV_IsDefaultVer", "TRV_IsValidated", "TRV_IsBackVersion", "TRV_IsSpecialVersion", "IDNew");
                TechRouteVersion.TableName = "TechRouteVersion";

               //对工艺路线表中是否默认版本进行修改
                count = TechRouteVersion.Rows.Count;

                for (int i = 0; i < count; i++)
                {
                    r1 = TechRouteVersion.Rows[i];

                    //如果中间表和数据表中同时存在默认版本的情况,将数据表中的默认版本取消.
                    if ((bool)r1["TRV_IsDefaultVer"] == true)
                    {
                        if (bar_techrouteversion.Select("TRV_ItemID = '" + r1["TRV_ItemID"] + "' and TRV_IsDefaultVer = true ").Length > 0)
                        {
                            r3 = bar_techrouteversion.Select("TRV_ItemID = '" + r1["TRV_ItemID"]  + "' and TRV_IsDefaultVer = true " )[0];

                            if (!r3["TRV_VersionCode"].Equals(r1["TRV_VersionCode"]))
                            {
                                r2 = TechRouteVersion.NewRow();

                                r2["ID"] = r3["ID"];
                                r2["TRV_ItemID"] = r3["TRV_ItemID"];
                                r2["TRV_VersionCode"] = r3["TRV_VersionCode"];
                                r2["TRV_VersionName"] = r3["TRV_VersionName"];
                                r2["TRV_ReportWay"] = r3["TRV_ReportWay"];
                                r2["TRV_IsDefaultVer"] = 0;
                                r2["TRV_IsValidated"] = r3["TRV_IsValidated"];
                                r2["TRV_IsBackVersion"] = r3["TRV_IsBackVersion"];
                                r2["TRV_IsSpecialVersion"] = r3["TRV_IsSpecialVersion"];
                                r2["IDNew"] = r3["ID"];

                                TechRouteVersion.Rows.Add(r2);
                                TechRouteVersion.AcceptChanges();
                            }
                        }
                    }
                }

                List<string> cloList = new List<string> { "ID", "TRV_ItemID", "TRV_VersionCode", "TRV_VersionName", "TRV_ReportWay", "TRV_IsDefaultVer", "TRV_IsValidated", "TRV_IsBackVersion", "TRV_IsSpecialVersion"};



                MyDBController.GetConnection();
                int updateNum, insertNum;
                MyDBController.InsertSqlBulk(TechRouteVersion, cloList, out updateNum, out insertNum);
                MyDBController.CloseConnection();
                

               
                //更新条码系统工艺路线版本表、部门表、更新工序表

                MyDBController.GetConnection();
                bar_techrouteversion.Clear();
                GetBarCode_TechRouteVersion();


                MyDBController.CloseConnection();


               
                count = TechRoute.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    r1 = TechRoute.Rows[i];

                    r2 = copiedData.Rows[i];

                    r3 = bar_iteminfo.Select("II_Code = '" + r2["II_Code"] + "'")[0];

                    //得到料品ID
                    r1["TR_ItemID"] = r3["ID"];

                    //得到版本ID
                    r1["TR_VersionID"] = bar_techrouteversion.Select("TRV_ItemID = '" + r3["ID"] + "' and TRV_VersionCode = '" + r2["TRV_VersionCode"] + "'")[0]["ID"];

                    //得到工序ID
                    r1["TR_ProcessID"] = bar_processname.Select("PN_Code = '" + r1["TR_ProcessCode"] + "'")[0]["ID"];


                    //得到工作中心ID,这个ID是工作中心在条码系统中的ID
                    r1["TR_WorkCenterID"] = bar_workcenter.Select("WC_Department_Code = '" + r2["WC_Department_Code"] + "'")[0]["WC_Department_ID"];

                    //检测工艺路线ID,有则返回.
                    if (bar_techroute.Select("TR_ItemCode = '" + r1["TR_ItemCode"] + "' and TR_VersionID = '" + r1["TR_VersionID"] + "' and TR_ProcessSequence = '" + r1["TR_ProcessSequence"] + "'").Length > 0)
                    {
                        r1["ID"] = bar_techroute.Select("TR_ItemCode = '" + r1["TR_ItemCode"] + "' and TR_VersionID = '" + r1["TR_VersionID"] + "' and TR_ProcessSequence = '" + r1["TR_ProcessSequence"] + "'")[0]["ID"];
                        r1["IDNew"] = r1["ID"];
                    }

                }




                //工艺路线表首、末道工序判断
                count = TechRouteVersion.Rows.Count;
                int maxprocess = 0;
                int minprocess = 1000;
                int count2 = 0;
                for (int i = 0; i < count; i++)
                {

                    r1 = TechRouteVersion.Rows[i];
                    //在条码系统中寻找是否有相同工艺路线版本号信息,有则将ID提取出来
                    if (bar_techrouteversion.Select("TRV_VersionCode = '" + r1["TRV_VersionCode"] + "' and TRV_ItemID ='" + r1["TRV_ItemID"] + "'").Length > 0)
                    {
                        r1["ID"] = bar_techrouteversion.Select("TRV_VersionCode = '" + r1["TRV_VersionCode"] + "' and TRV_ItemID ='" + r1["TRV_ItemID"] + "'")[0]["ID"];

                        //在中间表中寻找版本ID相同的工艺路线,并找到最大和最小值
                        count2 = TechRoute.Select("TR_VersionID = '" + r1["ID"] + "'").Length;

                        for (int j = 0; j < count2; j++)
                        {
                            r2 = TechRoute.Select("TR_VersionID = '" + r1["ID"] + "'")[j];

                            if ((int)r2["TR_ProcessSequence"] >= maxprocess)
                            {
                                maxprocess = (int)r2["TR_ProcessSequence"];
                            }

                            if ((int)r2["TR_ProcessSequence"] <= minprocess)
                            {
                                minprocess = (int)r2["TR_ProcessSequence"];
                            }
                        }

                        TechRoute.Select("TR_VersionID = '" + r1["ID"] + "' and  TR_ProcessSequence = '" + maxprocess + "'")[0]["TR_IsLastProcess"] = true;
                        TechRoute.Select("TR_VersionID = '" + r1["ID"] + "' and  TR_ProcessSequence = '" + minprocess + "'")[0]["TR_IsFirstProcess"] = true;


                        maxprocess = 0;
                        minprocess = 1000;
                    }
                }

                //中间表中的工序和条码系统中的工序比较,检测是否为首、末道工序

                count = TechRouteVersion.Rows.Count;

                for (int i = 0; i < count; i++)
                {
                    r1 = TechRouteVersion.Rows[i];

                    //当工艺路线表中存在此工艺路线时判断首、末道工序.
                    if (bar_techroute.Select("TR_VersionID = '" + r1["ID"] + "'").Length > 0)
                    {
                        string SQl = string.Format(@"select  MAX(TR_ProcessSequence) as maxprocesssequence from TechRoute where TR_VersionID = {0}", r1["ID"]);
                        MyDBController.GetConnection();

                        maxprocess = (int)MyDBController.ExecuteScalar(SQl);

                        SQl = string.Format(@"select  MIN(TR_ProcessSequence) as minprocesssequence from TechRoute where TR_VersionID = {0}", r1["ID"]);

                        minprocess = (int)MyDBController.ExecuteScalar(SQl);

                        MyDBController.CloseConnection();

                        r2 = TechRoute.Select("TR_VersionID = '" + r1["ID"] + "' and TR_IsFirstProcess = true")[0];

                        if ((int)r2["TR_ProcessSequence"] > minprocess)
                        {
                            r2["TR_IsFirstProcess"] = false;
                        }
                        else if ((int)r2["TR_ProcessSequence"] < minprocess)
                        {
                            r3 = bar_techroute.Select("TR_VersionID = '" + r1["ID"] + "'and TR_ProcessSequence = '" + minprocess + "'")[0];


                            r4 = TechRoute.NewRow() ;
                            r4["ID"] = r3["ID"];
                            r4["TR_ItemID"] = r3["TR_ItemID"];
                            r4["TR_ItemCode"] = r3["TR_ItemCode"];
                            r4["TR_VersionID"] = r3["TR_VersionID"];
                            r4["TR_ProcessSequence"] = r3["TR_ProcessSequence"];
                            r4["TR_ProcessName"] = r3["TR_ProcessName"];
                            r4["TR_ProcessCode"] = r3["TR_ProcessCode"];
                            r4["TR_ProcessID"] = r3["TR_ProcessID"];
                            r4["TR_WorkHour"] = r3["TR_WorkHour"];
                            r4["TR_IsReportPoint"] = r3["TR_IsReportPoint"];
                            r4["TR_IsExProcess"] = r3["TR_IsExProcess"];
                            r4["TR_WorkCenterID"] = r3["TR_WorkCenterID"];
                            r4["TR_IsFirstProcess"] = false;
                            r4["TR_IsLastProcess"] = r3["TR_IsLastProcess"];
                            r4["TR_IsTestProcess"] = r3["TR_IsTestProcess"];
                            r4["TR_IsBackProcess"] = r3["TR_IsBackProcess"];
                            r4["TR_DefaultCheckPersonName"] = r3["TR_DefaultCheckPersonName"];
                            r4["TR_BindingProcess"] = r3["TR_BindingProcess"];
                            r4["TR_IsReportDevice"] = r3["TR_IsReportDevice"];
                            r4["TR_IsDeviceCharging"] = r3["TR_IsDeviceCharging"];
                            r4["IDNew"] = r3["ID"];

                            TechRoute.Rows.Add(r4);
                            TechRoute.AcceptChanges();
                        }
                        else if ((int)r2["TR_ProcessSequence"] == minprocess)
                        {
 
                        }
                        

                        r2 = TechRoute.Select("TR_VersionID = '" + r1["ID"] + "' and TR_IsLastProcess = true")[0];

                        if ((int)r2["TR_ProcessSequence"] < maxprocess)
                        {
                            r2["TR_IsLastProcess"] = false;
                        }
                        else if ((int)r2["TR_ProcessSequence"] > maxprocess)
                        {
                            r3 = bar_techroute.Select("TR_VersionID = '" + r1["ID"] + "'and TR_ProcessSequence = '" + maxprocess + "'")[0];


                            r4 = TechRoute.NewRow();
                            r4["ID"] = r3["ID"];
                            r4["TR_ItemID"] = r3["TR_ItemID"];
                            r4["TR_ItemCode"] = r3["TR_ItemCode"];
                            r4["TR_VersionID"] = r3["TR_VersionID"];
                            r4["TR_ProcessSequence"] = r3["TR_ProcessSequence"];
                            r4["TR_ProcessName"] = r3["TR_ProcessName"];
                            r4["TR_ProcessCode"] = r3["TR_ProcessCode"];
                            r4["TR_ProcessID"] = r3["TR_ProcessID"];
                            r4["TR_WorkHour"] = r3["TR_WorkHour"];
                            r4["TR_IsReportPoint"] = r3["TR_IsReportPoint"];
                            r4["TR_IsExProcess"] = r3["TR_IsExProcess"];
                            r4["TR_WorkCenterID"] = r3["TR_WorkCenterID"];
                            r4["TR_IsFirstProcess"] = r3["TR_IsFirstProcess"];
                            r4["TR_IsLastProcess"] = false;
                            r4["TR_IsTestProcess"] = r3["TR_IsTestProcess"];
                            r4["TR_IsBackProcess"] = r3["TR_IsBackProcess"];
                            r4["TR_DefaultCheckPersonName"] = r3["TR_DefaultCheckPersonName"];
                            r4["TR_BindingProcess"] = r3["TR_BindingProcess"];
                            r4["TR_IsReportDevice"] = r3["TR_IsReportDevice"];
                            r4["TR_IsDeviceCharging"] = r3["TR_IsDeviceCharging"];
                            r4["IDNew"] = r3["ID"];
                            TechRoute.Rows.Add(r4);
                            TechRoute.AcceptChanges();
                        }
                        else if ((int)r2["TR_ProcessSequence"] == maxprocess)
                        {
 
                        }
                    }

                }


                List<string> cloList2 = new List<string> { "ID", 
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



                //刷新工艺路线表
                MyDBController.GetConnection();
                bar_techroute.Clear();
                GetBar_TechRoute();
                MyDBController.CloseConnection();





                //count = WorkHour.Rows.Count;
                //for (int i = 0; i < count; i++)
                //{

                //    r1 = WorkHour.Rows[i];

                //    r2 = copiedData.Rows[i];

                //    r3 = bar_iteminfo.Select("II_Code = '" + r2["II_Code"] + "'")[0];

                //    //r4 = bar_TechRouteVersion.Select("TRV_ItemID = '" + r3["ID"] + "' and TRV_VersionCode = '" + r2["TRV_VersionCode"] + "'")[0];
                //    r4 = bar_techrouteversion.Select("TRV_ItemID = '" + r3["ID"] + "' and TRV_VersionCode = '" + r2["TRV_VersionCode"] + "'")[0];

                    

                //    //得到工艺路线ID
                //    r1["WH_TechRouteID"] = bar_techroute.Select("TR_ItemCode = '" + r2["II_Code"] + "' and TR_VersionID = '" + r4["ID"] + "' and TR_ProcessSequence = '" + r2["TR_ProcessSequence"] + "'")[0]["ID"];

                //    //检测价表ID,有则返回
                //    if (bar_workhour.Select("WH_TechRouteID  ='" + r1["WH_TechRouteID"]  + "'").Length  > 0)
                //    {
                //        r1["ID"] = bar_workhour.Select("WH_TechRouteID  ='" + r1["WH_TechRouteID"] + "'")[0]["ID"];
                //        r1["IDNew"] = r1["ID"];
                //    }


                //}

                //List<string> cloList3 = new List<string> { "ID", "WH_TechRouteID", "WH_WorkHour", "WH_StartDate", "WH_EndDate" };

                //MyDBController.GetConnection();
                //MyDBController.InsertSqlBulk(WorkHour, cloList3, out updateNum, out insertNum);
                //MyDBController.CloseConnection();

                

                ////刷新价表
                //MyDBController.GetConnection();
                //bar_workhour.Clear();
                //GetBar_WorkHour();
                //MyDBController.CloseConnection();
                MessageBox.Show("导入成功", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                IsCheck = false;
            
            }
            else
            {
                MessageBox.Show(error[8], "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            this.Cursor = Cursors.Arrow;
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
        /// 清空按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            trils.Clear();
            listview1.ItemsSource = null;
            listview1.Items.Clear();
            listview1.Items.Refresh();
            IsCheck = false;
        }
        /// <summary>
        /// 检测按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Check_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            CheckIfRight();

            this.Cursor = Cursors.Arrow;
        }




    }
}
