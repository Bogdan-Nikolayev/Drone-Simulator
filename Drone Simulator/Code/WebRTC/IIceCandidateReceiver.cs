using Xam.WebRtc.Android;

namespace Drone_Simulator.WebRTC
{
    public interface IIceCandidateReceiver
    {
        void AddIceCandidate(IceCandidate candidate);
    }
}