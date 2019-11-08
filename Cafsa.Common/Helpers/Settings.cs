using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Cafsa.Common.Helpers
{
    public static class Settings
    {
        private const string _activityImages = "ActivityImages";
        private const string _activity = "Activity";
        private const string _token = "token";
        private const string _referee = "referee";
        private const string _isRemembered = "IsRemembered";
        private static readonly string _stringDefault = string.Empty;
        private static readonly bool _boolDefault = false;




        private static ISettings AppSettings => CrossSettings.Current;

        public static string ActivityImages
        {
            get => AppSettings.GetValueOrDefault(_activityImages, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_activityImages, value);
        }
        public static string Activity
        {
            get => AppSettings.GetValueOrDefault(_activity, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_activity, value);
        }
        public static string Token
        {
            get => AppSettings.GetValueOrDefault(_token, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_token, value);
        }

        public static string Referee
        {
            get => AppSettings.GetValueOrDefault(_referee, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_referee, value);
        }

        public static bool IsRemembered
        {
            get => AppSettings.GetValueOrDefault(_isRemembered, _boolDefault);
            set => AppSettings.AddOrUpdateValue(_isRemembered, value);
        }

    }
}
