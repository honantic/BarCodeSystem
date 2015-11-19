using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BarCodeSystem
{
    public class QualityIssuesLists
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID
        {
            get;
            set;
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get;
            set;
        }

        /// <summary>
        /// 质量问题信息编码
        /// </summary>
        public string QI_Code
        {
            get;
            set;
        }

        /// <summary>
        /// 质量问题信息Name
        /// </summary>
        public string QI_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 质量问题信息条码
        /// </summary>
        public string QI_BarCode
        {
            get;
            set;
        }

        /// <summary>
        /// 质量问题信息所属工作中心U9id
        /// </summary>
        public Int64 QI_WorkCenterID
        {
            get;
            set;
        }

        /// <summary>
        /// 质量问题类型 0:责废 1:料废 2:返工
        /// </summary>
        public int QI_Type { get; set; }
        /// <summary>
        /// 获取条码系统中质量问题信息清单
        /// </summary>
        /// <param name="_workCenterID">所属工作中心id，不填则会获取所有工作中心的质量问题信息</param>
        /// <returns></returns>
        public static List<QualityIssuesLists> FetchBCSQualityIssueInfo(Int64 _workCenterID = -1)
        {
            string SQl = "";


            if (_workCenterID == -1)
            {
                SQl = @"SELECT * FROM [QualityIssue]";
            }
            else
            {
                SQl = string.Format(@"Select * from [QualityIssue] where [QI_WorkCenterID]={0}", _workCenterID);
            }
            return ExecuteSQlCommand(SQl);
        }

        /// <summary>
        /// 根据工序获取质量问题信息
        /// </summary>
        /// <param name="_pnl"></param>
        /// <returns></returns>
        public static List<QualityIssuesLists> FetchBCSQIInfoByPNL(ProcessNameLists _pnl)
        {
            List<QualityIssuesLists> qilList = new List<QualityIssuesLists>();
            if (!string.IsNullOrEmpty(_pnl.PN_AssociatedQI))
            {
                string SQl = "";
                SQl = string.Format("Select * from [QualityIssue] where [Id] in ({0})", _pnl.PN_AssociatedQI);
                qilList = ExecuteSQlCommand(SQl);
            }
            return qilList;
        }

        /// <summary>
        /// 执行SQl命令
        /// </summary>
        /// <param name="_command"></param>
        /// <returns></returns>
        private static List<QualityIssuesLists> ExecuteSQlCommand(string _command)
        {
            MyDBController.GetConnection();
            List<QualityIssuesLists> qilList = new List<QualityIssuesLists>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            MyDBController.GetDataSet(_command, ds, "QualityIssue");
            dt = ds.Tables["QualityIssue"];
            int x = dt.Rows.Count;
            for (int i = 0; i < x; i++)
            {
                QualityIssuesLists qil = new QualityIssuesLists();
                qil.ID = (Int64)dt.Rows[i]["ID"];
                qil.QI_Code = dt.Rows[i]["QI_Code"].ToString();
                qil.QI_Name = dt.Rows[i]["QI_Name"].ToString();
                qil.QI_BarCode = dt.Rows[i]["QI_BarCode"].ToString();
                qil.QI_WorkCenterID = Convert.ToInt64(dt.Rows[i]["QI_WorkCenterID"]);
                qil.QI_Type = dt.Rows[i]["QI_Type"] is DBNull ? 0 : Convert.ToInt32(dt.Rows[i]["QI_Type"]);
                qilList.Add(qil);
            }
            qilList = qilList.OrderBy(p => p.QI_Code).ToList();
            MyDBController.CloseConnection();
            return qilList;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="_qil"></param>
        /// <returns></returns>
        public static bool SaveInfo(QualityIssuesLists _qil)
        {
            return SaveInfo(new List<QualityIssuesLists>() { _qil });
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="_qilList"></param>
        /// <returns></returns>
        public static bool SaveInfo(List<QualityIssuesLists> _qilList)
        {
            string SQl = string.Format("select top 0 * from [QualityIssue]");
            DataSet ds = new DataSet();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "QualityIssue");
            bool flag = MyDBController.InsertSqlBulk<QualityIssuesLists>(_qilList, ds.Tables["QualityIssue"]);
            MyDBController.CloseConnection();
            return flag;
        }
    }
}
