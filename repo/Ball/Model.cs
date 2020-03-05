using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// INotifyPropertyChanged
using System.ComponentModel;

// debug output
using System.Diagnostics;

// timer, sleep
using System.Threading;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

// hi res timer
using PrecisionTimers;

// Rectangle
// Must update References manually
using System.Drawing;

// observable collections
using System.Collections.ObjectModel;

namespace Ball
{
    class Model : INotifyPropertyChanged
    {
        //for the ball
        private static UInt32 _numBalls = 1;
        private UInt32[] _buttonPresses = new UInt32[_numBalls];
        Random _randomNumber = new Random();
        private TimerQueueTimer.WaitOrTimerDelegate _ballTimerCallbackDelegate;
        private TimerQueueTimer.WaitOrTimerDelegate _paddleTimerCallbackDelegate;
        private TimerQueueTimer _ballHiResTimer;
        private TimerQueueTimer _paddleHiResTimer;
        private double _ballXMove = 1;
        private double _ballYMove = 1;
        System.Drawing.Rectangle _ballRectangle;
        System.Drawing.Rectangle _paddleRectangle;
        bool _movepaddleLeft = false;
        bool _movepaddleRight = false;
        private bool _moveBall = false;
        //end of the ball





        private uint _paddleCanvasTop;
        public uint paddleCanvasTop
        {
            get { return _paddleCanvasTop; }
            set
            {
                _paddleCanvasTop = value;
                OnPropertyChanged("paddleCanvasTop");
            }
        }

        private uint _paddleCanvasLeft;
        public uint paddleCanvasLeft
        {
            get { return _paddleCanvasLeft; }
            set
            {
                _paddleCanvasLeft = value;
                OnPropertyChanged("paddleCanvasLeft");
            }
        }

        private string _leftMouseButtonStatus = "UP";
        public string leftMouseButtonStatus
        {
            get { return _leftMouseButtonStatus; }
            set
            {
                _leftMouseButtonStatus = value;
                OnPropertyChanged("leftMouseButtonStatus");
            }
        }

        private string _rightMouseButtonStatus = "UP";
        public string rightMouseButtonStatus
        {
            get { return _rightMouseButtonStatus; }
            set
            {
                _rightMouseButtonStatus = value;
                OnPropertyChanged("rightMouseButtonStatus");
            }
        }

        public void ProcessMouseDrag(uint x, uint y)
        {
            paddleCanvasLeft = x;
            paddleCanvasTop = y;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void ProcessLMBDown()
        {
            leftMouseButtonStatus = "DOWN";
        }

        public void ProcessLMBUp()
        {
            leftMouseButtonStatus = "UP";
        }

        public void ProcessRMBDown()
        {
            rightMouseButtonStatus = "DOWN";
        }

        public void ProcessRMBUp()
        {
            rightMouseButtonStatus = "UP";
        }

    }
}
