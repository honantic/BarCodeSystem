﻿#pragma checksum "..\..\..\..\ProductDispatch\FlowCard\ProduceOrderSearch_Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6A19EEE8077BC8271A4985D597133374"
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


namespace BarCodeSystem.ProductDispatch.FlowCard {
    
    
    /// <summary>
    /// ProduceOrderSearch_Page
    /// </summary>
    public partial class ProduceOrderSearch_Page : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\..\..\ProductDispatch\FlowCard\ProduceOrderSearch_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid datagrid_ProduceOrderInfo;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\ProductDispatch\FlowCard\ProduceOrderSearch_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Submit;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\..\ProductDispatch\FlowCard\ProduceOrderSearch_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.Toolkit.WatermarkTextBox txtb_ItemInfo;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\ProductDispatch\FlowCard\ProduceOrderSearch_Page.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_ItemSearch;
        
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
            System.Uri resourceLocater = new System.Uri("/BarCodeSystem;component/productdispatch/flowcard/produceordersearch_page.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\ProductDispatch\FlowCard\ProduceOrderSearch_Page.xaml"
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
            
            #line 9 "..\..\..\..\ProductDispatch\FlowCard\ProduceOrderSearch_Page.xaml"
            ((BarCodeSystem.ProductDispatch.FlowCard.ProduceOrderSearch_Page)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.datagrid_ProduceOrderInfo = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 3:
            this.btn_Submit = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\..\..\ProductDispatch\FlowCard\ProduceOrderSearch_Page.xaml"
            this.btn_Submit.Click += new System.Windows.RoutedEventHandler(this.btn_Submit_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txtb_ItemInfo = ((Xceed.Wpf.Toolkit.WatermarkTextBox)(target));
            
            #line 49 "..\..\..\..\ProductDispatch\FlowCard\ProduceOrderSearch_Page.xaml"
            this.txtb_ItemInfo.KeyUp += new System.Windows.Input.KeyEventHandler(this.txtb_ItemInfo_KeyUp);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btn_ItemSearch = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\..\..\ProductDispatch\FlowCard\ProduceOrderSearch_Page.xaml"
            this.btn_ItemSearch.Click += new System.Windows.RoutedEventHandler(this.btn_ItemSearch_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

