using System;
using Android.Net.Wifi.P2p;
using Drone_Simulator.WifiDirect.Sockets;
using Java.IO;
using Java.Net;

namespace Drone_Simulator.Sockets
{
    public class ServerSocket : ISocket
    {
        private Socket _clientSocket;

        public ServerSocket(WifiP2pInfo info)
        {
            Java.Net.ServerSocket serverSocket = new Java.Net.ServerSocket(8888, 1);
            _clientSocket = serverSocket.Accept();

            StartListening();
        }

        public event Action<string> StringReceived;

        public void SendString(string message)
        {
            throw new NotImplementedException();
        }

        private void StartListening()
        {
            while (true)
            {
                DataInputStream stream = new DataInputStream(_clientSocket.InputStream);
                StringReceived?.Invoke(stream.ReadUTF());
            }
        }
    }
}