using BarCodeSystem.PublicClass.HelperClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Xml;

namespace BarCodeSystem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Login_Window : Window
    {
        Storyboard storyboard = new Storyboard();
        List<RememberedUser> userList = new List<RememberedUser>();

        public Login_Window()
        {
            InitializeComponent();
        }



        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitImageFolder();
            SetAnimation();
            GetRememberedUser();
            NeedAutoLoginOrNot();
        }

        /// <summary>
        /// 登录按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            bool CheckResult = false;
            CheckResult = Login_Check.CheckIfExist(this.txtBox_Uid.Text.ToString().Trim());
            if (CheckResult)
            {
                if (txtBox_Uid.Text.Equals("admin"))
                {
                    CheckResult = Login_Check.AdminValidToLogin(this.txtBox_Uid.Text.ToString().Trim(), this.pdBox_Pwd.Password.ToString());
                }
                else
                {
                    CheckResult = Login_Check.IsValidToLogin(this.txtBox_Uid.Text.ToString().Trim(), this.pdBox_Pwd.Password.ToString());
                }
                if (!CheckResult)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("用户名或账号错误", "登陆失败", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                }
                else
                {
                    User_Info.User_Code = this.txtBox_Uid.Text.ToString().Trim();
                    storyboard.Begin();
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("该账号在条码系统中不存在！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        /// <summary>
        /// 拖动效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            double x = e.GetPosition(this).X;
            double y = e.GetPosition(this).Y;

            bool IsInput = 150.0 <= x && x <= 375.0 && 200.0 <= y && y <= 270.0;

            if (e.LeftButton == MouseButtonState.Pressed && !IsInput)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// 关闭按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 关闭登录窗体，开启主窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Close(object sender, EventArgs e)
        {
            this.Hide();
            Main_Window main = new Main_Window();
            App.Current.MainWindow = main;
            main.Show();
            this.Close();
        }

        /// <summary>
        /// 条码的图案储存文件夹
        /// </summary>
        private void InitImageFolder()
        {
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/image_barcode"))
            {

            }
            else
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/image_barcode");
            }
        }

        /// <summary>
        /// 设置窗体关闭动画
        /// </summary>
        private void SetAnimation()
        {
            DoubleAnimation dav = new DoubleAnimation();
            dav.To = 0;
            dav.Duration = TimeSpan.FromSeconds(0.5);
            Storyboard.SetTarget(dav, this);
            Storyboard.SetTargetProperty(dav, new PropertyPath("Height"));
            dav.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut };
            storyboard.Children.Add(dav);

            double top = this.Top;
            dav = new DoubleAnimation();
            dav.To = top + top / 2;
            dav.Duration = TimeSpan.FromSeconds(0.5);
            Storyboard.SetTarget(dav, this);
            Storyboard.SetTargetProperty(dav, new PropertyPath("Top"));
            dav.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseIn };
            storyboard.Children.Add(dav);

            storyboard.Completed += new EventHandler(storyboard_Completed);
        }

        /// <summary>
        /// 动画完毕后关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void storyboard_Completed(object sender, EventArgs e)
        {
            SaveUserAccountInfo();
            Window_Close(sender, e);
        }


        /// <summary>
        /// 从XML文件中读取常用账号信息，以及记住密码的账号密码信息
        /// </summary>
        private void GetRememberedUser()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                string account = "Account", isRemembered = "IsRemembered", isLastAccount = "IsLastAccount", isAutoLogin = "IsAutoLogin";
                if (!File.Exists(User_Info.rememberedAccountFileName))
                {
                    DirectoryInfo dir = new DirectoryInfo(User_Info.sysPara);
                    dir.Create();
                    FileStream fs = File.Create(User_Info.rememberedAccountFileName);
                    string s = @"<?xml version=""1.0"" encoding=""utf-8""?><!--This file is the remembered user name and password of BCS--><Accounts></Accounts>";
                    fs.Close();
                    StreamWriter sw = File.AppendText(User_Info.rememberedAccountFileName);
                    sw.Write(s);
                    sw.Close();
                    doc.Load(User_Info.rememberedAccountFileName);
                }
                else
                {
                    doc.Load(User_Info.rememberedAccountFileName);
                }
                XmlNodeList nodeList = doc.GetElementsByTagName(account);
                foreach (XmlNode item in nodeList)
                {
                    userList.Add(new RememberedUser
                    {
                        UserName = item.ChildNodes[0].InnerText,
                        PassWord = EncodeHelper.DecodeBase64(item.ChildNodes[1].InnerText),
                        IsLastAccount = Convert.ToBoolean(item.Attributes[isLastAccount].InnerText),
                        IsRemembered = Convert.ToBoolean(item.Attributes[isRemembered].InnerText),
                        IsAutoLogin = Convert.ToBoolean(item.Attributes[isAutoLogin].InnerText)
                    });
                }
                userList.Sort((p1, p2) => string.Compare(p2.IsLastAccount.ToString(), p1.IsLastAccount.ToString()));
                txtBox_Uid.ItemsSource = null;
                txtBox_Uid.ItemsSource = userList;
                txtBox_Uid.DisplayMemberPath = "UserName";
                int x = userList.Count;
                for (int i = 0; i < x; i++)
                {
                    if (userList[i].IsAutoLogin)
                    {
                        txtBox_Uid.SelectedIndex = i;
                        if (!userList[i].IsRemembered)//没有记住，但是自动登录，默认修改为记住密码状态
                        {
                            userList[i].IsRemembered = true;
                            nodeList[i].Attributes[isRemembered].InnerText = "true";
                            doc.Save(User_Info.rememberedAccountFileName);
                        }
                        break;
                    }
                    else if (i == (x - 1))
                    {
                        txtBox_Uid.SelectedIndex = 0;
                    }

                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

        }

        /// <summary>
        /// PassWordBox的password 不能绑定，在事件里面关联
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_Uid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (txtBox_Uid.SelectedIndex > -1)
            {
                txtBox_Uid.Text = userList[txtBox_Uid.SelectedIndex].UserName;
                cBox_KeepPassWord.IsChecked = userList[txtBox_Uid.SelectedIndex].IsRemembered;
                cBox_AutoLogin.IsChecked = userList[txtBox_Uid.SelectedIndex].IsAutoLogin;
                if ((bool)cBox_KeepPassWord.IsChecked)
                {
                    pdBox_Pwd.Password = userList[txtBox_Uid.SelectedIndex].PassWord;
                }
                else
                {
                    pdBox_Pwd.Password = "";
                }
            }
        }

        /// <summary>
        /// 检查是否需要自动登录
        /// </summary>
        private void NeedAutoLoginOrNot()
        {
            if ((bool)cBox_AutoLogin.IsChecked)
            {
                object sender = new object();
                RoutedEventArgs e = new RoutedEventArgs();
                btn_Login_Click(sender, e);
            }
        }

        /// <summary>
        /// 新账号登陆的话,将账号添加进xml文档
        /// 如果登录账号密码发生更改,在登录成功的状态下,把账号密码更新进入XML文档
        /// </summary>
        public  void SaveUserAccountInfo()
        {
            bool IsNewAccount = true, IsKeepPassWord = (bool)cBox_KeepPassWord.IsChecked, IsAutoLogin = (bool)cBox_AutoLogin.IsChecked;
            string passWord = "";

            //根据用户选择是否记住密码，来更改XMl文档信息
            passWord = IsKeepPassWord ? EncodeHelper.EncodeBase64(Encoding.UTF8, pdBox_Pwd.Password) : "";

            XmlDocument doc = new XmlDocument();
            doc.Load(User_Info.rememberedAccountFileName);
            XmlNodeList nodeList = doc.SelectNodes("/Accounts/Account");
            foreach (XmlNode item in nodeList)
            {
                XmlNode nodeUserName = item.SelectSingleNode("UserName");
                if (nodeUserName.InnerText.Equals(txtBox_Uid.Text.Trim()))
                {
                    IsNewAccount = false;
                    XmlNode nodePassWord = item.SelectSingleNode("PassWord");
                    if (!nodePassWord.InnerText.Equals(passWord))//密码发生了更改,更新XML文档中的密码信息
                    {
                        nodePassWord.InnerText = passWord;//根据用户选择是否记住密码，来更改XMl文档信息
                        item.Attributes["IsRemembered"].InnerText = IsKeepPassWord.ToString();
                        item.Attributes["IsAutoLogin"].InnerText = IsAutoLogin.ToString();
                        item.Attributes["IsLastAccount"].InnerText = "true";
                    }
                    else
                    {
                        item.Attributes["IsAutoLogin"].InnerText = IsAutoLogin.ToString();
                        item.Attributes["IsLastAccount"].InnerText = "true";
                    }
                }
                else
                {
                    item.Attributes["IsLastAccount"].InnerText = "false";
                }
            }

            //这段代码向XMl文档中添加新账号
            if (IsNewAccount)
            {
                XmlElement newAccount = doc.CreateElement("Account");
                newAccount.SetAttribute("IsRemembered", ((bool)cBox_KeepPassWord.IsChecked).ToString());
                newAccount.SetAttribute("IsAutoLogin", ((bool)cBox_AutoLogin.IsChecked).ToString());
                newAccount.SetAttribute("IsLastAccount", "true");
                XmlElement newUserName = doc.CreateElement("UserName");
                newUserName.InnerText = txtBox_Uid.Text;
                newAccount.AppendChild(newUserName);
                XmlElement newPassWord = doc.CreateElement("PassWord");
                newPassWord.InnerText = passWord;
                newAccount.AppendChild(newPassWord);
                doc.DocumentElement.PrependChild(newAccount);
            }
            doc.Save(User_Info.rememberedAccountFileName);
        }

        /// <summary>
        /// 账号框，enter事件处理程序，将焦点转移到密码框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_Uid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                pdBox_Pwd.Focus();
            }
        }

        /// <summary>
        /// 密码框，enter事件，回车登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pdBox_Pwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RoutedEventArgs newE = new RoutedEventArgs();
                btn_Login_Click(sender, newE);
            }
        }

        /// <summary>
        /// 拖动按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FatherGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }


        /// <summary>
        /// 自动登录勾选/去勾选事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cBox_AutoLogin_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)cBox_AutoLogin.IsChecked)
            {
                cBox_KeepPassWord.IsChecked = true;
            }
        }
    }
}
