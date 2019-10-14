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

        private string _password;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _loginCommand;


        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
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

            await App.Current.MainPage.DisplayAlert("ok", "una chimba!!!", "Accept");
        }
    }
}
