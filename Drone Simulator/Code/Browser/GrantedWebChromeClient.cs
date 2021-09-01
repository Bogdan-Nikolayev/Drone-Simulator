using Android.Webkit;

namespace Drone_Simulator.Browser
{
    public class GrantedWebChromeClient : WebChromeClient
    {
        public override void OnPermissionRequest(PermissionRequest? request)
        {
            request.Grant(request.GetResources());
        }

        public override void OnGeolocationPermissionsShowPrompt(string? origin,
            GeolocationPermissions.ICallback? callback)
        {
            callback.Invoke(origin, true, false);
        }
    }
}