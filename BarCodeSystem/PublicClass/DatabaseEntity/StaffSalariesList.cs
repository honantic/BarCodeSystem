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
        public string SS_PersonName
        {
            get;
            set;
        }
        /// <summary>
        /// 员工编码
        /// </summary>
        public string SS_PersonCode
        {
            get;
            set;
        }

        /// <summary>
        /// 员工工资
        /// </summary>
        public decimal SS_Salary
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
        public int QulifiedAmount
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
        /// 流转卡号
        /// </summary>
        public string FC_Code
        {
            get;
            set;
        }
    }
}
