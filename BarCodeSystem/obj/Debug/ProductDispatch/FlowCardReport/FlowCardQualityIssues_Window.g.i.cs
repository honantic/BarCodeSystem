﻿#pragma checksum "..\..\..\..\ProductDispatch\FlowCardReport\FlowCardQualityIssues_Window.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "523BBBF82CC10C768035372640B9E624"
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


namespace BarCodeSystem.ProductDispatch.FlowCardReport {
    
    
    /// <summary>
    /// FlowCardQualityIssues_Window
    /// </summary>
    public partial class FlowCardQualityIssues_Window : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 23 "..\..\..\..\ProductDispatch\FlowCardReport\FlowCardQualityIssues_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listview1;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\ProductDispatch\FlowCardReport\FlowCardQualityIssues_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Select;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\ProductDispatch\FlowCardReport\FlowCardQualityIssues_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Close;
        
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
            System.Uri resourceLocater = new System.Uri("/BarCodeSystem;component/productdispatch/flowcardreport/flowcardqualityissues_win" +
                    "dow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\ProductDispatch\FlowCardReport\FlowCardQualityIssues_Window.xaml"
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
            
            #line 9 "..\..\..\..\ProductDispatch\FlowCardReport\FlowCardQualityIssues_Window.xaml"
            ((BarCodeSystem.ProductDispatch.FlowCardReport.FlowCardQualityIssues_Window)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.listview1 = ((System.Windows.Controls.ListView)(target));
            
            #line 23 "..\..\..\..\ProductDispatch\FlowCardReport\FlowCardQualityIssues_Window.xaml"
            this.listview1.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.listview1_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btn_Select = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\..\ProductDispatch\FlowCardReport\FlowCardQualityIssues_Window.xaml"
            this.btn_Select.Click += new System.Windows.RoutedEventHandler(this.btn_Select_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btn_Close = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\..\ProductDispatch\FlowCardReport\FlowCardQualityIssues_Window.xaml"
            this.btn_Close.Click += new System.Windows.RoutedEventHandler(this.btn_Close_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

