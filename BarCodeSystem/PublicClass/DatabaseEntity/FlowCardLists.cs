using BarCodeSystem.PublicClass.HelperClass;
using BarCodeSystem.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace BarCodeSystem.PublicClass.DatabaseEntity
{
    public class FlowCardLists
    {
        /// <summary>
        /// 流转卡ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 流转卡类型0：普通流转卡，1：返工流转卡，2：分批流转卡，3：无来源流转卡
        /// </summary>
        public int FC_CardType { get; set; }
        /// <summary>
        /// 来源单据ID
        /// </summary>
        public Int64 FC_SourceOrderID { get; set; }
        /// <summary>
        /// 流转卡编号
        /// </summary>
        public string FC_Code { get; set; }
        /// <summary>
        /// 料品ID
        /// </summary>
        public Int64 FC_ItemID { get; set; }
        /// <summary>
        /// 料品工艺路线版本id
        /// </summary>
        public Int64 FC_ItemTechVersionID { get; set; }
        /// <summary>
        /// 流转数量、派工数量
        /// </summary>
        public int FC_Amount { get; set; }
        /// <summary>
        /// 生产车间ID
        /// </summary>
        public Int64 FC_WorkCenter { get; set; }
        /// <summary>
        /// 单据状态（0:开立，1:报工，2:完工未入库 3:被分批 4:被转序 5:完工已入库）
        /// </summary>
        public int FC_CardState { get; set; }
        /// <summary>
        /// 分批流转卡来源流转卡ID
        /// </summary>
        public Int64 FC_DistriSourceCard { get; set; }
        /// <summary>
        /// 流转卡编号的四位流水号
        /// </summary>
        public int FC_FlowNum { get; set; }
        /// <summary>
        /// 制单人
        /// </summary>
        public string FC_CreateBy { get; set; }
        /// <summary>
        /// 制单日期
        /// </summary>
        public DateTime FC_CreateTime { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string FC_CheckBy { get; set; }
        /// <summary>
        /// 审核日期
        /// </summary>
        public DateTime FC_CheckTime { get; set; }
        /// <summary>
        /// 部门名称，从工作中心表中关联得到
        /// </summary>
        public string WC_Department_Name { get; set; }
        /// <summary>
        /// 料品名称，从生产订单表中关联得到
        /// </summary>
        public string PO_ItemName { get; set; }
        /// <summary>
        /// 料品编码，从生产订单表中关联得到
        /// </summary>
        public string PO_ItemCode { get; set; }
        /// <summary>
        /// 料品规格，从生产订单表中关联得到
        /// </summary>
        public string PO_ItemSpec { get; set; }
        /// <summary>
        /// 工艺路线版本编码，从工艺路线版本表中关联得到
        /// </summary>
        public string TRV_VersionCode { get; set; }
        /// <summary>
        /// 工艺路线版本名称，从工艺路线版本表中关联得到
        /// </summary>
        public string TRV_VersionName { get; set; }
        /// <summary>
        /// 来源生产订单编号，从生产订单表中关联得到
        /// </summary>
        public string PO_Code { get; set; }

        /// <summary>
        /// 条码系统中的生产订单ID
        /// </summary>
        public Int64 FC_BCSOrderID
        {
            get;
            set;
        }

        /// <summary>
        /// 第一道工序号
        /// </summary>
        public int FC_FirstProcessNum { get; set; }

        /// <summary>
        /// 检查流转卡编号是否存在
        /// </summary>
        /// <param name="_fcCode"></param>
        /// <returns></returns>
        public static bool CheckForCode(string _fcCode)
        {
            bool flag = false;
            int count;
            string SQl = string.Format(@"select count(*) from [FlowCard] where [FC_Code]='{0}'", _fcCode);
            MyDBController.GetConnection();

            try
            {
                count = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));
                if (count > 0)
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡订单编号不存在！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                flag = false;
            }
            MyDBController.CloseConnection();
            return flag;
        }

        /// <summary>
        /// 根据流转卡号搜寻流转卡信息，返回流转卡列表，正常情况下，列表的长度<=1
        /// </summary>
        /// <param name="_fcCode"></param>
        /// <returns></returns>
        public static List<FlowCardLists> FetchFC_InfoByCode(string _fcCode)
        {
            DataSet ds = new DataSet();
            string SQl = string.Format(@"Select A.[ID],A.[FC_CardType],[FC_SourceOrderID],A.[FC_Code],A.[FC_ItemID],A.[FC_ItemTechVersionID],A.[FC_Amount],A.[FC_WorkCenter],A.[FC_CardState],A.[FC_DistriSourceCard],A.[FC_FlowNum],A.[FC_CreateBy],A.[FC_CreateTime],A.[FC_CheckBy],A.[FC_CheckTime],A.[FC_BCSOrderID],B.[PO_ItemCode],B.[PO_ItemName],B.[PO_ItemSpec],B.[PO_Code],C.[TRV_VersionCode],C.[TRV_VersionName],D.[WC_Department_Name] from [FlowCard] A left join [ProduceOrder] B on A.[FC_SourceOrderID]=B.[PO_ID] left join [TechRouteVersion] C on A.[FC_ItemTechVersionID]=C.[ID] left join [WorkCenter] D on A.[FC_WorkCenter]=D.[WC_Department_ID] where A.[FC_Code] ='{0}'", _fcCode);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCard");
            MyDBController.CloseConnection();

            List<FlowCardLists> fcls = new List<FlowCardLists>();
            foreach (DataRow row in ds.Tables["FlowCard"].Rows)
            {
                fcls.Add(new FlowCardLists()
                {
                    FC_Amount = Convert.ToInt32(row["FC_Amount"]),
                    FC_CardState = Convert.ToInt32(row["FC_CardState"]),
                    FC_CardType = Convert.ToInt32(row["FC_CardType"]),
                    FC_ItemID = Convert.ToInt64(row["FC_ItemID"]),
                    FC_FlowNum = Convert.ToInt32(row["FC_FlowNum"]),
                    FC_DistriSourceCard = Convert.ToInt64(row["FC_DistriSourceCard"]),
                    FC_ItemTechVersionID = Convert.ToInt64(row["FC_ItemTechVersionID"]),
                    FC_SourceOrderID = Convert.ToInt64(row["FC_SourceOrderID"]),
                    FC_WorkCenter = Convert.ToInt64(row["FC_WorkCenter"]),
                    FC_CheckBy = row["FC_CheckBy"].ToString(),
                    FC_CheckTime = Convert.ToDateTime(row["FC_CheckTime"]),
                    FC_Code = row["FC_Code"].ToString(),
                    FC_CreateBy = row["FC_CreateBy"].ToString(),
                    FC_CreateTime = Convert.ToDateTime(row["FC_CreateTime"]),
                    ID = Convert.ToInt64(row["ID"]),
                    PO_ItemCode = row["PO_ItemCode"].ToString(),
                    PO_ItemName = row["PO_ItemName"].ToString(),
                    PO_ItemSpec = row["PO_ItemSpec"].ToString(),
                    WC_Department_Name = row["WC_Department_Name"].ToString(),
                    TRV_VersionCode = row["TRV_VersionCode"].ToString(),
                    TRV_VersionName = row["TRV_VersionName"].ToString(),
                    PO_Code = row["PO_Code"].ToString(),
                    FC_BCSOrderID = row["FC_BCSOrderID"] is DBNull ? -1 : Convert.ToInt64(row["FC_BCSOrderID"])
                });
            }
            fcls = fcls.Distinct(new ListComparer<FlowCardLists>((x, y) => x.FC_Code.Equals(y.FC_Code))).ToList();
            return fcls;
        }

        /// <summary>
        /// 根据流转卡状态搜寻流转卡信息，返回流转卡列表
        /// </summary>
        /// <param name="_fcState"></param>
        /// <returns></returns>
        public static List<FlowCardLists> FetchFC_InfoByState(int _fcState)
        {
            DataSet ds = new DataSet();
            string SQl = string.Format(@"Select A.[ID],A.[FC_CardType],[FC_SourceOrderID],A.[FC_Code],A.[FC_ItemID],A.[FC_ItemTechVersionID],A.[FC_Amount],A.[FC_WorkCenter],A.[FC_CardState],A.[FC_DistriSourceCard],A.[FC_FlowNum],A.[FC_CreateBy],A.[FC_CreateTime],A.[FC_CheckBy],A.[FC_CheckTime],A.[FC_BCSOrderID],B.[PO_ItemCode],B.[PO_ItemName],B.[PO_ItemSpec],B.[PO_Code],C.[TRV_VersionCode],C.[TRV_VersionName],D.[WC_Department_Name] from [FlowCard] A left join [ProduceOrder] B on A.[FC_SourceOrderID]=B.[PO_ID] left join [TechRouteVersion] C on A.[FC_ItemTechVersionID]=C.[ID] left join [WorkCenter] D on A.[FC_WorkCenter]=D.[WC_Department_ID] where A.[FC_CardState] ={0}", _fcState);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCard");
            MyDBController.CloseConnection();

            List<FlowCardLists> fcls = new List<FlowCardLists>();
            foreach (DataRow row in ds.Tables["FlowCard"].Rows)
            {
                fcls.Add(new FlowCardLists()
                {
                    FC_Amount = Convert.ToInt32(row["FC_Amount"]),
                    FC_CardState = Convert.ToInt32(row["FC_CardState"]),
                    FC_CardType = Convert.ToInt32(row["FC_CardType"]),
                    FC_ItemID = Convert.ToInt64(row["FC_ItemID"]),
                    FC_FlowNum = Convert.ToInt32(row["FC_FlowNum"]),
                    FC_DistriSourceCard = Convert.ToInt64(row["FC_DistriSourceCard"]),
                    FC_ItemTechVersionID = Convert.ToInt64(row["FC_ItemTechVersionID"]),
                    FC_SourceOrderID = Convert.ToInt64(row["FC_SourceOrderID"]),
                    FC_WorkCenter = Convert.ToInt64(row["FC_WorkCenter"]),
                    FC_CheckBy = row["FC_CheckBy"].ToString(),
                    FC_CheckTime = Convert.ToDateTime(row["FC_CheckTime"]),
                    FC_Code = row["FC_Code"].ToString(),
                    FC_CreateBy = row["FC_CreateBy"].ToString(),
                    FC_CreateTime = Convert.ToDateTime(row["FC_CreateTime"]),
                    ID = Convert.ToInt64(row["ID"]),
                    PO_ItemCode = row["PO_ItemCode"].ToString(),
                    PO_ItemName = row["PO_ItemName"].ToString(),
                    PO_ItemSpec = row["PO_ItemSpec"].ToString(),
                    WC_Department_Name = row["WC_Department_Name"].ToString(),
                    TRV_VersionCode = row["TRV_VersionCode"].ToString(),
                    TRV_VersionName = row["TRV_VersionName"].ToString(),
                    PO_Code = row["PO_Code"].ToString(),
                    FC_BCSOrderID = row["FC_BCSOrderID"] is DBNull ? -1 : Convert.ToInt64(row["FC_BCSOrderID"])
                });
            }
            fcls = fcls.Distinct(new ListComparer<FlowCardLists>((x, y) => x.FC_Code.Equals(y.FC_Code))).ToList();
            return fcls;
        }

        /// <summary>
        /// 获取多种状态的的流转卡列表
        /// </summary>
        /// <param name="_fcState"></param>
        /// <returns></returns>
        public static List<FlowCardLists> FetchFC_InfoByState(params int[] _fcState)
        {
            string stateList = "";
            foreach (int _state in _fcState)
            {
                stateList += stateList.Length == 0 ? _state.ToString() : "," + _state.ToString();
            }
            DataSet ds = new DataSet();
            string SQl = string.Format(@"Select A.[ID],A.[FC_CardType],[FC_SourceOrderID],A.[FC_Code],A.[FC_ItemID],A.[FC_ItemTechVersionID],A.[FC_Amount],A.[FC_WorkCenter],A.[FC_CardState],A.[FC_DistriSourceCard],A.[FC_FlowNum],A.[FC_CreateBy],A.[FC_CreateTime],A.[FC_CheckBy],A.[FC_CheckTime],A.[FC_BCSOrderID],B.[PO_ItemCode],B.[PO_ItemName],B.[PO_ItemSpec],B.[PO_Code],C.[TRV_VersionCode],C.[TRV_VersionName],D.[WC_Department_Name] from [FlowCard] A left join [ProduceOrder] B on A.[FC_SourceOrderID]=B.[PO_ID] left join [TechRouteVersion] C on A.[FC_ItemTechVersionID]=C.[ID] left join [WorkCenter] D on A.[FC_WorkCenter]=D.[WC_Department_ID] where A.[FC_CardState] in ({0})", stateList);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCard");
            MyDBController.CloseConnection();

            List<FlowCardLists> fcls = new List<FlowCardLists>();
            foreach (DataRow row in ds.Tables["FlowCard"].Rows)
            {
                fcls.Add(new FlowCardLists()
                {
                    FC_Amount = Convert.ToInt32(row["FC_Amount"]),
                    FC_CardState = Convert.ToInt32(row["FC_CardState"]),
                    FC_CardType = Convert.ToInt32(row["FC_CardType"]),
                    FC_ItemID = Convert.ToInt64(row["FC_ItemID"]),
                    FC_FlowNum = Convert.ToInt32(row["FC_FlowNum"]),
                    FC_DistriSourceCard = Convert.ToInt64(row["FC_DistriSourceCard"]),
                    FC_ItemTechVersionID = Convert.ToInt64(row["FC_ItemTechVersionID"]),
                    FC_SourceOrderID = Convert.ToInt64(row["FC_SourceOrderID"]),
                    FC_WorkCenter = Convert.ToInt64(row["FC_WorkCenter"]),
                    FC_CheckBy = row["FC_CheckBy"].ToString(),
                    FC_CheckTime = Convert.ToDateTime(row["FC_CheckTime"]),
                    FC_Code = row["FC_Code"].ToString(),
                    FC_CreateBy = row["FC_CreateBy"].ToString(),
                    FC_CreateTime = Convert.ToDateTime(row["FC_CreateTime"]),
                    ID = Convert.ToInt64(row["ID"]),
                    PO_ItemCode = row["PO_ItemCode"].ToString(),
                    PO_ItemName = row["PO_ItemName"].ToString(),
                    PO_ItemSpec = row["PO_ItemSpec"].ToString(),
                    WC_Department_Name = row["WC_Department_Name"].ToString(),
                    TRV_VersionCode = row["TRV_VersionCode"].ToString(),
                    TRV_VersionName = row["TRV_VersionName"].ToString(),
                    PO_Code = row["PO_Code"].ToString(),
                    FC_BCSOrderID = row["FC_BCSOrderID"] is DBNull ? -1 : Convert.ToInt64(row["FC_BCSOrderID"])
                });
            }
            fcls = fcls.Distinct(new ListComparer<FlowCardLists>((x, y) => x.FC_Code.Equals(y.FC_Code))).ToList();
            return fcls;
        }

        /// <summary>
        /// 根据指定的时间字段，筛选符合时间条件的流转卡信息
        /// </summary>
        /// <param name="_startTime">开始时间，不能为空</param>
        /// <param name="_endTime">结束时间，不能为空</param>
        /// <param name="_dateType">筛选的字段，0:编制日期 1:审核日期 ，其余的将视为非法参数</param>
        /// <returns></returns>
        public static List<FlowCardLists> FetchFC_InfoByDate(DateTime _startTime, DateTime _endTime, int _dateType)
        {
            DataSet ds = new DataSet();
            List<FlowCardSubLists> fcsls = new List<FlowCardSubLists>();
            string SQl = "";
            if (_dateType == 0)
            {
                SQl = string.Format(@"Select A.[ID],A.[FC_CardType],[FC_SourceOrderID],A.[FC_Code],A.[FC_ItemID],A.[FC_ItemTechVersionID],A.[FC_Amount],A.[FC_WorkCenter],A.[FC_CardState],A.[FC_DistriSourceCard],A.[FC_FlowNum],A.[FC_CreateBy],A.[FC_CreateTime],A.[FC_CheckBy],A.[FC_CheckTime],A.[FC_BCSOrderID],B.[PO_ItemCode],B.[PO_ItemName],B.[PO_ItemSpec],B.[PO_Code],C.[TRV_VersionCode],C.[TRV_VersionName],D.[WC_Department_Name] from [FlowCard] A left join [ProduceOrder] B on A.[FC_SourceOrderID]=B.[PO_ID] left join [TechRouteVersion] C on A.[FC_ItemTechVersionID]=C.[ID] left join [WorkCenter] D on A.[FC_WorkCenter]=D.[WC_Department_ID] where A.[FC_CreateTime] >='{0}' and A.[FC_CreateTime] <='{1}' and A.[FC_CardState] not in (3)", MyDBController.HandleTimeInfo(_startTime.ToString()), MyDBController.HandleTimeInfo(_endTime.ToString()));
            }
            else if (_dateType == 1)
            {
                SQl = string.Format(@"Select A.[ID],A.[FC_CardType],[FC_SourceOrderID],A.[FC_Code],A.[FC_ItemID],A.[FC_ItemTechVersionID],A.[FC_Amount],A.[FC_WorkCenter],A.[FC_CardState],A.[FC_DistriSourceCard],A.[FC_FlowNum],A.[FC_CreateBy],A.[FC_CreateTime],A.[FC_CheckBy],A.[FC_CheckTime],A.[FC_BCSOrderID],B.[PO_ItemCode],B.[PO_ItemName],B.[PO_ItemSpec],B.[PO_Code],C.[TRV_VersionCode],C.[TRV_VersionName],D.[WC_Department_Name] from [FlowCard] A left join [ProduceOrder] B on A.[FC_SourceOrderID]=B.[PO_ID] left join [TechRouteVersion] C on A.[FC_ItemTechVersionID]=C.[ID] left join [WorkCenter] D on A.[FC_WorkCenter]=D.[WC_Department_ID] where A.[FC_CheckTime] >='{0}' and A.[FC_CheckTime] <='{1}' and A.[FC_CardState] not in (3)", MyDBController.HandleTimeInfo(_startTime.ToString()), MyDBController.HandleTimeInfo(_endTime.ToString()));
            }
            else
            {
                return null;
            }

            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCard");
            MyDBController.CloseConnection();

            List<FlowCardLists> fcls = new List<FlowCardLists>();
            foreach (DataRow row in ds.Tables["FlowCard"].Rows)
            {
                fcls.Add(new FlowCardLists()
                {
                    FC_Amount = Convert.ToInt32(row["FC_Amount"]),
                    FC_CardState = Convert.ToInt32(row["FC_CardState"]),
                    FC_CardType = Convert.ToInt32(row["FC_CardType"]),
                    FC_ItemID = Convert.ToInt64(row["FC_ItemID"]),
                    FC_FlowNum = Convert.ToInt32(row["FC_FlowNum"]),
                    FC_DistriSourceCard = Convert.ToInt64(row["FC_DistriSourceCard"]),
                    FC_ItemTechVersionID = Convert.ToInt64(row["FC_ItemTechVersionID"]),
                    FC_SourceOrderID = Convert.ToInt64(row["FC_SourceOrderID"]),
                    FC_WorkCenter = Convert.ToInt64(row["FC_WorkCenter"]),
                    FC_CheckBy = row["FC_CheckBy"].ToString(),
                    FC_CheckTime = Convert.ToDateTime(row["FC_CheckTime"]),
                    FC_Code = row["FC_Code"].ToString(),
                    FC_CreateBy = row["FC_CreateBy"].ToString(),
                    FC_CreateTime = Convert.ToDateTime(row["FC_CreateTime"]),
                    ID = Convert.ToInt64(row["ID"]),
                    PO_ItemCode = row["PO_ItemCode"].ToString(),
                    PO_ItemName = row["PO_ItemName"].ToString(),
                    PO_ItemSpec = row["PO_ItemSpec"].ToString(),
                    WC_Department_Name = row["WC_Department_Name"].ToString(),
                    TRV_VersionCode = row["TRV_VersionCode"].ToString(),
                    TRV_VersionName = row["TRV_VersionName"].ToString(),
                    PO_Code = row["PO_Code"].ToString(),
                    FC_BCSOrderID = row["FC_BCSOrderID"] is DBNull ? -1 : Convert.ToInt64(row["FC_BCSOrderID"])
                });
            }
            return fcls;
        }

        /// <summary>
        /// 根据料品编码，搜索相关的流转卡列表
        /// </summary>
        /// <param name="_itemCode"></param>
        /// <returns></returns>
        public static List<FlowCardLists> FetchFC_InfoByItemCode(string _itemCode)
        {
            DataSet ds = new DataSet();
            List<FlowCardLists> fcls = new List<FlowCardLists>();

            string SQl = string.Format(@"Select A.[ID],A.[FC_CardType],[FC_SourceOrderID],A.[FC_Code],A.[FC_ItemID],A.[FC_ItemTechVersionID],A.[FC_Amount],A.[FC_WorkCenter],A.[FC_CardState],A.[FC_DistriSourceCard],A.[FC_FlowNum],A.[FC_CreateBy],A.[FC_CreateTime],A.[FC_CheckBy],A.[FC_CheckTime],A.[FC_BCSOrderID],B.[PO_ItemCode],B.[PO_ItemName],B.[PO_ItemSpec],B.[PO_Code],C.[TRV_VersionCode],C.[TRV_VersionName],D.[WC_Department_Name] from [FlowCard] A left join [ProduceOrder] B on A.[FC_SourceOrderID]=B.[PO_ID] left join [TechRouteVersion] C on A.[FC_ItemTechVersionID]=C.[ID] left join [WorkCenter] D on A.[FC_WorkCenter]=D.[WC_Department_ID] where B.[PO_ItemCode] ='{0}' and A.[FC_CardState] not in (3)", _itemCode);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCard");
            MyDBController.CloseConnection();

            foreach (DataRow row in ds.Tables["FlowCard"].Rows)
            {
                fcls.Add(new FlowCardLists()
                {
                    FC_Amount = Convert.ToInt32(row["FC_Amount"]),
                    FC_CardState = Convert.ToInt32(row["FC_CardState"]),
                    FC_CardType = Convert.ToInt32(row["FC_CardType"]),
                    FC_ItemID = Convert.ToInt64(row["FC_ItemID"]),
                    FC_FlowNum = Convert.ToInt32(row["FC_FlowNum"]),
                    FC_DistriSourceCard = Convert.ToInt64(row["FC_DistriSourceCard"]),
                    FC_ItemTechVersionID = Convert.ToInt64(row["FC_ItemTechVersionID"]),
                    FC_SourceOrderID = Convert.ToInt64(row["FC_SourceOrderID"]),
                    FC_WorkCenter = Convert.ToInt64(row["FC_WorkCenter"]),
                    FC_CheckBy = row["FC_CheckBy"].ToString(),
                    FC_CheckTime = Convert.ToDateTime(row["FC_CheckTime"]),
                    FC_Code = row["FC_Code"].ToString(),
                    FC_CreateBy = row["FC_CreateBy"].ToString(),
                    FC_CreateTime = Convert.ToDateTime(row["FC_CreateTime"]),
                    ID = Convert.ToInt64(row["ID"]),
                    PO_ItemCode = row["PO_ItemCode"].ToString(),
                    PO_ItemName = row["PO_ItemName"].ToString(),
                    PO_ItemSpec = row["PO_ItemSpec"].ToString(),
                    WC_Department_Name = row["WC_Department_Name"].ToString(),
                    TRV_VersionCode = row["TRV_VersionCode"].ToString(),
                    TRV_VersionName = row["TRV_VersionName"].ToString(),
                    PO_Code = row["PO_Code"].ToString(),
                    FC_BCSOrderID = row["FC_BCSOrderID"] is DBNull ? -1 : Convert.ToInt64(row["FC_BCSOrderID"])
                });
            }

            fcls = fcls.Distinct(new ListComparer<FlowCardLists>((x, y) => x.FC_Code.Equals(y.FC_Code))).ToList();

            return fcls;
        }

        /// <summary>
        /// 根据生产订单编码，搜索相关流转卡列表
        /// </summary>
        /// <param name="_orderCode"></param>
        /// <returns></returns>
        public static List<FlowCardLists> FetchFC_InfoByOrderCode(string _orderCode)
        {
            DataSet ds = new DataSet();
            string SQl = string.Format(@"Select A.[ID],A.[FC_CardType],[FC_SourceOrderID],A.[FC_Code],A.[FC_ItemID],A.[FC_ItemTechVersionID],A.[FC_Amount],A.[FC_WorkCenter],A.[FC_CardState],A.[FC_DistriSourceCard],A.[FC_FlowNum],A.[FC_CreateBy],A.[FC_CreateTime],A.[FC_CheckBy],A.[FC_CheckTime],A.[FC_BCSOrderID],B.[PO_ItemCode],B.[PO_ItemName],B.[PO_ItemSpec],B.[PO_Code],C.[TRV_VersionCode],C.[TRV_VersionName],D.[WC_Department_Name] from [FlowCard] A left join [ProduceOrder] B on A.[FC_SourceOrderID]=B.[PO_ID] left join [TechRouteVersion] C on A.[FC_ItemTechVersionID]=C.[ID] left join [WorkCenter] D on A.[FC_WorkCenter]=D.[WC_Department_ID] where B.[PO_Code] ='{0}' and A.[FC_CardState] not in (3)", _orderCode);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCard");
            MyDBController.CloseConnection();

            List<FlowCardLists> fcls = new List<FlowCardLists>();
            foreach (DataRow row in ds.Tables["FlowCard"].Rows)
            {
                fcls.Add(new FlowCardLists()
                {
                    FC_Amount = Convert.ToInt32(row["FC_Amount"]),
                    FC_CardState = Convert.ToInt32(row["FC_CardState"]),
                    FC_CardType = Convert.ToInt32(row["FC_CardType"]),
                    FC_ItemID = Convert.ToInt64(row["FC_ItemID"]),
                    FC_FlowNum = Convert.ToInt32(row["FC_FlowNum"]),
                    FC_DistriSourceCard = Convert.ToInt64(row["FC_DistriSourceCard"]),
                    FC_ItemTechVersionID = Convert.ToInt64(row["FC_ItemTechVersionID"]),
                    FC_SourceOrderID = Convert.ToInt64(row["FC_SourceOrderID"]),
                    FC_WorkCenter = Convert.ToInt64(row["FC_WorkCenter"]),
                    FC_CheckBy = row["FC_CheckBy"].ToString(),
                    FC_CheckTime = Convert.ToDateTime(row["FC_CheckTime"]),
                    FC_Code = row["FC_Code"].ToString(),
                    FC_CreateBy = row["FC_CreateBy"].ToString(),
                    FC_CreateTime = Convert.ToDateTime(row["FC_CreateTime"]),
                    ID = Convert.ToInt64(row["ID"]),
                    PO_ItemCode = row["PO_ItemCode"].ToString(),
                    PO_ItemName = row["PO_ItemName"].ToString(),
                    PO_ItemSpec = row["PO_ItemSpec"].ToString(),
                    WC_Department_Name = row["WC_Department_Name"].ToString(),
                    TRV_VersionCode = row["TRV_VersionCode"].ToString(),
                    TRV_VersionName = row["TRV_VersionName"].ToString(),
                    PO_Code = row["PO_Code"].ToString(),
                    FC_BCSOrderID = row["FC_BCSOrderID"] is DBNull ? -1 : Convert.ToInt64(row["FC_BCSOrderID"])
                });
            }
            fcls = fcls.Distinct(new ListComparer<FlowCardLists>((x, y) => x.FC_Code.Equals(y.FC_Code))).ToList();
            return fcls;
        }

        /// <summary>
        /// 用flowcard的列表更新数据库，不涉及到修改数据的
        /// 一般用来修改如流转卡状态等字段
        /// </summary>
        /// <param name="_fcList"></param>
        public static void UpdateFCInfo(List<FlowCardLists> _fcList)
        {
            string SQl = string.Format(@"select top 0 * from [FlowCard]");
            int updateNum, insertNum;
            DataSet ds = new DataSet();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCard");
            MyDBController.AutoUpdateInsert(ds.Tables["FlowCard"], _fcList, out updateNum, out insertNum);
            MyDBController.CloseConnection();
        }

        /// <summary>
        /// 修改流转卡派工数量，同时修改来源生产订单的数量信息
        /// </summary>
        /// <param name="_fc"></param>
        public static bool UpdateFCAmount(FlowCardLists _newFC, FlowCardLists _oldFC)
        {
            bool flag = true;
            int diff = _newFC.FC_Amount - _oldFC.FC_Amount;
            string SQl = "";
            if (_newFC.FC_BCSOrderID != -1)
            {
                SQl = string.Format("select [PO_OrderAmount]-[PO_StartAmount] from [ProduceOrder] where [ID]={0}", _newFC.FC_BCSOrderID);
            }
            else
            {
                SQl = string.Format("select [PO_OrderAmount]-[PO_StartAmount] from [ProduceOrder] where [PO_ID]={0}", _newFC.FC_SourceOrderID);
            }
            MyDBController.GetConnection();

            int restAmount = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));
            if (restAmount >= diff)
            {
                SQl = string.Format("update [flowcard] set [FC_Amount]={0} where [ID]={1} \r\n", _newFC.FC_Amount, _newFC.ID);
                SQl += string.Format("update [produceorder] set [PO_StartAmount]=[PO_StartAmount]+{0} where [PO_ID]={2} \r\n", diff, _newFC.FC_BCSOrderID, _newFC.FC_SourceOrderID);
                SQl += string.Format("update [FlowCardSub] set [FCS_BeginAmount]=[FCS_BeginAmount]+{0} where [FCS_FlowCradID]={1}", diff, _newFC.ID);
                int count = MyDBController.ExecuteNonQuery(SQl);
                if (count > 0)
                {
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(string.Format("订单：{0} \r\n剩余可派工数量为：{1}\r\n新的派工数量增加了：{2}\r\n请检查！", _newFC.PO_Code, restAmount, diff), "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                flag = false;
            }
            MyDBController.CloseConnection();
            return flag;
        }

        /// <summary>
        /// 完工之后返填u9数据
        /// </summary>
        public static bool CreateU9POFinishReprot(int qualifiedAmount, Int64 orderID)
        {
            string message = "";
            bool flag = false;
            DataSet ds;

            //格式化DS
            FormFinishDS(qualifiedAmount, orderID, out ds);

            Task t1 = new Task(() => { WaitForCreating(ds, out message, out flag); }) { };
            t1.Start();
            while (!t1.IsCompleted)
            {

            }
            if (flag)
            {
                #region U9系统中自动审核完工报告，逻辑更改之后不需要了。代码保留
                //if (System.Windows.MessageBox.Show("成功生成完工报告" + message, "提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information) == MessageBoxResult.OK)
                //{
                //    DataSet ds1;
                //    //格式化ds
                //    FormCheckDS(out ds1, message);
                //    Task t2 = new Task(() => { WaitForChecking(ds1, out message); });
                //    t2.Start();
                //    while (!t2.IsCompleted)
                //    {

                //    }
                //   System.Windows.MessageBox.Show(message, "提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                //}
                #endregion


                System.Windows.MessageBox.Show("成功生成完工报告" + message, "提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                return true;
            }
            else
            {
                System.Windows.MessageBox.Show(message, "提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// 格式化调用U9完工报告生成接口需要的DS
        /// </summary>
        /// <param name="qualifiedAmount">完工数量</param>
        /// <param name="orderID">生产订单id</param>
        /// <param name="ds"></param>
        private static void FormFinishDS(int qualifiedAmount, Int64 orderID, out DataSet ds)
        {
            ds = new DataSet();
            DataTable dt1 = new DataTable(), dt2 = new DataTable();
            dt1.Columns.Add("serverAddress", typeof(string));
            dt1.Columns.Add("binding", typeof(string));
            dt1.Columns.Add("OrgID", typeof(Int64));
            dt1.Columns.Add("UserID", typeof(Int64));
            dt1.Columns.Add("CultureName", typeof(string));
            dt1.Columns.Add("DefaultCultureName", typeof(string));
            dt1.Columns.Add("EnterpriseID", typeof(string));
            dt1.Columns.Add("UserCode", typeof(string));
            dt1.Columns.Add("UserName", typeof(string));
            DataRow row = dt1.NewRow();
            row["serverAddress"] = "http://" + User_Info.U9Server + "/services/UFIDA.U9.ISV.MO.ICreateCompRptSrv.svc";
            row["binding"] = "BasicHttpBinding_UFIDA.U9.ISV.MO.ICreateCompRptSrv";
            row["OrgID"] = User_Info.Org_Id;
            row["UserID"] = User_Info.U9User_ID;
            row["CultureName"] = "zh_CN";
            row["DefaultCultureName"] = "zh_CN";
            row["EnterpriseID"] = User_Info.EnterpriseCode;
            row["UserCode"] = User_Info.User_Code;
            row["UserName"] = User_Info.User_Name;
            dt1.Rows.Add(row);


            dt2.Columns.Add("m_completeDocType.m_code", typeof(string));
            dt2.Columns.Add("m_direction", typeof(int));
            dt2.Columns.Add("m_mO.m_code", typeof(string));
            dt2.Columns.Add("m_completeDate", typeof(DateTime));
            dt2.Columns.Add("m_eligibleQty", typeof(int));
            dt2.Columns.Add("m_reworkingQty", typeof(int));
            dt2.Columns.Add("m_scrapQty", typeof(int));
            dt2.Columns.Add("m_actualRcvTime", typeof(DateTime));

            DataRow row1 = dt2.NewRow();
            row1["m_completeDocType.m_code"] = User_Info.POType;
            row1["m_direction"] = 0;
            row1["m_completeDate"] = DateTime.Now;
            row1["m_actualRcvTime"] = DateTime.Now;
            row1["m_eligibleQty"] = qualifiedAmount;
            row1["m_reworkingQty"] = 0;
            row1["m_scrapQty"] = 0;
            #region 报废数量、待处理数量都不填。
            //row1["m_scrapQty"] = fcsls.Distinct(new ListComparer<FlowCardSubLists>((x, y) => (x.FCS_TechRouteID.Equals(y.FCS_TechRouteID)))).Sum(p => p.FCS_ScrappedAmount);
            //row1["m_eligibleQty"] = 0;
            //row1["m_reworkingQty"] = fcsls.Distinct(new ListComparer<FlowCardSubLists>((x, y) => (x.FCS_TechRouteID.Equals(y.FCS_TechRouteID)))).Sum(p => p.FCS_UnprocessedAm);
            #endregion
            if (orderID < 0)
            {
                row1["m_mO.m_code"] = "";
            }
            else
            {
                string SQl = string.Format(@"select [PO_Code] from [ProduceOrder] where [PO_ID]={0}", orderID);
                MyDBController.GetConnection();
                row1["m_mO.m_code"] = MyDBController.ExecuteScalar(SQl).ToString();
            }
            MyDBController.CloseConnection();
            dt2.Rows.Add(row1);
            ds.Tables.Add(dt1);
            ds.Tables.Add(dt2);
        }

        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="ds"></param>
        private static void FormCheckDS(out DataSet ds, string message)
        {
            ds = new DataSet();
            DataTable dt = new DataTable();
            for (int i = 0; i < 10; i++)
            {
                dt.Columns.Add();
            }
            DataRow dr1 = dt.NewRow();
            dr1[0] = "http://" + "172.16.100.21" + "/U9/services/UFIDA.U9.ISV.MO.IApproveCompleteRpt4ExternalSrv.svc";
            dr1[1] = "BasicHttpBinding_UFIDA.U9.ISV.MO.IApproveCompleteRpt4ExternalSrv";
            dr1[2] = User_Info.Org_Id.ToString().Trim();  //1001309061215398		1001307169198957
            dr1[3] = PersonLists.GetU9Id(User_Info.User_Code).ToString();
            dr1[4] = "zh-CN";
            dr1[5] = "zh-CN";
            dr1[6] = User_Info.EnterpriseCode;  //0719  0801
            dr1[7] = User_Info.User_Code;
            dr1[8] = User_Info.User_Name;
            dr1[9] = message;  //WG-700150804-0005    WG-201150804-0023  WG-201150804-0014
            dt.Rows.Add(dr1);
            ds.Tables.Add(dt);
        }

        /// <summary>
        /// 等待系统创建u9完工报告
        /// </summary>
        /// <param name="_ds"></param>
        /// <param name="_str"></param>
        /// <param name="_flag"></param>
        private static void WaitForCreating(DataSet _ds, out string _str, out bool _flag)
        {
            string str = "";
            bool flag = false;
            Task t1 = new Task(() => { UpdateU9POInfo(_ds, out str, out flag); });
            t1.Start();
            Thread t3 = new Thread(() => { Loading_Window lw = new Loading_Window("生成U9完工报告中，请稍等...") { }; lw.ShowDialog(); }) { };
            t3.SetApartmentState(ApartmentState.STA);
            t3.Start();
            while (!t1.IsCompleted)
            {

            }
            if (!flag)
            {
                Task t2 = new Task(() => { UpdateU9POInfo(_ds, out str, out flag); });
                t2.Start();
                while (!t2.IsCompleted)
                {

                }
                t3.Abort();
            }
            else
            {
                t3.Abort();
            }
            _str = str;
            _flag = flag;
        }

        /// <summary>
        /// 等待系统审核U9完工报告
        /// </summary>
        private static void WaitForChecking(DataSet _ds, out string _str)
        {
            string str = "";
            bool flag = false;
            Task t1 = new Task(() => { CheckU9POInfo(_ds, out str, out flag); });
            t1.Start();
            Thread t3 = new Thread(() => { Loading_Window lw = new Loading_Window("审核U9完工报告中，请稍等...") { }; lw.ShowDialog(); }) { };
            t3.SetApartmentState(ApartmentState.STA);
            t3.Start();
            while (!t1.IsCompleted)
            {

            }
            if (!flag)
            {
                Task t2 = new Task(() => { CheckU9POInfo(_ds, out str, out flag); });
                t2.Start();
                while (!t2.IsCompleted)
                {

                }
                t3.Abort();
            }
            else
            {
                t3.Abort();
            }
            //t3.Abort();
            _str = str;
        }


        /// <summary>
        /// 调用U9完工报告生成接口
        /// </summary>
        /// <param name="_ds"></param>
        /// <param name="_str"></param>
        /// <param name="_flag"></param>
        private static void UpdateU9POInfo(DataSet _ds, out string _str, out bool _flag)
        {
            _flag = ProduceOrderLists.UpdateU9POInfo(_ds, out _str);
        }

        /// <summary>
        /// 调用U9完工报告审核接口
        /// </summary>
        /// <param name="_ds"></param>
        /// <param name="_str"></param>
        private static void CheckU9POInfo(DataSet _ds, out string _str, out bool _flag)
        {
            _flag = ProduceOrderLists.CheckU9POInfo(_ds, out _str);
        }

    }
}
