using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem.PublicClass.DatabaseEntity
{
    public class FlowCardSubLists
    {
        /// <summary>
        /// 主键，自增
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 主表id、
        /// </summary>
        public Int64 FCS_ItemId { get; set; }
        /// <summary>
        /// 料品id、
        /// </summary>
        public Int64 FCS_FlowCradID { get; set; }
        /// <summary>
        /// 工艺践线id、
        /// </summary>
        public Int64 FCS_TechRouteID { get; set; }
        /// <summary>
        /// 工序号id、
        /// </summary>
        public Int64 FCS_ProcessID { get; set; }
        /// <summary>
        /// 工序名称、
        /// </summary>
        public string FCS_ProcessName { get; set; }
        /// <summary>
        /// 人员编号
        /// </summary>
        public string FCS_PersonCode { get; set; }
        /// <summary>
        /// 人员姓名、
        /// </summary>
        public string FCS_PersonName { get; set; }
        /// <summary>
        /// 工序投入数量
        /// </summary>
        public int FCS_BeginAmount { get; set; }
        /// <summary>
        /// 合格数量、
        /// </summary>
        public int FCS_QulifiedAmount { get; set; }
        /// <summary>
        /// 报废数量、
        /// </summary>
        public int FCS_ScrappedAmount { get; set; }
        ///// <summary>
        ///// 责废数量
        ///// </summary>
        //public int FCS_ProcessScrap { get; set; }
        ///// <summary>
        ///// 料废数量
        ///// </summary>
        //public int FCS_ItemScrap { get; set; }
        ///// <summary>
        ///// 退回供方数量
        ///// </summary>
        //public int FCS_SendBackAmount { get; set; }
        /// <summary>
        /// 待处理数量
        /// </summary>
        public int FCS_UnprocessedAm { get; set; }
        /// <summary>
        /// 检验员ID，报工的时候填写质量信息的用户ID自动带过来
        /// </summary>
        public Int64 FCS_CheckByID { get; set; }
        /// <summary>
        /// 检验员姓名 ，报工的时候填写质量信息的用户姓名自动带过来
        /// </summary>
        public string FCS_CheckByName { get; set; }
        /// <summary>
        /// 计件数量、用来计算计件工资的，根据不同的工序类型有不同的算法，比如测试工序计件数量=投入数量，正常工序计件数量=合格数量，返工工序计件数量=0
        /// </summary>
        public int FCS_PieceAmount { get; set; }
        /// <summary>
        /// 计件除数、用来计算计件工资的，为该道工序加工的人数
        /// </summary>
        public int FCS_PieceDivNum { get; set; }
        ///// <summary>
        ///// 单件工资、(来自工艺路线表)
        ///// </summary>
        //public decimal FCS_WagePerPiece { get; set; }
        /// <summary>
        /// 是否首道工序、（来自工艺路线表）
        /// </summary>
        public bool FCS_IsFirstProcess { get; set; }
        /// <summary>
        /// 是否未道工序、（来自工艺路线表）
        /// </summary>
        public bool FCS_IsLastProcess { get; set; }
        /// <summary>
        /// 该道工序是否已经报工
        /// </summary>
        public bool FCS_IsReported { get; set; }
        /// <summary>
        /// 来自工艺路线表，工序号
        /// </summary>
        public int FCS_ProcessSequanece { get; set; }
    }
}
