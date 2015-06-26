using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace BarCodeSystem.TechRoute.TechRoute
{
    /// <summary>
    /// TechRouteCheckPerson_Window.xaml 的交互逻辑
    /// </summary>
    public partial class TechRouteCheckPerson_Window : Window
    {
        public TechRouteCheckPerson_Window()
        {
            InitializeComponent();
            key = "检验员";
            Title = "人员选择窗口";

        }

        public TechRouteCheckPerson_Window(string _key)
        {
            InitializeComponent();
            key = _key;
        }

        #region 变量
        public string checkPersonName;
        public PersonLists checkPerson;
        string key = "";
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listview1.ItemsSource = FetchCheckPerson();
        }

        /// <summary>
        /// 获取检验员列表
        /// </summary>
        /// <returns></returns>
        private List<PersonLists> FetchCheckPerson()
        {
            List<PersonLists> pls = new List<PersonLists>();
            DataSet ds = new DataSet();
            string SQl = string.Format("Select [ID],[P_Name],[P_Code] from [Person] where [P_Position]='{0}'", key);
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "Person");
            MyDBController.CloseConnection();

            foreach (DataRow row in ds.Tables["Person"].Rows)
            {
                pls.Add(new PersonLists()
                {
                    ID = Convert.ToInt64(row["ID"]),
                    name = row["P_Name"].ToString(),
                    code = row["P_Code"].ToString()
                });
            }
            return pls;
        }

        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Select_Click(object sender, RoutedEventArgs e)
        {
            if (listview1.SelectedIndex != -1)
            {
                checkPerson = (PersonLists)listview1.SelectedItem;
                checkPersonName = ((PersonLists)listview1.SelectedItem).name;
                this.DialogResult = true;
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Point formPoint = e.GetPosition(this);
            if (formPoint.Y > 60 && formPoint.Y < 90)//表头部分不做响应
            {

            }
            else
            {
                btn_Select_Click(sender, e);
            }
        }



        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
