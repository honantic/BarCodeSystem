﻿#pragma checksum "..\..\..\..\BasicFile\WorkTeam\WorkTeamDepartList_Window.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6EF5016AEE9C08979D77CFCFD3684A67"
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
    /// WorkTeamDepartList_Window
    /// </summary>
    public partial class WorkTeamDepartList_Window : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\..\BasicFile\WorkTeam\WorkTeamDepartList_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lv_listview1;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\BasicFile\WorkTeam\WorkTeamDepartList_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Chose;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\BasicFile\WorkTeam\WorkTeamDepartList_Window.xaml"
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
            System.Uri resourceLocater = new System.Uri("/BarCodeSystem;component/basicfile/workteam/workteamdepartlist_window.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\BasicFile\WorkTeam\WorkTeamDepartList_Window.xaml"
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
            
            #line 7 "..\..\..\..\BasicFile\WorkTeam\WorkTeamDepartList_Window.xaml"
            ((BarCodeSystem.WorkTeamDepartList_Window)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.lv_listview1 = ((System.Windows.Controls.ListView)(target));
            
            #line 22 "..\..\..\..\BasicFile\WorkTeam\WorkTeamDepartList_Window.xaml"
            this.lv_listview1.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.lv_listview1_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btn_Chose = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\..\..\BasicFile\WorkTeam\WorkTeamDepartList_Window.xaml"
            this.btn_Chose.Click += new System.Windows.RoutedEventHandler(this.btn_Chose_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btn_Close = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\..\BasicFile\WorkTeam\WorkTeamDepartList_Window.xaml"
            this.btn_Close.Click += new System.Windows.RoutedEventHandler(this.btn_Close_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

