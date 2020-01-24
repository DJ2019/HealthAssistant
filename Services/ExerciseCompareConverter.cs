using Commons.Classes;
using HealthAssistant.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HealthAssistant.Services
{
    public class ExerciseCompareConverter : IMultiValueConverter
    {
        //Gets the selected exercise by name 

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var itemssource = values[0] as ObservableCollection<Exercise>;
                var exercise = values[1] as Exercise;
                Exercise returnValue = itemssource.Where(x => x.ExerciseName == exercise.ExerciseName).FirstOrDefault();

                return returnValue;
            }
            catch (System.NullReferenceException)
            {
                return null;
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] {
                PersistentDataProvider.Current.AllExercises, value
            };
        }
    }
}
