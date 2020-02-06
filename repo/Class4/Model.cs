using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Class4
{
    class Model : INotifyPropertyChanged
    {
        public Model()
        {
            TopBoxText = " lucio ";
            BotBoxText = " ";
        }

        public void CopyText()
        {
            BotBoxText = TopBoxText;
        }

        private string _topBoxText;
        public string TopBoxText
        {
            get { return _topBoxText; }
            set
            {
                _topBoxText = value;
                OnPropertyChanged("TopBoxText");
            }
        }

        private string _botBoxText;
        public string BotBoxText
        {
            get { return _topBoxText; }
            set
            {
                _botBoxText = value;
                OnPropertyChanged("BotBoxText");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
