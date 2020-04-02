using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

// Sockets
using System.Net.Sockets;
using System.Net;

// Threads
using System.Threading;

namespace UDPGUIServer
{
    class Model : INotifyPropertyChanged
    {
        // some data that keeps track of ports and addresses
        private static int _localPort = 5000;
        private static string _localIPAddress = "127.0.0.1";

        // this is the thread that will run in the background
        // waiting for incomming data
        private static Thread _receiveDataThread;

        // this is the UDP socket that will be used to communicate
        // over the network
        private static UdpClient _dataSocket;

        public Model()
        {
            _dataSocket = new UdpClient(_localPort);
            StartThread();
            StatusBox = "UDP Server is Running!!\n";
        }

        private string _statusBox;
        public string StatusBox
        {
            get { return _statusBox; }
            set
            {
                _statusBox = value;
                OnPropertyChanged("StatusBox");
            }
        }

        private void ReceiveThreadFunction()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(_localIPAddress), (int)_localPort);
            while (true)
            {
                try
                {
                    // wait for data
                    // this is a blocking call
                    Byte[] receiveData = _dataSocket.Receive(ref endPoint);

                    // convert byte array to a string
                    StatusBox += DateTime.Now.ToString() + ": " + System.Text.Encoding.Default.GetString(receiveData) + "\n";

                    _dataSocket.Send(receiveData, receiveData.Length, endPoint);
                }
                catch (SocketException ex)
                {
                    // got here because either the Receive failed, or more
                    // or more likely the socket was destroyed by 
                    // exiting from the JoystickPositionWindow form
                    StatusBox += ex.ToString() + "\n";
                    //MessageBox.Show(ex.ToString(), "UDP Server");
                    return;
                }
            }
        }

        public void StartThread()
        {
            // start the thread to listen for data from other UDP peer
            ThreadStart threadFunction = new ThreadStart(ReceiveThreadFunction);
            _receiveDataThread = new Thread(threadFunction);
            _receiveDataThread.Start();
        }
        
        public void CloseSocket()
        {
            _receiveDataThread.Abort();
            _dataSocket.Close();
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
