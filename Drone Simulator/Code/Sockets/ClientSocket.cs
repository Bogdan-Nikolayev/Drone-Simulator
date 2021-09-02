using System;
using Android.Net.Wifi.P2p;
using Drone_Simulator.WifiDirect.Sockets;
using Java.IO;
using Java.Net;

namespace Drone_Simulator.Sockets
{
    public class ClientSocket : ISocket
    {
        private readonly Socket _socket;
        
        public ClientSocket(WifiP2pInfo info)
        {
            _socket = new Socket();
            _socket.Bind(null);
            _socket.Connect(new InetSocketAddress(info.GroupOwnerAddress, 8888), 500);
        }

        public void SendString(string message)
        {
            using DataOutputStream stream = new DataOutputStream(_socket.OutputStream);
            stream.WriteUTF(message);
            stream.Flush();
            
            Log.Debug("Sent: " + message);
        }

        public event Action<string> StringReceived;
    }
}