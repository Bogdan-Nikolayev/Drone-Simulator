using System.Text;
using Android.OS;
using Java.IO;
using Java.Lang;

namespace Drone_Simulator.Sockets
{
    // TODO: Refactoring.
    // TODO: Add binary communication.
    // TODO: Swap length and message type to remove counting of message type.
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
                Log.Debug("Sent: " + Encoding.UTF8.GetString(bytes));
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

                    using ByteArrayOutputStream resultStream = new ByteArrayOutputStream();

                    // The following code throws the following exception:
                    // android.runtime.JavaProxyThrowable: System.OutOfMemoryException:
                    // Insufficient memory to continue the execution of the program.
                    //
                    // byte[] buffer = new byte[totalLength - 1];
                    // inputStream.ReadFully(buffer);
                    // outputStream.Write(buffer);

                    // 1 byte from "messageType".
                    int progressLength = 1;
                    while (progressLength != totalLength)
                    {
                        byte[] buffer = new byte[1024];
                        int remainingLength = totalLength - progressLength;
                        int readLength = Math.Min(remainingLength, buffer.Length);

                        inputStream.ReadFully(buffer, 0, readLength);
                        resultStream.Write(buffer, 0, readLength);

                        progressLength += readLength;
                    }

                    string message = Encoding.UTF8.GetString(resultStream.ToByteArray());
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