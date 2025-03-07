using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
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
        }

        private void OnLoginBtnClick(object sender, RoutedEventArgs e)
        {
            this.MainFrame.NavigationService.Navigate(new Uri("Views/Login.xaml", UriKind.Relative));
        }

        private void OnRegisterBtnClick(object sender, RoutedEventArgs e)
        {
            this.MainFrame.NavigationService.Navigate(new Uri("Views/Register.xaml", UriKind.Relative));
        }

        private void OnUserBtnClick(object sender, RoutedEventArgs e)
        {
            this.MainFrame.NavigationService.Navigate(new Uri("Views/User.xaml", UriKind.Relative));
        }
    }
}