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
    /// Interaction logic for viewing line list window
    /// </summary>
    public partial class ViewLineListWindow : Window
    {
        IBL bl;

        public ViewLineListWindow(IBL _bl)
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
        /// closes chlidren windows. used when window is closed
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void CloseChildren(object sender, EventArgs e)
        {
            List<ViewLinePropertiesWindow> propertiesWindows = Application.Current.Windows.OfType<ViewLinePropertiesWindow>().ToList();
            
            foreach (var window in propertiesWindows)
            {
                window.Close();
            }
            
        }

                

        private void PropertiesBusLine_Clicked(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            if (cmd.DataContext is BO.BasicLine)
            {
                BO.BasicLine selectedLine = (BO.BasicLine)cmd.DataContext;
                ViewLinePropertiesWindow lineProperties = new ViewLinePropertiesWindow(bl, selectedLine.Id);
                lineProperties.Show();
            }
        }
                        
    }
}
