﻿#pragma checksum "..\..\..\..\SalaryManage\StaffSalaries\StaffSalariesDetail_Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A9EFF44D3AB88D091B84EA960F872261"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Chromes;
using Xceed.Wpf.Toolkit.Core.Converters;
using Xceed.Wpf.Toolkit.Core.Input;
using Xceed.Wpf.Toolkit.Core.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.Panels;
using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Commands;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using Xceed.Wpf.Toolkit.Zoombox;


namespace BarCodeSystem {
    
    
    /// <summary>
    /// StaffSalariesDetail_Page
    /// </summary>
    public partial class StaffSalariesDetail_Page : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\..\..\SalaryManage\StaffSalaries\StaffSalariesDetail_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid datagrid_SalariesInfo;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\SalaryManage\StaffSalaries\StaffSalariesDetail_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbk_tip;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\..\SalaryManage\StaffSalaries\StaffSalariesDetail_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Export;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/BarCodeSystem;component/salarymanage/staffsalaries/staffsalariesdetail_page.xaml" +
                    "", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\SalaryManage\StaffSalaries\StaffSalariesDetail_Page.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 9 "..\..\..\..\SalaryManage\StaffSalaries\StaffSalariesDetail_Page.xaml"
            ((BarCodeSystem.StaffSalariesDetail_Page)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.datagrid_SalariesInfo = ((System.Windows.Controls.DataGrid)(target));
            
            #line 31 "..\..\..\..\SalaryManage\StaffSalaries\StaffSalariesDetail_Page.xaml"
            this.datagrid_SalariesInfo.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.datagrid_SalariesInfo_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.tbk_tip = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.btn_Export = ((System.Windows.Controls.Button)(target));
            
            #line 54 "..\..\..\..\SalaryManage\StaffSalaries\StaffSalariesDetail_Page.xaml"
            this.btn_Export.Click += new System.Windows.RoutedEventHandler(this.btn_Export_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

