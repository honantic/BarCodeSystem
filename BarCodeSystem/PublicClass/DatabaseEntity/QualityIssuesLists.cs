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
        /// 获取条码系统中质量问题信息清单
        /// </summary>
        /// <param name="_workCenterID">所属工作中心id，不填则会获取所有工作中心的质量问题信息</param>
        /// <returns></returns>
        public static List<QualityIssuesLists> FetchBCSQualityIssueInfo(Int64 _workCenterID = -1)
        {
            MyDBController.GetConnection();
            List<QualityIssuesLists> qilList = new List<QualityIssuesLists>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string SQl = "";


            if (_workCenterID == -1)
            {
                SQl = @"SELECT * FROM [QualityIssue]";
            }
            else
            {
                SQl = string.Format(@"Select * from [QualityIssue] where [QI_WorkCenterID]={0}", _workCenterID);
            }

            MyDBController.GetDataSet(SQl, ds, "QualityIssue");
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
                qilList.Add(qil);
            }
            qilList = qilList.OrderBy(p => p.QI_Code).ToList();
            MyDBController.CloseConnection();
            return qilList;
        }

    }
}
