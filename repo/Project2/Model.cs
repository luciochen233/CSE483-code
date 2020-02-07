using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Project2
{
    class Model : INotifyPropertyChanged
    {
        public Model()
        {
            LeftNum = "Left";
            RightNum = "Right";
            Operation = "Operation";
            Result = "Result";

        }

        public void Clear()
        {
            LeftNum = " ";
            RightNum = " ";
            Operation = " ";
            Result = " ";
            ErrMsg = "All clear";
        }
        private double GetResult(double x, double y, string op)
        {
            switch (op)
            {
                case "times": return x * y;
                case "minus": return x - y;
                case "divide": return (x / y);
                case "plus": return x + y;
            }
            return 0.0;
        }
        public void Calculate()
        {
            double firstNum = 0;
            double secondNum = 0;
            try
            {
                firstNum = double.Parse(LeftNum);
                secondNum = double.Parse(RightNum);
                double temp = GetResult(firstNum, secondNum, Operation);
                Result= temp.ToString();
                ErrMsg = "";
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
            }
        }

        private string _leftNum;
        public string LeftNum
        {
            get { return _leftNum; }
            set
            {
                _leftNum = value;
                OnPropertyChanged("LeftNum");
            }
        }

        private string _rightNum;
        public string RightNum
        {
            get { return _rightNum; }
            set
            {
                _rightNum = value;
                OnPropertyChanged("RightNum");
            }
        }

        private string _result;
        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged("Result");
            }
        }

        private string _errMsg;
        public string ErrMsg
        {
            get { return _errMsg; }
            set
            {
                _errMsg = value;
                OnPropertyChanged("ErrMsg");
            }
        }

        private string _operation;
        public string Operation
        {
            get { return _operation; }
            set
            {
                _operation = value;
                OnPropertyChanged("Operation");
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
