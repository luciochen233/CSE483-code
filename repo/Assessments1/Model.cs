using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using MyIntegerSet;
using System.Windows.Media;

namespace Assessments1
{
    class Model : INotifyPropertyChanged
    {
        IntegerSet _s1 = new IntegerSet();
        IntegerSet _s2 = new IntegerSet(); //two integer sets for each box
        public Model()
        {
            In1Text = "1,2,3,4,5,6";
            In2Text = "4,5,6,7,8,9,10";
            UnionText = "Union";
            InterText = "Intersection";
            ErrColor = Brushes.Black;
            ErrText = "Error message goes here"; //initialize all element

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

        private System.Windows.Media.Brush _errColor;

        public System.Windows.Media.Brush ErrColor
        {
            get { return _errColor; }
            set
            {
                _errColor = value;
                OnPropertyChanged("ErrColor");
            }
        }

        #endregion
        public void Convert() // this function will be called when the compute button is pressed. 
        {
            In1Text = In1Text.TrimEnd(',',' ');
            In2Text = In2Text.TrimEnd(',',' ');
            string[] arr1 = In1Text.Split(',','，');
            string[] arr2 = In2Text.Split(',','，'); //seperate words into arrays
            _s1.Clear();
            _s2.Clear(); //make sure s1 and s2 are clear before use. 
            ErrText = ""; //clear error message
            ErrColor = Brushes.Black;
            if (In1Text == "")
            {
                ErrText += "Set 1 is empty\n";
            }
            else
            {
                foreach (string i in arr1)
                {
                    try
                    {
                        //if (i == "") continue;
                        uint j = uint.Parse(i);
                        if (j > 100)
                        {
                            throw new Exception("Number Exceed max value 100!");
                        }
                        _s1.InsertElement(j);
                    }
                    catch (Exception e)
                    {
                        ErrColor = Brushes.Red;
                        ErrText += e.Message;
                        _s1.Clear();
                        UnionText = "";
                        InterText = "";
                        return;
                    }
                }
            }

            if (In2Text == "")
            {
                ErrText += "Set 2 is empty\n";
            }
            else
            {
                foreach (string i in arr2)
                {
                    try
                    {
                        //if (i == "") continue;
                        uint j = uint.Parse(i);
                        if (j > 100)
                        {
                            throw new Exception("Number Exceed max value 100!");
                        }
                        _s2.InsertElement(j);
                    }
                    catch (Exception e)
                    {
                        ErrColor = Brushes.Red;
                        ErrText += e.Message;
                        _s2.Clear();
                        UnionText = "";
                        InterText = "";
                        return;
                    }
                }
            }
            
            IntegerSet inter = _s1.Intersection(_s2);
            IntegerSet uni   = _s1.Union(_s2);
            UnionText = uni.ToString();
            InterText = inter.ToString();
            ErrText += "Here is the result\n";
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
