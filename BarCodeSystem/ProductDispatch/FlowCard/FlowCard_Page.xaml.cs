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
using System.Windows.Threading;

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
        }

        #region 变量设置
        string amount = "";
        ProduceOrderLists selectedOrder = new ProduceOrderLists();
        List<TechRouteLists> selectedTechRoute = new List<TechRouteLists>();
        List<List<PersonLists>> techRoutePerson = new List<List<PersonLists>>();
        DispatcherTimer timer1 = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 2) };
        #endregion

        #region 初始化设置
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitFlowCardHeader();
            timer1.Tick += timer1_Tick;
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
            List<string> typeList = new List<string>() { "普通流转卡", "无来源流转卡" };
            cb_FlowCardType.ItemsSource = typeList;
            textb_CreatedBy.Text = "  " + User_Info.User_Name;
            datepicker_CreateDate.SelectedDate = DateTime.Now;
        }


        /// <summary>
        /// 页面大小改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
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
                        TechRouteSearch_Page trsp = new TechRouteSearch_Page(itemCode, FecthTechRouteInfo);
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
            if (!textb_SearchInfo.Text.Equals("生产订单筛选"))
            {
                Frame frame_SearchInfo = new Frame();
                gb_SearchInfo.Content = frame_SearchInfo;
                textb_SearchInfo.Text = "生产订单筛选";
                ProduceOrderSearch_Page posp = new ProduceOrderSearch_Page(FecthProduceOrderInfo);
                frame_SearchInfo.Navigate(posp);
            }
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
            btn_TechRouteSearch_Click(sender, e);
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
                foreach (PersonLists person in personList)
                {
                    //if (techRoutePerson[datagrid_TechRouteInfo.SelectedIndex].Count == 0)
                    //{
                    //    ((TechRouteLists)datagrid_TechRouteInfo.SelectedItem).personName += person.name;
                    //    techRoutePerson[datagrid_TechRouteInfo.SelectedIndex].Add(person);
                    //}
                    if (!techRoutePerson[datagrid_TechRouteInfo.SelectedIndex].Exists(p => p.code.Equals(person.code)))
                    {
                        int count = techRoutePerson[datagrid_TechRouteInfo.SelectedIndex].Count;
                        ((TechRouteLists)datagrid_TechRouteInfo.SelectedItem).personName += count == 0 ? person.name : "、" + person.name;
                        techRoutePerson[datagrid_TechRouteInfo.SelectedIndex].Add(person);
                    }
                }
                datagrid_TechRouteInfo.Items.Refresh();
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
            string message = "";
            if (CheckIfCanLegal(out message))
            {
                int flowNum = GetCurrentFlowNum();
                string flowCode = GenerateCode(flowNum);
                List<FlowCardLists> flowCardList = GenerateFlowCardInfo(flowNum, flowCode);
                FlowCardInfoToDatabase(flowCardList);
                UpdateSourceOrderInfo(Convert.ToInt32(textb_Amount.Text));
                SwitchReadOnlyPro();
            }
            else
            {
                MessageBox.Show(message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (!string.IsNullOrEmpty(selectedOrder.PO_ItemCode) && selectedTechRoute.Count != 0 && selectedOrder.PO_ItemCode.Equals(selectedTechRoute[0].TR_ItemCode))
            {
                if (!string.IsNullOrEmpty(datepicker_CreateDate.Text))
                {
                }
                else
                {
                    message = "请选择日期！";
                    flag = false;
                }
            }
            else
            {
                message = "缺少生产订单或者工艺路线信息,请检查！";
                flag = false;
            }

            if (flag)
            {
                foreach (TechRouteLists item in selectedTechRoute)
                {
                    if (string.IsNullOrEmpty(item.personName))
                    {
                        flag = false;
                        message = "请为每道工序选择操作人员！";
                        break;
                    }
                }
            }

            return flag;
        }

        /// <summary>
        /// 获得条码系统中当前日期的流转卡最大流水号，并加1，用来作为新的流转卡流水号
        /// </summary>
        /// <returns></returns>
        private int GetCurrentFlowNum()
        {
            int currentFlowNum = 0;
            string SQl = string.Format(@"select max(FC_FlowNum) FC_FlowNum from FlowCard where convert(date,FC_Createtime,102)=Convert(date,getDate(),102) group by Fc_Createtime");
            MyDBController.GetConnection();
            SqlDataReader sqlReader = MyDBController.GetDataReader(SQl);
            while (sqlReader.Read())
            {
                currentFlowNum = (int)sqlReader["FC_FlowNum"] + 1;
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
                case "无来源流转卡":
                    type = "WLY";
                    break;
                default:
                    break;
            }
            txtb_FlowCode.Text = type + "-" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + "-" + maxNum.ToString();
            return type + "-" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + "-" + maxNum.ToString();
        }

        /// <summary>
        /// 生成流转卡表头信息
        /// </summary>
        private List<FlowCardLists> GenerateFlowCardInfo(int maxNum, string flowCode)
        {
            List<FlowCardLists> flowCardList = new List<FlowCardLists>();
            foreach (TechRouteLists item in selectedTechRoute)
            {
                flowCardList.Add(new FlowCardLists()
                {
                    FC_CardType = cb_FlowCardType.SelectedIndex,
                    FC_SourceOrderID = selectedOrder.PO_ID,
                    FC_ItemID = Convert.ToInt64(selectedOrder.PO_ItemID),
                    FC_Amount = Convert.ToInt32(textb_Amount.Text),
                    FC_WorkCenter = selectedTechRoute[0].TR_WorkCenterID,
                    FC_CardState = 0,
                    FC_DistriSourceCard = 0,
                    FC_FlowNum = maxNum,
                    FC_CreateBy = User_Info.User_Name,
                    FC_CreateTime = DateTime.Now,
                    FC_Code = flowCode
                });
            }
            return flowCardList;
        }

        /// <summary>
        /// 将流转卡信息存入数据库
        /// </summary>
        private void FlowCardInfoToDatabase(List<FlowCardLists> flowCardList)
        {
            try
            {
                string SQl = "select top 0 * from FlowCard";
                DataSet ds = new DataSet();
                List<string> colList = new List<string>();

                MyDBController.GetConnection();
                MyDBController.GetDataSet(SQl, ds, "FlowCard");
                foreach (DataColumn column in ds.Tables["FlowCard"].Columns)
                {
                    colList.Add(column.ColumnName);
                }

                ds.Tables["FlowCard"].Columns.Add("IDNew", typeof(Int64));
                foreach (FlowCardLists item in flowCardList)
                {
                    DataRow row = ds.Tables["FlowCard"].NewRow();
                    row["ID"] = row["IDNew"] = item.ID;
                    row["FC_CardType"] = item.FC_CardType;
                    row["FC_SourceOrderID"] = item.FC_SourceOrderID;
                    row["FC_Code"] = item.FC_Code;
                    row["FC_ItemID"] = item.FC_ItemID;
                    row["FC_Amount"] = item.FC_Amount;
                    row["FC_WorkCenter"] = item.FC_WorkCenter;
                    row["FC_CardState"] = item.FC_CardState;
                    row["FC_DistriSourceCard"] = item.FC_DistriSourceCard;
                    row["FC_FlowNum"] = item.FC_FlowNum;
                    row["FC_CreateBy"] = item.FC_CreateBy;
                    row["FC_CheckTime"] = row["FC_CreateTime"] = item.FC_CreateTime;
                    row["FC_CheckBy"] = item.FC_CheckBy;
                    ds.Tables["FlowCard"].Rows.Add(row);
                }

                int updateNum, insertNum;
                MyDBController.InsertSqlBulk(ds.Tables["FlowCard"], colList, out updateNum, out insertNum);
                MyDBController.CloseConnection();

                string message = string.Format(@"共更新{0}条记录，新增{1}条记录", updateNum, insertNum);
                MessageBox.Show(message, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 派工结束之后返填生产订单信息，更新派工数量
        /// </summary>
        /// <param name="para"></param>
        private void UpdateSourceOrderInfo(int para)
        {
            string SQl = string.Format(@"update [ProduceOrder] set [PO_StartAmount]=[PO_StartAmount]+{0} where [PO_ID]={1}", para, selectedOrder.PO_ID);
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

            foreach (Button item in MyDBController.FindVisualChild<Button>(gb_header))
            {
                item.IsEnabled = false;
            }
            btn_DisFlowCard.IsEnabled = false;
            gb_SearchInfo.Content = null;
            cb_FlowCardType.IsReadOnly = true;
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
                textb_SearchInfo.Text = "选取班组信息";
                SaveWorkTeam_Page swtp = new SaveWorkTeam_Page(selectedTechRoute[0].TR_WorkCenterID, FetchPersonInfo);
                Frame frameSearch = new Frame();
                //frameSearch.Content = swtp;
                gb_SearchInfo.Content = frameSearch;
                frameSearch.Navigate(swtp);
            }
        }

        /// <summary>
        /// 接收筛选班组的人员信息的函数
        /// </summary>
        /// <param name="personList"></param>
        private void FectchTeamPerson(List<PersonLists> personList)
        {
            if (datagrid_TechRouteInfo.SelectedIndex != -1)
            {
                int x = datagrid_TechRouteInfo.SelectedIndex;
                foreach (PersonLists item in personList)
                {
                    if (techRoutePerson[x].Contains(item))
                    {
                        //什么都不做
                    }
                    else
                    {
                        int count = techRoutePerson[x].Count;
                        techRoutePerson[x].Add(item);
                        ((TechRouteLists)datagrid_TechRouteInfo.SelectedItem).personName += count == 0 ? item.name : "、" + item.name;
                    }
                }
                datagrid_TechRouteInfo.Items.Refresh();
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

        }
        #endregion
    }
}
