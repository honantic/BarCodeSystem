using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem.PublicClass
{
    public class QualitySortList
    {
        /// <summary>
        /// 是否被选中
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
        /// 质检分类编码
        /// </summary>
        public string QS_Code
        {
            get;
            set;
        }

        /// <summary>
        /// 质检分类名称
        /// </summary>
        public string QS_Name
        {
            get;
            set;
        }

    }
}
