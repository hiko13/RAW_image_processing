﻿#pragma checksum "..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "F2D50BF89C409111C1E7AEA29B52D355AD575344"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
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


namespace image_processing {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 37 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider Total_Offset_slider;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider R_Offset_slider;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider G_Offset_slider;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider B_Offset_slider;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider Total_Gain_slider;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider R_Gain_slider;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider G_Gain_slider;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider B_Gain_slider;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar ProgressBar1;
        
        #line default
        #line hidden
        
        
        #line 125 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider Gamma_slider;
        
        #line default
        #line hidden
        
        
        #line 171 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image image_processed;
        
        #line default
        #line hidden
        
        
        #line 207 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Address_TextBox;
        
        #line default
        #line hidden
        
        
        #line 235 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Pixel_Position;
        
        #line default
        #line hidden
        
        
        #line 267 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar ProgressBar_wait;
        
        #line default
        #line hidden
        
        
        #line 272 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Pixel_Position_Start;
        
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
            System.Uri resourceLocater = new System.Uri("/image_processing;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
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
            
            #line 17 "..\..\MainWindow.xaml"
            ((image_processing.MainWindow)(target)).DragEnter += new System.Windows.DragEventHandler(this.event_DragEnter);
            
            #line default
            #line hidden
            
            #line 17 "..\..\MainWindow.xaml"
            ((image_processing.MainWindow)(target)).Drop += new System.Windows.DragEventHandler(this.event_Drop);
            
            #line default
            #line hidden
            
            #line 17 "..\..\MainWindow.xaml"
            ((image_processing.MainWindow)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Window_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Total_Offset_slider = ((System.Windows.Controls.Slider)(target));
            
            #line 43 "..\..\MainWindow.xaml"
            this.Total_Offset_slider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.Total_Offset_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.R_Offset_slider = ((System.Windows.Controls.Slider)(target));
            
            #line 52 "..\..\MainWindow.xaml"
            this.R_Offset_slider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.R_Offset_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.G_Offset_slider = ((System.Windows.Controls.Slider)(target));
            
            #line 61 "..\..\MainWindow.xaml"
            this.G_Offset_slider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.G_Offset_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.B_Offset_slider = ((System.Windows.Controls.Slider)(target));
            
            #line 71 "..\..\MainWindow.xaml"
            this.B_Offset_slider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.B_Offset_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Total_Gain_slider = ((System.Windows.Controls.Slider)(target));
            
            #line 80 "..\..\MainWindow.xaml"
            this.Total_Gain_slider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.Total_Gain_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.R_Gain_slider = ((System.Windows.Controls.Slider)(target));
            
            #line 89 "..\..\MainWindow.xaml"
            this.R_Gain_slider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.R_Gain_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.G_Gain_slider = ((System.Windows.Controls.Slider)(target));
            
            #line 98 "..\..\MainWindow.xaml"
            this.G_Gain_slider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.G_Gain_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.B_Gain_slider = ((System.Windows.Controls.Slider)(target));
            
            #line 107 "..\..\MainWindow.xaml"
            this.B_Gain_slider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.B_Gain_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.ProgressBar1 = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 11:
            
            #line 122 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.Gamma_slider = ((System.Windows.Controls.Slider)(target));
            
            #line 132 "..\..\MainWindow.xaml"
            this.Gamma_slider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.Gamma_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 13:
            this.image_processed = ((System.Windows.Controls.Image)(target));
            
            #line 172 "..\..\MainWindow.xaml"
            this.image_processed.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(this.Image_MouseWheel);
            
            #line default
            #line hidden
            
            #line 173 "..\..\MainWindow.xaml"
            this.image_processed.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Image_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 174 "..\..\MainWindow.xaml"
            this.image_processed.MouseMove += new System.Windows.Input.MouseEventHandler(this.Image_MouseMove);
            
            #line default
            #line hidden
            
            #line 175 "..\..\MainWindow.xaml"
            this.image_processed.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.Image_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 14:
            this.Address_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 15:
            
            #line 226 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.FileSaveButton_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.Pixel_Position = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 17:
            this.ProgressBar_wait = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 18:
            this.Pixel_Position_Start = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

