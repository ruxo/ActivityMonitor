using System.Collections;
using System.Windows.Controls;

namespace PAM.Core
{
    /// <summary>
    /// Interaction logic for TreeViewer.xaml
    /// </summary>
    public partial class TreeViewer
    {
        public TreeViewer()
        {
            InitializeComponent();
        }

        public IEnumerable Applications
        {
            get { return treeView1.ItemsSource; }
            set { treeView1.ItemsSource = value; }
        }

    }
}
