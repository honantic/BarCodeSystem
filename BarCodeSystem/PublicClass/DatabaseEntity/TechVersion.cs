using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;

namespace BarCodeSystem
{
    public class TechVersion
    {
        public TechVersion()
        {
            TRV_VersionCode = "";
            TRV_VersionName = "";
            ID = -1;
            TRV_HasFlowCard = false;
            TRV_IsBackVersion = false;
            TRV_IsDefaultVer = true;
            TRV_IsShown = true;
            TRV_IsSpecialVersion = false;
            TRV_IsValidated = true;
            TRV_ReportWay = -1;
        }
        /// <summary>
        /// 料品的工艺路线版本中编码
        /// </summary>
        public string TRV_VersionCode
        {
            get;
            set;
        }
        /// <summary>
        /// 工艺路线版本名称
        /// </summary>
        public string TRV_VersionName
        {
            get;
            set;
        }
        /// <summary>
        /// 料品工艺路线版本ID
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }

        /// <summary>
        /// 料品ID
        /// </summary>
        public Int64 TRV_ItemID
        { get; set; }

        /// <summary>
        /// 是否默认工艺路线版本，默认false
        /// </summary>
        public bool TRV_IsDefaultVer
        {
            get;
            set;
        }
        /// <summary>
        /// 是否返工版本 0:否  1:是 默认否,新增
        /// </summary>
        public bool TRV_IsBackVersion
        {
            get;
            set;
        }
        /// <summary>
        /// 是否生效 0:否 1:是 默认否
        /// </summary>
        public bool TRV_IsValidated
        {
            get;
            set;
        }
        /// <summary>
        /// 是否特殊版本
        /// </summary>
        public bool TRV_IsSpecialVersion
        {
            get;
            set;
        }
        /// <summary>
        /// 该工艺版本是否已经有流转卡
        /// </summary>
        public bool TRV_HasFlowCard
        { get; set; }

        /// <summary>
        /// 该版本工艺是否展示
        /// </summary>
        public bool TRV_IsShown { get; set; }
        /// <summary>
        /// 料品料号
        /// </summary>
        public string II_Code { get; set; }
        /// <summary>
        /// 料品名称
        /// </summary>
        public string II_Name { get; set; }
        /// <summary>
        /// 报工方式 0：流水线报工  1：离散报工
        /// </summary>
        public int TRV_ReportWay { get; set; }

        /// <summary>
        /// 获取全部的工艺版本信息
        /// </summary>
        /// <returns></returns>
        public static List<TechVersion> FetchTechVersion()
        {
            string SQl = string.Format("Select A.[ID],A.[TRV_ItemID],A.[TRV_VersionCode],A.[TRV_VersionName],A.[TRV_ReportWay],A.[TRV_IsDefaultVer],A.[TRV_IsValidated],A.[TRV_IsBackVersion],A.[TRV_IsSpecialVersion],A.[TRV_IsShown],A.[TRV_HasFlowCard],B.[II_Code],B.[II_Name] from [TechRouteVersion] A left join [ItemInfo] B on A.[TRV_ItemID]=B.[ID]");
            return ExecuteSQlCommand(SQl);
        }

