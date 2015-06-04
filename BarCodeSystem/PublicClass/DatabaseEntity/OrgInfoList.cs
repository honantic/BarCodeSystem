using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem.PublicClass
{
    public class OrgInfoList
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
        /// 组织ID
        /// </summary>
        public Int64 OI_ID
        {
            get;
            set;
        }

        /// <summary>
        /// 组织编码
        /// </summary>
        public string OI_Code
        {
            get;
            set;
        }
        /// <summary>
        /// 组织名称
        /// </summary>
        public string OI_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string OI_Remark
        {
            get;
            set;
        }
    }
}
