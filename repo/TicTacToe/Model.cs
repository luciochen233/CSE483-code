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
    partial class Model : INotifyPropertyChanged
    {
        public ObservableCollection<Tile> TileCollection;
        private static UInt32 _numTiles = 9;
        private UInt32[] _buttonPresses = new UInt32[_numTiles];
        Random _randomNumber = new Random();

        private Ticlogic tic = new Ticlogic();
        

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

        private bool Cheat_flag = true;
        private int _side = 0;
        private double _slide = 0.0;
        public double Slide
        {
            get { return _slide; }
            set
            {
                if (!Cheat_flag)
                {
                    _slide = value;
                    _side = (int)(Slide);
                }
                else
                {
                    StatusText = "DON'T CHEAT";
                }

                Cheat_flag = true;
                OnPropertyChanged("Slide");
            }
        }

        public Model()
        {
            TileCollection = new ObservableCollection<Tile>();
            tic.Clear();
            Cheat_flag = false;
            Slide = 0;
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

        public void Clear()
        {
            tic.Clear();
            Cheat_flag = false;
            Slide = 0;
            foreach (var t in TileCollection)
            {
                t.TileLabel = " ";
            }
        }

        public void Switch_side()
        {
            Cheat_flag = false;
            Slide = (Slide == 0.0) ? 1 : 0;
        }

        public bool UserSelection(String buttonSelected)
        {
            Debug.Write("Button selected was " + buttonSelected + "\n");
            char[] _player = {'X', 'O'};
            Brush[] _ox_brush = {new SolidColorBrush(Colors.Red), new SolidColorBrush(Colors.Blue)};

            int index = int.Parse(buttonSelected);
            int _row = index / 3;
            int _col = index % 3;
            if (tic.Mark(_player[_side], _row, _col))
            {
                TileCollection[index].TileLabel = _player[_side].ToString();

                //TileCollection[index].TileBrush = new SolidColorBrush(Color.FromArgb(255, (byte)_randomNumber.Next(255), (byte)_randomNumber.Next(255), (byte)_randomNumber.Next(255)));
                TileCollection[index].TileBrush = _ox_brush[_side];
                //StatusText = "User Selected Button " + index.ToString() + "\n";
                StatusText = " ";
                return true;
            }
            else
            {
                StatusText = "Try Again";
            }

            //_buttonPresses[index]++;
            //TileCollection[index].TileLabel = _buttonPresses[index].ToString();

            return false;
        }

        public void Mark_Winner(int winner)
        {
            if (winner < 0 || winner > 7) return;
            Brush _win_color = new SolidColorBrush(Colors.Gold);
            if(winner <3)
            {
                TileCollection[0 + winner*3].TileBrush = _win_color;
                TileCollection[1 + winner*3].TileBrush = _win_color;
                TileCollection[2 + winner*3].TileBrush = _win_color;
                return;
            }
            if(winner < 6)
            {
                winner -= 3;
                TileCollection[0 + winner].TileBrush = _win_color;
                TileCollection[3 + winner].TileBrush = _win_color;
                TileCollection[6 + winner].TileBrush = _win_color;
                return;
            }
            if(winner == 6)
            {
                TileCollection[0].TileBrush = _win_color;
                TileCollection[4].TileBrush = _win_color;
                TileCollection[8].TileBrush = _win_color;
                return;
            }
            if(winner == 7)
            {
                TileCollection[2].TileBrush = _win_color;
                TileCollection[4].TileBrush = _win_color;
                TileCollection[6].TileBrush = _win_color;
                return;
            }
        }

        public char Win()
        {
            char __temp = tic.CheckWin();
            int __which_row = -1;
            if(__temp >= 'O' && __temp < 'X')  //black magic
            {
                __which_row = __temp - 'O';
                __temp = 'O';
            } else if(__temp >= 'X' && __temp < '`')
            {
                __which_row = __temp - 'X';
                __temp = 'X';
            }
            if(__which_row >=0)
            {
                Console.WriteLine(__which_row);
                Mark_Winner(__which_row);
            }
            

            switch (__temp)
            {
                case ' ': break;
                case 'D': StatusText = "DRAW"; break;
                case 'O': StatusText = "O WINS"; break;
                case 'X': StatusText = "X WINS"; break;
            }

            return __temp;
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
