using System;
using Cafsa.Common.Helpers;
using Cafsa.Common.Models;
using Cafsa.Common.Services;
using Cafsa.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

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
        private readonly INavigationService _navigationService;
        public readonly IApiService _apiService;
        private string _password;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _loginCommand;
        private DelegateCommand _registerCommand;
        private DelegateCommand _forgotPasswordCommand;

        public LoginPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            //titulo de la pantalla inicial de la app
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Login;
            IsEnabled = true;
            IsRemember = true;
        }

        //cuando le den tab en el command el ejecuta el metodo login
        //  operador -----       aqui pregunta si es null ?? 
        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(LoginAsync));
        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(Register));

        public DelegateCommand ForgotPasswordCommand => _forgotPasswordCommand ?? (_forgotPasswordCommand = new DelegateCommand(ForgotPassword));

      

        public string Email { get; set; }

        public bool IsRemember { get; set; }

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

        private async void LoginAsync()
        {
            //si no digitaron nada en el campo email
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailError,
                    Languages.Accept);
                return;
            }
            //si no digitaron password
            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordPlaceHolder,
                    Languages.Accept);
                return;
            }

            IsRunning = true;
            IsEnabled = false;

           //Se carga la url del diccionario de recursos
            var url = App.Current.Resources["UrlAPI"].ToString();
            //Se valida si hay coneccion a internet
            var connection = await _apiService.CheckConnectionAsync(url);

            if (!connection)
            {
                IsEnabled = true;
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.CheckConnection,
                    Languages.Accept);
                return;
            }

            var request = new TokenRequest
            {
                Password = Password,
                Username = Email

            };
         
            var response = await _apiService.GetTokenAsync(url, "Account", "/CreateToken", request);

            if (!response.IsSuccess)
	        {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(
                    Languages.Error, 
                    Languages.UserAndPasswordIncorrect, 
                    Languages.Accept);
    	        Password = string.Empty;
    	        return;
	        }

            var token = response.Result;
            //se consulta las activities del referee
            var response2 = await _apiService.GetRefereeByEmailAsync(
                url,
                "api",
                "/Referees/GetRefereeByEmail",
                "bearer",
                token.Token,
                Email);

            if (!response2.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(
                    Languages.Error, 
                    Languages.DataProblem,
                    Languages.Accept);           
                return;
            }

            var referee = response2.Result;

            Settings.Referee = JsonConvert.SerializeObject(referee);
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.IsRemembered = IsRemember;


            //Se navega a la pagina activities
            await _navigationService.NavigateAsync("/CafsaMasterDetailPage/NavigationPage/ActivitiesPage");

            IsRunning = false;
            IsEnabled = true;
        }

        private async void Register()
        {
            await _navigationService.NavigateAsync("RegisterPage");
        }

        private async void ForgotPassword()
        {
            await _navigationService.NavigateAsync("RememberPasswordPage");
        }
    }
}
