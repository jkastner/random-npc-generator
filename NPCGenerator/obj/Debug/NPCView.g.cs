﻿#pragma checksum "..\..\NPCView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EA2FA058FCB36FA2CCB330D76EE3A77F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
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


namespace NPCGenerator {
    
    
    /// <summary>
    /// NPCView
    /// </summary>
    public partial class NPCView : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\NPCView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox NPCList_ListBox;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\NPCView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid SingleNPC_DataGrid;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\NPCView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button NewNPC_Button;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\NPCView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SearchBox_TextBox;
        
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
            System.Uri resourceLocater = new System.Uri("/NPCGenerator;component/npcview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\NPCView.xaml"
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
            
            #line 5 "..\..\NPCView.xaml"
            ((NPCGenerator.NPCView)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 6 "..\..\NPCView.xaml"
            ((NPCGenerator.NPCView)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.NPCView_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 18 "..\..\NPCView.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenWold_MenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 22 "..\..\NPCView.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Exit_MenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.NPCList_ListBox = ((System.Windows.Controls.ListBox)(target));
            
            #line 26 "..\..\NPCView.xaml"
            this.NPCList_ListBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.NPCList_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 26 "..\..\NPCView.xaml"
            this.NPCList_ListBox.GotFocus += new System.Windows.RoutedEventHandler(this.SearchBox_GotFocus);
            
            #line default
            #line hidden
            return;
            case 5:
            this.SingleNPC_DataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 6:
            this.NewNPC_Button = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\NPCView.xaml"
            this.NewNPC_Button.Click += new System.Windows.RoutedEventHandler(this.NewNPC_Button_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.SearchBox_TextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 30 "..\..\NPCView.xaml"
            this.SearchBox_TextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SearchBox_TextBox_TextChanged);
            
            #line default
            #line hidden
            
            #line 30 "..\..\NPCView.xaml"
            this.SearchBox_TextBox.GotFocus += new System.Windows.RoutedEventHandler(this.SearchBox_GotFocus);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

