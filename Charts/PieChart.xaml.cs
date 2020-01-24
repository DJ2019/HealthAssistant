using HealthAssistant.Classes;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Windows.Controls;

namespace HealthAssistant.Charts
{
    //Pie Chart for macro nutrient intake distribution
    public partial class PieChart : UserControl
    {
       
        public PieChart()
        {
            InitializeComponent();
            DataContext = PersistentDataProvider.Current;

            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
        }
        public Func<ChartPoint, string> PointLabel { get; set; }
        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
    
}
