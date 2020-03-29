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

// Message Box in Console app
// Don't forget to add Reference to System.Windows.Forms
using System.Windows.Forms;

namespace ConsoleUDPServer
{
    class Program
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

        static void Main(string[] args)
        {
            MessageBox.Show("UDP Server is Running!!\n","UDP Server");
            _dataSocket = new UdpClient(_localPort);
            StartThread();
        }

        private static void ReceiveThreadFunction()
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
                    MessageBox.Show(DateTime.Now + ": " + System.Text.Encoding.Default.GetString(receiveData) + "\n","UDP Server");
                }
                catch (SocketException ex)
                {
                    // got here because either the Receive failed, or more
                    // or more likely the socket was destroyed by 
                    // exiting from the JoystickPositionWindow form
                    MessageBox.Show(ex.ToString(),"UDP Server");
                    return;
                }
            }
        }

        public static void StartThread()
        {
            // start the thread to listen for data from other UDP peer
            ThreadStart threadFunction = new ThreadStart(ReceiveThreadFunction);
            _receiveDataThread = new Thread(threadFunction);
            _receiveDataThread.Start();
        }
    }
}
