﻿#pragma checksum "..\..\..\..\BasicFile\Organization\OrgInfoModify_Window.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5BD885723CF1A54C9EE911708FA01AA9"
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
    /// OrgInfoModify_Window
    /// </summary>
    public partial class OrgInfoModify_Window : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 33 "..\..\..\..\BasicFile\Organization\OrgInfoModify_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txt_OI_Code;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\BasicFile\Organization\OrgInfoModify_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txt_OI_Name;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\BasicFile\Organization\OrgInfoModify_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txt_OI_Remark;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\BasicFile\Organization\OrgInfoModify_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Save;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\BasicFile\Organization\OrgInfoModify_Window.xaml"
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
            System.Uri resourceLocater = new System.Uri("/BarCodeSystem;component/basicfile/organization/orginfomodify_window.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\BasicFile\Organization\OrgInfoModify_Window.xaml"
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
            
            #line 6 "..\..\..\..\BasicFile\Organization\OrgInfoModify_Window.xaml"
            ((BarCodeSystem.OrgInfoModify_Window)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txt_OI_Code = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.txt_OI_Name = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.txt_OI_Remark = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.btn_Save = ((System.Windows.Controls.Button)(target));
            
            #line 45 "..\..\..\..\BasicFile\Organization\OrgInfoModify_Window.xaml"
            this.btn_Save.Click += new System.Windows.RoutedEventHandler(this.btn_Save_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btn_Close = ((System.Windows.Controls.Button)(target));
            
            #line 46 "..\..\..\..\BasicFile\Organization\OrgInfoModify_Window.xaml"
            this.btn_Close.Click += new System.Windows.RoutedEventHandler(this.btn_Close_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

