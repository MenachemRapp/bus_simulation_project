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

namespace targil3B
{
    /// <summary>
    /// Interaction logic for DistanceWindow.xaml
    /// </summary>
    public partial class DistanceWindow : Window
    {
        Bus bus;
        public DistanceWindow(Bus bus)
        {
            this.bus = bus;
            InitializeComponent();
        }

        private void button_sumbit_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(TextInputKm.Text) > 0)
            {
                if (!bus.drive(Convert.ToInt32(TextInputKm.Text)))
                {
                    MessageBox.Show("Bus don't driving", "don't driving", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Bus driving", "drive", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
        }
    }
}
