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

        public override bool OnConsoleMessage(ConsoleMessage? consoleMessage)
        {
            Log.Debug(
                $"[JavaScript]({consoleMessage.InvokeMessageLevel()}) {consoleMessage.Message()} " +
                $"[{consoleMessage.SourceId()}:{consoleMessage.LineNumber()}]");

            return true;
        }
    }
}