using BarCodeSystem.PublicClass.DatabaseEntity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BarCodeSystem.ProductDispatch.FlowCardPrint
{
    /// <summary>
    /// CustomedFlowCard_Window.xaml 的交互逻辑
    /// </summary>
    public partial class CustomedFlowCard_Window : Window
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomedFlowCard_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 搜索流转卡号的构造函数
        /// </summary>
        /// <param name="_fcCode">流转卡编号</param>
        public CustomedFlowCard_Window(string _fcCode)
        {
            InitializeComponent();
            fcCode = _fcCode;
            this.Title = fcCode + "  流转卡打印页面";
        }

        /// <summary>
        /// 流转卡编号
        /// </summary>
        string fcCode;

        /// <summary>
        /// 流转卡行表的textblock列表
        /// </summary>
        List<FlowCardSubLists> fcsList = new List<FlowCardSubLists>();

        /// <summary>
        /// 流转卡主表信息
        /// </summary>
        FlowCardLists fcl = new FlowCardLists();

        /// <summary>
        /// 打印区域的textblock列表
        /// </summary>
        //List<TextBlock> printTB;

        /// <summary>
        /// 人员姓名的textblock列表
        /// </summary>
        List<TextBlock> personNameList;

        /// <summary>
        /// 人员编号的textblock列表
        /// </summary>
        List<TextBlock> personCodeList;

        /// <summary>
        /// 工序名称的textblock列表
        /// </summary>
        //List<TextBlock> processNameList;

        /// <summary>
        /// 人工序编号的textblock列表
        /// </summary>
        //List<TextBlock> processCodeList;

        /// <summary>
        /// 边框列表
        /// </summary>
        List<Border> bdList = new List<Border>();

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(fcCode))
            {
                //获取流转卡主表信息
                fcl = FlowCardLists.FetchFC_InfoByCode(fcCode).FirstOrDefault();
                //获取流转卡行表信息
                fcsList = FlowCardSubLists.FetchFCS_InfoByFC_Id(fcl.ID);
                //获取一维码图片
                FetchBarCodeImage();
                //初始化各个TextBlock列表
                InitTextBlockList();
                //初始化结束后，展示信息
                DisplayInfo();
            }
            
        }

        /// <summary>
        /// 展示相应信息
        /// </summary>
        private void DisplayInfo()
        {
            int count = Math.Min(6, fcsList.Count);
            if (count > 6)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡行表超过6行，这里只显示前6行！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            for (int i = 0; i < count; i++)
            {
                personNameList[i].Text = fcsList[i].FCS_PersonName;
                personCodeList[i].Text = fcsList[i].FCS_PersonCode;
            }

            txtb_FlowCode.Text = fcl.FC_Code;
            txtb_ItemName.Text = fcl.PO_ItemName;
            txtb_ItemSpec.Text = fcl.PO_ItemSpec;
        }

        /// <summary>
        /// 初始化各个TextBlock列表
        /// </summary>
        private void InitTextBlockList()
        {
            personNameList = new List<TextBlock>() { txtb_PersonName_01, txtb_PersonName_02, txtb_PersonName_03, txtb_PersonName_04, txtb_PersonName_05, txtb_PersonName_06 };

            personCodeList = new List<TextBlock>() { txtb_PersonCode_01, txtb_PersonCode_02, txtb_PersonCode_03, txtb_PersonCode_04, txtb_PersonCode_05, txtb_PersonCode_06 };

            personCodeList.ForEach(p => p.FontSize = 8);
            personNameList.ForEach(p => p.FontSize = 8);
        }

        /// <summary>
        /// 获取一维码图片
        /// </summary>
        private void FetchBarCodeImage()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/image_barcode/" + fcl.FC_Code + ".jpg"))
            {
            }
            else
                BarCodeByDcj.BarCodeForCSByDcj.GetBarCode(fcl.FC_Code);

            Bitmap bitMap = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/image_barcode/" + fcl.FC_Code + ".jpg");
            int width = bitMap.Width, height = bitMap.Height;
            RectangleF sourRec = new Rectangle(width * 2 / 10, 0, width * 6 / 10, height * 4 / 5);
            //剪裁一维码图片
            Bitmap newMap = bitMap.Clone(sourRec, bitMap.PixelFormat);

            string fileName = AppDomain.CurrentDomain.BaseDirectory + "/image_barcode/" + fcl.FC_Code + "_副本.jpg";
            if (File.Exists(fileName))
            {
            }
            else
                //保存剪裁过的一维码图片
                newMap.Save(fileName, ImageFormat.Jpeg);

            BitmapImage bitImage = new BitmapImage(new Uri(fileName));
            image_BarCode.Source = bitImage;
        }


        /// <summary>
        /// 设置打印前的可见性
        /// </summary>
        private void PrePrintVisual()
        {
            MyDBController.FindVisualChild<TextBlock>(this).ForEach(e =>
            {
                if (e.Name.Length > 0 && !e.Name.Equals("textb_Titile"))
                {
                }
                else
                    e.Visibility = Visibility.Hidden;
            });



            MyDBController.FindVisualChild<Border>(this).ForEach(e =>
            {
                if (e.Name.Length > 0)
                {
                    e.Visibility = Visibility.Hidden;
                }
            });
            image_Logo.Visibility = Visibility.Hidden;
            btn_Print.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 设置打印后的可见性
        /// </summary>
        private void AfterPrintVisual()
        {
            MyDBController.FindVisualChild<TextBlock>(this).ForEach(e =>
            {
                if (e.Name.Length > 0 && !e.Name.Equals("textb_Titile"))
                {
                }
                else
                    e.Visibility = Visibility.Visible;
            });
            MyDBController.FindVisualChild<Border>(this).ForEach(e =>
            {
                if (e.Name.Length > 0)
                {
                    e.Visibility = Visibility.Visible;
                }
            });
            image_Logo.Visibility = Visibility.Visible;
            btn_Print.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 打印按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Print_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                PrePrintVisual();
                printDialog.PrintVisual(grid_FatherGrid, "流转卡打印模板");
                AfterPrintVisual();
            }
        }

        /// <summary>
        /// 快捷键处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.P && Keyboard.Modifiers == ModifierKeys.Control)
            {
                btn_Print_Click(null, null);
            }
        }
    }
}
