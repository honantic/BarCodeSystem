using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem
{
    public class TechRouteLists
    {

        public TechRouteLists()
        {
            ID = 0;
            TR_ItemID = 0;
            TRV_Version = "";
            II_Name = "";
            TR_ProcessSequence = 0;
            TR_ProcessName = "";
            TR_WagePerPiece = 0;
            TR_WorkHour = 0;
            TR_WorkCenterID = 0;
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
        /// 料品ID
        /// </summary>
        public Int64 TR_ItemID
        {
            get;
            set;
        }

        /// <summary>
        /// 料品编码
        /// </summary>
        public string TR_ItemCode
        {
            get;
            set;
        }

        /// <summary>
        /// 料品名称，工艺路线表里面没有这个字段，从料品表中取出
        /// </summary>
        public string II_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 工艺路线版本ID
        /// </summary>
        public Int64 TR_VersionID
        {
            get;
            set;
        }

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        public string TRV_Version
        {
            get;
            set;
        }

        /// <summary>
        /// 工艺序号
        /// </summary>
        public int TR_ProcessSequence
        {
            get;
            set;
        }

        /// <summary>
        /// 工艺名称
        /// </summary>
        public string TR_ProcessName
        {
            get;
            set;
        }

        /// <summary>
        /// 工艺编码
        /// </summary>
        public string TR_ProcessCode
        {
            get;
            set;
        }

        /// <summary>
        /// 工时
        /// </summary>
        public decimal TR_WorkHour
        {
            get;
            set;
        }

        /// <summary>
        /// 计件工资分配方式， 0:独立、1、平分、2、按合作人数分配配额公式计算
        /// </summary>
        public int TR_WageAllotScheme
        {
            get;
            set;
        }

        /// <summary>
        /// 计件工资分配方式  中文展示
        /// </summary>
        public string TR_WageAllotScheme_Show
        {
            get;
            set;
        }

        /// <summary>
        /// 工作中心ID
        /// </summary>
        public Int64 TR_WorkCenterID
        {
            get;
            set;
        }

        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string WC_Department_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 工序ID
        /// </summary>
        public Int64 TR_ProcessID
        {
            get;
            set;
        }

        private bool tr_IsReportPoint = false;
        /// <summary>
        /// 是否报工点
        /// </summary>
        public bool TR_IsReportPoint
        {
            get { return tr_IsReportPoint; }
            set { tr_IsReportPoint = value; }
        }


        public bool tr_IsExProcess = false;
        /// <summary>
        /// 是否委外工序
        /// </summary>
        public bool TR_IsExProcess
        {
            get { return tr_IsExProcess; }
            set { tr_IsExProcess = value; }
        }

        public bool tr_IsFirstProcess = false;
        /// <summary>
        /// 是否首道工序
        /// </summary>
        public bool TR_IsFirstProcess
        {
            get { return tr_IsFirstProcess; }
            set { tr_IsFirstProcess = value; }
        }

        public bool tr_IsLastProcess = false;
        /// <summary>
        /// 是否末道工序
        /// </summary>
        public bool TR_IsLastProcess
        {
            get { return tr_IsLastProcess; }
            set { tr_IsLastProcess = value; }
        }

        public bool tr_IsReportDevice = false;
        /// <summary>
        /// 是否报工设备
        /// </summary>
        public bool TR_IsReportDevice
        {
            get { return tr_IsReportDevice; }
            set { tr_IsReportDevice = value; }
        }

        public bool tr_IsDeviceCharging = false;
        /// <summary>
        /// 是否设备计费
        /// </summary>
        public bool TR_IsDeviceCharging
        {
            get { return tr_IsDeviceCharging; }
            set { tr_IsDeviceCharging = value; }
        }

        /// <summary>
        /// 标准单件工资
        /// </summary>
        public decimal TR_WagePerPiece
        {
            get;
            set;
        }

        /// <summary>
        /// 是否默认工艺路线版本
        /// </summary>
        public bool TR_IsDefaultVer
        {
            get;
            set;
        }

        public bool TR_IsBackProcess
        { get; set; }

        public bool TR_IsTestProcess
        {
            get;
            set;
        }

        public string personCode
        { get; set; }

        public string personName
        { get; set; }

        ///// <summary>
        ///// 工艺路线派工的时候，改道工序的操作人员列表
        ///// </summary>
        //public List<PersonLists> personList { get; set; }
    }
}
