using BarCodeSystem.ProductDispatch.FlowCardReport;
using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.ValueConverters;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;
using BarCodeSystem.PublicClass.HelperClass;

namespace BarCodeSystem.ProductDispatch.FlowCardTransfer
{
    /// <summary>
    /// FlowCardTransfer_Page.xaml 的交互逻辑
    /// </summary>
    public partial class FlowCardTransfer_Page : Page
    {
        public FlowCardTransfer_Page()
        {
            InitializeComponent();
        }


        #region 变量
        /// <summary>
        /// 可转序的流转卡信息
        /// </summary>
        FlowCardLists fc = new FlowCardLists();

        /// <summary>
        /// 转序的新流转卡
        /// </summary>
        FlowCardLists newFC = new FlowCardLists();

        /// <summary>
        /// 流转卡行表信息
        /// </summary>
        List<FlowCardSubLists> fcsls = new List<FlowCardSubLists>();

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        TechVersion tv = new TechVersion();

        /// <summary>
        /// 转序开始的工序序号
        /// </summary>
        int startProcess = 0;

        /// <summary>
        /// 转序派工数量
        /// </summary>
        int statrAmount = 0;

        int loadCount = 0;
        #endregion

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



        #region 获取可转序的流转卡信息
        /// <summary>
        /// 回车快捷搜索
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
        /// 获取流转卡信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FlowCardSearch_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (string.IsNullOrEmpty(txtb_FlowCardSearch.Text))
            {
                GetAllFlowCard();
            }
            else
            {
                bool flag = CheckForCode(txtb_FlowCardSearch.Text);
                if (flag)
                {
                    GetInputFlowCard();
                }
                else
                {
                    //Xceed.Wpf.Toolkit.MessageBox.Show("输入的流转卡有误，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 找寻所有可转序的流转卡
        /// </summary>
        private void GetAllFlowCard()
        {
            textb_SearchHeader.Visibility = Visibility.Visible;
            frame_SearchFlowCard.Navigate(new FlowCardSearch_Page(RecieveFlowCardInfo, 5, "All", true, false));
        }

        /// <summary>
        /// 找寻指定编号的流转卡
        /// </summary>
        private void GetInputFlowCard()
        {
            FlowCardSearch_Page fcs = new FlowCardSearch_Page(RecieveFlowCardInfo, 5, txtb_FlowCardSearch.Text, true, true);
        }

        /// <summary>
        /// 检查输入的流转卡编号是否正确
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        private bool CheckForCode(string _key)
        {
            bool flag = false;
            MyDBController.GetConnection();
            string SQl = string.Format(@"Select count(*) from [FlowCard] A left join  [FlowCardSub] B on A.[ID]=B.[FCS_FlowCardID]  where [FC_Code]='{0}' and  [FC_CardState] in {1} and A.[ID] in(select distinct FCS_FlowCardID from [FlowCardSub]  where [FCS_UnprocessedAm]>0)", _key, "(2,5)");
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
                            Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡没有完工，不能转序，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case 3:
                            Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡已经被分批，不能转序，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case 4:
                            Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡已经被转序，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case 2:
                            Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡没有待处理数，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        default:
                            break;
                    }
                }
            }
            MyDBController.CloseConnection();
            return flag;
        }

        /// <summary>
        /// 接收选取的流转卡信息委托
        /// </summary>
        /// <param name="_fc"></param>
        /// <param name="_fcsls"></param>
        /// <param name="_tv"></param>
        private void RecieveFlowCardInfo(FlowCardLists _fc, List<FlowCardSubLists> _fcsls, TechVersion _tv)
        {
            AcceptInfo(_fc, _fcsls, _tv);
            DisplayInfo();
        }

