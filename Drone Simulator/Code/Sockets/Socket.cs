using System.Text;
using System.Threading;
using Android.OS;
using Java.IO;
using Java.Lang;

namespace Drone_Simulator.Sockets
{
    // TODO: Refactoring.
    public abstract class Socket : ISocket
    {
        protected Java.Net.Socket _socket;

        public event SocketMessageHandler StringReceived;

        public void SendString(sbyte messageType, string message, bool log = true)
        {
            using DataOutputStream outputStream = new DataOutputStream(_socket.OutputStream);
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            // 1 byte from "messageType".
            outputStream.WriteInt(bytes.Length + 1);
            outputStream.WriteByte(messageType);
            outputStream.Write(bytes);

            if (log)
                Log.Debug("Sent: " + Encoding.ASCII.GetString(bytes));
        }

        protected void StartListening()
        {
            Java.Lang.Thread thread = new Java.Lang.Thread(new Runnable(() =>
            {
                using DataInputStream inputStream = new DataInputStream(_socket.InputStream);
                while (true)
                {
                    int totalLength = inputStream.ReadInt();
                    sbyte messageType = inputStream.ReadByte();

                    using ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
                    // 1 byte from "messageType".
                    int progressLength = 1;
                    while (progressLength != totalLength)
                    {
                        byte[] buffer = new byte[1024];
                        int remainingLength = totalLength - progressLength;
                        int readLength = Math.Min(remainingLength, buffer.Length);
                        inputStream.ReadFully(buffer, 0, readLength);
                        outputStream.Write(buffer, 0, readLength);

                        progressLength += readLength;
                    }

                    string message = Encoding.ASCII.GetString(outputStream.ToByteArray());
                    Log.Debug("Received: " + message);
                    // Run in UI thread.
                    new Handler(Looper.MainLooper).Post(() => StringReceived?.Invoke(messageType, message));
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