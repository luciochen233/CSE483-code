using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Brushes
using System.Windows.Media;

// observable collections
using System.Collections.ObjectModel;

// INotifyPropertyChanged
using System.ComponentModel;

using System.Diagnostics;

namespace TicTacToe
{
    class Model : INotifyPropertyChanged
    {
        public ObservableCollection<Tile> TileCollection;
        private static UInt32 _numTiles = 9;
        private UInt32[] _buttonPresses = new UInt32[_numTiles];
        Random _randomNumber = new Random();

        Ticlogic tic = new Ticlogic();

        private String _statusText = "";
        public String StatusText

        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                OnPropertyChanged("StatusText");
            }
        }

        public Model()
        {
            TileCollection = new ObservableCollection<Tile>();
            tic.Clear();
            for (int i = 0; i < _numTiles; i++)
            {
                TileCollection.Add(new Tile()
                {
                    TileBrush = Brushes.Transparent,
                    TileLabel = "",
                    TileName = i.ToString(),
                    TileBackground = Brushes.Bisque
                }) ;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void UserSelection(String buttonSelected)
        {
            Debug.Write("Button selected was " + buttonSelected + "\n");
            char[] _player = {'X', 'O'};

            int index = int.Parse(buttonSelected);
            int _row = index / 3;
            int _col = index % 3;
            tic.Mark(_player[0], _row, _col);
            TileCollection[index].TileLabel = _player[0].ToString();
            //_buttonPresses[index]++;
            //TileCollection[index].TileLabel = _buttonPresses[index].ToString();
            TileCollection[index].TileBrush = new SolidColorBrush(Color.FromArgb(255, (byte)_randomNumber.Next(255), (byte)_randomNumber.Next(255), (byte)_randomNumber.Next(255)));
            StatusText = "User Selected Button " + index.ToString() + "\n";
        }
    }
}
