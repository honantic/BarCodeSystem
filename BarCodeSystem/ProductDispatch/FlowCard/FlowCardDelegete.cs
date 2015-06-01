using BarCodeSystem.PublicClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodeSystem.ProductDispatch.FlowCard
{
    class FlowCardDelegete
    {
    }

    ///接受料品查询函数的委托
    public delegate string SubmitItemInfo(string value);

    ///接收工艺路线查询函数的委托
    public delegate void SubmitTechRouteInfo(string value,List<TechRouteLists> trls);

    ///接收生产订单查询函数的委托
    public delegate void SubmitProduceOrderInfo(ProduceOrderLists pol);

    /// <summary>
    /// 接收人员信息的委托
    /// </summary>
    /// <param name="person"></param>
    public delegate void SubmitPersonInfo(PersonLists person);
}
