using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem
{
    public class UserAuthorityLists
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
        /// 账号ID
        /// </summary>
        public Int64 UA_UserAccountID
        {
            get;
            set;
        }

        /// <summary>
        /// 账号
        /// </summary>
        public string UA_LoginAccount
        {
            get;
            set;
        }

        /// <summary>
        /// 权限ID
        /// </summary>
        public Int64 UA_SysAuthorityID
        {
            get;
            set;
        }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string SA_AuthorityName
        {
            get;
            set;
        }
    }
}
