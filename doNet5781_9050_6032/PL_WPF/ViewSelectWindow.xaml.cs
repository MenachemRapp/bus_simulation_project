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
    public partial class ViewSelectWindow : Window
    {
        IBL bl;
        public ViewSelectWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;

        }

        private void LineView_clicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.IsEnabled = false;
            LineListWindow lineWin = new LineListWindow(bl);
            lineWin.Show();
            lineWin.Closed += (x, y) => button.IsEnabled = true;
        }

        private void StationView_clicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.IsEnabled = false;
            StationViewWindow stationWin = new StationViewWindow(bl);
            stationWin.Show();
            stationWin.Closed += (x, y) => button.IsEnabled = true;
        }


    }
}
