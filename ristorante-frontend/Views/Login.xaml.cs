﻿using System;
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

            // Gestione del click sul bottone Login
            private void LoginButton_Click(object sender, RoutedEventArgs e)
            {
                // Aggiungi qui la logica per il login
            }

            // Gestione del click sul link per andare alla pagina di registrazione
            private void NavigateToRegisterPage(object sender, RoutedEventArgs e)
            {
                this.NavigationService.Navigate(new Uri("Views/Register.xaml", UriKind.Relative));
            }
        }
    }

