using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Security.Cryptography;
using System.Data.Sql;
using System.Data.SqlClient;

namespace BarCodeSystem
{
    public static class Login_Check
    {
        private static string uid;
        public static string Uid
        {
            get{ return Login_Check.uid;}
            set{ Login_Check.uid = value;}
        }

        private static string pwd;
        public static string Pwd
        {
            get { return Login_Check.pwd; }
            set { Login_Check.pwd = value; }
        }
        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="user_pwd"></param>
        /// <returns></returns>
        public static bool IsValidToLogin(string user_id,string user_pwd)
        {
            //这段代码将在正式程序中被保留，在U9库中进行登陆密码验证
            MyDBController.Server = User_Info.server[0];
            MyDBController.Database = User_Info.database[0];
            MyDBController.Uid = User_Info.uid[0];
            MyDBController.Pwd = User_Info.pwd[0];
            MyDBController.GetConnection();
            string U9_pwd="", Input_pwd;
            Input_pwd = Encrypt(user_pwd);
            string sqlstr = "select password  from base_user where Code='"+user_id+"'";
            SqlDataReader sql_reader = MyDBController.GetDataReader(sqlstr);
            while (sql_reader.Read())
            {
                U9_pwd = sql_reader["password"].ToString().Trim();
            }
            sql_reader.Close();
            MyDBController.CloseConnection();
            return U9_pwd==""?false: Equals(U9_pwd, Input_pwd);                  
        }

        /// <summary>
        /// 检验admin登陆
        /// </summary>
        /// <returns></returns>
        public static bool AdminValidToLogin(string user_id, string user_pwd)
        {
            //这段代码将在正式程序中被保留，在U9库中进行登陆密码验证
            MyDBController.Server = User_Info.server[1];
            MyDBController.Database = User_Info.database[1];
            MyDBController.Uid = User_Info.uid[1];
            MyDBController.Pwd = User_Info.pwd[1];
            MyDBController.GetConnection();
            string U9_pwd = "", Input_pwd;
            Input_pwd = Encrypt(user_pwd);
            string sqlstr = "select UA_PassWord  from [UserAccount] where [UA_LoginAccount]='" + user_id + "'";
            SqlDataReader sql_reader = MyDBController.GetDataReader(sqlstr);
            while (sql_reader.Read())
            {
                U9_pwd = sql_reader["UA_PassWord"].ToString().Trim();
            }
            sql_reader.Close();
            MyDBController.CloseConnection();
            return U9_pwd == "" ? false : Equals(U9_pwd, Input_pwd);     
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static string Encrypt(string source)
        {
            MD5 md = MD5.Create();
            byte[] bytes = new UnicodeEncoding().GetBytes(source);
            return Convert.ToBase64String(md.ComputeHash(bytes));
        }
    }
}
