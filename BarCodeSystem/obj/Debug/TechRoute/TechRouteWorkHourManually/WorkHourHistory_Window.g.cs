﻿#pragma checksum "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FB6A3B2E263C9621DB424915FD32A49B"
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


namespace BarCodeSystem.TechRoute.TechRouteWorkHourManually {
    
    
    /// <summary>
    /// WorkHourHistory_Window
    /// </summary>
    public partial class WorkHourHistory_Window : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dg_HistoryData;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dg_PresentData;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_ReWrite;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Close;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.Toolkit.WatermarkTextBox txtb_SearchProcess;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dp_StartDateAfter;
        
        #line default
        #line hidden
        
        
        #line 123 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dp_StartDateBefore;
        
        #line default
        #line hidden
        
        
        #line 142 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dp_EndtDateAfter;
        
        #line default
        #line hidden
        
        
        #line 143 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dp_EndDateBefore;
        
        #line default
        #line hidden
        
        
        #line 152 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_SearchInHistory;
        
        #line default
        #line hidden
        
        
        #line 153 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_SearchInAll;
        
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
            System.Uri resourceLocater = new System.Uri("/BarCodeSystem;component/techroute/techrouteworkhourmanually/workhourhistory_wind" +
                    "ow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
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
            
            #line 8 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
            ((BarCodeSystem.TechRoute.TechRouteWorkHourManually.WorkHourHistory_Window)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 9 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
            ((BarCodeSystem.TechRoute.TechRouteWorkHourManually.WorkHourHistory_Window)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dg_HistoryData = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 3:
            this.dg_PresentData = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 4:
            this.btn_ReWrite = ((System.Windows.Controls.Button)(target));
            
            #line 93 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
            this.btn_ReWrite.Click += new System.Windows.RoutedEventHandler(this.btn_ReWrite_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btn_Close = ((System.Windows.Controls.Button)(target));
            
            #line 94 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
            this.btn_Close.Click += new System.Windows.RoutedEventHandler(this.btn_Close_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txtb_SearchProcess = ((Xceed.Wpf.Toolkit.WatermarkTextBox)(target));
            return;
            case 7:
            this.dp_StartDateAfter = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 8:
            this.dp_StartDateBefore = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 9:
            this.dp_EndtDateAfter = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 10:
            this.dp_EndDateBefore = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 11:
            this.btn_SearchInHistory = ((System.Windows.Controls.Button)(target));
            
            #line 152 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
            this.btn_SearchInHistory.Click += new System.Windows.RoutedEventHandler(this.btn_SearchInHistory_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.btn_SearchInAll = ((System.Windows.Controls.Button)(target));
            
            #line 153 "..\..\..\..\TechRoute\TechRouteWorkHourManually\WorkHourHistory_Window.xaml"
            this.btn_SearchInAll.Click += new System.Windows.RoutedEventHandler(this.btn_SearchInAll_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

