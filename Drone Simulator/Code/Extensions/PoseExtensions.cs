using Org.Json;

namespace Drone_Simulator.Extensions
{
    public static class PoseExtensions
    {
        public static string ToJson(this Pose.Pose pose)
        {
            JSONObject json = new JSONObject();
            json.Put(nameof(pose.RotationX), pose.RotationX);
            json.Put(nameof(pose.RotationY), pose.RotationY);
            json.Put(nameof(pose.RotationZ), pose.RotationZ);

            return json.ToString();
        }

        // TODO: Make more specific extension method source (e. g. JSONObject).
        public static Pose.Pose ParsePoseJson(this string json)
        {
            JSONObject jsonObject = new JSONObject(json);

            return new Pose.Pose(
                (float)jsonObject.GetDouble(nameof(Pose.Pose.RotationX)),
                (float)jsonObject.GetDouble(nameof(Pose.Pose.RotationY)),
                (float)jsonObject.GetDouble(nameof(Pose.Pose.RotationZ))
            );
        }
    }
}