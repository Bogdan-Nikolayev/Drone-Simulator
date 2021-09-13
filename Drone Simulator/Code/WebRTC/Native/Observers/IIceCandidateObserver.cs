using Xam.WebRtc.Android;

namespace Drone_Simulator.WebRTC.Native.Observers
{
    public interface IIceCandidateObserver
    {
        void OnIceCandidate(IceCandidate candidate);
        void OnIceGatheringChange(PeerConnection.IceGatheringState state);
        void OnIceConnectionChange(PeerConnection.IceConnectionState state);
    }
}