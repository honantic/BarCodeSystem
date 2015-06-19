using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.ValueConverters;
using BarCodeSystem.TechRoute.TechRoute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Data;
using System.Text.RegularExpressions;
using Xceed.Wpf.Toolkit;
using BarCodeSystem.ProductDispatch.FlowCard;

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

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetBinding();
        }

        private void SetBinding()
        {
            Binding bd1 = new Binding("Text") { Source = txtb_BeginAmount };
            Binding bd2 = new Binding("Text") { Source = txtb_ScrappedAmount };
            Binding bd3 = new Binding("Text") { Source = txtb_UnprocessedAm };
            MultiBinding mb = new MultiBinding();
            mb.Converter = new QualifiedAmountConverter();
            mb.Bindings.Add(bd1);
            mb.Bindings.Add(bd2);
            mb.Bindings.Add(bd3);
            txtb_QulifiedAmount.SetBinding(TextBox.TextProperty, mb);
        }

        List<FlowCardSubLists> fcsls = new List<FlowCardSubLists>();
        /// <summary>
        /// 搜索流传卡号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FlowCardSearch_Click(object sender, RoutedEventArgs e)
        {
            if (textb_SearchInfo.Text.Equals("流转卡搜索页面"))
            {

            }
            else
            {
                textb_SearchInfo.Text = "流转卡搜索页面";
                Frame searchFrame = new Frame();
                gb_SearchInfo.Content = searchFrame;
                searchFrame.Navigate(new FlowCardSearch_Page(FetchFlowCard));
            }
        }

        /// <summary>
        /// 接收选定的流转卡信息的委托
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tv"></param>
        private void FetchFlowCard(FlowCardLists fc, List<FlowCardSubLists> _fcslist, TechVersion tv)
        {
            HandleFlowCardSubInfo(fc, _fcslist);
            bool flag = DisplayFlowCardInfo(fc, tv);
            if (flag)
            {
                BindProcess(tv, _fcslist);
            }
        }

        /// <summary>
        /// 处理流转卡子表信息。主要处理投入数、合格数信息
        /// </summary>
        /// <param name="_fcslist"></param>
        private void HandleFlowCardSubInfo(FlowCardLists fc, List<FlowCardSubLists> _fcslist)
        {
            _fcslist.Sort((p1, p2) => p1.FCS_ProcessSequanece.CompareTo(p2.FCS_ProcessSequanece));
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
        /// 为工序下拉框绑定工序信息数据源
        /// </summary>
        /// <param name="tv"></param>
        private void BindProcess(TechVersion tv, List<FlowCardSubLists> fcslist)
        {
            DataSet ds = new DataSet();
            string SQl = string.Format(@"Select [ID],[TR_ProcessSequence],[TR_ProcessName],[TR_ProcessCode],[TR_DefaultCheckPersonName] from [TechRoute] where [TR_VersionID]={0}", tv.ID);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "TechRoute");
            MyDBController.CloseConnection();

            List<TechRouteLists> trList = new List<TechRouteLists>();
            foreach (DataRow row in ds.Tables["TechRoute"].Rows)
            {
                trList.Add(new TechRouteLists()
                {
                    ID = Convert.ToInt64(row["ID"]),
                    TR_ProcessSequence = Convert.ToInt32(row["TR_ProcessSequence"]),
                    TR_ProcessName = row["TR_ProcessName"].ToString(),
                    TR_ProcessCode = row["TR_ProcessCode"].ToString(),
                    TR_DefaultCheckPersonName = row["TR_DefaultCheckPersonName"].ToString()
                });
            }
            if (tv.TRV_ReportWay == 0)//流水线报工
            {
                cb_ProcessSequence.ItemsSource = trList.Join(fcslist, tr => tr.ID, fcs => fcs.FCS_TechRouteID, (tr, fcs) => tr).Distinct();
            }
            else//离散报工
            {
                cb_ProcessSequence.ItemsSource = trList.Join(fcslist.Where(f => f.FCS_IsReported == false), tr => tr.ID, fcs => fcs.FCS_TechRouteID, (tr, fcs) => tr).Distinct();
            }
            cb_ProcessSequence.DisplayMemberPath = "TR_ProcessSequence";
            cb_ProcessSequence.SelectedValuePath = "ID";
        }

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
        /// 将选定的质量信息添加到列表中
        /// </summary>
        /// <param name="qil"></param>
        private void AcceptQualityIssue(QualityIssuesLists qil)
        {
            ((FlowCardQualityLists)datagrid_AmountInfo.SelectedItem).QI_Code = qil.QI_Code;
            ((FlowCardQualityLists)datagrid_AmountInfo.SelectedItem).QI_Name = qil.QI_Name;
        }

        /// <summary>
        /// 工序下拉框选择改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_ProcessSequence_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtb_CheckPerson.Text = ((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_DefaultCheckPersonName;
            txtb_ProcessName.Text = ((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_ProcessName;
            datagrid_PersonInfo.ItemsSource = fcsls.Where(p => p.FCS_TechRouteID.Equals(cb_ProcessSequence.SelectedValue));
            txtb_BeginAmount.Text = fcsls.Find(p => p.FCS_ProcessSequanece == ((TechRouteLists)cb_ProcessSequence.SelectedItem).TR_ProcessSequence).FCS_BeginAmount.ToString();
        }

        /// <summary>
        /// 添加报废信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddScrapInfo_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_AmountInfo.ItemsSource != null)
            {
                List<FlowCardQualityLists> list = datagrid_AmountInfo.ItemsSource.Cast<FlowCardQualityLists>().ToList<FlowCardQualityLists>();
                list.Add(new FlowCardQualityLists() { QI_Code = "", QI_Name = "" });
                datagrid_AmountInfo.ItemsSource = list;
            }
            else
            {
                datagrid_AmountInfo.ItemsSource = new List<FlowCardQualityLists>() { new FlowCardQualityLists() { QI_Code = "", QI_Name = "" } };
            }
        }

        /// <summary>
        /// 数量改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void integer_Amount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int diff = 0;
            int amount = string.IsNullOrEmpty(txtb_ScrappedAmount.Text) ? 0 : Convert.ToInt32(txtb_ScrappedAmount.Text);
            List<FlowCardQualityLists> list = (List<FlowCardQualityLists>)datagrid_AmountInfo.ItemsSource;
            int index = datagrid_AmountInfo.SelectedIndex;
            diff += (int)e.NewValue - ((e.OldValue == null) ? list[index].FCQ_ScrapAmount : (int)e.OldValue);
            txtb_ScrappedAmount.Text = (amount + diff).ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DeleteScrapInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Report_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
