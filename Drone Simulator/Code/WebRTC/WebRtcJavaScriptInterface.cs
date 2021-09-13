using System;
using Android.Webkit;
using Drone_Simulator.Browser;
using Drone_Simulator.WebRTC.Signaling;
using Java.Interop;

namespace Drone_Simulator.WebRTC
{
    public class WebRtcJavaScriptInterface : JavaScriptInterface
    {
        private readonly WebRtcSignalingServer _signalingServer;

        // TODO: Remove if not required.
        public event Action ReceiveCandidatesClicked;

        public WebRtcJavaScriptInterface(WebView webView, WebRtcSignalingServer signalingServer) : base(webView)
        {
            _signalingServer = signalingServer;

            _signalingServer.OfferReceived += ReceiveOffer;
            _signalingServer.AnswerReceived += ReceiveAnswer;
            _signalingServer.IceCandidateReceived += ReceiveIceCandidate;
        }

        [Export]
        [JavascriptInterface]
        // ReSharper disable once UnusedMember.Global
        public void SendOffer(string offer)
        {
            _signalingServer.SendOffer(offer);
        }

        [Export]
        [JavascriptInterface]
        // ReSharper disable once UnusedMember.Global
        public void SendAnswer(string answer)
        {
            _signalingServer.SendAnswer(answer);
        }

        [Export]
        [JavascriptInterface]
        // ReSharper disable once UnusedMember.Global
        public void SendIceCandidate(string iceCandidate)
        {
            _signalingServer.SendIceCandidate(iceCandidate);
        }

        [Export]
        [JavascriptInterface]
        // TODO: Remove if not required.
        // ReSharper disable once UnusedMember.Global
        public void OnReceiveCandidatesClicked()
        {
            ReceiveCandidatesClicked?.Invoke();
        }

        // TODO: Remove if not required.
        public void StartVideoStreaming()
        {
            InvokeJavaScriptFunction("startVideoStreaming");
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
    }
}