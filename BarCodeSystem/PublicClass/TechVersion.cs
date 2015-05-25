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
        /// 料品的工艺路线版本中文描述
        /// </summary>
        public string TRV_Version
        {
            get;
            set;
        }

        /// <summary>
        /// 料品工艺路线版本ID
        /// </summary>
        public Int64 TR_VersionID
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
    }
}
