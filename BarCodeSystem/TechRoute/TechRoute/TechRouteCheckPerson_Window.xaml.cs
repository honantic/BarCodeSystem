using BarCodeSystem.ProductDispatch.FlowCard;
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
        #region 构造函数
        /// <summary>
        /// 默认构造函数，选择检验员
        /// </summary>
        public TechRouteCheckPerson_Window()
        {
            InitializeComponent();
            key = "检验员";
            Title = "人员选择窗口";

        }

        /// <summary>
        /// 选择员工的构造函数
        /// </summary>
        /// <param name="_key"></param>
        public TechRouteCheckPerson_Window(string _key)
        {
            InitializeComponent();
            key = _key;
        }

        /// <summary>
        /// 扫描添加员工的构造函数
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_spi"></param>
        public TechRouteCheckPerson_Window(string _key, SubmitPersonInfo _spi)
        {
            InitializeComponent();
            key = _key;
            spi = _spi;
        }

        /// <summary>
        /// 扫描添加员工的构造函数
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_spi"></param>
        /// <param name="_selectType"></param>
        public TechRouteCheckPerson_Window(string _key, string _selectType, SubmitPersonInfo _spi)
        {
            InitializeComponent();
            key = _key;
            spi = _spi;
            SelectType = _selectType;
        }
        #endregion

        #region 变量
        /// <summary>
        /// 选中的检验员名称
        /// </summary>
        public string checkPersonName;
        /// <summary>
        /// 选中的检验员
        /// </summary>
        public PersonLists checkPerson;

        /// <summary>
        /// 选择方式 "手工","扫描"
        /// </summary>
        public string SelectType = "";
        /// <summary>
        /// 岗位
        /// </summary>
        string key = "";
        /// <summary>
        /// 系统中的员工列表
        /// </summary>
        List<PersonLists> pls;
        /// <summary>
        /// 扫描添加员工的委托函数
        /// </summary>
        SubmitPersonInfo spi;
        /// <summary>
        /// 扫描获得的员工列表
        /// </summary>
        List<PersonLists> scanPLList = new List<PersonLists>();
        #endregion

        #region 初始化
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (spi == null || SelectType == "手工")
            {
                listview1.ItemsSource = FetchCheckPerson();
                grid_Two.Visibility = Visibility.Hidden;
                txtb_SearchKey.Focus();
            }
            else
            {
                FetchCheckPerson();
                grid_One.Visibility = Visibility.Hidden;
                txtb_ScanKey.Focus();
            }
        }

        /// <summary>
        /// 获取检验员列表
        /// </summary>
        /// <returns></returns>
        private List<PersonLists> FetchCheckPerson()
        {
            pls = new List<PersonLists>();
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
        #endregion

        #region 手动选择人员
        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Select_Click(object sender, RoutedEventArgs e)
        {
            if (listview1.SelectedIndex != -1)
            {
                if (spi == null)
                {
                    checkPerson = (PersonLists)listview1.SelectedItem;
                    checkPersonName = ((PersonLists)listview1.SelectedItem).name;
                    this.DialogResult = true;
                }
                else
                {
                    List<PersonLists> plList = new List<PersonLists>() { (PersonLists)listview1.SelectedItem };
                    spi.Invoke(plList);
                    this.DialogResult = true;
                }
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
            btn_Select_Click(sender, e);
        }

        /// <summary>
        /// 搜索信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_SearchKey_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtb_SearchKey.Text))
            {
                listview1.ItemsSource = pls;
            }
            else
            {
                string key = txtb_SearchKey.Text;
                listview1.ItemsSource = pls.FindAll(p => p.name.IndexOf(key) != -1 || p.code.IndexOf(key) != -1);
            }
        }
        /// <summary>
        /// 拖动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        #endregion

        #region 扫描选择
        /// <summary>
        /// 扫描事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_ScanKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_ScanSearch_Click(null, null);
            }
        }

        /// <summary>
        /// 扫描
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ScanSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtb_ScanKey.Text))
            {
                string key = txtb_ScanKey.Text;
                scanPLList.AddRange(SearchPersonInfo(key));
                listview2.ItemsSource = scanPLList;
                listview2.Items.Refresh();
                txtb_ScanKey.Text = "";
                txtb_ScanKey.Focus();
            }
        }
        /// <summary>
        /// 搜索人员信息
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        private List<PersonLists> SearchPersonInfo(string _key)
        {
            List<PersonLists> plList = new List<PersonLists>();
            if (CheckKeyInWorkTeam(_key))
            {
                plList = SearchInWorkTeam(_key);
            }
            else
            {
                plList = SearchInPerson(_key);
            }
            if (plList.Count > 0)
            {
                plList = plList.FindAll(p => !scanPLList.Exists(item => item.code.Equals(p.code)));
            }
            return plList;
        }

        /// <summary>
        /// 在班组表中检查是否有当前扫描的编码
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        private bool CheckKeyInWorkTeam(string _key)
        {
            bool flag = false;
            flag = WorkTeamLists.CheckIfCodeExsist(_key);
            return flag;
        }


        /// <summary>
        /// 在班组里面搜索
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        private List<PersonLists> SearchInWorkTeam(string _key)
        {
            List<PersonLists> plList = WorkTeamLists.FetchPersonListByCode(_key);
            return plList;
        }

        /// <summary>
        /// 在人员里面搜索
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        private List<PersonLists> SearchInPerson(string _key)
        {
            List<PersonLists> plList = PersonLists.FetchPersonListByCode(_key);
            return plList;
        }


        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Ensure_Click(object sender, RoutedEventArgs e)
        {
            if (scanPLList.Count > 0)
            {
                spi.Invoke(scanPLList);
                this.DialogResult = true;
            }
        }
        #endregion
    }
}
