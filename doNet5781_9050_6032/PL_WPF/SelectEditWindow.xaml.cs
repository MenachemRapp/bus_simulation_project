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
using BLAPI;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for SimulationAndViewWindow.xaml
    /// </summary>
    public partial class SelectEditWindow : Window
    {
        IBL bl;
        public SelectEditWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            this.Closed += CloseChildren;
        }

        /// <summary>
        /// closes chlidren window. used when winfow is closed
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void CloseChildren(object sender,EventArgs e)
        {
            LineListEditWindow lineWindow = Application.Current.Windows.OfType<LineListEditWindow>().FirstOrDefault();
            LineListEditWindow stationWindow = Application.Current.Windows.OfType<LineListEditWindow>().FirstOrDefault();
            if (lineWindow != null)
                lineWindow.Close();
            if (stationWindow != null)
                stationWindow.Close();
        }

        private void LineView_clicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.IsEnabled = false;
            LineListEditWindow lineWin = new LineListEditWindow(bl);
            lineWin.Show();
            lineWin.Closed += (x, y) => button.IsEnabled = true;
        }

        private void StationView_clicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.IsEnabled = false;
            StationEditWindow stationWin = new StationEditWindow(bl);
            stationWin.Show();
            stationWin.Closed += (x, y) => button.IsEnabled = true;
            this.Closed += (x, y) => stationWin.Close(); 
        }

        

        private void SimulationMode_Click(object sender, RoutedEventArgs e)
        {
            new SimulationAndViewWindow(bl).Show();
            this.Close();
        }
    }
}
