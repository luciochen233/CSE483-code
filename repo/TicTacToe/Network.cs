﻿using System;
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
using System.Windows.Threading;

// byte data serialization
using System.Runtime.Serialization.Formatters.Binary;

// memory streams
using System.IO;

// observable collections
using System.Collections.ObjectModel;


// Brushes
using System.Windows.Media;


namespace TicTacToe
{
    partial class Model
    {
        //NETWORK Part
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

        [Serializable]
        struct GameData
        {
            //public ObservableCollection<Tile> TileCollection;
            //public Ticlogic tic;
            public double slide;
            public int which;
            public String message;


            public GameData(double s, int w, String msg)
            {
                //TileCollection = Tile;
                //tic = t;
                slide = s;
                which = w;
                message = msg;
            }
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

        public bool InitNetwork()
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
            //StatusText = StatusText + DateTime.Now + ":" + " Waiting for other UDP peer to join.\n";
            StatusText = "Waiting";
            _synchWithOtherPlayerThread.Start();

            // reset help text
            //HelpText = "";

            return true;
        }

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


            // we make sure that the data in the boxes is in the correct format
            try
            {
                //gameData.tic = tic;
                //gameData.TileCollection = TileCollection;
                gameData.which = current_selection;
                gameData.slide = Slide;
                gameData.message = StatusText;
            }
            catch (System.Exception)
            {
                // we get here if the format of teh data in the boxes was incorrect. Most likely the boxes we assumed
                // had integers in them had characters as well
                //StatusTextBox = StatusTextBox + DateTime.Now + " Data not in correct format! Try again.\n";
                StatusText = "Internal ERROR";
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
                //StatusTextBox = StatusTextBox + DateTime.Now + ":" + " ERROR: Message not sent!\n";
                StatusText = "NET ERROR";
                return;
            }

            //StatusTextBox = StatusTextBox + DateTime.Now + ":" + " Message sent successfully.\n";
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
            IPEndPoint endPointRecieve = new IPEndPoint(IPAddress.Any, 0);

            UdpClient synchSocket = new UdpClient((int)_localPort + 10);

            // set timeout of receive to 1 second
            _dataSocket.Client.ReceiveTimeout = 1000;

            while (true)
            {
                try
                {
                    synchSocket.Send(data, data.Length, endPointSend);
                    _dataSocket.Receive(ref endPointRecieve);
                    online = true;
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
                        //StatusText = StatusText + "Socket exception occurred. Unable to sync with other UDP peer.\n";
                        //StatusText = StatusText + ex.ToString();
                        StatusText = "NET ERROR";
                        online = false;
                        return;
                    }
                }
                catch (System.ObjectDisposedException ex)
                {
                    // something bad happened. close the socket and return
                    Console.WriteLine(ex.ToString());
                    synchSocket.Close();
                    //StatusText = StatusText + "Error occurred. Unable to sync with other UDP peer.\n";
                    StatusText = "NET ERROR";
                    online = false;
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
            //StatusText = StatusText + DateTime.Now + ":" + " Other UDP peer has joined the session.\n";
            StatusText = "MATCH FOUND";
            online = true;
            //HelpText = "Enter text in the Me box and hit the Send button.";
            //SendEnabled = true;
        }

        private void ReceiveThreadFunction()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    // wait for data
                    Byte[] receiveData;
                    try
                    {
                        receiveData = _dataSocket.Receive(ref endPoint);
                    }
                    catch (Exception e)
                    {
                        Debug.Write(e.ToString());
                        break;
                    }
                    

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
                    //TileCollection = gameData.TileCollection;
                    //tic = gameData.tic;
                    Cheat_flag = false;
                    Slide = gameData.slide;
                    StatusText = gameData.message;
                    if(gameData.message == "RESET")
                    {
                        tic.Clear();
                        Cheat_flag = false;
                        Slide = 0;
                        current_selection = -1;
                        no_update = false;
                        all_winner = -1;
                    }
                    //if(gameData.message == "O WINS")
                    //{
                    //    Win();
                    //}
                    //if (gameData.message == "X WINS")
                    //{
                    //    Win();
                    //}

                    int index = gameData.which;
                    if (index != -1)
                    {
                        current_selection = -1;
                        char[] _player = { 'X', 'O' };
                        Brush[] _ox_brush = { new SolidColorBrush(Colors.Red), new SolidColorBrush(Colors.Blue) };
                        int _row = index / 3;
                        int _col = index % 3;
                        if (tic.Mark(_player[_side], _row, _col))
                        {
                            TileCollection[index].TileLabel = _player[_side].ToString();
                            TileCollection[index].TileBackground = _ox_brush[_side];
                        }
                    }

                    //char[] _player = { 'X', 'O' };
                    //Brush[] _ox_brush = { new SolidColorBrush(Colors.Red), new SolidColorBrush(Colors.Blue) };

                    //for (int row = 0; row < 3; row++)
                    //{
                    //    for (int col = 0; col < 3; col++)
                    //    {
                    //        int index = row * 3 + col;
                    //        char tile_msg = gameData.tic._check[row, col];
                    //        //TileCollection[index].TileLabel = tile_msg.ToString();
                    //        if (tile_msg == 'X')
                    //        {
                    //            //TileCollection[index].TileBrush = new SolidColorBrush(Colors.Red);
                    //            TileCollection[index].TileLabel = 'X'.ToString();
                    //        }
                    //        else if (tile_msg == 'O')
                    //        {
                    //            //TileCollection[index].TileBrush = new SolidColorBrush(Colors.Blue);
                    //            TileCollection[index].TileLabel = 'O'.ToString();
                    //        }
                    //        else
                    //        {
                    //            //do nothing
                    //        }
                    //    }
                    //}

                    //Dispatcher.BeginInvoke(Update_Tile);
                    //Update_Tile();
                    //MyFriendBox = gameData.message;
                    //Data1 = gameData.data1.ToString();
                    //Data2 = gameData.data2.ToString();

                    // update status window
                    //StatusTextBox = StatusTextBox + DateTime.Now + ":" + " New message received.\n";

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
        /// called when the view is closing to ensure we clean up our socket
        /// if we don't, the application may hang on exit
        /// </summary>
        public void Model_Cleanup()
        {
            // important. Close socket or application will not exit correctly.
            if (_dataSocket != null) _dataSocket.Close();
            if (_receiveDataThread != null) _receiveDataThread.Abort();
            online = false;
        }

    }
}
