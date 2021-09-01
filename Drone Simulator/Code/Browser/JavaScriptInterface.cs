using Android.Webkit;
using Java.Interop;
using Java.Lang;

namespace Drone_Simulator.Browser
{
    public class JavaScriptInterface : Object
    {
        [Export]
        [JavascriptInterface]
        // ReSharper disable once UnusedMember.Global
        public void ReceiveData(string data)
        {
            Log.Debug(data);
        }
    }
}