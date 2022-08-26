using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace RandomPlayer
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (pbxPassword.Password == Properties.Settings.Default.Password)
            {
                _ = NavigationService.Navigate(new PlayerPage());
            }
        }
    }
}
