using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace BarCodeSystem.PublicClass.DatabaseEntity
{
    public class WorkHourLists
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public Int64 WH_TechRouteID { get; set; }

        /// <summary>
        /// 工时
        /// </summary>
        public decimal WH_WorkHour { get; set; }
        public string WorkHour { get { return WH_WorkHour.ToString("C"); } }
        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime WH_StartDate { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime WH_EndDate { get; set; }

        /// <summary>
        /// 工序名称，来自工艺路线表
        /// </summary>
        public string TR_ProcessName { get; set; }

        /// <summary>
        /// 工序行号，来自工艺路线表
        /// </summary>
        public int TR_ProcessSequence { get; set; }

        /// <summary>
        /// 检查工时数据的日期是否正确，保证各个工时版本能够衔接上，并且没有重复区域
        /// </summary>
        /// <param name="_whlList"></param>
        /// <returns></returns>
        public static bool CheckIfDateRight(List<WorkHourLists> _whlList)
        {
            bool flag = true;
            if (_whlList.Count > 0)
            {
                DataSet ds = new DataSet();
                string SQl = string.Format("select [WH_StartDate],[WH_EndDate] from [WorkHour] where [ID] =(select Max(ID) from [WorkHour] where [WH_TechRouteID]={0})", _whlList[0].WH_TechRouteID);
                MyDBController.GetConnection();
                MyDBController.GetDataSet(SQl, ds, "WorkHour");
                MyDBController.CloseConnection();
                if (ds.Tables["WorkHour"].Rows.Count > 0)
                {
                    if (_whlList[0].WH_StartDate.Date <= Convert.ToDateTime(ds.Tables["WorkHour"].Rows[0]["WH_StartDate"]).Date)
                    {
                        flag = false;
                        Xceed.Wpf.Toolkit.MessageBox.Show("开始时间\r\n早于上个版本工时的\r\n开始时间！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (_whlList[0].WH_StartDate.Date > Convert.ToDateTime(ds.Tables["WorkHour"].Rows[0]["WH_EndDate"]).AddDays(1).Date)
                    {
                        flag = false;
                        Xceed.Wpf.Toolkit.MessageBox.Show("开始时间\r\n比上个版本工时的\r\n结束时间\r\n晚一天以上！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 将指定的工时列表保存到数据库
        /// </summary>
        /// <param name="_whlList">原始数据列表</param>
        public static bool SaveWHInfo(List<WorkHourLists> _whlList)
        {
            try
            {
                bool flag = true;
                //将上个版本的工时的结束时间自动设为这个版本的开始时间前一天
                MyDBController.GetConnection();
                _whlList.ForEach(
                    _whl =>
                    {
                        string command = string.Format(@"update [WorkHour] set [WH_EndDate]='{0}' where [ID] =(select Max(ID) from [WorkHour] where [WH_TechRouteID]={1})", _whl.WH_StartDate.AddDays(-1).ToString("yyyy/MM/dd"), _whl.WH_TechRouteID);
                        MyDBController.ExecuteNonQuery(command);
                        //Trace.WriteLine(_whlList.IndexOf(_whl).ToString() + " |||| " + Task.CurrentId.ToString() + " |||| " + Thread.CurrentThread.ManagedThreadId.ToString());
                    });

                DataSet ds = new DataSet();
                int updateNum, insertNum;
                string SQl = string.Format(@"select top 0 * from [WorkHour]");
                MyDBController.GetDataSet(SQl, ds, "WorkHour");
                MyDBController.AutoUpdateInsert(ds.Tables["WorkHour"], _whlList, out updateNum, out insertNum);
                MyDBController.CloseConnection();
                Xceed.Wpf.Toolkit.MessageBox.Show("保存成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return flag;
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

        }

        /// <summary>
        /// 获取某个料品某个工艺版本的工时列表，包括历史数据
        /// </summary>
        /// <returns></returns>
        public static List<WorkHourLists> FetchWHInfo(string _itemCode, string _techVersionCode)
        {
            List<WorkHourLists> whlList = new List<WorkHourLists>();
            DataSet ds = new DataSet();
            string SQl = string.Format(@"select A.[ID],A.[WH_TechRouteID],A.[WH_WorkHour],A.[WH_StartDate],A.[WH_EndDate],B.[TR_ProcessSequence],B.[TR_ProcessName] from [WorkHour] A left join [TechRoute] B on A.[WH_TechRouteID] = B.[ID] where [WH_TechRouteID] in (select [ID] from [TechRoute] where [TR_VersionID] in (select [ID] from [TechRouteVersion] where [TRV_VersionCode]='{0}' and [TRV_ItemID] in (select ID from [ItemInfo] where [II_Code]='{1}')))", _techVersionCode, _itemCode);

            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "WorkHour");
            MyDBController.CloseConnection();

            foreach (DataRow row in ds.Tables["WorkHour"].Rows)
            {
                WorkHourLists whl = new WorkHourLists();
                whl.ID = Convert.ToInt64(row["ID"]);
                whl.TR_ProcessName = row["TR_ProcessName"].ToString();
                whl.TR_ProcessSequence = Convert.ToInt32(row["TR_ProcessSequence"]);
                whl.WH_StartDate = Convert.ToDateTime(row["WH_StartDate"]);
                whl.WH_EndDate = Convert.ToDateTime(row["WH_EndDate"]);
                whl.WH_WorkHour = Convert.ToDecimal(row["WH_WorkHour"]);
                whlList.Add(whl);
            }
            return whlList;
        }
    }
}
