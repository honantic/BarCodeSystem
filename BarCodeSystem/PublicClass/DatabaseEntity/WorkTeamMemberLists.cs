using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem.PublicClass.DatabaseEntity
{
    public class WorkTeamMemberLists
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 班组ID
        /// </summary>
        public Int64 WTM_WorkTeamID { get; set; }
        /// <summary>
        /// 班组工作中心名称
        /// </summary>
        public string WTM_WorkCenterName { get; set; }
        /// <summary>
        /// 帮组工作中心ID
        /// </summary>
        public Int64 WTM_WorkCenterID { get; set; }
        /// <summary>
        /// 人员ID
        /// </summary>
        public Int64 WTM_MemberPersonID { get; set; }
        /// <summary>
        /// 人员编号
        /// </summary>
        public string WTM_MemberPersonCode { get; set; }
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string WTM_MemberPersonName { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public int WTM_SortOrder { get; set; }
        /// <summary>
        /// 保留字段
        /// </summary>
        public string WT_ReservedSegment { get; set; }
        /// <summary>
        /// 班组名称
        /// </summary>
        public string WTM_WorkTeamName { get; set; }
        /// <summary>
        /// 班组编号
        /// </summary>
        public string WTM_WorkTeamCode { get; set; }
    }
}
