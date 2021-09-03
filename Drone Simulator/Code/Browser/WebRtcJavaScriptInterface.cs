using Android.Webkit;
using Drone_Simulator.Sockets;
using Java.Interop;
using Java.Lang;

namespace Drone_Simulator.Browser
{
    public class WebRtcJavaScriptInterface : Object
    {
        private readonly WebView _webView;
        private readonly ISocket _signalingSocket;

        public WebRtcJavaScriptInterface(WebView webView, ISocket signalingSocket)
        {
            _webView = webView;
            _signalingSocket = signalingSocket;

            _signalingSocket.StringReceived += (type, message) =>
            {
                switch ((WebRtcMessageType)type)
                {
                    case WebRtcMessageType.Offer:
                        ReceiveOffer(message);
                        break;
                    case WebRtcMessageType.Answer:
                        ReceiveAnswer(message);
                        break;
                }
            };
        }

        [Export]
        [JavascriptInterface]
        // ReSharper disable once UnusedMember.Global
        public void SendOffer(string offer)
        {
            Log.Debug("Sending offer (CSharp)" + offer);

            _signalingSocket.SendString((sbyte)WebRtcMessageType.Offer, offer);
        }

        [Export]
        [JavascriptInterface]
        // ReSharper disable once UnusedMember.Global
        public void SendAnswer(string answer)
        {
            Log.Debug("Sending answer (CSharp)" + answer);

            _signalingSocket.SendString((sbyte)WebRtcMessageType.Answer, answer);
        }

        public void ReceiveOffer(string offer)
        {
            _webView.LoadUrl("javascript:receiveOffer('" + offer + "');");
        }

        public void ReceiveAnswer(string answer)
        {
            _webView.LoadUrl("javascript:receiveAnswer('" + answer + "');");
        }
    }
}