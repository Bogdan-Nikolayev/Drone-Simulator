using System.Text;
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
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            stream.WriteByte(messageType);
            stream.WriteInt(bytes.Length);
            stream.Write(bytes);
            stream.Flush();

            Log.Debug("Sent: " + message);
        }

        protected void StartListening()
        {
            Thread thread = new Thread(new Runnable(() =>
            {
                using DataInputStream inputStream = new DataInputStream(_socket.InputStream);
                while (true)
                {
                    sbyte messageType = inputStream.ReadByte();
                    int totalLength = inputStream.ReadInt();

                    ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
                    byte[] buffer = new byte[1024];
                    int progressLength = 0;
                    while (progressLength != totalLength)
                    {
                        int currentLength = inputStream.Read(buffer);
                        outputStream.Write(buffer, 0, currentLength);

                        progressLength += currentLength;
                    }

                    string message = Encoding.UTF8.GetString(outputStream.ToByteArray());

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