using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// INotifyPropertyChanged
using System.ComponentModel;


namespace SampleUDPPeer
{
    partial class Model : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private String _meBox;
        public String MeBox
        {
            get { return _meBox; }
            set
            {
                _meBox = value;
                OnPropertyChanged("MeBox");
            }
        }

        private String _myFriendBox;
        public String MyFriendBox
        {
            get { return _myFriendBox; }
            set
            {
                _myFriendBox = value;
                OnPropertyChanged("MyFriendBox");
            }
        }

        private String _statusTextBox;
        public String StatusTextBox
        {
            get { return _statusTextBox; }
            set
            {
                _statusTextBox = value;
                OnPropertyChanged("StatusTextBox");
            }
        }

        private String _helpText;
        public String HelpText
        {
            get { return _helpText; }
            set
            {
                _helpText = value;
                OnPropertyChanged("HelpText");
            }
        }

        private String _data1;
        public String Data1
        {
            get { return _data1; }
            set
            {
                _data1 = value;
                OnPropertyChanged("Data1");
            }
        }

        private String _data2;
        public String Data2
        {
            get { return _data2; }
            set
            {
                _data2 = value;
                OnPropertyChanged("Data2");
            }
        }

        private bool _sendEnabled;
        public bool SendEnabled
        {
            get { return _sendEnabled; }
            set
            {
                _sendEnabled = value;
                OnPropertyChanged("SendEnabled");
            }
        }

    }
}
