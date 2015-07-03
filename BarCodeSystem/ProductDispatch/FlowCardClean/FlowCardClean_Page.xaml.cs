using BarCodeSystem.ProductDispatch.FlowCardReport;
using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.ValueConverters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace BarCodeSystem.ProductDispatch.FlowCardClean
{
    /// <summary>
    /// FlowCardClean_Page.xaml 的交互逻辑
    /// </summary>
    public partial class FlowCardClean_Page : Page
    {
        public FlowCardClean_Page()
        {
            InitializeComponent();
        }

        #region 变量
        /// <summary>
        /// 流转卡行表
        /// </summary>
        List<FlowCardSubLists> fcsls = new List<FlowCardSubLists>();

        /// <summary>
        /// 流转卡信息
        /// </summary>
        FlowCardLists fc = new FlowCardLists();

        /// <summary>
        /// 流转卡工艺路线版本
        /// </summary>
        TechVersion tv = new TechVersion();
        #endregion

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #region 清卡
        /// <summary>
        /// 清卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FlowCardClean_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ChangeLocalData();
            ChangeDatabaseData();
            AfterSetting();
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 更改本地缓存数据
        /// </summary>
        private void ChangeLocalData()
        {
            fc.FC_CardState = 0;
            foreach (FlowCardSubLists item in fcsls)
            {
                item.FCS_BeginAmount = fc.FC_Amount;
                item.FCS_IsReported = false;
                item.FCS_QulifiedAmount = item.FCS_ScrappedAmount = item.FCS_UnprocessedAm = 0;
            }
            DisplayInfo();
        }

        /// <summary>
        /// 更改数据库数据
        /// </summary>
        private void ChangeDatabaseData()
        {
            bool flag = UpdateFlowCard();
            if (flag)
            {
                UpdateFlowCardSub();
            }
        }

        /// <summary>
        /// 更新流转卡主表信息
        /// </summary>
        /// <returns></returns>
        private bool UpdateFlowCard()
        {
            bool flag = false;
            string SQl = string.Format(@"update [FlowCard] set [FC_CardState]=0 where [ID]={0}", fc.ID);
            MyDBController.GetConnection();
            while (!flag)
            {
                int count = MyDBController.ExecuteNonQuery(SQl);
                if (count == 1)
                {
                    flag = true;
                }
            }
            MyDBController.CloseConnection();
            return flag;
        }


        /// <summary>
        /// 更新流转卡子表信息
        /// </summary>
        private void UpdateFlowCardSub()
        {
            DataSet ds = new DataSet();
            StringBuilder subID = new StringBuilder();
            List<string> colList = new List<string>();
            string SQl = string.Format(@"Select top 0 * from [FlowCardSub]");
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCardSub");

            foreach (DataColumn item in ds.Tables["FlowCardSub"].Columns)
            {
                colList.Add(item.ColumnName);
            }

            ds.Tables["FlowCardSub"].Columns.Add(new DataColumn("IDNew", typeof(Int64)));
            foreach (FlowCardSubLists item in fcsls)
            {
                DataRow row = ds.Tables["FlowCardSub"].NewRow();
                row["ID"] = row["IDNew"] = item.ID;
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
                ds.Tables["FlowCardSub"].Rows.Add(row);

                subID.Append(subID.Length == 0 ? item.ID.ToString() : "," + item.ID.ToString());
            }
            int updateNum, insertNum;
            MyDBController.InsertSqlBulk(ds.Tables["FlowCardSub"], colList, out updateNum, out insertNum);
            MyDBController.GetConnection();
            if ((updateNum + insertNum).Equals(ds.Tables["FlowCardSub"].Rows.Count))
            {
                DeleteQualityInfo(subID);
                Xceed.Wpf.Toolkit.MessageBox.Show("清卡成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            MyDBController.CloseConnection();
        }

        /// <summary>
        /// 删除数据库中的报废信息
        /// </summary>
        private void DeleteQualityInfo(StringBuilder _subID)
        {
            string SQl = string.Format(@"delete from [FlowCardQulity] where [FCQ_FlowCardSubID] in ({0})", _subID);
            MyDBController.ExecuteNonQuery(SQl);
        }

        /// <summary>
        /// 清卡结束后的动作
        /// </summary>
        private void AfterSetting()
        {
            fc = null;
            fcsls = null;
            tv = null;
            DisplayInfo();
        }
        #endregion

        #region 获取流转卡信息
        /// <summary>
        /// 搜索流转卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FlowCardSearch_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            textb_SearchHeader.Visibility = Visibility.Visible;
            if (string.IsNullOrEmpty(txtb_FlowCardSearch.Text))
            {
                GetAllFlowCard();
            }
            else
            {
                GetInputFlowCard();
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 回车快捷键搜索
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
        /// 搜索所有的处在报工状态的流转卡
        /// </summary>
        private void GetAllFlowCard()
        {
            frame_FlowCardSearch.Navigate(new FlowCardSearch_Page(RecieveFlowCardInfo, 1, "All"));
        }

        /// <summary>
        /// 搜索指定编号的流转卡
        /// </summary>
        private void GetInputFlowCard()
        {
            string key = CheckFlowCardCode();
            if (string.IsNullOrEmpty(key))
            {
            }
            else
            {
                FlowCardSearch_Page fcs = new FlowCardSearch_Page(RecieveFlowCardInfo, 1, key, true);
            }
        }

        /// <summary>
        /// 检查输入的流转卡编号是否正确,返回正确的流转卡编号或者空字符串
        /// </summary>
        /// <returns></returns>
        private string CheckFlowCardCode()
        {
            string SQl = string.Format(@"Select count(*) from [FlowCard] where [FC_Code]='{0}'", txtb_FlowCardSearch.Text);
            MyDBController.GetConnection();
            int count = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));
            if (count > 0)
            {
                return txtb_FlowCardSearch.Text;
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("输入的流转卡编号有误，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return "";
            }
        }


        /// <summary>
        /// 处理选取的流转卡信息
        /// </summary>
        /// <param name="_fc"></param>
        /// <param name="_fcslist"></param>
        /// <param name="_tv"></param>
        private void RecieveFlowCardInfo(FlowCardLists _fc, List<FlowCardSubLists> _fcslist, TechVersion _tv)
        {
            this.Cursor = Cursors.Wait;
            SettingAltering(_fc, _fcslist, _tv);
            DisplayInfo();
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 接收选取的流转卡相关变量
        /// </summary>
        /// <param name="_fc"></param>
        /// <param name="_fcslist"></param>
        /// <param name="_tv"></param>
        private void SettingAltering(FlowCardLists _fc, List<FlowCardSubLists> _fcslist, TechVersion _tv)
        {
            textb_SearchHeader.Visibility = Visibility.Collapsed;
            frame_FlowCardSearch.Content = null;
            fc = _fc;
            fcsls = _fcslist;
            tv = _tv;
        }

        /// <summary>
        /// 处理流转卡信息并展示。
        /// </summary>
        private void DisplayInfo()
        {
            DisPlayMainInfo();
            DisplaySubInfo();
        }

        /// <summary>
        /// 展示主表信息
        /// </summary>
        private void DisPlayMainInfo()
        {
            bool flag = fc == null;
            txtb_FlowCardSearch.Text = flag ? "" : fc.FC_Code;
            txtb_ItemInfo.Text = flag ? "" : fc.PO_ItemCode + " | " + fc.PO_ItemName + " | " + fc.PO_ItemSpec;
            txtb_FlowCardNum.Text = flag ? "" : fc.FC_Amount.ToString();
            txtb_FlowCardState.Text = flag ? "" : (new FlowCardStateConverter()).Convert(fc.FC_CardState, typeof(string), null, new System.Globalization.CultureInfo("")).ToString();
            txtb_SourceOrderCode.Text = flag ? "" : fc.FC_SourceOrderID.ToString();
        }

        /// <summary>
        /// 展示行表信息
        /// </summary>
        private void DisplaySubInfo()
        {
            bool flag = fcsls == null;
            var subInfo = flag ? null : fcsls.Select(p => new { FCS_ProcessSequanece = p.FCS_ProcessSequanece, FCS_ProcessName = p.FCS_ProcessName, FCS_BeginAmount = p.FCS_BeginAmount, FCS_QulifiedAmount = p.FCS_QulifiedAmount, FCS_ScrappedAmount = p.FCS_ScrappedAmount, FCS_UnprocessedAm = p.FCS_UnprocessedAm, FCS_IsReported = p.FCS_IsReported }).Distinct();
            datagrid_FlowCardSubInfo.ItemsSource = subInfo;
        }
        #endregion

    }
}
