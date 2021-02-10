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
    /// Interaction logic for LinePropertiesWindow.xaml
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
            RefreshList();
           saveButton.IsEnabled = false;
        }


        private void RefreshList()
        {
            stationslb.ItemsSource = line.ListOfStation.ToList();
            stationst.DataContext = line;
            saveButton.IsEnabled = true;
                     

        }

        private void ModifyStation_Clicked(object sender, RoutedEventArgs e)
        {
            BO.ListedLineStation station = ((sender as Button).DataContext as BO.ListedLineStation);
            AdjacentStationsWindow adjacentStationsWindow = new AdjacentStationsWindow(bl, station.Code, line.ListOfStation.ElementAt(station.index).Code, station.index-1);
           // if (station.ThereIsTimeAndDistance)
                adjacentStationsWindow.SubmitDriveEvent += modifyAdjacent;
           // else
             //   adjacentStationsWindow.SubmitDriveEvent += addAdjacent;
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
            // bl.UpdateAdjacentStations(adjacent);
            line.ListOfStation.ElementAt(index).Time = adjacent.Time;
            line.ListOfStation.ElementAt(index).Distance = adjacent.Distance;
            line.ListOfStation.ElementAt(index).ThereIsTimeAndDistance = true;
            RefreshList();
        }
/*
        private void addAdjacent(BO.AdjacentStations adjacent)
        {
            //bl.AddAdjacentStations(adjacent);
            RefreshList();
        }
*/
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
            RefreshList();
        }

        
        private void RemoveStation_Clicked(object sender, RoutedEventArgs e)
        {
            BO.ListedLineStation station = ((sender as Button).DataContext as BO.ListedLineStation);
            bl.DelStatFromLine(station.index-1,line);
            RefreshList();
        }

        private void SaveLine_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
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
    
    }
    
}
