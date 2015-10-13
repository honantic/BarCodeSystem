using BarCodeSystem.PublicClass.DatabaseEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BarCodeSystem.SalaryManage.QualityMonthlySalary
{
    /// <summary>
    /// QualityMonthlySalary_Page.xaml 的交互逻辑
    /// </summary>
    public partial class QualityMonthlySalary_Page : Page
    {
        public QualityMonthlySalary_Page()
        {
            InitializeComponent();
        }


        List<QualityMonthlySalaryList> localList = new List<QualityMonthlySalaryList>();
        DataTable templateDT = new DataTable();
        int loadCount = 0;

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                templateDT = InitTemplateBT();
                loadCount++;
            }
        }

        /// <summary>
        /// 初始化导入导出dt格式
        /// </summary>
        /// <returns></returns>
        private DataTable InitTemplateBT()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("车间编号", typeof(string)));
            dt.Columns.Add(new DataColumn("车间名称", typeof(string)));
            dt.Columns.Add(new DataColumn("员工编号", typeof(string)));
            dt.Columns.Add(new DataColumn("员工姓名", typeof(string)));
            dt.Columns.Add(new DataColumn("质量奖赔", typeof(decimal)));
            dt.Columns.Add(new DataColumn("杂工工资", typeof(decimal)));
            dt.Columns.Add(new DataColumn("生效日期", typeof(DateTime)));
            return dt;
        }

        /// <summary>
        /// 新增奖赔单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddNew_Click(object sender, RoutedEventArgs e)
        {
            int flowNum = FetchQMSFlowNum();
            txtb_EffectiveTime.Text = DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month + "月";
            txtb_QMSCode.Text = DateTime.Now.Year.ToString() + string.Format("{0:00}", DateTime.Now.Month) + flowNum.ToString();
            txtb_CreateBy.Text = User_Info.User_Name;
            datagrid_QMSInfo.ItemsSource = null;
            localList.Clear();
            Xceed.Wpf.Toolkit.MessageBox.Show("新增成功！\r\n现在可以通过复制数据进行新增操作了！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 获取流水号
        /// </summary>
        /// <returns></returns>
        private int FetchQMSFlowNum()
        {
            int flowNum = 0;
            string SQl = string.Format(@"select max(QMSFC_Code) from [QMSFlowCode] where YEAR(QMSFC_EffectiveTime)='{0}' and MONTH(QMSFC_EffectiveTime)='{1}'", DateTime.Now.Year, DateTime.Now.Month);
            MyDBController.GetConnection();
            try
            {
                flowNum = Convert.ToInt32(MyDBController.ExecuteScalar(SQl));
            }
            catch (Exception)
            {
                flowNum = 1000;
            }
            flowNum++;
            SQl = string.Format(@"insert into [QMSFlowCode](QMSFC_Code,QMSFC_EffectiveTime) values({0},'{1}')", flowNum, DateTime.Now);
            MyDBController.ExecuteNonQuery(SQl);
            return flowNum;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_QMSInfo.HasItems)
            {
                if (Xceed.Wpf.Toolkit.MessageBox.Show("确定要删除吗?", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DeleteQMSInfo(txtb_QMSCode.Text);
                }
            }
        }

        /// <summary>
        /// 根据QMS编号删除信息
        /// </summary>
        /// <param name="_QMSCode"></param>
        private void DeleteQMSInfo(string _QMSCode)
        {
            string SQl = string.Format(@"Delete from [QualityMonthlySalary] where [QMS_Code]='{0}'", _QMSCode);
            MyDBController.GetConnection();
            MyDBController.ExecuteNonQuery(SQl);
            MyDBController.CloseConnection();
            datagrid_QMSInfo.ItemsSource = null;
            localList.Clear();
            Xceed.Wpf.Toolkit.MessageBox.Show("删除成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 查看质量奖赔列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ShowList_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            textb_SearchHeader.Visibility = Visibility.Visible;
            frame_SearchQMSInfo.Content = new QualityMonthlySalarySearch_Page(User_Info.User_Code, ShowQMSInfo);
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 展示选定的质量奖赔列表信息
        /// </summary>
        /// <param name="_qmsList"></param>
        private void ShowQMSInfo(List<QualityMonthlySalaryList> _qmsList)
        {
            this.Cursor = Cursors.Wait;
            localList = _qmsList;
            datagrid_QMSInfo.ItemsSource = _qmsList;
            datagrid_QMSInfo.Items.Refresh();
            QualityMonthlySalaryList item = _qmsList.FirstOrDefault();

            txtb_QMSCode.Text = item.QMS_Code;
            txtb_EffectiveTime.Text = item.QMS_EffectiveTime.Year.ToString() + "年" + item.QMS_EffectiveTime.Month.ToString() + "月";
            txtb_CreateBy.Text = item.QMS_CreateByName;
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_QMSInfo.HasItems)
            {
                bool flag = CheckForPersonCode(localList);
                if (flag)
                {
                    SaveInfoToDB(localList);
                }
            }
        }

        /// <summary>
        /// 检查当前月份，人员编号的信息是否唯一。是返回true，否返回false
        /// </summary>
        /// <param name="_personCode"></param>
        /// <returns></returns>
        private bool CheckForPersonCode(List<QualityMonthlySalaryList> _qmsList)
        {
            bool flag = true;
            string codeList = "";
            string existPersonCode = "";
            DataSet ds = new DataSet();
            foreach (QualityMonthlySalaryList item in _qmsList)
            {
                codeList += string.IsNullOrEmpty(codeList) ? item.QMS_PersonCode : "," + item.QMS_PersonCode;
            }
            string SQl = string.Format(@"select [QMS_PersonCode] from [QualityMonthlySalary] where MONTH([QMS_EffectiveTime])='{0}' and [QMS_PersonCode] in ('{1}')", DateTime.Now.Month, codeList);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "QMS_PersonCode");
            MyDBController.CloseConnection();
            if (ds.Tables["QMS_PersonCode"].Rows.Count == 0)
            {

            }
            else
            {
                flag = false;
                foreach (DataRow row in ds.Tables["QMS_PersonCode"].Rows)
                {
                    existPersonCode += string.IsNullOrEmpty(existPersonCode) ? row["QMS_PersonCode"].ToString() : "," + row["QMS_PersonCode"].ToString();
                }
                Xceed.Wpf.Toolkit.MessageBox.Show("当前月份如下人员已经存在质量奖赔信息：\r\n" + existPersonCode + "\r\n请不要重复导入！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return flag;
        }

        /// <summary>
        /// 当前信息保存 到数据库
        /// </summary>
        /// <param name="_qmsList"></param>
        private void SaveInfoToDB(List<QualityMonthlySalaryList> _qmsList)
        {
            string SQl = string.Format(@"select top 0 * from QualityMonthlySalary");
            DataSet ds = new DataSet();
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "QualityMonthlySalary");
            int updateNum, insertNum;
            MyDBController.AutoUpdateInsert<QualityMonthlySalaryList>(ds.Tables["QualityMonthlySalary"], _qmsList, out updateNum, out insertNum);
        }

        /// <summary>
        /// 导出模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Export_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            QkRowChangeToColClass qk = new QkRowChangeToColClass();
            qk.OutToExcel(templateDT);
            this.Cursor = Cursors.Arrow;
        }


        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ImportData_Click(object sender, RoutedEventArgs e)
        {
            string colList = "";
            DataTable dt = templateDT.Clone();
            foreach (DataColumn col in templateDT.Columns)
            {
                colList += string.IsNullOrEmpty(colList) ? col.ColumnName : "," + col.ColumnName;
            }
            QkRowChangeToColClass qk = new QkRowChangeToColClass();
            qk.write_excel_date_to_temp_table(dt, colList);
            if (dt.Rows.Count > 0)
            {
                localList.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    QualityMonthlySalaryList qmsl = new QualityMonthlySalaryList();
                    qmsl.QMS_Code = txtb_QMSCode.Text;
                    qmsl.QMS_CreateByCode = User_Info.User_Code;
                    qmsl.QMS_CreateByName = User_Info.User_Name;
                    qmsl.QMS_CreateOn = DateTime.Now;
                    qmsl.QMS_EffectiveTime = DateTime.Now;
                    qmsl.QMS_ModifyOn = DateTime.Now;
                    qmsl.QMS_ModifyBy = "";
                    qmsl.QMS_PersonCode = row["员工编号"].ToString();
                    qmsl.QMS_PersonName = row["员工姓名"].ToString();
                    qmsl.QMS_WorkerCenterCode = row["车间编号"].ToString();
                    qmsl.QMS_WorkerCenterName = row["车间名称"].ToString();
                    qmsl.QMS_QualityMoney = row["质量奖赔"] == null ? 0 : Convert.ToDecimal(row["质量奖赔"]);
                    qmsl.QMS_SundryMoney = row["杂工工资"] == null ? 0 : Convert.ToDecimal(row["杂工工资"]);
                    qmsl.QMS_HasCalculated = false;
                    qmsl.ID = -1;
                    localList.Add(qmsl);
                }
            }
            datagrid_QMSInfo.ItemsSource = localList;
        }
    }
}
