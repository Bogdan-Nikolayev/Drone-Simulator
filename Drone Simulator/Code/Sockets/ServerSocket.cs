using System;

namespace Drone_Simulator.Sockets
{
    public class ServerSocket : Socket, IDisposable
    {
        public ServerSocket(int port)
        {
            Java.Net.ServerSocket serverSocket = new Java.Net.ServerSocket(port);
            _socket = serverSocket.Accept();
            
            StartListening();
        }
    }
}