using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using PAM.Core;
using PAM.Core.Implementation.Monitor;
using Application = PAM.Core.Implementation.Application.Application;

namespace PAM
{
    /// <summary>
    /// Interaction logic for MainGraphWindow.xaml
    /// </summary>
    public partial class MainGraphWindow : Window
    {
        WindowState lastWindowState;
        bool shouldClose;

        public MainGraphWindow()
        {
            InitializeComponent();

        }

        protected override void OnStateChanged(EventArgs e)
        {
            lastWindowState = WindowState;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!shouldClose)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void OnNotificationAreaIconDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Open();
            }
        }

        private void OnMenuItemOpenClick(object sender, EventArgs e)
        {
            Open();
        }

        private void Open()
        {
            Show();
            WindowState = lastWindowState;
        }

        private void OnMenuItemExitClick(object sender, EventArgs e)
        {
            shouldClose = true;
            Close();
        }


        private Timer _timer;
        private Applications _applications;
        private string _previousApplicationName;
        private AppMonitor _monitor; 

        private void FormLoaded(object sender, RoutedEventArgs e)
        {

            //_timer = new Timer(1000);
            //_timer.Elapsed += _timer_Elapsed;
            //_applications = new Applications();


            //_timer.Enabled = true;

            _monitor = new AppMonitor();

        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false;

            var handle = GetForegroundWindow();

            int processId;

            var result = GetWindowThreadProcessId(new HandleRef(null, handle), out processId);

            try
            {
                var process = Process.GetProcessById(processId);

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


                        using (var iconStream = new MemoryStream())
                        {
                            var icon2 = ShellIcon.GetSmallIcon(process.MainModule.FileVersionInfo.FileName);

                            icon2.Save(iconStream);
                            iconStream.Seek(0, SeekOrigin.Begin);

                            var iconSource = System.Windows.Media.Imaging.BitmapFrame.Create(iconStream);


                            _applications.Add(new Application(process.MainModule.FileVersionInfo.FileDescription,
                                                                process.MainModule.FileVersionInfo.FileName) { Icon = iconSource });
                        }
                    }

                    var usage = new ApplicationUsage();
                    usage.Start();
                    _applications[process.MainModule.FileVersionInfo.FileDescription].Usage.Add(usage);

                }




                this.InvokeIfRequired((value) => appNameLabel.Content = value, process.MainModule.FileVersionInfo.FileDescription);
                this.InvokeIfRequired((value) => appPathLabel.Content = value, process.MainModule.FileVersionInfo.FileName);
                this.InvokeIfRequired((value) => appManufacturer.Content = value, process.MainModule.FileVersionInfo.CompanyName);
                this.InvokeIfRequired((value) => appUsage.Content = value, _applications[process.MainModule.FileVersionInfo.FileDescription].TotalUsageTime);

                var icon = ShellIcon.GetLargeIcon(process.MainModule.FileVersionInfo.FileName);


                using (var iconStream = new MemoryStream())
                {
                    icon.Save(iconStream);
                    iconStream.Seek(0, SeekOrigin.Begin);

                    var iconSource = System.Windows.Media.Imaging.BitmapFrame.Create(iconStream);

                    this.InvokeIfRequired((value) => appIcon.Source = value, iconSource);
                }


                this.InvokeIfRequired(() =>
                                          {
                                              const int maxWidthAvaliable = 400;
                                              var longestBarWidth = (from app in _applications
                                                                     select app.TotalUsageTime.TotalMinutes).Max();


                                              apps.Children.Clear();
                                              foreach (var app in _applications)
                                              {
                                                  var appStat = new AppStat
                                                                    {
                                                                        AppName =
                                                                            app.Name + " (" +
                                                                            app.TotalUsageTime.TotalMinutes.ToString("0") +
                                                                            ")",
                                                                        Progress =
                                                                            maxWidthAvaliable / longestBarWidth *
                                                                            app.TotalUsageTime.TotalMinutes,
                                                                        TimeSpent =
                                                                            app.TotalUsageTime.TotalMinutes.ToString(
                                                                                "0 minutes"),
                                                                        Icon = app.Icon
                                                                    };
                                                  apps.Children.Add(appStat);
                                              }

                                          });
            }
            catch
            {

            }
            finally
            {
                _timer.Enabled = true;
            }
        }





        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

    }
}