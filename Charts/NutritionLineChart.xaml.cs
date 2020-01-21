﻿using Commons.Classes;
using HealthAssistant.Classes;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HealthAssistant.Charts
{
    /// <summary>
    /// Interaktionslogik für NutritionLineChart.xaml
    /// </summary>
    public partial class NutritionLineChart : UserControl , INotifyPropertyChanged
    {
        #region Variables
        private bool useDailyChart;
        public bool UseDailyChart
        {
            get => useDailyChart;
            set => RaisePropertyChange(ref useDailyChart, value);
        }

        private bool useWeeklyChart;
        public bool UseWeeklyChart
        {
            get => useWeeklyChart;
            set => RaisePropertyChange(ref useWeeklyChart, value);
        }

        private DateTime nutritionLineStartDate;
        public DateTime NutritionLineStartDate
        {
            get => nutritionLineStartDate;
            set => RaisePropertyChange(ref nutritionLineStartDate, value);
        }

        private DateTime nutritionLineEndDate;
        public DateTime NutritionLineEndDate
        {
            get => nutritionLineEndDate;
            set => RaisePropertyChange(ref nutritionLineEndDate, value);
        }

        List<FoodEntry> allEntries { get; set; }

        private SeriesCollection collection;
        public SeriesCollection Collection
        {
            get => collection;
            set => RaisePropertyChange(ref collection, value);
        }

        private string[] labels;
        public string[] Labels
        {
            get => labels;
            set => RaisePropertyChange(ref labels, value);
        }
        public Func<double, string> YFormatter { get; set; }

        #endregion

        public NutritionLineChart()
        {
            InitializeComponent();

            #region Variable initialisation
            NutritionLineEndDate = DateTime.Today;

            NutritionLineStartDate = NutritionLineEndDate.AddMonths(-3);
            #endregion

            UpdateDaily();

            DataContext = this;

            #region auskommentiert
            //collection = new SeriesCollection
            //{
            //    new LineSeries
            //    {
            //        Title = "Kalorien",
            //        Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
            //    },
            //    new LineSeries
            //    {
            //        Title = "Proteins",
            //        Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
            //        PointGeometry = null
            //    },
            //    new LineSeries
            //    {
            //        Title = "Carbs",
            //        Values = new ChartValues<double> { 4,2,7,2,7 },
            //        PointGeometry = DefaultGeometries.Square,
            //        PointGeometrySize = 15
            //    },
            //    new LineSeries
            //    {
            //        Title = "Fate",
            //        Values = new ChartValues<double> { 4,2,7,2,7 },
            //        PointGeometry = DefaultGeometries.Square,
            //        PointGeometrySize = 15
            //    }
            //};
            ////modifying the series collection will animate and update the chart
            //collection.Add(new LineSeries
            //{
            //    Title = "Series 4",
            //    Values = new ChartValues<double> { 5, 3, 2, 4 },
            //    LineSmoothness = 0, //0: straight lines, 1: really smooth lines
            //    PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
            //    PointGeometrySize = 50,
            //    PointForeground = Brushes.Gray
            //});

            ////modifying any series values will also animate and update the chart
            //collection[3].Values.Add(5d);
            #endregion
        }

        #region PropertyChange
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChange<T>(ref T field, T newValue, [CallerMemberName] string propertyname = null)
        {
            field = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        #endregion

        #region Button Click
        private void BtnUpdateChart_Click(object sender, RoutedEventArgs e)
        {
            if (UseDailyChart)
            {
                UpdateDaily();
            }
            else
            {
                UpdateWeekly();
            }

        }
        #endregion

        #region CheckBox Handling
        private void radDaily_Checked(object sender, RoutedEventArgs e)
        {
            UseDailyChart = (bool)((RadioButton)sender).IsChecked;
            UseWeeklyChart = !UseDailyChart;
        }

        private void radWeekly_Checked(object sender, RoutedEventArgs e)
        {
            UseWeeklyChart = (bool)((RadioButton)sender).IsChecked;
            UseDailyChart = !UseWeeklyChart;
        }
        #endregion

        #region Chart Update

        #region Daily
        private void UpdateDaily()
        {
            #region Get x-Axis Labels
            var dates = new List<DateTime>();

            for (var dt = NutritionLineStartDate; dt <= NutritionLineEndDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }
            List<string> labelhelper = new List<string>();
            foreach (DateTime s in dates)
            {
                labelhelper.Add(s.ToShortDateString());
            }
            Labels = labelhelper.ToArray();

            #endregion
            #region Y-Axis Values

            LineSeries proteinSeries = new LineSeries();
            proteinSeries.Title = "Proteins";
            proteinSeries.Values = new ChartValues<double>();
            proteinSeries.LineSmoothness = 0.1;

            LineSeries carbSeries = new LineSeries();
            carbSeries.Title = "Carbs";
            carbSeries.Values = new ChartValues<double>();
            carbSeries.LineSmoothness = 0.1;

            LineSeries fatSeries = new LineSeries();
            fatSeries.Title = "Fate";
            fatSeries.Values = new ChartValues<double>();
            fatSeries.LineSmoothness = 0.1;

            allEntries = PersistentDataProvider.Current.databaseService.GetAllFoodEntries("Foodentries");

            //List<FoodEntry> test = allEntries.Where(x => x.Kalorien > 0).ToList();

            foreach (DateTime s in dates)
            {
                //caloriesSeries.Values.Add(allEntries.Where(x => x.Date == s.ToString()).Sum(x => x.Kalorien));
                proteinSeries.Values.Add(allEntries.Where(x => x.Date == s.ToString()).Sum(x => x.Proteins));
                carbSeries.Values.Add(allEntries.Where(x => x.Date == s.ToString()).Sum(x => x.Carbs));
                fatSeries.Values.Add(allEntries.Where(x => x.Date == s.ToString()).Sum(x => x.Fat));
            }

            Collection = new SeriesCollection
            {
              //caloriesSeries,
              proteinSeries,
              carbSeries,
              fatSeries
            };
            #endregion

            YFormatter = value => value.ToString("N1");
        }
        #endregion

        #region Weekly
        private void UpdateWeekly()
        {

            #region Get x-Axis Labels
            var dates = new List<DateTime>();

            var calendarweeks = new List<CalendarWeekDates>();

            // Gets the Calendar instance associated with a CultureInfo.
            CultureInfo myCI = new CultureInfo("de-AT");
            System.Globalization.Calendar myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            for (var dt = NutritionLineStartDate; dt <= NutritionLineEndDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);

                int i = myCal.GetWeekOfYear(dt, myCWR, myFirstDOW);

                CalendarWeekDates d = new CalendarWeekDates();
                d.Week = "KW " + i + " " + dt.Year.ToString();
                d.WeekInt = i;
                if (!calendarweeks.Where(x => x.Week == d.Week).Any())
                {
                    calendarweeks.Add(d);
                }
            }
            List<string> help = new List<string>();
            foreach (CalendarWeekDates d in calendarweeks)
            {
                List<DateTime> helper = dates.Where(x => myCal.GetWeekOfYear(x, myCWR, myFirstDOW) == Convert.ToInt32(d.WeekInt)).ToList();
                d.DaysOfWeek = helper.Where(x => x.Year.ToString() == d.Week.Substring(d.Week.Length - 4)).Select(x => x.ToString()).ToList();
                if (!help.Contains(d.Week))
                {
                    help.Add(d.Week);
                }
            }

            Labels = help.ToArray();

            #endregion

            #region Y-Axis Values

            LineSeries proteinSeries = new LineSeries();
            proteinSeries.Title = "Proteins";
            proteinSeries.Values = new ChartValues<double>();
            proteinSeries.LineSmoothness = 0.1;

            LineSeries carbSeries = new LineSeries();
            carbSeries.Title = "Carbs";
            carbSeries.Values = new ChartValues<double>();
            carbSeries.LineSmoothness = 0.1;

            LineSeries fatSeries = new LineSeries();
            fatSeries.Title = "Fate";
            fatSeries.Values = new ChartValues<double>();
            fatSeries.LineSmoothness = 0.1;

            allEntries = PersistentDataProvider.Current.databaseService.GetAllFoodEntries("Foodentries");



            foreach (CalendarWeekDates s in calendarweeks)
            {
                if (allEntries.Where(x => s.DaysOfWeek.Contains(x.Date)).Where(y => y.Proteins > 0).Any())
                {
                    proteinSeries.Values.Add(allEntries.Where(x => s.DaysOfWeek.Contains(x.Date)).Average(m => m.Proteins));
                }
                else
                {
                    proteinSeries.Values.Add(0d);
                }
                if (allEntries.Where(x => s.DaysOfWeek.Contains(x.Date)).Where(x => x.Carbs > 0).Any())
                {
                    carbSeries.Values.Add(allEntries.Where(x => s.DaysOfWeek.Contains(x.Date)).Average(x => x.Carbs));
                }
                else
                {
                    carbSeries.Values.Add(0d);
                }
                if (allEntries.Where(x => s.DaysOfWeek.Contains(x.Date)).Where(x => x.Fat > 0).Any())
                {
                    fatSeries.Values.Add(allEntries.Where(x => s.DaysOfWeek.Contains(x.Date)).Average(x => x.Fat));
                }
                else
                {
                    fatSeries.Values.Add(0d);
                }


            }

            Collection = new SeriesCollection
                {
                  //caloriesSeries,
                  proteinSeries,
                  carbSeries,
                  fatSeries
                };
            #endregion

            YFormatter = value => value.ToString("N1");

        }
        #endregion

        #endregion
    }
}