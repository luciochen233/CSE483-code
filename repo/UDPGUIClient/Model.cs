using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Sockets
using System.Net.Sockets;
using System.Net;

// Threads
using System.Threading;
//object
using System.ComponentModel;

namespace UDPGUIClient
{
    class Model : INotifyPropertyChanged
    {
        // some data that keeps track of ports and addresses
        private static int _remotePort = 5000;
        private static string _remoteIPAddress = "127.0.0.1";

        private static int _localPort = 5001;
        private static string _localIPAddress = "127.0.0.1";

        // this is the UDP socket that will be used to communicate
        // over the network
        static private UdpClient _dataSocket;

        // this is the thread that will run in the background
        // waiting for incomming data
        private static Thread _receiveDataThread;

        public Model()
        {
            _dataSocket = new UdpClient(_localPort);
            StartThread();
        }


        public void SendMessage()
        {
            IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);
            Byte[] sendBytes = Encoding.ASCII.GetBytes(MessageBox);

            try
            {
                _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);
            }
            catch (SocketException ex)
            {
                StatusBox += ex.ToString();
                _dataSocket.Close();
                return;
            }
            StatusBox += DateTime.Now.ToString() + ": Message " + MessageBox + " sent successfully" + "\n";
            MessageBox = "";
        }

        public void StartThread()
        {
            // start the thread to listen for data from other UDP peer
            ThreadStart threadFunction = new ThreadStart(ReceiveThreadFunction);
            _receiveDataThread = new Thread(threadFunction);
            _receiveDataThread.Start();
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
                    LoopBack = DateTime.Now.ToString() + ": " + System.Text.Encoding.Default.GetString(receiveData) + "\n";
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
        public void CloseSocket()
        {
            _receiveDataThread.Abort();
            _dataSocket.Close();
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

        private string _messageBox;
        public string MessageBox
        {
            get { return _messageBox; }
            set
            {
                _messageBox = value;
                OnPropertyChanged("MessageBox");
            }
        }

        private string _loopBack;
        public string LoopBack
        {
            get { return _loopBack; }
            set
            {
                _loopBack = value;
                OnPropertyChanged("LoopBack");
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
