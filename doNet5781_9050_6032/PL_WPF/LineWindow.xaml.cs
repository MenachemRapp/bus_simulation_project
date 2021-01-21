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
    /// Interaction logic for LineStationWindow.xaml
    /// </summary>
    public partial class LineListWindow : Window
    {

        IBL bl;
        
        public LineListWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            RefreshList();






        }

        void RefreshList()
        {
            LinesList.DataContext = bl.GetAllLines().ToList();
        }

        

        private void DeleteBusLine_Clicked(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            if (cmd.DataContext is BO.BasicLine)
            {
                BO.BasicLine selectedLine = (BO.BasicLine)cmd.DataContext;
                bl.DeleteLine(selectedLine.Id);
                RefreshList();
            }
        }

        private void PropertiesBusLine_Clicked(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            if (cmd.DataContext is BO.BasicLine)
            {
                cmd.IsEnabled = false;
                BO.BasicLine selectedLine = (BO.BasicLine)cmd.DataContext;
                LinePropertiesWindow lineProperties = new LinePropertiesWindow(bl, selectedLine.Id);
                lineProperties.updateLineAreaEvent += RefreshList;
                lineProperties.Closed += (x,y) => cmd.IsEnabled = true;  
                lineProperties.Show();
            }
        }

       
        private void AddLine_clicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
