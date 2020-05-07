using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Sockets
using System.Net.Sockets;
using System.Net;

// debug
using System.Diagnostics;

// threading
using System.Threading;

// byte data serialization
using System.Runtime.Serialization.Formatters.Binary;

// memory streams
using System.IO;


namespace SampleUDPPeer
{
    partial class Model
    {
        // make GameData serializeable. Otherwise, we can't send it over 
        // a byte stream (e.g. socket)
        [Serializable]
        struct GameData
        {
            public int data1, data2;
            public String message;

            public GameData(int p, int s, String msg)
            {
                data1 = p;
                data2 = s;
                message = msg;
            }
        }

        // this is the UDP socket that will be used to communicate
        // over the network
        UdpClient _dataSocket;

        // some data that keeps track of ports and addresses
        private static UInt32 _localPort;
        private static String _localIPAddress;
        private static UInt32 _remotePort;
        private static String _remoteIPAddress;

        // this is the thread that will run in the background
        // waiting for incomming data
        private Thread _receiveDataThread;

        // this thread is used to synchronize the startup of 
        // two UDP peers
        private Thread _synchWithOtherPlayerThread;

        private Random _randomNumber;

        public Model()
        {
            _randomNumber = new Random();
            Data1 = _randomNumber.Next(100).ToString();
            Data2 = _randomNumber.Next(100).ToString();

            // this disables the Send button initially
            SendEnabled = false;

            // initialize the help test
            HelpText = "Select Socket Setup button to being.";
        }

        /// <summary>
        /// this method is called to set this UDP peer's local port and address
        /// </summary>
        /// <param name="port"></param>
        /// <param name="ipAddress"></param>
        public void SetLocalNetworkSettings(UInt32 port, String ipAddress)
        {
            _localPort = port;
            _localIPAddress = ipAddress;
        }

        /// <summary>
        /// this method is called to set the remote UDP peer's port and address
        /// </summary>
        /// <param name="port"></param>
        /// <param name="ipAddress"></param>
        public void SetRemoteNetworkSettings(UInt32 port, String ipAddress)
        {
            _remotePort = port;
            _remoteIPAddress = ipAddress;
        }

        /// <summary>
        /// initialize the necessary data, and start the synchronization
        /// thread to wait for the other peer to join
        /// </summary>
        /// <returns></returns>
        public bool InitModel()
        {
            try
            {
                // set up generic UDP socket and bind to local port
                //
                _dataSocket = new UdpClient((int)_localPort);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return false;
            }

            ThreadStart threadFunction;
            threadFunction = new ThreadStart(SynchWithOtherPlayer);
            _synchWithOtherPlayerThread = new Thread(threadFunction);
            StatusTextBox = StatusTextBox + DateTime.Now + ":" + " Waiting for other UDP peer to join.\n";
            _synchWithOtherPlayerThread.Start();

            // reset help text
            HelpText = "";

            return true;
        }

        /// <summary>
        /// called to send some data to the other UDP peer
        /// </summary>
        public void SendMessage()
        {
            // data structure used to communicate data with the other player
            GameData gameData;

            // formatter used for serialization of data
            BinaryFormatter formatter = new BinaryFormatter();

            // stream needed for serialization
            MemoryStream stream = new MemoryStream();

            // Byte array needed to send data over a socket
            Byte[] sendBytes;

            // check to make sure boxes have something in them to send
            if (MeBox == "" || Data1 == "" || Data2 == "")
            {
                StatusTextBox = StatusTextBox + DateTime.Now + " Empty boxes! Try again.\n";
                return;
            }


            // we make sure that the data in the boxes is in the correct format
            try
            {
                gameData.data1 = int.Parse(Data1);
                gameData.data2 = int.Parse(Data2);
                gameData.message = MeBox;
            }
            catch (System.Exception)
            {
                // we get here if the format of teh data in the boxes was incorrect. Most likely the boxes we assumed
                // had integers in them had characters as well
                StatusTextBox = StatusTextBox + DateTime.Now + " Data not in correct format! Try again.\n";
                return;
            }

            // serialize the gameData structure to a stream
            formatter.Serialize(stream, gameData);

            // retrieve a Byte array from the stream
            sendBytes = stream.ToArray();

            // send the serialized data
            IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);
            try
            {
                _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);
            }
            catch (SocketException)
            {
                StatusTextBox = StatusTextBox + DateTime.Now + ":" + " ERROR: Message not sent!\n";
                return;
            }

