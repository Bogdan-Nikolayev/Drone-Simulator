using System;
using System.Collections;
using Android.Content;
using Android.OS;
using Drone_Simulator.Signaling;
using Drone_Simulator.WebRTC.Observers;
using Xam.WebRtc.Android;

namespace Drone_Simulator.WebRTC
{
    public class WebRtcIceCandidatesCollector : IIceCandidateObserver
    {
        private readonly WebRtcSignalingServer _signalingServer;
        private bool _isInitialized;

        public event Action Initialized;
        public event Action<IceCandidate> IceCandidateGathered;

        public WebRtcIceCandidatesCollector(Context context, bool isInitiator, WebRtcSignalingServer signalingServer)
        {
            _signalingServer = signalingServer;

            // https://amryousef.me/android-webrtc

            PeerConnectionFactory.InitializationOptions initializationOptions = PeerConnectionFactory
                .InitializationOptions
                .InvokeBuilder(context)
                .SetEnableInternalTracer(true)
                .SetFieldTrials("IncludeWifiDirect/Enabled/")
                .CreateInitializationOptions();
            PeerConnectionFactory.Initialize(initializationOptions);

            PeerConnection.IceServer iceServer = PeerConnection.IceServer.InvokeBuilder("stun:stun.l.google.com:19302")
                .CreateIceServer();
            PeerConnectionObserver peerConnectionObserver = new PeerConnectionObserver(this);

            IEglBase rootEglBase = EglBase.Create();
            PeerConnectionFactory.Options options = new PeerConnectionFactory.Options()
            {
                DisableEncryption = true,
                DisableNetworkMonitor = true
            };
            PeerConnection peerConnection = PeerConnectionFactory
                .InvokeBuilder()
                .SetVideoDecoderFactory(new DefaultVideoDecoderFactory(rootEglBase.EglBaseContext))
                .SetVideoEncoderFactory(new DefaultVideoEncoderFactory(rootEglBase.EglBaseContext, true, true))
                .SetOptions(options)
                .CreatePeerConnectionFactory()
                .CreatePeerConnection(new PeerConnection.IceServer[1] {iceServer}, peerConnectionObserver);

            if (isInitiator)
            {
                SdpObserver sdpObserver = new OfferingSdpObserver(peerConnection, _signalingServer);
                sdpObserver.SetSuccess += () => peerConnection.InvokeIceGatheringState();

                _signalingServer.AnswerReceived += answer =>
                {
                    Log.Debug("Answer received");

                    peerConnection.SetRemoteDescription(
                        sdpObserver, new SessionDescription(SessionDescription.SdpType.Answer, answer));
                };

                peerConnection.CreateOffer(sdpObserver, new MediaConstraints()
                {
                    Mandatory = new ArrayList
                        {new MediaConstraints.KeyValuePair("OfferToReceiveVideo", "true")}
                });
            }
            else
            {
                SdpObserver sdpObserver = new AnsweringSdpObserver(peerConnection, _signalingServer);
                sdpObserver.SetSuccess += () => peerConnection.InvokeIceGatheringState();

                _signalingServer.OfferReceived += offer =>
                {
                    Log.Debug("Offer received");

                    peerConnection.SetRemoteDescription(
                        sdpObserver, new SessionDescription(SessionDescription.SdpType.Offer, offer));

                    peerConnection.CreateAnswer(sdpObserver, new MediaConstraints());
                };
            }
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