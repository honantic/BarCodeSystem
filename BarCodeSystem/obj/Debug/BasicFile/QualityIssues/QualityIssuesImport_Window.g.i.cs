﻿#pragma checksum "..\..\..\..\BasicFile\QualityIssues\QualityIssuesImport_Window.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1A6F949C32ED27EC3E6DAB990E2CF151"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using BarCodeSystem;
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


namespace BarCodeSystem {
    
    
    /// <summary>
    /// QualityIssuesImport_Window
    /// </summary>
    public partial class QualityIssuesImport_Window : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\..\BasicFile\QualityIssues\QualityIssuesImport_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listview1;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\BasicFile\QualityIssues\QualityIssuesImport_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Copy;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\BasicFile\QualityIssues\QualityIssuesImport_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Import;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\BasicFile\QualityIssues\QualityIssuesImport_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_OutPrint;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\BasicFile\QualityIssues\QualityIssuesImport_Window.xaml"
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
            System.Uri resourceLocater = new System.Uri("/BarCodeSystem;component/basicfile/qualityissues/qualityissuesimport_window.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\BasicFile\QualityIssues\QualityIssuesImport_Window.xaml"
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
            
            #line 7 "..\..\..\..\BasicFile\QualityIssues\QualityIssuesImport_Window.xaml"
            ((BarCodeSystem.QualityIssuesImport_Window)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 7 "..\..\..\..\BasicFile\QualityIssues\QualityIssuesImport_Window.xaml"
            ((BarCodeSystem.QualityIssuesImport_Window)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Window_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.listview1 = ((System.Windows.Controls.ListView)(target));
            return;
            case 3:
            this.btn_Copy = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\..\..\BasicFile\QualityIssues\QualityIssuesImport_Window.xaml"
            this.btn_Copy.Click += new System.Windows.RoutedEventHandler(this.btn_Copy_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btn_Import = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\..\..\BasicFile\QualityIssues\QualityIssuesImport_Window.xaml"
            this.btn_Import.Click += new System.Windows.RoutedEventHandler(this.btn_Import_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btn_OutPrint = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\..\..\BasicFile\QualityIssues\QualityIssuesImport_Window.xaml"
            this.btn_OutPrint.Click += new System.Windows.RoutedEventHandler(this.btn_OutPrint_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btn_Close = ((System.Windows.Controls.Button)(target));
            
            #line 42 "..\..\..\..\BasicFile\QualityIssues\QualityIssuesImport_Window.xaml"
            this.btn_Close.Click += new System.Windows.RoutedEventHandler(this.btn_Close_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