            StatusTextBox = StatusTextBox + DateTime.Now + ":" + " Message sent successfully.\n";
        }

        /// <summary>
        /// called when the view is closing to ensure we clean up our socket
        /// if we don't, the application may hang on exit
        /// </summary>
        public void Model_Cleanup()
        {
            // important. Close socket or application will not exit correctly.
            if (_dataSocket != null) _dataSocket.Close();
            if (_receiveDataThread != null) _receiveDataThread.Abort();

        }

        // this is the thread that waits for incoming messages
        private void ReceiveThreadFunction()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    // wait for data
                    Byte[] receiveData = _dataSocket.Receive(ref endPoint);

                    // check to see if this is synchronization data 
                    // ignore it. we should not recieve any sychronization
                    // data here, because synchronization data should have 
                    // been consumed by the SynchWithOtherPlayer thread. but, 
                    // it is possible to get 1 last synchronization byte, which we
                    // want to ignore
                    if (receiveData.Length < 2)
                        continue;


                    // process and display data


                    GameData gameData;
                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream stream = new MemoryStream();

                    // deserialize data back into our GameData structure
                    stream = new System.IO.MemoryStream(receiveData);
                    gameData = (GameData)formatter.Deserialize(stream);

                    // update view data through our bound properties
                    MyFriendBox = gameData.message;
                    Data1 = gameData.data1.ToString();
                    Data2 = gameData.data2.ToString();

                    // update status window
                    StatusTextBox = StatusTextBox + DateTime.Now + ":" + " New message received.\n";

                }
                catch (SocketException ex)
                {
                    // got here because either the Receive failed, or more
                    // or more likely the socket was destroyed by 
                    // exiting from the JoystickPositionWindow form
                    Console.WriteLine(ex.ToString());
                    return;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }

            }
        }

        /// <summary>
        /// this thread is used at initialization to synchronize with the other
        /// UDP peer
        /// </summary>
        private void SynchWithOtherPlayer()
        {

            // set up socket for sending synch byte to UDP peer
            // we can't use the same socket (i.e. _dataSocket) in the same thread context in this manner
            // so we need to set up a separate socket here
            Byte[] data = new Byte[1];
            IPEndPoint endPointSend = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);
            IPEndPoint endPointRecieve = new IPEndPoint(IPAddress.Any,0);

            UdpClient synchSocket = new UdpClient((int)_localPort + 10);

            // set timeout of receive to 1 second
            _dataSocket.Client.ReceiveTimeout = 1000;

            while (true)
            {
                try
                {
                    synchSocket.Send(data, data.Length, endPointSend);
                    _dataSocket.Receive(ref endPointRecieve);

                    // got something, so break out of loop
                    break;
                }
                catch (SocketException ex)
                {
                    // we get an exception if there was a timeout
                    // if we timed out, we just go back and try again
                    if (ex.ErrorCode == (int)SocketError.TimedOut)
                    {
                        Debug.Write(ex.ToString());
                    }
                    else
                    {
                        // we did not time out, but got a really bad 
                        // error
                        synchSocket.Close();
                        StatusTextBox = StatusTextBox + "Socket exception occurred. Unable to sync with other UDP peer.\n";
                        StatusTextBox = StatusTextBox + ex.ToString();
                        return;
                    }
                }
                catch (System.ObjectDisposedException ex)
                {
                    // something bad happened. close the socket and return
                    Console.WriteLine(ex.ToString());
                    synchSocket.Close();
                    StatusTextBox = StatusTextBox + "Error occurred. Unable to sync with other UDP peer.\n";
                    return;
                }

            }

            // send synch byte
            synchSocket.Send(data, data.Length, endPointSend);

            // close the socket we used to send periodic requests to player 2
            synchSocket.Close();

            // reset the timeout for the dataSocket to infinite
            // _dataSocket will be used to recieve data from other UDP peer
            _dataSocket.Client.ReceiveTimeout = 0;

            // start the thread to listen for data from other UDP peer
            ThreadStart threadFunction = new ThreadStart(ReceiveThreadFunction);
            _receiveDataThread = new Thread(threadFunction);
            _receiveDataThread.Start();


            // got this far, so we received a response from player 2
            StatusTextBox = StatusTextBox + DateTime.Now + ":" + " Other UDP peer has joined the session.\n";
            HelpText = "Enter text in the Me box and hit the Send button.";
            SendEnabled = true;
        }
    }
}
