using System.Linq;
using Android.App;
using Android.Content.PM;

namespace Drone_Simulator.Extensions
{
    public static class ActivityExtensions
    {
        public static bool IsPermissionGranted(this Activity activity, string permission)
        {
            return activity.CheckSelfPermission(permission) == Permission.Granted;
        }

        public static bool AllOfPermissionsGranted(this Activity activity, params string[] permissions)
        {
            return permissions.All(activity.IsPermissionGranted);
        }
    }
}