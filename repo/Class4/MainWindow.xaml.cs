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

namespace Class4
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

        private void Bexit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Bcopy_Click(object sender, RoutedEventArgs e)
        {
            _myModel.CopyText();
        }

        private void TopBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if(e.Key == Key.Enter)
            {
                box.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                _myModel.CopyText();
            }
        }
    }
}
