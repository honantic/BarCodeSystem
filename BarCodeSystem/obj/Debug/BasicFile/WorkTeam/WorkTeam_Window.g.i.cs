﻿#pragma checksum "..\..\..\..\BasicFile\WorkTeam\WorkTeam_Window.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "007C95FC4B48D701A838F356E6FA76FE"
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
using MahApps.Metro.Controls;
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
    /// WorkTeam_Window
    /// </summary>
    public partial class WorkTeam_Window : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\..\BasicFile\WorkTeam\WorkTeam_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listview1;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\BasicFile\WorkTeam\WorkTeam_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Modify;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\BasicFile\WorkTeam\WorkTeam_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Add;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\BasicFile\WorkTeam\WorkTeam_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtb_Search;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\BasicFile\WorkTeam\WorkTeam_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Search;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\BasicFile\WorkTeam\WorkTeam_Window.xaml"
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
            System.Uri resourceLocater = new System.Uri("/BarCodeSystem;component/basicfile/workteam/workteam_window.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\BasicFile\WorkTeam\WorkTeam_Window.xaml"
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
            
            #line 7 "..\..\..\..\BasicFile\WorkTeam\WorkTeam_Window.xaml"
            ((BarCodeSystem.WorkTeam_Window)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.listview1 = ((System.Windows.Controls.ListView)(target));
            
            #line 18 "..\..\..\..\BasicFile\WorkTeam\WorkTeam_Window.xaml"
            this.listview1.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.listview1_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btn_Modify = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\..\BasicFile\WorkTeam\WorkTeam_Window.xaml"
            this.btn_Modify.Click += new System.Windows.RoutedEventHandler(this.btn_Modify_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btn_Add = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\..\..\BasicFile\WorkTeam\WorkTeam_Window.xaml"
            this.btn_Add.Click += new System.Windows.RoutedEventHandler(this.btn_Add_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.txtb_Search = ((System.Windows.Controls.TextBox)(target));
            
            #line 31 "..\..\..\..\BasicFile\WorkTeam\WorkTeam_Window.xaml"
            this.txtb_Search.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtb_Search_KeyDown);
            
            #line default
            #line hidden
            
            #line 31 "..\..\..\..\BasicFile\WorkTeam\WorkTeam_Window.xaml"
            this.txtb_Search.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtb_Search_TextChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btn_Search = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\..\BasicFile\WorkTeam\WorkTeam_Window.xaml"
            this.btn_Search.Click += new System.Windows.RoutedEventHandler(this.btn_Search_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btn_Close = ((System.Windows.Controls.Button)(target));
            
            #line 33 "..\..\..\..\BasicFile\WorkTeam\WorkTeam_Window.xaml"
            this.btn_Close.Click += new System.Windows.RoutedEventHandler(this.btn_Close_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

