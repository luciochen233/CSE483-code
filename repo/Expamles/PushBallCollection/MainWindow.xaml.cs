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

namespace PushBallCollection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model _model;
        private bool _leftMouseDown = false;


        public MainWindow()
        {
            InitializeComponent();

            // make it so the user cannot resize the window
            this.ResizeMode = ResizeMode.NoResize;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // create an instance of our Model
            _model = new Model();
            _model.WindowHeight = BallCanvas.RenderSize.Height;
            _model.WindowWidth = BallCanvas.RenderSize.Width;
            this.DataContext = _model;

            _model.InitModel();
            _model.SetStartPosition();


            // create an observable collection. this collection
            // contains the tiles the represent the Tic Tac Toe grid
            BrickItems.ItemsSource = _model.BrickCollection;


        }

        private void KeypadDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                _model.MoveLeft(true);
            else if (e.Key == Key.Right)
                _model.MoveRight(true);
            else if (e.Key == Key.R)
                _model.SetStartPosition();
        }

        private void KeypadUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                _model.MoveLeft(false);
            else if (e.Key == Key.Right)
                _model.MoveRight(false);
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _model.CleanUp();
        }

        private void BallCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_leftMouseDown)
            {
                Point p = e.GetPosition(this);
                _model.ProcessMouseDrag((uint)p.X, (uint)p.Y);
            }
        }

        private void BallCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this);
            _model.ProcessMouseClick((uint)p.X, (uint)p.Y);

        }

        private void TheBall_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _leftMouseDown = true;
        }

        private void TheBall_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _leftMouseDown = false;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // one of the buttons in our collection. need to figure out
            // which one. Since we know the button is part of a collection, we 
            // have a special way that we need to get at its bame

            var selectedButton = e.OriginalSource as FrameworkElement;
            if (selectedButton != null)
            {
                var currentTile = selectedButton.DataContext as Brick;
                _model.ToggleBrickColor(currentTile.BrickName);
            }
        }

    }
}