        /// <summary>
        /// 接收信息
        /// </summary>
        private void AcceptInfo(FlowCardLists _fc, List<FlowCardSubLists> _fcsls, TechVersion _tv)
        {
            fc = _fc;
            fcsls = _fcsls;
            tv = _tv;
            frame_SearchFlowCard.Content = null;
            textb_SearchHeader.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 展示信息
        /// </summary>
        private void DisplayInfo()
        {
            txtb_FlowCardSearch.Text = fc.FC_Code;
            txtb_FlowCardNum.Text = fc.FC_Amount.ToString();
            txtb_FlowCardState.Text = ((new FlowCardStateConverter()).Convert(fc.FC_CardState, typeof(string), null, new System.Globalization.CultureInfo(""))).ToString();
            txtb_ItemInfo.Text = fc.PO_ItemCode + " | " + fc.PO_ItemName + " | " + fc.PO_ItemSpec;
            txtb_QualifiedNum.Text = fcsls[fcsls.Count - 1].FCS_QulifiedAmount.ToString();
            txtb_RestNum.Text = fcsls.Distinct(new ListComparer<FlowCardSubLists>((x, y) => (x.FCS_TechRouteID.Equals(y.FCS_TechRouteID)))).Sum(p => p.FCS_UnprocessedAm).ToString();
            txtb_SourceOrderCode.Text = fc.PO_Code;
        }
        #endregion



        #region 转序
        /// <summary>
        /// 预览新流转卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PreviewNew_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (fcsls.Count > 0)
            {
                newFC = GenerateNewFlowCard(newFC);
                DisplayNewCard(newFC);
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 生成新的流转卡，供预览和保存
        /// </summary>
        /// <returns></returns>
        private FlowCardLists GenerateNewFlowCard(FlowCardLists _newFC)
        {
            int _flowNum = GetLatestFlowNum();
            _newFC = InitNewCard(_newFC, _flowNum);
            return _newFC;
        }

        /// <summary>
        /// 获取最新的流转号
        /// </summary>
        /// <returns></returns>
        private int GetLatestFlowNum()
        {
            int flowNum = 0;
            string SQl = string.Format(@"select max(FC_FlowNum) as FC_FlowNum from FlowCard where convert(date,FC_Createtime,102)=Convert(date,getDate(),102)");
            MyDBController.GetConnection();
            try
            {
                flowNum = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));
            }
            catch (Exception)
            {
            }
            MyDBController.CloseConnection();
            return flowNum;
        }

        /// <summary>
        /// 初始化新流转卡信息
        /// </summary>
        /// <param name="_newFC"></param>
        /// <returns></returns>
        private FlowCardLists InitNewCard(FlowCardLists _newFC, int _flowNum)
        {
            _newFC.FC_Amount = fcsls.Find(p => p.FCS_UnprocessedAm > 0).FCS_UnprocessedAm;
            _newFC.FC_FirstProcessNum = fcsls.Find(p => p.FCS_UnprocessedAm > 0).FCS_ProcessSequanece;
            _newFC.FC_CardState = 0;
            _newFC.FC_CardType = 2;
            _newFC.FC_Code = "ZX-" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + "-" + string.Format("{0:0000}", (_flowNum + 1));
            _newFC.FC_CreateBy = User_Info.User_Name;
            _newFC.FC_CreateTime = _newFC.FC_CheckTime = DateTime.Now;
            _newFC.FC_FlowNum = _flowNum + 1;
            _newFC.FC_ItemID = fc.FC_ItemID;
            _newFC.FC_ItemTechVersionID = fc.FC_ItemTechVersionID;
            _newFC.FC_DistriSourceCard = fc.ID;
            _newFC.FC_SourceOrderID = fc.FC_SourceOrderID;
            _newFC.FC_WorkCenter = fc.FC_WorkCenter;
            _newFC.FC_BCSOrderID = fc.FC_BCSOrderID;
            _newFC.FC_CanReproduce = _newFC.FC_CanTransfer = _newFC.FC_HasDistributed = _newFC.FC_HasReproduced = _newFC.FC_HasTransfered = false;
            _newFC.FC_CanDistribute = true;
            _newFC.FC_Remark = "本流转卡为转序流转卡,来源为:" + fc.FC_Code + "|";
            _newFC.FC_IsSalaryCalculating = fc.FC_IsSalaryCalculating;
            return _newFC;
        }

        /// <summary>
        /// 展示新流转卡信息，供预览
        /// </summary>
        /// <param name="_newFC"></param>
        private void DisplayNewCard(FlowCardLists _newFC)
        {
            List<FlowCardLists> fcList = new List<FlowCardLists>();
            fcList.Add(_newFC);
            datagrid_NewFlowCard.ItemsSource = fcList;
        }

