using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.ValueConverters;
using BarCodeSystem.TechRoute.TechRoute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using System.ComponentModel;
using BarCodeSystem.PublicClass.HelperClass;
using BarCodeSystem.PublicClass;
using System.Threading.Tasks;
using BarCodeSystem.SystemManage;
using System.Threading;
using System.Reflection;
using BarCodeSystem.FileQuery.FlowCardQuery;

namespace BarCodeSystem.ProductDispatch.FlowCardReport
{
    /// <summary>
    /// FlowCardReport_Page.xaml 的交互逻辑
    /// </summary>
    public partial class FlowCardReport_Page : Page
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FlowCardReport_Page()
        {
            InitializeComponent();
        }

        #region 变量
        /// <summary>
        /// 流转卡子表信息列表
        /// </summary>
        List<FlowCardSubLists> fcsls = new List<FlowCardSubLists>();

        /// <summary>
        /// 质量信息列表数据源
        /// </summary>
        List<FlowCardQualityLists> list = new List<FlowCardQualityLists>();

        /// <summary>
        /// 人员信息列表数据源
        /// </summary>
        List<FlowCardSubLists> personDataSource = new List<FlowCardSubLists>();

        /// <summary>
        /// 工艺路线列表,BindProcess函数里面使用
        /// </summary>
        List<TechRouteLists> trList = new List<TechRouteLists>();

        /// <summary>
        /// 流转卡主表信息
        /// </summary>
        FlowCardLists fcl = new FlowCardLists();

        /// <summary>
        /// 流转卡工艺路线版本
        /// </summary>
        TechVersion tvFlowCard = new TechVersion();

        /// <summary>
        /// 用来判断报废数量信息是否保存到数据源列表中，false代表否，true代表是
        /// </summary>
        bool isAmountSaved = true;

        /// <summary>
        /// 是否新流转卡
        /// </summary>
        public bool isNewFlowCard = false;
        /// <summary>
        /// 当前工序是否为平分工序
        /// </summary>
        public bool isEquallyDiviedProcess = true;
        /// <summary>
        /// 用来判断是否对数据库做出了改变
        /// </summary>
        public bool isChanged = false;

        /// <summary>
        /// 当前工序在数量更改后是否已经报工
        /// </summary>
        bool isReported = false;

        /// <summary>
        /// 是否所有工序都报工完毕
        /// </summary>
        public bool isFinished = false;

        /// <summary>
        /// 所有工序报工完毕之后是否进行入库操作
        /// </summary>
        public bool isAfterHandled = false;

        /// <summary>
        /// 数据源处理次数，每报工一次，该次数加1，离散报工的时候每报工一次，工序下拉框数据源都会改变，而流水线报工不需要更改。这个用来做判断
        /// 流水线只需要第一次处理就好
        /// </summary>
        int handledcount = 0;

        /// <summary>
        /// 工序下拉框的选中index
        /// </summary>
        int lastindex = 0;

        /// <summary>
        /// 上次的工序号
        /// </summary>
        int lastSequence = 0;
        /// <summary>
        /// 在未保存已更改数量的情况下，提示错误信息的次数，流水线报工的时候用的
        /// </summary>
        int selectErrorCount = 0;
        /// <summary>
        /// 加载次数
        /// </summary>
        int loadCount = 0;

        #endregion

