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
    /// Interaction logic for AddStationWindow.xaml
    /// </summary>
    public partial class AddStationWindow : Window
    {
        public IBL bl;
        public AddStationWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl; 
        }

        private void addStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Station newStation = new BO.Station()
                {
                    Name = txtName.Text,
                    Code = Convert.ToInt32(txtCode.Text),
                    Longitude = Convert.ToDouble(txtLongitude.Text),
                    Latitude = Convert.ToDouble(txtLatitude.Text)
                };
                if (newStation.Code<0 || newStation.Code>999999)
                {
                    throw new ArgumentOutOfRangeException();
                }

               bl.AddStation(newStation);
                this.Close();
                
                
                

            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message,"Format Error",  MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Code must be 6 digits", "Code Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BadStationCodeException ex)
            {
               MessageBox.Show(ex.Message, "Code Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
