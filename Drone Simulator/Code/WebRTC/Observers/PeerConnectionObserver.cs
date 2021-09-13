using Xam.WebRtc.Android;

namespace Drone_Simulator.WebRTC.Observers
{
    public class PeerConnectionObserver : Java.Lang.Object, PeerConnection.IObserver
    {
        private readonly IIceCandidateObserver _iceCandidateObserver;

        public PeerConnectionObserver(IIceCandidateObserver iceCandidateObserver)
        {
            _iceCandidateObserver = iceCandidateObserver;
        }

        public void OnAddStream(MediaStream p0)
        {
            Log.Debug(p0.Id);
        }

        public void OnAddTrack(RtpReceiver p0, MediaStream[] p1)
        {
            Log.Debug(p0.Id(), p1.Length);
        }

        public void OnDataChannel(DataChannel p0)
        {
            Log.Debug(p0.Label());
        }

        public void OnIceCandidate(IceCandidate p0)
        {
            Log.Debug(p0.Sdp, p0.ToString());

            _iceCandidateObserver.OnIceCandidate(p0);
        }

        public void OnIceCandidatesRemoved(IceCandidate[] p0)
        {
            Log.Debug(p0.Length);
        }

        public void OnIceConnectionChange(PeerConnection.IceConnectionState p0)
        {
            Log.Debug(p0.Name());

            _iceCandidateObserver.OnIceConnectionChange(p0);
        }

        public void OnIceConnectionReceivingChange(bool p0)
        {
            Log.Debug(p0);
        }

        public void OnIceGatheringChange(PeerConnection.IceGatheringState p0)
        {
            Log.Debug(p0.Name());

            _iceCandidateObserver.OnIceGatheringChange(p0);
        }

        public void OnRemoveStream(MediaStream p0)
        {
            Log.Debug(p0.Id);
        }

        public void OnRenegotiationNeeded()
        {
            Log.Debug();
        }

        public void OnSignalingChange(PeerConnection.SignalingState p0)
        {
            Log.Debug(p0.Name());
        }
    }
}