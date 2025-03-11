using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ristorante_frontend.Services;
using ristorante_frontend.ViewModel;

namespace ristorante_frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MainWindowViewModel.Instance;
        }

        private void OnLoginBtnClick(object sender, RoutedEventArgs e)
        {
            this.MainFrame.NavigationService.Navigate(new Uri("Views/Login.xaml", UriKind.Relative));
        }

        private void OnRegisterBtnClick(object sender, RoutedEventArgs e)
        {
            this.MainFrame.NavigationService.Navigate(new Uri("Views/Register.xaml", UriKind.Relative));
        }

        private void OnStartBtnClick(object sender, RoutedEventArgs e)
        {
            if (MainWindowViewModel.Instance.Jwt == null || MainWindowViewModel.Instance.Jwt.ExpirationUtc > DateTime.Now)
            {
                ApiService.Email = null;
                ApiService.Password = null;
                MainWindowViewModel.Instance.Jwt = null;
                MainWindowViewModel.Instance.IsNotLogged = true;
                MainWindowViewModel.Instance.IsLogged = false;
                MessageBox.Show("Effettua il login prima di procedere con l'applicazione!", "Fallimento", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else
            {
                this.MainFrame.NavigationService.Navigate(new Uri("Views/User.xaml", UriKind.Relative));
            }
            
        }

        private void OnLogoutBtnClick(object sender, RoutedEventArgs e)
        {
            var msg = MessageBox.Show("Desideri effettuare il logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msg == MessageBoxResult.Yes)
            {
                ApiService.Email = null;
                ApiService.Password = null;
                MainWindowViewModel.Instance.Jwt = null;
                MainWindowViewModel.Instance.IsNotLogged = true;
                MainWindowViewModel.Instance.IsLogged = false;
                MessageBox.Show("Logout effettuato con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
            } else
            {
                return;
            }
            
        }
    }
}