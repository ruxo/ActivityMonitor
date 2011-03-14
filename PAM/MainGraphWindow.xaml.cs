using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PAM.Core.Implementation.Monitor;
using PAM.Utils;

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

            AutostartMenuItem.Checked = AutoStarter.IsAutoStartEnabled;
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

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            var persister = new AppMonitorPersister(_monitor.Applications);
            persister.Save();
        }

        private void ReadButtonClick(object sender, RoutedEventArgs e)
        {
            var persister = new AppMonitorPersister(_monitor.Applications);
            _monitor.Applications = persister.Restore();

            appsTree.Applications = _monitor.Data;

        }

        private void FilterButtonClick(object sender, RoutedEventArgs e)
        {
            var result = from data in _monitor.Data
                         where (from details in data.Details
                                where details.Usages.Count > 10
                                select details) != null
                         select data;

            appsTree.Applications = result;
        }

        private void OnMenuItemAutostartClick(object sender, EventArgs e)
        {
            AutostartMenuItem.Checked = !AutostartMenuItem.Checked;

            if (AutostartMenuItem.Checked)
            {
                AutoStarter.SetAutoStart();
            }
            else
            {
                AutoStarter.UnSetAutoStart();
            }

        }




    }
}