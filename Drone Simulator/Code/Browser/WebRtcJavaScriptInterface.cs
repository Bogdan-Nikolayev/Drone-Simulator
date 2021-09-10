using System;
using Android.Webkit;
using Drone_Simulator.Signaling;
using Java.Interop;

namespace Drone_Simulator.Browser
{
    public class WebRtcJavaScriptInterface : Java.Lang.Object
    {
        private readonly WebView _webView;
        private readonly WebRtcSignalingServer _signalingServer;

        public event Action ReceiveCandidatesClicked;

        public WebRtcJavaScriptInterface(WebView webView, WebRtcSignalingServer signalingServer)
        {
            _webView = webView;
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
        // ReSharper disable once UnusedMember.Global
        public void OnReceiveCandidatesClicked()
        {
            ReceiveCandidatesClicked?.Invoke();
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
            // Log.Debug("[FUNCTION TEST] " + $"javascript:{functionName}('{parameter}');");
            
            _webView.LoadUrl($"javascript:{functionName}('{parameter}');");
        }
    }
}