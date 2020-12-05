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
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
        }

        private void addBus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bus myBus = new Bus(txtRegestration.Text, pickDate.DisplayDate);
                MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                mainWindow.addBus(myBus);
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Adding Bus", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
