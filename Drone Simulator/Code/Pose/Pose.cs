using System;

namespace Drone_Simulator.Pose
{
    [Serializable]
    public readonly struct Pose
    {
        public Pose(float rotationX, float rotationY, float rotationZ)
        {
            RotationX = rotationX;
            RotationY = rotationY;
            RotationZ = rotationZ;
        }

        public float RotationX { get; }
        public float RotationY { get; }
        public float RotationZ { get; }
    }
}