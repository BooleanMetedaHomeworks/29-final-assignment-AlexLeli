using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ristorante_frontend.Models;
using ristorante_frontend.Services;

namespace ristorante_frontend.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private static MainWindowViewModel _instance = new MainWindowViewModel();
        public static MainWindowViewModel Instance { get { return _instance; } }

        private Jwt _jwt;
        public Jwt Jwt
        {
            get { return _jwt; }
            set
            {
                if (value == _jwt)
                {
                    return;
                }
                _jwt = value;
                OnPropertyChanged(nameof(Jwt));
            }
        }

        private bool _isNotLogged = true;
        public bool IsNotLogged
        {
            get { return _isNotLogged; }
            set
            {
                if (value == _isNotLogged)
                {
                    return;
                }
                _isNotLogged = value;
                OnPropertyChanged(nameof(IsNotLogged));
            }
        }


        private bool _isLogged;
        public bool IsLogged
        {
            get { return _isLogged; }
            set
            {
                if (value == _isLogged)
                {
                    return;
                }
                _isLogged = value;
                OnPropertyChanged(nameof(IsLogged));
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            // Qui richiamiamo i callback delle funzioni che si sono iscritte all'evento PropertyChanged
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel()
        {

            _ = Initialize();
        }
        private async Task Initialize()
        {
            

            
        }
        


    }
}
