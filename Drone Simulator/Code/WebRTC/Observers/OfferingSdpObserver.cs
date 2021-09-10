using Drone_Simulator.Signaling;
using Xam.WebRtc.Android;

namespace Drone_Simulator.WebRTC.Observers
{
    public class OfferingSdpObserver : SdpObserver
    {
        public OfferingSdpObserver(PeerConnection peerConnection, WebRtcSignalingServer signalingServer)
            : base(peerConnection, signalingServer)
        {
        }

        public override void OnCreateSuccess(SessionDescription p0)
        {
            base.OnCreateSuccess(p0);

            SignalingServer.SendOffer(p0.Description);
        }
    }
}