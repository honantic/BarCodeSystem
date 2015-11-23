using BarCodeSystem.PublicClass.HelperClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;

namespace BarCodeSystem.PublicClass.DatabaseEntity
{
    public class FlowCardSubLists : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public void NotifyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// 是否盘库时统计工序
        /// </summary>
        public bool TR_IsCountingProcess { get; set; }

        /// <summary>
        /// 料品版本编号
        /// </summary>
        public string TRV_VersionCode { get; set; }

        /// <summary>
        /// 料品版本名称
        /// </summary>
        public string TRV_VersionName { get; set; }
        /// <summary>
        /// 投入数量
        /// </summary>
        public int FC_Amount { get; set; }

        /// <summary>
        /// 应发工资
        /// </summary>
        public decimal TotalPayMount { get; set; }


        /// <summary>
        /// 实发工资
        /// </summary>
        public decimal FinalPayMount { get; set; }

        /// <summary>
        /// 流转卡编号
        /// </summary>
        public string FC_Code { get; set; }


        /// <summary>
        /// 流转卡ID
        /// </summary>
        public Int64 FC_ID { get; set; }


        /// <summary>
        /// 料品编码
        /// </summary>
        public string II_Code { get; set; }


        /// <summary>
        /// 料品名称
        /// </summary>
        public string II_Name { get; set; }


        /// <summary>
        /// 料品型号
        /// </summary>
        public string II_Spec { get; set; }



        /// <summary>
        /// 工时
        /// </summary>
        public decimal WH_WorkHour { get; set; }

        /// <summary>
        /// 主键，自增
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 主表id、
        /// </summary>
        public Int64 FCS_ItemId { get; set; }
        /// <summary>
        /// 料品id、
        /// </summary>
        public Int64 FCS_FlowCardID { get; set; }
        /// <summary>
        /// 工艺践线id、
        /// </summary>
        public Int64 FCS_TechRouteID { get; set; }
        /// <summary>
        /// 工序号id、
        /// </summary>
        public Int64 FCS_ProcessID { get; set; }
        /// <summary>
        /// 工序名称、
        /// </summary>
        public string FCS_ProcessName { get; set; }
        /// <summary>
        /// 人员编号
        /// </summary>
        public string FCS_PersonCode { get; set; }
        /// <summary>
        /// 人员姓名、
        /// </summary>
        public string FCS_PersonName { get; set; }

        private int fcs_BeginAmount;
        /// <summary>
        /// 工序投入数量
        /// </summary>
        public int FCS_BeginAmount
        {
            get { return fcs_BeginAmount; }
            set
            {
                if (value != fcs_BeginAmount)
                {
                    fcs_BeginAmount = value;
                    NotifyChanged("FCS_BeginAmount");
                }
            }
        }


        private int fcs_QulifiedAmount;
        /// <summary>
        /// 合格数量、
        /// </summary>
        public int FCS_QulifiedAmount
        {
            get { return fcs_QulifiedAmount; }
            set
            {
                if (value != fcs_QulifiedAmount)
                {
                    fcs_QulifiedAmount = value;
                    NotifyChanged("FCS_QulifiedAmount");
                }
            }
        }

        private int fcs_ScrappedAmount;
        /// <summary>
        /// 报废数量、
        /// </summary>
        public int FCS_ScrappedAmount
        {
            get { return fcs_ScrappedAmount; }
            set
            {
                if (value != fcs_ScrappedAmount)
                {
                    fcs_ScrappedAmount = value;
                    NotifyChanged("FCS_ScrappedAmount");
                }
            }
        }

