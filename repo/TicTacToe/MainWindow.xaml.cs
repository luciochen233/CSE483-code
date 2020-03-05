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

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model _model;
        public MainWindow()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            _model = new Model();
            this.DataContext = _model;
            MyItemsControl.ItemsSource = _model.TileCollection;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // one of the buttons in our collection. need to figure out
            // which one. Since we know the button is part of a collection, we 
            // have a special way that we need to get at its bame

            var selectedButton = e.OriginalSource as FrameworkElement;
            if (selectedButton != null)
            {
                // get the currently selected item in the collection
                // which we know to be a Tile object
                // Tile has a TileName (refer to Tile.cs)
                var currentTile = selectedButton.DataContext as Tile;
                if (_model.UserSelection(currentTile.TileName))
                {
                    _model.Switch_side();
                    _model.Win();
                }
            }
            
        }

        private void Breset_Click(object sender, RoutedEventArgs e)
        {
            _model.Clear();
            _model.StatusText = "RESET";
        }

        //private void Side_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    var a = e.NewValue;
        //    _model.Switch_side(a);
        //}
    }
}
