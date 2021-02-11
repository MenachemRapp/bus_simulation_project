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
    /// Interaction logic for AddTripWindow.xaml
    /// </summary>
    public partial class AddTripWindow : Window
    {
        public IBL bl;
        
        public AddTripWindow(IBL _bl)
        {
            bl = _bl;
            InitializeComponent();
        
        }

        private void SaveTrip_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                int houres = Convert.ToInt32(hoursTb.Text);
                if (houres > 23 || houres < 0)
                {
                    throw new ArgumentOutOfRangeException("houres");
                }
                int minutes = Convert.ToInt32(minutesTb.Text);
                if (minutes > 59 || minutes < 0)
                {
                    throw new ArgumentOutOfRangeException("minutes");
                }
                TimeSpan tripTime = TimeSpan.FromHours(houres) + TimeSpan.FromMinutes(minutes);

                if (saveTripEvent!=null)
                {
                    saveTripEvent(tripTime);
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public delegate void saveTripHandler(TimeSpan time);
        public event saveTripHandler saveTripEvent;

    }
}
