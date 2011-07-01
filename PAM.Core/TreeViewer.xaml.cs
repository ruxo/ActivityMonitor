using System.Collections.Generic;
using System.Windows.Data;
using PAM.Core.Converters;
using PAM.Core.Implementation.ApplicationImp;

namespace PAM.Core
{
    /// <summary>
    /// Interaction logic for TreeViewer.xaml
    /// </summary>
    public partial class TreeViewer
    {

        private CollectionView _applications;
        public TreeViewer()
        {
            InitializeComponent();
        }

        public CollectionView Applications
        {
            set
            {
                _applications = value;
                tree.ItemsSource = _applications;
            }
            get { return _applications; }
        }

        private void TreeSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            WidthCalculator.MaxControlWidth = tree.ActualWidth;
        }



    }
}
