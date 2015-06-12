using BarCodeSystem.PublicClass.DatabaseEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace BarCodeSystem.ProductDispatch.FlowCard
{
    /// <summary>
    /// DisPlanConfirm_Page2.xaml 的交互逻辑
    /// </summary>
    public partial class DisPlanConfirm_Page2 : Page
    {
        public DisPlanConfirm_Page2()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 保存方案的构造函数
        /// </summary>
        /// <param name="_disPlanVersionList"></param>
        public DisPlanConfirm_Page2(List<DisPlanLists> _disPlanList, List<DisPlanVersionLists> _disPlanVersionList, ClearDisPlanList _cdpl)
        {
            InitializeComponent();
            disPlanVersionList = _disPlanVersionList;
            disPlanList = _disPlanList;
            DependencyObject obj = this.Parent;
            cdpl = _cdpl;
        }

        List<DisPlanVersionLists> disPlanVersionList;
        List<DisPlanLists> disPlanList;
        DataSet ds = new DataSet();
        ClearDisPlanList cdpl;
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            Int64 id = SaveDisPlanVersion();
            bool flag = SaveDisPlan(id);
            ClosePage(flag);
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 保存派工方案版本
        /// </summary>
        /// <returns></returns>
        private Int64 SaveDisPlanVersion()
        {
            string SQl = string.Format(@"Insert into [DisPlanVersion]([DPV_VersionName],[DPV_ItemID],[DPV_TechRouteVersionID]) values( '{0}',{1},{2})", txtb_Name.Text, disPlanVersionList[0].DPV_ItemID, disPlanVersionList[0].DPV_TechRouteVersionID);
            MyDBController.GetConnection();
            MyDBController.ExecuteNonQuery(SQl);

            SQl = string.Format(@"Select [ID] from [DisPlanVersion] where [DPV_VersionName]='{0}' and [DPV_ItemID]={1} and [DPV_TechRouteVersionID]={2}", txtb_Name.Text, disPlanVersionList[0].DPV_ItemID, disPlanVersionList[0].DPV_TechRouteVersionID);
            SqlDataReader reader = MyDBController.GetDataReader(SQl);
            Int64 versionID = 0;
            while (reader.Read())
            {
                versionID = Convert.ToInt64(reader[0]);
            }
            return versionID;
        }

        /// <summary>
        /// 保存派工方案详细
        /// </summary>
        /// <param name="versionID"></param>
        private bool SaveDisPlan(Int64 versionID)
        {
            ds.Clear();
            string SQl = "Select top 0 [ID],[DP_TechRouteID],[DP_PersonID],[DP_DisPlanVersionID] from [DisPlan]";
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "DisPlan");

            List<string> colList = new List<string>();
            foreach (DataColumn col in ds.Tables["DisPlan"].Columns)
            {
                colList.Add(col.ColumnName);
            }

            foreach (DisPlanLists item in disPlanList)
            {
                DataRow row = ds.Tables["DisPlan"].NewRow();
                row["DP_TechRouteID"] = item.DP_TechRouteID;
                row["DP_PersonID"] = item.DP_PersonID;
                row["DP_DisPlanVersionID"] = versionID;
                ds.Tables["DisPlan"].Rows.Add(row);
            }
            ds.Tables["DisPlan"].Columns.Add(new DataColumn("IDNew", typeof(Int64)));

            int updataNum, insertNum;
            MyDBController.InsertSqlBulk(ds.Tables["DisPlan"], colList, out updataNum, out insertNum);
            MessageBox.Show("成功保存派工方案！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            return (insertNum + updataNum) == ds.Tables["DisPlan"].Rows.Count;
        }

        /// <summary>
        /// 保存完毕后关闭页面
        /// </summary>
        /// <param name="flag"></param>
        private void ClosePage(bool flag)
        {
            if (flag)
            {
                ((DisPlanConfirm_Window)MyDBController.FindVisualParent<DisPlanConfirm_Window>(this)[0]).Close();
                cdpl.Invoke();
            }
        }

        /// <summary>
        /// 输入派工方案名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WatermarkTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtb_Name.Text))
            {
                bool result = CheckIsExist(txtb_Name.Text);
                image_Yes.Visibility = result ? Visibility.Hidden : Visibility.Visible;
                btn_Save.IsEnabled = !result;
            }
            else
            {
                image_Yes.Visibility = Visibility.Hidden;
                btn_Save.IsEnabled = false;
            }
        }

        /// <summary>
        /// 检查当前名称是否在数据库中存在
        /// </summary>
        /// <returns></returns>
        private bool CheckIsExist(string key)
        {
            bool flag = false;
            try
            {
                string SQl = string.Format(@"Select count(*) from [DisPlanVersion] where [DPV_ItemID]={0} and [DPV_TechRouteVersionID]={1} and [DPV_VersionName]='{2}'", disPlanVersionList[0].DPV_ItemID, disPlanVersionList[0].DPV_TechRouteVersionID, key);
                MyDBController.GetConnection();
                SqlDataReader reader = MyDBController.GetDataReader(SQl);
                int count = 0;
                while (reader.Read())
                {
                    count = Convert.ToInt32(reader[0]);
                }
                reader.Close();
                MyDBController.CloseConnection();
                if (count > 0)
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
                flag = true;
            }
            return flag;
        }
    }
}