        #region 初始化
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                textb_FlowCard.Focus();
                SetVisibleProperties(isEquallyDiviedProcess);
                SetBinding(true);
                loadCount++;
            }
        }

        /// <summary>
        /// 将合格数量自动计算绑定
        /// </summary>
        private void SetBinding(bool _isLoading = false)
        {
            if (_isLoading)
            {
                Binding bd1 = new Binding("Value") { Source = txtb_BeginAmount };
                Binding bd2 = new Binding("Text") { Source = txtb_ScrappedAmount };
                Binding bd3 = new Binding("Value") { Source = txtb_UnprocessedAm };
                Binding bd4 = new Binding("Text") { Source = txtb_AddAmount };
                MultiBinding mb = new MultiBinding();
                mb.Converter = new QualifiedAmountConverter();
                mb.Bindings.Add(bd1);
                mb.Bindings.Add(bd2);
                mb.Bindings.Add(bd3);
                mb.Bindings.Add(bd4);
                txtb_QulifiedAmount.SetBinding(TextBox.TextProperty, mb);

            }
            Binding bd5 = new Binding("HasItems") { };
            if (isEquallyDiviedProcess)
            {
                bd5.Source = datagrid_PersonInfo;
            }
            else
            {
                bd5.Source = dg_UnquallyDivideProcessInfo;
            }
            bd5.Converter = new FlowCardHasItemsConverter();
            txtb_UnprocessedAm.SetBinding(IntegerUpDown.IsReadOnlyProperty, bd5);
        }

        /// <summary>
        /// 根据工序报工性质设置可见性
        /// </summary>
        /// <param name="_isEquallyDivideProcess"></param>
        private void SetVisibleProperties(bool _isEquallyDivideProcess)
        {
            if (_isEquallyDivideProcess)
            {
                gb_UnequallyDivideProcess.Visibility = Visibility.Hidden;
                gb_PersonInfo.Visibility = gb_ScrapInfo.Visibility = Visibility.Visible;
            }
            else
            {
                gb_UnequallyDivideProcess.Visibility = Visibility.Visible;
                gb_PersonInfo.Visibility = gb_ScrapInfo.Visibility = Visibility.Hidden;
            }
        }
        #endregion

        #region 选定要报工的流转卡
        /// <summary>
        /// 搜索流传卡号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FlowCardSearch_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            textb_SearchInfo.Text = "流转卡搜索页面";
            if (string.IsNullOrEmpty(textb_FlowCard.Text))
            {
                GetAllFlowCards();
            }
            else
            {
                GetInputFlowCard();
            }

            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 回车快捷搜索流传卡编号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textb_FlowCard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_FlowCardSearch_Click(sender, e);
            }
        }

        /// <summary>
        /// 获取所有流转卡信息
        /// </summary>
        private void GetAllFlowCards()
        {
            searchFrame.Navigate(new FlowCardSearch_Page(FetchFlowCard));
        }

        /// <summary>
        /// 获取指定编号的流转卡
        /// </summary>
        private void GetInputFlowCard()
        {
            bool flag = CheckForCode(textb_FlowCard.Text);
            if (flag)
            {
                FlowCardSearch_Page fcs = new FlowCardSearch_Page(FetchFlowCard, "Report", textb_FlowCard.Text, true);
            }
        }

        /// <summary>
        /// 检查输入的流转卡编号是否存在
        /// </summary>
        /// <returns></returns>
        private bool CheckForCode(string _key)
        {
            bool flag = false;
            MyDBController.GetConnection();
            string SQl = string.Format(@"Select count(*) from [FlowCard] A left join  [FlowCardSub] B on A.[ID]=B.[FCS_FlowCardID]  where [FC_Code]='{0}' and  [FC_CardState] in ({1},{2})", _key, 0, 1);
            int count = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));
            if (count > 0)
            {
                flag = true;
            }
            else
            {
                SQl = string.Format(@"Select count(*) from [FlowCard] where [FC_Code]='{0}' ", _key);
                count = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));
                if (count == 0)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡编号不存在，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    SQl = string.Format(@"Select [FC_CardState] from [FlowCard] where [FC_Code]='{0}' ", _key);
                    int state = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));
                    switch (state)
                    {
                        case 0:
                        case 1:
                            break;
                        case 3:
                            Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡已经被分批，不能报工，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case 4:
                            Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡已经被转序，不能报工，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case 2:
                            Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡已经完工，不能报工，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        default:
                            break;
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// 接收选定的流转卡信息的委托
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tv"></param>
        private void FetchFlowCard(FlowCardLists fc, List<FlowCardSubLists> _fcslist, TechVersion tv)
        {
            handledcount = 0;
            fcl = fc;
            fcsls = _fcslist;
            bool flag = DisplayFlowCardInfo(fc, tv);
            if (flag)
            {
                BindProcess(tv, _fcslist);
                HandleFlowCardSubInfo(fc, _fcslist);
            }
            textb_SearchInfo.Text = "";
            searchFrame.Content = null;
        }


        /// <summary>
        /// 展示选中的流转卡信息
        /// </summary>
        /// <param name="fc"></param>
        /// <param name="tv"></param>
        /// <returns></returns>
        private bool DisplayFlowCardInfo(FlowCardLists fc, TechVersion tv)
        {
            try
            {
                tvFlowCard = tv;
                txtb_BeginAmount.Minimum = fc.FC_Amount;
                textb_FlowCard.Text = fc.FC_Code;
                txtb_Amount.Text = fc.FC_Amount.ToString();
                txtb_ItemInfo.Text = fc.PO_ItemCode + " | " + fc.PO_ItemName + " | " + fc.PO_ItemSpec;
                txtb_ReportWay.Text = (new ReportWayConverter()).Convert(tv.TRV_ReportWay, typeof(string), null, new System.Globalization.CultureInfo("")).ToString();
                txtb_IsSpecialTechVersion.Text = ((new TrueOrFalseConverter()).Convert(tv.TRV_IsSpecialVersion, typeof(string), null, new System.Globalization.CultureInfo(""))).ToString();
                txtb_CardState.Text = (new FlowCardStateConverter()).Convert(fc.FC_CardState, typeof(string), null, new System.Globalization.CultureInfo("")).ToString();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 处理流转卡子表信息。主要处理投入数、合格数信息
        /// </summary>
        /// <param name="_fcslist"></param>
        private void HandleFlowCardSubInfo(FlowCardLists fc, List<FlowCardSubLists> _fcslist)
        {
            if (handledcount == 0)
            {
                _fcslist.Sort((p1, p2) => p1.FCS_ProcessSequanece.CompareTo(p2.FCS_ProcessSequanece));
            }
            var sequaneceList = fcsls.Select(p => p.FCS_ProcessSequanece).Distinct();
            var fcslGroup = fcsls.GroupBy(p => p.FCS_ProcessSequanece).ToList();
            //list = FlowCardQualityLists.FetchFCQLByFCSInfo(fcsls);
            foreach (var group in fcsls.GroupBy(p => p.FCS_ProcessSequanece).ToList())
            {
                bool _isEqualProcess = trList.Find(p => p.TR_ProcessSequence.Equals(group.FirstOrDefault().FCS_ProcessSequanece)).TR_IsEquallyDivideProcess;
                int beginAmount = 0;
                if (fcslGroup.FindIndex(item => item.Key.Equals(group.Key)) == 0)
                {
                    beginAmount = fcl.FC_Amount;
                }
                else
                {
                    var _tempList = fcslGroup.Last(item => item.ToList().FirstOrDefault().FCS_ProcessSequanece < group.ToList().FirstOrDefault().FCS_ProcessSequanece).ToList();
                    beginAmount = trList.Find(item => item.ID.Equals(_tempList.FirstOrDefault().FCS_TechRouteID)).TR_IsEquallyDivideProcess ? _tempList.FirstOrDefault().FCS_QulifiedAmount : _tempList.Sum(item => item.FCS_QulifiedAmount);
                }
                group.ToList().ForEach(p =>
                    {
                        if (_isEqualProcess)
                        {
                            p.FCS_BeginAmount = beginAmount;
                            p.FCS_QulifiedAmount = p.FCS_BeginAmount - p.FCS_ScrappedAmount - p.FCS_UnprocessedAm + p.FCS_AddAmount;
                        }
                        else
                        {
                            //p.FCS_QulifiedAmount = 0;
                        }
                    });
            }

            #region 原逻辑
            //for (int i = 0; i < fcsls.Count; i++)
            //{
            //    for (int j = 0; j < sequaneceList.Count(); j++)
            //    {
            //        if (fcsls[i].FCS_ProcessSequanece == sequaneceList.ElementAt(j))
            //        {
            //            if (j == 0)
            //            {
            //                fcsls[i].FCS_BeginAmount = fc.FC_Amount + fcsls[i - 1].FCS_AddAmount;
            //            }
            //            else if (fcsls[i].FCS_ProcessSequanece != fcsls[i - 1].FCS_ProcessSequanece)
            //            {
            //                fcsls[i].FCS_BeginAmount = fcsls[i - 1].FCS_QulifiedAmount + fcsls[i - 1].FCS_AddAmount;
            //            }
            //            else
            //            {
            //                fcsls[i].FCS_BeginAmount = fcsls[i - 1].FCS_BeginAmount;
            //            }
            //            fcsls[i].FCS_QulifiedAmount = fcsls[i].FCS_BeginAmount - fcsls[i].FCS_ScrappedAmount - fcsls[i].FCS_UnprocessedAm;
            //            break;
            //        }
            //    }
            //}
            #endregion
        }
        #endregion

        #region 绑定下拉框数据源
        /// <summary>
        /// 为工序下拉框绑定工序信息数据源
        /// </summary>
        /// <param name="tv"></param>
        private void BindProcess(TechVersion tv, List<FlowCardSubLists> fcslist)
        {
            GetTechRoute(tv);
            HandleData(tv, fcslist);
        }

        /// <summary>
        /// BindProcess函数里面获取工艺陆路线
        /// </summary>
        private void GetTechRoute(TechVersion tv)
        {
            DataSet ds = new DataSet();
            string SQl = string.Format(@"Select [ID],[TR_ItemID],[TR_IsFirstProcess],[TR_IsLastProcess],[TR_IsTestProcess],[TR_IsBackProcess],[TR_ProcessSequence],[TR_ProcessName],[TR_ProcessCode],[TR_ProcessID],[TR_IsEquallyDivideProcess],[TR_DefaultCheckPersonName] from [TechRoute] where [TR_VersionID]={0} and [TR_ProcessSequence]>={1}", tv.ID, fcl.FC_FirstProcessNum);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "TechRoute");
            MyDBController.CloseConnection();

            trList = new List<TechRouteLists>();
            foreach (DataRow row in ds.Tables["TechRoute"].Rows)
            {
                trList.Add(new TechRouteLists()
                {
                    ID = Convert.ToInt64(row["ID"]),
                    TR_IsFirstProcess = Convert.ToBoolean(row["TR_IsFirstProcess"]),
                    TR_IsLastProcess = Convert.ToBoolean(row["TR_IsLastProcess"]),
                    TR_IsTestProcess = Convert.ToBoolean(row["TR_IsTestProcess"]),
                    TR_IsBackProcess = Convert.ToBoolean(row["TR_IsBackProcess"]),
                    TR_ProcessSequence = Convert.ToInt32(row["TR_ProcessSequence"]),
                    TR_ProcessID = Convert.ToInt64(row["TR_ProcessID"]),
                    TR_ProcessName = row["TR_ProcessName"].ToString(),
                    TR_ProcessCode = row["TR_ProcessCode"].ToString(),
                    TR_DefaultCheckPersonName = row["TR_DefaultCheckPersonName"].ToString(),
                    TR_ItemID = Convert.ToInt64(row["TR_ItemID"]),
                    TR_IsEquallyDivideProcess = row["TR_IsEquallyDivideProcess"] is DBNull ? true : Convert.ToBoolean(row["TR_IsEquallyDivideProcess"])
                });
            }
        }

        /// <summary>
        /// BindProcess函数里面处理下拉框数据源
        /// </summary>
        private void HandleData(TechVersion tv, List<FlowCardSubLists> fcslist)
        {
            //cb_ProcessSequence.ItemsSource = null;
            if (tv.TRV_ReportWay == 0)//流水线报工
            {
                if (handledcount == 0)
                {
                    cb_ProcessSequence.ItemsSource = trList.Distinct();
                }
                else
                {
                    cb_ProcessSequence.SelectedIndex = Math.Min(cb_ProcessSequence.SelectedIndex + 1, cb_ProcessSequence.Items.Count);
                }
            }
            else//离散报工
            {
                List<TechRouteLists> source = new List<TechRouteLists>();
                if (fcslist.Count > 0)//整个流转卡有人
                {
                    if (fcslist.Where(f => f.FCS_IsReported == true).Distinct().Count() > 0)//已经开始报工了
                    {
                        source.Add(trList.FirstOrDefault(p => p.TR_ProcessSequence.CompareTo((fcsls.Where(item => item.FCS_IsReported == true).Max(obj => obj.FCS_ProcessSequanece))) > 0));
                    }
                    else//没有开始报工
                    {
                        source.Add(trList.FirstOrDefault());
                    }
                    cb_ProcessSequence.ItemsSource = source;
                }
                else//整个流转卡没人
                {
                    source.Add(trList.FirstOrDefault());
                    cb_ProcessSequence.ItemsSource = source;
                }

            }
            cb_ProcessSequence.DisplayMemberPath = "TR_ProcessSequence";
            cb_ProcessSequence.SelectedValuePath = "ID";
            if (cb_ProcessSequence.HasItems)
            {
                cb_ProcessSequence.SelectedIndex = 0;
                SelectionChangedEvent();
            }
            if (datagrid_PersonInfo.HasItems)
            {
                image_No.Visibility = personDataSource[0].FCS_IsReported ? Visibility.Hidden : Visibility.Visible;
                image_Yes.Visibility = (!personDataSource[0].FCS_IsReported) ? Visibility.Hidden : Visibility.Visible;
            }
        }

        /// <summary>
        /// 工序下拉框选择改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_ProcessSequence_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            #region 操作
            if (isAmountSaved)
            {
                if (!isChanged)
                {
                    SelectionChangedEvent();
                }
                else
                {
                    if (selectErrorCount == 0 && !isReported)
                    {
                        selectErrorCount++;
                        isChanged = false;
                        SelectionChangedEvent();
                    }
                    else
                    {
                        selectErrorCount = 0;
                    }
                }
            }
            else
            {
                if (tvFlowCard.TRV_ReportWay == 0 && selectErrorCount == 0)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("请先保存填写的数量信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    selectErrorCount++;
                    cb_ProcessSequence.SelectedIndex = lastindex;
                }
                else
                {
                    selectErrorCount = 0;
                }
            }
            #endregion
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 下拉框选中项改变执行的语句
        /// </summary>
        private void SelectionChangedEvent()
        {
            List<Int64> _fcsID = new List<long>();
            //人员信息
            if (cb_ProcessSequence.SelectedItem != null)
            {
                isEquallyDiviedProcess = ((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_IsEquallyDivideProcess;
                SetVisibleProperties(isEquallyDiviedProcess);
                SetBinding();
                list = new List<FlowCardQualityLists>();
                txtb_CheckPerson.Text = ((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_DefaultCheckPersonName;
                txtb_ProcessName.Text = ((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_ProcessName;
                personDataSource = fcsls.FindAll(p => p.FCS_TechRouteID.Equals(cb_ProcessSequence.SelectedValue));
                if (!isEquallyDiviedProcess)
                {
                    dg_UnquallyDivideProcessInfo.ItemsSource = null;
                    dg_UnquallyDivideProcessInfo.ItemsSource = personDataSource;
                }
                else
                {
                    datagrid_PersonInfo.ItemsSource = null;
                    datagrid_PersonInfo.ItemsSource = personDataSource;
                }
                if (personDataSource.Count > 0)
                {
                    image_No.Visibility = personDataSource[0].FCS_IsReported ? Visibility.Hidden : Visibility.Visible;
                    image_Yes.Visibility = (!personDataSource[0].FCS_IsReported) ? Visibility.Hidden : Visibility.Visible;
                    SaveAmount();
                    btn_CancelAmount_Click(null, null);
                }
                else
                {
                    image_No.Visibility = Visibility.Visible;
                    image_Yes.Visibility = Visibility.Hidden;
                }
                if (lastSequence != ((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_ProcessSequence)
                {
                    lastSequence = ((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_ProcessSequence;
                    fcsls = FlowCardSubLists.FetchFCS_InfoByFC_Id(fcl.ID);
                    foreach (FlowCardSubLists item in fcsls.FindAll(p => p.FCS_TechRouteID.Equals(((TechRouteLists)cb_ProcessSequence.SelectedItem).ID)))
                    {
                        _fcsID.Add(item.ID);
                    }
                    //数量信息
                    datagrid_AmountInfo.ItemsSource = null;
                }
                if (_fcsID.Count > 0)
                {
                    lastindex = cb_ProcessSequence.SelectedIndex;
                    list = GetQualityInfo(_fcsID);
                    datagrid_AmountInfo.ItemsSource = list;
                    datagrid_AmountInfo.Items.Refresh();
                    int scrapamount = 0, beginamount = 0;
                    foreach (FlowCardQualityLists item in list)
                    {
                        scrapamount += item.FCQ_ScrapAmount;
                    }

                    if (!((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_IsFirstProcess)
                    {
                        //.OrderBy(p=>p.FCS_ProcessSequanece)
                        var _tempList = fcsls.GroupBy(p => p.FCS_ProcessSequanece).Last(item => item.ToList().FirstOrDefault().FCS_ProcessSequanece < ((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_ProcessSequence).ToList();
                        bool flag = trList.Find(item => item.ID.Equals(_tempList.FirstOrDefault().FCS_TechRouteID)).TR_IsEquallyDivideProcess;
                        if (flag)
                        {
                            beginamount = _tempList.FirstOrDefault().FCS_BeginAmount;
                        }
                        else
                        {
                            beginamount = _tempList.Sum(p => p.FCS_QulifiedAmount);
                        }
                        txtb_BeginAmount.Minimum = beginamount;
                        txtb_BeginAmount.Text = beginamount.ToString();
                    }
                    else
                    {
                        beginamount = fcl.FC_Amount;
                    }

                    txtb_BeginAmount.Minimum = beginamount;
                    txtb_BeginAmount.Text = beginamount.ToString();

                    txtb_ScrappedAmount.Text = scrapamount.ToString();
                    txtb_BeginAmount.IsReadOnly = !((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_IsFirstProcess;
                    txtb_UnprocessedAm.Text = fcsls.Find(p => p.FCS_ProcessSequanece == ((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_ProcessSequence).FCS_UnprocessedAm.ToString();

                    txtb_AddAmount.Text = fcsls.Find(p => p.FCS_ProcessSequanece == ((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_ProcessSequence).FCS_AddAmount.ToString();
                }
            }

        }

        /// <summary>
        /// 获取当前的流转卡子表的质量问题信息
        /// </summary>
        /// <param name="fcsID"></param>
        /// <returns></returns>
        private List<FlowCardQualityLists> GetQualityInfo(List<Int64> fcsID)
        {
            List<FlowCardQualityLists> _list = new List<FlowCardQualityLists>();
            DataSet ds = new DataSet();
            string fcsIDString = "";
            foreach (Int64 item in fcsID)
            {
                fcsIDString += string.IsNullOrEmpty(fcsIDString) ? item.ToString() : "," + item.ToString();
            }
            string SQl = string.Format("Select A.[ID],A.[FCQ_FlowCardSubID],A.[FCQ_QualityIssueID],A.[FCQ_ScrapAmount],B.[QI_Code],B.[QI_Name] from [FlowCardQuality] A left join [QualityIssue] B on A.[FCQ_QualityIssueID]=B.[ID] where [FCQ_FlowCardSubID] in ({0})", fcsIDString);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCardQuality");
            MyDBController.CloseConnection();

            foreach (DataRow row in ds.Tables["FlowCardQuality"].Rows)
            {
                _list.Add(new FlowCardQualityLists()
                {
                    ID = Convert.ToInt64(row["ID"]),
                    FCQ_FlowCardSubID = Convert.ToInt64(row["FCQ_FlowCardSubID"]),
                    FCQ_QualityIssueID = Convert.ToInt64(row["FCQ_QualityIssueID"]),
                    FCQ_ScrapAmount = Convert.ToInt32(row["FCQ_ScrapAmount"]),
                    QI_Code = row["QI_Code"].ToString(),
                    QI_Name = row["QI_Name"].ToString()
                });
            }
            return _list;
        }
        #endregion

        #region 检验员更改
        /// <summary>
        /// 搜索检验员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_QISearch_Click(object sender, RoutedEventArgs e)
        {
            TechRouteCheckPerson_Window trcp = new TechRouteCheckPerson_Window();
            trcp.ShowDialog();
            if ((bool)trcp.DialogResult)
            {
                foreach (FlowCardSubLists item in fcsls.FindAll(p => p.FCS_TechRouteID.Equals(cb_ProcessSequence.SelectedValue)))
                {
                    item.FCS_CheckByID = trcp.checkPerson.ID;
                    item.FCS_CheckByName = trcp.checkPerson.name;
                }
                txtb_CheckPerson.Text = trcp.checkPerson.name;
            }
        }
        #endregion

        #region 报废信息处理
        /// <summary>
        /// 添加报废信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddScrapInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (datagrid_PersonInfo.HasItems)
            {
                list.Add(new FlowCardQualityLists() { QI_Code = "", QI_Name = "" });
                datagrid_AmountInfo.ItemsSource = list;
                datagrid_AmountInfo.Items.Refresh();
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 搜索质量问题信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_QualityIssueSearch_Click(object sender, RoutedEventArgs e)
        {
            ProcessNameLists pnl = ProcessNameLists.FetchPNInfoByTRL((TechRouteLists)cb_ProcessSequence.SelectedItem);
            FlowCardQualityIssues_Window fci = new FlowCardQualityIssues_Window(AcceptQualityIssue, pnl);
            fci.ShowDialog();
        }

        /// <summary>
        /// 扫描质量问题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ScanScrapInfo_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_PersonInfo.HasItems)
            {
                ProcessNameLists pnl = ProcessNameLists.FetchPNInfoByTRL((TechRouteLists)cb_ProcessSequence.SelectedItem);
                FlowCardQualityIssues_Window fci = new FlowCardQualityIssues_Window(AcceptQualityIssueList, pnl);
                fci.ShowDialog();
            }
        }

        /// <summary>
        /// 非平分工序的时候，管理个人的报废信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ManageScrapInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (dg_UnquallyDivideProcessInfo.SelectedIndex != -1)
            {
                ManageScrapInfo((FlowCardSubLists)dg_UnquallyDivideProcessInfo.SelectedItem);
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 非平均报工的时候，管理员工报废信息
        /// </summary>
        /// <param name="_fcsl"></param>
        private void ManageScrapInfo(FlowCardSubLists _fcsl)
        {
            List<FlowCardSubLists> _fcslList = new List<FlowCardSubLists>() { _fcsl };
            ProcessNameLists pnl = ProcessNameLists.FetchPNInfoByFCS(_fcsl);
            ModifyWaySelect_Window mws = new ModifyWaySelect_Window(AcceptScrapInfo, _fcslList, pnl);
            mws.ShowDialog();
        }

        /// <summary>
        /// 接受报废信息的委托函数
        /// </summary>
        /// <param name="_fcqlList"></param>
        private void AcceptScrapInfo(List<FlowCardQualityLists> _fcqlList)
        {
            FlowCardQualityLists.SaveInfo(_fcqlList);
            KeyboardSimulation.Press(Key.Escape);
            KeyboardSimulation.Release(Key.Escape);
            ((FlowCardSubLists)dg_UnquallyDivideProcessInfo.SelectedItem).FCS_ScrappedAmount = _fcqlList.Sum(p => p.FCQ_ScrapAmount);
            txtb_ScrappedAmount.Text = personDataSource.Sum(p => p.FCS_ScrappedAmount).ToString();
            isAmountSaved = false;
        }

        /// <summary>
        /// 非平均工序的时候，每个人的合格数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iud_QualifiedAmount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            isAmountSaved = false;
            //CheckUnequalProcessAmountInfo(personDataSource);
        }


        /// <summary>
        /// 非平分工序的时候,扫描添加人员信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ScanPersonInfoUnequallyDivide_Click(object sender, RoutedEventArgs e)
        {
            btn_ScanPersonInfo_Click(null, null);
        }
        /// <summary>
        /// 非平分工序的时候，手工添加人员信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddPersonUnequallyDivide_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            TechRouteCheckPerson_Window trcp = new TechRouteCheckPerson_Window("操作工");
            trcp.ShowDialog();
            AcceptManualAddPersonInfo(trcp);
            this.Cursor = Cursors.Arrow;
        }
        /// <summary>
        /// 非平分工序的时候，移除人员信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RemovePersonUnequallyDivide_Click(object sender, RoutedEventArgs e)
        {
            btn_DeleteScrapInfo_Click(sender, e);
        }

        /// <summary>
        /// 将选定的质量信息添加到列表中,并且保存到数据库中
        /// </summary>
        /// <param name="qil"></param>
        private void AcceptQualityIssue(QualityIssuesLists qil)
        {
            if (list.Exists(p => p.FCQ_QualityIssueID.Equals(qil.ID)))
            {

            }
            else
            {
                isChanged = true;
                FlowCardQualityLists item = ((FlowCardQualityLists)datagrid_AmountInfo.SelectedItem);
                item.QI_Code = qil.QI_Code;
                item.QI_Name = qil.QI_Name;
                DataSet ds = new DataSet();
                string SQl = string.Format(@"Select top 0 * from [FlowCardQuality]");
                MyDBController.GetConnection();
                MyDBController.GetDataSet(SQl, ds, "FlowCardQuality");
                List<string> colList = new List<string>();
                foreach (DataColumn col in ds.Tables["FlowCardQuality"].Columns)
                {
                    colList.Add(col.ColumnName);
                }
                ds.Tables["FlowCardQuality"].Columns.Add(new DataColumn("IDNew", typeof(Int64)));
                DataRow row = ds.Tables["FlowCardQuality"].NewRow();
                row["ID"] = row["IDNew"] = item.ID;
                row["FCQ_FlowCardSubID"] = ((FlowCardSubLists)datagrid_PersonInfo.Items[0]).ID;
                row["FCQ_QualityIssueID"] = qil.ID;
                row["FCQ_ScrapAmount"] = item.FCQ_ScrapAmount;
                ds.Tables["FlowCardQuality"].Rows.Add(row);

                int updateNum, insertNum;
                MyDBController.InsertSqlBulk(ds.Tables["FlowCardQuality"], colList, out updateNum, out insertNum);
                MyDBController.CloseConnection();
                List<Int64> _fcsID = new List<long>();
                foreach (FlowCardSubLists _item in fcsls.FindAll(p => p.FCS_TechRouteID.Equals(cb_ProcessSequence.SelectedValue)))
                {
                    _fcsID.Add(_item.ID);
                }
                list = GetQualityInfo(_fcsID);
                datagrid_AmountInfo.ItemsSource = list;
                datagrid_AmountInfo.Items.Refresh();
            }
        }

        /// <summary>
        /// 接收扫描添加的报废信息
        /// </summary>
        /// <param name="_qilList"></param>
        private void AcceptQualityIssueList(List<QualityIssuesLists> _qilList)
        {
            isChanged = true;

            DataSet ds = new DataSet();
            string SQl = string.Format(@"Select top 0 * from [FlowCardQuality]");
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCardQuality");
            List<string> colList = new List<string>();
            foreach (DataColumn col in ds.Tables["FlowCardQuality"].Columns)
            {
                colList.Add(col.ColumnName);
            }
            ds.Tables["FlowCardQuality"].Columns.Add(new DataColumn("IDNew", typeof(Int64)));
            foreach (var _item in _qilList)
            {
                if (list.Exists(p => p.FCQ_QualityIssueID.Equals(_item.ID)))
                {
                    continue;
                }
                else
                {
                    FlowCardQualityLists item = new FlowCardQualityLists() { };
                    item.QI_Code = _item.QI_Code;
                    item.QI_Name = _item.QI_Name;
                    DataRow row = ds.Tables["FlowCardQuality"].NewRow();
                    row["ID"] = row["IDNew"] = item.ID;
                    row["FCQ_FlowCardSubID"] = item.FCQ_FlowCardSubID > -1 ? item.FCQ_FlowCardSubID : ((FlowCardSubLists)datagrid_PersonInfo.Items[0]).ID;
                    row["FCQ_QualityIssueID"] = _item.ID;
                    row["FCQ_ScrapAmount"] = item.FCQ_ScrapAmount;
                    ds.Tables["FlowCardQuality"].Rows.Add(row);
                }
            }

            int updateNum, insertNum;
            MyDBController.InsertSqlBulk(ds.Tables["FlowCardQuality"], colList, out updateNum, out insertNum);
            MyDBController.CloseConnection();
            List<Int64> _fcsID = new List<long>();
            foreach (FlowCardSubLists _item in fcsls.FindAll(p => p.FCS_TechRouteID.Equals(cb_ProcessSequence.SelectedValue)))
            {
                _fcsID.Add(_item.ID);
            }
            list = GetQualityInfo(_fcsID);
            datagrid_AmountInfo.ItemsSource = list;
            datagrid_AmountInfo.Items.Refresh();
        }
        /// <summary>
        /// 删除报废信息、移除工序操作人员信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DeleteScrapInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;
            switch (btn.Name)
            {
                case "btn_DeleteScrapInfo":
                    if (datagrid_AmountInfo.HasItems && datagrid_AmountInfo.SelectedIndex != -1)
                    {
                        if (Xceed.Wpf.Toolkit.MessageBox.Show("确定要删除该报废数量信息吗？" + "\r\n" + "该操作不可逆！", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            isChanged = true;
                            FlowCardQualityLists item = (FlowCardQualityLists)datagrid_AmountInfo.SelectedItem;
                            string SQl = string.Format(@"Delete from [FlowCardQuality] where [ID]={0}", item.ID);
                            MyDBController.GetConnection();
                            int x = MyDBController.ExecuteNonQuery(SQl);
                            MyDBController.CloseConnection();
                            Xceed.Wpf.Toolkit.MessageBox.Show(string.Format("成功删除该报废数量信息，共{0}条记录。", x), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            list.RemoveAt(datagrid_AmountInfo.SelectedIndex);
                            datagrid_AmountInfo.Items.Refresh();
                            int scrapamount = 0;
                            foreach (var _item in datagrid_AmountInfo.Items)
                            {
                                scrapamount += ((FlowCardQualityLists)_item).FCQ_ScrapAmount;
                            }
                            txtb_ScrappedAmount.Text = scrapamount.ToString();

                            foreach (FlowCardSubLists _item in datagrid_PersonInfo.Items)
                            {
                                _item.FCS_ScrappedAmount = scrapamount;
                            }
                            HandleFlowCardSubInfo(fcl, fcsls);
                        }
                    }
                    break;
                case "btn_RemovePerson":
                case "btn_RemovePersonUnequallyDivide":
                    int index = isEquallyDiviedProcess ? datagrid_PersonInfo.SelectedIndex : dg_UnquallyDivideProcessInfo.SelectedIndex;
                    if (personDataSource.Count > 0 && index != -1)
                    {
                        isChanged = true;
                        if (Xceed.Wpf.Toolkit.MessageBox.Show("确定要删除该人员信息吗？该操作不可逆！", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            FlowCardSubLists item = personDataSource[index];
                            personDataSource.RemoveAt(index);
                            string SQl = string.Format(@"Delete from [FlowCardSub] where [FCS_PersonCode]='{0}' and [FCS_TechRouteID]={1} and [FCS_FlowCardID]={2} and [FCS_ProcessID]={3}", item.FCS_PersonCode, item.FCS_TechRouteID, item.FCS_FlowCardID, item.FCS_ProcessID);
                            MyDBController.GetConnection();
                            int x = MyDBController.ExecuteNonQuery(SQl);
                            if (personDataSource.Count > 0)
                            {
                                FlowCardQualityLists.ChangeFCSID(new List<FlowCardSubLists>() { item }, personDataSource.FirstOrDefault());
                            }
                            else
                            {
                                SQl = string.Format("delete from [FlowCardQuality] where FCQ_FlowCardSubID={0}", item.ID);
                                MyDBController.ExecuteNonQuery(SQl);
                            }

                            Xceed.Wpf.Toolkit.MessageBox.Show(string.Format("成功删除该人员信息，共{0}条记录。", x), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            MyDBController.CloseConnection();

                            fcsls.Remove(item);
                            if (isEquallyDiviedProcess)
                            {
                                datagrid_PersonInfo.ItemsSource = fcsls.Where(p => p.FCS_TechRouteID.Equals(cb_ProcessSequence.SelectedValue));
                            }
                            else
                            {
                                dg_UnquallyDivideProcessInfo.ItemsSource = fcsls.Where(p => p.FCS_TechRouteID.Equals(cb_ProcessSequence.SelectedValue));
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 添加人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddPerson_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            TechRouteCheckPerson_Window trcp = new TechRouteCheckPerson_Window("操作工");
            trcp.ShowDialog();
            AcceptManualAddPersonInfo(trcp);
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 接受手动添加人员信息的函数
        /// </summary>
        /// <param name="_trcp"></param>
        private void AcceptManualAddPersonInfo(TechRouteCheckPerson_Window _trcp)
        {
            List<FlowCardSubLists> tempList = new List<FlowCardSubLists>();
            if ((bool)_trcp.DialogResult)
            {
                FlowCardSubLists newPerson = new FlowCardSubLists();
                if (personDataSource.Count > 0)
                {
                    FlowCardSubLists item = personDataSource[0];
                    newPerson = new FlowCardSubLists()
                    {
                        ID = -1,
                        FCS_BeginAmount = item.FCS_BeginAmount,
                        FCS_CheckByID = item.FCS_CheckByID,
                        FCS_CheckByName = item.FCS_CheckByName,
                        FCS_FlowCardID = item.FCS_FlowCardID,
                        FCS_IsFirstProcess = item.FCS_IsFirstProcess,
                        FCS_IsLastProcess = item.FCS_IsLastProcess,
                        FCS_IsReported = item.FCS_IsReported,
                        FCS_ItemId = item.FCS_ItemId,
                        FCS_PieceAmount = item.FCS_PieceAmount,
                        FCS_PieceDivNum = item.FCS_PieceDivNum,
                        FCS_ProcessID = item.FCS_ProcessID,
                        FCS_ProcessName = item.FCS_ProcessName,
                        FCS_ProcessSequanece = item.FCS_ProcessSequanece,
                        FCS_QulifiedAmount = isEquallyDiviedProcess ? item.FCS_QulifiedAmount : 0,
                        FCS_ScrappedAmount = isEquallyDiviedProcess ? item.FCS_ScrappedAmount : 0,
                        FCS_TechRouteID = item.FCS_TechRouteID,
                        FCS_AddAmount = item.FCS_AddAmount,
                        FCS_UnprocessedAm = item.FCS_UnprocessedAm,
                        FCS_PersonCode = _trcp.checkPerson.code,
                        FCS_PersonName = _trcp.checkPersonName,
                        FCS_ReportTime = DateTime.Now
                    };
                }
                else
                {
                    TechRouteLists trl = (TechRouteLists)cb_ProcessSequence.SelectedItem;
                    newPerson = new FlowCardSubLists()
                    {
                        ID = -1,
                        FCS_BeginAmount = fcl.FC_Amount,
                        //FCS_CheckByID = item.FCS_CheckByID,
                        FCS_CheckByName = trl.TR_DefaultCheckPersonName,
                        FCS_FlowCardID = fcl.ID,
                        FCS_IsFirstProcess = trl.TR_IsFirstProcess,
                        FCS_IsLastProcess = trl.TR_IsLastProcess,
                        FCS_IsReported = false,
                        FCS_ItemId = trl.TR_ItemID,
                        FCS_PieceAmount = 0,
                        FCS_PieceDivNum = 0,
                        FCS_ProcessID = trl.TR_ProcessID,
                        FCS_ProcessName = trl.TR_ProcessName,
                        FCS_ProcessSequanece = trl.TR_ProcessSequence,
                        FCS_QulifiedAmount = 0,
                        FCS_ScrappedAmount = 0,
                        FCS_TechRouteID = trl.ID,
                        FCS_AddAmount = 0,
                        FCS_UnprocessedAm = 0,
                        FCS_PersonCode = _trcp.checkPerson.code,
                        FCS_PersonName = _trcp.checkPersonName,
                        FCS_ReportTime = DateTime.Now
                    };
                }
                isChanged = true;
                tempList.Add(newPerson);
                FlowCardSubLists.SaveFCSInfo(tempList);
                fcsls = FlowCardSubLists.FetchFCS_InfoByFC_Id(fcl.ID);
                fcsls.Sort(new ListComparer<FlowCardSubLists>((p1, p2) => p1.FCS_ProcessSequanece.CompareTo(p2.FCS_ProcessSequanece)));
                HandleData(tvFlowCard, fcsls);
                personDataSource = fcsls.FindAll(p => p.FCS_TechRouteID.Equals(cb_ProcessSequence.SelectedValue));
                if (isEquallyDiviedProcess)
                {
                    datagrid_PersonInfo.ItemsSource = personDataSource;
                }
                else
                {
                    dg_UnquallyDivideProcessInfo.ItemsSource = personDataSource;
                }
                SaveAmount();
                btn_CancelAmount_Click(null, null);
                //Xceed.Wpf.Toolkit.MessageBox.Show("添加成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        /// <summary>
        /// 扫描添加人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ScanPersonInfo_Click(object sender, RoutedEventArgs e)
        {
            TechRouteCheckPerson_Window trcp = new TechRouteCheckPerson_Window("操作工", AcceptPersonList);
            trcp.ShowDialog();
        }

        /// <summary>
        /// 接收扫描添加的人员信息
        /// </summary>
        /// <param name="_plList"></param>
        private void AcceptPersonList(List<PersonLists> _plList)
        {
            if (personDataSource.Count > 0)
            {
                FlowCardSubLists item = personDataSource[0];
                List<FlowCardSubLists> tempList = new List<FlowCardSubLists>();
                foreach (PersonLists pl in _plList)
                {
                    FlowCardSubLists newPerson = new FlowCardSubLists()
                    {
                        ID = -1,
                        FCS_BeginAmount = item.FCS_BeginAmount,
                        FCS_CheckByID = item.FCS_CheckByID,
                        FCS_CheckByName = item.FCS_CheckByName,
                        FCS_FlowCardID = item.FCS_FlowCardID,
                        FCS_IsFirstProcess = item.FCS_IsFirstProcess,
                        FCS_IsLastProcess = item.FCS_IsLastProcess,
                        FCS_IsReported = item.FCS_IsReported,
                        FCS_ItemId = item.FCS_ItemId,
                        FCS_PieceAmount = item.FCS_PieceAmount,
                        FCS_PieceDivNum = item.FCS_PieceDivNum,
                        FCS_ProcessID = item.FCS_ProcessID,
                        FCS_ProcessName = item.FCS_ProcessName,
                        FCS_ProcessSequanece = item.FCS_ProcessSequanece,
                        FCS_QulifiedAmount = isEquallyDiviedProcess ? item.FCS_QulifiedAmount : 0,
                        FCS_ScrappedAmount = isEquallyDiviedProcess ? item.FCS_ScrappedAmount : 0,
                        FCS_TechRouteID = item.FCS_TechRouteID,
                        FCS_AddAmount = item.FCS_AddAmount,
                        FCS_UnprocessedAm = item.FCS_UnprocessedAm,
                        FCS_PersonCode = pl.code,
                        FCS_PersonName = pl.name,
                        FCS_ReportTime = DateTime.Now
                    };
                    if (!fcsls.Exists(p => p.FCS_PersonCode == newPerson.FCS_PersonCode && p.FCS_ProcessSequanece.Equals(newPerson.FCS_ProcessSequanece)))
                    {
                        fcsls.Add(newPerson);
                        tempList.Add(newPerson);
                    }
                }
                FlowCardSubLists.SaveFCSInfo(tempList);
            }
            else
            {
                TechRouteLists trl = (TechRouteLists)cb_ProcessSequence.SelectedItem;
                List<FlowCardSubLists> tempList = new List<FlowCardSubLists>();
                foreach (PersonLists pl in _plList)
                {
                    FlowCardSubLists newPerson = new FlowCardSubLists()
                    {
                        ID = -1,
                        FCS_BeginAmount = fcl.FC_Amount,
                        //FCS_CheckByID = item.FCS_CheckByID,
                        FCS_CheckByName = trl.TR_DefaultCheckPersonName,
                        FCS_FlowCardID = fcl.ID,
                        FCS_IsFirstProcess = trl.TR_IsFirstProcess,
                        FCS_IsLastProcess = trl.TR_IsLastProcess,
                        FCS_IsReported = false,
                        FCS_ItemId = trl.TR_ItemID,
                        FCS_PieceAmount = 0,
                        FCS_PieceDivNum = 0,
                        FCS_ProcessID = trl.TR_ProcessID,
                        FCS_ProcessName = trl.TR_ProcessName,
                        FCS_ProcessSequanece = trl.TR_ProcessSequence,
                        FCS_QulifiedAmount = 0,
                        FCS_ScrappedAmount = 0,
                        FCS_TechRouteID = trl.ID,
                        FCS_AddAmount = 0,
                        FCS_UnprocessedAm = 0,
                        FCS_PersonCode = pl.code,
                        FCS_PersonName = pl.name,
                        FCS_ReportTime = DateTime.Now
                    };
                    if (!fcsls.Exists(p => p.FCS_PersonCode == newPerson.FCS_PersonCode && p.FCS_ProcessSequanece.Equals(newPerson.FCS_ProcessSequanece)))
                    {
                        fcsls.Add(newPerson);
                        tempList.Add(newPerson);
                    }
                }
                FlowCardSubLists.SaveFCSInfo(tempList);
            }
            fcsls = FlowCardSubLists.FetchFCS_InfoByFC_Id(fcl.ID);
            fcsls.Sort(new ListComparer<FlowCardSubLists>((p1, p2) => p1.FCS_ProcessSequanece.CompareTo(p2.FCS_ProcessSequanece)));
            HandleData(tvFlowCard, fcsls);
            isAmountSaved = false;
            isChanged = true;
            personDataSource = fcsls.FindAll(p => p.FCS_TechRouteID.Equals(cb_ProcessSequence.SelectedValue));
            if (isEquallyDiviedProcess)
            {
                dg_UnquallyDivideProcessInfo.ItemsSource = personDataSource;
            }
            else
            {
                datagrid_PersonInfo.ItemsSource = personDataSource;
            }
            SaveAmount();
            btn_CancelAmount_Click(null, null);
        }
        /// <summary>
        /// 数量改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void integer_Amount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ((FlowCardQualityLists)datagrid_AmountInfo.SelectedItem).FCQ_ScrapAmount = (int)((IntegerUpDown)sender).Value;

            int? scrapamount = ((List<FlowCardQualityLists>)datagrid_AmountInfo.ItemsSource).Sum(p => p.FCQ_ScrapAmount);
            int qualifiedAmount = (int)(txtb_BeginAmount.Value - scrapamount - txtb_UnprocessedAm.Value);
            if (qualifiedAmount < 0)
            {
                ((IntegerUpDown)sender).Value = (int?)e.OldValue;
            }
            else
            {
                txtb_ScrappedAmount.Text = scrapamount.ToString();
            }
            isAmountSaved = false;
            isReported = false;
        }

        /// <summary>
        /// 当数量改变的时候，保存数量的按钮启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_QulifiedAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (datagrid_PersonInfo.HasItems)
            {
                if (!Convert.ToInt32(txtb_QulifiedAmount.Text).Equals(personDataSource[0].FCS_QulifiedAmount))
                {
                    btn_SaveAmount.IsEnabled = true;
                }
                else
                {
                    btn_SaveAmount.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// 投入数量改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_BeginAmount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (personDataSource.Count > 0 && !Convert.ToInt32(txtb_BeginAmount.Text).Equals(personDataSource[0].FCS_BeginAmount))
            {
                isReported = false;
                isAmountSaved = false;
            }
        }

        /// <summary>
        /// 待处理数失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_UnprocessedAm_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 待处理数改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_UnprocessedAm_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (datagrid_PersonInfo.HasItems)
            {
                if (string.IsNullOrEmpty(txtb_UnprocessedAm.Text))
                {
                    txtb_UnprocessedAm.Value = 0;
                    txtb_UnprocessedAm.Text = "0";
                }
                else
                {
                    int qualifiedAmount = (int)(txtb_BeginAmount.Value - txtb_UnprocessedAm.Value - Convert.ToInt32(txtb_ScrappedAmount.Text));
                    if (personDataSource.Count > 0 && !txtb_UnprocessedAm.Value.Equals(personDataSource[0].FCS_UnprocessedAm))
                    {
                        if (qualifiedAmount < 0)
                        {
                            txtb_UnprocessedAm.Text = ((int?)e.OldValue).ToString();
                            txtb_UnprocessedAm.Value = (int?)e.OldValue;
                        }
                        isAmountSaved = false;
                        isReported = false;
                    }
                }
            }
        }

        /// <summary>
        /// 保存报废数量信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveAmount_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            SaveAmount();
            Xceed.Wpf.Toolkit.MessageBox.Show("保存成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 保存报废数量的事情
        /// </summary>
        private void SaveAmount()
        {
            if (!isEquallyDiviedProcess && !isAmountSaved)
            {
                FlowCardSubLists.SaveFCSInfo(personDataSource, false);
                isAmountSaved = true;
                isChanged = false;
            }
            else
            {
                if (datagrid_PersonInfo.HasItems && !isAmountSaved)
                {
                    isChanged = true;
                    MyDBController.GetConnection();
                    foreach (var item in datagrid_PersonInfo.Items)
                    {
                        FlowCardSubLists _item = ((FlowCardSubLists)item);
                        _item.FCS_QulifiedAmount = Convert.ToInt32(txtb_QulifiedAmount.Text);
                        _item.FCS_ScrappedAmount = Convert.ToInt32(txtb_ScrappedAmount.Text);
                        _item.FCS_UnprocessedAm = Convert.ToInt32(txtb_UnprocessedAm.Text);
                        _item.FCS_BeginAmount = Convert.ToInt32(txtb_BeginAmount.Text);
                        string SQl = string.Format(@"Update [FlowCardSub] set [FCS_QulifiedAmount]={0},[FCS_ScrappedAmount]={1},[FCS_UnprocessedAm]={2},[FCS_BeginAmount]={3} where [ID]={4}", _item.FCS_QulifiedAmount, _item.FCS_ScrappedAmount, _item.FCS_UnprocessedAm, _item.FCS_BeginAmount, _item.ID);
                        MyDBController.ExecuteNonQuery(SQl);
                    }

                    foreach (var item in datagrid_AmountInfo.Items)
                    {
                        FlowCardQualityLists _item = (FlowCardQualityLists)item;
                        string SQl = string.Format(@"Update [FlowCardQuality] set [FCQ_ScrapAmount]={0} where [ID]={1}", _item.FCQ_ScrapAmount, _item.ID);
                        MyDBController.ExecuteNonQuery(SQl);
                    }
                    MyDBController.CloseConnection();
                    isAmountSaved = true;
                }
            }
            HandleFlowCardSubInfo(fcl, fcsls);
        }
        /// <summary>
        /// 取消数量更改，重置为原始值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CancelAmount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region 重置报废信息
                if (isEquallyDiviedProcess)
                {
                    if (datagrid_AmountInfo.HasItems)
                    {
                        List<Int64> _fcsID = new List<long>();
                        foreach (FlowCardSubLists item in fcsls.FindAll(p => p.FCS_TechRouteID.Equals(cb_ProcessSequence.SelectedValue)))
                        {
                            _fcsID.Add(item.ID);
                        }
                        list = GetQualityInfo(_fcsID);
                        datagrid_AmountInfo.ItemsSource = list;
                        datagrid_AmountInfo.Items.Refresh();
                    }
                    else
                    {

                    }
                }
                #endregion

                #region 重置未处理信息、重置投入数
                if (isEquallyDiviedProcess)
                {
                    if (personDataSource.Count > 0)
                    {
                        txtb_UnprocessedAm.Value = (personDataSource[0]).FCS_UnprocessedAm;//未处理信息
                        txtb_BeginAmount.Value = (personDataSource[0]).FCS_BeginAmount;//投入数
                        txtb_AddAmount.Text = (personDataSource[0]).FCS_AddAmount.ToString();//转序新增数
                    }
                }
                else
                {
                }

                isAmountSaved = true;
                #endregion
            }
            catch (Exception)
            {
                isAmountSaved = false;
            }
            finally
            {
                string message = isAmountSaved ? "重置成功！" : "重置失败，请重试！";
                if (!isAmountSaved)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(message, "提示", MessageBoxButton.OK, isAmountSaved ? MessageBoxImage.Information : MessageBoxImage.Error);
                }
            }
        }

        #endregion

        #region 报工处理
        /// <summary>
        /// 报工事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Report_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (CheckUnequalAmountInfo())
            {
                if (personDataSource.Count > 0)
                {
                    bool? flag = CheckIsFinishingReport();
                    isFinished = flag == null ? false : (bool)flag;
                    if (flag != null)
                    {
                        bool reportFlag = false;
                        if (!isAmountSaved)
                        {
                            if (Xceed.Wpf.Toolkit.MessageBox.Show("您还有数量信息没有保存！\r\n是否保存并报工？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                SaveAmount();
                                reportFlag = true;
                            }
                        }
                        else
                        {
                            reportFlag = true;
                        }
                        if (reportFlag)
                        {
                            UpdateProcess();
                            UpdateQuality();
                            UpdateFlowCard(flag);
                            HandleFlowCardSubInfo(fcl, fcsls);
                            handledcount++;
                            fcsls = FlowCardSubLists.FetchFCS_InfoByFC(fcl);
                            HandleData(tvFlowCard, fcsls);
                            isChanged = false;
                            isReported = true;
                            if ((bool)flag)
                            {
                                btn_TotalReport_Click(sender, e);
                                AfterReportSetting();
                            }
                            textb_FlowCard.Focus();
                        }
                    }
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("没有操作人员，不能报工！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 非平均报工的时候，检查数量信息
        /// </summary>
        /// <returns></returns>
        private bool CheckUnequalAmountInfo()
        {
            bool flag = false;
            if (isEquallyDiviedProcess)
            {
                return true;
            }
            else
            {
                int count = personDataSource.Sum(p => p.FCS_QulifiedAmount);
                if (count != Convert.ToInt32(txtb_QulifiedAmount.Text))
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("合格数量信息不吻合！\r\n请检查各个员工的报废数量信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    flag = true;
                }
            }
            return flag;
        }
        /// <summary>
        /// 报工完成之后，改变操作选项
        /// </summary>
        private void AfterReportSetting()
        {

            fcsls = new List<FlowCardSubLists>();
            list = new List<FlowCardQualityLists>();
            personDataSource = new List<FlowCardSubLists>();
            trList = new List<TechRouteLists>();
            fcl = new FlowCardLists();
            tvFlowCard = new TechVersion();
            isAmountSaved = true;
            isNewFlowCard = false;
            isChanged = false;
            isReported = false;
            isFinished = false;
            isAfterHandled = false;
            handledcount = 0;
            lastindex = 0;
            selectErrorCount = 0;
            textb_FlowCard.Focus();
            datagrid_AmountInfo.ItemsSource = null;
            datagrid_PersonInfo.ItemsSource = null;
            dg_UnquallyDivideProcessInfo.ItemsSource = null;

            SetVisibleProperties(true);
            MyDBController.FindVisualChild<TextBox>(this).ForEach(t =>
            {
                t.Text = "";
            });
            date_PresentDate.SelectedDate = DateTime.Now;
        }

        /// <summary>
        /// 检查是否最后一道报工的工序，最后一道工序的话，报工后，整张流转卡的状态变成完工状态。报工信息不再修改！
        /// 是最后一道并且继续，返回true，是最后一道，不继续，返回null，不是最后一道，返回false
        /// </summary>
        private bool? CheckIsFinishingReport()
        {
            switch (tvFlowCard.TRV_ReportWay)
            {
                case 0:
                    if (personDataSource.Count > 0)
                    {
                        bool flag = true;
                        var isreported = fcsls.Where(p => !p.FCS_ProcessSequanece.Equals((personDataSource[0]).FCS_ProcessSequanece)).Select(p => p.FCS_IsReported).Distinct();
                        foreach (bool item in isreported)
                        {
                            if (item)
                            {
                            }
                            else
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            if (Xceed.Wpf.Toolkit.MessageBox.Show("工艺路线中其他工序都已报工，该工序报工后" + "\r\n" + "将会结束报工，是否继续？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                            {
                                return true;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                case 1:
                default:
                    if (personDataSource.Count > 0)
                    {
                        if ((personDataSource[0]).FCS_IsLastProcess)
                        {
                            if (Xceed.Wpf.Toolkit.MessageBox.Show("这是末道工序，该工序报工后" + "\r\n" + "将会结束报工，是否继续？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                            {
                                return true;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
            }
        }

        /// <summary>
        /// 报工时处理工序
        /// </summary>
        private void UpdateProcess()
        {
            string SQl = "select top 0 * from [FlowCardSub]";
            List<string> colList = new List<string>();
            DataSet ds = new DataSet();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCardSub");
            foreach (DataColumn col in ds.Tables["FlowCardSub"].Columns)
            {
                colList.Add(col.ColumnName);
            }

            ds.Tables["FlowCardSub"].Columns.Add(new DataColumn("IDNew", typeof(Int64)));

            foreach (FlowCardSubLists item in personDataSource)
            {
                //item.FCS_ScrappedAmount = Convert.ToInt32(txtb_ScrappedAmount.Text);
                //item.FCS_UnprocessedAm = Convert.ToInt32(txtb_UnprocessedAm.Text);
                //item.FCS_QulifiedAmount = Convert.ToInt32(txtb_QulifiedAmount.Text);
                item.FCS_PieceDivNum = datagrid_PersonInfo.Items.Count;
                item.FCS_PieceAmount = trList.Find(p => p.ID.Equals(item.FCS_TechRouteID)).TR_IsTestProcess ? item.FCS_BeginAmount + item.FCS_AddAmount : item.FCS_QulifiedAmount;
                item.FCS_IsReported = true;
                DataRow row = ds.Tables["FlowCardSub"].NewRow();
                row["FCS_FlowCardID"] = item.FCS_FlowCardID;
                row["FCS_ItemId"] = item.FCS_ItemId;
                row["FCS_TechRouteID"] = item.FCS_TechRouteID;
                row["FCS_ProcessID"] = item.FCS_ProcessID;
                row["FCS_ProcessName"] = item.FCS_ProcessName;
                row["FCS_PersonCode"] = item.FCS_PersonCode;
                row["FCS_PersonName"] = item.FCS_PersonName;
                row["FCS_BeginAmount"] = item.FCS_BeginAmount;
                row["FCS_QulifiedAmount"] = item.FCS_QulifiedAmount;
                row["FCS_ScrappedAmount"] = item.FCS_ScrappedAmount;
                row["FCS_AddAmount"] = item.FCS_AddAmount;
                row["FCS_UnprocessedAm"] = item.FCS_UnprocessedAm;
                row["FCS_CheckByID"] = item.FCS_CheckByID;
                row["FCS_CheckByName"] = item.FCS_CheckByName;
                row["FCS_IsFirstProcess"] = item.FCS_IsFirstProcess;
                row["FCS_IsLastProcess"] = item.FCS_IsLastProcess;
                row["FCS_IsReported"] = item.FCS_IsReported;
                row["IDNew"] = row["ID"] = item.ID;
                TechRouteLists tl = (TechRouteLists)cb_ProcessSequence.SelectedItem;
                if (tl.TR_IsBackProcess)
                {
                    row["FCS_PieceAmount"] = 0;
                    row["FCS_PieceDivNum"] = personDataSource.Count;
                }
                else if (tl.TR_IsTestProcess)
                {
                    row["FCS_PieceAmount"] = txtb_BeginAmount.Text.Trim();
                    row["FCS_PieceDivNum"] = personDataSource.Count;
                }
                else
                {
                    row["FCS_PieceAmount"] = txtb_QulifiedAmount.Text.Trim();
                    row["FCS_PieceDivNum"] = personDataSource.Count;
                }
                row["FCS_ReportTime"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                ds.Tables["FlowCardSub"].Rows.Add(row);
            }
            if (personDataSource.Exists(p => p.FCS_UnprocessedAm > 0))
            {
                fcl.FC_CanTransfer = true;
            }

            int updateNum1, insertNum1;
            MyDBController.InsertSqlBulk(ds.Tables["FlowCardSub"], colList, out updateNum1, out insertNum1);
            MyDBController.CloseConnection();
        }

        /// <summary>
        /// 报工时处理质量问题信息
        /// </summary>
        private void UpdateQuality()
        {
            MyDBController.GetConnection();
            DataSet ds = new DataSet();
            string SQl = "Select top 0 * from [FlowCardQuality]";
            MyDBController.GetDataSet(SQl, ds, "FlowCardQuality");
            List<string> colList = new List<string>();
            foreach (DataColumn col in ds.Tables["FlowCardQuality"].Columns)
            {
                colList.Add(col.ColumnName);
            }
            ds.Tables["FlowCardQuality"].Columns.Add(new DataColumn("IDNew", typeof(Int64)));
            foreach (FlowCardQualityLists fcql in list)
            {
                DataRow row = ds.Tables["FlowCardQuality"].NewRow();
                row["ID"] = row["IDNew"] = fcql.ID;
                row["FCQ_FlowCardSubID"] = fcql.FCQ_FlowCardSubID;
                row["FCQ_QualityIssueID"] = fcql.FCQ_QualityIssueID;
                row["FCQ_ScrapAmount"] = fcql.FCQ_ScrapAmount;
                row["FCQ_HasReproduced"] = false;
                row["FCQ_Remark"] = "";
                ds.Tables["FlowCardQuality"].Rows.Add(row);
            }
            if (list.Exists(p => p.QI_Type == 2))
            {
                fcl.FC_CanReproduce = true;
            }
            int updateNum2, insertNum2;
            MyDBController.InsertSqlBulk(ds.Tables["FlowCardQuality"], colList, out updateNum2, out insertNum2);
            MyDBController.CloseConnection();
            Xceed.Wpf.Toolkit.MessageBox.Show(string.Format("报工成功"), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 更新流转卡状态，改为报工
        /// </summary>
        private void UpdateFlowCard(bool? _flag)
        {
            fcl.FC_CardState = (bool)_flag ? 2 : 1;
            string SQl = string.Format(@"Update [FlowCard] set [FC_CardState]={0},[FC_CanDistribute]='false' where [ID]={1}", fcl.FC_CardState, fcl.ID);
            MyDBController.GetConnection();
            MyDBController.ExecuteNonQuery(SQl);
            MyDBController.CloseConnection();
            txtb_CardState.Text = (new FlowCardStateConverter()).Convert(fcl.FC_CardState, typeof(string), null, new System.Globalization.CultureInfo("")).ToString();
        }

        /// <summary>
        /// 结束报工事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TotalReport_Click(object sender, RoutedEventArgs e)
        {
            isAfterHandled = true;
            if (fcsls.Any(p => p.FCS_ScrappedAmount > 0))
            {
                ScrapReportList.GenerateNewSR(fcl, fcsls);
            }
            if (fcsls.LastOrDefault().FCS_IsLastProcess)
            {
                FinishReportList.GenerateNewFR(fcl, fcsls);
            }
        }


        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Closing(object sender, CancelEventArgs e)
        {
            if (!isAmountSaved)
            {
                if (Xceed.Wpf.Toolkit.MessageBox.Show("还有数量信息没有保存，是否要退出？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {

                }
                else
                {
                    e.Cancel = true;
                }
            }
            else if (isChanged)
            {
                if (Xceed.Wpf.Toolkit.MessageBox.Show("数量保存后并没有对工序进行报工，" + "\r\n" + "是否要退出？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {

                }
                else
                {
                    e.Cancel = true;
                }
            }
            else if (isFinished && !isAfterHandled)
            {
                //if (Xceed.Wpf.Toolkit.MessageBox.Show("流转卡所有工序都已报工，但是并没有进行完工操作，" + "\r\n" + "是否要退出？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                //{

                //}
                //else
                //{
                //    e.Cancel = true;
                //}
            }
        }
        #endregion




    }
}
