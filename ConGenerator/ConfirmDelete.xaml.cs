using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConGenerator
{
    /// <summary>
    /// Lógica de interacción para ConfirmDelete.xaml
    /// </summary>
    public partial class ConfirmDelete : Window
    {
        private CGApi.SpecialFolder ftd { get; set; }
        public ConfirmDelete(CGApi.SpecialFolder ft)
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ftd.Delete();
            this.Close();
        }
    }
}
