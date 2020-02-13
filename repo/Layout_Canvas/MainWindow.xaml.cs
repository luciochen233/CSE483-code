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

namespace Layout_Canvas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random randomNumber = new Random(Guid.NewGuid().GetHashCode());
        private enum enumShapeType { ELLIPSE, RECTANGLE };


        public MainWindow()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
        }
        private void SetRandomShapes(Canvas e, enumShapeType t, int number)
        {
            double canvasHeight = e.ActualHeight;
            double canvasWidth = e.ActualWidth;
            Shape shape = new Ellipse();

            for (int count=0; count< number; count++)
            {
                switch (t)
                {
                    case enumShapeType.ELLIPSE:
                        shape = new Ellipse();
                        break;

                    case enumShapeType.RECTANGLE:
                        shape = new Rectangle();
                        break;

                }
                shape.Height = randomNumber.Next(10,25);
                shape.Width = shape.Height;
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();

                // Describes the brush's color using RGB values. 
                // Each value has a range of 0-255.

                mySolidColorBrush.Color = Color.FromArgb(255, (byte)randomNumber.Next(0,255), (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255));
                shape.Fill = mySolidColorBrush;
                e.Children.Add(shape);
                Canvas.SetLeft(shape, randomNumber.Next(0,(int)(canvasWidth-shape.Width)));
                Canvas.SetTop(shape, randomNumber.Next(0,(int)(canvasHeight-shape.Height)));
            }

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetRandomShapes(TopCanvas, enumShapeType.ELLIPSE, 25);
            SetRandomShapes(BottomCanvas, enumShapeType.RECTANGLE, 25);
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            TopCanvas.Children.Clear();
            BottomCanvas.Children.Clear();
            SetRandomShapes(TopCanvas, enumShapeType.ELLIPSE, int.Parse(NumShapes.Text));
            SetRandomShapes(BottomCanvas, enumShapeType.RECTANGLE, int.Parse(NumShapes.Text));
        }
    }

}
