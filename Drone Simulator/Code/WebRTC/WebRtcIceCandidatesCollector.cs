using System;
using Android.Content;
using Drone_Simulator.Signaling;
using Xam.WebRtc.Android;

namespace Drone_Simulator.WebRTC
{
    public class WebRtcIceCandidatesCollector : IIceCandidateObserver
    {
        private readonly WebRtcSignalingServer _signalingServer;
        private bool _isInitialized;

        public event Action Initialized;
        public event Action<IceCandidate> IceCandidateGathered;

        public WebRtcIceCandidatesCollector(Context context, WebRtcSignalingServer signalingServer)
        {
            _signalingServer = signalingServer;

            PeerConnectionFactory.InitializationOptions options = PeerConnectionFactory.InitializationOptions
                .InvokeBuilder(context).SetFieldTrials("IncludeWifiDirect/Enabled/").CreateInitializationOptions();
            PeerConnectionFactory.Initialize(options);

            // PeerConnection.IceServer iceServer = PeerConnection.IceServer.InvokeBuilder("stun:stun.l.google.com:19302")
            //     .CreateIceServer();
            PeerConnectionObserver peerConnectionObserver = new PeerConnectionObserver(this);

            PeerConnection peerConnection = PeerConnectionFactory.InvokeBuilder().CreatePeerConnectionFactory()
                .CreatePeerConnection(new PeerConnection.IceServer[0], peerConnectionObserver);
            peerConnection.CreateOffer(new SdpObserver(peerConnection), new MediaConstraints());
        }

        public void OnIceCandidate(IceCandidate candidate)
        {
            if (!_isInitialized)
            {
                _signalingServer.ClearEventSubscriptions();

                _isInitialized = true;
                Initialized?.Invoke();
            }

            IceCandidateGathered?.Invoke(candidate);
        }
    }
}