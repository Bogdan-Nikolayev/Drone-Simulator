using System.IO;
using System.Text;
using Android.OS;
using Java.IO;
using Java.Lang;

namespace Drone_Simulator.Sockets
{
    public abstract class Socket : ISocket
    {
        protected Java.Net.Socket _socket;
        private readonly object _lockObject = new object();
        private readonly object _lockObject2 = new object();

        public event SocketMessageHandler StringReceived;

        public void SendString(sbyte messageType, string message)
        {
            lock (_lockObject)
            {
                Log.Debug("Start sending: " + message);
                using DataOutputStream stream = new DataOutputStream(_socket.OutputStream);
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                stream.WriteInt(bytes.Length + 1);
                stream.WriteByte(messageType);
                stream.Write(bytes);

                Log.Debug("Sent: " + message);
            }
        }


        protected void StartListening()
        {
            Thread thread = new Thread(new Runnable(() =>
            {
                using DataInputStream inputStream = new DataInputStream(_socket.InputStream);
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int incomeLength = 0;
                    int readLength = 0;

                    while (incomeLength - readLength != 0)
                        ReadMessage();

                    void ReadMessage()
                    {
                        int totalLength = inputStream.ReadInt();
                        sbyte messageType = inputStream.ReadByte();

                        ByteArrayOutputStream outputStream = new ByteArrayOutputStream();

                        int progressLength = 1;
                        Log.Debug("total: " + totalLength);
                        while (progressLength != totalLength)
                        {
                            incomeLength = inputStream.Read(buffer);
                            int remainingLength = totalLength - progressLength;
                            readLength = Math.Min(remainingLength, buffer.Length);
                            outputStream.Write(buffer, 0, readLength);

                            progressLength += readLength;
                            Log.Debug("income: " + incomeLength);
                            Log.Debug("remain: " + remainingLength);
                            Log.Debug("read: " + readLength);
                            Log.Debug("progress: " + progressLength);
                        }

                        string message = Encoding.UTF8.GetString(outputStream.ToByteArray());

                        // Run in UI thread.
                        new Handler(Looper.MainLooper).Post(() => StringReceived?.Invoke(messageType, message));

                        Log.Debug("Received: " + message);
                    }
                }
            }));
            thread.Start();
        }

        // private void ReadMessage(DataInputStream inputStream)
        // {
        //     int totalLength = inputStream.ReadInt();
        //     sbyte messageType = inputStream.ReadByte();
        //
        //     ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
        //
        //     int progressLength = 1;
        //     Log.Debug("total: " + totalLength);
        //     while (progressLength != totalLength)
        //     {
        //         int incomeLength = inputStream.Read(buffer);
        //         int remainingLength = totalLength - progressLength;
        //         readLength = Math.Min(remainingLength, buffer.Length);
        //         outputStream.Write(buffer, 0, readLength);
        //
        //         progressLength += readLength;
        //         Log.Debug("income: " + incomeLength);
        //         Log.Debug("remain: " + remainingLength);
        //         Log.Debug("read: " + readLength);
        //         Log.Debug("progress: " + progressLength);
        //     }
        //
        //     string message = Encoding.UTF8.GetString(outputStream.ToByteArray());
        //
        //     // Run in UI thread.
        //     new Handler(Looper.MainLooper).Post(() => StringReceived?.Invoke(messageType, message));
        //
        //     Log.Debug("Received: " + message);
        // }

        public void Dispose()
        {
            _socket?.Dispose();
        }
    }
}