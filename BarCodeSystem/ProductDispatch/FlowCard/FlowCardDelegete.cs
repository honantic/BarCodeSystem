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
    /// 流转卡报工的时候选取质量问题信息的委托
    /// </summary>
    /// <param name="qil"></param>
    public delegate void SubmitQualityIssue(QualityIssuesLists qil);
}
