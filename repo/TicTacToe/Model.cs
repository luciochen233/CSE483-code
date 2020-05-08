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
using System.Threading;
using System.Diagnostics.Eventing.Reader;

namespace TicTacToe
{
    partial class Model : INotifyPropertyChanged
    {
        public ObservableCollection<Tile> TileCollection;
        private static UInt32 _numTiles = 9;
        //private UInt32[] _buttonPresses = new UInt32[_numTiles];
        //Random _randomNumber = new Random();
        //public event SimpleEventHandler UpdateTileEvent;

        private Ticlogic tic = new Ticlogic();
        private bool online = false;
        private bool no_update = false;
        private int all_winner = -1;
        int current_selection = -1;
        

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
        private bool Need_update = false;
        public double Slide
        {
            get { return _slide; }
            set
            {
                if (!Cheat_flag)
                {
                    _slide = value;
                    _side = (int)(Slide);
                    //Update_Tile();
                } else {
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
            UpdateHandler = UpdateEvent;
            UpdateHandler += IncomingUpdate;
            Update_Tile();
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
            current_selection = -1;
            no_update = false;
            all_winner = -1;
            StatusText = "RESET";
            if (online) SendMessage();
            
        }

        public void Switch_side()
        {
            Cheat_flag = false;
            Slide = (Slide == 0.0) ? 1 : 0;
        }

        public bool UserSelection(String buttonSelected)
        {
            StatusText = " ";
            Debug.Write("Button selected was " + buttonSelected + "\n");
            char[] _player = {'X', 'O'};
            Brush[] _ox_brush = {new SolidColorBrush(Colors.Red), new SolidColorBrush(Colors.Blue)};

            int index = int.Parse(buttonSelected);
            if (online && current_selection != -1)
            {
                StatusText = "Waiting for Opponent";
                return false;
            }


            current_selection = index;

            

            int _row = index / 3;
            int _col = index % 3;
            if (tic.Mark(_player[_side], _row, _col))
            {
                //Update_Tile();
                if (UpdateHandler != null)
                {
                    UpdateHandler(this, new SimpleEventArgs(tic));
                }
                Need_update = true;
                if(online) SendMessage();
                return true;
            }
            else
            {
                StatusText = "Try Again";
                current_selection = -1;
            }

            //_buttonPresses[index]++;
            //TileCollection[index].TileLabel = _buttonPresses[index].ToString();
            //if (online) SendMessage();
            return false;
        }

        public void Update_Tile()
        {
            
            char[] _player = { 'X', 'O' };
            Brush[] _ox_brush = { new SolidColorBrush(Colors.Red), new SolidColorBrush(Colors.Blue) };
            
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    int index = row * 3 + col;
                    char tile_msg = tic._check[row, col];
                    TileCollection[index].TileLabel = tile_msg.ToString();

                    if (tile_msg == 'X')
                    {
                        TileCollection[index].TileBrush = _ox_brush[0];
                    }
                    else if (tile_msg == 'O')
                    {
                        TileCollection[index].TileBrush = _ox_brush[1];
                    }
                }
            }
            if (no_update && all_winner != -1)
            {
                Mark_Winner(all_winner);
            }
        }

        public void Mark_Winner(int winner)
        {
            if (winner < 0 || winner > 7) return;
            no_update = true;
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
                all_winner = __which_row;
            }
            

            switch (__temp)
            {
                case ' ': break;
                case 'D': StatusText = "DRAW"; break;
                case 'O': StatusText = "O WINS"; break;
                case 'X': StatusText = "X WINS"; break;
            }

            if (online) SendMessage();
            return __temp;
        }

        public delegate void UpdateTileHandler(object sender, SimpleEventArgs e);
        public event UpdateTileHandler UpdateEvent;
        public UpdateTileHandler UpdateHandler;
        public void IncomingUpdate(object sender, SimpleEventArgs e)
        {
            Update_Tile();
        }

        //public void SendUpdate()
        //{
        //    if(UpdateHandler != null)
        //    {
        //        UpdateHandler(this, SimpleEventArgs(tic));
        //    }
        //}

        public class SimpleEventArgs : EventArgs
        {
            Ticlogic tic_temp;
            public SimpleEventArgs(Ticlogic t)
            {
                tic_temp = t;
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


    }
}
