using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCodeSystem.PublicClass.DatabaseEntity
{
    public class StaffSalariesList
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
        /// 员工姓名
        /// </summary>
        public string PersonName
        {
            get;
            set;
        }
        /// <summary>
        /// 员工编码
        /// </summary>
        public string PersonCode
        {
            get;
            set;
        }

        /// <summary>
        /// 员工工资
        /// </summary>
        public decimal Salary
        {
            get;
            set;
        }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get;
            set;
        }
        /// <summary>
        /// 投入数
        /// </summary>
        public int BeginAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 合格数
        /// </summary>
        public decimal QulifiedAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 报废数
        /// </summary>
        public int ScrappedAmount
        {
            get;
            set;
        }


        /// <summary>
        /// 人员数量
        /// </summary>
        public decimal PeopleAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 流转卡号
        /// </summary>
        public string FC_Code
        {
            get;
            set;
        }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string Department_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 报工时间
        /// </summary>
        public string ReportTime
        {
            get;
            set;
        }

        /// <summary>
        /// 工时
        /// </summary>
        public decimal WorkHour
        {
            get;
            set;
        }


        /// <summary>
        /// 流转卡ID
        /// </summary>
        public Int64 FC_ID
        {
            get;
            set;

        }

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public Int64 TR_ID
        {
            get;
            set;
        }


        /// <summary>
        /// 工序ID
        /// </summary>
        public Int64 ProcessID
        {
            get;
            set;
        }

    }
}
