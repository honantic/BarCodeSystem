using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using BarCodeSystem.TechRoute.TechRoute;

namespace BarCodeSystem
{
    /// <summary>
    /// TechRouteModify_Window.xaml 的交互逻辑
    /// </summary>
    public partial class TechRouteModify_Window : Window
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TechRouteModify_Window()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 维护工时的构造函数
        /// </summary>
        /// <param name="str"></param>
        public TechRouteModify_Window(string str)
        {
            InitializeComponent();
            this.Title = "工时维护窗口";
            btn_AddVersion.IsEnabled = btn_ClearInfo.IsEnabled = btn_DeleteProcess.IsEnabled = false;
            radio_IsBackProcess_False.IsEnabled = radio_IsBackProcess_True.IsEnabled = radio_IsBackVersion_False.IsEnabled = radio_IsBackVersion_True.IsEnabled = radio_IsTestProcess_False.IsEnabled = radio_IsTestProcess_True.IsEnabled = radio_ReportWay_Flow.IsEnabled = radio_ReportWay_Scatter.IsEnabled = false;
            textb_ChooseItem.IsEnabled = textb_GetProcess.IsEnabled = textb_GetWorkCenter.IsEnabled = textb_ChooseCheckPerson.IsEnabled = false;
            txtb_ProcessName.IsReadOnly = txtb_ItemCode.IsReadOnly = txtb_ItemName.IsReadOnly = txtb_TechSequence.IsReadOnly = txtb_TechVersion.IsReadOnly = txtb_TechVersionName.IsReadOnly = txtb_WorkCenterName.IsReadOnly = true;
            cb_IsDefaultVersion.IsEnabled = false;
            cb_IsSpecialVersion_False.IsEnabled = cb_IsSpecialVersion_True.IsEnabled = false;
            txtb_WorkHour.IsReadOnly = false;
        }

        //选择的工序ID、工作中心ID
        Int64 choosedProcessID, choosedWorkCenterID, choosedItemID, trv_versionID;
        string choosedProcessCode;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        List<TechRouteLists> RemovedList = new List<TechRouteLists>();

        /// <summary>
        /// 当窗体为修改窗体的时候的传值,当前窗体为新增窗体的时候new
        /// ,为当前料品对应的版本的工序信息
        /// </summary>
        public List<TechRouteLists> trls
        {
            get;
            set;
        }

        /// <summary>
        /// 选中的要修改的料品信息
        /// </summary>
        public ItemInfoLists selectedItem
        {
            get;
            set;
        }

        /// <summary>
        /// 选中的修改的工艺路线版本
        /// </summary>
        public TechVersion techversion { get; set; }
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