        private int fcs_AddAmount;
        /// <summary>
        /// 转序新增数量，默认0
        /// </summary>
        public int FCS_AddAmount
        {
            get { return fcs_AddAmount; }
            set
            {
                if (value != fcs_AddAmount)
                {
                    fcs_AddAmount = value;
                    NotifyChanged("FCS_AddAmount");
                }
            }
        }
        ///// <summary>
        ///// 责废数量
        ///// </summary>
        //public int FCS_ProcessScrap { get; set; }
        ///// <summary>
        ///// 料废数量
        ///// </summary>
        //public int FCS_ItemScrap { get; set; }
        ///// <summary>
        ///// 退回供方数量
        ///// </summary>
        //public int FCS_SendBackAmount { get; set; }
        /// <summary>
        /// 待处理数量
        /// </summary>
        public int FCS_UnprocessedAm { get; set; }
        /// <summary>
        /// 检验员ID，报工的时候填写质量信息的用户ID自动带过来
        /// </summary>
        public Int64 FCS_CheckByID { get; set; }
        /// <summary>
        /// 检验员姓名 ，报工的时候填写质量信息的用户姓名自动带过来
        /// </summary>
        public string FCS_CheckByName { get; set; }
        /// <summary>
        /// 计件数量、用来计算计件工资的，根据不同的工序类型有不同的算法，比如测试工序计件数量=投入数量，正常工序计件数量=合格数量，返工工序计件数量=0
        /// </summary>
        public int FCS_PieceAmount { get; set; }
        /// <summary>
        /// 计件除数、用来计算计件工资的，为该道工序加工的人数
        /// </summary>
        public int FCS_PieceDivNum { get; set; }
        ///// <summary>
        ///// 单件工资、(来自工艺路线表)
        ///// </summary>
        //public decimal FCS_WagePerPiece { get; set; }
        /// <summary>
        /// 是否首道工序、（来自工艺路线表）
        /// </summary>
        public bool FCS_IsFirstProcess { get; set; }
        /// <summary>
        /// 是否未道工序、（来自工艺路线表）
        /// </summary>
        public bool FCS_IsLastProcess { get; set; }
        /// <summary>
        /// 该道工序是否已经报工
        /// </summary>
        public bool FCS_IsReported { get; set; }
        /// <summary>
        /// 该道工序是否是以班组为单位派工的，默认否。  0:否 1:是  。为1的时候，流转卡打印的时候，只显示班组名称
        /// </summary>
        //public bool FCS_IsWorkTeam { get; set; }
        /// <summary>
        /// 如果该道工序为班组派工，则记录其班组id，没有则设置为-1
        /// </summary>
        //public Int64 FCS_WorkTeamID { get; set; }
        /// <summary>
        /// 来自工艺路线表，工序号
        /// </summary>
        public int FCS_ProcessSequanece { get; set; }
        /// <summary>
        /// 来自工艺路线表，工序编码
        /// </summary>
        public string FCS_ProcessCode { get; set; }
        /// <summary>
        /// 来自工序表，工序在车间的编号
        /// </summary>
        public string PN_CodeInWorkCenter { get; set; }

        /// <summary>
        /// 报工时间
        /// </summary>
        public DateTime FCS_ReportTime { get; set; }

        /// <summary>
        /// 根据流转卡主表id搜索行表信息，返回行表的列表
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public static List<FlowCardSubLists> FetchFCS_InfoByFC_Id(Int64 _id)
        {
            string SQl = string.Format(@"Select A.[ID],A.[FCS_FlowCardID],A.[FCS_ItemId],A.[FCS_TechRouteID],A.[FCS_ProcessID],A.[FCS_ProcessName],A.[FCS_PersonCode],A.[FCS_PersonName],A.[FCS_BeginAmount],A.[FCS_QulifiedAmount],A.[FCS_ScrappedAmount],A.[FCS_AddAmount],A.[FCS_UnprocessedAm],A.[FCS_CheckByID],A.[FCS_CheckByName],A.[FCS_PieceAmount],A.[FCS_PieceDivNum],A.[FCS_IsFirstProcess],A.[FCS_IsLastProcess],A.[FCS_IsReported],A.[FCS_ReportTime],B.[TR_ProcessSequence],B.[TR_ProcessCode],C.[PN_CodeInWorkCenter],D.[II_Name],D.[II_Code],D.[II_Spec],D.[II_Version],E.[FC_Code],F.[TRV_VersionCode],F.[TRV_VersionName] from [FlowCardSub] A left join [TechRoute] B on A.[FCS_TechRouteID]=B.[ID] left join [ProcessName] C on A.[FCS_ProcessID]=C.[ID] left join [ItemInfo] D  on A.[FCS_ItemId] = D.[ID] left join [FlowCard] E on A.[FCS_FlowCardID] = E.[ID] left join [TechRouteVersion] F on B.[TR_VersionID]=F.[ID]  where [FCS_FlowCardID]={0}", _id);
            return ExecuteSQlCommand(SQl);

        }

