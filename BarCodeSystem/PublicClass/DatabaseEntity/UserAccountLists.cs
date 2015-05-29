using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace BarCodeSystem
{
    public class UserAccountLists 
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
        /// 登陆账号
        /// </summary>
        public string UA_LoginAccount
        {
            get;
            set;
        }

        /// <summary>
        /// 登陆账号的姓名
        /// </summary>
        public string UA_UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool UA_IsValidated
        {
            get;
            set;
        }

        /// <summary>
        /// 登陆账号部门ID
        /// </summary>
        public Int64 UA_DepartmentID
        {
            get;
            set;
        }

        /// <summary>
        /// 登陆账号部门名称
        /// </summary>
        public string UA_DepartmentName
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
    }
}
