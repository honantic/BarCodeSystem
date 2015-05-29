using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem
{
    public class RememberedUser
    {
        /// <summary>
        /// 用户名
        /// </summary>
        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// 密码,XML中用二进制加密
        /// </summary>
        private string passWord;
        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }

        /// <summary>
        /// 是否记住密码,在登录窗口登录时,如果记住密码,则在XML文档中添加密码信息
        /// 否则,将密码信息变成空
        /// </summary>
        private bool isRemembered;
        public bool IsRemembered
        {
            get { return isRemembered; }
            set { isRemembered = value; }
        }

        /// <summary>
        /// 是否上次登录账号
        /// </summary>
        private bool isLastAccount;
        public bool IsLastAccount
        {
            get { return isLastAccount; }
            set { isLastAccount = value; }
        }

        /// <summary>
        /// 是否自动登录
        /// </summary>
        private bool isAutoLogin;
        public bool IsAutoLogin
        {
            get { return isAutoLogin; }
            set { isAutoLogin = value; }
        }
    }
}
