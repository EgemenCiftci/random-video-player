using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace RandomPlayer
{
    public partial class PlayerPage : Page
    {
        private bool _offsetMoveMode;
        private DispatcherTimer timer;
        private ObservableCollection<FileInfo> history;

        public PlayerPage()
        {
            InitializeComponent();
        }

        public void Play()
        {
            me.Play();
        }

        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            Random();
        }

        public void Random()
        {
            if (Properties.Settings.Default.Folder.Trim() == "")
            {
                _ = NavigationService.Navigate(new SettingsPage());
                return;
            }

            DirectoryInfo di = new DirectoryInfo(Properties.Settings.Default.Folder);
            FileInfo[] fi = new string[] { "*.avi", "*.wmv", "*.mp4", "*.mov", "*.mpeg", "*.mpg", "*.asf" }.SelectMany(f => di.GetFiles(f, SearchOption.AllDirectories)).Where(f => history.Count(g => g.FullName == f.FullName) == 0).ToArray();

            Random r = new Random();
            int ran = r.Next(0, fi.Count());
            me.Source = new Uri(fi[ran].FullName);
            sld.Value = 0;
            Window.GetWindow(this).Title = "Destursoft Random Player - " + fi[ran].FullName;
            history.Add(fi[ran]);
            MenuItem mi = new MenuItem
            {
                Header = fi[ran].Name,
                Tag = fi[ran]
            };
            mi.Click += new RoutedEventHandler(mi_Click);
            _ = miHistory.Items.Add(mi);

            Play();
        }

        private void mi_Click(object sender, RoutedEventArgs e)
        {
            FileInfo fi = (FileInfo)((MenuItem)sender).Tag;
            me.Source = new Uri(fi.FullName);
            sld.Value = 0;
            Window.GetWindow(this).Title = "Destursoft Random Player - " + fi.FullName;
            Play();
        }

        private void sld_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _offsetMoveMode = true;
        }

        private void sld_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_offsetMoveMode)
            {
                Point pts = e.GetPosition(sld);
                double total = sld.Maximum;
                double res = pts.X * 100 / ((double)sld.ActualWidth) / 100;
                me.Position = TimeSpan.FromMilliseconds(total * res);
            }
            else
            {
                me.Position = TimeSpan.FromMilliseconds(sld.Value);
            }

            _offsetMoveMode = false;
        }

        private void me_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (me.NaturalDuration.HasTimeSpan)
            {
                sld.Maximum = me.NaturalDuration.TimeSpan.TotalMilliseconds;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            history = new ObservableCollection<FileInfo>();

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            sld.Value = me.Position.TotalMilliseconds;
            if (me.NaturalDuration != null && me.NaturalDuration.HasTimeSpan)
            {
                tbkInfo.Text = me.Position.ToString() + " / " + me.NaturalDuration.TimeSpan.ToString();
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void miRandom_Click(object sender, RoutedEventArgs e)
        {
            Random();
        }

        private void miSettings_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService.Navigate(new SettingsPage());
        }

        private void miLock_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService.Navigate(new LoginPage());
        }

    }
}
