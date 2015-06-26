using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.ValueConverters;
using BarCodeSystem.TechRoute.TechRoute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
//using System.Windows.Controls;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using System.Threading;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace BarCodeSystem.ProductDispatch.FlowCardReport
{
    /// <summary>
    /// FlowCardReport_Page.xaml 的交互逻辑
    /// </summary>
    public partial class FlowCardReport_Page : Page
    {
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
        /// 用来判断是否对数据库做出了改变
        /// </summary>
        public bool isChanged = false;

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
        /// 在未保存已更改数量的情况下，提示错误信息的次数
        /// </summary>
        int selectErrorCount = 0;
        #endregion

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            textb_FlowCard.Focus();
            SetBinding();
        }

        #region 绑定合格数量
        /// <summary>
        /// 将合格数量自动计算绑定
        /// </summary>
        private void SetBinding()
        {
            Binding bd1 = new Binding("Value") { Source = txtb_BeginAmount };
            Binding bd2 = new Binding("Text") { Source = txtb_ScrappedAmount };
            Binding bd3 = new Binding("Value") { Source = txtb_UnprocessedAm };
            MultiBinding mb = new MultiBinding();
            mb.Converter = new QualifiedAmountConverter();
            mb.Bindings.Add(bd1);
            mb.Bindings.Add(bd2);
            mb.Bindings.Add(bd3);
            txtb_QulifiedAmount.SetBinding(TextBox.TextProperty, mb);

            Binding bd4 = new Binding("HasItems") { Source = datagrid_PersonInfo };
            bd4.Converter = new FlowCardHasItemsConverter();
            txtb_UnprocessedAm.SetBinding(IntegerUpDown.IsReadOnlyProperty, bd4);
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
            if (textb_SearchInfo.Text.Equals("流转卡搜索页面"))
            {

            }
            else
            {
                textb_SearchInfo.Text = "流转卡搜索页面";
                searchFrame.Navigate(new FlowCardSearch_Page(FetchFlowCard));
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 接收选定的流转卡信息的委托
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tv"></param>
        private void FetchFlowCard(FlowCardLists fc, List<FlowCardSubLists> _fcslist, TechVersion tv)
        {
            handledcount = 0;

            HandleFlowCardSubInfo(fc, _fcslist);

            bool flag = DisplayFlowCardInfo(fc, tv);
            if (flag)
            {
                BindProcess(tv, _fcslist);
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
            fcl = fc;
            fcsls = _fcslist;
            var sequaneceList = fcsls.Select(p => p.FCS_ProcessSequanece).Distinct();
            for (int i = 0; i < fcsls.Count; i++)
            {
                for (int j = 0; j < sequaneceList.Count(); j++)
                {
                    if (fcsls[i].FCS_ProcessSequanece == sequaneceList.ElementAt(j))
                    {
                        if (j == 0)
                        {
                            fcsls[i].FCS_BeginAmount = fc.FC_Amount;
                        }
                        else if (fcsls[i].FCS_ProcessSequanece != fcsls[i - 1].FCS_ProcessSequanece)
                        {
                            fcsls[i].FCS_BeginAmount = fcsls[i - 1].FCS_QulifiedAmount;
                        }
                        else
                        {
                            fcsls[i].FCS_BeginAmount = fcsls[i - 1].FCS_BeginAmount;
                        }
                        fcsls[i].FCS_QulifiedAmount = fcsls[i].FCS_BeginAmount - fcsls[i].FCS_ScrappedAmount - fcsls[i].FCS_UnprocessedAm;
                        break;
                    }
                }
            }
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
            string SQl = string.Format(@"Select [ID],[TR_IsFirstProcess],[TR_IsLastProcess],[TR_IsTestProcess],[TR_IsBackProcess],[TR_ProcessSequence],[TR_ProcessName],[TR_ProcessCode],[TR_DefaultCheckPersonName] from [TechRoute] where [TR_VersionID]={0}", tv.ID);
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
                    TR_ProcessName = row["TR_ProcessName"].ToString(),
                    TR_ProcessCode = row["TR_ProcessCode"].ToString(),
                    TR_DefaultCheckPersonName = row["TR_DefaultCheckPersonName"].ToString()
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
                    cb_ProcessSequence.ItemsSource = trList.Join(fcslist, tr => tr.ID, fcs => fcs.FCS_TechRouteID, (tr, fcs) => tr).Distinct();
                }
                else
                {
                    cb_ProcessSequence.SelectedIndex = Math.Min(cb_ProcessSequence.SelectedIndex + 1, cb_ProcessSequence.Items.Count);
                }
            }
            else//离散报工
            {
                List<TechRouteLists> source = new List<TechRouteLists>();
                if (trList.Join(fcslist.Where(f => f.FCS_IsReported == false), tr => tr.ID, fcs => fcs.FCS_TechRouteID, (tr, fcs) => tr).Distinct().Count() > 0)
                {
                    source.Add(trList.Join(fcslist.Where(f => f.FCS_IsReported == false), tr => tr.ID, fcs => fcs.FCS_TechRouteID, (tr, fcs) => tr).Distinct().FirstOrDefault());
                }
                cb_ProcessSequence.ItemsSource = source;
            }
            cb_ProcessSequence.DisplayMemberPath = "TR_ProcessSequence";
            cb_ProcessSequence.SelectedValuePath = "ID";
        }


        /// <summary>
        /// 工序下拉框选择改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_ProcessSequence_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (!isNewFlowCard)
            {
                if (isAmountSaved)
                {
                    List<Int64> _fcsID = new List<long>();
                    foreach (FlowCardSubLists item in fcsls.FindAll(p => p.FCS_TechRouteID.Equals(cb_ProcessSequence.SelectedValue)))
                    {
                        _fcsID.Add(item.ID);
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

                        txtb_CheckPerson.Text = ((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_DefaultCheckPersonName;
                        txtb_ProcessName.Text = ((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_ProcessName;


                        personDataSource = fcsls.FindAll(p => p.FCS_TechRouteID.Equals(cb_ProcessSequence.SelectedValue));
                        datagrid_PersonInfo.ItemsSource = personDataSource;

                        beginamount = fcsls.Find(p => p.FCS_ProcessSequanece == ((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_ProcessSequence).FCS_BeginAmount;
                        txtb_BeginAmount.Minimum = beginamount;
                        txtb_BeginAmount.Text = beginamount.ToString();

                        txtb_ScrappedAmount.Text = scrapamount.ToString();
                        txtb_BeginAmount.IsReadOnly = !((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_IsFirstProcess;
                        txtb_UnprocessedAm.Text = fcsls.Find(p => p.FCS_ProcessSequanece == ((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_ProcessSequence).FCS_UnprocessedAm.ToString();



                        txtb_QulifiedAmount.Text = (beginamount - scrapamount - Convert.ToInt32(txtb_UnprocessedAm.Text)).ToString();
                    }
                    image_No.Visibility = personDataSource[0].FCS_IsReported ? Visibility.Hidden : Visibility.Visible;
                    image_Yes.Visibility = (!personDataSource[0].FCS_IsReported) ? Visibility.Hidden : Visibility.Visible;
                }
                else
                {
                    if (selectErrorCount == 0)
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
            }
            this.Cursor = Cursors.Arrow;
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
            string SQl = string.Format("Select A.[ID],A.[FCQ_FlowCardSubID],A.[FCQ_QulityIssueID],A.[FCQ_ScrapAmount],B.[QI_Code],B.[QI_Name] from [FlowCardQulity] A left join [QualityIssue] B on A.[FCQ_QulityIssueID]=B.[ID] where [FCQ_FlowCardSubID] in ({0})", fcsIDString);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCardQulity");
            MyDBController.CloseConnection();

            foreach (DataRow row in ds.Tables["FlowCardQulity"].Rows)
            {
                _list.Add(new FlowCardQualityLists()
                {
                    ID = Convert.ToInt64(row["ID"]),
                    FCQ_FlowCardSubID = Convert.ToInt64(row["FCQ_FlowCardSubID"]),
                    FCQ_QulityIssueID = Convert.ToInt64(row["FCQ_QulityIssueID"]),
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
                if (datagrid_AmountInfo.HasItems)
                {
                    list.Add(new FlowCardQualityLists() { QI_Code = "", QI_Name = "" });
                    datagrid_AmountInfo.Items.Refresh();
                }
                else
                {
                    list.Add(new FlowCardQualityLists() { QI_Code = "", QI_Name = "" });
                    datagrid_AmountInfo.Items.Refresh();
                }
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
            FlowCardQualityIssues_Window fci = new FlowCardQualityIssues_Window(AcceptQualityIssue);
            fci.ShowDialog();
        }

        /// <summary>
        /// 将选定的质量信息添加到列表中,并且保存到数据库中
        /// </summary>
        /// <param name="qil"></param>
        private void AcceptQualityIssue(QualityIssuesLists qil)
        {
            isChanged = true;
            FlowCardQualityLists item = ((FlowCardQualityLists)datagrid_AmountInfo.SelectedItem);
            item.QI_Code = qil.QI_Code;
            item.QI_Name = qil.QI_Name;
            DataSet ds = new DataSet();
            string SQl = string.Format(@"Select top 0 * from [FlowCardQulity]");
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCardQulity");
            List<string> colList = new List<string>();
            foreach (DataColumn col in ds.Tables["FlowCardQulity"].Columns)
            {
                colList.Add(col.ColumnName);
            }
            ds.Tables["FlowCardQulity"].Columns.Add(new DataColumn("IDNew", typeof(Int64)));
            DataRow row = ds.Tables["FlowCardQulity"].NewRow();
            row["ID"] = row["IDNew"] = item.ID;
            row["FCQ_FlowCardSubID"] = item.FCQ_FlowCardSubID > -1 ? item.FCQ_FlowCardSubID : ((FlowCardSubLists)datagrid_PersonInfo.Items[0]).ID;
            row["FCQ_QulityIssueID"] = qil.ID;
            row["FCQ_ScrapAmount"] = item.FCQ_ScrapAmount;
            ds.Tables["FlowCardQulity"].Rows.Add(row);

            int updateNum, insertNum;
            MyDBController.InsertSqlBulk(ds.Tables["FlowCardQulity"], colList, out updateNum, out insertNum);
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
                        if (Xceed.Wpf.Toolkit.MessageBox.Show("确定要删除该报废数量信息吗？该操作不可逆！", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            isChanged = true;
                            FlowCardQualityLists item = (FlowCardQualityLists)datagrid_AmountInfo.SelectedItem;
                            string SQl = string.Format(@"Delete from [FlowCardQulity] where [ID]={0}", item.ID);
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
                    if (datagrid_PersonInfo.HasItems && datagrid_PersonInfo.SelectedIndex != -1)
                    {
                        isChanged = true;
                        if (Xceed.Wpf.Toolkit.MessageBox.Show("确定要删除该人员信息吗？该操作不可逆！", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            FlowCardSubLists item = (FlowCardSubLists)datagrid_PersonInfo.SelectedItem;
                            string SQl = string.Format(@"Delete from [FlowCardSub] where [FCS_PersonCode]='{0}' and [FCS_TechRouteID]={1}", item.FCS_PersonCode, item.FCS_TechRouteID);
                            MyDBController.GetConnection();
                            int x = MyDBController.ExecuteNonQuery(SQl);
                            MyDBController.CloseConnection();
                            Xceed.Wpf.Toolkit.MessageBox.Show(string.Format("成功删除该人员信息，共{0}条记录。", x), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            fcsls.Remove(item);
                            datagrid_PersonInfo.ItemsSource = fcsls.Where(p => p.FCS_TechRouteID.Equals(cb_ProcessSequence.SelectedValue));
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
            if (datagrid_PersonInfo.HasItems)
            {
                TechRouteCheckPerson_Window trcp = new TechRouteCheckPerson_Window("操作工");
                trcp.ShowDialog();
                if ((bool)trcp.DialogResult)
                {
                    FlowCardSubLists item = (FlowCardSubLists)datagrid_PersonInfo.Items[0];
                    FlowCardSubLists newPerson = new FlowCardSubLists()
                    {
                        FCS_BeginAmount = item.FCS_BeginAmount,
                        FCS_CheckByID = item.FCS_CheckByID,
                        FCS_CheckByName = item.FCS_CheckByName,
                        FCS_FlowCradID = item.FCS_FlowCradID,
                        FCS_IsFirstProcess = item.FCS_IsFirstProcess,
                        FCS_IsLastProcess = item.FCS_IsLastProcess,
                        FCS_IsReported = item.FCS_IsReported,
                        FCS_ItemId = item.FCS_ItemId,
                        FCS_PieceAmount = item.FCS_PieceAmount,
                        FCS_PieceDivNum = item.FCS_PieceDivNum,
                        FCS_ProcessID = item.FCS_ProcessID,
                        FCS_ProcessName = item.FCS_ProcessName,
                        FCS_ProcessSequanece = item.FCS_ProcessSequanece,
                        FCS_QulifiedAmount = item.FCS_QulifiedAmount,
                        FCS_ScrappedAmount = item.FCS_ScrappedAmount,
                        FCS_TechRouteID = item.FCS_TechRouteID,
                        FCS_UnprocessedAm = item.FCS_UnprocessedAm,
                        FCS_PersonCode = trcp.checkPerson.code,
                        FCS_PersonName = trcp.checkPersonName
                    };
                    string SQl = string.Format(@"insert into [FlowCardSub]([FCS_FlowCradID],[FCS_ItemId],[FCS_TechRouteID],[FCS_ProcessID],[FCS_ProcessName],[FCS_PersonCode],[FCS_PersonName],[FCS_BeginAmount],[FCS_QulifiedAmount],[FCS_ScrappedAmount],[FCS_UnprocessedAm],[FCS_CheckByID],[FCS_CheckByName],[FCS_PieceAmount],[FCS_PieceDivNum],[FCS_IsFirstProcess],[FCS_IsLastProcess],[FCS_IsReported]) values({0},{1},{2},{3},'{4}','{5}','{6}',{7},{8},{9},{10},{11},'{12}',{13},{14},'{15}','{16}','{17}') ", newPerson.FCS_FlowCradID, newPerson.FCS_ItemId, newPerson.FCS_TechRouteID, newPerson.FCS_ProcessID, newPerson.FCS_ProcessName, newPerson.FCS_PersonCode, newPerson.FCS_PersonName, newPerson.FCS_BeginAmount, 0, 0, newPerson.FCS_UnprocessedAm, newPerson.FCS_CheckByID, newPerson.FCS_CheckByName, newPerson.FCS_PieceAmount, newPerson.FCS_PieceDivNum, newPerson.FCS_IsFirstProcess, newPerson.FCS_IsLastProcess, newPerson.FCS_IsReported);
                    MyDBController.GetConnection();
                    int count = MyDBController.ExecuteNonQuery(SQl);
                    MyDBController.CloseConnection();
                    if (count == 1)
                    {
                        isChanged = true;
                        Xceed.Wpf.Toolkit.MessageBox.Show("添加成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        fcsls.Add(newPerson);
                        personDataSource = fcsls.FindAll(p => p.FCS_TechRouteID.Equals(cb_ProcessSequence.SelectedValue));
                        datagrid_PersonInfo.ItemsSource = personDataSource;
                    }
                }
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 数量改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void integer_Amount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int? scrapamount = MyDBController.FindVisualChild<IntegerUpDown>(datagrid_AmountInfo).Sum(p => p.Value);
            int qualifiedAmount = (int)(txtb_BeginAmount.Value - scrapamount - txtb_UnprocessedAm.Value);
            if (qualifiedAmount < 0)
            {
                ((IntegerUpDown)sender).Value = (int?)e.OldValue;
            }
            else
            {
                //int diff = 0;
                //int amount = string.IsNullOrEmpty(txtb_ScrappedAmount.Text) ? 0 : Convert.ToInt32(txtb_ScrappedAmount.Text);

                //List<FlowCardQualityLists> _list = (List<FlowCardQualityLists>)datagrid_AmountInfo.ItemsSource;
                //int index = datagrid_AmountInfo.SelectedIndex;
                //diff += (int)e.NewValue - ((e.OldValue == null) ? _list[index].FCQ_ScrapAmount : (int)e.OldValue);
                //txtb_ScrappedAmount.Text = (amount + diff).ToString();
                txtb_ScrappedAmount.Text = scrapamount.ToString();
            }
            isAmountSaved = false;
        }

        /// <summary>
        /// 当数量改变的时候，保存数量的按钮启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_QulifiedAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            btn_SaveAmount.IsEnabled = true;
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
                    string SQl = string.Format(@"Update [FlowCardQulity] set [FCQ_ScrapAmount]={0} where [ID]={1}", _item.FCQ_ScrapAmount, _item.ID);
                    MyDBController.ExecuteNonQuery(SQl);
                }
                MyDBController.CloseConnection();
                isAmountSaved = true;
                HandleFlowCardSubInfo(fcl, fcsls);
            }
            Xceed.Wpf.Toolkit.MessageBox.Show("保存成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Cursor = Cursors.Arrow;
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
            if (isAmountSaved)
            {
                UpdateProcess();
                UpdateQuality();
                UpdateFlowCard();


                handledcount++;
                HandleData(tvFlowCard, fcsls);
                isChanged = false;
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("您还有数量信息没有保存，请先保存数量！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Cursor = Cursors.Arrow;
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
                item.FCS_ScrappedAmount = Convert.ToInt32(txtb_ScrappedAmount.Text);
                item.FCS_UnprocessedAm = Convert.ToInt32(txtb_UnprocessedAm.Text);
                item.FCS_QulifiedAmount = Convert.ToInt32(txtb_QulifiedAmount.Text);
                item.FCS_IsReported = true;
                DataRow row = ds.Tables["FlowCardSub"].NewRow();
                row["FCS_FlowCradID"] = item.FCS_FlowCradID;
                row["FCS_ItemId"] = item.FCS_ItemId;
                row["FCS_TechRouteID"] = item.FCS_TechRouteID;
                row["FCS_ProcessID"] = item.FCS_ProcessID;
                row["FCS_ProcessName"] = item.FCS_ProcessName;
                row["FCS_PersonCode"] = item.FCS_PersonCode;
                row["FCS_PersonName"] = item.FCS_PersonName;
                row["FCS_BeginAmount"] = item.FCS_BeginAmount;
                row["FCS_QulifiedAmount"] = item.FCS_QulifiedAmount;
                row["FCS_ScrappedAmount"] = item.FCS_ScrappedAmount;
                row["FCS_UnprocessedAm"] = item.FCS_UnprocessedAm;
                row["FCS_CheckByID"] = item.FCS_CheckByID;
                row["FCS_CheckByName"] = item.FCS_CheckByName;
                row["FCS_PieceAmount"] = item.FCS_PieceAmount;
                row["FCS_PieceDivNum"] = item.FCS_PieceDivNum;
                row["FCS_IsFirstProcess"] = item.FCS_IsFirstProcess;
                row["FCS_IsLastProcess"] = item.FCS_IsLastProcess;
                row["FCS_IsReported"] = item.FCS_IsReported;
                row["IDNew"] = row["ID"] = item.ID;
                ds.Tables["FlowCardSub"].Rows.Add(row);
            }
            int updateNum1, insertNum1;
            MyDBController.InsertSqlBulk(ds.Tables["FlowCardSub"], colList, out updateNum1, out insertNum1);
        }

        /// <summary>
        /// 报工时处理质量问题信息
        /// </summary>
        private void UpdateQuality()
        {
            MyDBController.GetConnection();
            DataSet ds = new DataSet();
            string SQl = "Select top 0 * from [FlowCardQulity]";
            MyDBController.GetDataSet(SQl, ds, "FlowCardQulity");
            List<string> colList = new List<string>();
            foreach (DataColumn col in ds.Tables["FlowCardQulity"].Columns)
            {
                colList.Add(col.ColumnName);
            }
            ds.Tables["FlowCardQulity"].Columns.Add(new DataColumn("IDNew", typeof(Int64)));
            foreach (FlowCardQualityLists fcql in list)
            {
                DataRow row = ds.Tables["FlowCardQulity"].NewRow();
                row["ID"] = row["IDNew"] = fcql.ID;
                row["FCQ_FlowCardSubID"] = fcql.FCQ_FlowCardSubID;
                row["FCQ_QulityIssueID"] = fcql.FCQ_QulityIssueID;
                row["FCQ_ScrapAmount"] = fcql.FCQ_ScrapAmount;
                ds.Tables["FlowCardQulity"].Rows.Add(row);
            }

            int updateNum2, insertNum2;
            MyDBController.InsertSqlBulk(ds.Tables["FlowCardQulity"], colList, out updateNum2, out insertNum2);
            MyDBController.CloseConnection();
            Xceed.Wpf.Toolkit.MessageBox.Show(string.Format("报工成功"), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 更新流转卡状态，改为报工
        /// </summary>
        private void UpdateFlowCard()
        {
            fcl.FC_CardState = 1;
            string SQl = string.Format(@"Update [FlowCard] set [FC_CardState]={0} where [ID]={1}", fcl.FC_CardState, fcl.ID);
            MyDBController.GetConnection();
            MyDBController.ExecuteNonQuery(SQl);
            MyDBController.CloseConnection();
            txtb_CardState.Text = (new FlowCardStateConverter()).Convert(fcl.FC_CardState, typeof(string), null, new System.Globalization.CultureInfo("")).ToString();
        }

        /// <summary>
        /// 所有工序完成报工之后，就可以进行整个流转卡的报工了，报工之后进行类似于完工报、入库单等等。
        /// </summary>
        private void TotalReport()
        {

        }
        #endregion


    }
}
