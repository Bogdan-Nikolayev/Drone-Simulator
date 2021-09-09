using Android.Content;
using Xam.WebRtc.Android;

namespace Drone_Simulator.WebRTC
{
    public class WebRtcConnection
    {
        public WebRtcConnection(Context context, IIceCandidateReceiver iceCandidateReceiver)
        {
            PeerConnectionFactory.InitializationOptions options = PeerConnectionFactory.InitializationOptions
                .InvokeBuilder(context).SetFieldTrials("IncludeWifiDirect/Enabled/").CreateInitializationOptions();
            PeerConnectionFactory.Initialize(options);

            PeerConnection.IceServer iceServer = PeerConnection.IceServer.InvokeBuilder("stun:stun.l.google.com:19302")
                .CreateIceServer();
            PeerConnectionObserver peerConnectionObserver = new PeerConnectionObserver(iceCandidateReceiver);

            PeerConnection peerConnection = PeerConnectionFactory.InvokeBuilder().CreatePeerConnectionFactory()
                .CreatePeerConnection(new[] {iceServer}, peerConnectionObserver);
            peerConnection.CreateOffer(new SdpObserver(peerConnection), new MediaConstraints());
            // peerConnection.Ice
            // peerConnection.InvokeIceGatheringState();
        }
    }
}