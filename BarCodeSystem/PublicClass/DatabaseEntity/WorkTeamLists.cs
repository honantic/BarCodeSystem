using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem
{
    public class WorkTeamLists
    {
        //班组编码
        public string WT_Code
        {
            get;
            set;
        }

        //班组名称
        public string WT_Name
        {
            get;
            set;
        }

        //工作中心ID
        public Int64 WT_WorkCenterID
        {
            get;
            set;
        }

        //工作中心编码
        public string workcenterCode
        {
            get;
            set;
        }

        //工作中心名称
        public string workcenterName
        {
            get;
            set;
        }
    }
}
