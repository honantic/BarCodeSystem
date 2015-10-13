using BarCodeSystem.PublicClass.HelperClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCodeSystem.PublicClass.DatabaseEntity
{
    public class ScrapReportList
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }

        /// <summary>
        /// 条码系统组织id
        /// </summary>
        public Int64 SR_OrgID
        {
            get;
            set;
        }


        /// <summary>
        /// 流转卡编号
        /// </summary>
        public string SR_Code
        {
            get;
            set;
        }

        /// <summary>
        /// 单据类型
        /// </summary>
        public int SR_Type
        {
            get;
            set;
        }

        /// <summary>
        /// 缴废数量=流转卡各个工序报废数量之和
        /// </summary>
        public int SR_ScrapAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 审核数量，默认等于缴费数量。审核时候修改
        /// </summary>
        public int SR_CheckAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime SR_EntranceTime
        {
            get;
            set;
        }

        /// <summary>
        /// 条码系统仓库ID
        /// </summary>
        public Int64 SR_WareHouseID
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string SR_CreateBy
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime SR_CreateOn
        {
            get;
            set;
        }

        /// <summary>
        /// 审核人姓名
        /// </summary>
        public string SR_CheckBy
        {
            get;
            set;
        }

        /// <summary>
        /// 审核日期
        /// </summary>
        public DateTime SR_CheckOn
        {
            get;
            set;
        }

        /// <summary>
        /// 流转卡ID
        /// </summary>
        public Int64 SR_FlowCardID
        {
            get;
            set;
        }

        /// <summary>
        /// 流转卡编号
        /// </summary>
        public string SR_FlowCardCode
        {
            get;
            set;
        }

        /// <summary>
        /// 料品ID
        /// </summary>
        public Int64 SR_ItemID
        {
            get;
            set;
        }

        /// <summary>
        /// 车间ID
        /// </summary>
        public Int64 SR_WorkCenterID
        {
            get;
            set;
        }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string SR_WorkCenterName
        {
            get;
            set;
        }

        /// <summary>
        /// 单据单据状态0:未审核 1：已审核
        /// </summary>
        public int SR_State
        {
            get;
            set;
        }
        /// <summary>
        /// 料品名称
        /// </summary>
        public string SR_ItemName
        {
            get;
            set;
        }

        /// <summary>
        /// 料品名称
        /// </summary>
        public string SR_ItemCode
        {
            get;
            set;
        }

        /// <summary>
        /// 料品名称
        /// </summary>
        public string SR_ItemSpec
        {
            get;
            set;
        }

        /// <summary>
        /// 流转卡报工完成的时候自动生成相应的缴废单
        /// 满足条件：流转卡报工完成，且有报废记录生成
        /// </summary>
        public static void GenerateNewSR(FlowCardLists _fcl, List<FlowCardSubLists> _fcslList)
        {
            string SQl = "Select top 0 * from [ScrapReport]";
            DataSet ds = new DataSet();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "ScrapReport");

            ScrapReportList srl = new ScrapReportList();
            List<ScrapReportList> srls = new List<ScrapReportList>();

            List<FlowCardSubLists> fcslList = _fcslList.Distinct(new ListComparer<FlowCardSubLists>((x, y) => (x.FCS_TechRouteID.Equals(y.FCS_TechRouteID)))).ToList();

            srl.SR_ScrapAmount = fcslList.Sum(p => p.FCS_ScrappedAmount);
            srl.SR_FlowCardID = _fcl.ID;
            srl.SR_FlowCardCode = _fcl.FC_Code;
            srl.SR_ItemID = _fcl.FC_ItemID;
            srl.SR_ItemName = _fcl.PO_ItemName;
            srl.SR_ItemSpec = _fcl.PO_ItemSpec;
            srl.SR_ItemCode = _fcl.PO_ItemCode;
            srl.SR_State = 0;
            srl.SR_CreateBy = User_Info.User_Name;
            srl.SR_CreateOn = DateTime.Now;
            srl.SR_EntranceTime = srl.SR_CheckOn = DateTime.Now;
            srl.SR_WorkCenterID = _fcl.FC_WorkCenter;
            srl.SR_WorkCenterName = _fcl.WC_Department_Name;
            srl.SR_OrgID = User_Info.Org_Id;

            srls.Add(srl);

            int updateNum, insertNum;
            MyDBController.AutoUpdateInsert(ds.Tables["ScrapReport"], srls, out updateNum, out insertNum);
        }
    }
}
