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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.Logging;
using ristorante_frontend.Services;
using ristorante_frontend.ViewModel;

namespace ristorante_frontend.Views
{
    /// <summary>
    /// Logica di interazione per Login.xaml
    /// </summary>
    public partial class Login : Page
    {


        public Login()
        {
            InitializeComponent();
            //MainWindowViewModel viewModel = new MainWindowViewModel();
        }



        // Gestione dell'evento GotFocus per il campo Username
        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Nascondi il placeholder quando l'utente clicca nel TextBox
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
                UsernamePlaceholder.Visibility = Visibility.Collapsed;
        }

        // Gestione dell'evento LostFocus per il campo Username
        private void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Mostra il placeholder se il TextBox è vuoto
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
                UsernamePlaceholder.Visibility = Visibility.Visible;
        }

        // Gestione dell'evento GotFocus per il campo Password
        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Nascondi il placeholder quando l'utente clicca nel PasswordBox
            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                PasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        // Gestione dell'evento LostFocus per il campo Password
        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Mostra il placeholder se il PasswordBox è vuoto
            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                PasswordPlaceholder.Visibility = Visibility.Visible;
        }


            private async void LoginButton_Click(object sender, RoutedEventArgs e)
            {
            ApiService.Email = this.UsernameTextBox.Text;
            ApiService.Password = this.PasswordBox.Password;
            var tokenApiResult = await ApiService.GetJwtToken();
            if (tokenApiResult.Data == null)
            {
                MessageBox.Show($"Errore login! {tokenApiResult.ErrorMessage}");
                return;
            }
            MainWindowViewModel.Instance.Jwt = tokenApiResult.Data;
            MainWindowViewModel.Instance.IsLogged = true;
            MainWindowViewModel.Instance.IsNotLogged = false;
            MessageBox.Show("Login effettuato con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
            
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            Window currentWindow = Window.GetWindow(this);
            if (currentWindow != null)
            {
                currentWindow.Close();
            }

            Console.WriteLine($"IsLogged: {MainWindowViewModel.Instance.IsLogged}");

        }

            // Gestione del click sul link per andare alla pagina di registrazione
            private void NavigateToRegisterPage(object sender, RoutedEventArgs e)
            {
                this.NavigationService.Navigate(new Uri("Views/Register.xaml", UriKind.Relative));
            }


        }
    }

