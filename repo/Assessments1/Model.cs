using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using MyIntegerSet;

namespace Assessments1
{
    class Model : INotifyPropertyChanged
    {
        IntegerSet _s1 = new IntegerSet();
        IntegerSet _s2 = new IntegerSet();
        public Model()
        {
            In1Text = "1,2,3,4,5,6";
            In2Text = "4,5,6,7,8,9,10";
            UnionText = "Union";
            InterText = "Intersection";
            ErrText = "Error message goes here";

        }
        #region Getter ans Setter
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
        #endregion
        public void Convert()
        {
            string[] arr1 = In1Text.Split(',');
            string[] arr2 = In2Text.Split(',');
            _s1.Clear();
            _s2.Clear();
            ErrText = "";
            foreach (string i in arr1)
            {
                try
                {
                    if (i == "") continue;
                    uint j = uint.Parse(i);
                    if(j > 100)
                    {
                        throw new Exception("Number Exceed max value!");
                    }
                    _s1.InsertElement(j);
                }
                catch(Exception e)
                {
                    ErrText += e.Message;
                    _s1.Clear();
                    UnionText = "";
                    InterText = "";
                    return;
                }
            }

            foreach (string i in arr2)
            {
                try
                {
                    if (i == "") continue;
                    uint j = uint.Parse(i);
                    if (j > 100)
                    {
                        throw new Exception("Number Exceed max value!");
                    }
                    _s2.InsertElement(j);
                }
                catch (Exception e)
                {
                    ErrText += e.Message;
                    _s2.Clear();
                    UnionText = "";
                    InterText = "";
                    return;
                }
            }
            
            IntegerSet inter = _s1.Intersection(_s2);
            IntegerSet uni   = _s1.Union(_s2);
            UnionText = uni.ToString();
            InterText = inter.ToString();
        }

        #region Data Binding Stuff
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
