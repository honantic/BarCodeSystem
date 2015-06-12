using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem.PublicClass.DatabaseEntity
{
    public class DisPlanVersionLists
    {
        /// <summary>
        /// 主键,自增
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 料品ID
        /// </summary>
        public Int64 DPV_ItemID { get; set; }
        /// <summary>
        /// 料品名称
        /// </summary>
        public string DPV_ItemName { get; set; }
        /// <summary>
        /// 料品编码
        /// </summary>
        public string DPV_ItemCode { get; set; }
        /// <summary>
        /// 工艺路线版本ID
        /// </summary>
        public Int64 DPV_TechRouteVersionID { get; set; }
        /// <summary>
        /// 工艺路线版本名称
        /// </summary>
        public string DPV_TechRouteVersionName { get; set; }
        /// <summary>
        /// 派工方案名称
        /// </summary>
        public string DPV_VersionName { get; set; }
    }
}
