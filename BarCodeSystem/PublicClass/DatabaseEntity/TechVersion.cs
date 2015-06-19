using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace BarCodeSystem
{
    public class TechVersion 
    {
        /// <summary>
        /// 料品的工艺路线版本中编码
        /// </summary>
        public string TRV_VersionCode
        {
            get;
            set;
        }
        /// <summary>
        /// 工艺路线版本名称
        /// </summary>
        public string TRV_VersionName
        {
            get;
            set;
        }
        /// <summary>
        /// 料品工艺路线版本ID
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }

        /// <summary>
        /// 是否默认工艺路线版本，默认false
        /// </summary>
        public bool TRV_IsDefaultVer
        {
            get;
            set;
        }
        /// <summary>
        /// 是否返工版本 0:否  1:是 默认否,新增
        /// </summary>
        public bool TRV_IsBackVersion
        {
            get;
            set;
        }
        /// <summary>
        /// 是否生效 0:否 1:是 默认否
        /// </summary>
        public bool TRV_IsValidated
        {
            get;
            set;
        }
        /// <summary>
        /// 是否特殊版本
        /// </summary>
        public bool TRV_IsSpecialVersion
        {
            get;
            set;
        }

        /// <summary>
        /// 报工方式 0：流水线报工  1：离散报工
        /// </summary>
        public int TRV_ReportWay { get; set; }
    }
}
