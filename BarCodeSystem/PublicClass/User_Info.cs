using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem
{
    public class User_Info
    {
        private static Int64 user_id;
        public static Int64 User_ID
        {
            get { return user_id; }
            set { user_id = value; }
        }

        private static string user_name="";
        public static string User_Name
        {
            get { return user_name; }
            set { user_name = value; }
        }
        

        public static string[] User_Org_Code = { "201"};
        //家用四通阀事业部：201；

        public static string[] server = {"172.16.100.123","172.16.100.40" };
        //U9测试库；条码测试库；

        public static string[] database = { "sanhuadata20131011", "BarCodeSystem" };
        //U9测试数据库；条码测试数据库

        public static string[] uid = { "sa","sa"};
        //U9测试库账号；条码测试库账号

        public static string[] pwd = { "Aa1","Bb123"};
        //U9测试库密码；条码测试库密码

        public static string[] pattern = {@"^\d+\z" ,@"^\d+\.?\d*\z"};
        //编码文本框校验表达式；

        public static string rememberedAccountFileName = "D:/Program Files/BarCodeSystem/SysPara/RememberedAccount.xml";

        public static string sysPara = "D:/Program Files/BarCodeSystem/Syspara";
        /// <summary>
        /// 当前屏幕分辨率参数：高
        /// </summary>
        public static int ScreenHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 当前屏幕分辨率参数：宽
        /// </summary>
        public static int ScreenWidth
        {
            get;
            set;
        }
    }
}
