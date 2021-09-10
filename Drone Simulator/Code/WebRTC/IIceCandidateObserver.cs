using Xam.WebRtc.Android;

namespace Drone_Simulator.WebRTC
{
    public interface IIceCandidateObserver
    {
        void OnIceCandidate(IceCandidate candidate);
    }
}