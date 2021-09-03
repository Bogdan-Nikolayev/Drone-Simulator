using System;
using Java.Net;

namespace Drone_Simulator.Sockets
{
    public class ClientSocket : Socket, IDisposable
    {
        public ClientSocket(InetAddress address, int port)
        {
            _socket = new Java.Net.Socket();
            _socket.Bind(null);
            _socket.Connect(new InetSocketAddress(address, port), 500);

            StartListening();
        }
    }
}