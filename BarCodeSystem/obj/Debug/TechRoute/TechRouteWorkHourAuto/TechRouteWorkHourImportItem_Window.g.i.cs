﻿#pragma checksum "..\..\..\..\TechRoute\TechRouteWorkHourAuto\TechRouteWorkHourImportItem_Window.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2CA27B9A75F3B509FCC50251469BC377"
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
using BarCodeSystem.PublicClass.ValueConverters;
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


namespace BarCodeSystem.TechRoute.TechRoute {
    
    
    /// <summary>
    /// TechRouteWorkHourImportItem_Window
    /// </summary>
    public partial class TechRouteWorkHourImportItem_Window : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 25 "..\..\..\..\TechRoute\TechRouteWorkHourAuto\TechRouteWorkHourImportItem_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listview1;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\TechRoute\TechRouteWorkHourAuto\TechRouteWorkHourImportItem_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Confirm;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\..\TechRoute\TechRouteWorkHourAuto\TechRouteWorkHourImportItem_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_AllSelect;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\..\TechRoute\TechRouteWorkHourAuto\TechRouteWorkHourImportItem_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_ReSelect;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\TechRoute\TechRouteWorkHourAuto\TechRouteWorkHourImportItem_Window.xaml"
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
            System.Uri resourceLocater = new System.Uri("/BarCodeSystem;component/techroute/techrouteworkhourauto/techrouteworkhourimporti" +
                    "tem_window.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\TechRoute\TechRouteWorkHourAuto\TechRouteWorkHourImportItem_Window.xaml"
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
            
            #line 8 "..\..\..\..\TechRoute\TechRouteWorkHourAuto\TechRouteWorkHourImportItem_Window.xaml"
            ((BarCodeSystem.TechRoute.TechRoute.TechRouteWorkHourImportItem_Window)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.listview1 = ((System.Windows.Controls.ListView)(target));
            return;
            case 3:
            this.btn_Confirm = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\..\..\TechRoute\TechRouteWorkHourAuto\TechRouteWorkHourImportItem_Window.xaml"
            this.btn_Confirm.Click += new System.Windows.RoutedEventHandler(this.btn_Confirm_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btn_AllSelect = ((System.Windows.Controls.Button)(target));
            
            #line 48 "..\..\..\..\TechRoute\TechRouteWorkHourAuto\TechRouteWorkHourImportItem_Window.xaml"
            this.btn_AllSelect.Click += new System.Windows.RoutedEventHandler(this.btn_AllSelect_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btn_ReSelect = ((System.Windows.Controls.Button)(target));
            
            #line 49 "..\..\..\..\TechRoute\TechRouteWorkHourAuto\TechRouteWorkHourImportItem_Window.xaml"
            this.btn_ReSelect.Click += new System.Windows.RoutedEventHandler(this.btn_ReSelect_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btn_Close = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\..\..\TechRoute\TechRouteWorkHourAuto\TechRouteWorkHourImportItem_Window.xaml"
            this.btn_Close.Click += new System.Windows.RoutedEventHandler(this.btn_Close_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

