﻿#pragma checksum "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C0F332D699514D5D09D7D30767C2B600"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18444
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
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
    /// QualitySort_Window
    /// </summary>
    public partial class QualitySort_Window : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 25 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listview1;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Add;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Modify;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Delete;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_SelectAll;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_ReSelect;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtb_SearchKey;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Search;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
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
            System.Uri resourceLocater = new System.Uri("/BarCodeSystem;component/basicfile/qualitysort/qualitysort_window.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
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
            
            #line 8 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
            ((BarCodeSystem.QualitySort_Window)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.listview1 = ((System.Windows.Controls.ListView)(target));
            
            #line 25 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
            this.listview1.PreviewMouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.listview1_PreviewMouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btn_Add = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
            this.btn_Add.Click += new System.Windows.RoutedEventHandler(this.btn_Add_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btn_Modify = ((System.Windows.Controls.Button)(target));
            
            #line 48 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
            this.btn_Modify.Click += new System.Windows.RoutedEventHandler(this.btn_Modify_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btn_Delete = ((System.Windows.Controls.Button)(target));
            
            #line 49 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
            this.btn_Delete.Click += new System.Windows.RoutedEventHandler(this.btn_Delete_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btn_SelectAll = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
            this.btn_SelectAll.Click += new System.Windows.RoutedEventHandler(this.btn_SelectAll_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btn_ReSelect = ((System.Windows.Controls.Button)(target));
            
            #line 51 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
            this.btn_ReSelect.Click += new System.Windows.RoutedEventHandler(this.btn_ReSelect_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.txtb_SearchKey = ((System.Windows.Controls.TextBox)(target));
            
            #line 52 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
            this.txtb_SearchKey.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtb_SearchKey_TextChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btn_Search = ((System.Windows.Controls.Button)(target));
            
            #line 53 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
            this.btn_Search.Click += new System.Windows.RoutedEventHandler(this.btn_Search_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btn_Close = ((System.Windows.Controls.Button)(target));
            
            #line 54 "..\..\..\..\BasicFile\QualitySort\QualitySort_Window.xaml"
            this.btn_Close.Click += new System.Windows.RoutedEventHandler(this.btn_Close_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

