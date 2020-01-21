using Commons.Classes;
using HealthAssistant.Classes;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class WeightLineChart : UserControl, INotifyPropertyChanged
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

        List<Bodyweight> alleEintrage { get; set; }

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
        public WeightLineChart()
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

            UpdateDaily();

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

        #region Update Chart
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

            LineSeries weightseries = new LineSeries();
            weightseries.Title = "Körpergewicht";
            weightseries.Values = new ChartValues<double>();

            alleEintrage = PersistentDataProvider.Current.databaseService.GetAllBodyweights("Bodyweights");

            double jetzigesGewicht = 0;

            foreach (DateTime s in dates)
            {
                if (alleEintrage.Where(x => x.Date == s.ToString()).Any())
                {
                    if (alleEintrage.Where(x => x.Date == s.ToString()).FirstOrDefault().Weight != jetzigesGewicht)
                    {
                        jetzigesGewicht = alleEintrage.Where(x => x.Date == s.ToString()).FirstOrDefault().Weight;
                    }
                }

                weightseries.Values.Add(jetzigesGewicht);
            }

            Collection = new SeriesCollection
            {
              weightseries
            };
            #endregion

            YFormatter = value => value.ToString("N1");
        }

        #endregion
    }
}
