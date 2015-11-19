using System;
using System.Collections.Generic;
using System.Data;
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



        /// <summary>
        /// 根据参数返回员工工资信息
        /// </summary>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public static List<StaffSalariesList> Fetch_SSInfo(DateTime start_time, DateTime end_time, string dept_code)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<StaffSalariesList> ssList = new List<StaffSalariesList>();

            string SQl = string.Format(@"select B.FCS_PersonCode,B.FCS_PersonName,C.WC_Department_Name,A.FC_Code,B.FCS_FlowCardID,B.FCS_TechRouteID,B.FCS_ProcessID,B.FCS_ProcessName,B.FCS_ReportTime,FCS_QulifiedAmount,E.WH_WorkHour  from FlowCard as A  left join FlowCardSub as B on (A.ID = B.FCS_FlowCardID)  left join WorkCenter as C on (A.FC_WorkCenter = C.WC_Department_ID)  left join TechRoute as D on (D.ID = B.FCS_TechRouteID)  left join WorkHour as E on (D.ID = E.WH_TechRouteID)  where  CONVERT(date, B.FCS_ReportTime) >='{0}' and CONVERT(date,B.FCS_ReportTime) <='{1}' and C.WC_Department_Code ='{2}' and B.FCS_IsReported='true' and E.ID = (select MAX(F.id) from WorkHour F where F.WH_TechRouteID=D.ID and CONVERT(date, F.WH_StartDate)<=CONVERT(date, B.FCS_ReportTime) and CONVERT(date, F.WH_EndDate)>=CONVERT(date, B.FCS_ReportTime))", start_time.ToString("yyyy-MM-dd HH:mm:ss"), end_time.ToString("yyyy-MM-dd HH:mm:ss"),dept_code);

            MyDBController.GetConnection();
            dt = MyDBController.GetDataSet(SQl, ds, "StaffSalaries").Tables["StaffSalaries"];
            MyDBController.CloseConnection();

            foreach (DataRow row in dt.Rows)
            {
                StaffSalariesList ssl = new StaffSalariesList();
                ssl.PersonCode = row["FCS_PersonCode"].ToString();
                ssl.PersonName = row["FCS_PersonName"].ToString();
                ssl.Department_Name = row["WC_Department_Name"].ToString();
                ssl.FC_Code = row["FC_Code"].ToString();

                ssl.FC_ID = Int64.Parse(row["FCS_FlowCardID"].ToString());
                ssl.TR_ID = Int64.Parse(row["FCS_TechRouteID"].ToString());
                ssl.ProcessID = Int64.Parse(row["FCS_ProcessID"].ToString());

                ssl.ProcessName = row["FCS_ProcessName"].ToString();
                ssl.ReportTime = row["FCS_ReportTime"].ToString();
                ssl.QulifiedAmount = decimal.Parse(row["FCS_QulifiedAmount"].ToString());
                ssl.WorkHour = decimal.Parse(row["WH_WorkHour"].ToString());
                ssList.Add(ssl);
            }
            return ssList;          
        }

    }
}
