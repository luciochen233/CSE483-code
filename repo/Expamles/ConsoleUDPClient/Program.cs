using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Sockets
using System.Net.Sockets;
using System.Net;

// Message Box
// don't forget to add Reference System.Windows.Forms
using System.Windows.Forms;

namespace ConsoleUDPClient
{
    class Program
    {

        // some data that keeps track of ports and addresses
        private static UInt32 _remotePort = 5000;
        private static String _remoteIPAddress = "127.0.0.1";

        // this is the UDP socket that will be used to communicate
        // over the network
        static private UdpClient _dataSocket;

        static void Main(string[] args)
        {
                try
                {
                    // set up generic UDP socket and bind to local port
                    //
                    _dataSocket = new UdpClient();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }

            SendMessage();
        }

        public static void SendMessage()
        {
            IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);
            Byte[] sendBytes = Encoding.ASCII.GetBytes("Hello World!!");

            try
            {
                _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }
    }
}
