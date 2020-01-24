using Commons.Classes;
using HealthAssistant.Classes;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace HealthAssistant.Charts
{
    //Line Chart to display Workout Volume
    public partial class TrainingVolumeLineChart : UserControl, INotifyPropertyChanged
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

        List<Workout> alleEintrage { get; set; }

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
        public TrainingVolumeLineChart()
        {
            InitializeComponent();

            #region Variable initialisation
            NutritionLineEndDate = DateTime.Today;

            NutritionLineStartDate = NutritionLineEndDate.AddMonths(-3);
            #endregion

            UpdateDaily();

            DataContext = this;
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



            LineSeries trainingSeries = new LineSeries();
            trainingSeries.Title = "Traininsvolumen";
            trainingSeries.Values = new ChartValues<double>();
            trainingSeries.LineSmoothness = 0.1;

            alleEintrage = PersistentDataProvider.Current.databaseService.GetAllWorkouts("Workouts");


            foreach (DateTime s in dates)
            {
                if (alleEintrage.Where(x => x.Date == s).Any())
                {
                    trainingSeries.Values.Add(alleEintrage.Where(x => x.Date == s).SelectMany(p => p.ExerciseList).SelectMany(y => y.Sets).Sum(c => c.Weight * c.Reps));
                }
                else
                {
                    trainingSeries.Values.Add(0d);
                }

            }

            Collection = new SeriesCollection
            {
              trainingSeries
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

            LineSeries trainingSeries = new LineSeries();
            trainingSeries.Title = "Trainingsvolumen";
            trainingSeries.Values = new ChartValues<double>();
            trainingSeries.LineSmoothness = 0.1;

            alleEintrage = PersistentDataProvider.Current.databaseService.GetAllWorkouts("Workouts");

            foreach (CalendarWeekDates s in calendarweeks)
            {
                if (alleEintrage.Where(x => s.DaysOfWeek.Contains(x.Date.ToString())).Any())
                {
                    trainingSeries.Values.Add(alleEintrage.Where(x => s.DaysOfWeek.Contains(x.Date.ToString())).SelectMany(p => p.ExerciseList).SelectMany(y => y.Sets).Sum(c => c.Weight * c.Reps) / alleEintrage.Where(x => s.DaysOfWeek.Contains(x.Date.ToString())).Count());
                }
                else
                {
                    trainingSeries.Values.Add(0d);
                }
            }

            Collection = new SeriesCollection
                    {
                      trainingSeries
                    };
            #endregion

            YFormatter = value => value.ToString("N1");

        }
        #endregion

        #endregion
    }
}
