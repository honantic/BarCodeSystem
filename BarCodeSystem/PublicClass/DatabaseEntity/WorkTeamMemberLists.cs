using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem.PublicClass.DatabaseEntity
{
    public class WorkTeamMemberLists
    {
        public Int64 ID { get; set; }

        public Int64 WTM_WorkTeamID { get; set; }

        public Int64 WTM_MemberPersonID { get; set; }

        public int WTM_SortOrder { get; set; }

        public string WT_ReservedSegment { get; set; }
    }
}