        /// <summary>
        /// 根据id获取工艺路线版本信息
        /// </summary>
        /// <param name="_id">id</param>
        /// <param name="_idType">id种类 1:代表工艺版本id 2:代表流转卡id</param>
        /// <returns></returns>
        public static TechVersion FetchTechVersion(Int64 _id, int _idType)
        {
            try
            {
                DataSet ds = new DataSet();
                string SQl = "";
                switch (_idType)
                {
                    case 1:
                        SQl = string.Format(@"Select A.[ID],A.[TRV_ItemID],A.[TRV_VersionCode],A.[TRV_VersionName],A.[TRV_ReportWay],A.[TRV_IsDefaultVer],A.[TRV_IsValidated],A.[TRV_IsBackVersion],A.[TRV_IsSpecialVersion],A.[TRV_IsShown],A.[TRV_HasFlowCard],B.[II_Code],B.[II_Name] from [TechRouteVersion] A left join [ItemInfo] B on A.[TRV_ItemID]=B.[ID] where A.[ID]={0}", _id);
                        break;
                    case 2:
                        SQl = string.Format(@"Select A.[ID],A.[TRV_ItemID],A.[TRV_VersionCode],A.[TRV_VersionName],A.[TRV_ReportWay],A.[TRV_IsDefaultVer],A.[TRV_IsValidated],A.[TRV_IsBackVersion],A.[TRV_IsSpecialVersion],A.[TRV_IsShown],A.[TRV_HasFlowCard] ,B.[II_Code],B.[II_Name] from [TechRouteVersion] A left join [ItemInfo] B on A.[TRV_ItemID]=B.[ID]  left join [FlowCard] C on A.[ID] = C.[FC_ItemTechVersionID] where C.[ID]={0}", _id);
                        break;
                    default:
                        return null;
                }
                return ExecuteSQlCommand(SQl).FirstOrDefault();
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message + "\r\n工艺版本信息有误！请检查！", "提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// 根据料品ID搜索工艺路线版本
        /// </summary>
        /// <param name="_itemCode"></param>
        /// <returns></returns>
        public static List<TechVersion> FetchTechVersionByItemID(Int64 _itemID)
        {
            try
            {
                string SQl = string.Format(@"Select A.[ID],A.[TRV_ItemID],A.[TRV_VersionCode],A.[TRV_VersionName],A.[TRV_ReportWay],A.[TRV_IsDefaultVer],A.[TRV_IsValidated],A.[TRV_IsBackVersion],A.[TRV_IsSpecialVersion],A.[TRV_IsShown],A.[TRV_HasFlowCard],B.[II_Code],B.[II_Name] from [TechRouteVersion] A left join [ItemInfo] B on A.[TRV_ItemID]=B.[ID] where A.[TRV_ItemID]={0}", _itemID);
                return ExecuteSQlCommand(SQl);
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message + "\r\n工艺版本信息有误！请检查！", "提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// 根据料品ID搜索工艺路线版本
        /// </summary>
        /// <param name="_itemCode"></param>
        /// <returns></returns>
        public static List<TechVersion> FetchTechVersionByItemCode(string _itemCode)
        {
            try
            {
                string SQl = string.Format(@"Select A.[ID],A.[TRV_ItemID],A.[TRV_VersionCode],A.[TRV_VersionName],A.[TRV_ReportWay],A.[TRV_IsDefaultVer],A.[TRV_IsValidated],A.[TRV_IsBackVersion],A.[TRV_IsSpecialVersion],A.[TRV_IsShown],A.[TRV_HasFlowCard],B.[II_Code],B.[II_Name] from [TechRouteVersion] A left join [ItemInfo] B on A.[TRV_ItemID]=B.[ID] where A.[TRV_ItemID]=(select [ID] from [ItemInfo] where [II_Code]='{0}')", _itemCode);
                return ExecuteSQlCommand(SQl);
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message + "\r\n工艺版本信息有误！请检查！", "提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return null;
            }
        }


        /// <summary>
        /// 执行sql命令
        /// </summary>
        /// <param name="_command"></param>
        /// <returns></returns>
        private static List<TechVersion> ExecuteSQlCommand(string _command)
        {
            List<TechVersion> tvList = new List<TechVersion>();
            DataSet ds = new DataSet();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(_command, ds, "TechRouteVersion");
            foreach (DataRow row in ds.Tables["TechRouteVersion"].Rows)
            {
                tvList.Add(new TechVersion()
                {
                    ID = Convert.ToInt64(row["ID"]),
                    TRV_VersionName = row["TRV_VersionName"].ToString(),
                    TRV_VersionCode = row["TRV_VersionCode"].ToString(),
                    TRV_IsBackVersion = Convert.ToBoolean(row["TRV_IsBackVersion"]),
                    TRV_IsDefaultVer = Convert.ToBoolean(row["TRV_IsDefaultVer"]),
                    TRV_IsSpecialVersion = Convert.ToBoolean(row["TRV_IsSpecialVersion"]),
                    TRV_IsValidated = Convert.ToBoolean(row["TRV_IsValidated"]),
                    TRV_ReportWay = Convert.ToInt32(row["TRV_ReportWay"]),
                    TRV_IsShown = row["TRV_IsShown"] is DBNull ? true : Convert.ToBoolean(row["TRV_IsShown"]),
                    TRV_HasFlowCard = row["TRV_HasFlowCard"] is DBNull ? HasFCOrNot(Convert.ToInt64(row["ID"])) : Convert.ToBoolean(row["TRV_HasFlowCard"]),
                    II_Code = row["II_Code"].ToString(),
                    II_Name = row["II_Name"].ToString(),
                    TRV_ItemID = Convert.ToInt64(row["TRV_ItemID"])
                });
            }
            MyDBController.CloseConnection();
            return tvList;
        }

        /// <summary>
        /// 获取当前工艺路线，是否拥有流转卡信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private static bool HasFCOrNot(long ID)
        {
            string SQl = string.Format("select count(*) from FlowCard where [FC_ItemTechVersionID]={0}", ID);
            int count = -1;
            try
            {
                count = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));
            }
            catch (Exception)
            {
                count = -1;
            }
            return count > 0;
        }

        /// <summary>
        /// 检查版本编号是否存在
        /// </summary>
        /// <param name="_versionCode">版本编号</param>
        /// <param name="_itemID">料品ID</param>
        /// <returns></returns>
        public static bool CheckForCode(string _versionCode, Int64 _itemID)
        {
            bool flag = false;
            string SQl = string.Format("select count(*) from [TechRouteVersion] where [TRV_VersionCode]='{0}' and [TRV_ItemID]='{1}'", _versionCode, _itemID);
            int count = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));
            flag = count > 0;
            return flag;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="_tv"></param>
        /// <returns></returns>
        public static bool SaveInfo(TechVersion _tv)
        {
            List<TechVersion> _tvList = new List<TechVersion>() { _tv };
            return SaveInfo(_tvList);
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="_tvList"></param>
        /// <returns></returns>
        public static bool SaveInfo(List<TechVersion> _tvList)
        {
            MyDBController.GetConnection();
            string SQl = string.Format("select top 0 * from [TechRouteVersion]");
            DataSet ds = new DataSet();
            _tvList.ForEach(p => AcceptDefaultVersion(p));
            MyDBController.GetDataSet(SQl, ds, "TechRouteVersion");
            bool flag = MyDBController.InsertSqlBulk<TechVersion>(_tvList, ds.Tables["TechRouteVersion"]);
            MyDBController.CloseConnection();
            return flag;
        }

        /// <summary>
        /// 接受新的默认版本
        /// </summary>
        /// <param name="_tv"></param>
        private static void AcceptDefaultVersion(TechVersion _tv)
        {
            if (_tv.TRV_IsDefaultVer)
            {
                string SQl = string.Format("update [TechRouteVersion] set [TRV_IsDefaultVer]='false' where TRV_ItemID={0} and ID != {1}", _tv.TRV_ItemID, _tv.ID);
                MyDBController.ExecuteNonQuery(SQl);
            }
        }

        /// <summary>
        /// 根据版本信息获取其id
        /// </summary>
        /// <param name="_tv"></param>
        /// <returns></returns>
        public static Int64 FetchVersionID(TechVersion _tv)
        {
            string SQl = string.Format("select top 1 [ID] from [TechRouteVersion]  where  [TRV_VersionCode]='{0}' and [TRV_ItemID]='{1}'", _tv.TRV_VersionCode, _tv.TRV_ItemID);
            MyDBController.GetConnection();
            Int64 _id = -1;
            try
            {
                _id = Convert.ToInt64(MyDBController.ExecuteScalar(SQl));
            }
            catch (Exception)
            {
            }
            return _id;
        }
    }
}
