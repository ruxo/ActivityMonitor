using PAM.Core.Implementation.ApplicationImp;

namespace PAM.Core
{
    /// <summary>
    /// Interaction logic for TreeViewer.xaml
    /// </summary>
    public partial class TreeViewer
    {

        private Applications _applications;
        public TreeViewer()
        {
            InitializeComponent();
        }

        public Applications Applications
        {
            set
            {
                _applications = value;
                tree.ItemsSource = _applications;
               
            }
        }


    }
}
