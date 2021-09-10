using Xam.WebRtc.Android;

namespace Drone_Simulator.WebRTC
{
    public class SdpObserver : Java.Lang.Object, ISdpObserver
    {
        private readonly PeerConnection _peerConnection;

        public SdpObserver(PeerConnection peerConnection)
        {
            _peerConnection = peerConnection;
        }

        public void OnCreateFailure(string p0)
        {
            Log.Debug(p0);
        }

        public void OnCreateSuccess(SessionDescription p0)
        {
            Log.Debug(p0.Description);

            _peerConnection.SetLocalDescription(this, p0);
        }

        public void OnSetFailure(string p0)
        {
            Log.Debug(p0);
        }

        public void OnSetSuccess()
        {
            Log.Debug();
        }
    }
}