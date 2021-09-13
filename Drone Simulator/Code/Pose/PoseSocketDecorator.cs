using System;
using Drone_Simulator.Extensions;
using Drone_Simulator.Sockets;

namespace Drone_Simulator.Pose
{
    // TODO: Replace string serialization by binary serialization to improve performance and traffic usage.
    public class PoseSocketDecorator
    {
        private readonly ISocket _socket;

        public event Action<Pose> PoseReceived;

        public PoseSocketDecorator(ISocket socket)
        {
            _socket = socket;
            _socket.StringReceived += (type, message) =>
            {
                if (type == (sbyte)PoseMessageType.Pose)
                    PoseReceived?.Invoke(message.ParsePoseJson());
            };
        }

        public void SendPose(Pose pose)
        {
            _socket.SendString((sbyte)PoseMessageType.Pose, pose.ToJson());
        }
    }
}