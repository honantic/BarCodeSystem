﻿#pragma checksum "..\..\..\..\ProductDispatch\FlowCard\TechRouteSearch_Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0F3D58B48D1129881578800E0CDE242A"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18444
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
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


namespace BarCodeSystem.ProductDispatch.FlowCard {
    
    
    /// <summary>
    /// TechRouteSearch_Page
    /// </summary>
    public partial class TechRouteSearch_Page : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\..\ProductDispatch\FlowCard\TechRouteSearch_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textb_PageTitle;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\ProductDispatch\FlowCard\TechRouteSearch_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid datagrid_RouteVersion;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\..\ProductDispatch\FlowCard\TechRouteSearch_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid datagrid_RouteProcess;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\..\ProductDispatch\FlowCard\TechRouteSearch_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label_ErrorInfo;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\..\ProductDispatch\FlowCard\TechRouteSearch_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Submit;
        
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
            System.Uri resourceLocater = new System.Uri("/BarCodeSystem;component/productdispatch/flowcard/techroutesearch_page.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\ProductDispatch\FlowCard\TechRouteSearch_Page.xaml"
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
            
            #line 8 "..\..\..\..\ProductDispatch\FlowCard\TechRouteSearch_Page.xaml"
            ((BarCodeSystem.ProductDispatch.FlowCard.TechRouteSearch_Page)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.textb_PageTitle = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.datagrid_RouteVersion = ((System.Windows.Controls.DataGrid)(target));
            
            #line 38 "..\..\..\..\ProductDispatch\FlowCard\TechRouteSearch_Page.xaml"
            this.datagrid_RouteVersion.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.datagrid_RouteVersion_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.datagrid_RouteProcess = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 5:
            this.label_ErrorInfo = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.btn_Submit = ((System.Windows.Controls.Button)(target));
            
            #line 63 "..\..\..\..\ProductDispatch\FlowCard\TechRouteSearch_Page.xaml"
            this.btn_Submit.Click += new System.Windows.RoutedEventHandler(this.btn_Submit_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
