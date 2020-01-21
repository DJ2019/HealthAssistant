﻿#pragma checksum "..\..\..\Windows\Dashboard.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "998D762FC60F4C36126BCFE4D672D9E37D75EA1BDF78AC940D210C80C6C5C151"
//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using Commons.Classes;
using Commons.Services;
using Commons.Views;
using HealthAssistant.Charts;
using HealthAssistant.Services;
using HealthAssistant.Windows;
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


namespace HealthAssistant.Windows {
    
    
    /// <summary>
    /// Dashboard
    /// </summary>
    public partial class Dashboard : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 187 "..\..\..\Windows\Dashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.DialogHost RootDialog;
        
        #line default
        #line hidden
        
        
        #line 203 "..\..\..\Windows\Dashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox grpFoods;
        
        #line default
        #line hidden
        
        
        #line 217 "..\..\..\Windows\Dashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox grpProgressbars;
        
        #line default
        #line hidden
        
        
        #line 243 "..\..\..\Windows\Dashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblCalorieProgress;
        
        #line default
        #line hidden
        
        
        #line 245 "..\..\..\Windows\Dashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar prgCalories;
        
        #line default
        #line hidden
        
        
        #line 260 "..\..\..\Windows\Dashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblProteinProgress;
        
        #line default
        #line hidden
        
        
        #line 262 "..\..\..\Windows\Dashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar prgProteins;
        
        #line default
        #line hidden
        
        
        #line 277 "..\..\..\Windows\Dashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblCarbProgress;
        
        #line default
        #line hidden
        
        
        #line 279 "..\..\..\Windows\Dashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar prgCarbs;
        
        #line default
        #line hidden
        
        
        #line 294 "..\..\..\Windows\Dashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblFatProgress;
        
        #line default
        #line hidden
        
        
        #line 296 "..\..\..\Windows\Dashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar prgFats;
        
        #line default
        #line hidden
        
        
        #line 321 "..\..\..\Windows\Dashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox grpFoodlist;
        
        #line default
        #line hidden
        
        
        #line 378 "..\..\..\Windows\Dashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddFood;
        
        #line default
        #line hidden
        
        
        #line 395 "..\..\..\Windows\Dashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox grpTraining;
        
        #line default
        #line hidden
        
        
        #line 415 "..\..\..\Windows\Dashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Calendar TrainingCalendar;
        
        #line default
        #line hidden
        
        
        #line 438 "..\..\..\Windows\Dashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddExercise;
        
        #line default
        #line hidden
        
        
        #line 447 "..\..\..\Windows\Dashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox listUebungen;
        
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
            System.Uri resourceLocater = new System.Uri("/HealthAssistant;component/windows/dashboard.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\Dashboard.xaml"
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
            case 4:
            this.RootDialog = ((MaterialDesignThemes.Wpf.DialogHost)(target));
            
            #line 187 "..\..\..\Windows\Dashboard.xaml"
            this.RootDialog.DialogClosing += new MaterialDesignThemes.Wpf.DialogClosingEventHandler(this.DialogHost_DialogClosing);
            
            #line default
            #line hidden
            return;
            case 5:
            this.grpFoods = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 6:
            this.grpProgressbars = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 7:
            this.lblCalorieProgress = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.prgCalories = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 9:
            this.lblProteinProgress = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.prgProteins = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 11:
            this.lblCarbProgress = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 12:
            this.prgCarbs = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 13:
            this.lblFatProgress = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 14:
            this.prgFats = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 15:
            
            #line 307 "..\..\..\Windows\Dashboard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenStatistics_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 310 "..\..\..\Windows\Dashboard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenSettings_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            this.grpFoodlist = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 18:
            
            #line 370 "..\..\..\Windows\Dashboard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnOpenFoodDialogForManipulation_Click);
            
            #line default
            #line hidden
            return;
            case 19:
            this.btnAddFood = ((System.Windows.Controls.Button)(target));
            
            #line 382 "..\..\..\Windows\Dashboard.xaml"
            this.btnAddFood.Click += new System.Windows.RoutedEventHandler(this.btnOpenFoodDialog_Click);
            
            #line default
            #line hidden
            return;
            case 20:
            this.grpTraining = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 21:
            this.TrainingCalendar = ((System.Windows.Controls.Calendar)(target));
            
            #line 420 "..\..\..\Windows\Dashboard.xaml"
            this.TrainingCalendar.SelectedDatesChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.Calendar_SelectedDatesChanged);
            
            #line default
            #line hidden
            return;
            case 23:
            this.btnAddExercise = ((System.Windows.Controls.Button)(target));
            
            #line 438 "..\..\..\Windows\Dashboard.xaml"
            this.btnAddExercise.Click += new System.Windows.RoutedEventHandler(this.btnNewExercise_Click);
            
            #line default
            #line hidden
            return;
            case 24:
            
            #line 439 "..\..\..\Windows\Dashboard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SaveTraining_Click);
            
            #line default
            #line hidden
            return;
            case 25:
            this.listUebungen = ((System.Windows.Controls.ListBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 1:
            
            #line 103 "..\..\..\Windows\Dashboard.xaml"
            ((System.Windows.Controls.DatePicker)(target)).PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            break;
            case 2:
            
            #line 111 "..\..\..\Windows\Dashboard.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.NumberValidationTextBox);
            
            #line default
            #line hidden
            break;
            case 3:
            
            #line 166 "..\..\..\Windows\Dashboard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonRemoveFoodEntry_Click);
            
            #line default
            #line hidden
            break;
            case 22:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.FrameworkElement.LoadedEvent;
            
            #line 423 "..\..\..\Windows\Dashboard.xaml"
            eventSetter.Handler = new System.Windows.RoutedEventHandler(this.calendarButton_Loaded);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            case 26:
            
            #line 478 "..\..\..\Windows\Dashboard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonRemoveExercise_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

