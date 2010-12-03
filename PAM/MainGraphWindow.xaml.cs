using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Input;

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
        private void FormLoaded(object sender, RoutedEventArgs e)
        {

            _timer = new Timer(1000);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Enabled = true;
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var chars = 256;
            var buff = new StringBuilder(chars);

            var handle = GetForegroundWindow();

            int processId;
            var result = GetWindowThreadProcessId(new HandleRef(null, handle), out processId);

            try
            {
                var process = Process.GetProcessById(processId);


                this.InvokeIfRequired((value) => appNameLabel.Content = value, process.MainModule.FileVersionInfo.FileDescription);
                this.InvokeIfRequired((value) => appPathLabel.Content = value, process.MainModule.FileVersionInfo.FileName);
                this.InvokeIfRequired((value) => appManufacturer.Content = value, process.MainModule.FileVersionInfo.CompanyName);
                var icon = ShellIcon.GetLargeIcon(process.MainModule.FileVersionInfo.FileName);


                using (MemoryStream iconStream = new MemoryStream())
                {
                    icon.Save(iconStream);
                    iconStream.Seek(0, SeekOrigin.Begin);

                    var iconSource = System.Windows.Media.Imaging.BitmapFrame.Create(iconStream);

                    this.InvokeIfRequired((value) => appIcon.Source = value, iconSource);
                }
            }
            catch
            {

            }
        }





        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

    }
}