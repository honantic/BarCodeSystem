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
    }
}
