using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem
{
    public class QualityIssuesLists
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get;
            set;
        }

        /// <summary>
        /// 质量问题信息编码
        /// </summary>
        public string QI_Code
        {
            get;
            set;
        }

        /// <summary>
        /// 质量问题信息Name
        /// </summary>
        public string QI_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 质量问题信息条码
        /// </summary>
        public string QI_BarCode
        {
            get;
            set;
        }

        ///// <summary>
        ///// 是否材料问题，数据库中该字段为bool型，默认值为false
        ///// 这里为了展示方便，转换为字符型，转换关系 false:否  true:是
        ///// </summary>
        //public string QI_IsItemIssue_Show
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 是否材料问题，数据库中该字段为bool型，默认值为false
        ///// 该属性用来对数据库进行操作
        ///// </summary>
        //public bool QI_IsItemIssue
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 是否加工问题，数据库中该字段为bool型，默认值为false
        ///// 这里为了展示方便，转换为字符型，转换关系 false:否  true:是
        ///// </summary>
        //public string QI_IsProduceIssue_Show
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 是否加工问题，数据库中该字段为bool型，默认值为false
        ///// 该属性用来对数据库进行操作
        ///// </summary>
        //public bool QI_IsProduceIssue
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 是否上道工序问题，数据库中该字段为bool型，默认值为false
        ///// 这里为了展示方便，转换为字符型，转换关系 false:否  true:是
        ///// </summary>
        //public string QI_IsPreviousIssue_Show
        //{
        //    get;
        //    set;
        //}
                
        ///// <summary>
        ///// 是否上道工序问题，数据库中该字段为bool型，默认值为false
        ///// 该属性用来对数据库进行操作
        ///// </summary>
        //public bool QI_IsPreviousIssue
        //{
        //    get;
        //    set;
        //}
    }
}
