using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using ristorante_frontend.Services;
using ristorante_frontend.Models;

namespace ristorante_frontend.Views
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        // Metodo per il click del pulsante di registrazione
        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Verifica che tutti i campi siano compilati
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password))
            {
                MessageBox.Show("Per favore, compila tutti i campi!", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Verifica che l'email sia valida
            if (!IsValidEmail(EmailTextBox.Text))
            {
                MessageBox.Show("L'email non è valida!", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Verifica che la password e la conferma della password siano uguali
            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Le password non corrispondono!", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            ApiService.Email = this.EmailTextBox.Text;
            ApiService.Password = this.PasswordBox.Password;
            
            var registerApiResult = await ApiService.Register();
            if (registerApiResult.Data == false)
            {
                MessageBox.Show($"Errore registrazione! {registerApiResult.ErrorMessage}");
                return;
            }
            

            MessageBox.Show("Registrazione completata con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);

            // Dopo la registrazione, naviga alla pagina di login
            this.NavigationService.Navigate(new Uri("Views/Login.xaml", UriKind.Relative));
        }

        
        private bool IsValidEmail(string email)
        {
            try
            {
                var emailAddress = new System.Net.Mail.MailAddress(email);
                return emailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }


        private void EmailTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
                EmailPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void EmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
                EmailPlaceholder.Visibility = Visibility.Visible;
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                PasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                PasswordPlaceholder.Visibility = Visibility.Visible;
        }

        private void ConfirmPasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password))
                ConfirmPasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void ConfirmPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password))
                ConfirmPasswordPlaceholder.Visibility = Visibility.Visible;
        }

        
        private void NavigateToLoginPage(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Views/Login.xaml", UriKind.Relative));
        }

        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
