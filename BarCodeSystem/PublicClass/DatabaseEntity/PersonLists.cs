using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Data;

namespace BarCodeSystem
{
    public class PersonLists : StyleSelector
    {
        public Int64 ID { get; set; }
        public bool IsSelected
        {
            get;
            set;
        }
        public bool isRightDepart
        {
            get;
            set;
        }
        public string name
        {
            get;
            set;
        }

        public string code
        {
            get;
            set;
        }
        public string departCode
        {
            get;
            set;
        }
        public string departName
        {
            get;
            set;
        }

        public Int64 departid
        {
            get;
            set;
        }

        /// <summary>
        /// 员工岗位
        /// </summary>
        public string position
        {
            get;
            set;
        }


        Int64 p_U9ID;

        /// <summary>
        /// U9ID
        /// </summary>
        public Int64 P_U9ID
        {
            get { return p_U9ID; }
            set
            {
                p_U9ID = GetU9PersonId(code);
            }
        }

        /// <summary>
        /// 根据员工编号获取U9ID
        /// </summary>
        /// <param name="_code"></param>
        /// <returns></returns>
        Int64 GetU9PersonId(string _code)
        {
            Int64 id = 0;
            string SQl = string.Format(@"select [ID] from [Base_user] where [code]='{0}'", _code);
            SqlConnection sqlcon = new SqlConnection() { ConnectionString = string.Format(@"server={0};database={1};uid={2};pwd={3}", User_Info.server[0], User_Info.database[0], User_Info.uid[0], User_Info.pwd[0]) };
            try
            {
                id = Convert.ToInt64(MyDBController.ExecuteScalar(sqlcon, SQl));
            }
            catch (Exception)
            {
                id = 0;
            }
            return id;
        }



        /// <summary>
        /// 根据员工编号获取U9ID
        /// </summary>
        /// <param name="_code"></param>
        /// <returns></returns>
        public static Int64 GetU9Id(string _code)
        {
            Int64 id = 0;
            string SQl = string.Format(@"select [ID] from [Base_user] where [code]='{0}'", _code);
            SqlConnection sqlcon = new SqlConnection() { ConnectionString = string.Format(@"server={0};database={1};uid={2};pwd={3}", User_Info.server[0], User_Info.database[0], User_Info.uid[0], User_Info.pwd[0]) };
            try
            {
                id = Convert.ToInt64(MyDBController.ExecuteScalar(sqlcon, SQl));
            }
            catch (Exception)
            {
                id = 0;
            }
            return id;
        }

        /// <summary>
        /// 根据员工编号获取员工信息
        /// </summary>
        /// <param name="_personCode"></param>
        /// <returns></returns>
        public static PersonLists FetchPersonInfoByCode(string _personCode)
        {
            DataSet ds = new DataSet();
            PersonLists pl = new PersonLists();
            string SQl = string.Format(@"select * from [person] where [P_Code]='{0}'", _personCode);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "Person");
            MyDBController.CloseConnection();
            if (ds.Tables["Person"].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables["Person"].Rows)
                {
                    pl.ID = Convert.ToInt64(row["ID"]);
                    pl.name = row["P_Name"].ToString();
                    pl.code = row["P_Code"].ToString();
                    pl.departid = Convert.ToInt64(row["P_WorkCenterID"]);
                    pl.position = row["P_Position"].ToString();
                }
            }
            return pl;
        }

        /// <summary>
        /// 根据员工编号获取员工信息列表
        /// </summary>
        /// <param name="_personCode"></param>
        /// <returns></returns>
        public static List<PersonLists> FetchPersonListByCode(string _personCode)
        {
            List<PersonLists> plList = new List<PersonLists>() { };
            PersonLists pl = FetchPersonInfoByCode(_personCode);
            if (!string.IsNullOrEmpty(pl.code))
            {
                plList.Add(pl);
            }
            return plList;
        }
    }
}
