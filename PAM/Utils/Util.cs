using System.Reflection;
using Microsoft.Win32;

namespace PAM.Utils
{
    public static class AutoStarter
    {
        private const string RunRegistryKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string ValueName = "PAM - Personal Activity Monitor";
        
        public static void SetAutoStart()
        {
            var key = Registry.CurrentUser.CreateSubKey(RunRegistryKey);
            if (key != null) key.SetValue(ValueName, Assembly.GetExecutingAssembly().Location);
        }

        public static bool IsAutoStartEnabled
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
        }

        public static void UnSetAutoStart()
        {
            var key = Registry.CurrentUser.CreateSubKey(RunRegistryKey);
            if (key != null) key.DeleteValue(ValueName);
        }
    }
}