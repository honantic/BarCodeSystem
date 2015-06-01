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
using System.Data;
using System.Threading;

namespace BarCodeSystem
{
    /// <summary>
    /// TechRoute_Window.xaml 的交互逻辑
    /// </summary>
    public partial class TechRoute_Window : Window
    {

        public TechRoute_Window()
        {
            InitializeComponent();
        }

        DataTable itemTech = new DataTable();
        DataTable process = new DataTable();
        DataSet ds = new DataSet();

        //料品信息列表
        List<ItemInfoLists> itemList = new List<ItemInfoLists> { };
        //总工艺路线列表
        List<TechRouteLists> techList = new List<TechRouteLists> { };
        //选中的料品的工艺路线列表
        List<TechRouteLists> trls = new List<TechRouteLists> { };
        //listview1中的combobox列表，用来获取listview1选中行对应的combobox信息
        List<ComboBox> cbs = new List<ComboBox> { };
        //窗体点击数量，用来确定是否第一次点击，第一次点击用来加载所有的combobox
        int formClickCount = 0;
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //这段代码在正式环境中将被注释掉，测试用
            //MyDBController.Server = User_Info.server[1];
            //MyDBController.Database = User_Info.database[1];
            //MyDBController.Pwd = User_Info.pwd[1];
            //MyDBController.Uid = User_Info.uid[1];
            GetBCSItemTechRoute();
        }



        /// <summary>
        /// 获得条码系统中，已经有工艺路线的料品及其对应工艺路线的信息
        /// </summary>
        private void GetBCSItemTechRoute()
        {
            ds.Clear();
            cbs.Clear();
            itemList.Clear();
            techList.Clear();
            MyDBController.GetConnection();
            string SQl = @" SELECT A.[ID],A.[TR_ItemID],A.[TR_ItemCode],C.[II_Name],A.[TR_VersionID],B.[TRV_Version],B.[TRV_IsDefaultVer],D.                 [WC_Department_Name],A.[TR_WorkCenterID],A.[TR_ProcessSequence],A.[TR_ProcessName],A.[TR_ProcessCode],
                            A.[TR_ProcessID],A.[TR_WagePerPiece],A.[TR_WorkHour],A.[TR_WageAllotScheme],
                            CASE A.[TR_WageAllotScheme] WHEN 0 THEN '独立分配' WHEN 1 THEN '平均分配' WHEN 2 THEN '按合作人数分配配额公式计算'
                            END  AS [TR_WageAllotScheme_Show] FROM [TechRoute] A LEFT JOIN [TechRouteVersion] B 
                            ON A.[TR_ItemID]=B.[TRV_ItemID]  AND A.[TR_VersionID]=B.[ID]
                            LEFT JOIN [ItemInfo] C ON A.[TR_ItemID]=C.[ID]
                            LEFT JOIN [WorkCenter] D ON A.[TR_WorkCenterID]=D.[WC_Department_ID] 
                            ORDER BY A.[TR_ItemCode],B.[TRV_IsDefaultVer] desc";
            itemTech = MyDBController.GetDataSet(SQl, ds, "itemTech").Tables["itemTech"];

            SQl = @" SELECT [PN_Code],[PN_Name] FROM [ProcessName]";
            process = MyDBController.GetDataSet(SQl, ds, "process").Tables["process"];

            MyDBController.CloseConnection();

            /* 接下来的代码将获得料品及其对应的工艺路线版本、各版本的具体工序的列表
             * 获得列表后，将listview1的数据源绑定为料品的列表，listview2显示工序信息
             * 只有用户在listview1中选中某行料品记录的时候，绑定相应料品ID+工艺路线版本ID
             * 共同确定的工序信息*/
            GetItemTechList(itemTech, out itemList, out techList);

            //list 的sort委托
            itemList.Sort(delegate(ItemInfoLists x, ItemInfoLists y)
            {
                return x.ID.CompareTo(y.ID);
            });

            listview1.ItemsSource = null;
            listview1.ItemsSource = itemList;
        }

