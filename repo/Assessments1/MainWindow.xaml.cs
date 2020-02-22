﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assessments1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Model _myModel;
        public MainWindow()
        {
            InitializeComponent();
            _myModel = new Model();
            this.DataContext = _myModel;
        }

        private void But1_Click(object sender, RoutedEventArgs e)
        {
            _myModel.Convert();
        }

        private void Union_Intersection_Loaded(object sender, RoutedEventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            toolTip1.Content = "Input number seperated with , the max value is 100";
            Bin1.ToolTip = toolTip1;
        }
    }
}
