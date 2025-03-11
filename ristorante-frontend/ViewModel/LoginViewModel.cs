using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ristorante_frontend.ViewModel
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {

            _ = Initialize();
        }
        private async Task Initialize()
        {
            
            MainWindowViewModel.Instance.IsLogged = false;
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
