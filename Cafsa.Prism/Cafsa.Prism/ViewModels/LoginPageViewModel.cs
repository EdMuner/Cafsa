using Cafsa.Common.Models;
using Cafsa.Common.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafsa.Prism.ViewModels
{
    //Aqui es el codigo del front de LoginPage, estan ligadas.
    public class LoginPageViewModel : ViewModelBase
    {


        //Orden Atributos Privados
        //Contructor
        //Propiedades
        //Metodos publicos
        //Metodos Privados
        private readonly IApiService _apiService;
        private string _password;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _loginCommand;


        public LoginPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _apiService = apiService;
            Title = "Login";
            IsEnabled = true;
          
        }

        //cuando le den tab en el command el ejecuta el metodo login
        //  operador ternario-----       aqui pregunta si es null ?? 
        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Login));
 
        public string Email { get; set; }

        //Se implementa para que cuando se cambie la propiedad password desde la viewmodel, nos va a reflejar la view
        public string Password
        {
            get => _password;
            //este setProperty refresca la interfaz de usuario.
            set => SetProperty(ref _password, value);
        }
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        public IApiService ApiService { get; }

        private async void Login()
        {
            //si no digitaron nada en el campo email
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must an email.", "Accept");
                return;
            }
            //si no digitaron password
            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter an password.", "Accept");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

          

            var request = new TokenRequest
            {
                Password = Password,
                Username = Email

            };

            //Consumo del diccionario de recursos de App.xaml la direccion de cafsaAzure
            var url = App.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.GetTokenAsync(url, "Account", "/CreateToken", request);

            IsRunning = false;
            IsEnabled = true;

           

            // se valida si se logueo
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", "User or password incorrect.", "Accept");
                Password = string.Empty;
                return;
            }

            await App.Current.MainPage.DisplayAlert("ok", "una cuca!!!", "Accept");
        }
    }
}
