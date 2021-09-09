using Android.Webkit;
using Drone_Simulator.Signaling;
using Drone_Simulator.WebRTC;
using Java.Interop;
using Xam.WebRtc.Android;

namespace Drone_Simulator.Browser
{
    public class WebRtcJavaScriptInterface : Java.Lang.Object, IIceCandidateReceiver
    {
        private readonly WebView _webView;
        private readonly WebRtcSignalingInterface _signalingInterface;

        public WebRtcJavaScriptInterface(WebView webView, WebRtcSignalingInterface signalingInterface)
        {
            _webView = webView;
            _signalingInterface = signalingInterface;

            _signalingInterface.OfferReceived += ReceiveOffer;
            _signalingInterface.AnswerReceived += ReceiveAnswer;
            _signalingInterface.IceCandidateReceived += ReceiveIceCandidate;
        }

        public void AddIceCandidate(IceCandidate candidate)
        {
            SendIceCandidate(candidate.Sdp);
        }

        [Export]
        [JavascriptInterface]
        // ReSharper disable once UnusedMember.Global
        public void SendOffer(string offer)
        {
            _signalingInterface.SendOffer(offer);
        }

        [Export]
        [JavascriptInterface]
        // ReSharper disable once UnusedMember.Global
        public void SendAnswer(string answer)
        {
            _signalingInterface.SendAnswer(answer);
        }

        [Export]
        [JavascriptInterface]
        // ReSharper disable once UnusedMember.Global
        public void SendIceCandidate(string iceCandidate)
        {
            _signalingInterface.SendIceCandidate(iceCandidate);
        }

        private void ReceiveOffer(string offer)
        {
            InvokeJavaScriptFunction("receiveOffer", offer);
        }

        private void ReceiveAnswer(string answer)
        {
            InvokeJavaScriptFunction("receiveAnswer", answer);
        }

        private void ReceiveIceCandidate(string iceCandidate)
        {
            InvokeJavaScriptFunction("receiveIceCandidate", iceCandidate);
        }

        private void InvokeJavaScriptFunction(string functionName, string parameter)
        {
            _webView.LoadUrl($"javascript:{functionName}('{parameter}');");
        }
    }
}