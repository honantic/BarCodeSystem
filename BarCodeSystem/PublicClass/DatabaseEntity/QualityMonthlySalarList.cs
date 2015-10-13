using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCodeSystem.PublicClass.DatabaseEntity
{
    public class QualityMonthlySalaryList
    {
        /// <summary>
        /// 主键，自增
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }

        /// <summary>
        /// 编号，规则当前年月+流水号
        /// </summary>
        public string QMS_Code
        {
            get;
            set;
        }

        /// <summary>
        /// 车间编号
        /// </summary>
        public string QMS_WorkerCenterCode
        {
            get;
            set;
        }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string QMS_WorkerCenterName
        {
            get;
            set;
        }



        /// <summary>
        /// 员工编号
        /// </summary>
        public string QMS_PersonCode
        {
            get;
            set;
        }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string QMS_PersonName
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人编号 
        /// </summary>
        public string QMS_CreateByCode
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人编号 
        /// </summary>
        public string QMS_CreateByName
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人
        /// </summary>
        public string QMS_ModifyBy
        {
            get;
            set;
        }

        /// <summary>
        /// 生效年月
        /// </summary>
        public DateTime QMS_EffectiveTime
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime QMS_CreateOn
        {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime QMS_ModifyOn
        {
            get;
            set;
        }

        /// <summary>
        /// 质量奖赔
        /// </summary>
        public decimal QMS_QualityMoney
        {
            get;
            set;
        }

        /// <summary>
        /// 杂工工资
        /// </summary>
        public decimal QMS_SundryMoney
        {
            get;
            set;
        }

        /// <summary>
        /// 是否已经计算工资，默认否
        /// </summary>
        public bool QMS_HasCalculated
        {
            get;
            set;
        }
    }
}
