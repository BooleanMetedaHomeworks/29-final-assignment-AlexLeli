using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;

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
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
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

            // Logica di registrazione (salvataggio nel database o altro)
            // Qui aggiungeresti il codice per registrare l'utente (ad esempio, nel database)

            // Simuliamo una registrazione riuscita
            MessageBox.Show("Registrazione completata con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);

            // Dopo la registrazione, naviga alla pagina di login
            this.NavigationService.Navigate(new Uri("Views/Login.xaml", UriKind.Relative));
        }

        // Metodo per validare l'email (utilizza una regex)
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

        // Eventi di gestione focus per il controllo TextBox (per placeholder)
        private void EmailTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (EmailTextBox.Text == "Email")
                EmailTextBox.Text = "";
        }

        private void EmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
                EmailTextBox.Text = "Email";
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password == "Password")
                PasswordBox.Password = "";
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                PasswordBox.Password = "Password";
        }

        private void ConfirmPasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ConfirmPasswordBox.Password == "Confirm Password")
                ConfirmPasswordBox.Password = "";
        }

        private void ConfirmPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password))
                ConfirmPasswordBox.Password = "Confirm Password";
        }

        // Navigazione alla pagina di login
        private void NavigateToLoginPage(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Views/Login.xaml", UriKind.Relative));
        }
    }
}