        /// <summary>
        /// 保存新流转卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveNew_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_NewFlowCard.HasItems)
            {
                this.Cursor = Cursors.Wait;
                TrySaveNewCard();
                this.Cursor = Cursors.Arrow;
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请先预览新流转卡信息再保存！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// 尝试保存新流转卡信息
        /// </summary>
        private void TrySaveNewCard()
        {
            bool flag = CheckWhetherRightFlowNum();
            while (!flag)
            {
                newFC.FC_FlowNum = GetLatestFlowNum();
                newFC.FC_Code = "ZX-" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + "-" + string.Format("{0:0000}", (newFC.FC_FlowNum + 1));
                flag = CheckWhetherRightFlowNum();
            }
            Int64 newID = CompileSaving();
            CompileSavingSub(newID);
            AfterSetting();
        }

        /// <summary>
        /// 检查是否最新流水号
        /// </summary>
        /// <returns></returns>
        private bool CheckWhetherRightFlowNum()
        {
            bool flag = false;
            string SQl = string.Format(@"Select count(*) from [FlowCard] where  convert(date,FC_Createtime,102)=Convert(date,getDate(),102) and [FC_FlowNum]={0}", newFC.FC_FlowNum);
            MyDBController.GetConnection();
            int count = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));
            MyDBController.CloseConnection();
            if (count == 0)
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 保存主表信息进入数据库
        /// </summary>
        private Int64 CompileSaving()
        {
            Int64 newID = 0;

            MyDBController.GetConnection();
            FlowCardLists.SaveInfo(MyDBController.CreateSqlCon(), newFC);
            fc.FC_Remark = "转序为流转卡:" + newFC.FC_Code + "|";
            fc.FC_CheckTime = DateTime.Now;
            string SQl = string.Format(@"Update [FlowCard] set [FC_CardState]=5,[FC_CheckTime]='{0}',[FC_HasTransfered]='true',[FC_CanTransfer]='false' ,[FC_Remark]='{1}' where [ID]={2}", fc.FC_CheckTime, fc.FC_Remark, fc.ID);
            int count = MyDBController.ExecuteNonQuery(SQl);
            if (count > 0)
            {
                SQl = string.Format(@"Select [ID] from  [FlowCard] where [FC_Code]='{0}'", newFC.FC_Code);
                newID = Convert.ToInt64(MyDBController.ExecuteScalar(SQl));
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("保存出错，请重试！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MyDBController.CloseConnection();
            return newID;
        }

        /// <summary>
        /// 保存行表信息进入数据库
        /// </summary>
        private void CompileSavingSub(Int64 _newID)
        {
            startProcess = fcsls.Find(p => p.FCS_UnprocessedAm > 0).FCS_ProcessSequanece;
            statrAmount = fcsls.Find(p => p.FCS_UnprocessedAm > 0).FCS_UnprocessedAm;
            var subList = fcsls.FindAll(p => p.FCS_ProcessSequanece >= startProcess);
            DataSet ds = new DataSet();
            int updateNum, insertNum;
            List<string> colList = new List<string>();
            string SQl = string.Format(@"Select top 0 * from [FlowCardSub]");
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCardSub");
            foreach (DataColumn col in ds.Tables["FlowCardSub"].Columns)
            {
                colList.Add(col.ColumnName);
            }
            ds.Tables["FlowCardSub"].Columns.Add(new DataColumn("IDNew", typeof(Int64)));
            foreach (FlowCardSubLists item in subList)
            {
                DataRow row = ds.Tables["FlowCardSub"].NewRow();
                row["FCS_FlowCardID"] = _newID;
                row["FCS_ItemId"] = item.FCS_ItemId;
                row["FCS_TechRouteID"] = item.FCS_TechRouteID;
                row["FCS_ProcessID"] = item.FCS_ProcessID;
                row["FCS_ProcessName"] = item.FCS_ProcessName;
                row["FCS_PersonCode"] = item.FCS_PersonCode;
                row["FCS_PersonName"] = item.FCS_PersonName;
                row["FCS_BeginAmount"] = statrAmount;
                #region 初始化 默认值
                row["FCS_QulifiedAmount"] = 0;
                row["FCS_ScrappedAmount"] = 0;
                if (item.FCS_ProcessSequanece == startProcess)
                {
                    row["FCS_AddAmount"] = 0;
                    row["FCS_IsFirstProcess"] = true;
                }
                else
                {
                    row["FCS_AddAmount"] = item.FCS_UnprocessedAm;
                    row["FCS_IsFirstProcess"] = false;
                }
                row["FCS_UnprocessedAm"] = 0;
                #endregion
                row["FCS_CheckByID"] = item.FCS_CheckByID;
                row["FCS_CheckByName"] = item.FCS_CheckByName;
                row["FCS_PieceAmount"] = item.FCS_PieceAmount;
                row["FCS_PieceDivNum"] = item.FCS_PieceDivNum;
                row["FCS_IsLastProcess"] = item.FCS_IsLastProcess;
                row["FCS_IsReported"] = false;
                ds.Tables["FlowCardSub"].Rows.Add(row);
            }
            MyDBController.InsertSqlBulk(ds.Tables["FlowCardSub"], colList, out updateNum, out insertNum);
            Xceed.Wpf.Toolkit.MessageBox.Show(string.Format("保存成功！\r\n新的流转卡编号为:{0}", newFC.FC_Code), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            MyDBController.CloseConnection();
        }



        /// <summary>
        /// 保存成功之后更改数据源
        /// </summary>
        private void AfterSetting()
        {
            fc = new FlowCardLists();
            fcsls = new List<FlowCardSubLists>();
            newFC = new FlowCardLists();
            datagrid_NewFlowCard.ItemsSource = null;
            foreach (TextBox item in MyDBController.FindVisualChild<TextBox>(this))
            {
                item.Text = "";
            }
        }
        #endregion


    }
}
