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

namespace Class1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void B1_Click(object sender, RoutedEventArgs e)
        {
            if(WB1.Text == "Hello World!")
            {
                WB1.Text = "你好世界";
            }
            else
            {
                WB1.Text = "Hello World!";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PB1.Value += (99 - PB1.Value)/10;
        }
    }
}
