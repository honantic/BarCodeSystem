using BarCodeSystem.PublicClass.DatabaseEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;

namespace BarCodeSystem
{
    public class ProcessNameLists
    {
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get;
            set;
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
        /// 工序编码
        /// </summary>
        public string PN_Code
        {
            get;
            set;
        }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string PN_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 序在车间里面的代码，用来打印流转卡的
        /// </summary>
        public string PN_CodeInWorkCenter
        {
            get;
            set;
        }

        /// <summary>
        /// 工序所属工作中心id
        /// </summary>
        public Int64 PN_WorkCenterID
        {
            get;
            set;
        }

        /// <summary>
        /// 与工序关联的质量问题原因的ID列表
        /// </summary>
        public string PN_AssociatedQI { get; set; }

        /// <summary>
        /// 与工序相关的质量问题原因ID的列表
        /// </summary>
        public List<Int64> PN_AssociatedQIList { get; set; }

        /// <summary>
        /// 将数据库中的qi列表转换成list
        /// </summary>
        /// <param name="_associatedQI"></param>
        /// <returns></returns>
        private static List<Int64> GetQIList(string _associatedQI)
        {
            List<Int64> _idList = new List<long>();
            try
            {
                _associatedQI.Split(',').ToList().ForEach(p => { _idList.Add(Convert.ToInt64(p)); });
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return _idList;
        }

        /// <summary>
        /// 将QI的ID list转换成string
        /// </summary>
        /// <param name="_idList"></param>
        /// <returns></returns>
        private static string GetQIListBack(List<Int64> _idList)
        {
            string id = "";
            _idList.ForEach(p => { id += string.IsNullOrEmpty(id) ? p.ToString() : "," + p.ToString(); });
            return id;
        }

        /// <summary>
        /// 根据车间id获取工序信息
        /// </summary>
        /// <param name="_wcID"></param>
        /// <returns></returns>
        public static List<ProcessNameLists> FetchPNInfoByWC(Int64 _wcID)
        {
            string SQl = string.Format("select * from [ProcessName] where [PN_WorkCenterID]={0}", _wcID);
            return GetPNInfoBySQl(SQl);
        }

        /// <summary>
        /// 执行sql命令，返回list
        /// </summary>
        /// <param name="_command"></param>
        /// <returns></returns>
        private static List<ProcessNameLists> GetPNInfoBySQl(string _command)
        {
            List<ProcessNameLists> pnlList = new List<ProcessNameLists>();
            MyDBController.GetConnection();
            DataTable dt = ExecuteSQlCommand(_command);
            MyDBController.CloseConnection();
            foreach (DataRow row in dt.Rows)
            {
                ProcessNameLists pnl = new ProcessNameLists();
                pnl.ID = Convert.ToInt64(row["ID"]);
                pnl.PN_Code = row["PN_Code"].ToString();
                pnl.PN_CodeInWorkCenter = row["PN_CodeInWorkCenter"].ToString();
                pnl.PN_Name = row["PN_Name"].ToString();
                pnl.PN_WorkCenterID = Convert.ToInt64(row["PN_WorkCenterID"]);
                pnl.PN_AssociatedQI = row["PN_AssociatedQI"].ToString();
                if (!string.IsNullOrEmpty(pnl.PN_AssociatedQI))
                {
                    pnl.PN_AssociatedQIList = GetQIList(pnl.PN_AssociatedQI);
                }
                pnlList.Add(pnl);
            }
            return pnlList;
        }

        /// <summary>
        /// 执行sql命令，返回dt
        /// </summary>
        /// <param name="_command"></param>
        /// <returns></returns>
        private static DataTable ExecuteSQlCommand(string _command)
        {
            DataSet ds = new DataSet();
            MyDBController.GetDataSet(_command, ds, "ProcessName");
            return ds.Tables["ProcessName"];
        }

        /// <summary>
        /// 保存工序信息
        /// </summary>
        /// <param name="_pnlList"></param>
        /// <returns></returns>
        public static bool SaveInfo(List<ProcessNameLists> _pnlList)
        {
            bool flag = false;
            _pnlList.ForEach(p => { p.PN_AssociatedQI = GetQIListBack(p.PN_AssociatedQIList); });
            string SQl = "select top 0 * from [ProcessName]";
            MyDBController.GetConnection();
            DataTable dt = ExecuteSQlCommand(SQl);
            MyDBController.InsertSqlBulk<ProcessNameLists>(_pnlList, dt);
            MyDBController.CloseConnection();
            return flag;
        }

        /// <summary>
        /// 根据流转卡字表获取工序信息
        /// </summary>
        /// <param name="_fcs"></param>
        public static ProcessNameLists FetchPNInfoByFCS(FlowCardSubLists _fcs)
        {
            string SQl = string.Format("select * from [ProcessName] A where ID=(select [FCS_ProcessID] from [FlowCardSub] B where B.[ID]={0})", _fcs.ID);
            return GetPNInfoBySQl(SQl).FirstOrDefault();
        }

        /// <summary>
        /// 根据工艺路线信息获取工序信息
        /// </summary>
        /// <param name="_trl"></param>
        /// <returns></returns>
        public static ProcessNameLists FetchPNInfoByTRL(TechRouteLists _trl)
        {
            string SQl = string.Format("select * from [ProcessName] A where ID=(select [TR_ProcessID] from [TechRoute] B where B.[ID]={0})", _trl.ID);
            return GetPNInfoBySQl(SQl).FirstOrDefault();
        }
    }
}
