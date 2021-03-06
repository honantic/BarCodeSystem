﻿using BarCodeSystem.ProductDispatch.FlowCardReport;
using BarCodeSystem.PublicClass.DatabaseEntity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BarCodeSystem.ProductDispatch.FlowCardPrint;

namespace BarCodeSystem.ProductDispatch.FlowCardDistribute
{
    /// <summary>
    /// FlowCardDistribute_Page.xaml 的交互逻辑
    /// </summary>
    public partial class FlowCardDistribute_Page : Page
    {
        public FlowCardDistribute_Page()
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
            if (loadCount == 0)
            {
                txtb_FlowCardSearch.Focus();
                loadCount++;
            }
        }

        #region 变量
        /// <summary>
        /// 需要分批的流转卡
        /// </summary>
        FlowCardLists fc = new FlowCardLists();

        /// <summary>
        /// 拆分成新的流转卡的列表
        /// </summary>
        List<FlowCardLists> newfcList = new List<FlowCardLists>();

        ///// <summary>
        ///// 拆分的新流转卡主表ID
        ///// </summary>
        //List<Int64> newfcID = new List<long>();

        /// <summary>
        /// 需要分批的流转卡行表信息
        /// </summary>
        List<FlowCardSubLists> fcslist = new List<FlowCardSubLists>();

        /// <summary>
        /// 需要分批的流转卡工艺路线版本
        /// </summary>
        TechVersion tv = new TechVersion();

        int loadCount = 0;
        #endregion


