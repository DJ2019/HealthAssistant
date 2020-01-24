using Commons.Classes;
using HealthAssistant.Classes;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace HealthAssistant
{
    public partial class App : Application
    {
            public App()
            {
                #region define Culture
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-AT");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-AT");
                FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            #endregion

                #region Create Persistent Data Object
                PersistentData context = new PersistentData();
                PersistentDataProvider.Current.allTableNames = PersistentDataProvider.Current.databaseService.GetAllTables();
                #endregion

                #region Initialise Foods
                if (!PersistentDataProvider.Current.allTableNames.Contains("Foods"))
                {
                    PersistentDataProvider.Current.databaseService.CreateTable("Foods", typeof(Food));

                    #region FoodList
                    List<Food> foodHelperList = new List<Food>()
                    {
                    new Food("Huhn", 23, 1, 1),
                    new Food("Faschiertes (mager)", 20.5, 0, 7),
                    new Food("Faschiertes", 20.3, 1, 8.4),
                    new Food("Truthahn", 24.1, 0, 4),
                    new Food("Fisch (fett)", 21, 0, 15),
                    new Food("Fisch (mager)", 17, 0, 0.8),
                    new Food("Fleisch (mager)", 21, 1, 2),
                    new Food("Casein", 80, 2.6, 4.8),
                    new Food("Whey", 76, 5, 5.6),
                    new Food("Kartoffeln", 1.9, 15.6, 0),
                    new Food("Apfel", 0.3, 14.4, 0.1),
                    new Food("Banane", 1, 20, 0.2),
                    new Food("Vollkornbrot", 6.5, 35, 1.9),
                    new Food("Reis", 6.9, 76.5, 3.2),
                    new Food("Haferflocken", 12.6, 55.7, 7),
                    new Food("Süßkartoffel", 1, 20, 0.1),
                    new Food("Nudeln", 12, 71, 1.2),
                    new Food("Nudeln (Vollkorn)", 12.5, 66, 2.4),
                    new Food("Mais (Dose)", 2.9, 10.8, 1.9),
                    new Food("Bohnen (Dose)", 5.6, 14, 0.5),
                    new Food("Linsen trocken", 25.5, 50, 1.5),
                    new Food("Nüsse", 20, 10, 55),
                    new Food("Erdnussbutter", 30, 12, 46),
                    new Food("Maiskeimöl", 0, 0, 91.4),
                    new Food("Olivenöl", 0, 0, 91.5),
                    new Food("Butter", 0.7, 0.6, 83),
                    new Food("Avocado", 1, 3, 12),
                    new Food("Ei", 12, 1.2, 9.3)
                    };
                    #endregion

                    foreach (Food s in foodHelperList)
                    {
                        PersistentDataProvider.Current.databaseService.InsertValues("Foods", typeof(Food), s);
                    }
                }
                #endregion

                #region Initialise Bodyweight
                if (!PersistentDataProvider.Current.allTableNames.Contains("Bodyweights"))
                {
                    PersistentDataProvider.Current.databaseService.CreateTable("Bodyweights", typeof(Bodyweight));
                    PersistentDataProvider.Current.databaseService.InsertValues("Bodyweights", typeof(Bodyweight), new Bodyweight(DateTime.Today.ToString(), 85));
                }
                #endregion

                #region Initialise Exercises
                if (!PersistentDataProvider.Current.allTableNames.Contains("Exercises"))
                {

                    PersistentDataProvider.Current.databaseService.CreateTable("Exercises", typeof(Exercise));
                    List<Exercise> exerciseHelperList = new List<Exercise>()
                    {
                        new Exercise("Bankdrücken", "Brust"),
                        new Exercise("Bankdrücken schräg", "Brust"),
                        new Exercise("Flies Kurzhantel", "Brust"),
                        new Exercise("Dips", "Brust"),
                        new Exercise("Lat-Ziehen", "Rücken"),
                        new Exercise("Rudern", "Rücken"),
                        new Exercise("Reverse Flies", "Rücken"),
                        new Exercise("Klimmzüge", "Rücken"),
                        new Exercise("Kreuzheben", "Rücken"),
                        new Exercise("Crunches", "Bauch"),
                        new Exercise("Sit-Ups", "Bauch"),
                        new Exercise("Plank", "Bauch"),
                        new Exercise("Bizeps Curls KH", "Ärmel"),
                        new Exercise("Trizepsdrücken KH", "Ärmel"),
                        new Exercise("Bizepscurls Turm", "Ärmel"),
                        new Exercise("Trizepsdrücken Turm", "Ärmel"),
                        new Exercise("Schulterdrücken", "Schultern"),
                        new Exercise("Seitheben", "Schultern"),
                        new Exercise("Frontheben", "Schultern"),
                        new Exercise("Shrugs", "Schultern"),
                        new Exercise("Kniebeugen", "Beine"),
                        new Exercise("Wadendrücker", "Beine")
                    };
                    foreach (Exercise s in exerciseHelperList)
                    {
                        PersistentDataProvider.Current.databaseService.InsertValues("Exercises", typeof(Exercise), s);
                    }
                }


                #endregion

                #region Initialise Chartvalues 
                PersistentDataProvider.Current.Proteinvalue = new ChartValues<double>();
                PersistentDataProvider.Current.Carbvalue = new ChartValues<double>();
                PersistentDataProvider.Current.Fatvalue = new ChartValues<double>();
                #endregion

                #region Initialise Target Values
                if (!PersistentDataProvider.Current.allTableNames.Contains("Targetproteins"))
                {
                    PersistentDataProvider.Current.databaseService.CreateTable("Targetproteins", typeof(double));
                    PersistentDataProvider.Current.databaseService.InsertValues("Targetproteins", typeof(double), 2);
                }
                if (!PersistentDataProvider.Current.allTableNames.Contains("Targetcarbs"))
                {
                    PersistentDataProvider.Current.databaseService.CreateTable("Targetcarbs", typeof(double));
                    PersistentDataProvider.Current.databaseService.InsertValues("Targetcarbs", typeof(double), 2);
                }
                if (!PersistentDataProvider.Current.allTableNames.Contains("Targetfats"))
                {
                    PersistentDataProvider.Current.databaseService.CreateTable("Targetfats", typeof(double));
                    PersistentDataProvider.Current.databaseService.InsertValues("Targetfats", typeof(double), 1.2);
                }
                #endregion

                #region Table Creation of initially empty tables
                PersistentDataProvider.Current.databaseService.CreateTable("Foodentries", typeof(FoodEntry));

                PersistentDataProvider.Current.databaseService.CreateTable("Workouts", typeof(Workout));
                #endregion

                #region Initialise Nutrition Variables
                PersistentDataProvider.Current.AllFoods = new ObservableCollection<Food>(PersistentDataProvider.Current.databaseService.GetAllFoods("Foods"));
                PersistentDataProvider.Current.AllWeights = new ObservableCollection<Bodyweight>(PersistentDataProvider.Current.databaseService.GetAllBodyweights("Bodyweights"));
                PersistentDataProvider.Current.FoodEntriesToday = new ObservableCollection<FoodEntry>(PersistentDataProvider.Current.databaseService.GetFoodEntriesOfToday("Foodentries"));
                PersistentDataProvider.Current.TargetProteins = PersistentDataProvider.Current.databaseService.GetTargetValue("Targetproteins");
                PersistentDataProvider.Current.TargetCarbs = PersistentDataProvider.Current.databaseService.GetTargetValue("Targetcarbs");
                PersistentDataProvider.Current.TargetFats = PersistentDataProvider.Current.databaseService.GetTargetValue("Targetfats");
                #endregion

                #region Initialise Workout Variables

                PersistentDataProvider.Current.AllExercises = new ObservableCollection<Exercise>(PersistentDataProvider.Current.databaseService.GetAllExercises("Exercises"));

                PersistentDataProvider.Current.AllWorkouts = new ObservableCollection<Workout>(PersistentDataProvider.Current.databaseService.GetAllWorkouts("Workouts"));

                #endregion
            }


        }
}
