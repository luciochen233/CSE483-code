using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// INotifyPropertyChanged
using System.ComponentModel;

namespace TimerDemo
{
    partial class Model : INotifyPropertyChanged
    {
        private UInt32 _mmTimerPeriod;
        public UInt32 MMTimerPeriod
        {
            get { return _mmTimerPeriod; }
            set
            {
                _mmTimerPeriod = value;
                OnPropertyChanged("MMTimerPeriod");
            }
        }

        private float _mmTimerAverage;
        public float MMTimerAverage
        {
            get { return _mmTimerAverage; }
            set
            {
                _mmTimerAverage = value;
                OnPropertyChanged("MMTimerAverage");
            }
        }

        private UInt32 _netDispatchTimerPeriod;
        public UInt32 NETDispatchTimerPeriod
        {
            get { return _netDispatchTimerPeriod; }
            set
            {
                _netDispatchTimerPeriod = value;
                OnPropertyChanged("NETDispatchTimerPeriod");
            }
        }

        private float _netDispatchTimerAverage;
        public float NETDispatchTimerAverage
        {
            get { return _netDispatchTimerAverage; }
            set
            {
                _netDispatchTimerAverage = value;
                OnPropertyChanged("NETDispatchTimerAverage");
            }
        }


        private UInt32 _netTimerTimerPeriod;
        public UInt32 NETTimerTimerPeriod
        {
            get { return _netTimerTimerPeriod; }
            set
            {
                _netTimerTimerPeriod = value;
                OnPropertyChanged("NETTimerTimerPeriod");
            }
        }

        private float _netTimerTimerAverage;
        public float NETTimerTimerAverage
        {
            get { return _netTimerTimerAverage; }
            set
            {
                _netTimerTimerAverage = value;
                OnPropertyChanged("NETTimerTimerAverage");
            }
        }


        private UInt32 _netThreadSleepPeriod;
        public UInt32 NETThreadSleepPeriod
        {
            get { return _netThreadSleepPeriod; }
            set
            {
                _netThreadSleepPeriod = value;
                OnPropertyChanged("NETThreadSleepPeriod");
            }
        }

        private float _netThreadSleepAverage;
        public float NETThreadSleepAverage
        {
            get { return _netThreadSleepAverage; }
            set
            {
                _netThreadSleepAverage = value;
                OnPropertyChanged("NETThreadSleepAverage");
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
