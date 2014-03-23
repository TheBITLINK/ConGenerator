using CGApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Xml.XPath;

namespace ConGenerator
{
    /// <summary>
    /// Lógica de interacción para Manage.xaml
    /// </summary>
    public partial class Manage : Window
    {
        private System.Windows.Forms.FolderBrowserDialog fb = new System.Windows.Forms.FolderBrowserDialog();

        public static Manage MainInstance;
        public Manage()
        {
            if (System.Globalization.CultureInfo.CurrentCulture.EnglishName.ToUpperInvariant() == "Spanish".ToUpperInvariant() && Properties.Settings.Default.firstrun)
            {
                Properties.Settings.Default.lang = "es_ES.tbl";
            }
            Properties.Settings.Default.firstrun = false;
            Properties.Settings.Default.Save();
            CGApi.Localization.LangFile = @".\Lang\" + Properties.Settings.Default.lang;
            XmlDataProvider asd = (XmlDataProvider)App.Current.Resources["thislang"];
            asd.Source = new Uri("pack://siteoforigin:,,,/Lang/" + Properties.Settings.Default.lang);
            InitializeComponent();
            if (!IsWinVistaOrHigher())
            {
                MessageBox.Show("Not compatible with XP", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            fb.SelectedPath = Environment.GetEnvironmentVariable("HOMEPATH") + @"\Desktop";
            MainInstance = this;
        }

        void updatefind_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            WebRequest request = WebRequest.Create("http://www.thebitlink.com/updates/cg/2.0/data.txt");
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string updatechannel = reader.ReadToEnd();
            if (updatechannel != "updated")
            {
                Common.RunOnUIDespatcher((Action)(() =>
                {
                    updateinfo.Content = updatechannel;
                    updatebtn.Visibility = System.Windows.Visibility.Visible;
                }), this);
            }
        }

        static bool IsWinVistaOrHigher()
        {
            OperatingSystem OS = Environment.OSVersion;
            return (OS.Platform == PlatformID.Win32NT) && (OS.Version.Major >= 6);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            cleardir();
            scandir();
            detectlang();
            Style s = new Style();
            s.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            tc.ItemContainerStyle = s;
            tc.SelectedIndex = 0;
            actualfolder.Content = CGApi.Localization.getlocalizedstring("/tbl/data/mesages/actualfolder") + SpecialFolders.GetName(fb.SelectedPath);
            screenname.Content = CGApi.Localization.getlocalizedstring("/tbl/data/mesages/manage");
            System.ComponentModel.BackgroundWorker updatefind = new System.ComponentModel.BackgroundWorker();
            updatefind.DoWork += updatefind_DoWork;
            updatefind.RunWorkerAsync();
        }

        public void cleardir() 
        {
            spflist.Items.Clear();
            spflist.Items.Add(new SPFHeader());
        }

        public void scandir()
        {
            SpecialFolder[] SPFA = SpecialFolders.GetSpecialFolderByDirectory(fb.SelectedPath);
                foreach (SpecialFolder SPF in SPFA)
                {
                    spflist.Items.Add(new SpecialFolderitem(SPF));
                }
        }

        private void Name_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Name.Text == CGApi.Localization.getlocalizedstring("/tbl/data/mesages/name"))
            {
                Name.Text = "";
                Name.Foreground = Brushes.Black;
            }
        }

