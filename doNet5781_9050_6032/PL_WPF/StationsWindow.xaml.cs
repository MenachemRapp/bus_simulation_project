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
    /// Interaction logic for StationsWindow.xaml
    /// </summary>

    
    public partial class StationsWindow : Window
    {
        IBL bl;
        public StationsWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            StationListlb.ItemsSource = bl.GetAllStations().ToList();
        }

      

        private void SelectStation_clicked(object sender, RoutedEventArgs e)
        {
            BO.Station addedSelectedStation = ((sender as Button).DataContext as BO.Station);
                   if (selectStationEvent!=null)
                   selectStationEvent(addedSelectedStation.Code);
                this.Close();

            
        }

        private void AddStation_clicked(object sender, RoutedEventArgs e)
        {
            AddStationWindow window = new AddStationWindow(bl);
            window.Show();
        }
        public delegate void selectStationHandler(int stationCode);
        public event selectStationHandler selectStationEvent;
    }
}
