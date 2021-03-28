using System.Runtime.Versioning;
using PAM.Core.MvvmFramework;

namespace PAM.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private int _autoIdle;

        public int AutoIdle
        {
            get
            {
                // todo refactor ISettingsManager etc
                return Core.SettingsManager.Settings.IdleTime;
            }
            set
            {
                if (value == _autoIdle) return;

                _autoIdle = value;
                // todo refactor ISettingsManager etc
                Core.SettingsManager.Settings.IdleTime = value;
                RaisePropertyChanged("AutoIdle");
            }
        }

        [SupportedOSPlatform("windows")]
        public bool Autostart
        {
            get
            {
                return Core.SettingsManager.Settings.Autostart;
            }
            set
            {
                if (value == Autostart) return;

                Core.SettingsManager.Settings.Autostart = value;
                RaisePropertyChanged("Autostart");
            }
        }

        private bool _autoExportEnabled;
        public bool AutoexportEnabled
        {
            get { return Core.SettingsManager.Settings.AutoExportEnabled; }
            set
            {
                if (value == _autoExportEnabled) return;

                _autoExportEnabled = value;
                Core.SettingsManager.Settings.AutoExportEnabled = value;
                RaisePropertyChanged("AutoexportEnabled");
            }
        }

        private int _autoExportInterval = 10;
        public int AutoExportInterval
        {
            get { return Core.SettingsManager.Settings.AutoExportInterval; }
            set
            {
                if (value == _autoExportInterval) return;

                Core.SettingsManager.Settings.AutoExportInterval = value;
                _autoExportInterval = value;
                RaisePropertyChanged("AutoExportInterval");
            }
        }

        private string _autoExportPath = "Path";
        public string AutoExportPath
        {
            get { return Core.SettingsManager.Settings.AutoExportPath; }
            set
            {
                if (value == _autoExportPath) return;

                Core.SettingsManager.Settings.AutoExportPath = value;
                _autoExportPath = value;
                RaisePropertyChanged("AutoExportPath");
            }
        }
    }
}
