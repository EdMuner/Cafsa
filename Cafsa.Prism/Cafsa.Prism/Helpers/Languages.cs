using Cafsa.Prism.Interfaces;
using Cafsa.Prism.Resources;
using Xamarin.Forms;

namespace Cafsa.Prism.Helpers
{
    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept => Resource.Accept;

        public static string EmailError => Resource.EmailError;

        public static string Error => Resource.Error;

        public static string Email => Resource.Email;

        public static string EmailPlaceHolder => Resource.EmailPlaceHolder;

        public static string Password => Resource.Password;

        public static string PasswordPlaceHolder => Resource.PasswordPlaceHolder;

        public static string Rememberme => Resource.Rememberme;

        public static string ForgotPassword => Resource.ForgotPassword;

        public static string Login => Resource.Login;

        public static string Register => Resource.Register;

        public static string Loading => Resource.LoadingPlaceHolder;

        public static string ImageError => Resource.ImageError;

        public static string Neighborhood => Resource.Neighborhood;

        public static string Address => Resource.Address;

        public static string Price => Resource.Price;

        public static string ActivityType => Resource.ActivityType;

        public static string IsAvailable => Resource.IsAvailable;

        public static string Activities => Resource.Activities;

        public static string Details => Resource.Details;

        public static string Services => Resource.Services;

        public static string ModifyUser => Resource.ModifyUser;

        public static string Map => Resource.Map;

        public static string Logout => Resource.Logout;

        public static string ChangePassword => Resource.ChangePassword;

        public static string Ok => Resource.Ok;

        public static string CurrentPassword => Resource.CurrentPassword;

        public static string LengthPassword => Resource.LengthPassword;

        public static string ConfirmPassword => Resource.ConfirmPassword;

        public static string ErrorConfirmPassword => Resource.ErrorConfirmPassword;

        public static string CheckConnection => Resource.CheckConnection;

        public static string UserAndPasswordIncorrect => Resource.UserAndPasswordIncorrect;

        public static string DataProblem => Resource.DataProblem;

        public static string UserUpdateSuccessfull => Resource.UserUpdateSuccessfull;

        public static string EnterDocument => Resource.EnterDocument;

        public static string EnterFirstName => Resource.EnterFirstName;

        public static string EnterLastName => Resource.EnterLastName;

        public static string EnterAddres => Resource.EnterAddres;

        public static string EnterPhone => Resource.EnterPhone;

        public static string RegisterNewUser => Resource.RegisterNewUser;

        public static string EnterEmail => Resource.EnterEmail;

        public static string SelectRole => Resource.SelectRole;

        public static string EnterPassword => Resource.EnterPassword;

        public static string RecoverPassword => Resource.RecoverPassword;

        public static string EmailValid => Resource.EmailValid;

        public static string Service => Resource.Service;

        public static string Remarks => Resource.Remarks;

        public static string CurrentPasswordPlaceHolder => Resource.CurrentPasswordPlaceHolder;

        public static string NewPassword => Resource.NewPassword;

        public static string NewPasswordPlaceHolder => Resource.NewPasswordPlaceHolder;

        public static string CurrentPasswordNew => Resource.CurrentPasswordNew;

        public static string ConfirmNewPasswordPlaceHolder => Resource.ConfirmNewPasswordPlaceHolder;

        public static string Procesing => Resource.Procesing;

        public static string Document => Resource.Document;

        public static string DocumentPlaceHolder => Resource.DocumentPlaceHolder;

        public static string FirstName => Resource.FirstName;

        public static string FirstNamePlaceHolder => Resource.FirstNamePlaceHolder;

        public static string LastName => Resource.LastName;

        public static string LastNamePlaceHolder => Resource.LastNamePlaceHolder;

        public static string Addres => Resource.Addres;

        public static string AddresPlaceHolder => Resource.AddresPlaceHolder;

        public static string PhoneNumber => Resource.PhoneNumber;

        public static string PhonePlaceHolder => Resource.PhonePlaceHolder;

        public static string Save => Resource.Save;

        public static string EmailNew => Resource.EmailNew;

        public static string RegisterAs => Resource.RegisterAs;

        public static string SelectAnRole => Resource.SelectAnRole;

        public static string EnterAnPassword => Resource.EnterAnPassword;

        public static string ConfirmAPassword => Resource.ConfirmAPassword;

        public static string ConfirmAPasswordPlaceHolder => Resource.ConfirmAPasswordPlaceHolder;

        public static string Registering => Resource.Registering;

        public static string Recovering => Resource.Recovering;

        public static string Customer => Resource.Customer;

        public static string StartDate => Resource.StartDate;



    }
}