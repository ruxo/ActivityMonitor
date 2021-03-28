using System.Diagnostics;
using System.Windows.Navigation;

namespace PAM.Views
{
    public partial class NewVersionPopupView
    {
        public NewVersionPopupView()
        {
            InitializeComponent();
        }

        void HyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));

            e.Handled = true;
        }
    }
}
