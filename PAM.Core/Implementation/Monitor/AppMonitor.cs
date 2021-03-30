using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using LanguageExt.UnitsOfMeasure;
using Microsoft.FSharp.Core;
using Microsoft.Win32;
using PAM.Core.Implementation.ApplicationImp;
using PAM.Core.SettingsManager;
using PAM.Core.Tracker;

namespace PAM.Core.Implementation.Monitor
{
    public sealed class AppMonitor : INotifyPropertyChanged
    {
        readonly Timer      timer;
        readonly AppUpdater appUpdater;
        string              currentApplicationName           = string.Empty;
        TimeSpan            currentApplicationTotalUsageTime = TimeSpan.Zero;
        string              currentApplicationPath           = string.Empty;
        ImageSource?        currentApplicationIcon;
        readonly Dispatcher dispatcher;
        readonly AppTracker appTracker = new();

        readonly DateTime startTime = DateTime.Now;

        public Applications Data { get; }

        public ICollectionView SortedData { get; }

        public AppMonitor(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            Data            = new();
            SortedData      = CollectionViewSource.GetDefaultView(Data);
            SortedData.SortDescriptions.Add(new("TotalUsageTime", ListSortDirection.Descending));

            appUpdater = new(Data, dispatcher);

            timer         =  new() {Interval = 1000, AutoReset = false};
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string CurrentApplicationName
        {
            get => currentApplicationName;
            private set
            {
                if (value == currentApplicationName) return;
                currentApplicationName = value;
                NotifyPropertyChanged("CurrentApplicationName");
            }
        }

        public TimeSpan CurrentApplicationTotalUsageTime
        {
            get => currentApplicationTotalUsageTime;
            private set
            {
                if (value == currentApplicationTotalUsageTime) return;
                currentApplicationTotalUsageTime = value;
                NotifyPropertyChanged("CurrentApplicationTotalUsageTime");
            }
        }

        public string CurrentApplicationPath
        {
            get => currentApplicationPath;
            private set
            {
                if (value == currentApplicationPath) return;
                currentApplicationPath = value;
                NotifyPropertyChanged("CurrentApplicationPath");
            }
        }

        public ImageSource? CurrentApplicationIcon
        {
            get => currentApplicationIcon;
            private set
            {
                if (value == currentApplicationIcon) return;
                currentApplicationIcon = value;
                NotifyPropertyChanged("CurrentApplicationIcon");
            }
        }

        public TimeSpan TotalTimeSpentInApplications
        {
            get
            {
                var totalTime = Data.Sum(s => s.TotalTimeInMunites);
                return TimeSpan.FromMinutes(totalTime);
            }
        }

        public TimeSpan TotalTimeRunning                   => DateTime.Now.Subtract(startTime);
        void            NotifyPropertyChanged(string info) => PropertyChanged?.Invoke(this, new(info));

        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var currentProcess = Platform.getActiveProcess(Settings.IdleTime.Seconds());
            var appInfo        = appTracker.Update(currentProcess);

            if (appInfo.IsSome())
            {
                var ai = appInfo.Value;
                CurrentApplicationName           = $"({ai.Name}) {ai.Title}";
                CurrentApplicationPath           = ai.Path.GetOrElse("(unknown)");
                CurrentApplicationTotalUsageTime = ai.GetTotalUsageTime();

                if (ai.Icon.IsSome())
                    CurrentApplicationIcon = ai.Icon.Value;
                else if (ai.Path.IsSome())
                    dispatcher.Invoke(() =>
                    {
                        ai.Icon                = LoadIcon(ai.Path.Value);
                        CurrentApplicationIcon = ai.Icon.GetOrElse(null);
                    }, DispatcherPriority.Normal);
            }

            NotifyPropertyChanged("TotalTimeRunning");
            NotifyPropertyChanged("TotalTimeSpentInApplications");

            timer.Start();
        }

        static FSharpOption<ImageSource> LoadIcon(string path)
        {
            using var icon = Icon.ExtractAssociatedIcon(path);
            if (icon == null) return FSharpOption<ImageSource>.None;

            using var bmp    = icon.ToBitmap();
            var       stream = new MemoryStream();
            bmp.Save(stream, ImageFormat.Png);

            var bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            stream.Seek(0, SeekOrigin.Begin);
            bmpImage.StreamSource = stream;
            bmpImage.EndInit();
            return FSharpOption<ImageSource>.Some(bmpImage);
        }
    }
}