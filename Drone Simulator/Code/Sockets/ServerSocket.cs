using System;
using Java.IO;
using Java.Lang;
using Java.Net;

namespace Drone_Simulator.Sockets
{
    public class ServerSocket : ISocket, IDisposable
    {
        private readonly Socket _clientSocket;

        public ServerSocket(int port)
        {
            Java.Net.ServerSocket serverSocket = new Java.Net.ServerSocket(port);
            _clientSocket = serverSocket.Accept();

            StartListening();
        }

        public event Action<string> StringReceived;

        public void SendString(string message)
        {
            using DataOutputStream stream = new DataOutputStream(_clientSocket.OutputStream);
            stream.WriteUTF(message);
            stream.Flush();

            Log.Debug("Sent: " + message);
        }

        private void StartListening()
        {
            Thread thread = new Thread(new Runnable(() =>
            {
                using DataInputStream stream = new DataInputStream(_clientSocket.InputStream);
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
            _clientSocket?.Dispose();
        }
    }
}