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
using System.Globalization;
using BLAPI;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for AddLineWindow.xaml
    /// </summary>
    public partial class AddLineWindow : Window
    {
        public IBL bl;
        BO.NewLine line;
        public AddLineWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            line = new BO.NewLine();
            line.ListOfStation = new List<BO.ListedLineStation>();
            line.ListOfTrips = new List<BO.ListedLineTrip>();
            areacb.ItemsSource = Enum.GetValues(typeof(BO.Areas));
            areacb.SelectedItem = line.Area;

            RefreshStationList();
            RefreshTripList();
        }

        private void RefreshStationList()
        {
            if (line.ListOfStation != null && line.ListOfStation.Count() > 0)
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

        private void RefreshTripList()
        {
            if (line.ListOfTrips != null && line.ListOfTrips.Count() > 0)
            {
                triplb.Visibility = System.Windows.Visibility.Visible;
                triplb.ItemsSource = line.ListOfTrips.ToList();
            }
            else
            {
                triplb.Visibility = System.Windows.Visibility.Collapsed;

            }

        }

        private void AddStation_Clicked(object sender, RoutedEventArgs e)
        {
            int index;
            index = line.ListOfStation.Count();
            SelectStationWindow stationsWindow = new SelectStationWindow(bl, index);
            stationsWindow.selectStationEvent += AddStationToLine;
            stationsWindow.ShowDialog();

        }

        private void DeleteStation_Clicked(object sender, RoutedEventArgs e)
        {
            bl.DelLastStation(line);
            RefreshStationList();
        }

        private void AddStationToLine(int newStationCode, int index)
        {

            //bl.AddStatToLine(newStationCode, index, line);
            /*if (!bl.HasTimeAndDistance(newStationCode, line))
            {
                AdjacentStationsWindow adjacentStationsWindow = new AdjacentStationsWindow(bl, line.ListOfStation.ElementAt(line.ListOfStation.Count() - 1).Code,newStationCode,0);
                adjacentStationsWindow.SubmitDriveEvent +=(adj,x)=> bl.AddAdjacentStations(adj);
                adjacentStationsWindow.ShowDialog();
            }*/
            try
            {
                bl.AddLastStation(newStationCode, line);

            }
            catch (Exception ex)// type of exception
            {

                MessageBox.Show(ex.Message, "Adding Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            RefreshStationList();
            stationslb.ScrollIntoView(stationslb.Items.GetItemAt(stationslb.Items.Count - 1));
        }

        private void SaveLine_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                line.Code = Convert.ToInt32(numbertb.Text);
                bl.SaveLine(line);
                SavedLineEvent(sender, e);
                Close();
            }
            catch (Exception ex)// type of exception
            {
                MessageBox.Show(ex.Message, "Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public event EventHandler SavedLineEvent;

        private void ModifyStation_Clicked(object sender, RoutedEventArgs e)
        {
            BO.ListedLineStation station = ((sender as Button).DataContext as BO.ListedLineStation);
            AdjacentStationsWindow adjacentStationsWindow = new AdjacentStationsWindow(bl, station.Code, line.ListOfStation.ElementAt(station.index).Code, station.index - 1);
            adjacentStationsWindow.SubmitDriveEvent += modifyAdjacent;
            adjacentStationsWindow.ShowDialog();

        }



        private void modifyAdjacent(BO.AdjacentStations adjacent, int index)
        {
            // bl.UpdateAdjacentStations(adjacent);
            /* line.ListOfStation.ElementAt(index).Time = adjacent.Time;
             line.ListOfStation.ElementAt(index).Distance = adjacent.Distance;
             line.ListOfStation.ElementAt(index).ThereIsTimeAndDistance = true;*/
            bl.AddTimeAndDistance(adjacent, line);
            RefreshStationList();

        }

        private void RemoveTrip_Clicked(object sender, RoutedEventArgs e)
        {
            BO.ListedLineTrip trip = ((sender as Button).DataContext as BO.ListedLineTrip);
            bl.DelTripFromLine(trip, line);
            RefreshTripList();
        }

        private void AddTrip_Clicked(object sender, RoutedEventArgs e)
        {

            AddTripWindow tripWindow = new AddTripWindow(bl);
            tripWindow.saveTripEvent += AddTripToLine;
            tripWindow.ShowDialog();
        }

        private void AddTripToLine(TimeSpan tripTime)
        {

            bl.AddTripToLine(tripTime, line);
            RefreshTripList();
        }
    }

}
