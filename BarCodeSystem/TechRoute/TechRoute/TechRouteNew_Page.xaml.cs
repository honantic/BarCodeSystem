using BarCodeSystem.PublicClass.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace BarCodeSystem.TechRoute.TechRoute
{
    /// <summary>
    /// Interaction logic for TechRouteNew_Page.xaml
    /// </summary>
    public partial class TechRouteNew_Page : Page
    {
        #region 构造函数
        /// <summary>
        /// 默认构造函数，用来新增料品
        /// </summary>
        public TechRouteNew_Page()
        {
            InitializeComponent();
            iil = new ItemInfoLists();
            tv = new TechVersion();
            trlList = new List<TechRouteLists>();
            isNewItem = true;
            isNewVersion = true;
            hasItemInfo = false;
            hasTechVersion = false;
            hasChanged = false;
            hasFlowCard = false;
        }

        /// <summary>
        /// 料品新增工艺版本的构造函数
        /// </summary>
        /// <param name="_iil"></param>
        public TechRouteNew_Page(ItemInfoLists _iil)
        {
            InitializeComponent();
            iil = _iil;
            tv = new TechVersion();
            trlList = new List<TechRouteLists>();
            isNewItem = false;
            isNewVersion = true;
            hasItemInfo = true;
            hasTechVersion = false;
            hasChanged = false;
            hasFlowCard = false;
        }

        /// <summary>
        /// 修改已经存在的工艺路线的构造函数
        /// </summary>
        /// <param name="_iil"></param>
        /// <param name="_tv"></param>
        /// <param name="_trlList"></param>
        public TechRouteNew_Page(ItemInfoLists _iil, TechVersion _tv, List<TechRouteLists> _trlList)
        {
            InitializeComponent();
            iil = _iil;
            tv = _tv;
            trlList = _trlList;
            isNewItem = false;
            isNewVersion = false;
            hasItemInfo = true;
            hasTechVersion = true;
            hasChanged = false;
            hasFlowCard = _tv.TRV_HasFlowCard;
        }
        #endregion

        #region 变量
        /// <summary>
        /// 是否被修改
        /// </summary>
        private bool hasChanged = false;

        /// <summary>
        /// 是否是新增料品
        /// </summary>
        private bool isNewItem;

        /// <summary>
        /// 不是新料品，但是是新版本
        /// </summary>
        private bool isNewVersion;

        /// <summary>
        /// 料品信息
        /// </summary>
        private ItemInfoLists iil;

        /// <summary>
        /// 工艺路线版本信息
        /// </summary>
        private TechVersion tv;

        /// <summary>
        /// 工艺路线信息列表
        /// </summary>
        private List<TechRouteLists> trlList;

        /// <summary>
        /// 删除的工艺路线列表
        /// </summary>
        private List<TechRouteLists> trlDeleteList = new List<TechRouteLists>();
        /// <summary>
        /// 是否有流转卡
        /// </summary>
        private bool hasFlowCard;

        /// <summary>
        /// 加载次数
        /// </summary>
        private int loadCount = 0;

        /// <summary>
        /// 是否选择了料品
        /// </summary>
        private bool hasItemInfo = false;

        /// <summary>
        /// 是否有工艺路线本班
        /// </summary>
        private bool hasTechVersion = false;

        /// <summary>
        /// 系统中工艺版本列表
        /// </summary>
        private List<TechVersion> tvList = new List<TechVersion>();
        #endregion

        #region 初始化
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                SetInitEnabledProperties();
                EmbedCheckEvents();
                DisplayInitInformation();
                FetchBCSTVInfo();
                loadCount++;
            }
        }

        /// <summary>
        /// 绑定改变事件
        /// </summary>
        private void EmbedCheckEvents()
        {
            MyDBController.FindVisualChild<TextBox>(this).ForEach(p => p.TextChanged += cbClick);
            MyDBController.FindVisualChild<RadioButton>(this).ForEach(p => p.Click += cbClick);
            MyDBController.FindVisualChild<CheckBox>(this).ForEach(p => p.Click += cbClick);
        }

        /// <summary>
        /// 绑定的改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbClick(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox && !((TextBox)sender).IsReadOnly)
            {
                hasChanged = true;
            }
            if (sender is CheckBox)
            {
                switch (((CheckBox)sender).Name)
                {
                    case "cb_TRVIsShown":
                        if (((CheckBox)sender).IsChecked == false)
                        {
                            MessageBoxResult mbr = Xceed.Wpf.Toolkit.MessageBox.Show("确定要隐藏吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (mbr == MessageBoxResult.No)
                            {
                                ((CheckBox)sender).IsChecked = true;
                            }
                        }
                        break;
                    case "cb_IsBackVersion":
                        if (((CheckBox)sender).IsChecked == true)
                        {
                            MessageBoxResult mbr = Xceed.Wpf.Toolkit.MessageBox.Show("确定要设为返工版本吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (mbr == MessageBoxResult.No)
                            {
                                ((CheckBox)sender).IsChecked = false;
                            }
                        }
                        break;
                    default:
                        break;
                }
                hasChanged = true;
            }
        }

        /// <summary>
        /// 获取系统的工艺版本列表
        /// </summary>
        private void FetchBCSTVInfo()
        {
            Task t = new Task(() => { tvList = TechVersion.FetchTechVersion(); });
            t.Start();
        }

        /// <summary>
        /// 设置初始化的可操作性设置
        /// </summary>
        private void SetInitEnabledProperties()
        {
            SetInitItemProperties();
            SetInitVersionProperties();
        }

        /// <summary>
        /// 料品相关的操作性
        /// </summary>
        private void SetInitItemProperties()
        {
            btn_ChooseItem.IsEnabled = isNewItem;
        }

        /// <summary>
        /// 版本的操作性
        /// </summary>
        private void SetInitVersionProperties()
        {
            cb_TRV_HasFlowCard.IsChecked = hasFlowCard;
            btn_AddNewVersion.IsEnabled = isNewVersion;
        }

        /// <summary>
        /// 展示初始化信息
        /// </summary>
        private void DisplayInitInformation()
        {
            DisplayInitItemInfo();
            DisplayInitVersionInfo();
            DisplayInitTRInfo();
        }

        /// <summary>
        /// 展示初始化料品信息
        /// </summary>
        private void DisplayInitItemInfo()
        {
            if (iil != null)
            {
                tb_ItemName.Text = iil.II_Name;
                tb_ItemCode.Text = iil.II_Code;
                tb_ItemSpec.Text = iil.II_Spec;
                tb_ItemVersion.Text = iil.II_Version;
            }
        }

        /// <summary>
        /// 展示初始化版本信息
        /// </summary>
        private void DisplayInitVersionInfo()
        {
            if (tv != null)
            {
                tb_VersionCode.Text = tv.TRV_VersionCode;
                tb_VersionName.Text = tv.TRV_VersionName;
                cb_TRVIsShown.IsChecked = tv.TRV_IsShown;
                cb_IsDefaulVersion.IsChecked = tv.TRV_IsDefaultVer;
                cb_TRV_HasFlowCard.IsChecked = tv.TRV_HasFlowCard;
                cb_IsBackVersion.IsChecked = tv.TRV_IsBackVersion;
                switch (tv.TRV_ReportWay)
                {
                    case 0:
                        rb_IntegrateReport.IsChecked = true;
                        break;
                    case 1:
                    default:
                        rb_ScatterReport.IsChecked = true;
                        break;
                }
            }
        }

        /// <summary>
        /// 展示初始化工艺路线信息
        /// </summary>
        private void DisplayInitTRInfo()
        {
            if (trlList != null)
            {
                dg_TechRouteInfo.ItemsSource = trlList;
            }
        }
        #endregion

        #region 操作
        /// <summary>
        /// 选择料品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ChooseItem_Click(object sender, RoutedEventArgs e)
        {
            TechRouteItemList_Window tril = new TechRouteItemList_Window();
            tril.ShowDialog();
            if (tril.DialogResult == true)
            {
                iil.II_Name = tril.II_Name;
                iil.II_Code = tril.II_Code;
                iil.II_Spec = tril.II_Spec;
                iil.II_Version = tril.II_Version;
                iil.ID = tril.II_ID;
                DisplayInitItemInfo();
                hasChanged = true;
                hasItemInfo = true;
            }
        }

        /// <summary>
        /// 新增编号版本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddNewVersion_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (hasItemInfo)
            {
                if (btn_AddNewVersion.Content.Equals("新增版本"))
                {
                    tb_VersionName.IsReadOnly = tb_VersionCode.IsReadOnly = false;
                    tb_VersionName.Background = tb_VersionCode.Background = Brushes.White;
                    tb_VersionCode.Focus();
                    btn_AddNewVersion.Content = "保存版本";
                    hasChanged = true;
                }
                else
                {
                    if (!string.IsNullOrEmpty(tb_VersionCode.Text.Trim()) && !string.IsNullOrEmpty(tb_VersionName.Text.Trim()))
                    {
                        bool flag = CheckVersionCode(true);
                        if (!flag)
                        {
                            AcceptTechVersionInfo();
                            tb_VersionName.IsReadOnly = tb_VersionCode.IsReadOnly = true;
                            btn_AddNewVersion.Content = "新增版本";
                            tb_VersionName.Background = tb_VersionCode.Background = Brushes.LightGray;
                        }
                    }
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("工艺版本编号和名称不能为空！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请先选择料品信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 将工艺版本信息保存到本地缓存
        /// </summary>
        private void AcceptTechVersionInfo()
        {
            if (tv != null)
            {
                tv.TRV_VersionName = tb_VersionName.Text.Trim();
                tv.TRV_VersionCode = tb_VersionCode.Text.Trim();
                tv.TRV_ItemID = iil.ID;
                tv.TRV_IsShown = Convert.ToBoolean(cb_TRVIsShown.IsChecked);
                tv.TRV_IsDefaultVer = Convert.ToBoolean(cb_IsDefaulVersion.IsChecked);
                tv.TRV_IsBackVersion = Convert.ToBoolean(cb_IsBackVersion.IsChecked);
                tv.TRV_HasFlowCard = Convert.ToBoolean(cb_TRV_HasFlowCard.IsChecked);
                tv.TRV_ReportWay = rb_ScatterReport.IsChecked == true ? 1 : (rb_IntegrateReport.IsChecked == true ? 0 : 1);
                hasChanged = true;
                hasTechVersion = true;
            }
        }

        /// <summary>
        /// 失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_VersionCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (isNewVersion)
            {
                CheckVersionCode();
            }
        }

        /// <summary>
        /// 检查版本
        /// </summary>
        private bool CheckVersionCode(bool _isSaving = false)
        {
            string _versionCode = tb_VersionCode.Text.Trim();
            bool flag = CheckForVersionCode(_versionCode, iil.ID);
            ShowWrongVersionWarning(flag, _isSaving);
            return flag;
        }

        /// <summary>
        /// 错误编号的警示
        /// </summary>
        /// <param name="_flag"></param>
        private void ShowWrongVersionWarning(bool _flag, bool _isSaving = false)
        {
            if (_flag)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("该料品下已经存在此编号！\r\n请重新维护！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!_flag & _isSaving)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("保存成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// 检测版本编号是否重复
        /// </summary>
        /// <param name="_versionCode"></param>
        /// <returns></returns>
        private bool CheckForVersionCode(string _versionCode, Int64 _itemID)
        {
            bool flag = false;
            foreach (TechVersion item in tvList)
            {
                if (item.TRV_VersionCode.Equals(_versionCode) && item.TRV_ItemID.Equals(_itemID))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        /// <summary>
        /// 工艺版本编号文本框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_VersionCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (isNewVersion)
            {
                switch (CheckForVersionCode(tb_VersionCode.Text.Trim(), iil.ID))
                {
                    case true:
                        tb_VersionCode.Background = Brushes.Red;
                        break;
                    case false:
                    default:
                        tb_VersionCode.Background = Brushes.White;
                        break;
                }
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 选择检验员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ChooseCheckPerson_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (dg_TechRouteInfo.SelectedIndex != -1)
            {
                TechRouteCheckPerson_Window trcp = new TechRouteCheckPerson_Window();
                trcp.ShowDialog();
                if (trcp.DialogResult == true)
                {
                    ((TechRouteLists)dg_TechRouteInfo.SelectedItem).TR_DefaultCheckPersonName = trcp.checkPersonName;
                    KeyboardSimulation.Press(Key.Escape);
                    KeyboardSimulation.Release(Key.Escape);
                    hasChanged = true;
                }
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 增加工艺路线的工序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddTR_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (hasItemInfo)
            {
                if (hasTechVersion)
                {
                    trlList.Add(new TechRouteLists() { TR_ItemCode = iil.II_Code, TR_ItemID = iil.ID, TR_IsTestProcess = false, TR_IsBackProcess = false, TR_IsReportDevice = false, TR_IsDeviceCharging = false, TR_WorkCenterID = User_Info.User_Workcenter_ID });
                    SortTechRouteList();
                    hasChanged = true;
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("请先增加版本信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请先选择料品信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 删除工序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DeleteTR_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (!tv.TRV_HasFlowCard)
            {
                if (dg_TechRouteInfo.SelectedIndex != -1)
                {
                    trlDeleteList.Add((TechRouteLists)dg_TechRouteInfo.SelectedItem);
                    trlList.Remove((TechRouteLists)dg_TechRouteInfo.SelectedItem);
                    SortTechRouteList();
                    hasChanged = true;
                }
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 对工艺路线列表进行排序，并计算是否首/末道工序
        /// </summary>
        private void SortTechRouteList()
        {
            trlList = trlList.OrderBy(p => p.TR_ProcessSequence).ToList();
            trlList.ForEach(p =>
            {
                if (trlList.IndexOf(p) == 0)
                {
                    p.TR_IsFirstProcess = true;
                }
                else
                {
                    p.TR_IsFirstProcess = false;
                }
                if (trlList.IndexOf(p) == trlList.Count - 1)
                {
                    p.TR_IsLastProcess = true;
                    p.TR_IsCountingProcess = true;
                }
                else
                {
                    p.TR_IsLastProcess = false;
                    p.TR_IsCountingProcess = false;
                }
            });
            dg_TechRouteInfo.ItemsSource = null;
            dg_TechRouteInfo.ItemsSource = trlList;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            bool flag = false;
            if (hasChanged)
            {
                MessageBoxResult mbr = Xceed.Wpf.Toolkit.MessageBox.Show("更改的内容没有保存，是否继续？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mbr == MessageBoxResult.Yes)
                {
                    flag = true;
                }
            }
            else
            {
                flag = true;
            }
            if (flag)
            {
                MyDBController.FindVisualParent<TechRoute_Page>(this).ForEach(p =>
                    {
                        p.frame_TechInfoFrame.GoBack();
                    });
            }
        }

        /// <summary>
        /// 选择工序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ChooseProcess_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ProcessNameList_Window pnlw = new ProcessNameList_Window();
            pnlw.ShowDialog();
            if (pnlw.DialogResult == true)
            {
                ((TechRouteLists)dg_TechRouteInfo.SelectedItem).TR_ProcessName = pnlw.PN_Name;
                ((TechRouteLists)dg_TechRouteInfo.SelectedItem).TR_ProcessCode = pnlw.PN_Code;
                ((TechRouteLists)dg_TechRouteInfo.SelectedItem).TR_ProcessID = pnlw.PN_ID;
                KeyboardSimulation.Press(Key.Escape);
                KeyboardSimulation.Release(Key.Escape);
                hasChanged = true;
            }
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 行号改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iud_TechSequence_LostFocus(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            int index = dg_TechRouteInfo.SelectedIndex;
            if (index != -1)
            {
                int sequence = Convert.ToInt32(((IntegerUpDown)sender).Value);
                foreach (TechRouteLists trl in trlList)
                {

                    if (trl.TR_ProcessSequence.Equals(sequence))
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("工序号不能重复", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        ((IntegerUpDown)sender).Value = 0;
                        break;
                    }
                }
                trlList[index].TR_ProcessSequence = sequence;
                hasChanged = true;
                SortTechRouteList();
            }
            this.Cursor = Cursors.Arrow;
        }


        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveInfo_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (dg_TechRouteInfo.HasItems)
            {
                if (hasItemInfo & hasTechVersion)
                {
                    AcceptTechVersionInfo();
                    bool flag = TechVersion.SaveInfo(tv);
                    if (flag)
                    {
                        Int64 _id = -1;
                        if (tv.ID != -1)
                        {
                            _id = tv.ID;
                        }
                        else
                        {
                            _id = TechVersion.FetchVersionID(tv);
                        }
                        if (_id == -1)
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("保存失败，请重试!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            trlList.ForEach(p => p.TR_VersionID = _id);
                            flag = TechRouteLists.SaveInfo(trlList);
                            if (flag)
                            {
                                TechRouteLists.DeleteInfo(trlDeleteList);
                                trlDeleteList.Clear();
                                Xceed.Wpf.Toolkit.MessageBox.Show("保存成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                                MyDBController.FindVisualParent<TechRoute_Page>(this).ForEach(p => p.frame_TechInfoFrame.GoBack());
                            }
                            else
                            {
                                Xceed.Wpf.Toolkit.MessageBox.Show("保存失败，请重试!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("保存失败，请重试!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("没有任何工序信息，请重试!", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Cursor = Cursors.Arrow;
        }
        #endregion
    }
}
