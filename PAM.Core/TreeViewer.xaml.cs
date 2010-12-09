﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PAM.Core
{
    /// <summary>
    /// Interaction logic for TreeViewer.xaml
    /// </summary>
    public partial class TreeViewer : UserControl
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
