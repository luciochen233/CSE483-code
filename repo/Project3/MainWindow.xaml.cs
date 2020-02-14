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

namespace Project3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random randomNumber = new Random(Guid.NewGuid().GetHashCode());
        private enum enumShapeType { ELLIPSE, RECTANGLE , TRIANGLE};
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

            for (int count = 0; count < number; count++)
            {
                switch (t)
                {
                    case enumShapeType.ELLIPSE:
                        shape = new Ellipse();
                        break;

                    case enumShapeType.RECTANGLE:
                        shape = new Rectangle();
                        break;
                    case enumShapeType.TRIANGLE:
                        Polygon temp = new Polygon();
                        int temp_len = randomNumber.Next(10, 25);
                        //10,110 60,10 110,110
                        System.Windows.Point Point1 = new System.Windows.Point(0, 0);
                        System.Windows.Point Point2 = new System.Windows.Point(base, 0);
                        System.Windows.Point Point3 = new System.Windows.Point(7, 7);
                        PointCollection myPointCollection = new PointCollection
                        {
                            Point1,
                            Point2,
                            Point3
                        };
                        temp.Points = myPointCollection;
                        shape = temp;
                        break;

                }
                
                shape.Height = randomNumber.Next(10, 25);
                shape.Width = shape.Height;
                
                

                SolidColorBrush mySolidColorBrush = new SolidColorBrush();

                // Describes the brush's color using RGB values. 
                // Each value has a range of 0-255.

                mySolidColorBrush.Color = Color.FromArgb(255, (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255));
                shape.Fill = mySolidColorBrush;
                e.Children.Add(shape);
                Canvas.SetLeft(shape, randomNumber.Next(0, (int)(canvasWidth - shape.Width)));
                Canvas.SetTop(shape, randomNumber.Next(0, (int)(canvasHeight - shape.Height)));
            }

        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            SetRandomShapes(Canvas00, enumShapeType.ELLIPSE, 25);
            SetRandomShapes(Canvas01, enumShapeType.RECTANGLE, 25);
            SetRandomShapes(Canvas10, enumShapeType.TRIANGLE, 25);
            SetRandomShapes(Canvas11, enumShapeType.ELLIPSE, 25);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Canvas00.Children.Clear();
            Canvas01.Children.Clear();
            Canvas10.Children.Clear();
            Canvas11.Children.Clear();
            SetRandomShapes(Canvas00, enumShapeType.ELLIPSE, 25);
            SetRandomShapes(Canvas01, enumShapeType.RECTANGLE, 25);
            SetRandomShapes(Canvas10, enumShapeType.TRIANGLE, 25);
            SetRandomShapes(Canvas11, enumShapeType.ELLIPSE, 25);
        }

        private void B00_Click(object sender, RoutedEventArgs e)
        {
            Canvas00.Children.Clear();
            SetRandomShapes(Canvas00, enumShapeType.ELLIPSE, 25);
        }

        private void B01_Click(object sender, RoutedEventArgs e)
        {
            Canvas01.Children.Clear();
            SetRandomShapes(Canvas01, enumShapeType.RECTANGLE, 25);
        }

        private void B10_Click(object sender, RoutedEventArgs e)
        {
            Canvas10.Children.Clear();
            SetRandomShapes(Canvas10, enumShapeType.TRIANGLE, 25);
        }

        private void B11_Click(object sender, RoutedEventArgs e)
        {
            Canvas11.Children.Clear();
            SetRandomShapes(Canvas11, enumShapeType.ELLIPSE, 25);
        }
    }
}
