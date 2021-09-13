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
        // ReSharper disable once UnusedMember.Global
        public void ReceivePose(Pose pose)
        {
            InvokeJavaScriptFunction("receivePose", pose.ToString());
        }
    }
}