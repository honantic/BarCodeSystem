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

namespace BarCodeSystem
{
    /// <summary>
    /// OrgInfoModify_Window.xaml 的交互逻辑
    /// </summary>
    public partial class OrgInfoModify_Window : Window
    {


        public OrgInfoList oil;

        public OrgInfoModify_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 保存按钮(确认删除按钮)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (this.Title.Equals("组织信息删除窗口"))
            {
                MyDBController.GetConnection();

                string SQl = string.Format(@"DELETE [OrgInfo] Where OI_Code = '{0}'", txt_OI_Code.Text);

                MyDBController.ExecuteNonQuery(SQl);

                MessageBox.Show("删除成功！");

                btn_Close_Click(sender,e);
            }
            else
            {
                MyDBController.GetConnection();

                string SQl = string.Format(@"UPDATE [OrgInfo] SET [OI_Name]='{0}', [OI_Remark]='{1}' Where OI_Code = '{2}'", txt_OI_Name.Text, txt_OI_Remark.Text, txt_OI_Code.Text);

                MyDBController.ExecuteNonQuery(SQl);

                MessageBox.Show("更新成功！");
            }
                                                                  
        }
        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (this.Title.Equals("组织信息删除窗口"))
            {
                btn_Save.Content = "确认删除";
            }
            
            txt_OI_Code.Text = oil.OI_Code;

            txt_OI_Name.Text = oil.OI_Name;

            txt_OI_Remark.Text = oil.OI_Remark;
        }
    }
}
