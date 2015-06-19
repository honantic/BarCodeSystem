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

namespace BarCodeSystem.BasicFile.ItemInfo
{
    /// <summary>
    /// ItemInfoModify_Window.xaml 的交互逻辑
    /// </summary>
    /// 
    

    public partial class ItemInfoModify_Window : Window
    {

        public ItemInfoLists iil;
        Int64 chose_id;

        public ItemInfoModify_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            MyDBController.GetConnection();

            string SQl = string.Format(@"UPDATE [ItemInfo] SET [II_QualitySortID]={0}  Where II_Code = '{1}'", chose_id, txtb_II_Code.Text);

            MyDBController.ExecuteNonQuery(SQl);

            MessageBox.Show("保存成功！");

            this.DialogResult = true;
            this.Close();
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
        /// 窗口弹出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtb_II_Code.Text = iil.II_Code;
            txtb_II_Name.Text = iil.II_Name;
            txtb_II_Spec.Text = iil.II_Spec;
            txtb_II_UnitCode.Text = iil.II_UnitCode;
            txtb_II_UnitID.Text = iil.II_UnitID.ToString();
            txtb_II_UnitName.Text = iil.II_UnitName;
            txtb_II_Version.Text = iil.II_Version;
            txtb_II_QualitySortID.Text = iil.II_QualitySortName;

        }

        /// <summary>
        /// 质检分类鼠标点击选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_Getname_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ItemInfoQualitySort_Window iiqs = new ItemInfoQualitySort_Window();
            iiqs.ShowDialog();

            if((bool)iiqs.DialogResult)
            {
                txtb_II_QualitySortID.Text = iiqs.chose_name;
                this.chose_id = iiqs.chose_id;
            }
        }
    }
}
