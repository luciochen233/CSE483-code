using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// INotifyPropertyChanged
using System.ComponentModel;

// Brushes
using System.Windows.Media;


namespace PaddleDemo
{
    public partial class Model : INotifyPropertyChanged
    {
        private bool _move = false;
        public bool Move
        {
            get { return _move; }
            set
            {
                _move = value;
                OnPropertyChanged("Move");
            }
        }


        private double _ballCanvasTop;
        public double BallCanvasTop
        {
            get { return _ballCanvasTop; }
            set
            {
                _ballCanvasTop = value;
                OnPropertyChanged("BallCanvasTop");
            }
        }

        private double _ballCanvasLeft;
        public double BallCanvasLeft
        {
            get { return _ballCanvasLeft; }
            set
            {
                _ballCanvasLeft = value;
                OnPropertyChanged("BallCanvasLeft");
            }
        }

        private double _ballHeight;
        public double BallHeight
        {
            get { return _ballHeight; }
            set
            {
                _ballHeight = value;
                OnPropertyChanged("ballHeight");
            }
        }

        private double _ballWidth;
        public double BallWidth
        {
            get { return _ballWidth; }
            set
            {
                _ballWidth = value;
                OnPropertyChanged("BallWidth");
            }
        }

        public void ProcessMouseDrag(uint x, uint y)
        {
            if (Move)
            {
                BallCanvasLeft = x - BallWidth / 2;
                BallCanvasTop = y - BallHeight / 2;
            }
            
        }

    }
}
