using BarCodeSystem.ProductDispatch.FlowCardPrint;
using BarCodeSystem.ProductDispatch.FlowCardReport;
using BarCodeSystem.PublicClass;
using BarCodeSystem.PublicClass.DatabaseEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using Xceed.Wpf.AvalonDock.Layout;
using System.Linq;
using BarCodeSystem.PublicClass.HelperClass;
using System.Threading.Tasks;
using System.Threading;
using BarCodeSystem.SystemManage;
using System.Diagnostics;

namespace BarCodeSystem.ProductDispatch.FlowCard
{
    /// <summary>
    /// FlowCrad_Page.xaml 的交互逻辑
    /// </summary>
    public partial class FlowCard_Page : Page
    {
        public FlowCard_Page()
        {
            InitializeComponent();
            cardType = 0;
            cb_IsSalaryCalcuating.Visibility = Visibility.Hidden;
            textb_IsSalaryCalculating.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 返工流转卡的构造函数
        /// </summary>
        /// <param name="_fclList"></param>
        /// <param name="_fcqlList"></param>
        public FlowCard_Page(List<FlowCardLists> _fclList, List<FlowCardQualityLists> _fcqlList)
        {
            InitializeComponent();
            isReproduceFC = true;
            _reproduceFCList = _fclList;
            _reproduceFCQLList = _fcqlList;
            cardType = 3;
            DisplayInitReproduceInfo(_fclList, _fcqlList);
            amount = _fcqlList.Sum(p => p.FCQ_ScrapAmount).ToString();
            textb_POInfo.Visibility = Visibility.Hidden;
            sp_POInfo.Visibility = Visibility.Hidden;
        }

        #region 变量设置
        string amount = "";
        ProduceOrderLists selectedOrder = new ProduceOrderLists();
        List<TechRouteLists> selectedTechRoute = new List<TechRouteLists>();
        List<List<PersonLists>> techRoutePerson = new List<List<PersonLists>>();
        DispatcherTimer timer1 = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 2) };
        DataSet ds = new DataSet();
        int loadCount = 0;
        /// <summary>
        /// 是否是编制返工流转卡
        /// </summary>
        bool isReproduceFC = false;

        /// <summary>
        /// 返工流转卡的来源流转卡
        /// </summary>
        List<FlowCardLists> _reproduceFCList;
        /// <summary>
        /// 返工流转卡数据来源的质量问题
        /// </summary>
        List<FlowCardQualityLists> _reproduceFCQLList;
        /// <summary>
        /// 流转卡类型
        /// </summary>
        int cardType;
        #endregion

