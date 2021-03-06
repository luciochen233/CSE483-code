﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// observable collections
using System.Collections.ObjectModel;

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

// INotifyPropertyChanged
using System.ComponentModel;

// Timer.Timer
using System.Timers;

namespace BrickBreakergame
{
    public partial class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

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

        //Brick Collection
        public ObservableCollection<Brick> BrickCollection;
        private static UInt32 _numBricks = 45;
        Rectangle[] _brickRectangles = new Rectangle[_numBricks];
        // note that the brick hight, number of brick columns and rows
        // must match our window demensions.
        private double _brickHeight = 25;
        private double _brickWidth = 70;



        private bool _moveBall = false;
        public bool MoveBall
        {
            get { return _moveBall; }
            set { _moveBall = value; }
        }

        private double _windowHeight = 100;
        public double WindowHeight
        {
            get { return _windowHeight; }
            set { _windowHeight = value; }
        }

        private double _windowWidth = 100;
        public double WindowWidth
        {
            get { return _windowWidth; }
            set { _windowWidth = value; }
        }

        private uint _score = 0;
        public uint Score
        {
            get { return _score; }
            set { _score = value; OnPropertyChanged("Score"); }
        }


        /// <summary>
        /// Model constructor
        /// </summary>
        /// <returns></returns>
        public Model()
        {
        }

        public void InitModel()
        {
            // this delegate is needed for the multi media timer defined 
            // in the TimerQueueTimer class.
            _ballTimerCallbackDelegate = new TimerQueueTimer.WaitOrTimerDelegate(BallMMTimerCallback);
            _paddleTimerCallbackDelegate = new TimerQueueTimer.WaitOrTimerDelegate(paddleMMTimerCallback);

            // create our multi-media timers
            _ballHiResTimer = new TimerQueueTimer();
            try
            {
                // create a Multi Media Hi Res timer.
                _ballHiResTimer.Create(2, 2, _ballTimerCallbackDelegate);
            }
            catch (QueueTimerException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Failed to create Ball timer. Error from GetLastError = {0}", ex.Error);
            }

            _paddleHiResTimer = new TimerQueueTimer();
            try
            {
                // create a Multi Media Hi Res timer.
                _paddleHiResTimer.Create(2, 2, _paddleTimerCallbackDelegate);
            }
            catch (QueueTimerException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Failed to create paddle timer. Error from GetLastError = {0}", ex.Error);
            }

            _brickWidth = (_windowWidth - 20) / 15;
            Create_Brick();
            NETTimerTimerStart(true);
        }

        public void CleanUp()
        {
            _ballHiResTimer.Delete();
            _paddleHiResTimer.Delete();
            NETTimerTimerStart(false);
        }


        public void SetStartPosition()
        {

            BallHeight = 30;
            BallWidth = 30;
            paddleWidth = 150;
            paddleHeight = 10;

            Replace_ball();

            paddleCanvasLeft = _windowWidth / 2 - paddleWidth / 2;
            paddleCanvasTop = _windowHeight - paddleHeight;
            _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);