        /// <summary>
        /// 根据流转卡主表信息，搜索行表信息列表
        /// </summary>
        /// <param name="_fc"></param>
        /// <returns></returns>
        public static List<FlowCardSubLists> FetchFCS_InfoByFC(FlowCardLists _fc)
        {
            string SQl = string.Format(@"Select A.[ID],A.[FCS_FlowCardID],A.[FCS_ItemId],A.[FCS_TechRouteID],A.[FCS_ProcessID],A.[FCS_ProcessName],A.[FCS_PersonCode],A.[FCS_PersonName],A.[FCS_BeginAmount],A.[FCS_QulifiedAmount],A.[FCS_ScrappedAmount],A.[FCS_AddAmount],A.[FCS_UnprocessedAm],A.[FCS_CheckByID],A.[FCS_CheckByName],A.[FCS_PieceAmount],A.[FCS_PieceDivNum],A.[FCS_IsFirstProcess],A.[FCS_IsLastProcess],A.[FCS_IsReported],A.[FCS_ReportTime],B.[TR_ProcessSequence],B.[TR_ProcessCode],C.[PN_CodeInWorkCenter],D.[II_Name],D.[II_Code],D.[II_Spec],D.[II_Version],E.[FC_Code],F.[TRV_VersionCode],F.[TRV_VersionName] from [FlowCardSub] A left join [TechRoute] B on A.[FCS_TechRouteID]=B.[ID] left join [ProcessName] C on A.[FCS_ProcessID]=C.[ID] left join [ItemInfo] D  on A.[FCS_ItemId] = D.[ID] left join [FlowCard] E on A.[FCS_FlowCardID] = E.[ID] left join [TechRouteVersion] F on B.[TR_VersionID]=F.[ID] where [FCS_FlowCardID]={0}", _fc.ID);
            return ExecuteSQlCommand(SQl);
        }

        /// <summary>
        /// 根据参数返回行表信息
        /// </summary>
        /// <param name="start_time">起始时间</param>
        /// <param name="end_time">结束时间</param>
        /// <param name="dept_code">部门编码</param>
        /// <returns></returns>
        public static List<FlowCardSubLists> FetchFCS_Info(DateTime start_time, DateTime end_time, string dept_code)
        {
            DataSet ds = new DataSet();
            DataTable tb = new DataTable();
            List<FlowCardSubLists> fcsll = new List<FlowCardSubLists>();

            string SQl = string.Format(@"select F.II_Code,F.II_Name,F.II_Spec,A.FCS_ProcessName,A.FCS_QulifiedAmount,B.FC_Code,D.WH_WorkHour,B.ID as FC_ID,C.ID as TR_ID,A.FCS_ProcessID as ProcessID from FlowCardSub as A left join  FlowCard as B on (A.FCS_FlowCardID = B.ID) left join  TechRoute as C on (A.FCS_TechRouteID = C.ID) left join  WorkHour as D on (D.WH_TechRouteID = C.ID) left join WorkCenter as E on (E.WC_Department_ID = C.TR_WorkCenterID) left join ItemInfo as F on (F.ID = C.TR_ItemID) where E.WC_Department_Code ='{0}'and A.FCS_ReportTime >= '{1}'and A.FCS_ReportTime <='{2}'and D.ID = (select MAX(ID) from WorkHour as G  where G.WH_TechRouteID = C.ID and CONVERT(date, G.WH_StartDate)<=CONVERT(date, A.FCS_ReportTime)and CONVERT(date, G.WH_EndDate)>=CONVERT(date, A.FCS_ReportTime)) and A.FCS_IsReported = 1", dept_code, start_time.ToString("yyyy-MM-dd HH:mm:ss"), end_time.ToString("yyyy-MM-dd HH:mm:ss"));


            MyDBController.GetConnection();
            tb = MyDBController.GetDataSet(SQl, ds, "GiveSalaries").Tables["GiveSalaries"];
            MyDBController.CloseConnection();

            foreach (DataRow dr in tb.Rows)
            {
                FlowCardSubLists fcsl = new FlowCardSubLists();
                fcsl.FCS_FlowCardID = (Int64)dr["FC_ID"];
                fcsl.FCS_TechRouteID = (Int64)dr["TR_ID"];
                fcsl.FCS_ProcessID = (Int64)dr["ProcessID"];

                fcsl.FC_Code = dr["FC_Code"].ToString();
                fcsl.II_Code = dr["II_Code"].ToString();
                fcsl.II_Name = dr["II_Name"].ToString();
                fcsl.II_Spec = dr["II_Spec"].ToString();

                fcsl.FCS_ProcessName = dr["FCS_ProcessName"].ToString();
                fcsl.WH_WorkHour = decimal.Parse(dr["WH_WorkHour"].ToString());
                fcsl.FCS_QulifiedAmount = Convert.ToInt32(dr["FCS_QulifiedAmount"].ToString());

                fcsll.Add(fcsl);
            }
            return fcsll;
        }

