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
        private string operation = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Bplus_Click(object sender, RoutedEventArgs e)
        {
            Text_operation.Text = "Plus";
            operation = "plus";
        }

        private void Bminus_Click(object sender, RoutedEventArgs e)
        {
            Text_operation.Text = "Minus";
            operation = "minus";
        }

        private void Btimes_Click(object sender, RoutedEventArgs e)
        {
            Text_operation.Text = "Times";
            operation = "times";
        }

        private void Bdivide_Click(object sender, RoutedEventArgs e)
        {
            Text_operation.Text = "Divide";
            operation = "divide";
        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private double GetResult(double x, double y, string op)
        {
            switch (op)
            {
                case "times": return x * y;
                case "minus": return x - y;
                case "divide": return (x / y);
                case "plus": return x + y;
            }
            return 0.0;
        }

        private void Bequal_Click(object sender, RoutedEventArgs e)
        {
            double firstNum = 0;
            double secondNum = 0;
            try
            {
                firstNum = double.Parse(Input1.Text);
                secondNum = double.Parse(Input2.Text);
                double temp = GetResult(firstNum, secondNum, operation);
                Result.Text = temp.ToString();
                Text_error.Text = "";
            }
            catch(Exception ex)
            {
                Text_error.Text = ex.Message;
                Text_error.Foreground = Brushes.Red;
            }

        }

        private void Bclear_Click(object sender, RoutedEventArgs e)
        {
            Input1.Clear();
            Input2.Clear();
            Text_operation.Clear();
            Result.Clear();
            operation = "";
            Text_error.Text = "All clear";
        }

        private void Bexit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
