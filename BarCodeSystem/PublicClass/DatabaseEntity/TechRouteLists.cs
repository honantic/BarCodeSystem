using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BarCodeSystem
{
    public class TechRouteLists
    {

        public TechRouteLists()
        {
            ID = -1;
            TR_ItemID = 0;
            TRV_VersionCode = "";
            II_Name = "";
            TR_ProcessSequence = 0;
            TR_ProcessName = "";
            //TR_WagePerPiece = 0;
            TR_WorkHour = 0;
            TR_WorkCenterID = 0;
        }

        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }

        /// <summary>
        /// 料品ID
        /// </summary>
        public Int64 TR_ItemID
        {
            get;
            set;
        }

        /// <summary>
        /// 料品编码
        /// </summary>
        public string TR_ItemCode
        {
            get;
            set;
        }

        /// <summary>
        /// 料品名称，工艺路线表里面没有这个字段，从料品表中取出
        /// </summary>
        public string II_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 工艺路线版本ID
        /// </summary>
        public Int64 TR_VersionID
        {
            get;
            set;
        }

        /// <summary>
        /// 工艺路线版本编码
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
        /// 工艺序号
        /// </summary>
        public int TR_ProcessSequence
        {
            get;
            set;
        }

        /// <summary>
        /// 工艺名称
        /// </summary>
        public string TR_ProcessName
        {
            get;
            set;
        }

        /// <summary>
        /// 工艺编码
        /// </summary>
        public string TR_ProcessCode
        {
            get;
            set;
        }

        /// <summary>
        /// 来自工序表，工序在车间里面的代码，用来打印流转卡的
        /// </summary>
        public string PN_CodeInWorkCenter
        {
            get;
            set;
        }

        /// <summary>
        /// 工时
        /// </summary>
        public decimal TR_WorkHour
        {
            get;
            set;
        }

        ///// <summary>
        ///// 计件工资分配方式， 0:独立、1、平分、2、按合作人数分配配额公式计算
        ///// </summary>
        //public int TR_WageAllotScheme
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 计件工资分配方式  中文展示
        ///// </summary>
        //public string TR_WageAllotScheme_Show
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 工作中心ID
        /// </summary>
        public Int64 TR_WorkCenterID
        {
            get;
            set;
        }

        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string WC_Department_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 工序ID
        /// </summary>
        public Int64 TR_ProcessID
        {
            get;
            set;
        }

        private bool tr_IsReportPoint = false;
        /// <summary>
        /// 是否报工点
        /// </summary>
        public bool TR_IsReportPoint
        {
            get { return tr_IsReportPoint; }
            set { tr_IsReportPoint = value; }
        }


        bool tr_IsExProcess = false;
        /// <summary>
        /// 是否委外工序
        /// </summary>
        public bool TR_IsExProcess
        {
            get { return tr_IsExProcess; }
            set { tr_IsExProcess = value; }
        }

        bool tr_IsFirstProcess = false;
        /// <summary>
        /// 是否首道工序
        /// </summary>
        public bool TR_IsFirstProcess
        {
            get { return tr_IsFirstProcess; }
            set { tr_IsFirstProcess = value; }
        }

        bool tr_IsLastProcess = false;
        /// <summary>
        /// 是否末道工序
        /// </summary>
        public bool TR_IsLastProcess
        {
            get { return tr_IsLastProcess; }
            set { tr_IsLastProcess = value; }
        }

        bool tr_IsReportDevice = false;
        /// <summary>
        /// 是否报工设备
        /// </summary>
        public bool TR_IsReportDevice
        {
            get { return tr_IsReportDevice; }
            set { tr_IsReportDevice = value; }
        }

        bool tr_IsDeviceCharging = false;
        /// <summary>
        /// 是否设备计费
        /// </summary>
        public bool TR_IsDeviceCharging
        {
            get { return tr_IsDeviceCharging; }
            set { tr_IsDeviceCharging = value; }
        }

        ///// <summary>
        ///// 标准单件工资
        ///// </summary>
        //public decimal TR_WagePerPiece
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 是否默认工艺路线版本
        /// </summary>
        public bool TR_IsDefaultVer
        {
            get;
            set;
        }

        /// <summary>
        /// 是否返工工序
        /// </summary>
        public bool TR_IsBackProcess
        { get; set; }

        /// <summary>
        /// 是否测试工序
        /// </summary>
        public bool TR_IsTestProcess
        {
            get;
            set;
        }

        public string WorkHour { get { return TR_WorkHour.ToString("C"); } }
        /// <summary>
        /// 默认检验员名字
        /// </summary>
        public string TR_DefaultCheckPersonName { get; set; }

        /// <summary>
        /// 绑定工序
        /// </summary>
        public int TR_BindingProcess { get; set; }
        /// <summary>
        /// 人员编码
        /// </summary>
        public string personCode
        { get; set; }

        public string personName
        { get; set; }

        /// <summary>
        /// 是否是盘库时统计工序
        /// </summary>
        public bool TR_IsCountingProcess { get; set; }

        /// <summary>
        /// 是否是平分工序，默认是
        /// </summary>
        public bool TR_IsEquallyDivideProcess { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public bool TRV_IsSpecialVersion { get; set; }

        ///// <summary>
        ///// 工艺路线派工的时候，改道工序的操作人员列表
        ///// </summary>
        //public List<PersonLists> personList { get; set; }

        /// <summary>
        /// 根据工作中心id查询工艺路线列表
        /// </summary>
        /// <param name="_wcID"></param>
        /// <returns></returns>
        public static List<TechRouteLists> FetchTechRouteByWCID(Int64 _wcID)
        {
            string SQl = string.Format(@" SELECT A.[ID],A.[TR_ItemID],A.[TR_ItemCode],C.[II_Name],C.[II_Spec],C.[II_UnitName],C.[II_Version],A.[TR_VersionID],A.[TR_IsTestProcess],A.[TR_DefaultCheckPersonName],A.[TR_WorkHour],A.[TR_IsFirstProcess],A.[TR_IsLastProcess],A.[TR_IsBackProcess],B.[TRV_VersionCode],B.[TRV_VersionName],B.[TRV_IsDefaultVer],B.[TRV_IsSpecialVersion],B.[TRV_ReportWay],D.[WC_Department_Name],A.[TR_WorkCenterID],A.[TR_ProcessSequence],A.[TR_ProcessName],A.[TR_ProcessCode],A.[TR_ProcessID],A.[TR_IsCountingProcess],A.[TR_IsEquallyDivideProcess],E.[PN_CodeInWorkCenter] FROM [TechRoute] A LEFT JOIN [TechRouteVersion] B ON A.[TR_ItemID]=B.[TRV_ItemID]  AND A.[TR_VersionID]=B.[ID]LEFT JOIN [ItemInfo] C ON A.[TR_ItemID]=C.[ID] LEFT JOIN [WorkCenter] D ON A.[TR_WorkCenterID]=D.[WC_Department_ID] left join [ProcessName] E on A.[TR_ProcessID] =E.[ID]where A.[TR_WorkCenterID]={0} ORDER BY A.[TR_ItemCode],B.[TRV_IsDefaultVer] desc,A.[TR_VersionID],A.[TR_ProcessSequence]", _wcID);
            return ExecuteSQlCommand(SQl);
        }

        /// <summary>
        /// 根据料品编号搜索工艺路线，可选参数工艺路线版本
        /// </summary>
        /// <param name="_itemCode">料品编号</param>
        /// <param name="_techrouteVersionCode">工艺路线版本编号</param>
        /// <returns></returns>
        public static List<TechRouteLists> FetchTechRouteByItemCode(string _itemCode, string _techrouteVersionCode = "")
        {

            string SQl = "";
            if (string.IsNullOrEmpty(_techrouteVersionCode))
            {
                SQl = string.Format(@" SELECT A.[ID],A.[TR_ItemID],A.[TR_ItemCode],C.[II_Name],C.[II_Spec],C.[II_UnitName],C.[II_Version],A.[TR_VersionID],A.[TR_IsTestProcess],A.[TR_DefaultCheckPersonName],A.[TR_WorkHour],A.[TR_IsFirstProcess],A.[TR_IsLastProcess],A.[TR_IsBackProcess],B.[TRV_VersionCode],B.[TRV_VersionName],B.[TRV_IsDefaultVer],B.[TRV_IsSpecialVersion],B.[TRV_ReportWay],D.[WC_Department_Name],A.[TR_WorkCenterID],A.[TR_ProcessSequence],A.[TR_ProcessName],A.[TR_ProcessCode],A.[TR_ProcessID],A.[TR_IsCountingProcess],A.[TR_IsEquallyDivideProcess],E.[PN_CodeInWorkCenter] FROM [TechRoute] A LEFT JOIN [TechRouteVersion] B ON A.[TR_ItemID]=B.[TRV_ItemID]  AND A.[TR_VersionID]=B.[ID]LEFT JOIN [ItemInfo] C ON A.[TR_ItemID]=C.[ID] LEFT JOIN [WorkCenter] D ON A.[TR_WorkCenterID]=D.[WC_Department_ID] left join [ProcessName] E on A.[TR_ProcessID] =E.[ID] where A.[TR_ItemCode]='{0}' ORDER BY A.[TR_ItemCode],B.[TRV_IsDefaultVer] desc,A.[TR_VersionID],A.[TR_ProcessSequence]", _itemCode);
            }
            else
            {
                SQl = string.Format(@" SELECT A.[ID],A.[TR_ItemID],A.[TR_ItemCode],C.[II_Name],C.[II_Spec],C.[II_UnitName],C.[II_Version],A.[TR_VersionID],A.[TR_IsTestProcess],A.[TR_DefaultCheckPersonName],A.[TR_WorkHour],A.[TR_IsFirstProcess],A.[TR_IsLastProcess],A.[TR_IsBackProcess],B.[TRV_VersionCode],B.[TRV_VersionName],B.[TRV_IsDefaultVer],B.[TRV_IsSpecialVersion],B.[TRV_ReportWay],D.[WC_Department_Name],A.[TR_WorkCenterID],A.[TR_ProcessSequence],A.[TR_ProcessName],A.[TR_ProcessCode],A.[TR_ProcessID],A.[TR_IsCountingProcess],A.[TR_IsEquallyDivideProcess],E.[PN_CodeInWorkCenter] FROM [TechRoute] A LEFT JOIN [TechRouteVersion] B ON A.[TR_ItemID]=B.[TRV_ItemID]  AND A.[TR_VersionID]=B.[ID]LEFT JOIN [ItemInfo] C ON A.[TR_ItemID]=C.[ID] LEFT JOIN [WorkCenter] D ON A.[TR_WorkCenterID]=D.[WC_Department_ID] left join [ProcessName] E on A.[TR_ProcessID] =E.[ID] where A.[TR_ItemCode]='{0}' and  B.[TRV_VersionCode]='{1}' ORDER BY A.[TR_ItemCode],B.[TRV_IsDefaultVer] desc,A.[TR_VersionID],A.[TR_ProcessSequence]", _itemCode, _techrouteVersionCode);
            }
            return ExecuteSQlCommand(SQl);
        }

        /// <summary>
        /// 根据工艺路线版本获取工艺路线信息
        /// </summary>
        /// <param name="_tv"></param>
        /// <returns></returns>
        public static List<TechRouteLists> FetchTechRouteByVersion(TechVersion _tv)
        {
            string SQl = string.Format(@" SELECT A.[ID],A.[TR_ItemID],A.[TR_ItemCode],C.[II_Name],C.[II_Spec],C.[II_UnitName],C.[II_Version],A.[TR_VersionID],A.[TR_IsTestProcess],A.[TR_DefaultCheckPersonName],A.[TR_WorkHour],A.[TR_IsFirstProcess],A.[TR_IsLastProcess],A.[TR_IsBackProcess],B.[TRV_VersionCode],B.[TRV_VersionName],B.[TRV_IsDefaultVer],B.[TRV_IsSpecialVersion],B.[TRV_ReportWay],D.[WC_Department_Name],A.[TR_WorkCenterID],A.[TR_ProcessSequence],A.[TR_ProcessName],A.[TR_ProcessCode],A.[TR_ProcessID],A.[TR_IsCountingProcess],A.[TR_IsEquallyDivideProcess],E.[PN_CodeInWorkCenter]  FROM [TechRoute] A LEFT JOIN [TechRouteVersion] B ON A.[TR_ItemID]=B.[TRV_ItemID]  AND A.[TR_VersionID]=B.[ID]LEFT JOIN [ItemInfo] C ON A.[TR_ItemID]=C.[ID] LEFT JOIN [WorkCenter] D ON A.[TR_WorkCenterID]=D.[WC_Department_ID] left join [ProcessName] E on A.[TR_ProcessID] =E.[ID] where B.[ID]={0} ORDER BY A.[TR_ItemCode],B.[TRV_IsDefaultVer] desc,A.[TR_VersionID],A.[TR_ProcessSequence]", _tv.ID);
            return ExecuteSQlCommand(SQl);
        }

        /// <summary>
        /// 根据流转卡行表信息获取对应的工艺路线记录
        /// </summary>
        /// <param name="flowCardSubLists"></param>
        /// <returns></returns>
        public static TechRouteLists FetchTRLInfoByFCS(PublicClass.DatabaseEntity.FlowCardSubLists flowCardSubLists)
        {
            string SQl = string.Format(@" SELECT A.[ID],A.[TR_ItemID],A.[TR_ItemCode],C.[II_Name],C.[II_Spec],C.[II_UnitName],C.[II_Version],A.[TR_VersionID],A.[TR_IsTestProcess],A.[TR_DefaultCheckPersonName],A.[TR_WorkHour],A.[TR_IsFirstProcess],A.[TR_IsLastProcess],A.[TR_IsBackProcess],B.[TRV_VersionCode],B.[TRV_VersionName],B.[TRV_IsDefaultVer],B.[TRV_IsSpecialVersion],B.[TRV_ReportWay],D.[WC_Department_Name],A.[TR_WorkCenterID],A.[TR_ProcessSequence],A.[TR_ProcessName],A.[TR_ProcessCode],A.[TR_ProcessID],A.[TR_IsCountingProcess],A.[TR_IsEquallyDivideProcess],E.[PN_CodeInWorkCenter]  FROM [TechRoute] A LEFT JOIN [TechRouteVersion] B ON A.[TR_ItemID]=B.[TRV_ItemID]  AND A.[TR_VersionID]=B.[ID]LEFT JOIN [ItemInfo] C ON A.[TR_ItemID]=C.[ID] LEFT JOIN [WorkCenter] D ON A.[TR_WorkCenterID]=D.[WC_Department_ID] left join [ProcessName] E on A.[TR_ProcessID] =E.[ID] where A.[ID]={0} ORDER BY A.[TR_ItemCode],B.[TRV_IsDefaultVer] desc,A.[TR_VersionID],A.[TR_ProcessSequence]", flowCardSubLists.FCS_TechRouteID);
            return ExecuteSQlCommand(SQl).FirstOrDefault();
        }

        /// <summary>
        /// 查看料品各版本盘库时统计工序
        /// </summary>
        /// <returns></returns>
        public static List<TechRouteLists> FetchTechRouteCheckIsCountingProcess()
        {
            List<TechRouteLists> trList = new List<TechRouteLists>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string SQl = string.Format(@"select A.TR_ItemCode,C.II_Name,B.TRV_VersionCode,B.TRV_VersionName,A.TR_ProcessSequence,A.TR_ProcessCode,A.TR_ProcessName,A.TR_IsCountingProcess from  TechRoute as A left join TechRouteVersion as B on (A.TR_VersionID = B.ID) left join ItemInfo as C on (A.TR_ItemID = C.ID)  where A.TR_IsCountingProcess = 1 order by A.TR_ItemCode,B.TRV_VersionCode,A.TR_ProcessSequence");

            MyDBController.GetConnection();
            dt = MyDBController.GetDataSet(SQl, ds, "TechRoute").Tables["TechRoute"];
            MyDBController.CloseConnection();

            foreach (DataRow dr in dt.Rows)
            {
                TechRouteLists tr = new TechRouteLists();
                tr.II_Name = dr["II_Name"].ToString();
                tr.TR_ItemCode = dr["TR_ItemCode"].ToString();
                tr.TRV_VersionCode = dr["TRV_VersionCode"].ToString();
                tr.TRV_VersionName = dr["TRV_VersionName"].ToString();
                tr.TR_ProcessSequence = Convert.ToInt32(dr["TR_ProcessSequence"].ToString());
                tr.TR_ProcessCode = dr["TR_ProcessCode"].ToString();
                tr.TR_ProcessName = dr["TR_ProcessName"].ToString();
                trList.Add(tr);
            }
            return trList;
        }

        /// <summary>
        /// 执行sql命令 
        /// </summary>
        /// <param name="_command"></param>
        /// <returns></returns>
        private static List<TechRouteLists> ExecuteSQlCommand(string _command)
        {
            List<TechRouteLists> trlList = new List<TechRouteLists>();
            DataSet ds = new DataSet();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(_command, ds, "TechRoute");
            MyDBController.CloseConnection();
            foreach (DataRow row in ds.Tables["TechRoute"].Rows)
            {
                TechRouteLists trl = new TechRouteLists();
                trl.ID = Convert.ToInt64(row["ID"]);
                trl.TR_ItemID = Convert.ToInt64(row["TR_ItemID"]);
                trl.TR_ItemCode = row["TR_ItemCode"].ToString();
                trl.II_Name = row["II_Name"].ToString();
                trl.TR_VersionID = Convert.ToInt64(row["TR_VersionID"]);
                trl.TR_IsTestProcess = Convert.ToBoolean(row["TR_IsTestProcess"]);
                trl.WC_Department_Name = row["WC_Department_Name"].ToString();
                trl.TR_WorkCenterID = Convert.ToInt64(row["TR_WorkCenterID"]);
                trl.TR_ProcessSequence = Convert.ToInt32(row["TR_ProcessSequence"]);
                trl.TR_ProcessName = row["TR_ProcessName"].ToString();
                trl.TR_ProcessCode = row["TR_ProcessCode"].ToString();
                trl.TR_ProcessID = Convert.ToInt64(row["TR_ProcessID"]);
                trl.TRV_VersionName = row["TRV_VersionName"].ToString();
                trl.TRV_VersionCode = row["TRV_VersionCode"].ToString();
                trl.TR_DefaultCheckPersonName = row["TR_DefaultCheckPersonName"].ToString();
                trl.TR_WorkHour = Convert.ToDecimal(row["TR_WorkHour"]);
                trl.TR_IsLastProcess = Convert.ToBoolean(row["TR_IsLastProcess"]);
                trl.TR_IsFirstProcess = Convert.ToBoolean(row["TR_IsFirstProcess"]);
                trl.PN_CodeInWorkCenter = row["PN_CodeInWorkCenter"].ToString();
                trl.TR_IsCountingProcess = row["TR_IsCountingProcess"] is DBNull ? false : Convert.ToBoolean(row["TR_IsCountingProcess"]);
                trl.TR_IsEquallyDivideProcess = row["TR_IsEquallyDivideProcess"] is DBNull ? true : Convert.ToBoolean(row["TR_IsEquallyDivideProcess"]);
                trlList.Add(trl);
            }
            return trlList;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="_trl"></param>
        /// <returns></returns>
        public static bool SaveInfo(TechRouteLists _trl)
        {
            List<TechRouteLists> _trlList = new List<TechRouteLists>() { _trl };
            return SaveInfo(_trlList);
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="_trlList"></param>
        /// <returns></returns>
        public static bool SaveInfo(List<TechRouteLists> _trlList)
        {
            DataSet ds = new DataSet();
            string SQl = string.Format("select top 0 * from [TechRoute]");
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "TechRoute");
            bool flag = MyDBController.InsertSqlBulk<TechRouteLists>(_trlList, ds.Tables["TechRoute"]);
            MyDBController.CloseConnection();
            return flag;
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="_trl"></param>
        public static void DeleteInfo(TechRouteLists _trl)
        {
            List<TechRouteLists> _trlList = new List<TechRouteLists>() { _trl };
            DeleteInfo(_trlList);
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="_trlList"></param>
        public static void DeleteInfo(List<TechRouteLists> _trlList)
        {
            MyDBController.GetConnection();
            string SQl = string.Format("select top 0 * from [TechRoute] ");
            DataSet ds = new DataSet();
            MyDBController.GetDataSet(SQl, ds, "TechRoute");
            string message = "";
            MyDBController.DeleteSqlBulk<TechRouteLists>(_trlList, ds.Tables["TechRoute"], out message);
        }


    }
}
