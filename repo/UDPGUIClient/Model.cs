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
        private static UInt32 _remotePort = 5000;
        private static String _remoteIPAddress = "127.0.0.1";

        // this is the UDP socket that will be used to communicate
        // over the network
        static private UdpClient _dataSocket;


        public void Send()
        {
            try
            {
                // set up generic UDP socket and bind to local port
                _dataSocket = new UdpClient();
            }
            catch (Exception ex)
            {
                StatusBox += ex.ToString();
                return;
            }

            SendMessage();
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
