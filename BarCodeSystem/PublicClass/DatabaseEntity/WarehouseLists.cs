using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem
{
    public class WarehouseLists
    {

        private bool isselected = false;
        public bool IsSelected
        {
            get { return isselected; }
            set { isselected = value; }
        }

        public string W_Code
        {
            get;
            set;
        }

        public Int64 W_ID
        {
            get;
            set;
        }

        public string W_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 仓库信息录入来源，数据库中字段类型为int
        /// 0：U9导入，1：手工录入.这里为了方便展示，转换为字符型。这里是用来展示的属性
        /// </summary>
        public string W_SourceType_Show
        {
            get;
            set;
        }

        /// <summary>
        /// 仓库是否启用，数据库中字段类型为bool
        /// 0：否，1：是.这里为了方便展示，转换为字符型。这里是用来展示的属性
        /// </summary>
        public string W_IsValidated_Show
        {
            get;
            set;
        }


        /// <summary>
        /// 仓库信息录入来源，数据库中字段类型为int
        /// 0：U9导入，1：手工录入.这里是用来存入数据库的属性
        /// </summary>
        public int W_SourceType
        {
            get;
            set;
        }

        /// <summary>
        /// 仓库是否启用，数据库中字段类型为bool
        /// 0：否，1：是.这里是用来存入数据库的属性
        /// </summary>
        public bool W_IsValidated
        {
            get;
            set;
        }
    }
}
