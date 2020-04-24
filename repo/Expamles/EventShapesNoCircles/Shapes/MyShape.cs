using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// INotifyPropertyChanged
using System.ComponentModel;

// Brush
using System.Windows.Media;



namespace EventShapes
{
    public class MyShape : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private double _canvasTop;
        public double CanvasTop
        {
            get { return _canvasTop; }
            set
            {
                _canvasTop = value;
                OnPropertyChanged("CanvasTop");
            }
        }

        private double _canvasLeft;
        public double CanvasLeft
        {
            get { return _canvasLeft; }
            set
            {
                _canvasLeft = value;
                OnPropertyChanged("CanvasLeft");
            }
        }

        private double _height;
        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged("Height");
            }
        }

        private double _width;
        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged("Width");
            }
        }

        private Brush _fill;
        public Brush Fill
        {
            get { return _fill; }
            set
            {
                _fill = value;
                OnPropertyChanged("Fill");
            }
        }

        private PointCollection _triangle;
        public PointCollection Tri_dimention
        {
            get { return _triangle; }
            set
            {
                _triangle = value;
                OnPropertyChanged("Tri_dimention");
            }
        }

        public void Calculate_triangle()
        {
            System.Windows.Point Point1 = new System.Windows.Point(Width / 2, 0);
            System.Windows.Point Point2 = new System.Windows.Point(0,Height);
            System.Windows.Point Point3 = new System.Windows.Point(Width, Height);

            //System.Windows.Point Point1 = new System.Windows.Point(10, 10);
            //System.Windows.Point Point2 = new System.Windows.Point(15, 15);
            //System.Windows.Point Point3 = new System.Windows.Point(10, 15);
            PointCollection myPointCollection = new PointCollection
                        {
                            Point1,
                            Point2,
                            Point3
                        };
            Tri_dimention = myPointCollection;
        }

        public string Name { get; set; }

        public void ChangeColorHandler(object sender, EventShapes.Model.ChangeColorEventArgs e)
        {
            Fill = e.brush;
        }
    }
}
