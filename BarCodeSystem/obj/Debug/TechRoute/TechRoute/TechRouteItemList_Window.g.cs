﻿#pragma checksum "..\..\..\..\TechRoute\TechRoute\TechRouteItemList_Window.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DFE8270E9E0BDED12EA66FCE703A29F8"
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


namespace BarCodeSystem {
    
    
    /// <summary>
    /// TechRouteItemList_Window
    /// </summary>
    public partial class TechRouteItemList_Window : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\..\TechRoute\TechRoute\TechRouteItemList_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listview1;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\TechRoute\TechRoute\TechRouteItemList_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.Toolkit.WatermarkTextBox txtb_SearchKey;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\TechRoute\TechRoute\TechRouteItemList_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Chose;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\TechRoute\TechRoute\TechRouteItemList_Window.xaml"
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
            System.Uri resourceLocater = new System.Uri("/BarCodeSystem;component/techroute/techroute/techrouteitemlist_window.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\TechRoute\TechRoute\TechRouteItemList_Window.xaml"
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
            
            #line 8 "..\..\..\..\TechRoute\TechRoute\TechRouteItemList_Window.xaml"
            ((BarCodeSystem.TechRouteItemList_Window)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.listview1 = ((System.Windows.Controls.ListView)(target));
            
            #line 25 "..\..\..\..\TechRoute\TechRoute\TechRouteItemList_Window.xaml"
            this.listview1.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.listview1_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtb_SearchKey = ((Xceed.Wpf.Toolkit.WatermarkTextBox)(target));
            
            #line 33 "..\..\..\..\TechRoute\TechRoute\TechRouteItemList_Window.xaml"
            this.txtb_SearchKey.KeyDown += new System.Windows.Input.KeyEventHandler(this.WatermarkTextBox_KeyDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btn_Chose = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\..\..\TechRoute\TechRoute\TechRouteItemList_Window.xaml"
            this.btn_Chose.Click += new System.Windows.RoutedEventHandler(this.btn_Chose_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btn_Close = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\..\TechRoute\TechRoute\TechRouteItemList_Window.xaml"
            this.btn_Close.Click += new System.Windows.RoutedEventHandler(this.btn_Close_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
