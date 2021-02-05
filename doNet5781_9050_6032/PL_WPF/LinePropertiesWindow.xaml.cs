using System;
using System.Globalization;
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
    /// Interaction logic for LinePropertiesWindow.xaml
    /// </summary>
    public partial class LinePropertiesWindow : Window
    {
        public IBL bl;
        
        
        BO.LineTotal line;
   
        public LinePropertiesWindow(IBL _bl, int lineId)
        {
            InitializeComponent();
            bl = _bl;
            line = bl.GetLineNew(lineId);
            areacb.ItemsSource = Enum.GetValues(typeof(BO.Areas));
            RefreshList();
           saveButton.IsEnabled = false;
        }


        private void RefreshList()
        {
            stationslb.ItemsSource = line.ListOfStation.ToList();
            stationst.DataContext = line;
            saveButton.IsEnabled = true;
            var baby = stationslb.ItemTemplate.LoadContent();
            foreach (var stack in FindVisualChildren<StackPanel>(baby))
            {
                if (stack.Name == "codeAndNames")
                {
                    BO.ListedLineStation sts = stack.DataContext as BO.ListedLineStation;
                  //  stack.Visibility = Visibility.Collapsed;
                   // stack.DataContext = Visibility.Collapsed;
                   // stack.DataContext = line.ListOfStation.Last();
                    {

                    }
                }
            }
            var nose1 = baby.DependencyObjectType;
            var nose2 = baby.GetLocalValueEnumerator();
            //baby = stationslb.ItemTemplate.VisualTree;
            foreach (var stack in FindVisualChildren<StackPanel>(baby))
            {
                if (stack.Name == "codeAndNames")
                {
                    BO.ListedLineStation sts = stack.DataContext as BO.ListedLineStation;
                  //  stack.Visibility = Visibility.Collapsed;
                    //stack.DataContext = line.ListOfStation.Last();
                    {

                    }
                }
            }
            if (stationslb.Items.Count>0 && IsLoaded)
            {
               // var baby = stationslb.ItemTemplate.LoadContent();
               
               foreach (var stack in FindVisualChildren<StackPanel>(baby))
                 {
                     if (stack.Name == "codeAndNames")
                     {
                       BO.ListedLineStation sts = stack.DataContext as BO.ListedLineStation;
                        stack.Visibility = Visibility.Collapsed;
                       //if (stack.DataContext)
                        {

                        }
                     }
                 }
                //      StackPanel dataTemplate = FindResource("codeAndNames") as StackPanel;
                DataTemplate dataTemplate = FindResource("BusListAndButtons") as DataTemplate;
                foreach (var item in dataTemplate.Resources)
                {
                    if (item.ToString()== "codeAndNames")
                    {
                        int g = 0;
                    }
                } 
             //  VisualTreeHelper.GetChild(dataTemplate, 0);
              //  ContentPresenter cp = tabControl.Template.FindName("PART_SelectedContentHost", tabControl) as ContentPresenter;
               // StackPanel c = dataTemplate.FindName("codeAndNames", dataTemplate.) as StackPanel;
                // StackPanel stack = FindChild<StackPanel>(dataTemplate, "codeAndNames");
                ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(stationslb);
                DataTemplate yourDataTemplate = contentPresenter.ContentTemplate;
                /*//MediaElement yourMediaElement = yourDataTemplate.FindName("codeAndNames", contentPresenter) as MediaElement;
                if (yourMediaElement != null)
                {
                    // Do something with yourMediaElement here
                }
                */
                
                var m = stationslb;
                VisualTreeHelper.GetChild(stationst, 0);
                //var child = FindChild<StackPanel>(stationst, "distanceAndTime");
                int childCount = VisualTreeHelper.GetChildrenCount(stationslb);
                for (int i = 0; i < childCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(stationslb, 0);

                }
               // var child = VisualTreeHelper.GetChild(stationslb, 0);
                var template = stationslb.Template;
                var myControl = (StackPanel)template.FindName("codeAndNames", stationslb);
                //  m.index
                // DataTemplate stack = (stationslb.Items.GetItemAt(stationslb.Items.Count - 1) as DataTemplate);
                // stack. = Visibility.Hidden;
                //   stationslb.Items.GetItemAt(stationslb.Items.Count - 1).Visibility = Visibility.Hidden;

            }

        }

        private void ModifyStation_Clicked(object sender, RoutedEventArgs e)
        {
            BO.ListedLineStation station = ((sender as Button).DataContext as BO.ListedLineStation);
            AdjacentStationsWindow adjacentStationsWindow = new AdjacentStationsWindow(bl, station.Code, line.ListOfStation.ElementAt(station.index).Code, station.index-1);
           // if (station.ThereIsTimeAndDistance)
                adjacentStationsWindow.SubmitDriveEvent += modifyAdjacent;
           // else
             //   adjacentStationsWindow.SubmitDriveEvent += addAdjacent;
            adjacentStationsWindow.ShowDialog();
            
        }

        private void AddNextStation_Clicked(object sender, RoutedEventArgs e)
        {
            BO.ListedLineStation station = ((sender as Button).DataContext as BO.ListedLineStation);
            SelectStationWindow stationsWindow = new SelectStationWindow(bl,station.index);
            stationsWindow.selectStationEvent += AddStationToLine;
            stationsWindow.ShowDialog();
        }

        private void modifyAdjacent(BO.AdjacentStations adjacent, int index)
        {
            // bl.UpdateAdjacentStations(adjacent);
            line.ListOfStation.ElementAt(index).Time = adjacent.Time;
            line.ListOfStation.ElementAt(index).Distance = adjacent.Distance;
            line.ListOfStation.ElementAt(index).ThereIsTimeAndDistance = true;
            RefreshList();
        }
/*
        private void addAdjacent(BO.AdjacentStations adjacent)
        {
            //bl.AddAdjacentStations(adjacent);
            RefreshList();
        }
*/
        private void areacb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                saveButton.IsEnabled = true;
            }
                      
        }

        private void AddFirstStation_Clicked(object sender, RoutedEventArgs e)
        {
            SelectStationWindow stationsWindow = new SelectStationWindow(bl, 0);
            stationsWindow.selectStationEvent += AddStationToLine;
            stationsWindow.ShowDialog();
        }

        private void AddStationToLine(int newStationCode, int index)
        {
            
            bl.AddStatToLine(newStationCode, index, line);
            RefreshList();
        }

        
        private void RemoveStation_Clicked(object sender, RoutedEventArgs e)
        {
            BO.ListedLineStation station = ((sender as Button).DataContext as BO.ListedLineStation);
            bl.DelStatFromLine(station.index-1,line);
            RefreshList();
        }

        private void SaveLine_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SaveLine(line);
                SavedLineEvent(sender, e);
                Close();
            }
            catch (Exception ex)// type of exception
            {

                MessageBox.Show(ex.Message, "Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

           
        }
     
        public event EventHandler SavedLineEvent;
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)
       where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static childItem FindVisualChild<childItem>(DependencyObject obj)
            where childItem : DependencyObject
        {
            foreach (childItem child in FindVisualChildren<childItem>(obj))
            {
                return child;
            }

            return null;
        }
        public static T FindChild<T>(DependencyObject parent, string childName)
          where T : DependencyObject
        {
            // Confirm parent and childName are valid.
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child.
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                    else
                    {
                        // recursively drill down the tree
                        foundChild = FindChild<T>(child, childName);

                        // If the child is found, break so we do not overwrite the found child.
                        if (foundChild != null) break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
    
}
