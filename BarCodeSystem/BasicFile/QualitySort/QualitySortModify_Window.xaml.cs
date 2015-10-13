using BarCodeSystem.PublicClass;
using BarCodeSystem.PublicClass.HelperClass;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// QualitySortModify_Window.xaml 的交互逻辑
    /// </summary>
    public partial class QualitySortModify_Window : Window
    {

        public QualitySortList qsl;
        //QualitySortList qsl = new QualitySortList();

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        public QualitySortModify_Window()
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

            string SQl ="";
            string mess = "";
            if (string.IsNullOrEmpty(txtb_Code.Text) || string.IsNullOrEmpty(txtb_Name.Text))
            {
                MessageBox.Show("编号和名称不能为空!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                DBLog _dbLog = new DBLog();
                _dbLog.DBL_OperateBy = User_Info.User_Code + "|" + User_Info.User_Name;
                _dbLog.DBL_OperateTable = "QualitySort";
                _dbLog.DBL_OperateTime = DateTime.Now.ToString();
                if (this.Title.Equals("质检分类新增窗口"))
                {
                    if (Check_QulitySort_Code(txtb_Code.Text.Trim()))
                    {
                        if (dt.Select("QS_Code = '" + txtb_Code.Text.Trim() + "'").Length > 0)
                        {

                            MessageBox.Show("编号已经存在,请重新输入!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            SQl = string.Format("INSERT INTO [QualitySort]([QS_Code],[QS_NAME]) VALUES('{0}','{1}')", txtb_Code.Text.Trim(), txtb_Name.Text.Trim());
                            mess = "新增信息成功!";
                            _dbLog.DBL_OperateType = OperateType.Insert;
                            _dbLog.DBL_Content = "新增质量问题原因:" + txtb_Code.Text + "|" + txtb_Name.Text;
                            _dbLog.DBL_AssociateCode = txtb_Code.Text;
                        }
                    }
                    else
                    {
                        MessageBox.Show("编号必须为小于5位的数字!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if (this.Title.Equals("质检分类修改窗口"))
                {
                    SQl = string.Format(@"UPDATE [QualitySort] SET [QS_Code]='{0}',[QS_Name]='{1}' WHERE [QS_Code]='{2}'", txtb_Code.Text.Trim(), txtb_Name.Text.Trim(), txtb_Code.Text.Trim());
                    _dbLog.DBL_OperateType = OperateType.Update;
                    _dbLog.DBL_Content = "修改质量问题原因:" + txtb_Code.Text + "|" + txtb_Name.Text;
                    _dbLog.DBL_AssociateCode = txtb_Code.Text;
                    mess = "修改信息成功!";
                }
                else
                {
                    SQl = string.Format(@"DELETE [QualitySort]  WHERE [QS_Code]='{0}'", txtb_Code.Text.Trim());
                    _dbLog.DBL_OperateType = OperateType.Delete;
                    _dbLog.DBL_Content = "删除质量问题原因:" + txtb_Code.Text + "|" + txtb_Name.Text;
                    _dbLog.DBL_AssociateCode = txtb_Code.Text;
                    mess = "删除信息成功!";
                }

                if (!string.IsNullOrEmpty(mess))
                {
                    MyDBController.GetConnection();
                    try
                    {
                        MyDBController.ExecuteNonQuery(SQl);
                        //if (MessageBox.Show(mess, "提示", MessageBoxButton.OK, MessageBoxImage.Information) ==
                        //    MessageBoxResult.OK)
                        //{
                        //    this.DialogResult = true;
                        //}
                        //MessageBox.Show(mess);
                        MessageBox.Show(mess);

                        txtb_Code.Text = "";
                        txtb_Name.Text = "";

                        this.DialogResult = true;
                        this.Close();
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.Message);
                    }

                    MyDBController.CloseConnection();
                }
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
        }


        /// <summary>
        /// 窗口加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //这段代码在正式环境中将被注释掉，测试用
            MyDBController.Server = User_Info.server[1];
            MyDBController.Database = User_Info.database[1];
            MyDBController.Pwd = User_Info.pwd[1];
            MyDBController.Uid = User_Info.uid[1];

            GetBCSQualitySortLists();

            if (this.Title.Equals("质检分类修改窗口"))
            {
                txtb_Code.Text = qsl.QS_Code;
                txtb_Name.Text = qsl.QS_Name;
                txtb_Code.IsReadOnly = true;
            }
            else if (this.Title.Equals("质检分类删除窗口"))
            {
                txtb_Code.Text = qsl.QS_Code;
                txtb_Name.Text = qsl.QS_Name;
                txtb_Code.IsReadOnly = true;
                txtb_Name.IsReadOnly = true;

                btn_Save.Content = "确认删除";
            }
        }

        /// <summary>
        /// 连接数据库并读取质检分类表信息
        /// </summary>
        public void GetBCSQualitySortLists()
        {
            MyDBController.GetConnection();

            string SQl = @"select ID,QS_Code,QS_Name from  QualitySort";
            dt = MyDBController.GetDataSet(SQl, ds, "QualitySort").Tables["QualitySort"];
            MyDBController.CloseConnection();
        }

        /// <summary>
        /// 判断质检分类编码是否为不超过5位的数字
        /// </summary>
        public bool Check_QulitySort_Code(string strInput)
        {
            try
            {

                if (strInput.Trim().Length > 5)
                {
                    return false;
                }

                decimal num = Convert.ToDecimal(strInput);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
