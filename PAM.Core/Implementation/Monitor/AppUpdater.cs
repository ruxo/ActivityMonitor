using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Threading;

namespace PAM.Core.Implementation.Monitor
{
    public class AppUpdater
    {
        private readonly Applications _applications;
        private readonly Dispatcher _dispatcher;
        private string _previousApplicationName = string.Empty;

        public AppUpdater(Applications applications, Dispatcher dispatcher)
        {
            _applications = applications;
            _dispatcher = dispatcher;
        }

        public void Update(Process process)
        {
            try
            {


                if (_previousApplicationName != process.MainModule.FileVersionInfo.FileDescription)
                {


                    if (_applications[_previousApplicationName] != null &&
                        _applications[_previousApplicationName].Usage.FindLast((u) => !u.IsClosed) != null)
                    {
                        _applications[_previousApplicationName].Usage.FindLast((u) => !u.IsClosed).End();
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
                                    new Application.Application(
                                        process.MainModule.FileVersionInfo.FileDescription,
                                        process.MainModule.FileVersionInfo.FileName) { Icon = iconSource })));

                            }

                        }
                        catch (Exception ex)
                        {


                        }
                    }

                    var usage = new ApplicationUsage();
                    usage.DetailedName = process.MainWindowTitle;
                    usage.Start();
                    _applications[process.MainModule.FileVersionInfo.FileDescription].Usage.Add(usage);


                    
                }



                //update collection
                _dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                {
                    _applications.Refresh();
                }));
            }
            catch (Exception)
            {

            }



        }
    }
}