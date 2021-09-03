using System;

namespace Drone_Simulator.Sockets
{
    public interface ISocket
    {
        void SendString(string message);

        event Action<string> StringReceived;
    }
}