        private void Name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text))
            {
                Name.Text = CGApi.Localization.getlocalizedstring("/tbl/data/mesages/name");
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

        private void cd_Click(object sender, RoutedEventArgs e)
        {
            fb.ShowDialog();
            cleardir();
            scandir();
            actualfolder.Content = CGApi.Localization.getlocalizedstring("/tbl/data/mesages/actualfolder") + new DirectoryInfo(fb.SelectedPath).Name;
        }

        private void newf_Click(object sender, RoutedEventArgs e)
        {
            tc.SelectedIndex = 1;
            backmenu.Visibility = System.Windows.Visibility.Visible;
            mainmenu.Visibility = System.Windows.Visibility.Hidden;
            screenname.Content = CGApi.Localization.getlocalizedstring("/tbl/data/mesages/newtitle");
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            tc.SelectedIndex = 0;
            backmenu.Visibility = System.Windows.Visibility.Hidden;
            mainmenu.Visibility = System.Windows.Visibility.Visible;
            screenname.Content = CGApi.Localization.getlocalizedstring("/tbl/data/mesages/manage");
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            tc.SelectedIndex = 2;
            backmenu.Visibility = System.Windows.Visibility.Visible;
            mainmenu.Visibility = System.Windows.Visibility.Hidden;
            screenname.Content = CGApi.Localization.getlocalizedstring("/tbl/data/mesages/settings");
        }

        private void hhelp_Click(object sender, RoutedEventArgs e)
        {
            tc.SelectedIndex = 3;
            backmenu.Visibility = System.Windows.Visibility.Visible;
            mainmenu.Visibility = System.Windows.Visibility.Hidden;
            screenname.Content = CGApi.Localization.getlocalizedstring("/tbl/data/mesages/help");
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            tc.SelectedIndex = 4;
            backmenu.Visibility = System.Windows.Visibility.Visible;
            mainmenu.Visibility = System.Windows.Visibility.Hidden;
            screenname.Content = CGApi.Localization.getlocalizedstring("/tbl/data/mesages/about");
        }

        private void detectlang()
        {
            langcombo.Items.Clear();
            DirectoryInfo langdir = new DirectoryInfo(".\\Lang");
            FileInfo[] langfiles = langdir.GetFiles("*.tbl");
            foreach (FileInfo langfile in langfiles)
            {
                    ComboBoxItem cbi = new ComboBoxItem();
                    cbi.Name = langfile.Name.Remove(langfile.Name.Length - 4) + "_" + langfile.Extension.Remove(0,1);
                    cbi.Content = CGApi.Localization.getlocalizedstringfromlangfile(langfile.FullName, "/tbl/data/langname") + " (" + langfile.Name + ")";
                    langcombo.Items.Add(cbi);
            }
            FileInfo[] xmllangfiles = langdir.GetFiles("*.xml");
            foreach (FileInfo langfile in xmllangfiles)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Name = langfile.Name.Remove(langfile.Name.Length - 4) + "_" + langfile.Extension.Remove(0, 1);
                cbi.Content = CGApi.Localization.getlocalizedstringfromlangfile(langfile.FullName, "/tbl/data/langname") + " (" + langfile.Name + ")";
                langcombo.Items.Add(cbi);
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text) || Name.Text == CGApi.Localization.getlocalizedstring("/tbl/data/mesages/name"))
            {
                System.Media.SystemSounds.Asterisk.Play();
            }
            else
            {
                SpecialFolders.Create(fb.SelectedPath, Name.Text);
                System.Threading.Thread.Sleep(500);
                cleardir();
                scandir();
                tc.SelectedIndex = 0;
                backmenu.Visibility = System.Windows.Visibility.Hidden;
                mainmenu.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void langcombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cbi = ((ComboBoxItem)langcombo.SelectedValue);
            char[] gb = {Convert.ToChar("_")};
            string[] langdata = cbi.Name.Split(gb[0]);
            string selectedlang = langdata[0] + "_" + langdata[1] + "." + langdata[2];
            Properties.Settings.Default.lang = selectedlang;
            Properties.Settings.Default.Save();
            CGApi.Localization.LangFile = @".\Lang\" + Properties.Settings.Default.lang;
            XmlDataProvider asd = (XmlDataProvider)App.Current.Resources["thislang"];
            asd.Source = new Uri("pack://siteoforigin:,,,/Lang/" + Properties.Settings.Default.lang);
            actualfolder.Content = CGApi.Localization.getlocalizedstring("/tbl/data/mesages/actualfolder") + SpecialFolders.GetName(fb.SelectedPath);
            screenname.Content = CGApi.Localization.getlocalizedstring("/tbl/data/mesages/settings");
        }

        private void gotoweb(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://cg.thebitlink.com/");
        }

        private void downloadlangs(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://cg.thebitlink.com/langs/");
        }

        private void translate(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://cg.thebitlink.com/langs/submit");
        }
    }
}
