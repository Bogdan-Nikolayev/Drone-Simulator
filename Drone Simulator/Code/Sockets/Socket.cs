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
            // 1 byte from "messageType".
            stream.WriteInt(bytes.Length + 1);
            stream.WriteByte(messageType);
            stream.Write(bytes);

            Log.Debug("Sent: " + message);
        }


        protected void StartListening()
        {
            Thread thread = new Thread(new Runnable(() =>
            {
                using DataInputStream inputStream = new DataInputStream(_socket.InputStream);
                while (true)
                {
                    int totalLength = inputStream.ReadInt();
                    sbyte messageType = inputStream.ReadByte();

                    ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
                    // 1 byte from "messageType".
                    int progressLength = 1;
                    while (progressLength != totalLength)
                    {
                        byte[] buffer = new byte[1024];
                        int remainingLength = totalLength - progressLength;
                        int readLength = Math.Min(remainingLength, buffer.Length);
                        inputStream.Read(buffer, 0, readLength);
                        outputStream.Write(buffer, 0, readLength);

                        progressLength += readLength;
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