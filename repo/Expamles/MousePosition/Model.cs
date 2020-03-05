using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// INotifyPropertyChanged
using System.ComponentModel;


namespace MousePosition
{
    class Model : INotifyPropertyChanged
    {
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
