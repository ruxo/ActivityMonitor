using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using PAM.Core.Implementation.Monitor;

namespace PAM
{
    /// <summary>
    /// Interaction logic for MainGraphWindow.xaml
    /// </summary>
    public partial class MainGraphWindow
    {
        WindowState _lastWindowState;
        bool _shouldClose;
        private AppMonitor _monitor;

        public MainGraphWindow()
        {
            InitializeComponent();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            _lastWindowState = WindowState;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_shouldClose) return;
            e.Cancel = true;
            Hide();
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
            WindowState = _lastWindowState;
        }

        private void OnMenuItemExitClick(object sender, EventArgs e)
        {
            _shouldClose = true;
            Close();
        }



        private void FormLoaded(object sender, RoutedEventArgs e)
        {
            _monitor = new AppMonitor(this.Dispatcher);
            appsTree.Applications = _monitor.Data;
            CurrentApp.DataContext = _monitor;
        }


    }
}