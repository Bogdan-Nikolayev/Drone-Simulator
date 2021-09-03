namespace Drone_Simulator.Sockets
{
    public delegate void SocketMessageHandler(sbyte messageType, string message);

    public interface ISocket
    {
        void SendString(sbyte messageType, string message);

        event SocketMessageHandler StringReceived;
    }
}