            //去除关闭按钮
            //2.在装载事件中加入
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            InitShow();
        }

        //去除关闭按钮
        //1.Window 类中申明
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


        /// <summary>
        /// 展示初始化信息
        /// </summary>
        private void InitShow()
        {
            #region 条码系统暂定全部平均分配
            #endregion

            if (this.Title == "工艺路线新增窗口")
            {
                trls = new List<TechRouteLists> { };
                listview1.ItemsSource = trls;
                txtb_ItemCode.IsReadOnly = txtb_ItemName.IsReadOnly =
                    txtb_ProcessName.IsReadOnly = txtb_WorkCenterName.IsReadOnly = true;
                btn_AddVersion.IsEnabled = false;
            }
            else
            {
                listview1.ItemsSource = trls;
                txtb_ItemCode.IsReadOnly = txtb_ItemName.IsReadOnly =
                    txtb_WorkCenterName.IsReadOnly = true;
                textb_ChooseItem.IsEnabled = false;
                txtb_ItemCode.Text = selectedItem.II_Code;
                txtb_ItemName.Text = selectedItem.II_Name;
                choosedItemID = selectedItem.ID;
                trv_versionID = trls[0].TR_VersionID;
                cb_IsDefaultVersion.IsChecked = trls[0].TR_IsDefaultVer;
                cb_IsSpecialVersion_False.IsChecked = !techversion.TRV_IsSpecialVersion;
                radio_IsBackVersion_False.IsChecked = !techversion.TRV_IsBackVersion;
                radio_ReportWay_Flow.IsChecked = techversion.TRV_ReportWay == 0 ? true : false;
                btn_DeleteProcess.IsEnabled = false;
            }

        }

        /// <summary>
        /// 选择料品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textb_ChooseItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TechRouteItemList_Window tril = new TechRouteItemList_Window();
            tril.ShowDialog();
            if ((bool)tril.DialogResult)
            {
                txtb_ItemCode.Text = tril.II_Code;
                txtb_ItemName.Text = tril.II_Name;
                choosedItemID = tril.II_ID;
            }
        }

        /// <summary>
        /// 选择工序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textb_GetProcess_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ProcessNameList_Window pnl = new ProcessNameList_Window();
            pnl.ShowDialog();
            if ((bool)pnl.DialogResult)
            {
                txtb_ProcessName.Text = pnl.PN_Name;
                choosedProcessID = pnl.PN_ID;
                choosedProcessCode = pnl.PN_Code;
            }
        }

        /// <summary>
        /// 选择工作中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textb_GetWorkCenter_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WorkTeamDepartList_Window wtdl = new WorkTeamDepartList_Window();
            wtdl.ShowDialog();
            if ((bool)wtdl.DialogResult)
            {
                txtb_WorkCenterName.Text = wtdl.choosedName;
                choosedWorkCenterID = wtdl.choosedID;
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }


        /// <summary>
        /// 保存工艺路线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            btn_Save_Click1(sender, e);
        }

        /// <summary>
        /// 保存当前工艺路线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click1(object sender, RoutedEventArgs e)
        {
            if (listview1.Items.Count > 0)
            {
                #region 对当前工艺路线进行处理，对数据操作前的准备
                ds = new DataSet();
                //排在第一的工序设置为首道工序，最后的设置为末道工序
                trls.ForEach(
                    p =>
                    {
                        if (trls.IndexOf(p) == 0)
                        {
                            p.TR_IsFirstProcess = true;
                        }
                        else
                        {
                            p.TR_IsFirstProcess = false;
                        }
                        if (trls.IndexOf(p) == trls.Count - 1)
                        {
                            p.TR_IsLastProcess = true;
                        }
                        else
                        {
                            p.TR_IsLastProcess = false;
                        }
                    });
                Int64 trv_itemID = trls[0].TR_ItemID;
                string trv_versionCode = trls[0].TRV_VersionCode, trv_versionName = trls[0].TRV_VersionName;

                //当前版本是否为默认版本，当前版本是否启用,是否返工版本
                bool isdefaultver = (bool)cb_IsDefaultVersion.IsChecked, isvalidate = true, isBackVersion = !(bool)radio_IsTestProcess_False.IsChecked, isSpecialVersion = !(bool)cb_IsSpecialVersion_False.IsChecked;
                //报工方式
                int reportWay = (bool)radio_ReportWay_Scatter.IsChecked ? 1 : 0;//0:流水线报工，1：离散报工

                //获取系统中是否有当前料品+工艺路线版本的信息
                MyDBController.GetConnection();
                string SQl = string.Format(@"SELECT [ID] FROM [TechRouteVersion] WHERE [TRV_ItemID]={0} AND [TRV_VersionCode]='{1}'", trv_itemID, trv_versionCode);
                dt.Clear();
                dt = MyDBController.GetDataSet(SQl, ds).Tables[0];
                int x = dt.Rows.Count;

                //有当前版本信息，对versionID赋值，没有，则在TechRouteVersion表中插入信息，然后再为versionID赋值。
                if (x > 0)
                {
                    if (this.Title.Equals("工艺路线新增窗口"))
                    {
                        MessageBox.Show("该工艺路线版本已经存在！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    trv_versionID = (Int64)dt.Rows[0]["ID"];
                }
                else
                {
                    //在工艺路线版本表中添加新版本
                    string SQl1 = string.Format(@"INSERT INTO [TechRouteVersion]([TRV_ItemID],[TRV_VersionCode],[TRV_VersionName],[TRV_IsDefaultVer],[TRV_IsValidated],[TRV_IsBackVersion],[TRV_ReportWay],[TRV_IsSpecialVersion]) VALUES({0},'{1}','{2}','{3}','{4}','{5}',{6},'{7}')", trv_itemID, trv_versionCode, trv_versionName, isdefaultver, isvalidate, isBackVersion, reportWay, isSpecialVersion);
                    MyDBController.ExecuteNonQuery(SQl1);
                    SqlDataReader reader = MyDBController.GetDataReader(SQl);
                    while (reader.Read())
                    {
                        trv_versionID = (Int64)reader[0];
                    }
                    reader.Close();
                    //新增默认版本
                    if (isdefaultver)
                    {
                        SQl = string.Format("Update [TechRouteVersion] set [TRV_IsDefaultVer]=0 where [TRV_ItemID]={0} and [ID] !={1}", trls[0].TR_ItemID, trv_versionID);
                        MyDBController.ExecuteNonQuery(SQl);
                    }
                }
                #endregion

                #region 对数据库进行操作
                SQl = string.Format(@"Select top 0 * from [TechRoute]");
                MyDBController.GetDataSet(SQl, ds, "TechRoute");
                List<string> colList = new List<string>();

                foreach (DataColumn col in ds.Tables["TechROute"].Columns)
                {
                    colList.Add(col.ColumnName);
                }
                ds.Tables["TechROute"].Columns.Add(new DataColumn("IDNew", typeof(Int64)));
                //工序列表的长度，trls为listview1的itemssource
                x = trls.Count;
                try
                {
                    foreach (TechRouteLists trl in trls)
                    {
                        DataRow row = ds.Tables["TechRoute"].NewRow();
                        row["ID"] = row["IDNew"] = trl.ID;
                        row["TR_ItemID"] = trl.TR_ItemID;
                        row["TR_ItemCode"] = trl.TR_ItemCode;
                        row["TR_VersionID"] = trv_versionID;
                        row["TR_ProcessSequence"] = trl.TR_ProcessSequence;
                        row["TR_ProcessName"] = trl.TR_ProcessName;
                        row["TR_ProcessCode"] = trl.TR_ProcessCode;
                        row["TR_ProcessID"] = trl.TR_ProcessID;
                        row["TR_WorkHour"] = trl.TR_WorkHour;
                        row["TR_IsReportPoint"] = trl.TR_IsReportPoint;
                        row["TR_IsExProcess"] = trl.TR_IsExProcess;
                        row["TR_WorkCenterID"] = trl.TR_WorkCenterID;
                        row["TR_IsFirstProcess"] = trl.TR_IsFirstProcess;
                        row["TR_IsLastProcess"] = trl.TR_IsLastProcess;
                        row["TR_IsTestProcess"] = trl.TR_IsTestProcess;
                        row["TR_IsBackProcess"] = trl.TR_IsBackProcess;
                        row["TR_DefaultCheckPersonName"] = trl.TR_DefaultCheckPersonName;
                        row["TR_BindingProcess"] = trl.TR_BindingProcess;
                        row["TR_IsReportDevice"] = trl.TR_IsReportDevice;
                        row["TR_IsDeviceCharging"] = trl.TR_IsDeviceCharging;
                        ds.Tables["TechRoute"].Rows.Add(row);
                    }
                    int updateNum, insertNum;
                    MyDBController.InsertSqlBulk(ds.Tables["TechRoute"], colList, out updateNum, out insertNum);
                    MyDBController.GetConnection();
                    //将本来的默认工艺路线改为不是默认工艺路线
                    if (!trls[0].TR_IsDefaultVer.Equals(isdefaultver) && !isdefaultver)
                    {
                        SQl = string.Format("Update [TechRouteVersion] set [TRV_IsDefaultVer]=0 where [TRV_ItemID]={0} and [ID] ={1}", trls[0].TR_ItemID, trv_versionID);
                        MyDBController.ExecuteNonQuery(SQl);
                    }
                    //将本来不是默认工艺路线的改为默认工艺路线
                    else if (!trls[0].TR_IsDefaultVer.Equals(isdefaultver) && isdefaultver)
                    {
                        SQl = string.Format("Update [TechRouteVersion] set [TRV_IsDefaultVer]=1 where [TRV_ItemID]={0} and [ID] ={1}", trls[0].TR_ItemID, trv_versionID);
                        MyDBController.ExecuteNonQuery(SQl);
                        SQl = string.Format("Update [TechRouteVersion] set [TRV_IsDefaultVer]=0 where [TRV_ItemID]={0} and [ID] !={1}", trls[0].TR_ItemID, trv_versionID);
                        MyDBController.ExecuteNonQuery(SQl);
                    }
                    SQl = string.Format("Update [TechRouteVersion] set [TRV_ReportWay]={0} where [TRV_ItemID]={1} and [ID] ={2}", reportWay, trls[0].TR_ItemID, trv_versionID);
                    MyDBController.ExecuteNonQuery(SQl);

                    //将删除的工序从工艺路线表中删除
                    if (RemovedList.Count > 0)
                    {
                        string idList = "";
                        RemovedList.ForEach(p => { idList += idList.Length == 0 ? p.ID.ToString() : "," + p.ID.ToString(); });
                        idList = "(" + idList + ")";
                        SQl = string.Format("delete from [TechRoute] where [ID] in {0}", idList);
                        MyDBController.ExecuteNonQuery(SQl);
                    }

                    if (MessageBox.Show("工艺路线信息保存成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                    {
                        this.DialogResult = true;
                    }
                }
                catch (Exception ee)
                {

                    MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                #endregion
            }
            else
            {
                MessageBox.Show("工艺路线为空！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        /// <summary>
        /// 文本框双击全选文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_ItemCode_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.SelectAll();
        }

        /// <summary>
        /// 保存当前工序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveProcess_Click(object sender, RoutedEventArgs e)
        {
            if (txtb_ItemCode.Text.Length == 0 || txtb_ItemName.Text.Length == 0 || txtb_ProcessName.Text.Length == 0 || txtb_TechSequence.Text.Length == 0 || txtb_TechVersion.Text.Length == 0 || txtb_WorkCenterName.Text.Length == 0 || txtb_TechVersionName.Text.Length == 0 || txtb_TR_DefaultCheckPersonName.Text.Length == 0)
            {
                MessageBox.Show("任何信息不能为空", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                if (Regex.IsMatch(txtb_TechSequence.Text.Trim(), User_Info.pattern[0]))
                {
                    bool IsRightVersion = true;
                    bool NewOrNot = true;
                    if (listview1.Items != null)
                    {
                        foreach (TechRouteLists item in listview1.Items)
                        {
                            if (item.TRV_VersionCode != txtb_TechVersion.Text.Trim() || item.TRV_VersionName != txtb_TechVersionName.Text.Trim())
                            {
                                MessageBox.Show("工序版本不一致！ \n请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                IsRightVersion = false;
                                break;
                            }
                            else if (item.TR_ItemCode != txtb_ItemCode.Text.Trim())
                            {
                                MessageBox.Show("料品编号不一致！\n请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                                IsRightVersion = false;
                                break;
                            }
                            else if (item.TR_ProcessSequence == Convert.ToInt32(txtb_TechSequence.Text.Trim()))
                            {
                                if (MessageBox.Show("该工序序号已经存在，是否要覆盖？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information)
                                    == MessageBoxResult.OK)
                                {
                                    NewOrNot = false;
                                    break;
                                }
                                else
                                {
                                    IsRightVersion = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (IsRightVersion)
                    {
                        TechRouteLists trl;
                        if (this.Title == "工艺路线修改窗口")
                        {
                            if (!NewOrNot)//不是新工序
                            {
                                int index = listview1.SelectedIndex;
                                trl = trls[index];
                                trl.TR_ProcessName = txtb_ProcessName.Text;
                                trl.TR_ProcessCode = choosedProcessCode == null ? trl.TR_ProcessCode : choosedProcessCode;
                                trl.TR_ProcessID = choosedProcessID == 0 ? trl.TR_ProcessID : choosedProcessID;
                                trl.TR_WorkCenterID = choosedWorkCenterID == 0 ? trl.TR_WorkCenterID : choosedWorkCenterID;
                                trl.TR_ProcessSequence = Convert.ToInt32(txtb_TechSequence.Text.Trim());
                                trl.WC_Department_Name = txtb_WorkCenterName.Text.Trim();
                                trl.TR_WorkHour = Convert.ToDecimal(txtb_WorkHour.Text);/*----------------------这段是预留的，工时根据单件工资计算---------------- */
                                trl.TRV_VersionCode = txtb_TechVersion.Text.Trim();
                                trl.TRV_VersionName = txtb_TechVersionName.Text.Trim();
                                trl.TR_DefaultCheckPersonName = txtb_TR_DefaultCheckPersonName.Text;
                                trl.TR_IsBackProcess = !(bool)radio_IsBackProcess_False.IsChecked;
                                trl.TR_IsTestProcess = !(bool)radio_IsTestProcess_False.IsChecked;
                            }
                            else//新工序
                            {
                                trl = new TechRouteLists();
                                trl.TR_ItemID = choosedItemID;
                                trl.TR_VersionID = trv_versionID;
                                trl.TR_ItemCode = txtb_ItemCode.Text;
                                trl.II_Name = txtb_ItemName.Text.Trim();
                                trl.TR_ProcessName = txtb_ProcessName.Text;
                                trl.TR_ProcessCode = choosedProcessCode;
                                trl.TR_ProcessID = choosedProcessID;
                                trl.TR_WorkCenterID = choosedWorkCenterID == 0 ? trls[0].TR_WorkCenterID : choosedWorkCenterID;
                                trl.TR_ProcessSequence = Convert.ToInt32(txtb_TechSequence.Text.Trim());
                                trl.WC_Department_Name = txtb_WorkCenterName.Text.Trim();
                                trl.TR_WorkHour = Convert.ToDecimal(txtb_WorkHour.Text);/*----------------------这段是预留的，工时根据单件工资计算---------------- */
                                trl.TRV_VersionCode = txtb_TechVersion.Text.Trim();
                                trl.TRV_VersionName = txtb_TechVersionName.Text.Trim();
                                trl.TR_DefaultCheckPersonName = txtb_TR_DefaultCheckPersonName.Text;
                                trl.TR_IsBackProcess = !(bool)radio_IsBackProcess_False.IsChecked;
                                trl.TR_IsTestProcess = !(bool)radio_IsTestProcess_False.IsChecked;
                                trls.Add(trl);
                            }

                            trls.Sort(delegate(TechRouteLists x, TechRouteLists y)
                            {
                                return x.TR_ProcessSequence.CompareTo(y.TR_ProcessSequence);
                            });
                            listview1.Items.Refresh();
                        }
                        else
                        {
                            if (!NewOrNot)
                            {
                                trl = trls.Find(p => p.TR_ProcessSequence.Equals(Convert.ToInt32(txtb_TechSequence.Text)));
                                trl.TR_ProcessName = txtb_ProcessName.Text;
                                trl.TR_ProcessCode = choosedProcessCode == null ? trl.TR_ProcessCode : choosedProcessCode;
                                trl.TR_ProcessID = choosedProcessID == 0 ? trl.TR_ProcessID : choosedProcessID;
                                trl.TR_WorkCenterID = choosedWorkCenterID == 0 ? trl.TR_WorkCenterID : choosedWorkCenterID;
                                trl.TR_ProcessSequence = Convert.ToInt32(txtb_TechSequence.Text.Trim());
                                trl.WC_Department_Name = txtb_WorkCenterName.Text.Trim();
                                trl.TR_WorkHour = Convert.ToDecimal(txtb_WorkHour.Text);/*----------------------这段是预留的，工时根据单件工资计算---------------- */
                                trl.TRV_VersionCode = txtb_TechVersion.Text.Trim();
                                trl.TRV_VersionName = txtb_TechVersionName.Text.Trim();
                                trl.TR_DefaultCheckPersonName = txtb_TR_DefaultCheckPersonName.Text;
                                trl.TR_IsBackProcess = !(bool)radio_IsBackProcess_False.IsChecked;
                                trl.TR_IsTestProcess = !(bool)radio_IsTestProcess_False.IsChecked;

                            }
                            else
                            {
                                trl = new TechRouteLists();
                                trl.TR_ItemID = choosedItemID;
                                trl.TR_ItemCode = txtb_ItemCode.Text;
                                trl.II_Name = txtb_ItemName.Text.Trim();
                                trl.TR_ProcessName = txtb_ProcessName.Text;
                                trl.TR_ProcessCode = choosedProcessCode;
                                trl.TR_ProcessSequence = Convert.ToInt32(txtb_TechSequence.Text.Trim());
                                trl.TR_ProcessID = choosedProcessID;
                                trl.TR_WorkCenterID = choosedWorkCenterID;
                                trl.WC_Department_Name = txtb_WorkCenterName.Text.Trim();
                                //trl.TR_WorkHour = Convert.ToDecimal(txtb_WorkHour.Text);/*----------------------这段是预留的，工时根据单件工资计算---------------- */
                                trl.TR_VersionID = trv_versionID;
                                trl.TRV_VersionCode = txtb_TechVersion.Text.Trim();
                                trl.TRV_VersionName = txtb_TechVersionName.Text.Trim();
                                trl.TR_DefaultCheckPersonName = txtb_TR_DefaultCheckPersonName.Text;
                                trl.TR_IsBackProcess = !(bool)radio_IsBackProcess_False.IsChecked;
                                trl.TR_IsTestProcess = !(bool)radio_IsTestProcess_False.IsChecked;
                                trls.Add(trl);
                            }
                            trls.Sort(delegate(TechRouteLists x, TechRouteLists y)
                            {
                                return x.TR_ProcessSequence.CompareTo(y.TR_ProcessSequence);
                            });
                            listview1.Items.Refresh();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("工序序号和工时必须是数字! \n请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            txtb_TechSequence.Text = "";
        }

        /// <summary>
        /// 清空信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ClearInfo_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确定要清空所有信息吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information)
                == MessageBoxResult.OK)
            {
                txtb_ItemCode.Text = txtb_ItemName.Text = txtb_ProcessName.Text =
                    txtb_TechSequence.Text = txtb_TechVersion.Text = txtb_WorkCenterName.Text =
                    txtb_WorkHour.Text = "";
            }

        }

        /// <summary>
        /// 列表中左键点击事件，自动为各个文本框添加点击的条目内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point formPoint = e.GetPosition(listview1);

            if (formPoint.Y < 20)
            {
            }
            else
            {
                int x = (int)(formPoint.Y - 20) / 20;
                if (listview1.Items.Count - 1 >= x)
                {
                    TechRouteLists trl = listview1.Items[x] as TechRouteLists;
                    if (trl != null)
                    {
                        txtb_ItemCode.Text = trl.TR_ItemCode;
                        txtb_ItemName.Text = trl.II_Name;
                        txtb_ProcessName.Text = trl.TR_ProcessName;
                        txtb_TechSequence.Text = trl.TR_ProcessSequence.ToString();
                        txtb_TechVersion.Text = trl.TRV_VersionCode;
                        txtb_TechVersionName.Text = trl.TRV_VersionName;
                        txtb_WorkCenterName.Text = trl.WC_Department_Name;
                        txtb_WorkHour.Text = trl.TR_WorkHour.ToString();
                        txtb_TR_DefaultCheckPersonName.Text = trl.TR_DefaultCheckPersonName;
                        //txtb_WagePerPiece.Text = trl.TR_WagePerPiece.ToString();
                        //cb_WageAllotScheme.SelectedValue = trl.TR_WageAllotScheme;
                    }
                }
            }
        }

        /// <summary>
        /// 删除选中工序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DeleteProcess_Click(object sender, RoutedEventArgs e)
        {
            if (listview1.SelectedItem == null)
            {
                MessageBox.Show("请选中一条工序！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                TechRouteLists trl = listview1.SelectedItem as TechRouteLists;
                trls.Remove(trl);
                RemovedList.Add(trl);
                listview1.Items.Refresh();
                if (trls.Count == 0)
                {
                    txtb_ProcessName.Text = txtb_WorkCenterName.Text = "";
                }
            }
        }

        /// <summary>
        /// 添加新版本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddVersion_Click(object sender, RoutedEventArgs e)
        {
            trls.Clear();
            listview1.Items.Refresh();
            RemovedList.Clear();
            this.Title = "工艺路线新增窗口";
            txtb_ItemName.IsReadOnly = txtb_ItemCode.IsReadOnly = true;
            txtb_ProcessName.Text = txtb_TechSequence.Text = txtb_WorkCenterName.Text
                = txtb_WorkHour.Text = txtb_TechVersion.Text = "";
            btn_DeleteProcess.IsEnabled = true;
            //cb_WageAllotScheme.SelectedIndex = -1;
        }



        /// <summary>
        /// 选择检验员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textb_ChooseCheckPerson_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TechRouteCheckPerson_Window tcp = new TechRouteCheckPerson_Window();
            tcp.ShowDialog();
            if ((bool)tcp.DialogResult)
            {
                txtb_TR_DefaultCheckPersonName.Text = tcp.checkPersonName;
            }
        }
    }
}
