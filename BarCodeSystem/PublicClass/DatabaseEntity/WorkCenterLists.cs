using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem
{
    public class WorkCenterLists
    {
        private  bool isselected = false;
        public  bool IsSelected
        {
            get { return isselected; }
            set { isselected = value; }
        }

        //部门编码
        public  string department_code
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

        //部门id
        public Int64 department_id
        {
            get;
            set;
        }

        //是否启用
        public string isvalidated
        {
            get;
            set;
        }


        //是否启用
        public bool isvalidated_DB
        {
            get;
            set;
        }

        //是否按照领料单控制派工数量
        public string isordercontroled
        {
            get;
            set;
        }

        //是否按照领料单控制派工数量
        public bool isordercontroled_DB
        {
            get;
            set;
        }

        //是否工作中心
        public string isworkcenter
        {
            get;
            set;
        }


        //是否工作中心
        public bool isworkcenter_DB
        {
            get;
            set;
        }


        //最新修改时间
        public string lastoperatetime
        {
            get;
            set;
        }

        //最新修改时间
        public DateTime lastoperatetime_DB
        {
            get;
            set;
        }


        //最新修改操作人员
        public string lastoperateby
        {
            get;
            set;
        }
    }
}
