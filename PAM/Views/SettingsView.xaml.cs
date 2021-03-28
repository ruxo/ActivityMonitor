using System.Windows;
using System.Windows.Forms;
using PAM.ViewModels;

namespace PAM.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        void ButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() != DialogResult.OK) return;
            var model = (SettingsViewModel)DataContext;
            model.AutoExportPath = dialog.SelectedPath;
        }




    }
}
