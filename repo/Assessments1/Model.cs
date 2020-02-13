using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Assessments1
{
    class Model : INotifyPropertyChanged
    {
        public Model()
        {
            In1Text = " lucio ";
            In2Text = " hello ";
            UnionText = "Union";
            InterText = "Intersection";
            ErrText = "Error Message";

        }

        private string _in1;
        public string In1Text
        {
            get { return _in1; }
            set
            {
                _in1 = value;
                OnPropertyChanged("TopBoxText");
            }
        }

        private string _in2;
        public string In2Text
        {
            get { return _in2; }
            set
            {
                _in2 = value;
                OnPropertyChanged("BotBoxText");
            }
        }

        private string _union;
        public string UnionText
        {
            get { return _union; }
            set
            {
                _union = value;
                OnPropertyChanged("UnionText");
            }
        }

        private string _inter;
        public string InterText
        {
            get { return _inter; }
            set
            {
                _inter = value;
                OnPropertyChanged("InterText");
            }
        }

        private string _err;
        public string ErrText
        {
            get { return _err; }
            set
            {
                _err = value;
                OnPropertyChanged("ErrText");
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
