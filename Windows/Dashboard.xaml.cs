using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Commons.Classes;
using Commons.Views;
using HealthAssistant.Classes;
using MaterialDesignThemes.Wpf;
using System.Windows.Data;

namespace HealthAssistant.Windows
{

    public partial class Dashboard : Window
        {

            public Dashboard()
            {

                InitializeComponent();

                //Set DataContext to current static PersistentData Instance
                this.DataContext = PersistentDataProvider.Current;

                TrainingCalendar.SelectedDate = DateTime.Today;

            }

            #region Food Entry Handling

            #region Open Food Dialog
            private async void btnOpenFoodDialog_Click(object sender, RoutedEventArgs e)
            {
                //Open DialogHost for Insertion of a nutrition entry
                FoodEntryView view = new FoodEntryView();
                view.FoodSelections = PersistentDataProvider.Current.AllFoods;

                await DialogHost.Show(view, "RootDialog");

            }
            #endregion

            #region Open All Food Entries Dialog
            private async void btnOpenFoodDialogForManipulation_Click(object sender, RoutedEventArgs e)
            {
                //Open DialogHost for Insertion of a nutrition entry
                FoodEntryModificationView view = new FoodEntryModificationView();
                view.AllEntries = new ObservableCollection<FoodEntry>(PersistentDataProvider.Current.databaseService.GetAllFoodEntries("Foodentries"));
                await DialogHost.Show(view, "RootDialog");
            }
            #endregion

            #region Handle Dialog Closing => Add Food
            private void DialogHost_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
            {
                try
                {
                    //Get Parameter and check for type
                    var input = eventArgs.Parameter;

                    //To do: Make Insertion possible for any datetime
                    #region Insert Food Entry
                    if (input.GetType() == typeof(FoodEntryView))
                    {
                        try
                        {
                        //Insert Food Entry into DB
                        FoodEntryView helper = (FoodEntryView)input;

                            if (helper.SelectedFood != null && helper.Weight != 0)
                            {
                                Food lebensmittelHinzugefuegt = helper.SelectedFood;
                                FoodEntry eintrag = new FoodEntry(lebensmittelHinzugefuegt, helper.Weight, helper.SelectedDate.ToString());
                                PersistentDataProvider.Current.databaseService.InsertValues("Foodentries", typeof(FoodEntry), eintrag);
                            }
                            else
                            {
                                MessageBox.Show("Eintrag unvollständig, bitte erneut versuchen.");
                            }
                        }
                        catch (System.InvalidOperationException)
                        {
                            MessageBox.Show("Datum auswählen!");
                        }
                    }
                    #endregion

                }
                catch (System.NullReferenceException)
                {

                }
            }
            #endregion

            #region Food Entry removal
            private void ButtonRemoveFoodEntry_Click(object sender, RoutedEventArgs e)
            {
                var item = (FrameworkElement)e.OriginalSource;
                var context = (FoodEntry)item.DataContext;
                var session = (FoodEntryModificationView)RootDialog.CurrentSession.Content;


                try
                {
                    PersistentDataProvider.Current.databaseService.DeleteFromTable(context.ID.ToString(), "ID", "Foodentries");
                    session.AllEntries.Remove(context);
                    if (session.AllEntries.Count == 0)
                    {
                        RootDialog.CurrentSession.Close();
                    }
                }
                catch (System.InvalidOperationException)
                {
                    MessageBox.Show("Datum auswählen!");
                }
            }

            #endregion

            #endregion

            #region Regex
            private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            #endregion

            #region Calendar Handling
            //When the SelectedDate Property changes, update today's workout; if none exists, create a default one
            private void Calendar_SelectedDatesChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
            {
                DateTime d = (DateTime)TrainingCalendar.SelectedDate;
                Workout training = (from p in PersistentDataProvider.Current.AllWorkouts
                                     where p.Date == d
                                     select p).FirstOrDefault();

                PersistentDataProvider.Current.WorkoutOfToday = training == null ? new Workout() : training;
            }
            //Styling calendar items depending on whether a workout exists on the respective day
            private void calendarButton_Loaded(object sender, EventArgs e)
            {
                CalendarDayButton button = (CalendarDayButton)sender;
                DateTime date = (DateTime)button.DataContext;
                HighlightDay(button, date);
                button.DataContextChanged += new DependencyPropertyChangedEventHandler(calendarButton_DataContextChanged);
            }
            private void calendarButton_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
            {
                CalendarDayButton button = (CalendarDayButton)sender;
                DateTime date = (DateTime)button.DataContext;
                HighlightDay(button, date);
            }
            private void HighlightDay(CalendarDayButton button, DateTime date)
            {
                if (PersistentDataProvider.Current.WorkoutDays.Contains(date))
                    button.Background = Brushes.LawnGreen;
                else
                    button.Background = Brushes.Transparent;
            }
            #endregion

