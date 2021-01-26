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
        BO.LineTotal line;
        public AddLineWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            line = new BO.LineTotal();
            line.ListOfStation = new List<BO.ListedLineStation>();
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
            int index;
            index = line.ListOfStation.Count();
            SelectStationWindow stationsWindow = new SelectStationWindow(bl,index);
            stationsWindow.selectStationEvent += AddStationToLine;
            stationsWindow.ShowDialog();
        }

        private void DeleteStation_Clicked(object sender, RoutedEventArgs e)
        {
            bl.DelStatFromLine(line.ListOfStation.Count()-1, line);
            RefreshList();
        }

        private void AddStationToLine(int newStationCode, int index)
        {

            bl.AddStatToLine(newStationCode, index, line);
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
            catch (Exception)
            {

                throw;
            }
        }
        public event EventHandler SavedLineEvent;
    }
}
