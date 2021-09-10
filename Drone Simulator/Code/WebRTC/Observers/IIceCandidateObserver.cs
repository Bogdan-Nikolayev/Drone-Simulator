using Xam.WebRtc.Android;

namespace Drone_Simulator.WebRTC.Observers
{
    public interface IIceCandidateObserver
    {
        void OnIceCandidate(IceCandidate candidate);
        void OnIceGatheringChange(PeerConnection.IceGatheringState state);
    }
}