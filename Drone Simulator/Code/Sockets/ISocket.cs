using System;

namespace Drone_Simulator.WifiDirect.Sockets
{
    public interface ISocket
    {
        void SendString(string message);

        event Action<string> StringReceived;
    }
}