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
                        SQl = string.Format(@"Select A.[ID],A.[TRV_ItemID],A.[TRV_VersionCode],A.[TRV_VersionName],A.[TRV_ReportWay],A.[TRV_IsDefaultVer],A.[TRV_IsValidated],A.[TRV_IsBackVersion],A.[TRV_IsSpecialVersion] from [TechRouteVersion] A where A.[ID]={0}", _id);
                        break;
                    case 2:
                        SQl = string.Format(@"Select A.[ID],A.[TRV_ItemID],A.[TRV_VersionCode],A.[TRV_VersionName],A.[TRV_ReportWay],A.[TRV_IsDefaultVer],A.[TRV_IsValidated],A.[TRV_IsBackVersion],A.[TRV_IsSpecialVersion] from [TechRouteVersion] A left join [FlowCard] B on A.[ID] = B.[FC_ItemTechVersionID] where B.[ID]={0}", _id);
                        break;
                    default:
                        return null;
                }

                MyDBController.GetConnection();
                MyDBController.GetDataSet(SQl, ds, "TechRouteVersion");
                MyDBController.CloseConnection();

                DataRow row = ds.Tables["TechRouteVersion"].Rows[0];
                return new TechVersion()
                {
                    ID = Convert.ToInt64(row["ID"]),
                    TRV_VersionName = row["TRV_VersionName"].ToString(),
                    TRV_VersionCode = row["TRV_VersionCode"].ToString(),
                    TRV_IsBackVersion = Convert.ToBoolean(row["TRV_IsBackVersion"]),
                    TRV_IsDefaultVer = Convert.ToBoolean(row["TRV_IsDefaultVer"]),
                    TRV_IsSpecialVersion = Convert.ToBoolean(row["TRV_IsSpecialVersion"]),
                    TRV_IsValidated = Convert.ToBoolean(row["TRV_IsValidated"]),
                    TRV_ReportWay = Convert.ToInt32(row["TRV_ReportWay"])
                };
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
        public static List<TechVersion> FetchTechVersionByItemCode(Int64 _itemID)
        {
            try
            {
                List<TechVersion> tvList = new List<TechVersion>();
                DataSet ds = new DataSet();
                string SQl = string.Format(@"Select A.[ID],A.[TRV_ItemID],A.[TRV_VersionCode],A.[TRV_VersionName],A.[TRV_ReportWay],A.[TRV_IsDefaultVer],A.[TRV_IsValidated],A.[TRV_IsBackVersion],A.[TRV_IsSpecialVersion],B.[II_Code],B.[II_Name] from [TechRouteVersion] A left join [ItemInfo] B on A.[TRV_ItemID]=B.[ID] where A.[TRV_ItemID]={0}", _itemID);

                MyDBController.GetConnection();
                MyDBController.GetDataSet(SQl, ds, "TechRouteVersion");
                MyDBController.CloseConnection();
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
                        II_Code=row["II_Code"].ToString(),
                        II_Name=row["II_Name"].ToString()
                    });
                }
                return tvList;
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message + "\r\n工艺版本信息有误！请检查！", "提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return null;
            }
        }
    }
}
