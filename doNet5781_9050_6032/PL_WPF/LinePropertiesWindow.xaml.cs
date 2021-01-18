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

        private void ModifyBus_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void AddNextBus_Clicked(object sender, RoutedEventArgs e)
        {
            StationsWindow stationsWindow = new StationsWindow(bl);
            stationsWindow.selectStationEvent += AddStation;
            stationsWindow.ShowDialog();
        }

        private void RemoveBus_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void areacb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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

        private void AddStation(BO.Station station)
        {
            RefreshList();
        }

        public delegate void updateLineAreaHandler();
        public event updateLineAreaHandler updateLineAreaEvent;
    }
}
