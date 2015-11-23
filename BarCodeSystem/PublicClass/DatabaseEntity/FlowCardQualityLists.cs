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
        public Int64 FCQ_QualityIssueID { get; set; }
        /// <summary>
        /// 报废数量
        /// </summary>
        public int FCQ_ScrapAmount { get; set; }
        /// <summary>
        /// 工序行号，来自工艺路线表
        /// </summary>
        public int TR_ProcessSequence { get; set; }

        /// <summary>
        /// 质量问题类型
        /// </summary>
        public int QI_Type { get; set; }


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

        /// <summary>
        /// 流转卡号
        /// </summary>
        public string FC_Code { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public string II_Code { get; set; }
        /// <summary>
        /// 品名
        /// </summary>
        public string II_Name { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string II_Spec { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string II_Version { get; set; }
        /// <summary>
        /// 流转卡主表id
        /// </summary>
        public Int64 FCS_FlowCardID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string FCQ_Remark { get; set; }

        /// <summary>
        /// 是否已经返工，针对可以返工的质量问题而言
        /// </summary>
        public bool FCQ_HasReproduced { get; set; }

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
            foreach (FlowCardSubLists p in _fcsList)
            {
                SQl = string.Format(@"select A.[ID],A.[FCQ_FlowCardSubID],A.[FCQ_QualityIssueID],A.[FCQ_ScrapAmount],B.[QI_Code],B.[QI_Name],B.[QI_Type],C.[TR_ProcessSequence],E.[FC_Code],F.[II_Name],F.[II_Code],F.[II_Spec],F.[II_Version],D.[FCS_FlowCardID],A.[FCQ_Remark],A.[FCQ_HasReproduced] from [FlowCardQuality] A left join [QualityIssue] B on A.[FCQ_QualityIssueID]=B.[ID] left join [FlowCardSub] D on A.[FCQ_FlowCardSubID]=D.[ID] left join [TechRoute] C on D.[FCS_TechRouteID]=C.[ID] left join [FlowCard] E on D.[FCS_FlowCardID]=E.[ID] left join [ItemInfo] F on D.[FCS_ItemID]=F.[ID] where [FCQ_FlowCardSubID]={0}", p.ID);
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
            string SQl = string.Format(@"select A.[ID],A.[FCQ_FlowCardSubID],A.[FCQ_QualityIssueID],A.[FCQ_ScrapAmount],C.[TR_ProcessSequence],D.[QI_Name],D.[QI_Code],D.[QI_Type],E.[FC_Code],F.[II_Name],F.[II_Code],F.[II_Spec],F.[II_Version],B.[FCS_FlowCardID],A.[FCQ_Remark],A.[FCQ_HasReproduced] from [FlowCardQuality] A left join [FlowCardSub] B on A.[FCQ_FlowCardSubID]=B.[ID] left join [TechRoute] C on B.[FCS_TechRouteID] = C.[ID] left join [QualityIssue] D on A.[FCQ_QualityIssueID] = D.[ID] left join [FlowCard] E on B.[FCS_FlowCardID]=E.[ID] left join [ItemInfo] F on B.[FCS_ItemID]=F.[ID] where A.[FCQ_FlowCardSubID] in ( select ID from [Flowcardsub] where [FCS_FlowCardID]={0})", _fc.ID);
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
                    fcql.FCQ_QualityIssueID = Convert.ToInt64(row["FCQ_QualityIssueID"]);
                    fcql.FCQ_ScrapAmount = Convert.ToInt32(row["FCQ_ScrapAmount"]);
                    fcql.ID = Convert.ToInt64(row["ID"]);
                    fcql.QI_Name = row["QI_Name"].ToString();
                    fcql.QI_Code = row["QI_Code"].ToString();
                    fcql.TR_ProcessSequence = Convert.ToInt32(row["TR_ProcessSequence"]);
                    fcql.QI_Type = row["QI_Type"] is DBNull ? 0 : Convert.ToInt32(row["QI_Type"]);
                    fcql.FC_Code = row["FC_Code"].ToString();
                    fcql.II_Code = row["II_Code"].ToString();
                    fcql.II_Name = row["II_Name"].ToString();
                    fcql.II_Spec = row["II_Spec"].ToString();
                    fcql.II_Version = row["II_Version"].ToString();
                    fcql.FCS_FlowCardID = Convert.ToInt64(row["FCS_FlowCardID"]);
                    fcql.FCQ_Remark = row["FCQ_Remark"] is DBNull ? "" : row["FCQ_Remark"].ToString();
                    fcql.FCQ_HasReproduced = row["FCQ_HasReproduced"] is DBNull ? false : Convert.ToBoolean(row["FCQ_HasReproduced"]);
                    fcqlList.Add(fcql);
                }
            }
            MyDBController.CloseConnection();
            return fcqlList;
        }

        /// <summary>
        /// 获取系统中可以用来返工的数据信息
        /// </summary>
        /// <param name="_year"></param>
        /// <param name="_month"></param>
        /// <returns></returns>
        public static List<FlowCardQualityLists> FetchReproduceFCQInfo(string _year, string _month)
        {
            string SQl = string.Format("select A.[ID],A.[FCQ_FlowCardSubID],A.[FCQ_QualityIssueID],A.[FCQ_ScrapAmount],B.[QI_Code],B.[QI_Name],B.[QI_Type],C.[TR_ProcessSequence],E.[FC_Code],F.[II_Name],F.[II_Code],F.[II_Spec],F.[II_Version],D.[FCS_FlowCardID],A.[FCQ_Remark],A.[FCQ_HasReproduced] from [FlowCardQuality] A left join [QualityIssue] B on A.[FCQ_QualityIssueID]=B.[ID] left join [FlowCardSub] D on A.[FCQ_FlowCardSubID]=D.[ID] left join [TechRoute] C on D.[FCS_TechRouteID]=C.[ID] left join [FlowCard] E on D.[FCS_FlowCardID]=E.[ID] left join [ItemInfo] F on D.[FCS_ItemID]=F.[ID] where B.[QI_Type]=2 and E.[FC_CardState]=5 and E.[FC_CanReproduce]='true' and E.[FC_HasReproduced]='false' and YEAR(E.[FC_CheckTime])={0} and Month(E.[FC_CheckTime])={1}", _year, _month);
            return ExecuteSQlCommand(SQl);
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

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="_fcsl"></param>
        /// <returns></returns>
        public static bool DeleteInfo(List<FlowCardSubLists> _fcslList)
        {
            bool flag = true;
            if (_fcslList.Count > 0)
            {
                try
                {
                    string _fcslIDList = "";
                    foreach (var item in _fcslList)
                    {
                        _fcslIDList += _fcslIDList.Length == 0 ? item.ID.ToString() : "," + item.ID;
                    }
                    string SQl = string.Format("delete from [flowcardquality] where FCQ_FlowCardSubID in ({0})", _fcslIDList);
                    MyDBController.GetConnection();
                    MyDBController.ExecuteNonQuery(SQl);
                    MyDBController.CloseConnection();
                }
                catch (Exception)
                {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 更改质量问题信息的流转卡行表id
        /// </summary>
        /// <param name="_tempList"></param>
        /// <param name="flowCardSubLists"></param>
        public static void ChangeFCSID(List<FlowCardSubLists> _tempList, FlowCardSubLists flowCardSubLists)
        {
            if (_tempList.Count > 0)
            {
                string _fcslID = "";
                _tempList.ForEach(p =>
                {
                    _fcslID += _fcslID.Length == 0 ? p.ID.ToString() : "," + p.ID.ToString();
                });
                string SQl = string.Format("update FlowCardQuality set [FCQ_FlowCardSubID]={0} where [FCQ_FlowCardSubID] in ({1})", flowCardSubLists.ID, _fcslID);
                MyDBController.GetConnection();
                MyDBController.ExecuteNonQuery(SQl);
                MyDBController.CloseConnection();
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="_fcqList"></param>
        /// <returns></returns>
        public static bool SaveInfo(List<FlowCardQualityLists> _fcqList)
        {
            string SQl = string.Format("select top 0 * from [FlowCardQuality]");
            DataSet ds = new DataSet();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "FlowCardQuality");
            bool flag = MyDBController.InsertSqlBulk<FlowCardQualityLists>(_fcqList, ds.Tables["FlowCardQuality"]);
            MyDBController.CloseConnection();
            return flag;
        }


    }
}
