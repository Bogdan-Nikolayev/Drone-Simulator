using Android.Webkit;
using Drone_Simulator.Sockets;
using Java.Interop;

namespace Drone_Simulator.Browser
{
    public class WebRtcJavaScriptInterface : Java.Lang.Object
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
                    case WebRtcMessageType.IceCandidate:
                        ReceiveIceCandidate(message);
                        break;
                }
            };
        }

        [Export]
        [JavascriptInterface]
        // ReSharper disable once UnusedMember.Global
        public void SendOffer(string offer)
        {
            _signalingSocket.SendString((sbyte)WebRtcMessageType.Offer, offer);
        }

        [Export]
        [JavascriptInterface]
        // ReSharper disable once UnusedMember.Global
        public void SendAnswer(string answer)
        {
            _signalingSocket.SendString((sbyte)WebRtcMessageType.Answer, answer);
        }
        
        [Export]
        [JavascriptInterface]
        // ReSharper disable once UnusedMember.Global
        public void SendIceCandidate(string iceCandidate)
        {
            _signalingSocket.SendString((sbyte)WebRtcMessageType.IceCandidate, iceCandidate);
        }

        private void ReceiveOffer(string offer)
        {
            _webView.LoadUrl($"javascript:receiveOffer('{offer}');");
        }

        private void ReceiveAnswer(string answer)
        {
            _webView.LoadUrl($"javascript:receiveAnswer('{answer}');");
        }

        private void ReceiveIceCandidate(string iceCandidate)
        {
            _webView.LoadUrl($"javascript:receiveIceCandidate('{iceCandidate}');");
        }
    }
}