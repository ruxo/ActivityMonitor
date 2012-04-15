
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
        private static bool _autoExportEnabled;
        private static int _autoExportInterval;
        private static string _autoExportPath;


        public static bool AutoExportEnabled
        {
            get
            {
                return _settingsProvider == null ? _autoExportEnabled : _settingsProvider.AutoExportEnabled;
            }
            set
            {
                if (_settingsProvider == null)
                {
                    _autoExportEnabled = value;

                }
                else
                {
                    _settingsProvider.AutoExportEnabled = value;
                }
            }
        }
        public static int AutoExportInterval
        {
            get
            {
                return _settingsProvider == null ? _autoExportInterval : _settingsProvider.AutoExportInterval;
            }
            set
            {
                if (_settingsProvider == null)
                {
                    _autoExportInterval = value;

                }
                else
                {
                    _settingsProvider.AutoExportInterval= value;
                }
            }
        }
        public static string AutoExportPath
        {
            get
            {
                return _settingsProvider == null ? _autoExportPath : _settingsProvider.AutoExportPath;
            }
            set
            {
                if (_settingsProvider == null)
                {
                    _autoExportPath = value;

                }
                else
                {
                    _settingsProvider.AutoExportPath = value;
                }
            }
        }

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
        bool AutoExportEnabled { get; set; }
        int AutoExportInterval { get; set; }
        string AutoExportPath { get; set; }
    }
}
