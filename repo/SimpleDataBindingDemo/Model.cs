using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// added for INotifyPropertyChanged
using System.ComponentModel;

// for sleep
// using System.Threading;

// for Brush
using System.Windows.Media;



namespace SimpleDataBindingDemo
{
    class Model : INotifyPropertyChanged
    {
        private String _topTextBoxText;
        public String TopTextBoxText
        {
            get { return _topTextBoxText; }
            set
            {
                _topTextBoxText = value;

                try
                {
                    if (_topTextBoxText.Contains("Red"))
                        TopBoxBackground = Brushes.Red;
                    else if (_topTextBoxText.Contains("Green"))
                        TopBoxBackground = Brushes.Lime;
                    else if (_topTextBoxText.Contains("Blue"))
                        TopBoxBackground = Brushes.Blue;
                }
                catch { }

                OnPropertyChanged("TopTextBoxText");
            }
        }

        private Brush _topBoxBackground;
        public Brush TopBoxBackground
        {
            get { return _topBoxBackground; }
            set
            {
                _topBoxBackground = value;
                OnPropertyChanged("TopBoxBackground");
            }
        }

        private String _bottomTextBoxText;
        public String BottomTextBoxText
        {
            get { return _bottomTextBoxText; }
            set
            {
                _bottomTextBoxText = value;

                try
                {
                    if (_bottomTextBoxText.Contains("Red"))
                        BottomBoxBackground = Brushes.Red;
                    else if (_bottomTextBoxText.Contains("Green"))
                        BottomBoxBackground = Brushes.Lime;
                    else if (_bottomTextBoxText.Contains("Blue"))
                        BottomBoxBackground = Brushes.Blue;
                }
                catch { }

                OnPropertyChanged("BottomTextBoxText");
            }
        }

        private Brush _bottomBoxBackground;
        public Brush BottomBoxBackground
        {
            get { return _bottomBoxBackground; }
            set
            {
                _bottomBoxBackground = value;
                OnPropertyChanged("BottomBoxBackground");
            }
        }

        public void SetColor(MY_COLOR clr)
        {
            switch(clr)
            {
                case MY_COLOR.RED:
                    TopBoxBackground = Brushes.Red;
                    BottomBoxBackground = Brushes.Red;
                    break;

                case MY_COLOR.GREEN:
                    TopBoxBackground = Brushes.Lime;
                    BottomBoxBackground = Brushes.Lime;
                    break;

                case MY_COLOR.BLUE:
                    TopBoxBackground = Brushes.Blue;
                    BottomBoxBackground = Brushes.Blue;
                    break;

                default:
                    TopBoxBackground = Brushes.White;
                    BottomBoxBackground = Brushes.White;
                    break;
            }
        }

        public void Update()
        {
            String temp = BottomTextBoxText;
            BottomTextBoxText = TopTextBoxText;
            TopTextBoxText = temp;
        }


        #region Data Binding Stuff
        // define our property chage event handler, part of data binding
        public event PropertyChangedEventHandler PropertyChanged;

        // implements method for data binding to any and all properties
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        #endregion

    }
}