        /// <summary>
        /// 根据参数返回投入产出信息
        /// </summary>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public static List<FlowCardSubLists> FetchFCS_IOInfo(DateTime start_time, DateTime end_time, string dept_code)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<FlowCardSubLists> fcslList = new List<FlowCardSubLists>();
            string SQl = string.Format(@"select B.FC_Code,B.FC_Amount,D.II_Code,D.II_Name,D.II_Spec,A.FCS_ProcessName,A.FCS_PersonName,C.TR_IsFirstProcess,C.TR_IsLastProcess,A.FCS_BeginAmount,A.FCS_QulifiedAmount  from FlowCardSub as A  left join FlowCard as B on (A.FCS_FlowCardID = B.ID) left join TechRoute as C on (A.FCS_TechRouteID = C.ID) left join ItemInfo as D on (C.TR_ItemID = D.ID) left join WorkCenter as E on (E.WC_Department_ID = C.TR_WorkCenterID) where  E.WC_Department_Code = '{0}' and A.FCS_ReportTime >='{1}' and A.FCS_ReportTime <='{2}'and B.FC_CardState = 5 and A.FCS_IsReported = 1", dept_code, start_time.ToString("yyyy-MM-dd HH:mm:ss"), end_time.ToString("yyyy-MM-dd HH:mm:ss"));

            MyDBController.GetConnection();
            dt = MyDBController.GetDataSet(SQl, ds, "InputOutput").Tables["InputOutput"];
            MyDBController.CloseConnection();

            foreach (DataRow dr in dt.Rows)
            {
                FlowCardSubLists fcsl = new FlowCardSubLists();
                fcsl.FC_Code = dr["FC_Code"].ToString();
                fcsl.FC_Amount = Int32.Parse(dr["FC_Amount"].ToString());
                fcsl.II_Code = dr["II_Code"].ToString();
                fcsl.II_Name = dr["II_Name"].ToString();
                fcsl.II_Spec = dr["II_Spec"].ToString();
                fcsl.FCS_ProcessName = dr["FCS_ProcessName"].ToString();
                fcsl.FCS_PersonName = dr["FCS_PersonName"].ToString();
                fcsl.FCS_IsFirstProcess = Convert.ToBoolean(dr["TR_IsFirstProcess"].ToString());
                fcsl.FCS_IsLastProcess = Convert.ToBoolean(dr["TR_IsLastProcess"].ToString());
                fcsl.FCS_BeginAmount = Int32.Parse(dr["FCS_BeginAmount"].ToString());
                fcsl.FCS_QulifiedAmount = Int32.Parse(dr["FCS_QulifiedAmount"].ToString());
                fcslList.Add(fcsl);
            }

