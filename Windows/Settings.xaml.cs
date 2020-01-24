using Commons.Classes;
using HealthAssistant.Classes;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace HealthAssistant.Windows
{
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            DataContext = PersistentDataProvider.Current;
        }
        #region Back
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();
            this.Close();
        }
        #endregion

        #region Remove Food
        private void ButtonRemoveFood_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = (FrameworkElement)e.OriginalSource;
                var context = (Food)item.DataContext;
            
                PersistentDataProvider.Current.databaseService.DeleteFromTable(context.ID.ToString(), "ID", "Foods");
                PersistentDataProvider.Current.AllFoods.Remove(context);
            }
            catch (System.InvalidOperationException)
            {
                MessageBox.Show("Fehler!");
            }
            catch(System.InvalidCastException)
            {
                MessageBox.Show("Wie wüst wos leeres löschen?");
            }
        }
        #endregion

        #region Save Food
        private void ButtonSaveFood_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = (FrameworkElement)e.OriginalSource;

                var context = (Food)item.DataContext;
            
                Food updated = new Food(context.Name, context.Proteincontent, context.Carbcontent, context.Fatcontent);
                PersistentDataProvider.Current.databaseService.DeleteFromTable(context.ID.ToString(), "ID", "Foods");
                PersistentDataProvider.Current.databaseService.InsertValues("Foods", typeof(Food), updated);
            }
            catch (System.InvalidOperationException)
            {
                MessageBox.Show("Fehler!");
            }
            catch(System.InvalidCastException)
            {
                MessageBox.Show("Wie wüst wos leeres speichern?");
            }
        }
        #endregion

        #region Remove Exercise
        private void ButtonRemoveExercise_Click(object sender, RoutedEventArgs e)
        {
            var item = (FrameworkElement)e.OriginalSource;
            var context = (Exercise)item.DataContext;

            try
            {
                PersistentDataProvider.Current.databaseService.DeleteFromTable(context.ID.ToString(), "ID", "Exercises");
                PersistentDataProvider.Current.AllExercises.Remove(context);

            }
            catch (System.InvalidOperationException)
            {
                MessageBox.Show("Fehler!");
            }
        }

        #endregion

        #region Save Exercise
        private void ButtonSaveExercise_Click(object sender, RoutedEventArgs e)
        {

            var item = (FrameworkElement)e.OriginalSource;
            var context = (Exercise)item.DataContext;

            try
            {
                Exercise updated = new Exercise(context.ExerciseName, context.MuscleGroup);
                PersistentDataProvider.Current.databaseService.DeleteFromTable(context.ExerciseName, "ExerciseName", "Exercises");
                PersistentDataProvider.Current.databaseService.InsertValues("Exercises", typeof(Exercise), updated);
            }
            catch (System.InvalidOperationException)
            {
                MessageBox.Show("Fehler!");
            }
        }
        #endregion

        #region Remove Bodyweight
        private void ButtonRemoveBodyweight_Click(object sender, RoutedEventArgs e)
        {
            var item = (FrameworkElement)e.OriginalSource;
            var context = (Bodyweight)item.DataContext;

            try
            {
                PersistentDataProvider.Current.databaseService.DeleteFromTable(context.ID.ToString(), "ID", "Bodyweights");
                PersistentDataProvider.Current.AllWeights.Remove(context);

            }
            catch (System.InvalidOperationException)
            {
                MessageBox.Show("Fehler!");
            }
        }
        #endregion

        #region Save Bodyweight
        private void ButtonSaveBodyweight_Click(object sender, RoutedEventArgs e)
        {

            var item = (FrameworkElement)e.OriginalSource;

            var context = (Bodyweight)item.DataContext;

            try
            {
                DateTime d;
                if (DateTime.TryParse(context.Date, out d))
                {
                    Bodyweight updated = new Bodyweight(d.ToString(), context.Weight);
                    PersistentDataProvider.Current.databaseService.DeleteFromTable(context.ID.ToString(), "ID", "Bodyweights");
                    PersistentDataProvider.Current.databaseService.InsertValues("Bodyweights", typeof(Bodyweight), updated);
                }
                else
                {
                    MessageBox.Show("Bitte Datum korrigieren!");
                }
            }
            catch (System.InvalidOperationException)
            {
                MessageBox.Show("Fehler!");
            }
        }
        #endregion

        #region Regex
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(",[^0-9]+");
            //Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        #endregion

        #region Target Values
        private void btnProteinarget_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double newtarget = Convert.ToDouble(txtProteinTarget.Text.Replace(".", ","));
                PersistentDataProvider.Current.databaseService.DeleteTable("Targetproteins");
                PersistentDataProvider.Current.databaseService.CreateTable("Targetproteins", typeof(double));
                PersistentDataProvider.Current.databaseService.InsertValues("Targetproteins", typeof(double), newtarget);
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Falsches Format!");
            }
        }

        private void btnCarbTarget_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double newtarget = Convert.ToDouble(txtCarbTarget.Text.Replace(".", ","));
                PersistentDataProvider.Current.databaseService.DeleteTable("Targetcarbs");
                PersistentDataProvider.Current.databaseService.CreateTable("Targetcarbs", typeof(double));
                PersistentDataProvider.Current.databaseService.InsertValues("Targetcarbs", typeof(double), newtarget);
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Falsches Format!");
            }
        }

        private void btnFatTarget_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double newtarget = Convert.ToDouble(txtFatTarget.Text.Replace(".", ","));
                PersistentDataProvider.Current.databaseService.DeleteTable("Targetfats");
                PersistentDataProvider.Current.databaseService.CreateTable("Targetfats", typeof(double));
                PersistentDataProvider.Current.databaseService.InsertValues("Targetfats", typeof(double), newtarget);
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Falsches Format!");
            }
        }
        #endregion
    }
}
