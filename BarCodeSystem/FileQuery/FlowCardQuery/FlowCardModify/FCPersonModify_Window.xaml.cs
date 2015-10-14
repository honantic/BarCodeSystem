﻿using BarCodeSystem.ProductDispatch.FlowCard;
using BarCodeSystem.PublicClass.DatabaseEntity;
using BarCodeSystem.TechRoute.TechRoute;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BarCodeSystem.FileQuery.FlowCardQuery.FlowCardModify
{
    /// <summary>
    /// FCPersonModify_Window.xaml 的交互逻辑
    /// </summary>
    public partial class FCPersonModify_Window : Window
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FCPersonModify_Window()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 修改人员的构造函数
        /// </summary>
        /// <param name="_fcl">流转卡主表信息</param>
        /// <param name="_fcslList">修改报工数量之前，流转卡行表信息列表</param>
        /// <param name="_sfcs">返回修改后人员信息的委托函数</param>
        public FCPersonModify_Window(FlowCardLists _fcl, List<FlowCardSubLists> _fcslList, SubmitFlowCardSub _sfcs)
        {
            InitializeComponent();
            fcslList = _fcslList;
            fcl = _fcl;
            sfcs = _sfcs;
        }

        #region 变量
        /// <summary>
        /// 修改报工数量之前，流转卡行表信息列表
        /// </summary>
        public List<FlowCardSubLists> fcslList = new List<FlowCardSubLists>();

        /// <summary>
        /// 添加人员的临时list
        /// </summary>
        private List<FlowCardSubLists> tempList = new List<FlowCardSubLists>();
        /// <summary>
        /// 流转卡主表信息
        /// </summary>
        public FlowCardLists fcl;
        /// <summary>
        /// 是否改变
        /// </summary>
        private bool IsChanged = false;

        /// <summary>
        /// 加载次数
        /// </summary>
        private int loadCount = 0;

        /// <summary>
        /// 返回修改后人员信息的委托函数
        /// </summary>
        private SubmitFlowCardSub sfcs;

        /// <summary>
        /// 是否删除人员
        /// </summary>
        private bool IsDeleted = false;

        #endregion

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (loadCount == 0)
            {
                datagrid_PersonInfo.ItemsSource = fcslList;
                loadCount++;
            }
        }

        /// <summary>
        /// 拖拽事件
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
        /// 扫描人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ScanPerson_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_PersonInfo.HasItems)
            {
                TechRouteCheckPerson_Window trcp = new TechRouteCheckPerson_Window("操作工", "扫描", AcceptPersonInfo);
                trcp.ShowDialog();
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("没有人员信息！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 接收人员信息的委托函数
        /// </summary>
        /// <param name="_plList"></param>
        private void AcceptPersonInfo(List<PersonLists> _plList)
        {
            FlowCardSubLists item = (FlowCardSubLists)datagrid_PersonInfo.Items[0];
            tempList = new List<FlowCardSubLists>();
            foreach (PersonLists pl in _plList)
            {
                FlowCardSubLists newPerson = new FlowCardSubLists()
                {
                    ID = -1,
                    FCS_BeginAmount = item.FCS_BeginAmount,
                    FCS_CheckByID = item.FCS_CheckByID,
                    FCS_CheckByName = item.FCS_CheckByName,
                    FCS_FlowCradID = item.FCS_FlowCradID,
                    FCS_IsFirstProcess = item.FCS_IsFirstProcess,
                    FCS_IsLastProcess = item.FCS_IsLastProcess,
                    FCS_IsReported = item.FCS_IsReported,
                    FCS_ItemId = item.FCS_ItemId,
                    FCS_PieceAmount = item.FCS_PieceAmount,
                    FCS_PieceDivNum = item.FCS_PieceDivNum,
                    FCS_ProcessID = item.FCS_ProcessID,
                    FCS_ProcessName = item.FCS_ProcessName,
                    FCS_ProcessSequanece = item.FCS_ProcessSequanece,
                    FCS_QulifiedAmount = item.FCS_QulifiedAmount,
                    FCS_ScrappedAmount = item.FCS_ScrappedAmount,
                    FCS_TechRouteID = item.FCS_TechRouteID,
                    FCS_AddAmount = item.FCS_AddAmount,
                    FCS_UnprocessedAm = item.FCS_UnprocessedAm,
                    FCS_PersonCode = pl.code,
                    FCS_PersonName = pl.name,
                    FCS_ReportTime = DateTime.Now
                };
                if (!fcslList.Exists(p => p.FCS_PersonCode == newPerson.FCS_PersonCode && p.FCS_ProcessSequanece.Equals(newPerson.FCS_ProcessSequanece)))
                {
                    fcslList.Add(newPerson);
                    tempList.Add(newPerson);
                    datagrid_PersonInfo.ItemsSource = fcslList;
                    datagrid_PersonInfo.Items.Refresh();
                    IsChanged = true;
                }
            }
        }

        /// <summary>
        /// 手工添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddPerson_Click(object sender, RoutedEventArgs e)
        {

            if (datagrid_PersonInfo.HasItems)
            {
                TechRouteCheckPerson_Window trcp = new TechRouteCheckPerson_Window("操作工", "手工", AcceptPersonInfo);
                trcp.ShowDialog();
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("没有人员信息！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DeletePerson_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_PersonInfo.SelectedItem != null)
            {
                FlowCardSubLists.DeleteFCSInfo((FlowCardSubLists)datagrid_PersonInfo.SelectedItem);
                fcslList.RemoveAt(datagrid_PersonInfo.SelectedIndex);
                datagrid_PersonInfo.Items.Refresh();
                IsDeleted = true;
                IsChanged = true;
            }
        }

        /// <summary>
        /// 保存更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (IsChanged)
            {
                bool flag = FlowCardSubLists.SaveFCSInfo(tempList);
                fcslList = FlowCardSubLists.FetchFCS_InfoByFC_Id(fcl.ID);
                if (flag)
                {
                    sfcs.Invoke(fcslList);
                    IsChanged = false;
                    IsDeleted = false;
                    this.DialogResult = true;
                }
            }
        }

        /// <summary>
        /// 取消/关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (IsDeleted)
            {
                fcslList = FlowCardSubLists.FetchFCS_InfoByFC_Id(fcl.ID);
                sfcs.Invoke(fcslList);
                this.DialogResult = true;
            }
            else
            {
                this.Close();
            }
        }

    }
}