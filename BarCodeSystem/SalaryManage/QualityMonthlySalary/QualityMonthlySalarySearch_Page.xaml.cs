using BarCodeSystem.ProductDispatch.FlowCard;
using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.PublicClass.HelperClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarCodeSystem.SalaryManage.QualityMonthlySalary
{
    /// <summary>
    /// QualityMonthlySalarySearch_Page.xaml 的交互逻辑
    /// </summary>
    public partial class QualityMonthlySalarySearch_Page : Page
    {
        public QualityMonthlySalarySearch_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public QualityMonthlySalarySearch_Page(string _userCode, SubmitQualitySalary _sqs)
        {
            InitializeComponent();
            userCode = _userCode;
            sqs = _sqs;
        }

        string userCode;
        int loadCount = 0;
        List<QualityMonthlySalaryList> qmsList;
        SubmitQualitySalary sqs;

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                datagrid_QMSCode.ItemsSource = FetchQMSInfo(userCode).Distinct(new ListComparer<QualityMonthlySalaryList>((x, y) => (x.QMS_Code.Equals(y.QMS_Code)))).Select(p => new QualityMonthlySalaryList { QMS_Code = p.QMS_Code });
                loadCount++;
            }
        }

        /// <summary>
        /// 获取当前的质量奖赔信息
        /// </summary>
        /// <returns></returns>
        private List<QualityMonthlySalaryList> FetchQMSInfo(string _userCode)
        {
            qmsList = new List<QualityMonthlySalaryList>();
            string SQl = "";
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(_userCode))
            {
                SQl = string.Format(@"select * from [QualityMonthlySalary]");
            }
            else
            {
                SQl = string.Format(@"Select * from [QualityMonthlySalary] where [QMS_HasCalculated]='{0}' and [QMS_CreateByCode]='{1}' and MONTH([QMS_CreateOn])='{2}'", false, _userCode, DateTime.Now.Month);
            }
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "QualityMonthlySalary");
            MyDBController.CloseConnection();

            foreach (DataRow row in ds.Tables["QualityMonthlySalary"].Rows)
            {
                QualityMonthlySalaryList qmsl = new QualityMonthlySalaryList();
                qmsl.ID = Convert.ToInt64(row["ID"]);
                qmsl.QMS_Code = row["QMS_Code"].ToString();
                qmsl.QMS_CreateByCode = row["QMS_CreateByCode"].ToString();
                qmsl.QMS_CreateByName = row["QMS_CreateByName"].ToString();
                qmsl.QMS_CreateOn = Convert.ToDateTime(row["QMS_CreateOn"]);
                qmsl.QMS_EffectiveTime = Convert.ToDateTime(row["QMS_EffectiveTime"]);
                qmsl.QMS_ModifyOn = Convert.ToDateTime(row["QMS_ModifyOn"]);
                qmsl.QMS_ModifyBy = row["QMS_ModifyBy"].ToString();
                qmsl.QMS_WorkerCenterCode = row["QMS_WorkerCenterCode"].ToString();
                qmsl.QMS_WorkerCenterName = row["QMS_WorkerCenterName"].ToString();
                qmsl.QMS_PersonCode = row["QMS_PersonCode"].ToString();
                qmsl.QMS_PersonName = row["QMS_PersonName"].ToString();
                qmsl.QMS_SundryMoney = Convert.ToDecimal(row["QMS_SundryMoney"]);
                qmsl.QMS_QualityMoney = Convert.ToDecimal(row["QMS_QualityMoney"]);
                qmsList.Add(qmsl);
            }
            return qmsList;
        }

        /// <summary>
        /// 选择项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_QMSCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            QualityMonthlySalaryList qms = (QualityMonthlySalaryList)datagrid_QMSCode.SelectedItem;
            List<QualityMonthlySalaryList> newList = qmsList.FindAll(p => p.QMS_Code.Equals(qms.QMS_Code));
            datagrid_QMSInfo.ItemsSource = newList;
        }


        /// <summary>
        /// 选定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Select_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (datagrid_QMSCode.SelectedIndex != -1)
            {
                QualityMonthlySalaryList qms = (QualityMonthlySalaryList)datagrid_QMSCode.SelectedItem;
                List<QualityMonthlySalaryList> newList = qmsList.FindAll(p => p.QMS_Code.Equals(qms.QMS_Code));
                sqs.Invoke(newList);
                MyDBController.FindVisualParent<QualityMonthlySalary_Page>(this).ForEach(p => { p.frame_SearchQMSInfo.Content = null; });
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_QMSCode_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btn_Select_Click(sender, e);
        }
    }
}