            return fcslList;
        }


        /// <summary>
        /// 根据参数返回日报工信息
        /// </summary>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public static List<FlowCardSubLists> FetchFCS_DRInfo(DateTime start_time, DateTime end_time, string dept_code)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<FlowCardSubLists> fcslList = new List<FlowCardSubLists>();

            string SQl = string.Format(@"select B.FC_Code,F.TRV_VersionCode,F.TRV_VersionName,B.FC_Amount,D.II_Code,D.II_Name,D.II_Spec,A.FCS_ProcessName,A.FCS_PersonName,A.FCS_ReportTime,C.TR_IsFirstProcess,C.TR_IsLastProcess,A.FCS_QulifiedAmount from FlowCardSub as A left join FlowCard as B on (A.FCS_FlowCardID = B.ID)left join TechRoute as C on (A.FCS_TechRouteID = C.ID) left join ItemInfo as D on (C.TR_ItemID = D.ID) left join WorkCenter as E on (E.WC_Department_ID = C.TR_WorkCenterID)left join TechRouteVersion as F on(B.FC_ItemTechVersionID = F.ID) where E.WC_Department_Code = '{0}'and A.FCS_ReportTime >= '{1}'and A.FCS_ReportTime <='{2}'and B.FC_CardState = 5 and A.FCS_IsReported = 1", dept_code, start_time.ToString("yyyy-MM-dd HH:mm:ss"), end_time.ToString("yyyy-MM-dd HH:mm:ss"));

            MyDBController.GetConnection();
            dt = MyDBController.GetDataSet(SQl, ds, "DailyReport").Tables["DailyReport"];
            foreach (DataRow dr in dt.Rows)
            {
                FlowCardSubLists fcsl = new FlowCardSubLists();
                fcsl.FC_Code = dr["FC_Code"].ToString();
                fcsl.II_Code = dr["II_Code"].ToString();
                fcsl.II_Name = dr["II_Name"].ToString();
                fcsl.II_Spec = dr["II_Spec"].ToString();
                fcsl.TRV_VersionCode = dr["TRV_VersionCode"].ToString();
                fcsl.TRV_VersionName = dr["TRV_VersionName"].ToString();
                fcsl.FCS_ProcessName = dr["FCS_ProcessName"].ToString();
                fcsl.FCS_PersonName = dr["FCS_PersonName"].ToString();
                fcsl.FCS_IsFirstProcess = Convert.ToBoolean(dr["TR_IsFirstProcess"].ToString());
                fcsl.FCS_IsLastProcess = Convert.ToBoolean(dr["TR_IsLastProcess"].ToString());
                fcsl.FCS_ReportTime = DateTime.Parse(dr["FCS_ReportTime"].ToString());
                fcsl.FCS_QulifiedAmount = (Int32)dr["FCS_QulifiedAmount"];
                fcslList.Add(fcsl);
            }

            return fcslList;
        }

        /// <summary>
        /// 根据参数返回在制品工序完成信息
        /// </summary>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public static List<FlowCardSubLists> FetchFCS_SRInfo(DateTime start_time, DateTime end_time, string dept_code)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<FlowCardSubLists> fcslList = new List<FlowCardSubLists>();

            string SQl = string.Format(@"select B.FC_Code,B.FC_Amount,D.II_Code,D.II_Name,D.II_Spec,F.TRV_VersionCode,F.TRV_VersionName,C.TR_ProcessCode,C.TR_IsCountingProcess,C.TR_ProcessSequence,A.FCS_ProcessName,A.FCS_PersonName,C.TR_IsFirstProcess,C.TR_IsLastProcess,A.FCS_BeginAmount,A.FCS_QulifiedAmount,A.FCS_IsReported from FlowCardSub as A  left join FlowCard as B on (A.FCS_FlowCardID = B.ID)  left join TechRoute as C on (A.FCS_TechRouteID = C.ID) left join ItemInfo as D on (C.TR_ItemID = D.ID)  left join WorkCenter as E on (E.WC_Department_ID = C.TR_WorkCenterID) left join TechRouteVersion as F on (f.ID = B.FC_ItemTechVersionID) where E.WC_Department_Code = '{0}'and A.FCS_ReportTime >= '{1}' and A.FCS_ReportTime <='{2}' and B.FC_CardState = 1 and A.FCS_IsReported = 1", dept_code, start_time.ToString("yyyy-MM-dd HH:mm:ss"), end_time.ToString("yyyy-MM-dd HH:mm:ss"));

            MyDBController.GetConnection();
            dt = MyDBController.GetDataSet(SQl, ds, "StoreReport").Tables["StoreReport"];
            MyDBController.CloseConnection();

            foreach (DataRow dr in dt.Rows)
            {
                FlowCardSubLists fcsl = new FlowCardSubLists();
                fcsl.FC_Code = dr["FC_Code"].ToString();
                fcsl.FC_Amount = Int32.Parse(dr["FC_Amount"].ToString());
                fcsl.II_Code = dr["II_Code"].ToString();
                fcsl.II_Name = dr["II_Name"].ToString();
                fcsl.II_Spec = dr["II_Spec"].ToString();
                fcsl.TRV_VersionCode = dr["TRV_VersionCode"].ToString();
                fcsl.TRV_VersionName = dr["TRV_VersionName"].ToString();
                //fcsl.TR_ProcessSequence = Int32.Parse(dr["TR_ProcessSequence"].ToString());
                fcsl.FCS_ProcessSequanece = Int32.Parse(dr["TR_ProcessSequence"].ToString());
                fcsl.FCS_ProcessCode = dr["TR_ProcessCode"].ToString();
                fcsl.FCS_ProcessName = dr["FCS_ProcessName"].ToString();
                fcsl.FCS_PersonName = dr["FCS_PersonName"].ToString();
                fcsl.FCS_IsFirstProcess = Convert.ToBoolean(dr["TR_IsFirstProcess"].ToString());
                fcsl.FCS_IsLastProcess = Convert.ToBoolean(dr["TR_IsLastProcess"].ToString());
                fcsl.FCS_BeginAmount = (int)dr["FCS_BeginAmount"];
                fcsl.FCS_QulifiedAmount = (int)dr["FCS_QulifiedAmount"];
                fcsl.FCS_IsReported = Convert.ToBoolean(dr["FCS_IsReported"].ToString());
                //fcsl.TR_IsCountingProcess =      Convert.ToBoolean(dr["TR_IsCountingProcess"].ToString());
                fcsl.TR_IsCountingProcess = dr["TR_IsCountingProcess"] is DBNull ? false : Convert.ToBoolean(dr["TR_IsCountingProcess"].ToString());
                fcslList.Add(fcsl);
            }
            return fcslList;
        }




        /// <summary>
        /// 执行sql命令
        /// </summary>
        /// <param name="_command">sql命令</param>
        /// <returns></returns>
        public static List<FlowCardSubLists> ExecuteSQlCommand(string _command)
        {
            DataSet ds = new DataSet();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(_command, ds, "FlowCardSub");
            List<FlowCardSubLists> fcsls = new List<FlowCardSubLists>();
            foreach (DataRow item in ds.Tables["FlowCardSub"].Rows)
            {
                FlowCardSubLists fcs = new FlowCardSubLists();
                fcs.ID = Convert.ToInt64(item["ID"]);
                fcs.FCS_FlowCardID = Convert.ToInt64(item["FCS_FlowCardID"]);
                fcs.FCS_ItemId = Convert.ToInt64(item["FCS_ItemId"]);
                fcs.FCS_TechRouteID = Convert.ToInt64(item["FCS_TechRouteID"]);
                fcs.FCS_ProcessID = Convert.ToInt64(item["FCS_ProcessID"]);
                fcs.FCS_PersonName = item["FCS_PersonName"].ToString();
                fcs.FCS_BeginAmount = Convert.ToInt32(item["FCS_BeginAmount"]);
                fcs.FCS_QulifiedAmount = Convert.ToInt32(item["FCS_QulifiedAmount"]);
                fcs.FCS_ScrappedAmount = Convert.ToInt32(item["FCS_ScrappedAmount"]);
                fcs.FCS_AddAmount = Convert.ToInt32(item["FCS_AddAmount"]);
                fcs.FCS_UnprocessedAm = Convert.ToInt32(item["FCS_UnprocessedAm"]);
                fcs.FCS_CheckByID = Convert.ToInt64(item["FCS_CheckByID"]);
                fcs.FCS_CheckByName = item["FCS_CheckByName"].ToString();
                fcs.FCS_PieceAmount = Convert.ToInt32(item["FCS_PieceAmount"]);
                fcs.FCS_PieceDivNum = Convert.ToInt32(item["FCS_PieceDivNum"]);
                fcs.FCS_IsFirstProcess = Convert.ToBoolean(item["FCS_IsFirstProcess"]);
                fcs.FCS_IsLastProcess = Convert.ToBoolean(item["FCS_IsLastProcess"]);
                fcs.FCS_IsReported = Convert.ToBoolean(item["FCS_IsReported"]);
                fcs.FCS_ProcessSequanece = item["TR_ProcessSequence"] is DBNull ? -1 : Convert.ToInt32(item["TR_ProcessSequence"]);
                fcs.FCS_ProcessCode = item["TR_ProcessCode"] is DBNull ? "" : item["TR_ProcessCode"].ToString();
                fcs.FCS_ProcessName = item["FCS_ProcessName"].ToString();
                fcs.PN_CodeInWorkCenter = item["PN_CodeInWorkCenter"].ToString();
                fcs.FCS_ProcessName = item["FCS_ProcessName"].ToString();
                fcs.FCS_PersonCode = item["FCS_PersonCode"].ToString();
                fcs.FCS_ReportTime = item["FCS_ReportTime"] is DBNull ? DateTime.Now : Convert.ToDateTime(item["FCS_ReportTime"]);
                fcs.II_Code = item["II_Code"].ToString();
                fcs.II_Name = item["II_Name"].ToString();
                fcs.II_Spec = item["II_Spec"].ToString();
                fcs.TRV_VersionCode = item["TRV_VersionCode"].ToString();
                fcs.TRV_VersionName = item["TRV_VersionName"].ToString();
                fcsls.Add(fcs);
            }
            MyDBController.CloseConnection();
            fcsls.Sort(new ListComparer<FlowCardSubLists>((p1, p2) => p1.FCS_ProcessSequanece.CompareTo(p2.FCS_ProcessSequanece)));
            return fcsls;
        }

        /// <summary>
        /// 快速保存数据
        /// </summary>
        /// <param name="_fcsls"></param>
        public static bool SaveFCSInfo(List<FlowCardSubLists> _fcsls, bool _showInfo = true)
        {
            try
            {
                string SQl = "select top 0 * from [FlowCardSub]";
                DataSet ds = new DataSet();
                int updateNum, insertNum;
                MyDBController.GetConnection();
                MyDBController.GetDataSet(SQl, ds, "FlowCardSub");
                MyDBController.AutoUpdateInsert(ds.Tables["FlowCardSub"], _fcsls, out updateNum, out insertNum);
                if (_showInfo)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("保存成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                MyDBController.CloseConnection();
                return true;
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// 删除行表数据
        /// </summary>
        /// <param name="_fcsl"></param>
        /// <returns></returns>
        public static bool DeleteFCSInfo(FlowCardSubLists _fcsl)
        {
            try
            {
                if (_fcsl != null && _fcsl.ID >= 0)
                {
                    string SQl = string.Format("Delete from [FlowCardSub] where [ID]={0}", _fcsl.ID);
                    MyDBController.GetConnection();
                    int count = MyDBController.ExecuteNonQuery(SQl);
                    MyDBController.CloseConnection();
                    if (count > 0)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("删除成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                    else
                        Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡行表信息不存在！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    {
                        return false;
                    }
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡行表信息存在错误！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// 删除行表数据
        /// </summary>
        /// <param name="_fcslList"></param>
        /// <returns></returns>
        public static bool DeleteFCSInfo(List<FlowCardSubLists> _fcslList)
        {
            string SQl = string.Format("select top 0 * from [FlowCardSub]");
            DataSet ds = new DataSet();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCardSub");
            string message = "";
            bool flag = MyDBController.DeleteSqlBulk<FlowCardSubLists>(_fcslList, ds.Tables["FlowCardSub"], out message);
            MyDBController.CloseConnection();
            if (!flag)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("删除成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return flag;
        }
    }
}