            #region Exercise removal and addition
            #region Add a new exercise to the selected workout instance
            private void btnNewExercise_Click(object sender, RoutedEventArgs e)
            {

                var x = (FrameworkElement)e.OriginalSource;
                var context = (Workout)x.DataContext;
                if (context.ExerciseList != null)
                {
                    context.ExerciseList.Add(new ExerciseEntry()
                    {
                        SelectedExercise = PersistentDataProvider.Current.AllExercises.First(),
                        Sets = new List<ExerciseSet>()
                       {
                           new ExerciseSet()
                           {
                               Reps=0,
                               Weight=0
                           }
                       }
                    });
                }
                else
                {
                    context.ExerciseList = new ObservableCollection<ExerciseEntry>()
                {
                   new ExerciseEntry()
                   {
                       SelectedExercise=PersistentDataProvider.Current.AllExercises.First(),
                       Sets=new List<ExerciseSet>()
                       {
                           new ExerciseSet()
                           {
                               Reps=0,
                               Weight=0
                           }
                       }
                   }
                };
                }
                try
                {
                    context.Date = (DateTime)TrainingCalendar.SelectedDate;
                    string datum = TrainingCalendar.SelectedDate.ToString();
                    PersistentDataProvider.Current.databaseService.DeleteFromTable(datum, "date", "Workouts");
                    PersistentDataProvider.Current.databaseService.InsertValues("Workouts", typeof(Workout), context);
                }
                catch (System.InvalidOperationException)
                {
                    RootDialog.CurrentSession.Close();
                    MessageBox.Show("Datum auswählen!");
                }

            }
            #endregion

            #region Training Editor
            private void SaveTraining_Click(object sender, RoutedEventArgs e)
            {
                //Get Parameter and check for type
                var x = (FrameworkElement)e.OriginalSource;
                var input = (Workout)x.DataContext;

                if (input.GetType() == typeof(Workout))
                {
                    //Insert Training into DB
                    try
                    {
                    Workout helper = (Workout)input;
                        helper.Date = (DateTime)TrainingCalendar.SelectedDate;
                        string datum = TrainingCalendar.SelectedDate.ToString();
                        PersistentDataProvider.Current.databaseService.DeleteFromTable(datum, "date", "Workouts");
                        PersistentDataProvider.Current.databaseService.InsertValues("Workouts", typeof(Workout), helper);
                    }
                    catch (System.InvalidOperationException)
                    {
                        MessageBox.Show("Datum auswählen!");
                    }

                }

            }
            #endregion

            #region Remove exercise from respective workout
            private void ButtonRemoveExercise_Click(object sender, RoutedEventArgs e)
            {
                var item = (FrameworkElement)e.OriginalSource;
                var context = (ExerciseEntry)item.DataContext;
                var training = PersistentDataProvider.Current.WorkoutOfToday;
                training.ExerciseList.Remove(context);

                try
                {
                    string datum = TrainingCalendar.SelectedDate.ToString();
                    PersistentDataProvider.Current.databaseService.DeleteFromTable(datum, "date", "Workouts");
                    PersistentDataProvider.Current.databaseService.InsertValues("Workouts", typeof(Workout), training);
                }
                catch (System.InvalidOperationException)
                {
                    MessageBox.Show("Datum auswählen!");
                }
            }
            #endregion

            #endregion

            #region Open Statistics

            private void OpenStatistics_Click(object sender, RoutedEventArgs e)
            {
                Statistics statistics = new Statistics();
                statistics.Show();
                this.Close();
            }

            #endregion

            #region Open Settings
            private void OpenSettings_Click(object sender, RoutedEventArgs e)
            {
                Settings settings = new Settings();
                settings.Show();
                this.Close();
            }
            #endregion
        }
    }

