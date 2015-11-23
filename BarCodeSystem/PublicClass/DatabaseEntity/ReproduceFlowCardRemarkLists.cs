using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;

namespace BarCodeSystem.PublicClass.DatabaseEntity
{
    public class ReproduceFlowCardRemarkLists
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Int64 ID { get; set; }

        /// <summary>
        /// 返工流转卡的id
        /// </summary>
        public Int64 RFCR_FlowCardID { get; set; }

        /// <summary>
        /// 流转卡质量问题id
        /// </summary>
        public Int64 RFCR_FlowCardQualityID { get; set; }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="_rfcrList"></param>
        /// <returns></returns>
        public static bool SaveInfo(List<ReproduceFlowCardRemarkLists> _rfcrList)
        {
            bool flag = false;
            DataSet ds = new DataSet();
            string SQl = string.Format("select top 0 * from [ReproduceFlowCardRemark]");
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "ReproduceFlowCardRemark");
            flag = MyDBController.InsertSqlBulk<ReproduceFlowCardRemarkLists>(_rfcrList, ds.Tables["ReproduceFlowCardRemark"]);
            MyDBController.CloseConnection();
            if (flag)
            {
                //Xceed.Wpf.Toolkit.MessageBox.Show("保存成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("保存失败！请重试！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return flag;
        }
    }
}
