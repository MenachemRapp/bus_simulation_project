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
    public partial class StationViewWindow : Window
    {
        IBL bl;
        public StationViewWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            Refresh();
        }

        private void Refresh()
        {
            StationListlb.ItemsSource = bl.GetAllStations().ToList();
        }


        private void View_Station_clicked(object sender, RoutedEventArgs e)
        {
            BO.Station SelectedStation = ((sender as Button).DataContext as BO.Station);
            new StationPropertiesWindow(bl, SelectedStation.Code).Show();

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
    }
}
