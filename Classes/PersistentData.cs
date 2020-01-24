using Commons.Classes;
using HealthAssistant.Services;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HealthAssistant.Classes
{
    //static Provider of current PersistentData instance
    public static class PersistentDataProvider
    {
        private static PersistentData g_Current = new PersistentData();
        public static PersistentData Current
        {
            get
            {
                return g_Current;
            }
        }

    }


    public class PersistentData : INotifyPropertyChanged
    {
        //Initialised in App.xaml.cs

        public event PropertyChangedEventHandler PropertyChanged;
        public DBService databaseService = new DBService();

        public List<string> allTableNames { get; set; }

        #region Nutrition
        #region Bodyweight

        //List of all stored bodyweights
        private ObservableCollection<Bodyweight> allWeights;
        public ObservableCollection<Bodyweight> AllWeights
        {
            get => allWeights;
            set
            {
                value = new ObservableCollection<Bodyweight>(value.OrderBy(x => x.Date));
                CurrentBodyWeight = value.Any() ? value.ToList().OrderByDescending(x => x.Date).FirstOrDefault().Weight : 0;
                RaisePropertyChange(ref allWeights, value);
            }
        }


        private double currentBodyweight;
        public double CurrentBodyWeight
        {
            get => currentBodyweight;
            set
            {
                RaisePropertyChange(ref currentBodyweight, value);
                ProteinProgress = ProteingramsToday / (TargetProteins * CurrentBodyWeight) * 100;
                CarbProgress = CarbgramsToday / (TargetCarbs * CurrentBodyWeight) * 100;
                FatProgress = FatgramsToday / (TargetFats * CurrentBodyWeight) * 100;
                TargetCalories = (TargetProteins * 4.2 + TargetCarbs * 4.1 + TargetFats * 9) * CurrentBodyWeight;
            }
        }
        #endregion

        #region Target Values

        private double targetCalories;
        private double targetProteins;
        private double targetCarbs;
        private double targetFats;

        public double TargetCalories
        {
            get => targetCalories;
            set
            {
                targetCalories = value;
                RaisePropertyChange(ref targetCalories, value);
                CalorieProgress = CaloriesToday / targetCalories * 100;
            }
        }
        public double TargetProteins
        {
            get => targetProteins;
            set
            {
                targetProteins = value;
                RaisePropertyChange(ref targetProteins, value);
                TargetCalories = (TargetProteins * 4.2 + TargetCarbs * 4.1 + TargetFats * 9) * CurrentBodyWeight;
                ProteinProgress = ProteingramsToday / (targetProteins * CurrentBodyWeight) * 100;
            }
        }
        public double TargetCarbs
        {
            get => targetCarbs;
            set
            {
                targetCarbs = value;
                RaisePropertyChange(ref targetCarbs, value);
                TargetCalories = (TargetProteins * 4.2 + TargetCarbs * 4.1 + TargetFats * 9) * CurrentBodyWeight;
                CarbProgress = CarbgramsToday / (targetCarbs * CurrentBodyWeight) * 100;
            }
        }
        public double TargetFats
        {
            get => targetFats;
            set
            {
                targetFats = value;
                RaisePropertyChange(ref targetFats, value);
                TargetCalories = (TargetProteins * 4.2 + TargetCarbs * 4.1 + TargetFats * 9) * CurrentBodyWeight;
                FatProgress = FatgramsToday / (targetFats * CurrentBodyWeight) * 100;
            }
        }
        #endregion

        #region alleLebensmittel
        private ObservableCollection<Food> allFoods;

        public ObservableCollection<Food> AllFoods
        {
            get => allFoods;
            set
            {
                value = new ObservableCollection<Food>(value.OrderBy(x => x.Name));
                RaisePropertyChange(ref allFoods, value);
            }
        }
        #endregion

        #region Progressbar Values
        // Progressbar section of Dashboard

        private double calorieProgress;
        private string calorieProgressDisplayString;
        private double proteinProgress;
        private string proteinProgressDisplayString;
        private double carbProgress;
        private string carbProgressDisplayString;
        private double fatProgress;
        private string fatProgressDisplayString;

        public double CalorieProgress
        {
            get => calorieProgress;
            set
            {
                calorieProgress = value;
                RaisePropertyChange(ref calorieProgress, value);
                CalorieProgressDisplayString = Math.Round(calorieProgress, 1).ToString() + " %";
            }
        }
        public string CalorieProgressDisplayString
        {
            get => calorieProgressDisplayString;
            set
            {
                calorieProgressDisplayString = value;
                RaisePropertyChange(ref calorieProgressDisplayString, value);
            }
        }
        public double ProteinProgress
        {
            get => proteinProgress;
            set
            {
                proteinProgress = value;
                RaisePropertyChange(ref proteinProgress, value);
                ProteinProgressDisplayString = Math.Round(proteinProgress, 1).ToString() + " %";
            }

        }
        public string ProteinProgressDisplayString
        {

            get => proteinProgressDisplayString;
            set
            {
                proteinProgressDisplayString = value;
                RaisePropertyChange(ref proteinProgressDisplayString, value);
            }
        }
        public double CarbProgress
        {
            get => carbProgress;
            set
            {
                carbProgress = value;
                RaisePropertyChange(ref carbProgress, value);
                CarbProgressDisplayString = Math.Round(carbProgress, 1).ToString() + " %";
            }

        }
        public string CarbProgressDisplayString
        {
            get => carbProgressDisplayString;
            set
            {
                carbProgressDisplayString = value;
                RaisePropertyChange(ref carbProgressDisplayString, value);
            }
        }
        public double FatProgress
        {
            get => fatProgress;
            set
            {
                fatProgress = value;
                RaisePropertyChange(ref fatProgress, value);
                FatProgressDisplayString = Math.Round(fatProgress, 1).ToString() + " %";
            }

        }
        public string FatProgressDisplayString
        {
            get => fatProgressDisplayString;
            set
            {
                fatProgressDisplayString = value;
                RaisePropertyChange(ref fatProgressDisplayString, value);
            }
        }
        #endregion

        #region Today's food intake
        //For Pie Chart value calculation

        private double proteingramsToday;
        public double ProteingramsToday
        {
            get => proteingramsToday;
            set
            {
                proteingramsToday = value;
                RaisePropertyChange(ref proteingramsToday, value);
                ProteinProgress = proteingramsToday / (TargetProteins * CurrentBodyWeight) * 100;
                Proteinvalue.Clear();
                Proteinvalue.Add(Math.Round((proteingramsToday / (proteingramsToday + carbgramsToday + fatgramsToday)) * 100, 1));
                Carbvalue.Clear();
                Carbvalue.Add(Math.Round((carbgramsToday / (proteingramsToday + carbgramsToday + fatgramsToday)) * 100, 1));
                Fatvalue.Clear();
                Fatvalue.Add(Math.Round((fatgramsToday / (proteingramsToday + carbgramsToday + fatgramsToday)) * 100, 1));


            }
        }
        private double carbgramsToday;
        public double CarbgramsToday
        {
            get => carbgramsToday;
            set
            {
                carbgramsToday = value;
                RaisePropertyChange(ref carbgramsToday, value);
                CarbProgress = carbgramsToday / (TargetCarbs * CurrentBodyWeight) * 100;
                Proteinvalue.Clear();
                Proteinvalue.Add(Math.Round((proteingramsToday / (proteingramsToday + carbgramsToday + fatgramsToday)) * 100, 1));
                Carbvalue.Clear();
                Carbvalue.Add(Math.Round((carbgramsToday / (proteingramsToday + carbgramsToday + fatgramsToday)) * 100, 1));
                Fatvalue.Clear();
                Fatvalue.Add(Math.Round((fatgramsToday / (proteingramsToday + carbgramsToday + fatgramsToday)) * 100, 1));
            }
        }
        private double fatgramsToday;
        public double FatgramsToday
        {
            get => fatgramsToday;
            set
            {
                fatgramsToday = value;
                RaisePropertyChange(ref fatgramsToday, value);
                FatProgress = fatgramsToday / (TargetFats * CurrentBodyWeight) * 100;
                Proteinvalue.Clear();
                Proteinvalue.Add(Math.Round((proteingramsToday / (proteingramsToday + carbgramsToday + fatgramsToday)) * 100, 1));
                Carbvalue.Clear();
                Carbvalue.Add(Math.Round((carbgramsToday / (proteingramsToday + carbgramsToday + fatgramsToday)) * 100, 1));
                Fatvalue.Clear();
                Fatvalue.Add(Math.Round((fatgramsToday / (proteingramsToday + carbgramsToday + fatgramsToday)) * 100, 1));
            }
        }
        private double caloriesToday;
        public double CaloriesToday
        {
            get => caloriesToday;
            set
            {
                caloriesToday = value;
                RaisePropertyChange(ref caloriesToday, value);
                CalorieProgress = caloriesToday / TargetCalories * 100;
            }
        }

        private ObservableCollection<FoodEntry> foodEntriesToday;
        public ObservableCollection<FoodEntry> FoodEntriesToday
        {
            get => foodEntriesToday;
            set
            {
                RaisePropertyChange(ref foodEntriesToday, value);
                ProteingramsToday = value.Sum(x => x.Proteins);
                CarbgramsToday = value.Sum(x => x.Carbs);
                FatgramsToday = value.Sum(x => x.Fat);
                CaloriesToday = value.Sum(x => x.Calories);

            }
        }
        #endregion

        #region PieChart-Values
        private ChartValues<double> proteinvalue;
        private ChartValues<double> carbvalue;
        private ChartValues<double> fatvalue;


        public ChartValues<double> Proteinvalue
        {
            get => proteinvalue;
            set
            {
                proteinvalue = value;
                RaisePropertyChange(ref proteinvalue, value);
            }
        }
        public ChartValues<double> Carbvalue
        {
            get => carbvalue;
            set
            {
                carbvalue = value;
                RaisePropertyChange(ref carbvalue, value);
            }
        }
        public ChartValues<double> Fatvalue
        {
            get => fatvalue;
            set
            {
                fatvalue = value;
                RaisePropertyChange(ref fatvalue, value);
            }
        }

        #endregion
        #endregion

        #region Workout

        private ObservableCollection<Exercise> allExercises;
        public ObservableCollection<Exercise> AllExercises
        {
            get => allExercises;
            set
            {
                RaisePropertyChange(ref allExercises, value);
            }
        }

        private ObservableCollection<Workout> allWorkouts;
        public ObservableCollection<Workout> AllWorkouts
        {
            get => allWorkouts;
            set
            {
                WorkoutDays = new ObservableCollection<DateTime>(value.Select(x => x.Date));
                RaisePropertyChange(ref allWorkouts, value);
            }
        }

        private Workout workoutOfToday;
        public Workout WorkoutOfToday
        {
            get => workoutOfToday;
            set => RaisePropertyChange(ref workoutOfToday, value);
        }

        //All days with a stored workout 
        private ObservableCollection<DateTime> workoutDays;
        public ObservableCollection<DateTime> WorkoutDays
        {
            get => workoutDays;
            set => RaisePropertyChange(ref workoutDays, value);
        }

        #endregion

        protected virtual void RaisePropertyChange<T>(ref T field, T newValue, [CallerMemberName] string propertyname = null)
        {
            field = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }


    }
}