        /// <summary>
        /// 获得由工艺路线的料品的ID列表,列表经过distinct处理,每个料品只对应一个列表项
        /// </summary>
        /// <param name="dt">从条码系统中取得的料品数据表,为本程序的itemTech内部全局变量</param>
        /// <returns>返回List<ItemInfoLists>,作为料品列表的数据源</returns>
        private void GetItemTechList(DataTable dt, out List<ItemInfoLists> iteml, out List<TechRouteLists> techl)
        {
            List<ItemInfoLists> iils = new List<ItemInfoLists> { };
            List<TechRouteLists> trls_local = new List<TechRouteLists> { };
            int x = dt.Rows.Count;
            try
            {
                for (int i = 0; i < x; i++)
                {
                    int y = iils.Count;
                    if (i == 0)
                    {
                        #region
                        //增加料品信息
                        ItemInfoLists iil = new ItemInfoLists();
                        iil.ID = (Int64)dt.Rows[i]["TR_ItemID"];
                        iil.II_Code = dt.Rows[i]["TR_ItemCode"].ToString();
                        iil.II_Name = dt.Rows[i]["II_Name"].ToString();
                        TechVersion tv = new TechVersion();
                        tv.TR_VersionID = (Int64)dt.Rows[i]["TR_VersionID"];
                        tv.TRV_Version = dt.Rows[i]["TRV_Version"].ToString();
                        tv.TRV_IsDefaultVer = (bool)dt.Rows[i]["TRV_IsDefaultVer"];
                        iil.TechVersionList.Add(tv);
                        iils.Add(iil);

                        //增加工序信息
                        TechRouteLists trl = new TechRouteLists();
                        trl.ID = (Int64)dt.Rows[i]["ID"];
                        trl.TR_ItemID = (Int64)dt.Rows[i]["TR_ItemID"];
                        trl.II_Name = dt.Rows[i]["II_Name"].ToString();
                        trl.TR_ItemCode = dt.Rows[i]["TR_ItemCode"].ToString();
                        trl.TR_ProcessName = dt.Rows[i]["TR_ProcessName"].ToString();
                        trl.TR_ProcessCode = dt.Rows[i]["TR_ProcessCode"].ToString();
                        trl.TR_ProcessID = (Int64)dt.Rows[i]["TR_ProcessID"];
                        trl.TR_ProcessSequence = (int)dt.Rows[i]["TR_ProcessSequence"];
                        trl.TR_IsDefaultVer = (bool)dt.Rows[i]["TRV_IsDefaultVer"];
                        trl.TR_VersionID = (Int64)dt.Rows[i]["TR_VersionID"];
                        trl.TR_IsDefaultVer = (bool)dt.Rows[i]["TRV_IsDefaultVer"];
                        trl.TRV_Version = dt.Rows[i]["TRV_Version"].ToString();
                        trl.TR_WagePerPiece = (decimal)dt.Rows[i]["TR_WagePerPiece"];
                        trl.TR_WorkHour = (decimal)dt.Rows[i]["TR_WorkHour"];
                        trl.TR_WageAllotScheme = (int)dt.Rows[i]["TR_WageAllotScheme"];
                        trl.TR_WageAllotScheme_Show = dt.Rows[i]["TR_WageAllotScheme_Show"].ToString();
                        trl.TR_WorkCenterID = (Int64)dt.Rows[i]["TR_WorkCenterID"];
                        trl.WC_Department_Name = dt.Rows[i]["WC_Department_Name"].ToString();
                        trls_local.Add(trl);
                        #endregion
                    }
                    else
                    {
                        if ((Int64)dt.Rows[i]["TR_ItemID"] == (Int64)dt.Rows[i - 1]["TR_ItemID"])
                        //同一个料品
                        {
                            #region
                            if ((Int64)dt.Rows[i]["TR_VersionID"] == (Int64)dt.Rows[i - 1]["TR_VersionID"])
                            //同一个工艺版本
                            {
                                //增加工序信息
                                TechRouteLists trl = new TechRouteLists();
                                trl.ID = (Int64)dt.Rows[i]["ID"];
                                trl.TR_ItemID = (Int64)dt.Rows[i]["TR_ItemID"];
                                trl.II_Name = dt.Rows[i]["II_Name"].ToString();
                                trl.TR_ItemCode = dt.Rows[i]["TR_ItemCode"].ToString();
                                trl.TR_ProcessName = dt.Rows[i]["TR_ProcessName"].ToString();
                                trl.TR_ProcessCode = dt.Rows[i]["TR_ProcessCode"].ToString();
                                trl.TR_ProcessID = (Int64)dt.Rows[i]["TR_ProcessID"];
                                trl.TR_ProcessSequence = (int)dt.Rows[i]["TR_ProcessSequence"];
                                trl.TR_VersionID = (Int64)dt.Rows[i]["TR_VersionID"];
                                trl.TRV_Version = dt.Rows[i]["TRV_Version"].ToString();
                                trl.TR_IsDefaultVer = (bool)dt.Rows[i]["TRV_IsDefaultVer"];
                                trl.TR_WagePerPiece = (decimal)dt.Rows[i]["TR_WagePerPiece"];
                                trl.TR_WorkHour = (decimal)dt.Rows[i]["TR_WorkHour"];
                                trl.TR_WageAllotScheme = (int)dt.Rows[i]["TR_WageAllotScheme"];
                                trl.TR_WageAllotScheme_Show = dt.Rows[i]["TR_WageAllotScheme_Show"].ToString();
                                trl.TR_WorkCenterID = (Int64)dt.Rows[i]["TR_WorkCenterID"];
                                trl.WC_Department_Name = dt.Rows[i]["WC_Department_Name"].ToString();
                                trls_local.Add(trl);
                            }
                            else
                            {
                                //增加料品的工艺版本信息
                                TechVersion tv = new TechVersion();
                                tv.TR_VersionID = (Int64)dt.Rows[i]["TR_VersionID"];
                                tv.TRV_Version = dt.Rows[i]["TRV_Version"].ToString();
                                tv.TRV_IsDefaultVer = (bool)dt.Rows[i]["TRV_IsDefaultVer"];
                                iils[y - 1].TechVersionList.Add(tv);

                                //增加工序信息
                                TechRouteLists trl = new TechRouteLists();
                                trl.ID = (Int64)dt.Rows[i]["ID"];
                                trl.TR_ItemID = (Int64)dt.Rows[i]["TR_ItemID"];
                                trl.II_Name = dt.Rows[i]["II_Name"].ToString();
                                trl.TR_ItemCode = dt.Rows[i]["TR_ItemCode"].ToString();
                                trl.TR_ProcessName = dt.Rows[i]["TR_ProcessName"].ToString();
                                trl.TR_ProcessCode = dt.Rows[i]["TR_ProcessCode"].ToString();
                                trl.TR_ProcessID = (Int64)dt.Rows[i]["TR_ProcessID"];
                                trl.TR_ProcessSequence = (int)dt.Rows[i]["TR_ProcessSequence"];
                                trl.TR_VersionID = (Int64)dt.Rows[i]["TR_VersionID"];
                                trl.TRV_Version = dt.Rows[i]["TRV_Version"].ToString();
                                trl.TR_IsDefaultVer = (bool)dt.Rows[i]["TRV_IsDefaultVer"];
                                trl.TR_WagePerPiece = (decimal)dt.Rows[i]["TR_WagePerPiece"];
                                trl.TR_WorkHour = (decimal)dt.Rows[i]["TR_WorkHour"];
                                trl.TR_WageAllotScheme = (int)dt.Rows[i]["TR_WageAllotScheme"];
                                trl.TR_WageAllotScheme_Show = dt.Rows[i]["TR_WageAllotScheme_Show"].ToString();
                                trl.TR_WorkCenterID = (Int64)dt.Rows[i]["TR_WorkCenterID"];
                                trl.WC_Department_Name = dt.Rows[i]["WC_Department_Name"].ToString();
                                trls_local.Add(trl);
                            }
                            #endregion
                        }

                        else
                        //不是同一个料品
                        {
                            #region
                            //增加料品信息
                            ItemInfoLists iil = new ItemInfoLists();
                            iil.ID = (Int64)dt.Rows[i]["TR_ItemID"];
                            iil.II_Code = dt.Rows[i]["TR_ItemCode"].ToString();
                            iil.II_Name = dt.Rows[i]["II_Name"].ToString();
                            TechVersion tv = new TechVersion();
                            tv.TR_VersionID = (Int64)dt.Rows[i]["TR_VersionID"];
                            tv.TRV_Version = dt.Rows[i]["TRV_Version"].ToString();
                            tv.TRV_IsDefaultVer = (bool)dt.Rows[i]["TRV_IsDefaultVer"];
                            iil.TechVersionList.Add(tv);

                            iils.Add(iil);
                            //增加工序信息
                            TechRouteLists trl = new TechRouteLists();
                            trl.ID = (Int64)dt.Rows[i]["ID"];
                            trl.TR_ItemID = (Int64)dt.Rows[i]["TR_ItemID"];
                            trl.II_Name = dt.Rows[i]["II_Name"].ToString();
                            trl.TR_ItemCode = dt.Rows[i]["TR_ItemCode"].ToString();
                            trl.TR_ProcessName = dt.Rows[i]["TR_ProcessName"].ToString();
                            trl.TR_ProcessCode = dt.Rows[i]["TR_ProcessCode"].ToString();
                            trl.TR_ProcessID = (Int64)dt.Rows[i]["TR_ProcessID"];
                            trl.TR_ProcessSequence = (int)dt.Rows[i]["TR_ProcessSequence"];
                            trl.TR_VersionID = (Int64)dt.Rows[i]["TR_VersionID"];
                            trl.TRV_Version = dt.Rows[i]["TRV_Version"].ToString();
                            trl.TR_IsDefaultVer = (bool)dt.Rows[i]["TRV_IsDefaultVer"];
                            trl.TR_WagePerPiece = (decimal)dt.Rows[i]["TR_WagePerPiece"];
                            trl.TR_WorkHour = (decimal)dt.Rows[i]["TR_WorkHour"];
                            trl.TR_WageAllotScheme = (int)dt.Rows[i]["TR_WageAllotScheme"];
                            trl.TR_WageAllotScheme_Show = dt.Rows[i]["TR_WageAllotScheme_Show"].ToString();
                            trl.TR_WorkCenterID = (Int64)dt.Rows[i]["TR_WorkCenterID"];
                            trl.WC_Department_Name = dt.Rows[i]["WC_Department_Name"].ToString();
                            trls_local.Add(trl);
                            #endregion
                        }
                    }
                }
                iteml = (iils);
                techl = trls_local;
            }
            catch (Exception ee)
            {
                iteml = (iils);
                techl = trls_local;
                MessageBox.Show(ee.Message);
            }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// listview1选中项改变的时候，刷新listview2中对应的工序信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ItemInfoLists itemInfox = listview1.SelectedItem as ItemInfoLists;
                int x = listview1.SelectedIndex;
                if (itemInfox != null)
                {
                    ListViewItem litem = listview1.ItemContainerGenerator.ContainerFromIndex(x) as ListViewItem;
                    List<ComboBox> cbl = FindVisualChild<ComboBox>(litem);
                    trls = new List<TechRouteLists> { };
                    ComboBox cb = cbl[0];
                    Int64 versionID = (Int64)cb.SelectedValue;
                    IEnumerable<TechRouteLists> IEtrls =
                        from item in techList
                        where item.TR_ItemID == itemInfox.ID && item.TR_VersionID == versionID
                        select item;

                    foreach (TechRouteLists item in IEtrls)
                    {
                        trls.Add(item);
                    }
                    listview2.ItemsSource = null;
                    listview2.ItemsSource = trls;
                }
            }
            catch (Exception ee)
            {
            }

        }


        /// <summary>
        /// 添加料品工序按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddItem_Click(object sender, RoutedEventArgs e)
        {
            TechRouteModify_Window trm = new TechRouteModify_Window();
            trm.Title = "工艺路线新增窗口";
            trm.ShowDialog();
            if ((bool)trm.DialogResult)
            {
                //Window_Loaded(sender, e);
                GetBCSItemTechRoute();
                //btn_GetVersion_Click(sender, e);
                formClickCount = 0;
            }
        }

        /// <summary>
        /// 搜索按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            listview1.ItemsSource = itemList;
            if (txtb_SearchKey.Text.Trim().Length > 0)
            {
                string key = txtb_SearchKey.Text.Trim();
                List<ItemInfoLists> iils = new List<ItemInfoLists> { };
                IEnumerable<ItemInfoLists> IEiils =
                    from item in itemList
                    where item.II_Code.IndexOf(key) != -1 || item.II_Name.IndexOf(key) != -1
                    select item;
                foreach (ItemInfoLists item in IEiils)
                {
                    iils.Add(item);
                }
                listview1.ItemsSource = null;
                listview1.ItemsSource = iils;
                //每搜索一次，重新加载一次combobox
                btn_GetVersion_Click(sender, e);
            }
            else
            {
            }
        }

        /// <summary>
        /// 搜索文本框事件关联
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtb_SearchKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            btn_Search_Click(sender, e);
        }

        /// <summary>
        /// 修改工序信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            if (listview1.SelectedItem != null)
            {
                int x = listview1.SelectedIndex;
                ListViewItem litem = listview1.ItemContainerGenerator.ContainerFromIndex(x) as ListViewItem;
                List<ComboBox> cbl = FindVisualChild<ComboBox>(litem);
                if (cbl[0].SelectedIndex == -1)
                {
                    if (MessageBox.Show("您还没有选取工序版本！\n是否现在获取系统工序版本信息？", "提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Information)
                        == MessageBoxResult.Yes)
                    {
                        btn_GetVersion_Click(sender, e);
                    }
                }
                else
                {
                    TechRouteModify_Window trm = new TechRouteModify_Window();
                    trm.Title = "工艺路线修改窗口";
                    trm.trls = trls;
                    trm.selectedItem = listview1.SelectedItem as ItemInfoLists;
                    trm.ShowDialog();
                    if ((bool)trm.DialogResult)
                    {
                        Window_Loaded(sender, e);
                        btn_GetVersion_Click(sender, e);
                    }
                }
            }

        }

        /// <summary>
        /// 为列表中的每一个combobox绑定数据源
        /// 该方法存在缺陷，因为combobox放在datatamplate中，WPF的控件加载机制决定了
        /// 在listview显示区域之外的listviewitem中的控件暂不加载，待到所属listviewitem显示的时候再行加载
        /// 其加载顺序和listview的itemssource的遍历顺序存在错位情况，无法控制。故不用该方法。
        /// datatemplate中的combobox数据源需要在C# code中显式加载.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (cbs.Count < listview1.Items.Count)
            {
                ComboBox cb = (ComboBox)sender;
                ItemInfoLists iil = listview1.Items[cbs.Count] as ItemInfoLists;
                List<TechVersion> tvs = iil.TechVersionList;
                cb.ItemsSource = tvs;
                cb.DisplayMemberPath = "TRV_Version";
                cb.SelectedValuePath = "TR_VersionID";
                cb.SelectedIndex = 0;
                cbs.Add(cb);
            }
            else
            {
                cbs.Clear();
                ComboBox cb = (ComboBox)sender;
                ItemInfoLists iil = listview1.Items[cbs.Count] as ItemInfoLists;

                List<TechVersion> tvs = iil.TechVersionList;
                cb.ItemsSource = tvs;
                cb.DisplayMemberPath = "TRV_Version";
                cb.SelectedValuePath = "TR_VersionID";
                cb.SelectedIndex = 0;
                cbs.Add(cb);
            }
        }

        /// <summary>
        /// 将itemList中的每一个iteminfolists的CB_TechVersion赋值,buyong
        /// </summary>
        private List<ItemInfoLists> SetComboboxSource(List<ItemInfoLists> iils)
        {
            for (int i = 0; i < iils.Count; i++)
            {
                ItemInfoLists iil = iils[i] as ItemInfoLists;
                List<TechVersion> tvs = iil.TechVersionList;
                ComboBox cb = new ComboBox();
                cb.ItemsSource = tvs;
                cb.DisplayMemberPath = "TRV_Version";
                cb.SelectedValuePath = "TR_VersionID";
                cb.SelectedIndex = 0;
                iil.CB_TechVersion = cb;
            }
            return iils;
        }

        /// <summary>
        /// 利用visualtreehelper寻找窗体中的控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private List<T> FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            try
            {
                List<T> TList = new List<T> { };
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        TList.Add((T)child);
                    }
                    else
                    {
                        List<T> childOfChildren = FindVisualChild<T>(child);
                        if (childOfChildren != null)
                        {
                            TList.AddRange(childOfChildren);
                        }
                    }
                }
                return TList;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return null;
            }

        }

        /// <summary>
        /// listview1鼠标左键点击事件,本程序不需要使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            ItemInfoLists itemInfox = listview1.SelectedItem as ItemInfoLists;
            int x = listview1.SelectedIndex;
            if (itemInfox != null)
            {
                ListViewItem litem = listview1.ItemContainerGenerator.ContainerFromIndex(x) as ListViewItem;
                List<ComboBox> cbl = FindVisualChild<ComboBox>(litem);
                trls = new List<TechRouteLists> { };
                Int64 versionID = (Int64)cbl[0].SelectedValue;
                IEnumerable<TechRouteLists> IEtrls =
                    from item in techList
                    where item.TR_ItemID == itemInfox.ID && item.TR_VersionID == versionID
                    select item;

                foreach (TechRouteLists item in IEtrls)
                {
                    trls.Add(item);
                }
                listview2.ItemsSource = null;
                listview2.ItemsSource = trls;
            }
        }

        /// <summary>
        /// 滚动Listview1
        /// 因为combobox是放在datatemplate中的，wpf的加载机制就是加载当前listview中显示区域的
        /// datatemplate中的控件，显示区域之外的控件不加载。不加载的combobox在系统中并没有生成
        /// 变量实例，因为无法为每一个combobox的数据源进行赋值。该方法需要放在window_loaded事件之外！
        /// 故此，采用将listview“从头滚动到底”的方式，强制加载所有的combobox。此方法应该有更好的替代方法，暂时没找到。
        /// </summary>
        private bool ScrollListview(int index)
        {
            if (listview1.Items.Count > 0)
            {
                int count = listview1.Items.Count;
                for (int i = 0; i < count; i++)
                {
                    listview1.ScrollIntoView(listview1.Items[i]);
                }

                if (index == -1)
                {
                    listview1.ScrollIntoView(listview1.Items[0]);
                }
                else if (index > -1)
                {
                    listview1.ScrollIntoView(listview1.Items[index]);
                }
                else
                {

                }
                //listview1.SelectedIndex = index;
            }
            int x = FindVisualChild<ComboBox>(this).Count;
            int y = listview1.Items.Count;
            return x == y;
        }

        /// <summary>
        /// 加载所有的combobox
        /// </summary>
        private void LoadAllCombobox()
        {
            int count = listview1.Items.Count;
            List<ComboBox> cbl = FindVisualChild<ComboBox>(listview1);
            for (int i = 0; i < count; i++)
            {
                //这段代码将combobox和listviewitem内容一一对应起来。
                ItemInfoLists item = listview1.Items[i] as ItemInfoLists;
                cbl[i].ItemsSource = item.TechVersionList;
                cbl[i].DisplayMemberPath = "TRV_Version";
                cbl[i].SelectedValuePath = "TR_VersionID";
                cbl[i].SelectedIndex = 0;
            }
        }


        /// <summary>
        /// 获取版本信息按钮
        /// 程序内部分为1.滚动lietview，让所有datatemplate中控件得以实例化
        /// 2.加载所有datatemplate中的控件
        /// 3.datatemplate控件实例化失败则重复第1步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_GetVersion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = listview1.SelectedIndex;
                bool IsAllLoaded = ScrollListview(index);
                if (IsAllLoaded)
                {
                    LoadAllCombobox();
                }
                else
                {
                    btn_GetVersion_Click(sender, e);
                }
            }
            catch (Exception ee)
            {

                MessageBox.Show(ee.Message);
            }

        }

        /// <summary>
        /// listview的鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview1_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Point formPoint = e.GetPosition(this);
            if (formPoint.Y > 60 && formPoint.Y < 80)//表头部分不作响应
            {
            }
            else
            {
                btn_Modify_Click(sender, e);
            }
        }


        /// <summary>
        /// 窗体鼠标滑动事件，第一次做响应，加载工艺路线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            while (formClickCount == 0)
            {
                btn_GetVersion_Click(sender, e);
                formClickCount++;
            }
        }


    }
}
