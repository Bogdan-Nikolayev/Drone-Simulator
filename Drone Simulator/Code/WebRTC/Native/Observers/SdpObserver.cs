using System;
using Drone_Simulator.WebRTC.Signaling;
using Xam.WebRtc.Android;

namespace Drone_Simulator.WebRTC.Native.Observers
{
    public class SdpObserver : Java.Lang.Object, ISdpObserver
    {
        protected readonly WebRtcSignalingServer SignalingServer;
        private readonly PeerConnection _peerConnection;

        public event Action SetSuccess;

        protected SdpObserver(PeerConnection peerConnection, WebRtcSignalingServer signalingServer)
        {
            SignalingServer = signalingServer;

            _peerConnection = peerConnection;
        }

        public void OnCreateFailure(string p0)
        {
            Log.Debug(p0);
        }

        public virtual void OnCreateSuccess(SessionDescription p0)
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

            SetSuccess?.Invoke();
        }
    }
}