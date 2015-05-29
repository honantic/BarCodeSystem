using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem.PublicClass
{
    public class ProduceOrderLists
    {
        /// <summary>
        /// 条码系统里面的ID
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }
        /// <summary>
        /// U9订单Id
        /// </summary>
        public Int64 PO_ID
        {
            get;
            set;
        }
        /// <summary>
        /// U9订单编号
        /// </summary>
        public string PO_Code
        {
            get;
            set;
        }
        /// <summary>
        /// U9料品ID，条码料品表里面没有这个字段
        /// </summary>
        public string PO_ItemID
        {
            get;
            set;
        }
        /// <summary>
        /// U9料品规格
        /// </summary>
        public string PO_ItemSpec
        {
            get;
            set;
        }
        /// <summary>
        /// U9料品编码
        /// </summary>
        public string PO_ItemCode
        {
            get;
            set;
        }
        /// <summary>
        /// U9料品名称
        /// </summary>
        public string PO_ItemName
        {
            get;
            set;
        }
        /// <summary>
        /// U9料品型号
        /// </summary>
        public string PO_ItemVersion
        {
            get;
            set;
        }
        /// <summary>
        /// U9项目编号，跟组织关联
        /// </summary>
        public string PO_ProjectNum
        {
            get;
            set;
        }
        /// <summary>
        /// 条码系统里面的工作中心ID
        /// </summary>
        public Int64 PO_WorkCenter
        {
            get;
            set;
        }
        /// <summary>
        /// 料品计量单位
        /// </summary>
        public string PO_Itemunit
        {
            get;
            set;
        }
        /// <summary>
        /// 单据创建日期
        /// </summary>
        public DateTime PO_CreateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 创建人员姓名
        /// </summary>
        public string PO_CreateBy
        {
            get;
            set;
        }
        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime PO_DemandDate
        {
            get;
            set;
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime PO_ModifyTime
        {
            get;
            set;
        }
        /// <summary>
        ///修改人员姓名
        /// </summary>
        public string PO_ModifyBy
        {
            get;
            set;
        }
        /// <summary>
        /// 开工日期
        /// </summary>
        public DateTime PO_StartDate
        {
            get;
            set;
        }
        /// <summary>
        /// 订单数量
        /// </summary>
        public Int32 PO_OrderAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 开工数量
        /// </summary>
        public Int32 PO_StartAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 已完工数量
        /// </summary>
        public Int32 PO_FinishedAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 单据来源 0:来自U9 1:条码系统手工录入 
        /// </summary>
        public Int32 PO_OrderSource
        {
            get;
            set;
        }
        /// <summary>
        /// 生产部门 
        /// </summary>
        public string PO_ProduceDepart
        {
            get;
            set;
        }
        /// <summary>
        /// 是否返工订单
        /// </summary>
        public bool PO_IsReturn
        {
            get;
            set;
        }
    }
}
