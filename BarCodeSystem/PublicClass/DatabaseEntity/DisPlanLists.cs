using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem.PublicClass.DatabaseEntity
{
    public class DisPlanLists
    {
        /// <summary>
        /// 主键,自增
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 派工方案版本ID
        /// </summary>
        public Int64 DP_DisPlanVersionID { get; set; }
        /// <summary>
        /// 工艺路线ID，对应每一个工序
        /// </summary>
        public Int64 DP_TechRouteID { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string DP_ProcessName { get; set; }
        /// <summary>
        /// 工序编号
        /// </summary>
        public int DP_ProcessSequence { get; set; }
        /// <summary>
        /// 员工ID
        /// </summary>
        public Int64 DP_PersonID { get; set; }
        /// <summary>
        /// 员工名称
        /// </summary>
        public string DP_PersonName { get; set; }
        /// <summary>
        /// 员工编码
        /// </summary>
        public string DP_PersonCode { get; set; }
    }
}
