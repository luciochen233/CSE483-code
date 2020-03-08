using System;
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

// Rectangle
// Must update References manually
using System.Drawing;

// INotifyPropertyChanged
using System.ComponentModel;

// Threading.Timer
using System.Windows.Threading;

// Timer.Timer
using System.Timers;

namespace PushBallCollection
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

        Random _randomNumber = new Random();
        System.Drawing.Rectangle _paddleRectangle;
        System.Drawing.Rectangle _ballRectangle;
        bool _movepaddleLeft = false;
        bool _movepaddleRight = false;
        uint _pushMove = 20;
        System.Windows.Media.Brush FillColorRed;
        System.Windows.Media.Brush FillColorBlue;

        public ObservableCollection<Brick> BrickCollection;
        int _numBricks = 4;
        Rectangle[] _brickRectangles = new Rectangle[4];
        // note that the brick hight, number of brick columns and rows
        // must match our window demensions.
        double _brickHeight = 50;
        double _brickWidth = 120;




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

        /// <summary>
        /// Model constructor
        /// </summary>
        /// <returns></returns>
        public Model()
        {
            SolidColorBrush mySolidColorBrushRed = new SolidColorBrush();
            SolidColorBrush mySolidColorBrushBlue = new SolidColorBrush();

            // Describes the brush's color using RGB values. 
            // Each value has a range of 0-255.

            mySolidColorBrushRed.Color = System.Windows.Media.Color.FromRgb(255, 0, 0);
            FillColorRed = mySolidColorBrushRed;
            mySolidColorBrushBlue.Color = System.Windows.Media.Color.FromRgb(0, 0, 255);
            FillColorBlue = mySolidColorBrushBlue;
        }

        public void InitModel()
        {
            // create brick collection
            // place them manually at the top of the item collection in the view
            BrickCollection = new ObservableCollection<Brick>();
            for (int brick = 0; brick < _numBricks; brick++)
            {
                BrickCollection.Add(new Brick()
                {
                    BrickFill = FillColorRed,
                    BrickHeight = _brickHeight,
                    BrickWidth = _brickWidth,
                    BrickVisible = System.Windows.Visibility.Visible,
                    BrickName = brick.ToString(),
                });

                BrickCollection[brick].BrickCanvasLeft = _windowWidth / 2 - _brickWidth / 2;
                BrickCollection[brick].BrickCanvasTop = brick * _brickHeight + 150; // offset the bricks from the top of the screen by a bitg
            }

            UpdateRects();
        }

        public void CleanUp()
        {
        }

        public void ToggleBrickColor(String name)
        {
            int index = int.Parse(name);
            if (BrickCollection[index].BrickFill == FillColorBlue)
            {
                BrickCollection[index].BrickFill = FillColorRed;
                return;
            }

            if (BrickCollection[index].BrickFill == FillColorRed)
            {
                BrickCollection[index].BrickFill = FillColorBlue;
                return;
            }
        }

        public void SetStartPosition()
        {
            ballWidth = 50;
            ballHeight = 50;

            ballCanvasLeft = 100;
            ballCanvasTop = 100;

            UpdateRects();
        }

        public void ProcessMouseClick(uint x, uint y)
        {
            ballCanvasLeft = x - ballWidth / 2;
            ballCanvasTop = y - ballHeight / 2;
            UpdateRects();
        }

        public void ProcessMouseDrag(uint x, uint y)
        {
            System.Windows.Point p = new System.Windows.Point();
            p.X = (double)x;
            p.Y = (double)y;
            ballCanvasLeft = x - ballWidth / 2;
            ballCanvasTop = y - ballHeight / 2;
            CheckPush();
            UpdateRects();
        }

        private void UpdateRects()
        {
            _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)ballWidth, (int)ballHeight);
            for (int brick=0; brick<_numBricks; brick++)
                BrickCollection[brick].BrickRectangle = new System.Drawing.Rectangle((int)BrickCollection[brick].BrickCanvasLeft, 
                    (int)BrickCollection[brick].BrickCanvasTop, (int)BrickCollection[brick].BrickWidth, (int)BrickCollection[brick].BrickHeight);
        }

        private void CheckPush()
        {
            for (int brick=0; brick < _numBricks; brick++)
            {
                if (BrickCollection[brick].BrickFill != FillColorRed) continue;

                InterectSide whichSide = IntersectsAt(BrickCollection[brick].BrickRectangle, _ballRectangle);
                switch (whichSide)
                {
                    case InterectSide.NONE:
                        break;

                    case InterectSide.TOP:
                        BrickCollection[brick].BrickCanvasTop += _pushMove;
                        break;

                    case InterectSide.BOTTOM:
                        BrickCollection[brick].BrickCanvasTop -= _pushMove;
                        break;

                    case InterectSide.LEFT:
                        BrickCollection[brick].BrickCanvasLeft += _pushMove;
                        break;

                    case InterectSide.RIGHT:
                        BrickCollection[brick].BrickCanvasLeft -= _pushMove;
                        break;
                }
            }
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
    }
}
