using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;

namespace PL_WPF
{
    /// <summary>
    /// converts a field's vissibilty to be "collapsed" if the distance is "-1". this is used to mark the last stop in the line
    /// </summary>
    public class StationToVisibilityConverter : IValueConverter
    {
        public object Convert(
          object value,
          Type targetType,
          object parameter,
          CultureInfo culture)
        {
            BO.ListedLineStation station1 = (BO.ListedLineStation)value;
            if (station1.ThereIsTimeAndDistance == false && station1.Distance == -1)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(
          object value,
          Type targetType,
          object parameter,
          CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

   /// <summary>
   /// shortens strings whitch are longer than 12 characters to 10 characters and adds "..." in the end
   /// </summary>
    public class ShortenStringConverter : IValueConverter
    {
        public object Convert(
          object value,
          Type targetType,
          object parameter,
          CultureInfo culture)
        {
            string shortString;
            string longString = (string)value;
            if (longString.Length > 12)
            {
                shortString = longString.Substring(0, 10) + "...";
            }
            else
            {
                shortString = longString;
            }

            return shortString;
        }

        public object ConvertBack(
          object value,
          Type targetType,
          object parameter,
          CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

