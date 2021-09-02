using Android.Webkit;
using Java.Interop;
using Java.Lang;

namespace Drone_Simulator.Browser
{
    public class JavaScriptWebRtcInterface : Object
    {
        [Export]
        [JavascriptInterface]
        // ReSharper disable once UnusedMember.Global
        public void SendOffer(string offer)
        {
            Log.Debug(offer);
        }
    }
}