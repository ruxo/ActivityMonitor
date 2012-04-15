using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PAM.Core.Implementation.Monitor;
using PAM.Core.SettingsManager;
using PAM.Utils.Export;

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
            set
            {
                Properties.Settings.Default.IdleTime = value;
                Properties.Settings.Default.Save();
            }
        }

        public bool AutoExportEnabled
        {
            get
            {
                return Properties.Settings.Default.AutoExportEnabled;
            }
            set
            {
                Properties.Settings.Default.AutoExportEnabled = value;
                Properties.Settings.Default.Save();
            }
        }

        public int AutoExportInterval
        {
            get
            {
                return Properties.Settings.Default.AutoExportInterval;
            }
            set
            {
                Properties.Settings.Default.AutoExportInterval = value;
                Properties.Settings.Default.Save();
            }
        }

        public string AutoExportPath
        {
            get
            {
                return Properties.Settings.Default.AutoExportPath;
            }
            set
            {
                Properties.Settings.Default.AutoExportPath = value;
                Properties.Settings.Default.Save();
            }
        }

        public void RunAutoExport(AppMonitor monitor)
        {
            Task.Factory.StartNew(() =>
                                      {
                                          if (AutoExportEnabled)
                                          {
                                              if (!Directory.Exists(AutoExportPath))
                                              {
                                                  Directory.CreateDirectory(AutoExportPath);
                                              }
                                              var file = File.Open(Path.Combine(AutoExportPath,
                                                                                DateTime.Now.Date.ToString("yyyy-MM-dd") + ".xml"),FileMode.Create,FileAccess.Write);
                                              var exporter = new DataExporter(monitor.Data);
                                              exporter.SaveToXml(file);
                                          }
                                          Thread.Sleep(AutoExportInterval*1000);
                                      }).ContinueWith((task => RunAutoExport(monitor)));
        }
    }
}