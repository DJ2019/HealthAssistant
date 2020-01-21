using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
