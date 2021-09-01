using Android.Util;
using Android.Webkit;
using Java.Interop;
using Java.Lang;

namespace Drone_Simulator
{
    public class JavaScriptInterface : Object
    {
        [Export]
        [JavascriptInterface]
        // ReSharper disable once UnusedMember.Global
        public void ReceiveData(string data)
        {
            Log.Debug(Constants.Tag.DroneSimulator, string.Join(" ",
                nameof(JavaScriptInterface), nameof(ReceiveData), data));
        }
    }
}