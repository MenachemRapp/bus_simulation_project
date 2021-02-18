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
    /// Modify a station
    /// </summary>
    public partial class ModifyStationWindow : Window
    {
        IBL bl;
        BO.Station station;
        public ModifyStationWindow(IBL _bl, int stationCode)
        {
            InitializeComponent();
            bl = _bl;
            station = bl.GetStation(stationCode);
            mainGrid.DataContext = station;
            
        }

        private void saveStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                BO.Station newStation = new BO.Station()
                {
                    Name = txtName.Text,
                    Code = station.Code,
                    Longitude = Convert.ToDouble(txtLongitude.Text),
                    Latitude = Convert.ToDouble(txtLatitude.Text)
                };

                bl.UpdateStation(newStation);
                this.Close();

            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message, "Format Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            catch (BO.BadStationCodeException ex)
            {
                MessageBox.Show(ex.Message, "Code Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
