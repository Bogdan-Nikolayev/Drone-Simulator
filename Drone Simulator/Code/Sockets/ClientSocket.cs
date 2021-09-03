using System;
using Java.IO;
using Java.Lang;
using Java.Net;

namespace Drone_Simulator.Sockets
{
    public class ClientSocket : ISocket, IDisposable
    {
        private readonly Socket _socket;

        public ClientSocket(InetAddress address, int port)
        {
            _socket = new Socket();
            _socket.Bind(null);
            _socket.Connect(new InetSocketAddress(address, port), 500);

            StartListening();
        }

        public event Action<string> StringReceived;

        public void SendString(string message)
        {
            using DataOutputStream stream = new DataOutputStream(_socket.OutputStream);
            stream.WriteUTF(message);
            stream.Flush();

            Log.Debug("Sent: " + message);
        }

        private void StartListening()
        {
            Thread thread = new Thread(new Runnable(() =>
            {
                using DataInputStream stream = new DataInputStream(_socket.InputStream);
                while (true)
                {
                    string message = stream.ReadUTF();
                    StringReceived?.Invoke(message);

                    Log.Debug("Received: " + message);
                }
            }));
            thread.Start();
        }

        public void Dispose()
        {
            _socket?.Dispose();
        }
    }
}