            for (int brick = 0; brick < _numBricks; brick++)
            {
                BrickCollection[brick].BrickVisible = Visibility. Visible;
            }
            Score = 0;
            Time = 0;
            
        }

        public void Replace_ball()
        {
            _moveBall = false;
            ballCanvasLeft = _windowWidth / 2 - BallWidth / 2;
            ballCanvasTop = _windowHeight / 3;
            _ballXMove = (double)_randomNumber.Next(-70, 70) / 50;
            _ballYMove = 1;
        }

        public void Create_Brick()
        {
            BrickCollection = new ObservableCollection<Brick>();
            for (int i = 0; i < _numBricks; i++)
            {
                BrickCollection.Add(new Brick()
                {
                    BrickFill = System.Windows.Media.Brushes.Black,
                    BrickName = "B" + i.ToString(),
                    BrickHeight = _brickHeight,
                    BrickWidth = _brickWidth,
                    BrickCanvasLeft = (10 + i * _brickWidth) % (_windowWidth-20),
                    BrickCanvasTop = ((int)((10 + i * _brickWidth) / (_windowWidth - 20))) * _brickHeight,
                    BrickVisible = System.Windows.Visibility.Visible,
                }); 
            }
            UpdateBrickRects();
        }

        private void UpdateBrickRects()
        {
            for (int brick = 0; brick < _numBricks; brick++)
                BrickCollection[brick].BrickRectangle = new System.Drawing.Rectangle((int)BrickCollection[brick].BrickCanvasLeft,
                    (int)BrickCollection[brick].BrickCanvasTop, (int)BrickCollection[brick].BrickWidth, (int)BrickCollection[brick].BrickHeight);
        }
        enum InterectSide { NONE, LEFT, RIGHT, TOP, BOTTOM };
        private InterectSide IntersectsAt(Rectangle brick, Rectangle ball)
        {
            if (brick.IntersectsWith(ball) == false)
                return InterectSide.NONE;

            Rectangle r = Rectangle.Intersect(brick, ball);

            // did we hit the top of the brick
            if (ball.Top + ball.Height - 1 == r.Top &&
                r.Height == 1)
                return InterectSide.TOP;

            if (ball.Top == r.Top &&
                r.Height == 1)
                return InterectSide.BOTTOM;

            if (ball.Left == r.Left &&
                r.Width == 1)
                return InterectSide.RIGHT;

            if (ball.Left + ball.Width - 1 == r.Left &&
                r.Width == 1)
                return InterectSide.LEFT;

            return InterectSide.NONE;
        }
        public void MoveLeft(bool move)
        {
            _movepaddleLeft = move;
        }

        public void MoveRight(bool move)
        {
            _movepaddleRight = move;
        }

        private void BallMMTimerCallback(IntPtr pWhat, bool success)
        {

            if (!_moveBall)
                return;

            // start executing callback. this ensures we are synched correctly
            // if the form is abruptly closed
            // if this function returns false, we should exit the callback immediately
            // this means we did not get the mutex, and the timer is being deleted.
            if (!_ballHiResTimer.ExecutingCallback())
            {
                Console.WriteLine("Aborting timer callback.");
                return;
            }

            ballCanvasLeft += _ballXMove;
            ballCanvasTop += _ballYMove;

            // check to see if ball has it the left or right side of the drawing element
            if ((ballCanvasLeft + BallWidth >= _windowWidth) ||
                (ballCanvasLeft <= 0))
                _ballXMove = -_ballXMove;


            // check to see if ball has it the top of the drawing element
            if (ballCanvasTop <= 0)
                _ballYMove = -_ballYMove;

            if (ballCanvasTop + BallWidth >= _windowHeight)
            {
                // we hit bottom. stop moving the ball
                _moveBall = false;
            }

            // see if we hit the paddle
            _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)BallWidth, (int)BallHeight);
            if (_ballRectangle.IntersectsWith(_paddleRectangle))
            {
                // hit paddle. reverse direction in Y direction
                _ballYMove = -_ballYMove;

                // move the ball away from the paddle so we don't intersect next time around and
                // get stick in a loop where the ball is bouncing repeatedly on the paddle
                ballCanvasTop += 2 * _ballYMove;

                // add move the ball in X some small random value so that ball is not traveling in the same 
                // pattern
                ballCanvasLeft += _randomNumber.Next(5);
            }

            //see if we hit any of the brick
            check_brick_hit();

            // done in callback. OK to delete timer
            _ballHiResTimer.DoneExecutingCallback();
        }

        private void check_brick_hit()
        {
            for (int brick = 0; brick < _numBricks; brick++)
            {
                if (BrickCollection[brick].BrickVisible == Visibility.Hidden) continue;
                switch (IntersectsAt(BrickCollection[brick].BrickRectangle, _ballRectangle))
                {
                    case InterectSide.NONE: break;
                    case InterectSide.LEFT: BrickCollection[brick].BrickVisible = Visibility.Hidden; _ballXMove = -_ballXMove; Score += 1; return;
                    case InterectSide.RIGHT: BrickCollection[brick].BrickVisible = Visibility.Hidden; _ballXMove = -_ballXMove; Score += 1; return;
                    case InterectSide.TOP: BrickCollection[brick].BrickVisible = Visibility.Hidden; _ballYMove = -_ballYMove; Score += 1; return;
                    case InterectSide.BOTTOM: BrickCollection[brick].BrickVisible = Visibility.Hidden; _ballYMove = -_ballYMove; Score += 1; return;
                }
            }
        }

        private void paddleMMTimerCallback(IntPtr pWhat, bool success)
        {

            // start executing callback. this ensures we are synched correctly
            // if the form is abruptly closed
            // if this function returns false, we should exit the callback immediately
            // this means we did not get the mutex, and the timer is being deleted.
            if (!_paddleHiResTimer.ExecutingCallback())
            {
                Console.WriteLine("Aborting timer callback.");
                return;
            }

            if (_movepaddleLeft && paddleCanvasLeft > 0)
                paddleCanvasLeft -= 2;
            else if (_movepaddleRight && paddleCanvasLeft < _windowWidth - paddleWidth)
                paddleCanvasLeft += 2;

            _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);


            // done in callback. OK to delete timer
            _paddleHiResTimer.DoneExecutingCallback();
        }


        #region .NET Timer Timer
        bool _netTimerTimerRunning = false;
        // used for measuring the period of the .NET timer timer
        uint NETTimerTimerTicks = 0;
        long NETTimerTimerTotalTime = 0;
        long NETTimerTimerPreviousTime;

        System.Timers.Timer dotNetTimerTimer;

        private uint _time = 0;
        public uint Time
        {
            get { return _time; }
            set { _time = value; OnPropertyChanged("Time"); }
        }
        public bool NETTimerTimerStart(bool startStop)
        {
            if (startStop == true)
            {
                dotNetTimerTimer = new System.Timers.Timer(1000); // hard coded 1 second in this timer
                dotNetTimerTimer.Elapsed += new ElapsedEventHandler(NetTimerTimerHandler);
                dotNetTimerTimer.Start();

            }
            else if (_netTimerTimerRunning)
            {
                dotNetTimerTimer.Stop();
            }

            return true;
        }

        private void NetTimerTimerHandler(object source, ElapsedEventArgs e)
        {
            if(MoveBall)Time++;
        }

        #endregion

    }
}
