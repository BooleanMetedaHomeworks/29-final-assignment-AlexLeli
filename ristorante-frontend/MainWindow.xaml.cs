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
            if (MainWindowViewModel.Instance.Jwt == null)
            {
                ResetAuthenticationData();
                MessageBox.Show("Effettua il login prima di procedere con l'applicazione!", "Fallimento", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else
            {
                if (MainWindowViewModel.Instance.Jwt.ExpirationUtc < DateTime.UtcNow)
                {
                    ResetAuthenticationData();
                    MessageBox.Show("Il tuo token è scaduto! Procedi con il login per poter continuare.", "Token Expired", MessageBoxButton.OK, MessageBoxImage.Warning);
                    MessageBoxResult msg = MessageBox.Show("Vuoi andare alla pagina di login?", "Login request", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (msg == MessageBoxResult.Yes)
                    {
                        Console.WriteLine("Tentativo di navigazione a Login.xaml...");
                        if (this.MainFrame?.NavigationService != null)
                        {
                            Console.WriteLine("NavigationService disponibile.");
                            this.MainFrame.NavigationService.Navigate(new Uri("Views/Login.xaml", UriKind.Relative));
                        }
                        else
                        {
                            Console.WriteLine("NavigationService non disponibile.");
                        }
                        //this.MainFrame.NavigationService.Navigate(new Uri("Views/Login.xaml", UriKind.Relative));
                    } else
                    {
                        return;
                    }
                }
                this.MainFrame.NavigationService.Navigate(new Uri("Views/User.xaml", UriKind.Relative));
            }
            
        }

        private void OnLogoutBtnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msg = MessageBox.Show("Desideri effettuare il logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msg == MessageBoxResult.Yes)
            {
                ResetAuthenticationData();
                MessageBox.Show("Logout effettuato con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
            } else
            {
                return;
            }
            
        }

        private void ResetAuthenticationData()
        {
            ApiService.Email = null;
            ApiService.Password = null;
            MainWindowViewModel.Instance.Jwt = null;
            MainWindowViewModel.Instance.IsNotLogged = true;
            MainWindowViewModel.Instance.IsLogged = false;
        }
    }
}