using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Cafsa.Common.Helpers
{
    public static class Settings
    {
        private const string _activityImages = "ActivityImages";
        private const string _activity = "Activity";
        private static readonly string _settingsDefault = string.Empty;

        private static ISettings AppSettings => CrossSettings.Current;

        public static string ActivityImages
        {
            get => AppSettings.GetValueOrDefault(_activityImages, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_activityImages, value);
        }
        public static string Activity
        {
            get => AppSettings.GetValueOrDefault(_activity, _settingsDefault);
            set => AppSettings.AddOrUpdateValue(_activity, value);
        }
    }
}
