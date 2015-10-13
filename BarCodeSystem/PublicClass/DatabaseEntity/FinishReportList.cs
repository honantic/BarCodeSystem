using BarCodeSystem.PublicClass.HelperClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BarCodeSystem.PublicClass.DatabaseEntity
{
    public class FinishReportList
    {
        /// <summary>
        /// id
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }

        /// <summary>
        /// 完工报告编号
        /// </summary>
        public string FR_Code
        {
            get;
            set;
        }

        /// <summary>
        /// 生产订单ID
        /// </summary>
        public Int64 FR_ProduceOrderID
        {
            get;
            set;
        }

        /// <summary>
        /// 生产定单编号
        /// </summary>
        public string FR_ProduceOrderCode
        {
            get;
            set;
        }
        /// <summary>
        /// 流转卡ID
        /// </summary>
        public Int64 FR_FlowCardID
        {
            get;
            set;
        }

        /// <summary>
        /// 流转卡编号，来自于FlowCard表
        /// </summary>
        public string FR_FlowCardCode
        {
            get;
            set;
        }

        /// <summary>
        /// 料品ID
        /// </summary>
        public Int64 FR_ItemID
        {
            get;
            set;
        }

        /// <summary>
        /// 料品名称
        /// </summary>
        public string FR_ItemName
        {
            get;
            set;
        }

        /// <summary>
        /// 料品编号
        /// </summary>
        public string FR_ItemCode
        {
            get;
            set;
        }

        /// <summary>
        /// 料品规格
        /// </summary>
        public string FR_ItemSpec
        {
            get;
            set;
        }
        /// <summary>
        /// 项目编号
        /// </summary>
        public string FR_ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 收发类型，在U9中是固定的枚举值
        /// </summary>
        public int FR_OperateType
        {
            get;
            set;
        }

        /// <summary>
        /// 车间ID
        /// </summary>
        public Int64 FR_WorkCenterID
        {
            get;
            set;
        }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string FR_WorkCenterName
        {
            get;
            set;
        }
        /// <summary>
        /// 完工日期
        /// </summary>
        public DateTime FR_FinishTime
        {
            get;
            set;
        }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public Int64 FR_WarehouseID
        {
            get;
            set;
        }

        /// <summary>
        /// 合格数量
        /// </summary>
        public int FR_QualifiedAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 报废数量
        /// </summary>
        public int FR_ScrappedAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 默认入库数量=合格数量，在审核的时候填写
        /// </summary>
        public int FR_StockAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 单据状态 0:未审核 1:已审核
        /// </summary>
        public int FR_State
        {
            get;
            set;
        }

        /// <summary>
        /// 编制日期
        /// </summary>
        public DateTime FR_CreateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 编制人员姓名
        /// </summary>
        public string FR_CreateBy
        {
            get;
            set;
        }

        /// <summary>
        /// 审核日期、
        /// </summary>
        public DateTime FR_CheckTime
        {
            get;
            set;
        }

        /// <summary>
        /// 审核人姓名
        /// </summary>
        public string FR_CheckBy
        {
            get;
            set;
        }

        /// <summary>
        /// 报工完成的时候，生成相应的完工报告的操作。
        /// 需要满足条件：当前流转卡最后一道工序为选定的料品工艺路线的最后一道工序
        /// </summary>
        public static void GenerateNewFR(FlowCardLists _fcl, List<FlowCardSubLists> _fcslList)
        {
            string SQl = "Select top 0 * from [FinishReport]";
            int flowcardState = 2;//流转卡状态：完工已入库
            DataSet ds = new DataSet();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FinishReport");

            SQl = string.Format(@"update [FlowCard] set [FC_CardState]={0} where [FC_Code]='{1}'", flowcardState, _fcl.FC_Code);
            MyDBController.ExecuteNonQuery(SQl);

            FinishReportList frl = new FinishReportList();
            List<FinishReportList> frls = new List<FinishReportList>();

            frl.FR_ProduceOrderID = _fcl.FC_SourceOrderID;
            frl.FR_ProduceOrderCode = _fcl.PO_Code;
            frl.FR_FlowCardID = _fcl.ID;
            frl.FR_FlowCardCode = _fcl.FC_Code;
            frl.FR_ItemID = _fcl.FC_ItemID;
            frl.FR_ItemName = _fcl.PO_ItemName;
            frl.FR_ItemCode = _fcl.PO_ItemCode;
            frl.FR_ItemSpec = _fcl.PO_ItemSpec;
            frl.FR_WorkCenterID = _fcl.FC_WorkCenter;
            frl.FR_WorkCenterName = _fcl.WC_Department_Name;
            frl.FR_WarehouseID = -1;
            frl.FR_State = 0;
            frl.FR_FinishTime = DateTime.Now;
            frl.FR_CreateTime = DateTime.Now;
            frl.FR_CheckTime = DateTime.Now;
            frl.FR_CreateBy = User_Info.User_Name;
            frl.FR_StockAmount = frl.FR_QualifiedAmount = _fcslList.Distinct(new ListComparer<FlowCardSubLists>((x, y) => (x.FCS_TechRouteID.Equals(y.FCS_TechRouteID)))).LastOrDefault().FCS_QulifiedAmount;
            frl.FR_ScrappedAmount = _fcslList.Distinct(new ListComparer<FlowCardSubLists>((x, y) => (x.FCS_TechRouteID.Equals(y.FCS_TechRouteID)))).ToList().Sum(p => p.FCS_ScrappedAmount);

            frls.Add(frl);
            int updateNum, insertNum;
            MyDBController.AutoUpdateInsert(ds.Tables["FinishReport"], frls, out updateNum, out insertNum);
        }
    }
}
