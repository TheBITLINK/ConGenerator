using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConGenerator
{
    /// <summary>
    /// Lógica de interacción para New.xaml
    /// </summary>
    public partial class New : Window
    {
        private string cf { get; set; }
        private string defaultname { get; set; }

        DirectoryInfo de { get; set; }
        public New(string folder)
        {
            cf = folder;
            InitializeComponent();
            de = new DirectoryInfo(cf);
            defaultname = Name.Text;
            destiny.Content = destiny.Content + de.Name;
        }

        private void Name_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "Nombre...")
            {
                Name.Text = "";
                Name.Foreground = Brushes.Black;
            }
        }

        private void Name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text))
            {
                Name.Text = "Nombre...";
                Name.Foreground = (Brush)App.Current.FindResource("NoFocusColor");
            }
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Name.Text.StartsWith("@"))
            {
                DoubleAnimation dA = new DoubleAnimation();
                dA.From = 1;
                dA.To = 0;
                dA.Duration = new TimeSpan(0, 0, 4);
                aterror.BeginAnimation(OpacityProperty, dA);
                Name.Text = Name.Text.Remove(0, 1);
                System.Media.SystemSounds.Asterisk.Play();
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
