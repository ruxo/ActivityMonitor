
using System.Reflection;
using Microsoft.Win32;

namespace PAM.Core.SettingsManager
{
    public class Settings
    {

        private const string RunRegistryKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string ValueName = "PAM - Personal Activity Monitor";

        static Settings()
        {
            IdleTime = 30;
        }

        public static int IdleTime { get; set; }

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
                    if(key.GetValue(ValueName) != null)
                        key.DeleteValue(ValueName);
                }
                else
                {
                    key.SetValue(ValueName, Assembly.GetExecutingAssembly().Location);
                }
                key.Close();
            }
        }

    }
}
