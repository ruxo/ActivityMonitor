using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using LanguageExt;
using PAM.Core.Abstract;
using PAM.Core.Implementation.ApplicationImp;
using RZ.Foundation.Extensions;
using static LanguageExt.Prelude;

namespace PAM.Core.Implementation.Monitor
{
    public sealed class AppUpdater
    {
        static   Applications? _applications;
        readonly Dispatcher    dispatcher;
        string                 previousApplicationName = string.Empty;

        public static double GetMaxValue => _applications?.Select(app => app.TotalUsageTime.TotalSeconds).Max() ?? 0;

        public AppUpdater(Applications applications, Dispatcher dispatcher)
        {
            _applications   = applications;
            this.dispatcher = dispatcher;
        }

        public void Stop(Process process)
        {
            var lastUsage = lastUsageOf(previousApplicationName);
            lastUsage.Do(u => u.End());

            var currentProcess = process.MainModule!.FileVersionInfo.FileDescription!;
            var currentUsage   = lastUsageOf(currentProcess);
            currentUsage.Do(u => u.End());
        }

        public Option<IApplication> Update(Process process)
        {
            // some process has higher priviledge and cannot retrieve the file info.
            var fn = Try(() => process.MainModule!.FileVersionInfo.FileName).ToOption();
            if (fn.IsNone)
                return None;
            var fileName = fn.Get();

            var processName     = process.ProcessName;
            if (processName == null) throw new InvalidOperationException("Unexpected invalid process name");

            var lastUsage = lastUsageOf(previousApplicationName);
            if (previousApplicationName == processName)
                return _applications![previousApplicationName];

            lastUsage.Do(u => u.End());

            previousApplicationName = processName;

            if (!_applications!.Contains(processName, fileName))
                dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    var          icon     = Icon.ExtractAssociatedIcon(fileName);
                    BitmapImage? bmpImage = null;
                    if (icon != null)
                    {
                        using var bmp  = icon.ToBitmap();
                        var       strm = new MemoryStream();
                        bmp.Save(strm, System.Drawing.Imaging.ImageFormat.Png);

                        bmpImage = new();

                        bmpImage.BeginInit();
                        strm.Seek(0, SeekOrigin.Begin);
                        bmpImage.StreamSource = strm;
                        bmpImage.EndInit();
                    }

                    Application application                = new(processName, fileName);
                    if (bmpImage != null) application.Icon = bmpImage;

                    _applications.Add(application);
                }));

            var usage = ApplicationUsage.Start(process.MainWindowTitle);
            _applications[processName].Do(a => a.Usage.Add(usage));

            //update collection
            dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => _applications.UIRefresh()));

            return _applications[previousApplicationName];
        }

        Option<ApplicationUsage> lastUsageOf(string appName) =>
            _applications![appName].Bind(a => Optional(a.Usage.FindLast(u => !u.IsClosed)!));
    }
}