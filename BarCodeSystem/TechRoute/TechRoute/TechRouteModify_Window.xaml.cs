using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;

namespace BarCodeSystem
{
    /// <summary>
    /// TechRouteModify_Window.xaml 的交互逻辑
    /// </summary>
    public partial class TechRouteModify_Window : Window
    {
        public TechRouteModify_Window()
        {
            InitializeComponent();
        }

        //选择的工序ID、工作中心ID
        Int64 choosedProcessID, choosedWorkCenterID, choosedItemID, trv_versionID;
        string choosedProcessCode;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();

        /// <summary>
        /// 当窗体为修改窗体的时候的传值，当前选中的料品信息
        /// </summary>
        //public ItemInfoLists iil
        //{
        //    get;
        //    set;
        //}

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


        /// <summary>
        /// 设置绑定
        /// </summary>
        private void SetBinding()
        {
            //trl = (TechRouteLists)listview1.SelectedItem ?? new TechRouteLists();
            //trl.TR_ProcessSequence;
            //Binding txtb_ItemCode_BD = new Binding("TR_ItemCode") { Source = trl, UpdateSourceTrigger = UpdateSourceTrigger.Default };
            //this.txtb_ItemCode.SetBinding(TextBox.TextProperty, txtb_ItemCode_BD);

            //Binding txtb_ItemName_BD = new Binding("II_Name") { Source = trl, UpdateSourceTrigger = UpdateSourceTrigger.Default };
            //this.txtb_ItemName.SetBinding(TextBox.TextProperty, txtb_ItemName_BD);

            //Binding txtb_TechSequence_BD = new Binding("TR_ProcessSequence") { Source = trl, UpdateSourceTrigger = UpdateSourceTrigger.Default };
            //Binding txtb_TechSequence_BD = new Binding() { Mode=BindingMode.OneWay};
            //txtb_TechSequence_BD.Source = trl;
            //txtb_TechSequence_BD.Converter = new TechRoute_ProcessSequenceConverter();
            //this.txtb_TechSequence.SetBinding(TextBox.TextProperty, txtb_TechSequence_BD);
            //txtb_ProcessName. = trl;

            //Binding txtb_ProcessName_BD = new Binding("SelectedIndex") { Source = listview1, UpdateSourceTrigger = UpdateSourceTrigger.Default };
            //this.txtb_ProcessName.SetBinding(TextBox.TextProperty, txtb_ProcessName_BD);

            //Binding txtb_TechVersion_BD = new Binding("SelectedItem.ToString()") { Source = trl, UpdateSourceTrigger = UpdateSourceTrigger.Default };
            //this.txtb_TechVersion.SetBinding(TextBox.TextProperty, txtb_TechVersion_BD);

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
            ObservableCollection<TechRouteLists> OCtrl =
                new ObservableCollection<TechRouteLists>() 
                {
                    //new TechRouteLists{ TR_WageAllotScheme=0,TR_WageAllotScheme_Show="独立分配"},
                    new TechRouteLists{ TR_WageAllotScheme=1,TR_WageAllotScheme_Show="平均分配"},
                    //new TechRouteLists{ TR_WageAllotScheme=2,TR_WageAllotScheme_Show="按合作人数分配配额公式计算"}
                };

            cb_WageAllotScheme.ItemsSource = OCtrl;

            #region 条码系统暂定全部平均分配
            //cb_WageAllotScheme.IsReadOnly = true;
            cb_WageAllotScheme.SelectedIndex = 0;
            #endregion

            if (this.Title == "工艺路线修改窗口")
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
            }
            else
            {
                trls = new List<TechRouteLists> { };
                listview1.ItemsSource = trls;
                txtb_ItemCode.IsReadOnly = txtb_ItemName.IsReadOnly =
                    txtb_ProcessName.IsReadOnly = txtb_WorkCenterName.IsReadOnly = true;
                btn_AddVersion.IsEnabled = false;
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
        /// 保存当前工艺路线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private MessageBoxResult btn_Save_Click1(object sender, RoutedEventArgs e)
        {
            if (listview1.Items.Count > 0)
            {
                #region 对当前工艺路线进行处理，对数据操作前的准备
                //排在第一的工序设置为首道工序，最后的设置为末道工序
                TechRouteLists item = listview1.Items[0] as TechRouteLists;
                item.TR_IsFirstProcess = true;
                item = listview1.Items[listview1.Items.Count - 1] as TechRouteLists;
                item.TR_IsLastProcess = true;

                Int64 trv_itemID = item.TR_ItemID;
                string trv_version = item.TRV_Version;

                //当前版本是否为默认版本，当前版本是否启用
                bool isdefaultver = (bool)cb_IsDefaultVersion.IsChecked, isvalidate = true;

                //获取系统中是否有当前料品+工艺路线版本的信息
                MyDBController.GetConnection();
                string SQl = string.Format(@"SELECT [ID] FROM [TechRouteVersion] WHERE [TRV_ItemID]={0} AND [TRV_Version]='{1}'",
                                    trv_itemID, trv_version);
                dt.Clear();
                dt = MyDBController.GetDataSet(SQl, ds).Tables[0];
                int x = dt.Rows.Count;

                //有当前版本信息，对versionID赋值，没有，则在TechRouteVersion表中插入信息，然后再为versionID赋值。
                if (x > 0)
                {
                    if (this.Title.Equals("工艺路线新增窗口"))
                    {
                        return MessageBox.Show("该工艺路线版本已经存在！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    trv_versionID = (Int64)dt.Rows[0]["ID"];
                }
                else
                {
                    //在工艺路线版本表中添加新版本
                    string SQl1 = string.Format(@"INSERT INTO [TechRouteVersion]([TRV_ItemID],[TRV_Version],[TRV_IsDefaultVer],[TRV_IsValidated]) VALUES({0},'{1}','{2}','{3}')", trv_itemID, trv_version, isdefaultver, isvalidate);
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

                SQl = string.Format(@"DELETE FROM [TechRoute] WHERE [TR_ItemID]={0} AND [TR_VersionID]={1}",
                             trv_itemID, trv_versionID);
                MyDBController.ExecuteNonQuery(SQl);

                //工序列表的长度，trls为listview1的itemssource
                x = trls.Count;
                try
                {
                    foreach (TechRouteLists trl in trls)
                    {
                        SQl = string.Format(@"INSERT INTO [TechRoute]([TR_ItemID],[TR_ItemCode],[TR_VersionID],[TR_ProcessSequence],
                                        [TR_ProcessName],[TR_ProcessCode],[TR_ProcessID],[TR_IsReportPoint],[TR_IsExProcess],
                                        [TR_WorkCenterID],[TR_IsFirstProcess],[TR_IsLastProcess],[TR_WagePerPiece],[TR_WorkHour],
                                        [TR_WageAllotScheme],[TR_AllotFormulaID],[TR_IsReportDevice],[TR_IsDeviceCharging],[TR_IsTestProcess],[TR_IsBackProcess]) 
                                        VALUES( {0},'{1}',{2},{3},'{4}','{5}',{6},'{7}','{8}',{9},'{10}','{11}',{12},
                                        {13},'{14}','{15}','{16}','{17}','{18}','{19}')",
                                            trl.TR_ItemID, trl.TR_ItemCode, trv_versionID, trl.TR_ProcessSequence, trl.TR_ProcessName,
                                            trl.TR_ProcessCode, trl.TR_ProcessID, trl.TR_IsReportPoint, trl.TR_IsExProcess, trl.TR_WorkCenterID,
                                            trl.TR_IsFirstProcess, trl.TR_IsLastProcess, trl.TR_WagePerPiece, trl.TR_WorkHour,
                                            trl.TR_WageAllotScheme, (Int64)trl.TR_WageAllotScheme, trl.TR_IsReportDevice, trl.TR_IsDeviceCharging,trl.TR_IsTestProcess,trl.TR_IsBackProcess);
                        MyDBController.ExecuteNonQuery(SQl);
                    }
                    //将本来的默认工艺路线改为不是默认工艺路线
                    if ( !trls[0].TR_IsDefaultVer.Equals(isdefaultver) && !isdefaultver)
                    {
                        SQl = string.Format("Update [TechRouteVersion] set [TRV_IsDefaultVer]=0 where [TRV_ItemID]={0} and [ID] !={1}", trls[0].TR_ItemID, trv_versionID);
                        MyDBController.ExecuteNonQuery(SQl);
                    }
                    //将本来不是默认工艺路线的改为默认工艺路线
                    else if ( !trls[0].TR_IsDefaultVer.Equals(isdefaultver) && isdefaultver)
                    {
                        SQl = string.Format("Update [TechRouteVersion] set [TRV_IsDefaultVer]=1 where [TRV_ItemID]={0} and [ID] ={1}", trls[0].TR_ItemID, trv_versionID);
                        MyDBController.ExecuteNonQuery(SQl);
                    }
                    return MessageBox.Show("工艺路线信息保存成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ee)
                {

                    return MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                #endregion
            }
            else
            {
                return MessageBox.Show("工艺路线为空！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (txtb_ItemCode.Text.Length == 0 || txtb_ItemName.Text.Length == 0 ||
                txtb_ProcessName.Text.Length == 0 || txtb_TechSequence.Text.Length == 0 ||
                txtb_TechVersion.Text.Length == 0 || txtb_WorkCenterName.Text.Length == 0 ||
                txtb_WagePerPiece.Text.Length == 0 || cb_WageAllotScheme.SelectedValue == null)
            {
                MessageBox.Show("任何信息不能为空", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                if (Regex.IsMatch(txtb_TechSequence.Text.Trim(), User_Info.pattern[0]) &&
                    Regex.IsMatch(txtb_WagePerPiece.Text.Trim(), User_Info.pattern[1]))
                {
                    bool IsRightVersion = true;
                    bool NewOrNot = true;
                    if (listview1.Items != null)
                    {
                        foreach (TechRouteLists item in listview1.Items)
                        {
                            if (item.TRV_Version != txtb_TechVersion.Text.Trim())
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
                                TechRouteLists sel = cb_WageAllotScheme.SelectedItem as TechRouteLists;
                                int index = listview1.SelectedIndex;
                                trl = trls[index];
                                trl.TR_ProcessName = txtb_ProcessName.Text;
                                trl.TR_ProcessCode = choosedProcessCode == null ? trl.TR_ProcessCode : choosedProcessCode;
                                trl.TR_ProcessID = choosedProcessID == 0 ? trl.TR_ProcessID : choosedProcessID;
                                trl.TR_WorkCenterID = choosedWorkCenterID == 0 ? trl.TR_WorkCenterID : choosedWorkCenterID;
                                trl.TR_ProcessSequence = Convert.ToInt32(txtb_TechSequence.Text.Trim());
                                trl.WC_Department_Name = txtb_WorkCenterName.Text.Trim();
                                trl.TR_WagePerPiece = Convert.ToDecimal(txtb_WagePerPiece.Text.Trim());
                                trl.TR_WorkHour = 0;/*----------------------这段是预留的，工时根据单件工资计算---------------- */
                                trl.TRV_Version = txtb_TechVersion.Text.Trim();
                                trl.TR_WageAllotScheme = (int)cb_WageAllotScheme.SelectedValue;
                                trl.TR_WageAllotScheme_Show = sel.TR_WageAllotScheme_Show;
                                trl.TR_IsBackProcess = !(bool)radio_IsBackProcess_False.IsChecked;
                                trl.TR_IsTestProcess = !(bool)radio_IsTestProcess_False.IsChecked;
                            }
                            else//新工序
                            {
                                TechRouteLists sel = cb_WageAllotScheme.SelectedItem as TechRouteLists;
                                trl = new TechRouteLists();
                                trl.TR_ItemID = choosedItemID;
                                trl.TR_ItemCode = txtb_ItemCode.Text;
                                trl.II_Name = txtb_ItemName.Text.Trim();
                                trl.TR_ProcessName = txtb_ProcessName.Text;
                                trl.TR_ProcessCode = trls[0].TR_ProcessCode;
                                trl.TR_ProcessID = trls[0].TR_ProcessID;
                                trl.TR_WorkCenterID = trls[0].TR_WorkCenterID;
                                trl.TR_ProcessSequence = Convert.ToInt32(txtb_TechSequence.Text.Trim());
                                trl.WC_Department_Name = txtb_WorkCenterName.Text.Trim();
                                trl.TR_WagePerPiece = Convert.ToDecimal(txtb_WagePerPiece.Text.Trim());
                                trl.TR_WorkHour = 0;/*----------------------这段是预留的，工时根据单件工资计算---------------- */
                                trl.TRV_Version = txtb_TechVersion.Text.Trim();
                                trl.TR_WageAllotScheme = (int)cb_WageAllotScheme.SelectedValue;
                                trl.TR_WageAllotScheme_Show = sel.TR_WageAllotScheme_Show;
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
                            TechRouteLists sel = cb_WageAllotScheme.SelectedItem as TechRouteLists;
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
                            trl.TR_WagePerPiece = Convert.ToDecimal(txtb_WagePerPiece.Text.Trim());
                            trl.TR_WorkHour = 0;/*----------------------这段是预留的，工时根据单件工资计算---------------- */
                            trl.TRV_Version = txtb_TechVersion.Text.Trim();
                            trl.TR_WageAllotScheme = (int)cb_WageAllotScheme.SelectedValue;
                            trl.TR_WageAllotScheme_Show = sel.TR_WageAllotScheme_Show;
                            trl.TR_IsBackProcess = !(bool)radio_IsBackProcess_False.IsChecked;
                            trl.TR_IsTestProcess = !(bool)radio_IsTestProcess_False.IsChecked;
                            trls.Add(trl);
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
                    txtb_WagePerPiece.Text = "";
                cb_WageAllotScheme.SelectedItem = null;
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
                        txtb_TechVersion.Text = trl.TRV_Version;
                        txtb_WorkCenterName.Text = trl.WC_Department_Name;
                        txtb_WagePerPiece.Text = trl.TR_WagePerPiece.ToString();
                        cb_WageAllotScheme.SelectedValue = trl.TR_WageAllotScheme;
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
                listview1.Items.Refresh();
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

            this.Title = "工艺路线新增窗口";
            txtb_ItemName.IsReadOnly = txtb_ItemCode.IsReadOnly = true;
            txtb_ProcessName.Text = txtb_TechSequence.Text = txtb_WorkCenterName.Text
                = txtb_WagePerPiece.Text = txtb_TechVersion.Text = "";
            cb_WageAllotScheme.SelectedIndex = -1;
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            btn_Save_Click1(sender,e);
        }
    }
}
