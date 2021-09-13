using Android.Webkit;
using Drone_Simulator.Browser;
using Java.Interop;

namespace Drone_Simulator.Pose
{
    public class PoseJavaScriptInterface : JavaScriptInterface
    {
        public PoseJavaScriptInterface(WebView webView) : base(webView)
        {
        }

        [Export]
        [JavascriptInterface]
        public void ReceivePose(string pose)
        {
            InvokeJavaScriptFunction("receivePose", pose);
        }
    }
}