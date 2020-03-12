/********************************************************************
	created:	2014/02/22
	created:	22:2:2014   12:26
	filename: 	R:\CSE483\VCStudio\WindowsProgramming\WPF Samples\BrickBreaker\BallPaddle.cs
	file path:	R:\CSE483\VCStudio\WindowsProgramming\WPF Samples\BrickBreaker
	file base:	BallPaddle
	file ext:	cs
	author:		Joe Waclawski
	
	purpose:	this file contains the bound properties associated with the
                ball and paddle in the Brick Breaker game
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// INotifyPropertyChanged
using System.ComponentModel;

// Brushes
using System.Windows.Media;


namespace SampleThread
{
    public partial class Model : INotifyPropertyChanged
    {

        // required for property data binding
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private String _threadAData;
        public String ThreadAData
        {
            get { return _threadAData; }
            set
            {
                _threadAData = value;
                OnPropertyChanged("ThreadAData");
            }
        }

        private String _threadBData;
        public String ThreadBData
        {
            get { return _threadBData; }
            set
            {
                _threadBData = value;
                OnPropertyChanged("ThreadBData");
            }
        } 
    }
}
