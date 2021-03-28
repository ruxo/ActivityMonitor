using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Timers;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using LanguageExt;
using Microsoft.Win32;
using PAM.Core.Implementation.ApplicationImp;
using PAM.Core.SettingsManager;
using RZ.Foundation.Extensions;
using static LanguageExt.Prelude;

namespace PAM.Core.Implementation.Monitor
{
    [SupportedOSPlatform("windows")]
    public sealed class AppMonitor : INotifyPropertyChanged
    {
        readonly Timer      timer;
        readonly AppUpdater appUpdater;
        string              currentApplicationName           = string.Empty;
        TimeSpan            currentApplicationTotalUsageTime = TimeSpan.Zero;
        string              currentApplicationPath           = string.Empty;
        ImageSource?        currentApplicationIcon;

        readonly DateTime startTime = DateTime.Now;

        public Applications Data { get; }

        public ICollectionView SortedData { get; }

        public AppMonitor(Dispatcher dispatcher)
        {
            Data       = new();
            SortedData = CollectionViewSource.GetDefaultView(Data);
            SortedData.SortDescriptions.Add(new("TotalUsageTime", ListSortDirection.Descending));

            appUpdater = new(Data, dispatcher);

            SystemEvents.SessionSwitch += SystemEventsSessionSwitch;

            timer         =  new() {Interval = 1000, AutoReset = false};
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        #region User session detection

        bool sessionIsActive = true;

        void SystemEventsSessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            sessionIsActive = e.Reason == SessionSwitchReason.SessionUnlock ||
                              e.Reason == SessionSwitchReason.RemoteConnect ||
                              e.Reason == SessionSwitchReason.SessionRemoteControl;
        }

        #endregion

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
            var handle = WinApi.GetForegroundWindow();

            WinApi.GetWindowThreadProcessId(new(null, handle), out var processId);
            var process   = TryGetProcessById(processId);
            var inputInfo = WinApi.GetLastInputInfo();

            var _ = from p in process
                    from i in inputInfo
                    select UpdateCurrentApplication(p, i);

            NotifyPropertyChanged("TotalTimeRunning");
            NotifyPropertyChanged("TotalTimeSpentInApplications");

            timer.Start();
        }

        Unit UpdateCurrentApplication(Process process, WinApi.LastInputInfo inputInfo)
        {
            // checking if the user is in idle mode - if so, don't update process
            var idleTime = (Environment.TickCount - inputInfo.dwTime) / 1000;
            if (idleTime < Settings.IdleTime && sessionIsActive)
            {
                appUpdater.Update(process)
                          .Then(a =>
                           {
                               CurrentApplicationName           = a.Name;
                               CurrentApplicationTotalUsageTime = a.TotalUsageTime;
                               CurrentApplicationPath           = a.Path;
                               CurrentApplicationIcon           = a.Icon;
                           });
            }
            else
                appUpdater.Stop(process);

            return Unit.Default;
        }

        static Option<Process> TryGetProcessById(int processId) => Try(() => Process.GetProcessById(processId)).ToOption();
    }
}