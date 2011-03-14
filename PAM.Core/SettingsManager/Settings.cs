
using System.Reflection;
using Microsoft.Win32;

namespace PAM.Core.SettingsManager
{
    public class Settings
    {

        private const string RunRegistryKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string ValueName = "PAM - Personal Activity Monitor";
        private static ISettingsProvider _settingsProvider;
        private static int _idleTime;

        static Settings()
        {
            IdleTime = 30;
        }

        public static int IdleTime
        {
            get
            {
                return _settingsProvider == null ? _idleTime : _settingsProvider.IdleTime;
            }
            set
            {
                if (_settingsProvider == null)
                {
                    _idleTime = value;

                }
                else
                {
                    _settingsProvider.IdleTime = value;
                }
            }
        }

        public static bool Autostart
        {
            get
            {
                var key = Registry.CurrentUser.OpenSubKey(RunRegistryKey);
                if (key == null)
                    return false;

                var value = (string)key.GetValue(ValueName);
                if (value == null)
                    return false;
                return (value == Assembly.GetExecutingAssembly().Location);
            }
            set
            {
                var key = Registry.CurrentUser.CreateSubKey(RunRegistryKey);

                if (key == null) return;

                if (value != true)
                {
                    if (key.GetValue(ValueName) != null)
                        key.DeleteValue(ValueName);
                }
                else
                {
                    key.SetValue(ValueName, Assembly.GetExecutingAssembly().Location);
                }
                key.Close();
            }
        }

        public static ISettingsProvider SettingsProvider
        {
            set { _settingsProvider = value; }
        }

    }

    public interface ISettingsProvider
    {
        int IdleTime { get; set; }
    }
}
