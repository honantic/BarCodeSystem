using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem
{
    public class ProcessNameLists
    {
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get;
            set;
        }


        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string PN_Code
        {
            get;
            set;
        }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string PN_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 序在车间里面的代码，用来打印流转卡的
        /// </summary>
        public string PN_CodeInWorkCenter
        {
            get;
            set;
        }

        /// <summary>
        /// 工序所属工作中心id
        /// </summary>
        public Int64 PN_WorkCenterID
        {
            get;
            set;
        }
    }
}
