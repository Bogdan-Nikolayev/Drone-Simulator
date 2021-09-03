using Android.Webkit;
using Drone_Simulator.Sockets;
using Java.Interop;
using Java.Lang;

namespace Drone_Simulator.Browser
{
    public class WebRtcJavaScriptInterface : Object
    {
        private readonly ISocket _signalingSocket;

        public WebRtcJavaScriptInterface(ISocket signalingSocket)
        {
            _signalingSocket = signalingSocket;
        }

        [Export]
        [JavascriptInterface]
        // ReSharper disable once UnusedMember.Global
        public void SendOffer(string offer)
        {
            Log.Debug("Sending offer" + offer);

            _signalingSocket.SendString(offer);
        }
    }
}