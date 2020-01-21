using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace HealthAssistant.Services
{
    public class ProgressForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double progress = (double)value;
            var param = (string)parameter;
            var converter = new System.Windows.Media.BrushConverter();
            Brush foreground = Brushes.DeepPink;
            if (param != null)
            {
                foreground = (Brush)converter.ConvertFromString(param);
            }
            else
            {
                foreground = (Brush)converter.ConvertFromString("#2196f3");
            }

            if (progress >= 100)
            {
                foreground = (Brush)converter.ConvertFromString("#c41c00");
            }
            else if (progress >= 80)
            {
                foreground = (Brush)converter.ConvertFromString("#ffeb3b");
            }

            return foreground;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
