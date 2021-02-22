using System;
using System.Globalization;
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
    /// Edit line Properties
    /// </summary>
    public partial class LinePropertiesWindow : Window
    {
        public IBL bl;
        BO.LineTotal line;
   
        public LinePropertiesWindow(IBL _bl, int lineId)
        {
            InitializeComponent();
            bl = _bl;
            line = bl.GetLineNew(lineId);
            areacb.ItemsSource = Enum.GetValues(typeof(BO.Areas));
            RefreshStationList();
            RefreshTripList();
           saveButton.IsEnabled = false; //cannot save before changes are made
        }


        private void RefreshStationList()
        {
            stationslb.ItemsSource = line.ListOfStation.
                Select(st => {
                    if (st.index==line.ListOfStation.Count())
                    {
                        return new BO.ListedLineStation
                        {
                            Code = st.Code,
                            index = st.index,
                            Name = st.Name,
                            Time = st.Time,
                            ThereIsTimeAndDistance = false,
                            Distance = -1                            
                        };
                    }
                    else
                    {
                        return st;
                    }

                }).ToList();// mark last station
            stationst.DataContext = line;
            saveButton.IsEnabled = true;
        }
        private void RefreshTripList()
        {
            triplb.ItemsSource = line.ListOfTrips.Where(t=>t.Valid).ToList();
            saveButton.IsEnabled = true;
        }

        private void ModifyStation_Clicked(object sender, RoutedEventArgs e)
        {
            BO.ListedLineStation station = ((sender as Button).DataContext as BO.ListedLineStation);
            AdjacentStationsWindow adjacentStationsWindow = new AdjacentStationsWindow(bl, station.Code, line.ListOfStation.ElementAt(station.index).Code, station.index-1);
           adjacentStationsWindow.SubmitDriveEvent += modifyAdjacent;
            adjacentStationsWindow.ShowDialog();
            
        }

        private void AddNextStation_Clicked(object sender, RoutedEventArgs e)
        {
            BO.ListedLineStation station = ((sender as Button).DataContext as BO.ListedLineStation);
            SelectStationWindow stationsWindow = new SelectStationWindow(bl,station.index);
            stationsWindow.selectStationEvent += AddStationToLine;
            stationsWindow.ShowDialog();
        }

        private void modifyAdjacent(BO.AdjacentStations adjacent, int index)
        {
            line.ListOfStation.ElementAt(index).Time = adjacent.Time;
            line.ListOfStation.ElementAt(index).Distance = adjacent.Distance;
            line.ListOfStation.ElementAt(index).ThereIsTimeAndDistance = true;
            RefreshStationList();
            
        }

        private void areacb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                saveButton.IsEnabled = true;
            }
                      
        }

        private void AddFirstStation_Clicked(object sender, RoutedEventArgs e)
        {
            SelectStationWindow stationsWindow = new SelectStationWindow(bl, 0);
            stationsWindow.selectStationEvent += AddStationToLine;
            stationsWindow.ShowDialog();
        }

        private void AddStationToLine(int newStationCode, int index)
        {
            
            bl.AddStatToLine(newStationCode, index, line);
            RefreshStationList();
        }

        
        private void RemoveStation_Clicked(object sender, RoutedEventArgs e)
        {
            BO.ListedLineStation station = ((sender as Button).DataContext as BO.ListedLineStation);
            bl.DelStatFromLine(station.index-1,line);
            RefreshStationList();
        }

        private void SaveLine_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SaveLine(line);
                SavedLineEvent(sender, e);
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

           
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


        public event EventHandler SavedLineEvent;
    }
    
}
