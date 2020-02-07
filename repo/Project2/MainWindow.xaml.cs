using System;
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

namespace Project2
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

        private void Operation_Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button bt = (System.Windows.Controls.Button)sender;
            if (bt.Name.Contains("Bplus")) _myModel.Operation = "plus";
            if (bt.Name.Contains("Bminus")) _myModel.Operation = "minus";
            if (bt.Name.Contains("Btimes")) _myModel.Operation = "times";
            if (bt.Name.Contains("Bdivide")) _myModel.Operation = "divide";
        }

        private void Bequal_Click(object sender, RoutedEventArgs e)
        {
            _myModel.Calculate();
        }

        private void Bclear_Click(object sender, RoutedEventArgs e)
        {
            _myModel.Clear();
        }

        private void Bexit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
