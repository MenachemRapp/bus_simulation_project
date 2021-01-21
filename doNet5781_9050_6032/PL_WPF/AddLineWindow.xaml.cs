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
    /// Interaction logic for AddLineWindow.xaml
    /// </summary>
    public partial class AddLineWindow : Window
    {
        public IBL bl;
        BO.LineAndStations line;
        public AddLineWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            line = new BO.LineAndStations(); //bl.GetLineAndStations(1); for test
            areacb.ItemsSource = Enum.GetValues(typeof(BO.Areas));
            areacb.SelectedItem = line.Area;

            RefreshList();
        }

        private void RefreshList()
        {
            if (line.ListOfStation!=null)
            {
                deleteButton.IsEnabled = true;
                stationslb.Visibility = System.Windows.Visibility.Visible;
                stationslb.ItemsSource = line.ListOfStation.ToList();
                saveButton.IsEnabled = (line.ListOfStation.Count() >= 2) ? true : false;
            }
            else
            {
                stationslb.Visibility = System.Windows.Visibility.Collapsed;
                deleteButton.IsEnabled = false;
                saveButton.IsEnabled = false;
            }
            

        }

        private void AddStation_Clicked(object sender, RoutedEventArgs e)
        {

            BO.ListedLineStation station = ((sender as Button).DataContext as BO.ListedLineStation);
            StationsWindow stationsWindow = new StationsWindow(bl);
            stationsWindow.selectStationEvent += AddStationToLine;
            stationsWindow.ShowDialog();
        }

        private void DeleteStation_Clicked(object sender, RoutedEventArgs e)
        {
            //line.ListOfStation;
            RefreshList();
        }

        private void AddStationToLine(int stationCode)
        {
            //BO.ListedLineStation station = bl.get(stationCode);
            //line.ListOfStation.Append(station);
            RefreshList();
        }

        private void SaveLine_Clicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
