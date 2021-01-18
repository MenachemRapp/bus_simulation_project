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
            BO.Station newStation = new BO.Station();
            newStation.Name = txtName.Text;
            try
            {
                newStation.Code = Convert.ToInt32(txtCode.Text);
                newStation.Longitude = Convert.ToDouble(txtLongitude.Text);
                newStation.Latitude = Convert.ToDouble(txtLatitude.Text);
            }
            catch (FormatException ex)
            {
                
            }
           

        }
    }
}
