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
    /// Add time and distance between stations
    /// </summary>
    public partial class AdjacentStationsWindow : Window
    {
        IBL bl;
        BO.AdjacentStations adjStations= new BO.AdjacentStations();
        int index;
        
        public AdjacentStationsWindow(IBL _bl, int station1, int station2, int _index)
        {
            InitializeComponent();
            bl = _bl;
            adjStations.Station1 = station1;
            adjStations.Station2 = station2;
            index = _index;

            string title = $"Fill in the values of the drive between:\nstation {station1} and station {station2}.";
            titletb.Text = title;
        }

        private void AddStationDrive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                adjStations.Distance = Convert.ToDouble(txtDistance.Text);
                adjStations.Time = TimeSpan.FromMinutes(Convert.ToDouble(txtTime.Text));
                if (adjStations.Time<TimeSpan.Zero || adjStations.Distance<0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (SubmitDriveEvent != null)
                    SubmitDriveEvent(adjStations, index);

                this.Close();

            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message, "Wrong Format", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BadAdjacentStationsException ex)
            {
                MessageBox.Show(ex.Message, "Bad Stations", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Numbers cannot be negative", "Out of Range", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public delegate void SubmittedDriveHandler(BO.AdjacentStations stations, int index);
        public event SubmittedDriveHandler SubmitDriveEvent;

    }



}
