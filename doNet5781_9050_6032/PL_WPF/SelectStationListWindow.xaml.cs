using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    
    public partial class SelectStationWindow : Window
    {
        IBL bl;
        int newStationIndex;
        public SelectStationWindow(IBL _bl, int index)
        {
            InitializeComponent();
            bl = _bl;
            newStationIndex = index;
            Refresh();
        }

        private void Refresh()
        {
            StationListlb.ItemsSource = bl.GetAllStations().ToList();
        }
      

        private void SelectStation_clicked(object sender, RoutedEventArgs e)
        {
            BO.Station addedSelectedStation = ((sender as Button).DataContext as BO.Station);
                   if (selectStationEvent!=null)
                   selectStationEvent(addedSelectedStation.Code, newStationIndex);
                this.Close();

            
        }

        private void AddStation_clicked(object sender, RoutedEventArgs e)
        {
            AddStationWindow window = new AddStationWindow(bl);
            window.Closing += Refresh_closing;
            window.Show();
        }

        private void Refresh_closing(object sender, CancelEventArgs e)
        {
            Refresh();
        }

        public delegate void selectStationHandler(int newStation, int index);
        public event selectStationHandler selectStationEvent;
    }
}
