using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// 单据状态（0:开立，1:报工，2:完工）
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
        public DateTime? FC_CheckTime { get; set; }
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
    }
}
