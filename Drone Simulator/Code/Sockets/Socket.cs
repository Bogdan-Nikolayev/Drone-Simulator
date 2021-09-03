using Android.OS;
using Java.IO;
using Java.Lang;

namespace Drone_Simulator.Sockets
{
    public abstract class Socket : ISocket
    {
        protected Java.Net.Socket _socket;

        public event SocketMessageHandler StringReceived;

        public void SendString(sbyte messageType, string message)
        {
            using DataOutputStream stream = new DataOutputStream(_socket.OutputStream);
            stream.WriteByte(messageType);
            stream.WriteUTF(message);
            stream.Flush();

            Log.Debug("Sent: " + message);
        }

        protected void StartListening()
        {
            Thread thread = new Thread(new Runnable(() =>
            {
                using DataInputStream stream = new DataInputStream(_socket.InputStream);
                while (true)
                {
                    sbyte messageType = stream.ReadByte();
                    string message = stream.ReadUTF();
                    
                    // Run in UI thread.
                    new Handler(Looper.MainLooper).Post(() => StringReceived?.Invoke(messageType, message));

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