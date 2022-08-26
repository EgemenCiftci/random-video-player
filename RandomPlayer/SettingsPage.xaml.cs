using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace RandomPlayer
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void btnBrowseFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Select Folder"
            };
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.Folder = fbd.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            _ = NavigationService.Navigate(new PlayerPage());
        }
    }
}
