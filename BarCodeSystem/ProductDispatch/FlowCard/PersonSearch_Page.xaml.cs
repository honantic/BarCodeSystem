using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BarCodeSystem.ProductDispatch.FlowCard
{
    /// <summary>
    /// PersonSearch_Page.xaml 的交互逻辑
    /// </summary>
    public partial class PersonSearch_Page : Page
    {
        List<PersonLists> person = new List<PersonLists>();
        SubmitPersonInfo spi;
        List<PersonLists> personOCL = new List<PersonLists>();
        DataSet ds = new DataSet();
        public PersonSearch_Page()
        {
            InitializeComponent();
        }
        public PersonSearch_Page(SubmitPersonInfo _spi)
        {
            InitializeComponent();
            spi = _spi;
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            datagrid_PersonInfo.ItemsSource = FetchPersonInfo();
        }

        private List<PersonLists> FetchPersonInfo()
        {
            string SQl = string.Format(@"Select * from [Person]");
            MyDBController.GetConnection();
            MyDBController.GetDataSet(SQl, ds, "Person");
            MyDBController.CloseConnection();

            foreach (DataRow row in ds.Tables["Person"].Rows)
            {
                personOCL.Add(new PersonLists()
                {
                    code = row["P_Code"].ToString(),
                    name = row["P_Name"].ToString(),
                });
            }
            return personOCL;
        }



        /// <summary>
        /// 选中人员信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            person.Clear();
            if (datagrid_PersonInfo.SelectedIndex > -1)
            {
                foreach (var item in datagrid_PersonInfo.SelectedItems)
                {
                    person.Add((PersonLists)item);
                }
                spi.Invoke(person);
            }

        }


        /// <summary>
        /// 搜索按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PersonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtb_PersonInfo.Text))
            {
                datagrid_PersonInfo.ItemsSource = personOCL;
            }
            else
            {
                string key = txtb_PersonInfo.Text.Trim();
                datagrid_PersonInfo.ItemsSource = personOCL.FindAll(p => p.name.IndexOf(key) != -1 || p.code.IndexOf(key) != -1);
            }
        }

        /// <summary>
        /// 回车事件关联搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_PersonInfo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_PersonSearch_Click(sender, e);
            }
        }
        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            datagrid_PersonInfo.ItemsSource = FetchPersonInfo();
            datagrid_PersonInfo.Items.Refresh();
        }
    }
}
