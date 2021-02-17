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
    public partial class LineListEditWindow : Window
    {

        IBL bl;
        
        public LineListEditWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            this.Closed += CloseChildren;
            RefreshList();
        }

        void RefreshList()
        {
            LinesList.DataContext = bl.GetAllLines().ToList();
        }

        /// <summary>
        /// closes chlidren windows. used when winfow is closed
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void CloseChildren(object sender, EventArgs e)
        {
            List<LinePropertiesWindow> propertiesWindows = Application.Current.Windows.OfType<LinePropertiesWindow>().ToList();
            List<AddLineWindow> addWindows = Application.Current.Windows.OfType<AddLineWindow>().ToList();
           
            foreach (var window in propertiesWindows)
            {
                window.Close();
            }
            foreach (var window in addWindows)
            {
                window.Close();
            }
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
                BO.BasicLine selectedLine = (BO.BasicLine)cmd.DataContext;
                LinePropertiesWindow lineProperties = new LinePropertiesWindow(bl, selectedLine.Id);
                //lineProperties.updateLineAreaEvent += RefreshList;
                lineProperties.SavedLineEvent += (x, y) => RefreshList();
                lineProperties.Show();
            }
        }

       
        private void AddLine_clicked(object sender, RoutedEventArgs e)
        {
            AddLineWindow window = new AddLineWindow(bl);
            window.SavedLineEvent += (x, y) => RefreshList();
            window.Show();
            
        }
    }
}
