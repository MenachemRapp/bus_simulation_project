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
            InitializeComponent();
            this.bus = bus;
        }

        
        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            
            TextBox text = sender as TextBox;
            if (text == null) return;
            if (e == null) return;
            
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                if (text.Text!= "" && Convert.ToInt32(text.Text) > 0)
                {
                    if (!bus.drive(Convert.ToInt32(text.Text)))
                    {
                        MessageBox.Show("Bus don't driving", "don't driving", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                        MessageBox.Show("Bus driving", "drive", MessageBoxButton.OK, MessageBoxImage.Information);
                                        
                }
                this.Close();
                
            }

            // It`s a system key (
            if (e.Key == Key.Escape || e.Key == Key.Tab || e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.RightShift ||
                e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl || e.Key == Key.LeftAlt ||
                e.Key == Key.RightAlt || e.Key == Key.LWin || e.Key == Key.RWin || e.Key == Key.System ||
                e.Key == Key.Left || e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Right)
                return;

            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);
            if (Char.IsControl(c)) return;
            
            //if the key is a number
            if (Char.IsNumber(c) || e.Key == Key.NumPad0 || e.Key == Key.NumPad1 || e.Key == Key.NumPad2 || e.Key == Key.NumPad3
                || e.Key == Key.NumPad4 || e.Key == Key.NumPad5 || e.Key == Key.NumPad6 || e.Key == Key.NumPad7
                || e.Key == Key.NumPad8 || e.Key == Key.NumPad9)
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift) ||
                      Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl) ||
                      Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return;
            e.Handled = true;
            MessageBox.Show("Only numbers are allowed", "Wrong Key", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
