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
    /// Interaction logic for LinePropertiesWindow.xaml
    /// </summary>
    public partial class LinePropertiesWindow : Window
    {
        public IBL bl;
        int lineNum;
        BO.LineAndStations line;
        public LinePropertiesWindow(IBL _bl, int lineId)
        {
            InitializeComponent();
            bl = _bl;
            lineNum = lineId;
            areacb.ItemsSource = Enum.GetValues(typeof(BO.Areas));
            RefreshList();
        }


        private void RefreshList()
        {
            line = bl.GetLineAndStations(lineNum);
            stationslb.ItemsSource = line.ListOfStation.ToList();
            stationst.DataContext = line;
        }

        private void ModifyStation_Clicked(object sender, RoutedEventArgs e)
        {
            BO.ListedLineStation station = ((sender as Button).DataContext as BO.ListedLineStation);
            AdjacentStationsWindow adjacentStationsWindow = new AdjacentStationsWindow(bl,line.ListOfStation.ElementAt(line.ListOfStation.ToList().FindIndex(s=>s.Code==station.Code)-1).Code,station.Code);//to change to index
            adjacentStationsWindow.SubmitDriveEvent += modifyStations;
            adjacentStationsWindow.ShowDialog();
        }

        private void AddNextStation_Clicked(object sender, RoutedEventArgs e)
        {
            BO.ListedLineStation station = ((sender as Button).DataContext as BO.ListedLineStation);
            SelectStationWindow stationsWindow = new SelectStationWindow(bl, station.Code);
            stationsWindow.selectStationEvent += AddStationToLine;
            stationsWindow.ShowDialog();
        }

        private void modifyStations(BO.AdjacentStations adjacent)
        {
            bl.UpdateAdjacentStations(adjacent);
            RefreshList();
        }

        private void areacb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //wait for save//////////////////////////////////////////////////////////////////////////
            if (IsLoaded)
            {
                try
                {
                    bl.UpdateLineArea(line.Id, line.Area);
                    MessageBox.Show("Area of the line has changed", "New Area", MessageBoxButton.OK, MessageBoxImage.Information);
                    if(updateLineAreaEvent!=null)
                        updateLineAreaEvent();
                    
                }
                catch (BO.BadLineIdException)
                {
                    MessageBox.Show("unable to update", "Updating Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            
        }

        private void AddFirstStation_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void AddStationToLine(int previuosStationCode, int newStationCode)
        {
            //add here

            RefreshList();
        }

        public delegate void updateLineAreaHandler();
        public event updateLineAreaHandler updateLineAreaEvent;

        private void RemoveStation_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void SaveLine_Clicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