        #region 初始化设置
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                InitFlowCardHeader();
                timer1.Tick += timer1_Tick;
                loadCount++;
            }
        }

        /// <summary>
        /// 定时器跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tooltip_SaveAsTeam.IsOpen)
            {
                tooltip_SaveAsTeam.IsOpen = false;
                timer1.IsEnabled = false;
                tooltip_SaveAsTeam.Visibility = Visibility.Hidden;
            }
        }


        /// <summary>
        /// 初始化流转卡表头，为Combobox关联items。带入编制人员信息
        /// </summary>
        private void InitFlowCardHeader()
        {
            List<string> typeList = new List<string>();
            if (isReproduceFC)
            {
                btn_SourceOrderSearch.IsEnabled = false;
                typeList.Add("返工流转卡");
                textb_Amount.IsReadOnly = true;
            }
            else
            {
                typeList.Add("普通流转卡");
            }
            cb_FlowCardType.ItemsSource = typeList;
            textb_CreatedBy.Text = "  " + User_Info.User_Name;
            datepicker_CreateDate.SelectedDate = DateTime.Now;
        }

        /// <summary>
        /// 展示返工信息 
        /// </summary>
        /// <param name="_fclList"></param>
        /// <param name="_fcqlList"></param>
        private void DisplayInitReproduceInfo(List<FlowCardLists> _fclList, List<FlowCardQualityLists> _fcqlList)
        {
            FlowCardLists fcl = _fclList.FirstOrDefault();
            txtb_ProduceOrderInfo.Text = "";
            txtb_ItemInfo.Text = fcl.PO_ItemCode + " | " + fcl.PO_ItemName + " | " + fcl.PO_ItemSpec;
            string x = _fcqlList.Sum(p => p.FCQ_ScrapAmount).ToString();
            textb_Amount.Text = x;
            btn_TechRouteSearch_Click(null, null);
        }
        /// <summary>
        /// 页面大小改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            textb_CheckedBy.Width = textb_Amount.Width = datepicker_CreateDate.Width = txtb_ItemInfo.Width = fourthCol.ActualWidth * 0.9;
        }

        /// <summary>
        /// 表头大小改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardHeaderGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            textb_CheckedBy.Width = textb_Amount.Width = datepicker_CreateDate.Width = txtb_ItemInfo.Width = fourthCol.ActualWidth * 0.9;
        }
        #endregion

        #region 流转卡表头操作
        /// <summary>
        /// 料品信息查询按钮(逻辑修改后程序没有用到这部分)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ItemSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!textb_SearchInfo.Text.Equals("料品筛选"))
            {
                Frame frame_SearchInfo = new Frame();
                gb_SearchInfo.Content = frame_SearchInfo;
                textb_SearchInfo.Text = "料品筛选";
                ItemSearch_Page isp = new ItemSearch_Page(SetItemInfo);
                frame_SearchInfo.Navigate(isp);
            }
        }

        /// <summary>
        /// 料品信息查询函数委托实例(逻辑修改后程序没有用到这部分)
        /// </summary>
        /// <returns></returns>
        public string SetItemInfo(string value)
        {
            this.txtb_ItemInfo.Text = value;
            return value;
        }

        /// <summary>
        /// 料品工艺路线信息查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TechRouteSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtb_ItemInfo.Text))
            {
                string itemCode = txtb_ItemInfo.Text.Split('|')[0].Trim();
                if (Regex.IsMatch(itemCode, User_Info.pattern[0]))
                {
                    if (!textb_SearchInfo.Text.Equals("工艺路线筛选"))
                    {
                        Frame frame_SearchInfo = new Frame();
                        gb_SearchInfo.Content = frame_SearchInfo;
                        textb_SearchInfo.Text = "工艺路线筛选";
                        TechRouteSearch_Page trsp = new TechRouteSearch_Page(itemCode, FecthTechRouteInfo, isReproduceFC);
                        frame_SearchInfo.Navigate(trsp);
                    }
                }
            }
            else
            {
            }
        }

        /// <summary>
        /// 工艺路线查询委托函数实例
        /// </summary>
        /// <param name="value"></param>
        /// <param name="trls"></param>
        private void FecthTechRouteInfo(string value, List<TechRouteLists> trls)
        {
            selectedTechRoute = trls;
            txtb_TechRouteVersion.Text = value;
            datagrid_TechRouteInfo.ItemsSource = trls;
            txtb_Department.Text = trls[0].WC_Department_Name;

            #region 根据工艺路线信息初始化人员列表
            techRoutePerson.Clear();
            for (int i = 0; i < trls.Count; i++)
            {
                techRoutePerson.Add(new List<PersonLists>());
                //techRouteWT.Add(new List<WorkTeamLists>());
            }
            #endregion
        }

        /// <summary>
        /// 生产订单查询按钮，只能查询 订单数量>派工数量 的生产订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SourceOrderSearch_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (!textb_SearchInfo.Text.Equals("生产订单筛选"))
            {
                ProduceOrderSearch_Page posp = new ProduceOrderSearch_Page(FecthProduceOrderInfo);
                Frame frame_SearchInfo = new Frame();
                gb_SearchInfo.Content = frame_SearchInfo;
                textb_SearchInfo.Text = "生产订单筛选";
                frame_SearchInfo.Navigate(posp);
            }
            this.Cursor = Cursors.Arrow;
        }


        /// <summary>
        /// 生产订单查询委托函数实例
        /// </summary>
        /// <param name="value"></param>
        /// <param name="trls"></param>
        private void FecthProduceOrderInfo(ProduceOrderLists pol)
        {
            selectedOrder = pol;
            txtb_ProduceOrderInfo.Text = pol.PO_Code;
            txtb_ItemInfo.Text = pol.PO_ItemCode + " | " + pol.PO_ItemName + " | " + pol.PO_ItemSpec;
            textb_Amount.Text = (pol.PO_OrderAmount - pol.PO_StartAmount).ToString();
            btn_TechRouteSearch_Click(null, null);
        }

        /// <summary>
        /// 不能输入非数字的字符，不能输入超过可派工总数的数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textb_Amount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textb_Amount.Text))
            {
                if (!isReproduceFC)
                {
                    if (Regex.IsMatch(textb_Amount.Text, User_Info.pattern[0]))
                    {

                        if (Convert.ToInt32(textb_Amount.Text) <= (selectedOrder.PO_OrderAmount - selectedOrder.PO_StartAmount))
                        {
                            toolTip_Amount.Visibility = Visibility.Hidden;
                            amount = textb_Amount.Text;
                            textb_Amount.SelectionStart = textb_Amount.Text.Length;
                        }
                        else
                        {
                            textb_Amount.Text = amount;
                            toolTip_Amount.Visibility = Visibility.Visible;
                            toolTip_Amount.IsOpen = true;
                        }
                    }
                    else
                    {
                        textb_Amount.Text = amount;
                        toolTip_Amount.Visibility = Visibility.Hidden;
                        textb_Amount.SelectionStart = textb_Amount.Text.Length;
                    }
                }
            }
            else
            {
            }

        }
        /// <summary>
        /// 料品信息改变事件，自动获取其工艺路线信息，并且将默认工艺路线信息带入行表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_ItemInfo_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        #endregion

        #region 行表信息

        /// <summary>
        /// 查询人员信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PersonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!textb_SearchInfo.Text.Equals("人员信息筛选"))
            {
                PersonSearch_Page ps = new PersonSearch_Page(FetchPersonInfo);
                Frame frame_SearchInfo = new Frame();
                gb_SearchInfo.Content = frame_SearchInfo;
                textb_SearchInfo.Text = "人员信息筛选";
                frame_SearchInfo.Navigate(ps);
            }
        }
        /// <summary>
        /// 接收人员信息的委托
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        private void FetchPersonInfo(List<PersonLists> personList)
        {
            if (datagrid_TechRouteInfo.SelectedIndex != -1)
            {
                try
                {
                    foreach (PersonLists person in personList)
                    {
                        if (!techRoutePerson[datagrid_TechRouteInfo.SelectedIndex].Exists(p => p.code.Equals(person.code)))
                        {
                            int count = techRoutePerson[datagrid_TechRouteInfo.SelectedIndex].Count;
                            ((TechRouteLists)datagrid_TechRouteInfo.SelectedItem).personName += count == 0 ? person.name : "、" + person.name;
                            techRoutePerson[datagrid_TechRouteInfo.SelectedIndex].Add(person);
                        }
                        datagrid_TechRouteInfo.Items.Refresh();
                    }
                }
                catch (Exception ee)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion

        #region 生成流转卡编号并派工
        /// <summary>
        /// 流转卡派工
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DisFlowCard_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            string message = "";

            List<string> WT_NameList = new List<string>();
            foreach (var item in techRoutePerson)
            {
                string WT_Name = WorkTeamLists.CheckIfMultiPersonMatchWT(item);
                if (string.IsNullOrEmpty(WT_Name))
                {

                }
                else
                    WT_NameList.Add(WT_Name);
            }

            if (CheckIfCanLegal(out message))
            {
                int flowNum = GetCurrentFlowNum();
                string flowCode = GenerateCode(flowNum);
                List<FlowCardLists> flowCardList = GenerateFlowCardInfo(flowNum, flowCode);

                bool flag = FlowCardInfoToDatabase(flowCardList);
                if (flag)
                {
                    UpdateSourceOrderInfo(Convert.ToInt32(textb_Amount.Text));
                    FlowCardSubIntoDatabase(flowCode);
                    SwitchReadOnlyPro();
                    RemoveFCReportFrame();
                    if (isReproduceFC)
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        UpdateSourceFCInfo(flowCardList);
                        sw.Stop();
                        MessageBox.Show(sw.Elapsed.ToString());
                    }
                    Xceed.Wpf.Toolkit.MessageBox.Show("派工完成！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 当时返工流转卡派工的时候，更新来源流转卡的状态
        /// </summary>
        private void UpdateSourceFCInfo(List<FlowCardLists> _flowCardList)
        {
            _reproduceFCQLList.ForEach(p => { p.FCQ_Remark = "去往返工流转卡" + _flowCardList[0].FC_Code; p.FCQ_HasReproduced = true; });
            FlowCardQualityLists.SaveInfo(_reproduceFCQLList);
            MyDBController.GetConnection();

            //foreach (FlowCardLists p in _reproduceFCList)
            //{
            //    p.FC_HasReproduced = true;
            //    p.FC_CanReproduce = FlowCardLists.CheckWhetherCanReproduce(p, MyDBController.CreateSqlCon());
            //    p.FC_Remark += "去往返工流转卡" + _flowCardList[0].FC_Code;
            //}

            Parallel.ForEach(_reproduceFCList, p =>
            {
                p.FC_HasReproduced = true;
                p.FC_CanReproduce = FlowCardLists.CheckWhetherCanReproduce(p, MyDBController.CreateSqlCon());
                p.FC_Remark += "去往返工流转卡" + _flowCardList[0].FC_Code;
                //Trace.WriteLine(Thread.CurrentThread.ManagedThreadId);
            });

            //_reproduceFCList.AsParallel().ToList().ForEach(p => { p.FC_HasReproduced = true; p.FC_CanReproduce = FlowCardLists.CheckWhetherCanReproduce(p, new SqlConnection() { ConnectionString = MyDBController.M_scn_myConn.ConnectionString }); p.FC_Remark += "去往返工流转卡" + _flowCardList[0].FC_Code; });
            MyDBController.CloseConnection();
            FlowCardLists.SaveInfo(_reproduceFCList);
        }

        /// <summary>
        /// 移除内容
        /// </summary>
        private void RemoveFCReportFrame()
        {
            foreach (LayoutAnchorable item in (MyDBController.FindVisualParent<Main_Window>(this)[0]).ldp.Children)
            {
                if (item.Title == "流转卡报工")
                {
                    ((FlowCardReport_Page)((Frame)item.Content).Content).searchFrame.Content = null;
                    ((FlowCardReport_Page)((Frame)item.Content).Content).textb_SearchInfo.Text = "";
                }
            }

        }
        /// <summary>
        /// 检查当前流转卡信息是否满足派工条件，满足返回true否则返回false
        /// </summary>
        /// <returns></returns>
        private bool CheckIfCanLegal(out string message)
        {
            message = "";
            bool flag = true;

            if (selectedTechRoute.Count != 0)
            {
                if (string.IsNullOrEmpty(selectedOrder.PO_ItemCode) && !isReproduceFC)
                {
                    message = "缺少生产订单信息，请检查!";
                    flag = false;
                }
            }
            else
            {
                message = "缺少工艺路线信息,请检查！";
                flag = false;
            }
            #region 派工的时候人员检验机制，需要保证每道工序都有人员，现在被拿掉。
            //if (flag)
            //{
            //    foreach (TechRouteLists item in selectedTechRoute)
            //    {
            //        if (string.IsNullOrEmpty(item.personName))
            //        {
            //            flag = false;
            //            message = "请为每道工序选择操作人员！";
            //            break;
            //        }
            //    }
            //}
            #endregion
            return flag;
        }

        /// <summary>
        /// 获得条码系统中当前日期的流转卡最大流水号，并加1，用来作为新的流转卡流水号
        /// </summary>
        /// <returns></returns>
        private int GetCurrentFlowNum()
        {
            int currentFlowNum = 0;
            string SQl = string.Format(@"select max(FC_FlowNum) FC_FlowNum from FlowCard where convert(date,FC_Createtime,102)=Convert(date,getDate(),102) ");
            MyDBController.GetConnection();
            SqlDataReader sqlReader = MyDBController.GetDataReader(SQl);
            while (sqlReader.Read())
            {
                if (sqlReader["FC_FlowNum"].GetType() != typeof(DBNull))
                {
                    currentFlowNum = Convert.ToInt32(sqlReader["FC_FlowNum"]) + 1;
                }
            }
            sqlReader.Close();
            MyDBController.CloseConnection();
            return currentFlowNum;
        }

        /// <summary>
        /// 生成流转卡流水号
        /// </summary>
        /// <param name="maxNum">四位尾号</param>
        /// <param name="type">流转卡类型缩写</param>
        /// <returns></returns>
        private string GenerateCode(int maxNum)
        {
            string type = "";
            switch (cb_FlowCardType.SelectedValue.ToString())
            {
                case "普通流转卡":
                    type = "PT";
                    break;
                case "分批流转卡":
                    type = "FP";
                    break;
                case "返工流转卡":
                    type = "FG";
                    break;
                case "无来源流转卡":
                    type = "WLY";
                    break;
                default:
                    break;
            }
            txtb_FlowCode.Text = type + "-" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + "-" + string.Format("{0:0000}", maxNum);
            return type + "-" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + "-" + string.Format("{0:0000}", maxNum);
        }

        /// <summary>
        /// 生成流转卡表头信息
        /// </summary>
        private List<FlowCardLists> GenerateFlowCardInfo(int maxNum, string flowCode)
        {
            List<FlowCardLists> flowCardList = new List<FlowCardLists>();

            flowCardList.Add(new FlowCardLists()
            {
                FC_CardType = cardType,
                FC_SourceOrderID = isReproduceFC ? -1 : selectedOrder.PO_ID,
                FC_ItemID = isReproduceFC ? _reproduceFCList.FirstOrDefault().FC_ItemID : Convert.ToInt64(selectedOrder.PO_ItemID),
                FC_ItemTechVersionID = Convert.ToInt64(selectedTechRoute[0].TR_VersionID),
                FC_Amount = Convert.ToInt32(textb_Amount.Text),
                FC_WorkCenter = selectedTechRoute[0].TR_WorkCenterID,
                FC_CardState = 0,
                FC_DistriSourceCard = -1,
                FC_FlowNum = maxNum,
                FC_CreateBy = User_Info.User_Name,
                FC_CreateTime = DateTime.Now,
                FC_Code = flowCode,
                FC_BCSOrderID = isReproduceFC ? -1 : selectedOrder.ID,
                FC_FirstProcessNum = selectedTechRoute.FirstOrDefault().TR_ProcessSequence,
                FC_CanDistribute = true,
                FC_CanReproduce = false,
                FC_CanTransfer = false,
                FC_CheckBy = "",
                FC_Remark = "",
                FC_CheckTime = DateTime.Now,
                FC_HasDistributed = false,
                FC_HasReproduced = false,
                FC_HasTransfered = false,
                FC_IsSalaryCalculating = isReproduceFC ? (cb_IsSalaryCalcuating.IsChecked == true) : true
            });
            return flowCardList;
        }

        /// <summary>
        /// 将流转卡信息存入数据库FlowCard表
        /// </summary>
        private bool FlowCardInfoToDatabase(List<FlowCardLists> flowCardList)
        {
            try
            {
                bool flag = FlowCardLists.SaveInfo(flowCardList);
                if (flag)
                {
                    Int64 _fcID = FlowCardLists.FetchFC_InfoByCode(flowCardList[0].FC_Code)[0].ID;
                    List<ReproduceFlowCardRemarkLists> _rfcrList = new List<ReproduceFlowCardRemarkLists>();
                    if (isReproduceFC)
                    {

                        _reproduceFCQLList.ForEach(
                            p =>
                            {
                                ReproduceFlowCardRemarkLists rfcr = new ReproduceFlowCardRemarkLists();
                                rfcr.RFCR_FlowCardID = _fcID;
                                rfcr.RFCR_FlowCardQualityID = p.ID;
                                _rfcrList.Add(rfcr);
                            }
                            );

                        ReproduceFlowCardRemarkLists.SaveInfo(_rfcrList);

                    }
                }
                return flag;
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// 流转卡行表信息进入数据库FlowCardSub表
        /// </summary>
        /// <param name="flowCode"></param>
        private void FlowCardSubIntoDatabase(string flowCode)
        {
            Int64 flowCardID = 0;
            String SQl = string.Format(@"Select ID from [FlowCard] where FC_Code='{0}'", flowCode);
            MyDBController.GetConnection();
            SqlDataReader reader = MyDBController.GetDataReader(SQl);
            while (reader.Read())
            {
                flowCardID = Convert.ToInt64(reader[0]);
            }
            reader.Close();
            SQl = string.Format(@"select top 0 * from [FlowCardSub]");
            MyDBController.GetDataSet(SQl, ds, "FlowCardSub");

            List<string> colList = new List<string>();
            foreach (DataColumn col in ds.Tables["FlowCardSub"].Columns)
            {
                colList.Add(col.ColumnName);
            }

            ds.Tables["FlowCardSub"].Columns.Add(new DataColumn("IDNew", typeof(Int64)));

            //生成流转卡子表的信息，识别是否按照班组派工
            for (int i = 0; i < selectedTechRoute.Count; i++)
            {
                #region 如果不是以班组为单位派工

                for (int j = 0; j < techRoutePerson[i].Count; j++)
                {
                    DataRow row = ds.Tables["FlowCardSub"].NewRow();
                    row["FCS_FlowCardID"] = flowCardID;
                    row["FCS_ItemId"] = selectedTechRoute[i].TR_ItemID;
                    row["FCS_TechRouteID"] = selectedTechRoute[i].ID;
                    row["FCS_ProcessID"] = selectedTechRoute[i].TR_ProcessID;
                    row["FCS_ProcessName"] = selectedTechRoute[i].TR_ProcessName;
                    row["FCS_PersonCode"] = techRoutePerson[i][j].code;
                    row["FCS_PersonName"] = techRoutePerson[i][j].name;
                    #region 这段代码为初始化代码，这些数据在派工的时候为空置的
                    row["FCS_QulifiedAmount"] = 0;
                    row["FCS_ScrappedAmount"] = 0;
                    row["FCS_UnprocessedAm"] = 0;
                    row["FCS_CheckByID"] = 0;
                    row["FCS_BeginAmount"] = 0;
                    row["FCS_CheckByName"] = 0;
                    row["FCS_PieceAmount"] = 0;
                    row["FCS_PieceDivNum"] = 0;
                    row["FCS_AddAmount"] = 0;
                    #endregion
                    row["FCS_IsFirstProcess"] = Convert.ToBoolean(selectedTechRoute[i].TR_IsFirstProcess);
                    row["FCS_IsLastProcess"] = Convert.ToBoolean(selectedTechRoute[i].TR_IsLastProcess);
                    row["FCS_IsReported"] = false;
                    //row["FCS_IsWorkTeam"] = false;
                    //row["FCS_WorkTeamID"] = -1;
                    ds.Tables["FlowCardSub"].Rows.Add(row);
                }
                #endregion

                #region 当是以班组为单位派工时
                //else if (techRouteWT[i].Count > 0)
                //{
                //    DataRow row = ds.Tables["FlowCardSub"].NewRow();
                //    row["FCS_FlowCardID"] = flowCardID;
                //    row["FCS_ItemId"] = selectedTechRoute[i].TR_ItemID;
                //    row["FCS_TechRouteID"] = selectedTechRoute[i].ID;
                //    row["FCS_ProcessID"] = selectedTechRoute[i].TR_ProcessID;
                //    row["FCS_ProcessName"] = selectedTechRoute[i].TR_ProcessName;
                //    row["FCS_PersonCode"] = "";
                //    row["FCS_PersonName"] = "";
                //    #region 这段代码为初始化代码，这些数据在派工的时候为空置的
                //    row["FCS_QulifiedAmount"] = 0;
                //    row["FCS_ScrappedAmount"] = 0;
                //    row["FCS_UnprocessedAm"] = 0;
                //    row["FCS_CheckByID"] = 0;
                //    row["FCS_BeginAmount"] = 0;
                //    row["FCS_CheckByName"] = 0;
                //    row["FCS_PieceAmount"] = 0;
                //    row["FCS_PieceDivNum"] = 0;
                //    row["FCS_AddAmount"] = 0;
                //    #endregion
                //    row["FCS_IsFirstProcess"] = Convert.ToBoolean(selectedTechRoute[i].TR_IsFirstProcess);
                //    row["FCS_IsLastProcess"] = Convert.ToBoolean(selectedTechRoute[i].TR_IsLastProcess);
                //    row["FCS_IsReported"] = false;
                //    row["FCS_IsWorkTeam"] = true;
                //    row["FCS_WorkTeamID"] = Convert.ToInt64(techRouteWT[i][0].ID);
                //    ds.Tables["FlowCardSub"].Rows.Add(row);
                //}
                #endregion

            }
            int updateNum, insertNum;
            MyDBController.InsertSqlBulk(ds.Tables["FlowCardSub"], colList, out updateNum, out insertNum);
            MyDBController.CloseConnection();
        }
        /// <summary>
        /// 派工结束之后返填生产订单信息，更新派工数量
        /// </summary>
        /// <param name="para"></param>
        private void UpdateSourceOrderInfo(int para)
        {
            string SQl = string.Format(@"update [ProduceOrder] set [PO_StartAmount]=[PO_StartAmount]+{0} where [ID]={1}", para, selectedOrder.ID);
            MyDBController.GetConnection();
            MyDBController.ExecuteNonQuery(SQl);
            MyDBController.CloseConnection();
        }

        /// <summary>
        ///派工后将当前页面设置为只读状态 
        /// </summary>
        private void SwitchReadOnlyPro()
        {
            Panel.SetZIndex(img_Watermark, 1);
            datagrid_TechRouteInfo.IsReadOnly = true;

            foreach (Button item in MyDBController.FindVisualChild<Button>(fatherGrid))
            {
                if (item.Name.Equals("btn_Refresh"))
                {
                }
                else if (item.Name.Equals("btn_Print"))
                {
                }
                else
                    item.IsEnabled = false;
            }
            btn_Print.Visibility = Visibility.Visible;
            gb_SearchInfo.Content = null;
            cb_FlowCardType.IsEnabled = false;
        }
        #endregion

        #region 班组信息操作
        /// <summary>
        /// 将选中的行的人员保存为班组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveAsTeam_Click(object sender, RoutedEventArgs e)
        {
            bool canSave = CheckIfProcessSelected();
            if (canSave)
            {
                textb_SearchInfo.Text = "保存班组信息";
                SaveWorkTeam_Page swp = new SaveWorkTeam_Page(techRoutePerson[datagrid_TechRouteInfo.SelectedIndex], selectedTechRoute[datagrid_TechRouteInfo.SelectedIndex].TR_WorkCenterID, selectedTechRoute[datagrid_TechRouteInfo.SelectedIndex].WC_Department_Name);
                Frame frame_SaveTeam = new Frame();
                gb_SearchInfo.Content = frame_SaveTeam;
                frame_SaveTeam.Navigate(swp);
            }
        }

        /// <summary>
        /// 点击保存班组的时候，检查是否选择了工序。选择了工序就继续往下走，否则停住
        /// </summary>
        private bool CheckIfProcessSelected()
        {
            int count = datagrid_TechRouteInfo.SelectedIndex;
            if (count != -1)
            {
                timer1.IsEnabled = false;
                if (tooltip_SaveAsTeam.IsOpen)
                {
                    tooltip_SaveAsTeam.IsOpen = false;
                }
                return true;
            }
            else
            {
                tooltip_SaveAsTeam.Visibility = Visibility.Visible;
                tooltip_SaveAsTeam.IsOpen = true;
                if (!timer1.IsEnabled)
                {
                    timer1.IsEnabled = true;
                }
                return false;
            }
        }

        /// <summary>
        /// 选取班组按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_GetTeam_Click(object sender, RoutedEventArgs e)
        {
            if (!textb_SearchInfo.Text.Equals("选取班组信息"))
            {
                try
                {
                    textb_SearchInfo.Text = "选取班组信息";
                    SaveWorkTeam_Page swtp = new SaveWorkTeam_Page(selectedTechRoute[0].TR_WorkCenterID, FectchTeamPerson);
                    Frame frameSearch = new Frame();
                    //frameSearch.Content = swtp;
                    gb_SearchInfo.Content = frameSearch;
                    frameSearch.Navigate(swtp);
                }
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        /// 接收筛选班组的人员信息的函数
        /// </summary>
        /// <param name="wtl"></param>
        private void FectchTeamPerson(WorkTeamLists wtl, List<WorkTeamMemberLists> wtmList)
        {
            if (datagrid_TechRouteInfo.SelectedIndex != -1)
            {
                int x = datagrid_TechRouteInfo.SelectedIndex;

                if (techRoutePerson[x].Count > 0)
                {
                    if (Xceed.Wpf.Toolkit.MessageBox.Show("该工序已经安排人员了，是否覆盖？\r\n点击yes选择覆盖，点击no选择放弃。", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        techRoutePerson[x].Clear();
                        foreach (var item in wtmList)
                        {
                            techRoutePerson[x].Add(new PersonLists() { code = item.WTM_MemberPersonCode, name = item.WTM_MemberPersonName, ID = item.WTM_MemberPersonID });
                        }
                        ((TechRouteLists)datagrid_TechRouteInfo.SelectedItem).personName = wtl.WT_Name;
                        datagrid_TechRouteInfo.Items.Refresh();
                    }
                }
                else
                {
                    foreach (var item in wtmList)
                    {
                        techRoutePerson[x].Add(new PersonLists() { code = item.WTM_MemberPersonCode, name = item.WTM_MemberPersonName, ID = item.WTM_MemberPersonID });
                    }
                    ((TechRouteLists)datagrid_TechRouteInfo.SelectedItem).personName = wtl.WT_Name;
                    datagrid_TechRouteInfo.Items.Refresh();

                    datagrid_TechRouteInfo.Items.Refresh();
                }
            }
        }
        #endregion

        #region 生成派工方案、获取派工方案
        /// <summary>
        /// 获取派工方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_GetDisPlan_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTechRoute.Count > 0)
            {
                if (!textb_SearchInfo.Text.Equals("筛选派工方案信息"))
                {
                    textb_SearchInfo.Text = "筛选派工方案信息";
                    SaveDisPlan_Page sdp = new SaveDisPlan_Page(selectedTechRoute[0].TR_VersionID, selectedTechRoute[0].TR_ItemID, FillDisPlan);
                    Frame frame_Search = new Frame() { };
                    gb_SearchInfo.Content = frame_Search;
                    frame_Search.Navigate(sdp);
                }
            }

        }

        /// <summary>
        /// 将选取的派工方案填到行表中
        /// </summary>
        /// <param name="list"></param>
        private void FillDisPlan(List<DisPlanLists> list)
        {
            for (int i = 0; i < selectedTechRoute.Count; i++)
            {
                foreach (DisPlanLists item in list.FindAll(p => p.DP_ProcessSequence.Equals(selectedTechRoute[i].TR_ProcessSequence)))
                {
                    int count = techRoutePerson[i].Count;
                    selectedTechRoute[i].personName += count == 0 ? item.DP_PersonName : "、" + item.DP_PersonName;
                    techRoutePerson[i].Add(new PersonLists()
                    {
                        code = item.DP_PersonCode,
                        name = item.DP_PersonName,
                        ID = item.DP_PersonID
                    });
                }
            }
            datagrid_TechRouteInfo.Items.Refresh();
        }
        /// <summary>
        /// 保存派工方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveAsDisPlan_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTechRoute.Count > 0)
            {
                if (!textb_SearchInfo.Text.Equals("保存派工方案信息"))
                {
                    textb_SearchInfo.Text = "保存派工方案信息";
                    SaveDisPlan_Page sdp = new SaveDisPlan_Page(GenerateDisPlan(), GenerateDisPlanVersion());
                    Frame frame_Search = new Frame() { };
                    gb_SearchInfo.Content = frame_Search;
                    frame_Search.Navigate(sdp);
                }
            }

        }

        /// <summary>
        /// 生成当前的派工方案信息
        /// </summary>
        /// <returns></returns>
        private List<DisPlanLists> GenerateDisPlan()
        {
            List<DisPlanLists> disPlanList = new List<DisPlanLists>();
            try
            {
                foreach (TechRouteLists trl in selectedTechRoute)
                {
                    int index = selectedTechRoute.FindIndex(p => p.Equals(trl));
                    if (techRoutePerson[index].Count > 0)
                    {
                        foreach (PersonLists person in techRoutePerson[index])
                        {
                            disPlanList.Add(new DisPlanLists()
                            {
                                DP_ProcessSequence = trl.TR_ProcessSequence,
                                DP_ProcessName = trl.TR_ProcessName,
                                DP_PersonID = person.ID,
                                DP_PersonCode = person.code,
                                DP_PersonName = person.name,
                                DP_TechRouteID = trl.ID,
                            });
                        }
                    }

                }
            }
            catch (Exception)
            {
            }
            return disPlanList;
        }

        /// <summary>
        /// 生成当前派工方案版本
        /// </summary>
        /// <returns></returns>
        private List<DisPlanVersionLists> GenerateDisPlanVersion()
        {
            try
            {
                List<DisPlanVersionLists> disPlanVersion = new List<DisPlanVersionLists>();
                disPlanVersion.Add(new DisPlanVersionLists()
                 {
                     DPV_ItemID = selectedTechRoute[0].TR_ItemID,
                     DPV_TechRouteVersionID = selectedTechRoute[0].TR_VersionID,
                     DPV_ItemCode = selectedTechRoute[0].TR_ItemCode,
                     DPV_ItemName = selectedTechRoute[0].II_Name,
                     DPV_TechRouteVersionName = selectedTechRoute[0].TRV_VersionCode
                 });
                return disPlanVersion;
            }
            catch (Exception)
            {
                return new List<DisPlanVersionLists>();
            }
        }

        #endregion

        /// <summary>
        /// 右键删除行记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_DeleteRow_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_TechRouteInfo.SelectedIndex != -1)
            {
                if (Xceed.Wpf.Toolkit.MessageBox.Show("确定要删除该行吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    int index = datagrid_TechRouteInfo.SelectedIndex;
                    datagrid_TechRouteInfo.ItemsSource = ((List<TechRouteLists>)datagrid_TechRouteInfo.ItemsSource).FindAll(p => !p.TR_ProcessSequence.Equals(((TechRouteLists)datagrid_TechRouteInfo.SelectedItem).TR_ProcessSequence));
                    techRoutePerson.RemoveAt(index);
                    selectedTechRoute.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Print_Click(object sender, RoutedEventArgs e)
        {
            //EmptyFlowCard_Window efc = new EmptyFlowCard_Window(txtb_FlowCode.Text);
            //efc.ShowDialog();
            PrintTemplateSelect_Window pts = new PrintTemplateSelect_Window(txtb_FlowCode.Text);
            pts.ShowDialog();
            //_10LinesFlowCard_Window _10lfc = new _10LinesFlowCard_Window(txtb_FlowCode.Text);
            //_10lfc.ShowDialog();
        }

        /// <summary>
        /// 右键重置行人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_ResetRow_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("确定要重置吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                techRoutePerson[datagrid_TechRouteInfo.SelectedIndex].Clear();
                ((TechRouteLists)datagrid_TechRouteInfo.SelectedItem).personName = "";
                datagrid_TechRouteInfo.Items.Refresh();
            }
        }


        /// <summary>
        /// 右键重置全部行人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_ResetAll_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("确定要重置吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                techRoutePerson.ForEach(t => t.Clear());
                foreach (TechRouteLists item in datagrid_TechRouteInfo.ItemsSource)
                {
                    item.personName = "";
                }
                datagrid_TechRouteInfo.Items.Refresh();
            }
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            amount = "";
            selectedOrder = new ProduceOrderLists();
            selectedTechRoute = new List<TechRouteLists>();
            techRoutePerson = new List<List<PersonLists>>();
            ds = new DataSet();
            gb_SearchInfo.Content = null;
            datagrid_TechRouteInfo.IsEnabled = true;
            datagrid_TechRouteInfo.ItemsSource = null;
            txtb_ProduceOrderInfo.Text = txtb_TechRouteVersion.Text = "点击放大镜选择";
            txtb_ItemInfo.Text = "来自生产订单";
            txtb_Department.Text = "系统自动生成";
            textb_Amount.Text = textb_SearchInfo.Text = "";
            Panel.SetZIndex(img_Watermark, 0);
            cb_FlowCardType.IsEnabled = true;
            txtb_FlowCode.Text = "系统自动生成";
            btn_Print.Visibility = Visibility.Hidden;
            foreach (Button item in MyDBController.FindVisualChild<Button>(fatherGrid))
            {
                if (item.Name.Equals("btn_Refresh"))
                {
                }
                else if (item.Name.Equals("btn_Print"))
                {
                }
                else
                    item.IsEnabled = true;
            }
        }

        /// <summary>
        /// 是否计算工资选项改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_IsSalaryCalcuating_Click(object sender, RoutedEventArgs e)
        {
            if (cb_IsSalaryCalcuating.IsChecked == false)
            {
                MessageBoxResult mbr = Xceed.Wpf.Toolkit.MessageBox.Show("确定不计算工资吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mbr == MessageBoxResult.Yes)
                {
                }
                else
                {
                    cb_IsSalaryCalcuating.IsChecked = true;
                }
            }
        }
    }
}
