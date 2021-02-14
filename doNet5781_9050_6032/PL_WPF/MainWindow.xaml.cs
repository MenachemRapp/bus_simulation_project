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
using System.Windows.Navigation;
using System.Windows.Shapes;

using BLAPI;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly IBL bl = BLFactory.GetBL("1");
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Edit_clicked(object sender, RoutedEventArgs e)
        {
            new SimulationAndViewWindow(bl).Show();
            this.Close();
        }

        private void View_clicked(object sender, RoutedEventArgs e)
        {
            new ViewSelectWindow(bl).Show();
            this.Close();
        }
    }
}
