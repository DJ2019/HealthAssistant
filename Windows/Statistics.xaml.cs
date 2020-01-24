using System.Windows;

namespace HealthAssistant.Windows
{
    /// <summary>
    /// Interaktionslogik für Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {
        public Statistics()
        {
            InitializeComponent();
        }

        #region Back
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dashboard db = new Dashboard();
            db.Show();
            this.Close();
        }
        #endregion
    }
}
