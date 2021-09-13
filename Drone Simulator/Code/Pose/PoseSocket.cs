using System;
using Drone_Simulator.Sockets;
using Org.Json;

namespace Drone_Simulator.Pose
{
    // TODO: Replace string serialization by binary serialization to improve performance and traffic usage.
    public class PoseSocket
    {
        private readonly ISocket _socket;

        public event Action<Pose> PoseReceived;

        public PoseSocket(ISocket socket)
        {
            _socket = socket;
            _socket.StringReceived += (type, message) =>
            {
                if (type == (sbyte)PoseMessageType.Pose)
                    PoseReceived?.Invoke(ParsePose(message));
            };
        }

        public void SendPose(Pose pose)
        {
            JSONObject json = new JSONObject();
            json.Put(nameof(pose.RotationX), pose.RotationX);
            json.Put(nameof(pose.RotationY), pose.RotationY);
            json.Put(nameof(pose.RotationZ), pose.RotationZ);

            _socket.SendString((sbyte)PoseMessageType.Pose, json.ToString());
        }

        private Pose ParsePose(string message)
        {
            JSONObject json = new JSONObject(message);

            return new Pose(
                (float)json.GetDouble("RotationX"),
                (float)json.GetDouble("RotationY"),
                (float)json.GetDouble("RotationZ")
            );
        }
    }
}