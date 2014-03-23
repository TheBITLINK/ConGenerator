using CGApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConGenerator
{
	/// <summary>
	/// Lógica de interacción para SpecialFolderitem.xaml
	/// </summary>
	public partial class SpecialFolderitem : UserControl
	{
        private SpecialFolder spf { get; set; }
		public SpecialFolderitem(SpecialFolder SPF)
		{
			this.InitializeComponent();
            spf = SPF;
		}

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Name.Content = spf.GetName();
            renamebox.Text = spf.GetName();
            Id.Content = spf.GetID();
            icon.Source = spf.GetIcon().ToImageSource();
        }

        private void renamebox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(renamebox.Text) || renamebox.Text == (string)Name.Content)
            {
                renamebox.Text = spf.GetName();
                renamebox.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                Name.Content = renamebox.Text;
                spf.Rename(renamebox.Text);
                renamebox.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void renamebox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrWhiteSpace(renamebox.Text)||renamebox.Text == Name.Content)
                {
                    renamebox.Text = spf.GetName();
                    renamebox.Visibility = System.Windows.Visibility.Hidden;
                }
                else 
                {
                    Name.Content = renamebox.Text;
                    spf.Rename(renamebox.Text);
                    renamebox.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }

        private void renamebox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (renamebox.Text.StartsWith("@"))
            {
                renamebox.Text = renamebox.Text.Remove(0, 1);
                System.Media.SystemSounds.Asterisk.Play();
            }
        }

        private void rename_MouseUp(object sender, MouseButtonEventArgs e)
        {
            renamebox.Visibility = System.Windows.Visibility.Visible;
        }

        private void remove_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ConfirmDelete cd = new ConfirmDelete(spf);
            cd.ShowDialog();
            Manage.MainInstance.cleardir();
            Manage.MainInstance.scandir();
        }
	}
    internal static class IconUtilities
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        public static ImageSource ToImageSource(this Icon icon)
        {
            Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new Win32Exception();
            }

            return wpfBitmap;
        }
    }
}