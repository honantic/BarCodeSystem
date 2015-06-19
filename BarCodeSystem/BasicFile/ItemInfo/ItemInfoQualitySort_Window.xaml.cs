using BarCodeSystem.PublicClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;

namespace BarCodeSystem.BasicFile.ItemInfo
{
    /// <summary>
    /// ItemInfoQualitySort_Window.xaml 的交互逻辑
    /// </summary>
    public partial class ItemInfoQualitySort_Window : Window
    {
        List<QualitySortList> qsls = new List<QualitySortList> { };

        DataTable dt = new DataTable();
        DataSet ds = new DataSet();

        public string chose_code;
        public string chose_name;
        public Int64 chose_id;

        public ItemInfoQualitySort_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选择按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Chose_Click(object sender, RoutedEventArgs e)
        {

            chose_code = "";
            chose_name = "";
            chose_id = 0;
            int x = listview1.SelectedIndex;

            if (x > -1)
            {
                chose_code = ((QualitySortList)listview1.SelectedItem).QS_Code;
                chose_name = ((QualitySortList)listview1.SelectedItem).QS_Name;
                chose_id = ((QualitySortList)listview1.SelectedItem).ID;
                this.DialogResult = true;
            }
            else
            {
            
            }
        }
        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //这段代码在正式程序中将被注释掉，测试用
            MyDBController.Server = User_Info.server[1];
            MyDBController.Database = User_Info.database[1];
            MyDBController.Uid = User_Info.uid[1];
            MyDBController.Pwd = User_Info.pwd[1];

            GetQualitySortList();

            listview1.ItemsSource = null;
            listview1.ItemsSource = qsls;
        }

        private void GetQualitySortList()
        {
            MyDBController.GetConnection();
            string SQl = "SELECT [ID], [QS_Code],[QS_Name] FROM [QualitySort]";
            dt = MyDBController.GetDataSet(SQl, ds).Tables[0];
            MyDBController.CloseConnection();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                QualitySortList qsl = new QualitySortList();
                qsl.QS_Code = dt.Rows[i]["QS_Code"].ToString();
                qsl.QS_Name = dt.Rows[i]["QS_Name"].ToString();
                qsl.ID = (Int64)dt.Rows[i]["ID"];
                qsls.Add(qsl);
            }

            
        }
        /// <summary>
        /// listview1鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview1_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btn_Chose_Click(sender, e);
        }
    }
}
