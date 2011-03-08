﻿using System.Collections.Generic;
using PAM.Core.Implementation.ApplicationImp;

namespace PAM.Core
{
    /// <summary>
    /// Interaction logic for TreeViewer.xaml
    /// </summary>
    public partial class TreeViewer
    {

        private IEnumerable<Application> _applications;
        public TreeViewer()
        {
            InitializeComponent();
        }

        public IEnumerable<Application> Applications
        {
            set
            {
                _applications = value;
                tree.ItemsSource = _applications;
            }
        }

    }
}
