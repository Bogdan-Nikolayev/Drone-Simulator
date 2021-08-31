using Android.Util;
using Android.Webkit;

namespace Drone_Simulator
{
    public class GrantedWebChromeClient : WebChromeClient
    {
        public override void OnPermissionRequest(PermissionRequest? request)
        {
            Log.Debug(Constants.Tag.DroneSimulator, string.Join(" ",
                nameof(GrantedWebChromeClient), nameof(OnPermissionRequest), string.Join(",", request.GetResources())));

            request.Grant(request.GetResources());
        }

        public override void OnGeolocationPermissionsShowPrompt(string? origin,
            GeolocationPermissions.ICallback? callback)
        {
            Log.Debug(Constants.Tag.DroneSimulator, string.Join(" ",
                nameof(GrantedWebChromeClient), nameof(OnGeolocationPermissionsShowPrompt)));

            callback.Invoke(origin, true, false);
        }
    }
}