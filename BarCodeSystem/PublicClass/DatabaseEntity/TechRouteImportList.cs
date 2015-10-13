using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem.PublicClass.DatabaseEntity
{
    public class TechRouteImportList
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int Line_Number
        {
            get;
            set;
        }
        /// <summary>
        /// 料品编码
        /// </summary>
        public string II_Code
        {
            get;
            set;
        }
        /// <summary>
        /// 料品名称
        /// </summary>
        public string II_Name
        {
            get;
            set;
        }
        /// <summary>
        /// 料品规格
        /// </summary>
        public string II_Spec
        {
            get;
            set;
        }
        /// <summary>
        /// 料品型号
        /// </summary>
        public string II_Version
        {
            get;
            set;

        }
        /// <summary>
        /// 工艺路线版本
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
        /// 是否默认版本
        /// </summary>
        public string TRV_IsDefaultVer
        {
            get;
            set;
        }
        /// <summary>
        /// 报工方式:0：流水线， 1:离散
        /// </summary>
        public string TRV_ReportWay
        {
            get;
            set;
        }
        /// <summary>
        /// 是否返工版本
        /// </summary>
        public string TRV_IsBackVersion
        {
            get;
            set;
        }
        /// <summary>
        /// 车间编码
        /// </summary>
        public string WC_Department_Code
        {
            get;
            set;
        }
        /// <summary>
        /// 车间名称
        /// </summary>
        public string WC_Department_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 工序号
        /// </summary>
        public int TR_ProcessSequence
        {
            get;
            set;
        }
        /// <summary>
        /// 工序编码
        /// </summary>
        public string TR_ProcessCode
        {
            get;
            set;
        }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string TR_ProcessName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否测试工序
        /// </summary>
        public string TR_IsTestProcess
        {
            get;
            set;
        }

        /// <summary>
        /// 默认检验员
        /// </summary>
        public string TR_DefaultCheckPersonName
        {
            get;
            set;
        }
        /// <summary>
        /// 绑定工序
        /// </summary>
        public int? TR_BindingProcess
        {
            get;
            set;
        }

        /// <summary>
        /// 工时
        /// </summary>
        public decimal WH_WorkHour
        {
            get;
            set;
        }

        /// <summary>
        /// 错误备注
        /// </summary>
        public string Error_Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string WH_StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string WH_EndDate { get; set; }
    }
}
