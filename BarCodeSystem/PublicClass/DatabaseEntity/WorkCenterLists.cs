using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BarCodeSystem
{
    public class WorkCenterLists
    {
        private bool isselected = false;
        public bool IsSelected
        {
            get { return isselected; }
            set { isselected = value; }
        }

        /// <summary>
        /// id
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }

        //部门编码
        public string department_code
        {
            get;
            set;
        }

        //部门名称
        public string department_name
        {
            get;
            set;
        }

        //部门简称
        public string department_shortname
        {
            get;
            set;
        }

        /// <summary>
        /// 部门U9id
        /// </summary>
        public Int64 department_id
        {
            get;
            set;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public string isvalidated
        {
            get;
            set;
        }


        /// <summary>
        /// 是否启用
        /// </summary>
        public bool isvalidated_DB
        {
            get;
            set;
        }

        /// <summary>
        /// 是否按照领料单控制派工数量
        /// </summary>
        public string isordercontroled
        {
            get;
            set;
        }

        /// <summary>
        /// 是否按照领料单控制派工数量
        /// </summary>
        public bool isordercontroled_DB
        {
            get;
            set;
        }
        /// <summary>
        /// 是否工作中心
        /// </summary>
        public string isworkcenter
        {
            get;
            set;
        }


        /// <summary>
        /// 是否工作中心
        /// </summary>
        public bool isworkcenter_DB
        {
            get;
            set;
        }


        /// <summary>
        /// 最新修改时间
        /// </summary>
        public string lastoperatetime
        {
            get;
            set;
        }

        /// <summary>
        /// 最新修改时间
        /// </summary>
        public DateTime lastoperatetime_DB
        {
            get;
            set;
        }


        /// <summary>
        /// 最新修改操作人员
        /// </summary>
        public string lastoperateby
        {
            get;
            set;
        }

        /// <summary>
        /// 获取车间信息
        /// </summary>
        /// <returns></returns>
        public static List<WorkCenterLists> FetchWCInfo()
        {
            List<WorkCenterLists> wclList = new List<WorkCenterLists>();
            DataSet ds = new DataSet();
            string SQl = string.Format(@"select * from [WorkCenter] where [WC_IsValidated]='true' and [WC_Department_ShortName] !=''");
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "WorkCenter");
            MyDBController.CloseConnection();
            foreach (DataRow row in ds.Tables["WorkCenter"].Rows)
            {
                WorkCenterLists wcl = new WorkCenterLists();
                wcl.ID = Convert.ToInt64(row["ID"]);
                wcl.department_code = row["WC_Department_Code"].ToString();
                wcl.department_name = row["WC_Department_Name"].ToString();
                wcl.department_id = Convert.ToInt64(row["WC_Department_ID"]);
                wcl.department_shortname = row["WC_Department_ShortName"].ToString();
                wcl.isvalidated_DB = Convert.ToBoolean(row["WC_IsValidated"]);
                wcl.isworkcenter_DB = Convert.ToBoolean(row["WC_IsWorkCenter"]);
                wcl.isordercontroled_DB = Convert.ToBoolean(row["WC_IsOrderControled"]);
                wcl.lastoperateby = row["WC_LastOprateBy"].ToString();
                wcl.lastoperatetime_DB = Convert.ToDateTime(row["WC_LastOperateTime"]);
                wclList.Add(wcl);
            }
            return wclList;
        }

        /// <summary>
        /// 根据车间编码获取车间信息
        /// </summary>
        /// <returns></returns>
        public static List<WorkCenterLists> FetchWCInfoByWCCode(string _wcCode)
        {
            List<WorkCenterLists> wclList = new List<WorkCenterLists>();
            DataSet ds = new DataSet();
            string SQl = string.Format(@"select * from [WorkCenter] where [WC_Department_Code]='{0}'", _wcCode);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "WorkCenter");
            MyDBController.CloseConnection();
            foreach (DataRow row in ds.Tables["WorkCenter"].Rows)
            {
                WorkCenterLists wcl = new WorkCenterLists();
                wcl.ID = Convert.ToInt64(row["ID"]);
                wcl.department_code = row["WC_Department_Code"].ToString();
                wcl.department_name = row["WC_Department_Name"].ToString();
                wcl.department_id = Convert.ToInt64(row["WC_Department_ID"]);
                wcl.department_shortname = row["WC_Department_ShortName"].ToString();
                wcl.isvalidated_DB = Convert.ToBoolean(row["WC_IsValidated"]);
                wcl.isworkcenter_DB = Convert.ToBoolean(row["WC_IsWorkCenter"]);
                wcl.isordercontroled_DB = Convert.ToBoolean(row["WC_IsOrderControled"]);
                wcl.lastoperateby = row["WC_LastOprateBy"].ToString();
                wcl.lastoperatetime_DB = Convert.ToDateTime(row["WC_LastOperateTime"]);
                wclList.Add(wcl);
            }
            return wclList;
        }

    }
}
