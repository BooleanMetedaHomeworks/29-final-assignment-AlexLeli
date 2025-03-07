using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ristorante_frontend.Services;

namespace ristorante_frontend.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
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
            //var tokenApiResult = await ApiService.GetJwtToken();
            //if (tokenApiResult != null && tokenApiResult.IsSuccess)
            //{
            //    IsLogged = true;
            //}
            //else
            //{
            //    IsLogged = false;
            //}
            //OnPropertyChanged(nameof(IsLogged));

            
        }
        


    }
}
