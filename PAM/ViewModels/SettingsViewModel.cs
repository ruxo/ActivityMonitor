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

    }
}
