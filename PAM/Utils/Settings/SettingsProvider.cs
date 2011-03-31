using PAM.Core.SettingsManager;

namespace PAM.Utils.Settings
{
    public class SettingsProvider : ISettingsProvider
    {
        public int IdleTime
        {
            get
            {
                return Properties.Settings.Default.IdleTime;
            }
            set { 
                Properties.Settings.Default.IdleTime = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}