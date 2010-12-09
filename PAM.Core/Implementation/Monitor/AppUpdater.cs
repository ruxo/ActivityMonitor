using System;
using System.Diagnostics;
using System.IO;

namespace PAM.Core.Implementation.Monitor
{
    public class AppUpdater
    {
        private readonly Applications _applications;
        private string _previousApplicationName = string.Empty;

        public AppUpdater(Applications applications)
        {
            _applications = applications;
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


                                _applications.Add(new Application.Application(process.MainModule.FileVersionInfo.FileDescription,
                                                                    process.MainModule.FileVersionInfo.FileName) { Icon = iconSource });
                            }

                        }
                        catch
                        {


                        }
                    }

                    var usage = new ApplicationUsage();
                    usage.DetailedName = process.MainWindowTitle;
                    usage.Start();
                    _applications[process.MainModule.FileVersionInfo.FileDescription].Usage.Add(usage);

                }
            }
            catch (Exception)
            {

            }
        }
    }
}