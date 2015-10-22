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
    public class FlowCardQualityLists : INotifyPropertyChanged
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FlowCardQualityLists()
        {
            FCQ_FlowCardSubID = -1;
            FCQ_ScrapAmount = 0;
        }
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 流转卡子表id
        /// </summary>
        public Int64 FCQ_FlowCardSubID { get; set; }
        /// <summary>
        /// 质量id
        /// </summary>
        public Int64 FCQ_QulityIssueID { get; set; }
        /// <summary>
        /// 报废数量
        /// </summary>
        public int FCQ_ScrapAmount { get; set; }
        /// <summary>
        /// 工序行号，来自工艺路线表
        /// </summary>
        public int TR_ProcessSequence { get; set; }


        string qi_Code = "";
        /// <summary>
        /// 质量问题编码，从质量问题信息表中关联得到
        /// </summary>
        public string QI_Code
        {
            get
            {
                if (!string.IsNullOrEmpty(qi_Code))
                {
                    return qi_Code;
                }
                else
                    return "";
            }
            set
            {
                if (!qi_Code.Equals(value))
                {
                    qi_Code = value;
                    OnPropertyChanged("QI_Code");
                }
            }
        }
        string qi_Name = "";
        /// <summary>
        /// 质量问题名称，从质量问题信息表中关联得到
        /// </summary>
        public string QI_Name
        {
            get
            {
                if (!string.IsNullOrEmpty(qi_Name))
                {
                    return qi_Name;
                }
                else
                    return "";
            }
            set
            {
                if (!qi_Name.Equals(value))
                {
                    qi_Name = value;
                    OnPropertyChanged("QI_Name");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// 根据流转卡子表列表搜索相关的质量信息
        /// </summary>
        /// <param name="_fcsList"></param>
        /// <returns></returns>
        public static List<FlowCardQualityLists> FetchFCQLByFCSInfo(List<FlowCardSubLists> _fcsList)
        {
            string SQl = "";
            List<FlowCardQualityLists> fcqList = new List<FlowCardQualityLists>();
            DataSet ds = new DataSet();
            MyDBController.GetConnection();
            _fcsList.Distinct(new ListComparer<FlowCardSubLists>((p1, p2) => p1.FCS_TechRouteID.Equals(p2.FCS_TechRouteID)));
            foreach (FlowCardSubLists p in _fcsList)
            {
                SQl = string.Format(@"select A.[ID],A.[FCQ_FlowCardSubID],A.[FCQ_QulityIssueID],A.[FCQ_ScrapAmount],B.[QI_Code],B.[QI_Name],C.[TR_ProcessSequence] from [FlowCardQuality] A left join [QualityIssue] B on A.[FCQ_QulityIssueID]=B.[ID] left join [FlowCardSub] D on A.[FCQ_FlowCardSubID]=D.[ID] left join [TechRoute] C on D.[FCS_TechRouteID]=C.[ID] where [FCQ_FlowCardSubID]={0}", p.ID);
                fcqList.AddRange(ExecuteSQlCommand(SQl));
            }
            MyDBController.CloseConnection();
            return fcqList;
        }

        /// <summary>
        /// 根据流转卡主表信息搜索相关的质量问题信息
        /// </summary>
        /// <param name="_fc"></param>
        /// <returns></returns>
        public static List<FlowCardQualityLists> FetchFCQByFlowCardInfo(FlowCardLists _fc)
        {
            string SQl = string.Format(@"select A.[ID],A.[FCQ_FlowCardSubID],A.[FCQ_QulityIssueID],A.[FCQ_ScrapAmount],C.[TR_ProcessSequence],D.[QI_Name],D.[QI_Code] from [FlowCardQuality] A left join [FlowCardSub] B on A.[FCQ_FlowCardSubID]=B.[ID] left join [TechRoute] C on B.[FCS_TechRouteID] = C.[ID] left join [QualityIssue] D on A.[FCQ_QulityIssueID] = D.[ID] where A.[FCQ_FlowCardSubID] in ( select ID from [Flowcardsub] where [FCS_FlowCradID]={0})", _fc.ID);
            return ExecuteSQlCommand(SQl);
        }

        /// <summary>
        /// 执行sql命令
        /// </summary>
        /// <param name="_command"></param>
        /// <returns></returns>
        private static List<FlowCardQualityLists> ExecuteSQlCommand(string _command)
        {
            List<FlowCardQualityLists> fcqlList = new List<FlowCardQualityLists>();
            DataSet ds = new DataSet();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(_command, ds, "FlowCardQuality");
            if (ds.Tables["FlowCardQuality"].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables["FlowCardQuality"].Rows)
                {
                    FlowCardQualityLists fcql = new FlowCardQualityLists();
                    fcql.FCQ_FlowCardSubID = Convert.ToInt64(row["FCQ_FlowCardSubID"]);
                    fcql.FCQ_QulityIssueID = Convert.ToInt64(row["FCQ_QulityIssueID"]);
                    fcql.FCQ_ScrapAmount = Convert.ToInt32(row["FCQ_ScrapAmount"]);
                    fcql.ID = Convert.ToInt64(row["ID"]);
                    fcql.QI_Name = row["QI_Name"].ToString();
                    fcql.QI_Code = row["QI_Code"].ToString();
                    fcql.TR_ProcessSequence = Convert.ToInt32(row["TR_ProcessSequence"]);
                    fcqlList.Add(fcql);
                }
            }
            MyDBController.CloseConnection();
            return fcqlList;
        }

        /// <summary>
        /// 更新质量问题信息
        /// </summary>
        public static bool UpdateFCQInfo(List<FlowCardQualityLists> _fcqlList)
        {
            string SQl = "select top 0  * from [FlowCardQuality]";
            DataSet ds = new DataSet();
            int updateNum, insertNum;
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCardQuality");
            bool flag = MyDBController.AutoUpdateInsert(ds.Tables["FlowCardQuality"], _fcqlList, out updateNum, out insertNum);
            if (flag)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("保存成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return flag;
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="_fcqlList"></param>
        public static bool DeleteInfo(List<FlowCardQualityLists> _fcqlList, out string _message)
        {
            DataSet ds = new DataSet();
            string SQl = "select top 0 * from  [FlowCardQuality]";
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCardQuality");
            bool flag = MyDBController.DeleteSqlBulk<FlowCardQualityLists>(_fcqlList, ds.Tables["FlowCardQuality"], out _message);
            MyDBController.CloseConnection();
            return flag;
        }
    }
}
