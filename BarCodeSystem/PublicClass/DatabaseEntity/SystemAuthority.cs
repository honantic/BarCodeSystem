using BarCodeSystem.PublicClass.HelperClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BarCodeSystem.PublicClass.DatabaseEntity
{
    public class SystemAuthority
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        public string SA_AuthorityCode { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string SA_AuthorityName { get; set; }

        /// <summary>
        /// 是否拥有子权限
        /// </summary>
        public bool SA_HasItems { get; set; }

        /// <summary>
        /// 父权限ID
        /// </summary>
        public Int64 SA_FatherNodeID { get; set; }

        /// <summary>
        /// 是否是树叶权限，跟是否拥有子权限相反
        /// </summary>
        public bool SA_IsLeafNode { get; set; }

        /// <summary>
        /// 菜单可见性
        /// </summary>
        public bool SA_IsVisible { get; set; }
        /// <summary>
        /// 获取系统所有菜单权限列表
        /// </summary>
        /// <returns></returns>
        public static List<SystemAuthority> FetchSAInfo()
        {
            List<SystemAuthority> saList = new List<SystemAuthority>();
            string SQl = string.Format("select * from [SystemAuthority]");
            DataSet ds = new DataSet();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "SystemAuthority");
            MyDBController.CloseConnection();
            foreach (DataRow row in ds.Tables["SystemAuthority"].Rows)
            {
                SystemAuthority sa = new SystemAuthority();
                sa.ID = Convert.ToInt64(row["ID"]);
                sa.SA_AuthorityCode = row["SA_AuthorityCode"].ToString();
                sa.SA_AuthorityName = row["SA_AuthorityName"].ToString();
                sa.SA_FatherNodeID = row["SA_FatherNodeID"] is DBNull ? -1 : Convert.ToInt64(row["SA_FatherNodeID"]);
                sa.SA_HasItems = row["SA_HasItems"] is DBNull ? false : Convert.ToBoolean(row["SA_HasItems"]);
                sa.SA_IsLeafNode = row["SA_IsLeafNode"] is DBNull ? false : Convert.ToBoolean(row["SA_IsLeafNode"]);
                sa.SA_IsVisible = row["SA_IsVisible"] is DBNull ? false : Convert.ToBoolean(row["SA_IsVisible"]);
                saList.Add(sa);
            }
            List<SystemAuthority> newsaList = new List<SystemAuthority>();

            saList.FindAll(p => p.SA_HasItems).ForEach(
                p =>
                {
                    newsaList.Add(p);
                    saList.ForEach(
                        item =>
                        {
                            if (item.SA_FatherNodeID.Equals(p.ID))
                            {
                                newsaList.Add(item);
                            }
                        });
                });
            return newsaList;
        }

        /// <summary>
        /// 检查系统菜单和后台数据库权限表示否匹配，不匹配则更新后台数据库，假设菜单最多只有两级
        /// </summary>
        public static void CheckSystemAuthority()
        {
            MyDBController.GetConnection();
            DataSet tempDS = new DataSet();
            string SQl = "select top 0 * from [SystemAuthority]";
            MyDBController.GetDataSet(SQl, tempDS, "SystemAuthority");
            MyDBController.CloseConnection();
            List<MenuItem> totalList = ((Main_Window)App.Current.MainWindow).GetMainMenuItemList();
            List<MenuItem> firstLevelList = MyDBController.FindVisualChild<MenuItem>(App.Current.MainWindow);
            List<SystemAuthority> totalSAList = new List<SystemAuthority>();
            List<SystemAuthority> firstLevelSAList = new List<SystemAuthority>();
            totalList.ForEach(p => { totalSAList.Add(new SystemAuthority() { SA_AuthorityCode = p.Name, SA_AuthorityName = p.Header.ToString(), SA_IsVisible = p.Visibility.Equals(Visibility.Visible) }); });
            totalSAList.ForEach(p => { SetSAID(p); });
            firstLevelList.ForEach(p => { firstLevelSAList.Add(new SystemAuthority() { SA_AuthorityCode = p.Name, SA_AuthorityName = p.Header.ToString(), SA_HasItems = true, SA_IsLeafNode = false, SA_IsVisible = p.IsVisible, SA_FatherNodeID = -1 }); });
            firstLevelSAList.ForEach(p => { SetSAID(p); });

            Int64 lastID = -1;
            totalSAList.ForEach(
                p =>
                {
                    if (firstLevelSAList.Count(item => p.SA_AuthorityCode.Equals(item.SA_AuthorityCode)) > 0)
                    {
                        SystemAuthority sa = firstLevelSAList.Find(item => p.SA_AuthorityCode.Equals(item.SA_AuthorityCode));
                        lastID = p.ID = sa.ID;
                        p.SA_FatherNodeID = sa.SA_FatherNodeID;
                        p.SA_HasItems = sa.SA_HasItems;
                        p.SA_IsLeafNode = sa.SA_IsLeafNode;
                    }
                    else
                    {
                        p.SA_FatherNodeID = lastID;
                        p.SA_HasItems = false;
                        p.SA_IsLeafNode = true;
                    }
                    DataRow row = tempDS.Tables["SystemAuthority"].NewRow();
                    row["ID"] = p.ID;
                    row["SA_AuthorityCode"] = p.SA_AuthorityCode;
                    row["SA_AuthorityName"] = p.SA_AuthorityName;
                    row["SA_IsLeafNode"] = p.SA_IsLeafNode;
                    row["SA_HasItems"] = p.SA_HasItems;
                    row["SA_FatherNodeID"] = p.SA_FatherNodeID;
                    row["SA_IsVisible"] = p.SA_IsVisible;
                    tempDS.Tables["SystemAuthority"].Rows.Add(row);
                });
            MyDBController.GetConnection();
            MyDBController.InsertSqlBulk(tempDS.Tables["SystemAuthority"]);
            MyDBController.CloseConnection();
        }

        /// <summary>
        /// 从条码系统中获取ID
        /// </summary>
        /// <param name="_sa"></param>
        private static void SetSAID(SystemAuthority p)
        {
            MyDBController.GetConnection();
            string command = string.Format("select ID  from [SystemAuthority] where SA_AuthorityCode='{0}'", p.SA_AuthorityCode);
            DataSet ds = new DataSet();
            int count = 0;
            MyDBController.GetDataSet(command, ds, "SystemAuthority");
            if (ds.Tables["SystemAuthority"].Rows.Count > 0)
            {
                p.ID = Convert.ToInt64(ds.Tables["SystemAuthority"].Rows[0]["ID"]);
            }
            else
            {
                string SQl = string.Format("insert into [SystemAuthority] (SA_AuthorityCode,SA_AuthorityName,SA_IsLeafNode,SA_HasItems,SA_FatherNodeID,SA_IsVisible) values('{0}','{1}','{2}','{3}',{4},'{5}')", p.SA_AuthorityCode, p.SA_AuthorityName, p.SA_IsLeafNode, p.SA_HasItems, p.SA_FatherNodeID, p.SA_IsVisible);
                count = MyDBController.ExecuteNonQuery(SQl);
                if (count == 1)
                {
                    MyDBController.GetDataSet(command, ds, "SystemAuthority");
                    if (ds.Tables["SystemAuthority"].Rows.Count > 0)
                    {
                        p.ID = Convert.ToInt64(ds.Tables["SystemAuthority"].Rows[0]["ID"]);
                    }
                }
                DBLog _dbLog = new DBLog();
                _dbLog.DBL_OperateBy = User_Info.User_Code + "|" + User_Info.User_Name;
                _dbLog.DBL_OperateTable = "SystemAuthority";
                _dbLog.DBL_OperateTime = DateTime.Now.ToString();
                _dbLog.DBL_OperateType = OperateType.Insert;
                _dbLog.DBL_Content = "新增质量系统菜单权限信息";
                DBLog.WriteDBLog(_dbLog);
            }
            MyDBController.CloseConnection();
        }
    }
}