        #region 搜索可以分批的流传卡
        /// <summary>
        /// 搜索流转卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FlowCardSearch_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            string flowcode = txtb_FlowCardSearch.Text;
            GetFlowCardInfo(flowcode);
            datagrid_NewFlowCardInfo.ItemsSource = null;
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 编号文本框回车键快捷搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_FlowCardSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_FlowCardSearch_Click(sender, e);
            }
        }

        /// <summary>
        /// 搜索流转卡信息
        /// </summary>
        /// <param name="_flowcode"></param>
        /// <returns></returns>
        private void GetFlowCardInfo(string _flowcode)
        {
            if (string.IsNullOrEmpty(_flowcode))//直接点击按钮，或者搜索空白流转卡号，展示所有可分批的流转卡
            {
                GetAllFlowCards();
            }
            else//输入某一流转卡号进行搜索，直接获取该流转卡信息
            {
                GetInputFlowCard(_flowcode);
            }
        }

        /// <summary>
        /// 获取所有可分批的流转卡，供用户选择
        /// </summary>
        /// <returns></returns>
        private void GetAllFlowCards()
        {
            textb_SearcInfo.Visibility = Visibility.Visible;
            FlowCardSearch_Page fcs = new FlowCardSearch_Page(RecieveFlowCardInfo, 0, "All", false);
            frame_FlowCardSearch.Navigate(fcs);
        }

        /// <summary>
        /// 接收选取的流转卡信息
        /// </summary>
        /// <param name="_fc"></param>
        /// <param name="_fcslist"></param>
        /// <param name="_tv"></param>
        private void RecieveFlowCardInfo(FlowCardLists _fc, List<FlowCardSubLists> _fcslist, TechVersion _tv)
        {
            textb_SearcInfo.Visibility = Visibility.Collapsed;
            frame_FlowCardSearch.Content = null;
            fc = _fc;
            fcslist = _fcslist;
            tv = _tv;
            if (fc != null)
            {
                txtb_OriginNum.Text = fc.FC_Amount.ToString();
                txtb_ItemInfo.Text = fc.PO_ItemCode + " | " + fc.PO_ItemName + " | " + fc.PO_ItemSpec;
                txtb_FlowCardSearch.Text = fc.FC_Code;
                txtb_SourceOrderCode.Text = fc.PO_Code;
            }
        }

        /// <summary>
        /// 直接获取输入的流转卡号对应信息
        /// </summary>
        /// <returns></returns>
        private void GetInputFlowCard(string _flowcode)
        {
            string SQl = string.Format(@"Select Count(*) from [FlowCard] where [FC_Code]='{0}' and [FC_CardState] = 0", _flowcode);
            DataSet ds = new DataSet();
            MyDBController.GetConnection();
            int count = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));

            if (count > 0)
            {
                FlowCardSearch_Page fcs = new FlowCardSearch_Page(RecieveFlowCardInfo, 0, txtb_FlowCardSearch.Text, true);
            }
            else
            {
                SQl = string.Format(@"Select count(*) from [FlowCard] where [FC_Code]='{0}' ", _flowcode);
                count = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));
                if (count == 0)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡编号不存在，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    SQl = string.Format(@"Select [FC_CardState] from [FlowCard] where [FC_Code]='{0}' ", _flowcode);
                    int state = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));
                    switch (state)
                    {
                        case 1:
                            Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡已经报工，不能分批，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case 2:
                            Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡已经完工，不能分批，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case 3:
                            Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡已经被分批，不能分批，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case 4:
                            Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡已经被转序，不能分批，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;

                        default:
                            break;
                    }
                    fc = null;
                }

                MyDBController.CloseConnection();
            }
        }
        #endregion

        #region 分批操作
        /// <summary>
        /// 开始拆分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TryDistribute_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(fc.FC_Code))
            {
                try
                {
                    int _currentFlowNum = CalCulateFlowNum();
                    newfcList.Clear();
                    for (int i = 0; i < intUD_DisNum.Value; i++)
                    {
                        newfcList.Add(GenerateNewFlowCard(fc, _currentFlowNum));
                        _currentFlowNum++;
                    }
                    datagrid_NewFlowCardInfo.ItemsSource = newfcList;
                    datagrid_NewFlowCardInfo.Items.Refresh();
                    textb_NewCardCount.Text = datagrid_NewFlowCardInfo.Items.Count.ToString();
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }

            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请先选择流转卡信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        /// <summary>
        /// 获取当前的可用的流水数最小值
        /// </summary>
        /// <returns></returns>
        private int CalCulateFlowNum()
        {
            int currentFlowNum = 0;
            string SQl = string.Format(@"select max(FC_FlowNum) as FC_FlowNum from FlowCard where convert(date,FC_Createtime,102)=Convert(date,getDate(),102) ");
            try
            {
                MyDBController.GetConnection();
                SqlDataReader sqlReader = MyDBController.GetDataReader(SQl);
                while (sqlReader.Read())
                {
                    if (sqlReader["FC_FlowNum"] is DBNull)
                    {
                        currentFlowNum = 1;
                    }
                    else
                    {
                        currentFlowNum = (int)sqlReader["FC_FlowNum"] + 1;
                    }
                }
                sqlReader.Close();
            }
            catch (Exception ee)
            {
                currentFlowNum = 1;
                MessageBox.Show(ee.Message + "\r\n请重试！");
            }
            MyDBController.CloseConnection();
            return currentFlowNum;
        }


        /// <summary>
        /// 生成新的流转卡信息
        /// </summary>
        /// <param name="_oldFlowCard"></param>
        /// <param name="_currentFlowNum"></param>
        /// <returns></returns>
        private FlowCardLists GenerateNewFlowCard(FlowCardLists _oldFlowCard, int _currentFlowNum)
        {
            return new FlowCardLists()
            {
                FC_Amount = 0,
                FC_CardState = 0,
                FC_CardType = 1,
                //FC_Code = (_oldFlowCard.FC_Code.Split('-'))[0] + "-" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + "-" + string.Format("{0:0000}", _currentFlowNum),
                FC_Code = "FP-" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + "-" + string.Format("{0:0000}", _currentFlowNum),
                //FC_CreateTime = DateTime.Now,
                FC_DistriSourceCard = _oldFlowCard.ID,
                FC_FlowNum = _currentFlowNum,
                FC_ItemID = _oldFlowCard.FC_ItemID,
                FC_ItemTechVersionID = _oldFlowCard.FC_ItemTechVersionID,
                FC_CreateBy = User_Info.User_Name,
                FC_SourceOrderID = _oldFlowCard.FC_SourceOrderID,
                FC_WorkCenter = _oldFlowCard.FC_WorkCenter,
                PO_Code = _oldFlowCard.PO_Code,
                PO_ItemCode = _oldFlowCard.PO_ItemCode,
                PO_ItemName = _oldFlowCard.PO_ItemName,
                PO_ItemSpec = _oldFlowCard.PO_ItemSpec,
                FC_BCSOrderID = _oldFlowCard.FC_BCSOrderID,
                FC_FirstProcessNum = _oldFlowCard.FC_FirstProcessNum,
                FC_CanDistribute = true,
                FC_CanReproduce = false,
                FC_CanTransfer = false,
                FC_CheckBy = "",
                FC_CheckTime = DateTime.Now,
                FC_CreateTime = DateTime.Now,
                FC_HasDistributed = false,
                FC_HasReproduced = false,
                FC_HasTransfered = false,
                FC_Remark = "本流转卡为分批流转卡，来源为:" + _oldFlowCard.FC_Code + "|",
                FC_IsSalaryCalculating = _oldFlowCard.FC_IsSalaryCalculating
            };
        }

        /// <summary>
        /// 取消分批
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CancelDistribute_Click(object sender, RoutedEventArgs e)
        {
            newfcList.Clear();
            datagrid_NewFlowCardInfo.ItemsSource = null;
        }

        /// <summary>
        /// 生成新流转卡信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_GenerateNewFlowCard_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            bool flag = CheckForAmount();
            if (flag)
            {
                NewFlowCardSubs();
                PrintNewFC(newfcList);
                AfterSetting();
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("新流转卡派工数量之和必须等于源流转卡派工数量！\r\n请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 批量打印
        /// </summary>
        private void PrintNewFC(List<FlowCardLists> fcList)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("是否需要批量打印？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                fcList.ForEach(p =>
                {
                    _10LinesFlowCard_Window _10lfc = new _10LinesFlowCard_Window(p.FC_Code);
                    _10lfc.ShowDialog();
                });
            }
        }


        /// <summary>
        ///  检查新的流转卡派工数量加起来是否等于源流转卡派工数量
        /// </summary>
        /// <returns></returns>
        private bool CheckForAmount()
        {
            bool flag = false;
            int amount = 0;
            foreach (FlowCardLists item in datagrid_NewFlowCardInfo.Items)
            {
                amount += item.FC_Amount;
            }
            if (amount.Equals(fc.FC_Amount))
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 生成新的流转卡子表信息
        /// </summary>
        private void NewFlowCardSubs()
        {
            bool canImport = EnsureFlowNum();
            while (!canImport)
            {
                canImport = EnsureFlowNum();
            }
            InsertNewFlowCardSub(newfcList, fcslist);
        }

        /// <summary>
        /// 确保流水号在保存的时候是没有重复的
        /// </summary>
        /// <returns></returns>
        private bool EnsureFlowNum()
        {
            int rightAmount = 0;
            if (fc != null)
            {
                bool flag = false;
                int maxFlowNum = newfcList.Max(p => p.FC_FlowNum);
                string SQl = "";
                MyDBController.GetConnection();

                try
                {
                    foreach (FlowCardLists item in newfcList)
                    {
                        int count = 0;
                        flag = false;
                        while (!flag)
                        {
                            SQl = string.Format(@"select count(*) from [FlowCard] where [FC_FlowNum]={0} and convert(date,FC_Createtime,102)=Convert(date,getDate(),102)", item.FC_FlowNum);
                            count = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));
                            if (count == 0)
                            {
                                InsertNewFlowCard(item);
                                rightAmount++;
                                flag = true;
                            }
                            else
                            {
                                maxFlowNum++;
                                item.FC_Code = item.FC_Code.Replace(string.Format("{0:0000}", item.FC_FlowNum), string.Format("{0:0000}", maxFlowNum));
                                item.FC_FlowNum = maxFlowNum;
                            }
                        }
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }

                MyDBController.CloseConnection();
            }

            return rightAmount.Equals(newfcList.Count);
        }

        /// <summary>
        /// 插入新的流传卡信息,并获得新流转卡的ID
        /// </summary>
        private void InsertNewFlowCard(FlowCardLists _fc)
        {
            string SQl1 = string.Format(@"Select [ID] from [FlowCard] where  [FC_CardType]={0} and [FC_SourceOrderID]={1} and [FC_DistriSourceCard]={2}  and [FC_Code]='{3}'", _fc.FC_CardType, _fc.FC_SourceOrderID, _fc.FC_DistriSourceCard, _fc.FC_Code);
            Int64 ID = 0;
            while (ID == 0)
            {
                FlowCardLists.SaveInfo(MyDBController.CreateSqlCon(), _fc);
                ID = Convert.ToInt64(MyDBController.ExecuteScalar(SQl1));
            }
            fc.FC_Remark += "拆分为流转卡:" + _fc.FC_Code + "|";
            _fc.ID = ID;
        }

        /// <summary>
        /// 插入新的流转卡行表信息
        /// </summary>
        /// <param name="_newfcList"></param>
        /// <param name="_fcslist"></param>
        private void InsertNewFlowCardSub(List<FlowCardLists> _newfcList, List<FlowCardSubLists> _fcslist)
        {
            DataSet ds = new DataSet();
            string SQl = string.Format(@"Select top 0 * from [FlowCardSub]");
            List<string> colList = new List<string>();
            MyDBController.GetConnection();
            try
            {
                MyDBController.GetDataSet(SQl, ds, "FlowCardSub");
                foreach (DataColumn col in ds.Tables["FlowCardSub"].Columns)
                {
                    colList.Add(col.ColumnName);
                }
                ds.Tables["FlowCardSub"].Columns.Add(new DataColumn("IDNew", typeof(Int64)));

                foreach (FlowCardLists _newfcl in _newfcList)
                {
                    foreach (FlowCardSubLists _oldfcl in _fcslist)
                    {
                        DataRow row = ds.Tables["FlowCardSub"].NewRow();
                        row["ID"] = row["IDNew"] = -1;
                        row["FCS_FlowCardID"] = _newfcl.ID;
                        row["FCS_ItemId"] = _oldfcl.FCS_ItemId;
                        row["FCS_TechRouteID"] = _oldfcl.FCS_TechRouteID;
                        row["FCS_ProcessID"] = _oldfcl.FCS_ProcessID;
                        row["FCS_ProcessName"] = _oldfcl.FCS_ProcessName;
                        row["FCS_PersonCode"] = _oldfcl.FCS_PersonCode;
                        row["FCS_PersonName"] = _oldfcl.FCS_PersonName;
                        row["FCS_BeginAmount"] = _newfcl.FC_Amount;
                        row["FCS_QulifiedAmount"] = _oldfcl.FCS_QulifiedAmount;
                        row["FCS_ScrappedAmount"] = _oldfcl.FCS_ScrappedAmount;
                        row["FCS_AddAmount"] = _oldfcl.FCS_AddAmount;
                        row["FCS_UnprocessedAm"] = _oldfcl.FCS_UnprocessedAm;
                        row["FCS_CheckByID"] = _oldfcl.FCS_CheckByID;
                        row["FCS_CheckByName"] = _oldfcl.FCS_CheckByName;
                        row["FCS_PieceAmount"] = _oldfcl.FCS_PieceAmount;
                        row["FCS_PieceDivNum"] = _oldfcl.FCS_PieceDivNum;
                        row["FCS_IsFirstProcess"] = _oldfcl.FCS_IsFirstProcess;
                        row["FCS_IsLastProcess"] = _oldfcl.FCS_IsLastProcess;
                        row["FCS_IsReported"] = _oldfcl.FCS_IsReported;
                        ds.Tables["FlowCardSub"].Rows.Add(row);
                    }
                }
                int updateNum, insertNum;
                MyDBController.InsertSqlBulk(ds.Tables["FlowCardSub"], colList, out updateNum, out insertNum);
                fc.FC_HasDistributed = true;
                fc.FC_CanDistribute = false;
                fc.FC_CardState = 5;
                fc.FC_CheckTime = DateTime.Now;
                FlowCardLists.SaveInfo(fc);
                if ((updateNum + insertNum).Equals(ds.Tables["FlowCardSub"].Rows.Count))
                {
                    string message = "";
                    foreach (FlowCardLists item in newfcList)
                    {
                        message += item.FC_Code + "\r\n";
                    }
                    Xceed.Wpf.Toolkit.MessageBox.Show("操作成功！新的流转卡号为：\r\n" + message, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {

                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

            MyDBController.CloseConnection();
        }


        /// <summary>
        /// 批量设置数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SetAmount_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_NewFlowCardInfo.HasItems)
            {
                FlowCardAmountSet_Window fas = new FlowCardAmountSet_Window(datagrid_NewFlowCardInfo.Items.Count, Convert.ToInt32(txtb_OriginNum.Text), SetNewFCAmount);
                fas.ShowDialog();
            }
        }

        /// <summary>
        /// 批量设置数量委托函数
        /// </summary>
        /// <param name="_fisrtSetCount"></param>
        /// <param name="_firstSetAmount"></param>
        /// <param name="_secSetCount"></param>
        /// <param name="_secSetAmount"></param>
        private void SetNewFCAmount(int _fisrtSetCount, int _firstSetAmount, int _secSetCount, int _secSetAmount)
        {
            if (fc.FC_Amount >= _fisrtSetCount * _firstSetAmount + _secSetCount * _secSetAmount)
            {
                for (int i = 0; i < newfcList.Count; i++)
                {
                    if (i < _fisrtSetCount)
                    {
                        newfcList[i].FC_Amount = _firstSetAmount;
                    }
                    else if (_fisrtSetCount <= i && i < _fisrtSetCount + _secSetCount)
                    {
                        newfcList[i].FC_Amount = _secSetAmount;
                    }
                }
                datagrid_NewFlowCardInfo.Items.Refresh();
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("总数量超过了原派工数量！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 分批完成之后复位操作
        /// </summary>
        private void AfterSetting()
        {
            datagrid_NewFlowCardInfo.ItemsSource = null;
            fc = null;
            newfcList.Clear();
            fcslist.Clear();
            tv = null;
            txtb_FlowCardSearch.Text = txtb_ItemInfo.Text = txtb_OriginNum.Text = txtb_SourceOrderCode.Text = "";
        }
        #endregion

    }
}
