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


// debug output
using System.Diagnostics;

namespace ObservableCollectionsDemo
{
    class Model : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableCollection<Tile> TileCollection;
        private static UInt32 _numTiles = 12;
        private UInt32[] _buttonPresses = new UInt32[_numTiles];
        Random _randomNumber = new Random();

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


        /// <summary>
        /// Model constructor
        /// </summary>
        /// <returns></returns>
        public Model()
        {
            TileCollection = new ObservableCollection<Tile>();
            for (int i = 0; i < _numTiles; i++)
            {
                TileCollection.Add(new Tile()
                {
                    TileBrush = Brushes.Black,
                    TileLabel = "",
                    TileName = i.ToString(),
                    TileBackground = Brushes.LightGray
                });
            }
        }

        /// <summary>
        /// processes all buttons. called from view when a button is clicked
        /// </summary>
        /// <param name="buttonSelected"></param>
        /// <returns></returns>
        public void UserSelection(String buttonSelected)
        {
            Debug.Write("Button selected was " + buttonSelected + "\n");

            int index = int.Parse(buttonSelected);
            _buttonPresses[index]++;
            TileCollection[index].TileLabel = _buttonPresses[index].ToString();
            TileCollection[index].TileBrush = new SolidColorBrush(Color.FromArgb(255, (byte)_randomNumber.Next(255), (byte)_randomNumber.Next(255), (byte)_randomNumber.Next(255)));
            StatusText = "User Selected Button " + index.ToString() + "\n";
        }

       
        /// <summary>
        /// resets all buttons back to their starting point
        /// </summary>
        /// <param name></param>
        /// <returns></returns>
        public void Clear()
        {
            for (int x = 0; x < _numTiles; x++)
            {
                TileCollection[x].TileBrush = Brushes.Black;
                TileCollection[x].TileLabel = "";
                _buttonPresses[x] = 0;
            }

            StatusText = "";
        }
    }
}
