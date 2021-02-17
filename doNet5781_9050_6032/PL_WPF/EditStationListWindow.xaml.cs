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
    /// Interaction logic for StationViewWindow.xaml
    /// </summary>
    public partial class StationEditWindow : Window
    {
        IBL bl;
        public StationEditWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            Closed += CloseChildren;
            Refresh();
        }


        /// <summary>
        /// closes chlidren windows. used when winfow is closed
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void CloseChildren(object sender, EventArgs e)
        {
            List<StationPropertiesWindow> propertiesWindows = Application.Current.Windows.OfType<StationPropertiesWindow>().ToList();
            List< AddStationWindow> addWindows = Application.Current.Windows.OfType< AddStationWindow> ().ToList();
            List<ModifyStationWindow> modifyWindows = Application.Current.Windows.OfType<ModifyStationWindow>().ToList();

            foreach (var window in propertiesWindows)
            {
                window.Close();
            }
            foreach (var window in addWindows)
            {
                window.Close();
            }
            foreach (var window in modifyWindows)
            {
                window.Close();
            }
        }


        private void Refresh()
        {
            StationListlb.ItemsSource = bl.GetAllStations().ToList();
        }


        private void View_Station_clicked(object sender, RoutedEventArgs e)
        {
            BO.Station SelectedStation = ((sender as Button).DataContext as BO.Station);
            StationPropertiesWindow window = new StationPropertiesWindow(bl, SelectedStation.Code);
            window.DeleteEvent+=(x, y) => Refresh();
            window.Show();
        }

        private void AddStation_clicked(object sender, RoutedEventArgs e)
        {
            AddStationWindow window = new AddStationWindow(bl);
            window.Closing += (x, y) => Refresh();
            window.Show();
        }

        private void Modify_Station_clicked(object sender, RoutedEventArgs e)
        {
            BO.Station SelectedStation = ((sender as Button).DataContext as BO.Station);
            ModifyStationWindow window = new ModifyStationWindow(bl, SelectedStation.Code);
            window.Closing += (x, y) => Refresh();
            window.Show();
        }
        private void Delete_Station_Clicked(object sender, RoutedEventArgs e)
        {
            BO.Station SelectedStation = ((sender as Button).DataContext as BO.Station);
            try
            {
                bl.DeleteStation(SelectedStation.Code);
                Refresh();
            }
            catch (BO.BadStationCodeException ex)
            {
                MessageBox.Show(ex.Message+"\nClick \"view\" for more information.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
