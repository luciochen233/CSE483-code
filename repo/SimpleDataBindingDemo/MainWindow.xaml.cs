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

namespace SimpleDataBindingDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    enum MY_COLOR { RED, GREEN, BLUE };

    public partial class MainWindow : Window
    {
        private Model _myModel;
        public MainWindow()
        {
            InitializeComponent();
            _myModel = new Model();
            this.DataContext = _myModel;
        }

        private void Update_TextBox_Click(object sender, RoutedEventArgs e)
        {
            _myModel.Update();
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Color_Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button bt = (System.Windows.Controls.Button)sender;
            if (bt.Name.Contains("Red")) _myModel.SetColor(MY_COLOR.RED);
            if (bt.Name.Contains("Green")) _myModel.SetColor(MY_COLOR.GREEN);
            if (bt.Name.Contains("Blue")) _myModel.SetColor(MY_COLOR.BLUE);
        }

        private void Top_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (e.Key == Key.Enter)
            {
                ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        private void Bottom_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (e.Key == Key.Enter)
            {
                ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }
    }
}
