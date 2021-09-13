using Drone_Simulator.WebRTC.Signaling;
using Xam.WebRtc.Android;

namespace Drone_Simulator.WebRTC.Native.Observers
{
    public class AnsweringSdpObserver : SdpObserver
    {
        public AnsweringSdpObserver(PeerConnection peerConnection, WebRtcSignalingServer signalingServer)
            : base(peerConnection, signalingServer)
        {
        }

        public override void OnCreateSuccess(SessionDescription p0)
        {
            base.OnCreateSuccess(p0);

            SignalingServer.SendAnswer(p0.Description);
        }
    }
}