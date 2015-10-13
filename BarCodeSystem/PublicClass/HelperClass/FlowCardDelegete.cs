using BarCodeSystem.PublicClass;
using BarCodeSystem.PublicClass.DatabaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem.ProductDispatch.FlowCard
{
    class FlowCardDelegete
    {
    }

    /// <summary>
    /// 接受料品查询函数的委托
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public delegate string SubmitItemInfo(string value);

    /// <summary>
    /// 接收工艺路线查询函数的委托
    /// </summary>
    /// <param name="value"></param>
    /// <param name="trls"></param>
    public delegate void SubmitTechRouteInfo(string value, List<TechRouteLists> trls);

    /// <summary>
    ///  接收生产订单查询函数的委托
    /// </summary>
    /// <param name="pol"></param>
    public delegate void SubmitProduceOrderInfo(ProduceOrderLists pol);

    /// <summary>
    /// 接收人员信息的委托
    /// </summary>
    /// <param name="person"></param>
    public delegate void SubmitPersonInfo(List<PersonLists> person);

    /// <summary>
    /// 接收班组信息的委托
    /// </summary>
    /// <param name="wtl"></param>
    public delegate void SubmitWorkTeamInfo(WorkTeamLists wtl, List<WorkTeamMemberLists> wtmList);
    /// <summary>
    /// 保存派工方案之后清除主界面的信息
    /// </summary>
    public delegate void ClearDisPlanList();

    /// <summary>
    /// 流转卡选取派工方案的委托
    /// </summary>
    /// <param name="list"></param>
    public delegate void FillDisPlan(List<DisPlanLists> list);

    /// <summary>
    /// 流转卡报工的时候选取流转卡的委托
    /// </summary>
    /// <param name="list"></param>
    /// <param name="tv"></param>
    public delegate void SubmitFlowCard(FlowCardLists fc, List<FlowCardSubLists> fcslist, TechVersion tv);

    /// <summary>
    /// 流转卡审核的时候，修改数据接收修改后流转卡行表信息的委托函数
    /// </summary>
    /// <param name="fcslist"></param>
    public delegate void SubmitFlowCardSub(List<FlowCardSubLists> fcslist);

    /// <summary>
    /// 流转卡报工的时候选取质量问题信息的委托
    /// </summary>
    /// <param name="qil"></param>
    public delegate void SubmitQualityIssue(QualityIssuesLists qil);

    /// <summary>
    /// 流转卡报工的时候，不间断扫描选取质量问题信息
    /// </summary>
    /// <param name="qilList"></param>
    public delegate void SubmitQualityIssueList(List<QualityIssuesLists> qilList);

    /// <summary>
    /// 完工报告搜索的委托
    /// </summary>
    /// <param name="frl"></param>
    public delegate void SubmitFinishReport(FinishReportList frl);

    /// <summary>
    /// 缴费单委托
    /// </summary>
    /// <param name="srl"></param>
    public delegate void SubmitScrapReport(ScrapReportList srl);

    /// <summary>
    /// 流转卡分批数据接收的委托函数
    /// </summary>
    public delegate void SubmitDistributeFCInfo(int _fisrtSetCount, int _firstSetAmount, int _secSetCount, int _secSetAmount);

    /// <summary>
    /// 质量奖赔委托
    /// </summary>
    /// <param name="qmsList"></param>
    public delegate void SubmitQualitySalary(List<QualityMonthlySalaryList> qmsList);

    /// <summary>
    /// 接收流转卡报废信息的委托函数list
    /// </summary>
    /// <param name="fcqlList"></param>
    public delegate void SubmitFlowCardQualitiesList(List<FlowCardQualityLists> fcqlList);
}
