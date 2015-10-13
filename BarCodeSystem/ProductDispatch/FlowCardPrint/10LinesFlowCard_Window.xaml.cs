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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BarCodeSystem.ProductDispatch.FlowCardPrint
{
    /// <summary>
    /// _10LinesFlowCard_Window.xaml 的交互逻辑
    /// </summary>
    public partial class _10LinesFlowCard_Window : Window
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public _10LinesFlowCard_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 搜索流转卡号的构造函数
        /// </summary>
        /// <param name="_fcCode">流转卡编号</param>
        public _10LinesFlowCard_Window(string _fcCode)
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
        /// 流转卡工艺路线的信息列表
        /// </summary>
        List<TechRouteLists> trlList = new List<TechRouteLists>();

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
        List<TextBlock> processNameList;

        /// <summary>
        /// 人工序编号的textblock列表
        /// </summary>
        List<TextBlock> processCodeList;

        /// <summary>
        /// 投入数的textblock列表
        /// </summary>
        List<TextBlock> startAmountList;

        /// <summary>
        /// 合格数的textblock列表
        /// </summary>
        List<TextBlock> qualifiedAmountList;

        /// <summary>
        /// 报废原因的textblock列表
        /// </summary>
        List<TextBlock> scrapReasonList;

        /// <summary>
        /// 边框列表
        /// </summary>
        List<Border> bdList = new List<Border>();

        /// <summary>
        /// 行数
        /// </summary>
        readonly int lineCount = 10;

        /// <summary>
        /// 拖动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }


        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //获取流转卡主表信息
            fcl = FlowCardLists.FetchFC_InfoByCode(fcCode).FirstOrDefault();
            //获取流转卡行表信息
            fcsList = FlowCardSubLists.FetchFCS_InfoByFC_Id(fcl.ID);
            //获取工艺路线信息
            trlList = TechRouteLists.FetchTechRouteByItemCode(fcl.PO_ItemCode, fcl.TRV_VersionCode);
            //获取一维码图片
            FetchBarCodeImage();
            //初始化各个TextBlock列表
            InitTextBlockList();
            //初始化结束后，展示信息
            DisplayInfo();
        }

        /// <summary>
        /// 展示相应信息
        /// </summary>
        private void DisplayInfo()
        {
            var sequenceList = trlList.Select(p => p.TR_ProcessSequence).Distinct().ToList();

            int count = Math.Min(personNameList.Count, sequenceList.Count);
            if (sequenceList.Count > count)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("该流转卡行表超过" + count + "行，这里只显示前" + count + "行！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            try
            {
                foreach (int p in sequenceList)
                {
                    int index = sequenceList.IndexOf(p);
                    //检查是多人还是单人
                    if (fcsList.FindAll(fcs => fcs.FCS_ProcessSequanece.Equals(p)).Count > 1)
                    {
                        personNameList[index].Text = WorkTeamLists.CheckIfMultiPersonMatchWT(fcsList.FindAll(fcs => fcs.FCS_ProcessSequanece.Equals(p)));
                        //报工数量
                        if (fcsList.Find(fcs => fcs.FCS_ProcessSequanece.Equals(p)).FCS_IsReported)
                        {
                            startAmountList[index].Text = fcsList.Find(fcs => fcs.FCS_ProcessSequanece.Equals(p)).FCS_BeginAmount.ToString();
                            qualifiedAmountList[index].Text = fcsList.Find(fcs => fcs.FCS_ProcessSequanece.Equals(p)).FCS_QulifiedAmount.ToString();
                            if (fcsList.Find(fcs => fcs.FCS_ProcessSequanece.Equals(p)).FCS_ScrappedAmount > 0)
                            {
                                List<FlowCardQualityLists> fcqList = FlowCardQualityLists.FetchFCQLByFCSInfo(fcsList.FindAll(item => item.FCS_ProcessSequanece.Equals(p)));
                                fcqList.ForEach(temp => scrapReasonList[index].Text += temp.QI_Name + "*" + temp.FCQ_ScrapAmount.ToString());
                            }
                        }
                    }
                    else if (fcsList.FindAll(fcs => fcs.FCS_ProcessSequanece.Equals(p)).Count == 1)
                    {
                        personNameList[index].Text = fcsList.FindAll(fcs => fcs.FCS_ProcessSequanece.Equals(p))[0].FCS_PersonName;
                        personCodeList[index].Text = fcsList.FindAll(fcs => fcs.FCS_ProcessSequanece.Equals(p))[0].FCS_PersonCode;
                        //报工数量
                        if (fcsList.Find(fcs => fcs.FCS_ProcessSequanece.Equals(p)).FCS_IsReported)
                        {
                            startAmountList[index].Text = fcsList.Find(fcs => fcs.FCS_ProcessSequanece.Equals(p)).FCS_BeginAmount.ToString();
                            qualifiedAmountList[index].Text = fcsList.Find(fcs => fcs.FCS_ProcessSequanece.Equals(p)).FCS_QulifiedAmount.ToString();
                            if (fcsList.Find(fcs => fcs.FCS_ProcessSequanece.Equals(p)).FCS_ScrappedAmount > 0)
                            {
                                List<FlowCardQualityLists> fcqList = FlowCardQualityLists.FetchFCQLByFCSInfo(fcsList.FindAll(item => item.FCS_ProcessSequanece.Equals(p)));
                                fcqList.ForEach(temp => scrapReasonList[index].Text += temp.QI_Name + "*" + temp.FCQ_ScrapAmount.ToString());
                            }
                        }
                    }
                    //工序
                    if (trlList.FindAll(trl => trl.TR_ProcessSequence.Equals(p)).Count > 0)
                    {
                        processNameList[index].Text = trlList.FindAll(trl => trl.TR_ProcessSequence.Equals(p))[0].TR_ProcessName;
                        processCodeList[index].Text = trlList.FindAll(trl => trl.TR_ProcessSequence.Equals(p))[0].TR_ProcessSequence.ToString();
                    }
                }
                startAmountList[0].Text = fcl.FC_Amount.ToString();
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            txtb_FlowCode.Text = fcl.FC_Code;
            txtb_ItemName.Text = fcl.PO_ItemName;
            txtb_ItemSpec.Text = fcl.PO_ItemSpec;
            txtb_SourceOrderCode.Text = fcl.PO_Code;
        }

        /// <summary>
        /// 初始化各个TextBlock列表
        /// </summary>
        private void InitTextBlockList()
        {
            GenerateTextBlock();
        }

        /// <summary>
        /// 动态生成各个textblock
        /// </summary>
        private void GenerateTextBlock()
        {
            personNameList = new List<TextBlock>();
            personCodeList = new List<TextBlock>();
            processNameList = new List<TextBlock>();
            processCodeList = new List<TextBlock>();
            startAmountList = new List<TextBlock>();
            qualifiedAmountList = new List<TextBlock>();
            scrapReasonList = new List<TextBlock>();

            for (int i = 0; i < lineCount; i++)
            {
                TextBlock t1 = new TextBlock() { Name = "txtb_PersonCode_" + string.Format("{0:00}", i + 1), VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, FontSize = 8 };
                grid_CenterGrid.Children.Add(t1);
                Grid.SetColumn(t1, 2); Grid.SetRow(t1, i + 1);
                personCodeList.Add(t1);

                TextBlock t2 = new TextBlock() { Name = "txtb_PersonName_" + string.Format("{0:00}", i + 1), VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, FontSize = 8 };
                grid_CenterGrid.Children.Add(t2);
                Grid.SetColumn(t2, 3); Grid.SetRow(t2, i + 1);
                personNameList.Add(t2);

                TextBlock t3 = new TextBlock() { Name = "txtb_ProcessCode_" + string.Format("{0:00}", i + 1), VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, FontSize = 8 };
                grid_CenterGrid.Children.Add(t3);
                Grid.SetColumn(t3, 4); Grid.SetRow(t3, i + 1);
                processCodeList.Add(t3);

                TextBlock t4 = new TextBlock() { Name = "txtb_ProcessName_" + string.Format("{0:00}", i + 1), VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, FontSize = 8 };
                grid_CenterGrid.Children.Add(t4);
                Grid.SetColumn(t4, 5); Grid.SetRow(t4, i + 1);
                processNameList.Add(t4);

                TextBlock t5 = new TextBlock() { Name = "txtb_StartAmount_" + string.Format("{0:00}", i + 1), VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, FontSize = 8 };
                grid_CenterGrid.Children.Add(t5);
                Grid.SetColumn(t5, 6); Grid.SetRow(t5, i + 1);
                startAmountList.Add(t5);

                TextBlock t6 = new TextBlock() { Name = "txtb_QualifiedAmount_" + string.Format("{0:00}", i + 1), VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, FontSize = 8 };
                grid_CenterGrid.Children.Add(t6);
                Grid.SetColumn(t6, 7); Grid.SetRow(t6, i + 1);
                qualifiedAmountList.Add(t6);

                TextBlock t7 = new TextBlock() { Name = "txtb_ScrapReason_" + string.Format("{0:00}", i + 1), VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left, FontSize = 8 };
                grid_CenterGrid.Children.Add(t7);
                Grid.SetColumn(t7, 8); Grid.SetRow(t7, i + 1); Grid.SetColumnSpan(t7, 2);
                scrapReasonList.Add(t7);
            }
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
                if (e.Name.Length > 0)
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
            btn_Close.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 设置打印后的可见性
        /// </summary>
        private void AfterPrintVisual()
        {
            MyDBController.FindVisualChild<TextBlock>(this).ForEach(e =>
            {
                if (e.Name.Length > 0)
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
            btn_Close.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 套打
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    PrePrintVisual();
                    printDialog.PrintVisual(grid_FatherGrid, "流转卡打印模板");
                    AfterPrintVisual();
                }
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// 全打
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FullPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    btn_Close.Visibility = btn_FullPrint.Visibility = btn_Print.Visibility = Visibility.Hidden;
                    printDialog.PrintVisual(grid_FatherGrid, "流转卡打印模板");
                    btn_Close.Visibility = btn_FullPrint.Visibility = btn_Print.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ee)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ee.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
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

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      
    }
}
