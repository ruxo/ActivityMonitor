using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;
using PAM.Core.Implementation.ApplicationImp;
using PAM.Core.SettingsManager;

namespace PAM.Core.Implementation.Monitor
{
    public class AppMonitor : INotifyPropertyChanged
    {
        private readonly Timer _timer;
        private readonly AppUpdater _appUpdater;
        private string _currentApplicationName;
        private TimeSpan _currentApplicationTotalUsageTime;
        private string _currentApplicationPath;
        private ImageSource _currentApplicationIcon;
        public Applications Applications
        {
            get { return Data; }
            set
            {
                Data = value;
                AppUpdater.Applications = value;
            }
        }

        private readonly DateTime _startTime = DateTime.Now;

        public AppMonitor(Dispatcher dispatcher)
        {
            _timer = new Timer { Interval = 1000 };
            _timer.Elapsed += TimerElapsed;
            Data = new Applications();
            _appUpdater = new AppUpdater(Data, dispatcher);

            _timer.Start();
            SystemEvents.SessionSwitch += SystemEventsSessionSwitch;
        }

        private bool _sessionStopped;
        public void SystemEventsSessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                case SessionSwitchReason.SessionLock:
                    _sessionStopped = true;
                    break;
                case SessionSwitchReason.SessionUnlock:
                    _sessionStopped = false;
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();

            try
            {
                // turn the timer off while processing elapsed because we want to avoid problems with threads - which are not needed here
                var handle = WinApi.GetForegroundWindow();
                int processId;
                //todo write result to trace and add try catch
                WinApi.GetWindowThreadProcessId(new HandleRef(null, handle), out processId);
                var process = Process.GetProcessById(processId);

                // checking if the user is in iddle mode - if so, dont updat process
                // todo refactor
                var inputInfo = new WinApi.Lastinputinfo();
                inputInfo.cbSize = (uint)Marshal.SizeOf(inputInfo);
                WinApi.GetLastInputInfo(ref inputInfo);
                var iddleTime = (Environment.TickCount - inputInfo.dwTime) / 1000;
                if (iddleTime < Settings.IdleTime && _sessionStopped == false)
                { // iddle time is less 30 sec then update process

                    var currentApplication = _appUpdater.Update(process);
                    if (currentApplication != null)
                    {
                        CurrentApplicationName = currentApplication.Name;
                        CurrentApplicationTotalUsageTime = currentApplication.TotalUsageTime;
                        CurrentApplicationPath = currentApplication.Path;
                        CurrentApplicationIcon = currentApplication.Icon;
                    }


                }
                else
                {
                    _appUpdater.Stop(process);
                }
            }
            catch (Exception)
            {

                // todo logging
            }

            NotifyPropertyChanged("TotalTimeRunning");
            NotifyPropertyChanged("TotalTimeSpentInApplications");

            _timer.Start();
        }


        private Applications _data;
        public Applications Data
        {
            get { return _data; }
            private set
            {
                _data = value;
                SortedData = CollectionViewSource.GetDefaultView(_data);
                SortedData.SortDescriptions.Add(new SortDescription("TotalUsageTime",ListSortDirection.Descending));
            }
        }


        public ICollectionView SortedData { get; private set; }

        public string CurrentApplicationName
        {
            get { return _currentApplicationName; }
            private set
            {
                if (value == null || value == _currentApplicationName) return;
                _currentApplicationName = value;
                NotifyPropertyChanged("CurrentApplicationName");
            }
        }


        public TimeSpan CurrentApplicationTotalUsageTime
        {
            get { return _currentApplicationTotalUsageTime; }
            private set
            {
                if (value == _currentApplicationTotalUsageTime) return;
                _currentApplicationTotalUsageTime = value;
                NotifyPropertyChanged("CurrentApplicationTotalUsageTime");
            }
        }

        public string CurrentApplicationPath
        {
            get { return _currentApplicationPath; }
            private set
            {
                if (value == _currentApplicationPath) return;
                _currentApplicationPath = value;
                NotifyPropertyChanged("CurrentApplicationPath");
            }
        }



        public ImageSource CurrentApplicationIcon
        {
            get { return _currentApplicationIcon; }
            private set
            {
                if (value == _currentApplicationIcon) return;
                _currentApplicationIcon = value;
                NotifyPropertyChanged("CurrentApplicationIcon");
            }
        }


        public TimeSpan TotalTimeSpentInApplications
        {
            get
            {
                var totalTime = Applications.Sum(s => s.TotalTimeInMunites);
                return TimeSpan.FromMinutes(totalTime);
            }


        }

        public TimeSpan TotalTimeRunning
        {
            get { return DateTime.Now.Subtract(_startTime); }

        }


    }
}