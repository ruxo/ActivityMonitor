using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Threading;
using PAM.Core.Abstract;
using PAM.Core.Implementation.ApplicationImp;

namespace PAM.Core.Implementation.Monitor
{
    public class AppUpdater
    {
        private static Applications _applications;
        private readonly Dispatcher _dispatcher;
        private string _previousApplicationName = string.Empty;
        public static Applications Applications
        {
            set { _applications = value; }
        }

        public static double GetMaxValue
        {
            get
            {
                return (from app in _applications
                        select app.TotalUsageTime.TotalSeconds).Max();
            }
        }

        public AppUpdater(Applications applications, Dispatcher dispatcher)
        {
            _applications = applications;
            _dispatcher = dispatcher;
        }

        public IApplication Update(Process process)
        {
            try
            {

                if (_previousApplicationName != process.MainModule.FileVersionInfo.FileDescription)
                {

                    if (_applications[_previousApplicationName] != null &&
                        _applications[_previousApplicationName].Usage.FindLast(u => !u.IsClosed) != null)
                    {
                        _applications[_previousApplicationName].Usage.FindLast(u => !u.IsClosed).End();
                    }

                    _previousApplicationName = process.MainModule.FileVersionInfo.FileDescription;

                    if (
                        !_applications.Contains(process.MainModule.FileVersionInfo.FileDescription,
                                                process.MainModule.FileVersionInfo.FileName))
                    {
                        try
                        {
                            using (var iconStream = new MemoryStream())
                            {
                                var icon = ShellIcon.GetSmallIcon(process.MainModule.FileVersionInfo.FileName);

                                icon.Save(iconStream);
                                iconStream.Seek(0, SeekOrigin.Begin);

                                var iconSource = System.Windows.Media.Imaging.BitmapFrame.Create(iconStream);

                                _dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => _applications.Add(
                                    new Application(
                                        process.MainModule.FileVersionInfo.FileDescription,
                                        process.MainModule.FileVersionInfo.FileName) { Icon = iconSource })));

                            }

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Source);
                            Debug.WriteLine(ex.Message);
                        }
                    }

                    var usage = new ApplicationUsage { DetailedName = process.MainWindowTitle };
                    usage.Start();
                    _applications[process.MainModule.FileVersionInfo.FileDescription].Usage.Add(usage);

                    
                }

                //update collection
                _dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => _applications.Refresh()));


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Source);
                Debug.WriteLine(ex.Message);
            }


             return _applications[_previousApplicationName];
        }

        protected IApplication CurrentApplication
        {
            get;
            private set;
        }
    }
}