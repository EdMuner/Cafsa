using Cafsa.Common.Models;
using Cafsa.Common.Services;
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
        public readonly IApiService _apiService;
        private string _password;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _loginCommand;


        public LoginPageViewModel(
            INavigationService navigationservice,
            IApiService apiService) : base(navigationservice)
        {
            //titulo de la pantalla inicial de la app
            _apiService = apiService;
            Title = "Login";
            IsEnabled = true;

            //TODO: delete this lines
            Email = "edisonmunera72@gmail.com";
            Password = "123456";

        }

        //cuando le den tab en el command el ejecuta el metodo login
        //  operador -----       aqui pregunta si es null ?? 
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

            IsRunning = true;
            IsEnabled = false;

            var request = new TokenRequest
            {
                Password = Password,
                Username = Email

            };

            var url = App.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.GetTokenAsync(url, "Account", "/CreateToken", request);



            IsRunning = false;
            IsEnabled = true;


            if (!response.IsSuccess)
	        {
    	        IsEnabled = true;
    	        IsRunning = false;
    	        await App.Current.MainPage.DisplayAlert("Error", "User or password incorrect.", "Accept");
    	        Password = string.Empty;
    	        return;
	        }

            var token = response.Result;
            await App.Current.MainPage.DisplayAlert("ok", "una chimba!!!", "Accept");
        }
    }
}
