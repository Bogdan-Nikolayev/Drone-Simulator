using System;
using Java.Net;

namespace Drone_Simulator.Sockets
{
    public class ClientSocket : Socket, IDisposable
    {
        public ClientSocket(InetAddress address, int port)
        {
            _socket = new Java.Net.Socket();
            // https://stackoverflow.com/questions/20068710/java-net-bindexception-bind-failed-eaddrinuse-address-already-in-use
            // https://stackoverflow.com/questions/34615588/keep-getting-annoying-warning-bind-failed-eaddrinuse-address-already-in-use
            _socket.ReuseAddress = true;
            _socket.Bind(null);
            _socket.Connect(new InetSocketAddress(address, port), 500);

            StartListening();
        }
    }
}