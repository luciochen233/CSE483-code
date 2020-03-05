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

namespace MousePosition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Model myModel;
        public MainWindow()
        {
            InitializeComponent();

            myModel = new Model();
            this.DataContext = myModel;
        }

        private void BallCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(this);
            myModel.ProcessMouseDrag((uint)p.X, (uint)p.Y);
        }

        private void BallCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            myModel.ProcessLMBDown();
        }

        private void BallCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            myModel.ProcessLMBUp();
        }

        private void BallCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            myModel.ProcessRMBDown();
        }

        private void BallCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            myModel.ProcessRMBUp();
        }
    }
}
